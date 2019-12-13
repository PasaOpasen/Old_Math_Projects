library(data.table)
library(tidyverse)
library(mice)
library(magrittr)
library(caret)
library(ucminf)


build=fread("building_metadata.csv") %>% 
  tbl_df() %>%
  mutate(primary_use=factor(primary_use))
build[,-c(1,2)]%<>% mice(method = "rf") %>% complete()
save(file="build.rdata",build)
#надо помнить, что среди года постройки половина пропущены, а среди числа этажей --
#две трети, так что в будущем надо бы это вообще вычеркнуть из прогноза
#

weather=fread("weather_train.csv") %>% tbl_df()%>%
  mutate(    
    site_id=factor(site_id)#,
    #timestamp=as.POSIXct(timestamp) 
  )
weather[,-2]%>% mice() %>% complete() 
save(file="weathermiced.rdata",weather)
#тут у переменной cloud_coverage половина пропущены
#а у precip треть


load("build.rdata")
load("weathermiced.rdata")


train=fread("train.csv")

train=train %>% tbl_df() %>% filter(abs(meter_reading-median(meter_reading))<=1.5*IQR(meter_reading))

weather %<>%tbl_df() %>% 
  mutate(
    cloud_coverage=factor(cloud_coverage),
    #precip_depth_1_hr=factor(sign(precip_depth_1_hr)),
    wind_direction=wind_direction/180*pi,
    timestamp=as.factor(timestamp)
  ) 
lev=levels(weather$timestamp)
train$timestamp=factor(train$timestamp,levels = lev)

save(file="weather.rdata",weather)
save(file="train.rdata",train)
load("weather.rdata")
load("train.rdata")


train2=merge(train,build,by="building_id")
train2=full_join(train2,weather) 
train2%<>% na.omit() %>% 
  tbl_df() %>% 
  mutate(
    meter=factor(meter),
    floor_count=factor(floor_count>=5,labels=c("more5","less5")),
    primary_use=factor(primary_use,levels(factor(build$primary_use))),
    how_old=as.POSIXlt(substr(as.character(timestamp), 1, 10))$year-year_built+1900,
    wind_direction=wind_direction-pi
  ) %>% select(-building_id,-timestamp,-site_id,-year_built) %>% 
  mutate(
    how_old=ifelse(how_old<0,0,how_old),
    sea_level_pressure=sea_level_pressure-900
  ) 
save(file="train2.rdata",train2)

train3 <- train2 %>% group_by(meter,primary_use,floor_count,cloud_coverage,precip_depth_1_hr) %>% sample_n(min(n(),300))

tg=train2$meter_reading
tg2=train3$meter_reading

save(file="ttgg.rdata",train3,tg2)
load("ttgg.rdata")

#####функции
score=function(target,predict){
  s=sum((log(1+predict)-log(1+target[seq(predict)]))^2)
  sqrt(s/length(predict))
}
predpred=function(model,df) round(abs(predict(model,df[,-2])),2)

Score=function(model,df,deg=2){
pr=predpred(model,df)
score(df$meter_reading,pr^deg)
}



control <- trainControl(method="repeatedcv", number=10, repeats=3)#трёхкратная десятиблочная кросс-валидация


############модели
train.fit <- train3 %>% 
  group_by(meter,
           primary_use,
           floor_count,
           #cloud_coverage,
           precip_depth_1_hr) %>% 
  sample_n(min(n(),40))

fit=lm(sqrt(meter_reading)~
         (meter+
           primary_use+
            poly(square_feet,3)  +
            air_temperature+
            cloud_coverage+
            floor_count+
            dew_temperature+
            abs(air_temperature-dew_temperature)+
            sea_level_pressure+
            wind_direction+
            wind_speed+
            precip_depth_1_hr+
            how_old+
            sqrt(how_old)
          )^2,
       train.fit)
summary(fit)#0.4219
Score(fit,train.fit)#1.667517


fit2 <- train(sqrt(meter_reading)~
                    (meter+
                       primary_use+
                       square_feet+
                       I(square_feet^2)+
                       air_temperature+
                       cloud_coverage+
                       floor_count+
                       dew_temperature+
                       I(abs(air_temperature-dew_temperature))+
                       sea_level_pressure+
                        wind_direction+
                       wind_speed+
                        precip_depth_1_hr+
                       sqrt(how_old)+
                       how_old),
                  data= train.fit, method = "ctree")
summary(fit2)

Score(fit2,train.fit) #1.503524



fit2 <- train(sqrt(meter_reading)~
                (meter+
                   primary_use+
                   square_feet+
                   I(square_feet^2)+
                   air_temperature+
                   cloud_coverage+
                   floor_count+
                   dew_temperature+
                   I(abs(air_temperature-dew_temperature))+
                   sea_level_pressure+
                   wind_direction+
                   wind_speed+
                   precip_depth_1_hr+
                   sqrt(how_old)+
                   how_old),
              data= train.fit, method = "ctree")
summary(fit2)

Score(fit2,train.fit) #1.503524




#модели-кандидаты
methods=c(
  'rpart1SE',
  'ppr',
  'gbm',
  'rf',
  'svmRadial',
  'pcr',
  'kernelpls',
  'ada',
  'nnet',
  'gpls',
  'gaussprRadial',
  'mars',
  'enet',
  'ranger'
)


results=list()
for(i in seq(methods)){
  
  cat("---------METHOD",methods[i] ," \n")
 time= system.time(gcFirst = TRUE,
 expr = (  ML= train(
     sqrt(meter_reading)~
       meter+
       primary_use+
       square_feet+
       I(square_feet^2)+
       air_temperature+
       cloud_coverage+
       floor_count+
       dew_temperature+
       I(abs(air_temperature-dew_temperature))+
       sea_level_pressure+
       wind_direction+
       wind_speed+
       precip_depth_1_hr+
       sqrt(how_old)+
       how_old,
          data= train.fit, method = methods[i]))
  )
  #summary(Lm_model)
  
  sc=Score(ML,train.fit,2)
  cat("***************************score",sc,"\n")
  
  results[[i]]=list(
    in.score=sc,
   # all.score=sc2,
    method=methods[i]#,
   # time=time
  ) 
  print(results)
}




#results <- resamples(list(CART=fit.cart, LDA=fit.lda, SVM=fit.svm, KNN=fit.knn, RF=fit.rf))
#summary(results)



#файл предсказаний
weather.test=fread("weather_test.csv")%>%
  tbl_df() %>%
  mutate(    
    site_id=factor(site_id)#,
    #timestamp=as.POSIXct(timestamp)
    )

weather.test[,-2] %<>% mice() %>% complete()

save(file="weathertestMICED.rdata",weather.test)
rm(weather.test)

load("weathertestMICED.rdata")


weather.test %<>% tbl_df() %>% 
  mutate(
    cloud_coverage=factor(cloud_coverage),
    #precip_depth_1_hr=factor(sign(precip_depth_1_hr)),
    wind_direction=wind_direction/180*pi,
    timestamp=as.factor(timestamp)
  ) 
save(file="weathertest.rdata",weather.test)
load("weathertest.rdata")


V=1000

test=fread("test.csv",nrow=V,skip=0)%>% tbl_df()

test2=merge(test,build,by="building_id")
test2=full_join(test2,weather.test) 
test2%<>% na.omit() %>% 
  tbl_df() %>% 
  mutate(
    meter=factor(meter,levels=levels(train$meter)),
    floor_count=factor(floor_count>=5,labels=c("more5","less5")),
    primary_use=factor(primary_use,levels(factor(build$primary_use))),
    how_old=as.POSIXlt(substr(as.character(timestamp), 1, 10))$year-year_built+1900,
    wind_direction=wind_direction-pi
  ) %>% select(-building_id,-timestamp,-site_id,-year_built) %>% 
  mutate(
    how_old=ifelse(how_old<0,0,how_old),
    sea_level_pressure=sea_level_pressure-900
  ) 

itrows=test2$row_id
test2=test2[,-2]#убрать row_id



res=abs(predict(fit,test2))

write_csv(cbind(itrows,res),"results.csv")

