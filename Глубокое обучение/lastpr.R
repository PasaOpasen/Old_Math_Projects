library(data.table)
library(dplyr)
library(mice)
library(magrittr)
library(caret)
library(ucminf)


build=fread("building_metadata.csv")%>% mice() %>% complete()
#надо помнить, что среди года постройки половина пропущены, а среди числа этажей --
#две трети, так что в будущем надо бы это вообще вычеркнуть из прогноза
#


weather=fread("weather_train.csv") %>% mice() %>% complete()%>% tbl_df() %>% 
  mutate(
    cloud_coverage=factor(cloud_coverage),
    precip_depth_1_hr=factor(sign(precip_depth_1_hr)),
    wind_direction=wind_direction/180*pi
  )
#тут у переменной cloud_coverage половина пропущены


save(file="build.rdata",build)
save(file="weather.rdata",weather)

load("build.rdata")
load("weather.rdata")


train=fread("train.csv")

train=train %>% tbl_df() %>% filter(abs(meter_reading-median(meter_reading))<=1.5*IQR(meter_reading))


weather$timestamp=as.factor(weather$timestamp)
lev=levels(weather$timestamp)
train$timestamp=factor(train$timestamp,levels = lev)

save(file="weather.rdata",weather)
save(file="train.rdata",train)
load("weather.rdata")
load("train.rdata")

percent=0.01
cl=sample(seq(nrow(train)),nrow(train)*percent)


train2=merge(train[cl,],build,by="building_id")
train2=inner_join(train2,weather) 
train2=train2%>% 
  tbl_df() %>% 
  mutate(
    #building_id=factor(building_id),
    meter=factor(meter),
    #site_id=factor(site_id),
    floor_count=factor(floor_count),
    primary_use=factor(primary_use,levels(factor(build$primary_use))),
    how_old=as.POSIXlt(substr(as.character(timestamp), 1, 10))$year-year_built+1900,
    wind_direction=wind_direction-pi
  ) %>% select(-building_id,-timestamp,-site_id,-year_built) %>% 
  mutate(
    how_old=ifelse(how_old<0,0,how_old),
    sea_level_pressure=sea_level_pressure-900
  ) 
tg=train2$meter_reading

#####функции
score=function(target,predict){
  s=sum((log(1+predict)-log(1+target[seq(predict)]))^2)
  sqrt(s/length(predict))
}
predpred=function(model,df) round(abs(predict(model,df[,-2])),2)


############модели

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
       train2)
summary(fit)
#97 блять процентов!!!!

res1=predpred(fit,train2)^2 #1.527063

score(tg,res1)


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
                  data= train2, method = "lmStepAIC")
summary(fit2)

res1=predpred(fit2,train2) #1.757885
score(tg,res1^2)




CT_model <- train(sqrt(meter_reading)~
                       (meter+
                       primary_use+
                       square_feet+
                       air_temperature+
                       #cloud_coverage+
                       floor_count+
                       dew_temperature+
                       #I(abs(air_temperature-dew_temperature))+
                       sea_level_pressure+
                      # wind_direction+
                       wind_speed+
                      # precip_depth_1_hr+
                       sqrt(how_old)+
                       how_old)^2,
                  data= train2, method = "ctree")
summary(CT_model)

res1=predpred(CT_model,train2) #1.51587
score(tg,res1^2)




#модели-кандидаты
methods=c(
  'lm',
  'ppr',
  'gbm',
  "lmStepAIC",
  'penalized',
  'pcr',
  'kernelpls',
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
               air_temperature+
               #cloud_coverage+
               floor_count+
               dew_temperature+
               #I(abs(air_temperature-dew_temperature))+
               sea_level_pressure+
               # wind_direction+
               wind_speed+
               # precip_depth_1_hr+
               sqrt(how_old)+
               how_old,
          data= train2, method = methods[i]))
  )
  #summary(Lm_model)
  
  res2=predpred(ML,train2)^2
  sc=score(tg,res2)
  cat("***************************score",sc,"\n")
  
  results[[i]]=list(
    in.score=sc,
   # all.score=sc2,
    method=methods[i]#,
   # time=time
  ) 
}
























