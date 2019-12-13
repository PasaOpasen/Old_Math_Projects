source("libraries.r", local=T)#--------------------------------------------------
source("functions.r", local=T)#--------------------------------------------------

load("build.rdata")#-------------------------------------------------------------
load("weathermiced.rdata")


train=fread("train.csv")
train=train %>% tbl_df() #%>% filter(abs(meter_reading-median(meter_reading))<=1.5*IQR(meter_reading))

weather %<>%tbl_df() %>% 
  mutate(
    #cloud_coverage=factor(round(cloud_coverage)%%10),
    #precip_depth_1_hr=factor(sign(precip_depth_1_hr)),
    wind_direction=wind_direction/180-1,
    timestamp=as.factor(timestamp)
  ) 
lev=levels(weather$timestamp)
train$timestamp=factor(train$timestamp,levels = lev)

save(file="weather.rdata",weather)
save(file="train.rdata",train)

load("weather.rdata")#-----------------------------------------------------------------
load("train.rdata")



train2=merge(train,build,by="building_id")%>% na.omit()
train2=left_join(train2,weather) 

train2%<>% tbl_df() %>% 
  mutate(
    meter=factor(meter),
    floor_count=factor(floor_count),#>=5,labels=c("more5","less5")),
    primary_use=factor(primary_use,levels(factor(build$primary_use))),
    how_old=as.POSIXlt(substr(as.character(timestamp), 1, 10))$year-year_built+1900
  ) %>% select(-building_id,-timestamp,-site_id,-year_built)# %>% 
 # mutate(
   # how_old=ifelse(how_old<0,0,how_old),
  #  sea_level_pressure=sea_level_pressure-900
  #) 



save(file="train2.rdata",train2)

load("train2.rdata")
tg=train2$meter_reading

t3=createDataPartition(train2,list=F,p=0.05)

train3 <- train2 %>% 
  group_by(meter,primary_use,floor_count) %>% 
 # filter(abs(meter_reading-median(meter_reading))<=1.5*IQR(meter_reading)) %>% 
  sample_n(min(n(),800)) %>%na.omit()

tg2=train3$meter_reading

save(file="ttgg.rdata",train3,tg2)
load("ttgg.rdata")#------------------------------------------------------------------------
train3 %<>% na.omit()
train3 %<>%   #group_by(meter,
                #       primary_use,
                 #      floor_count) %>% 
  filter(abs(meter_reading-median(meter_reading))<=1.5*IQR(meter_reading)) 

mt=createDataPartition(train3$meter,1,0.04,list=F)
pr=createDataPartition(train3$primary_use,1,0.04,list=F)
fc=createDataPartition(train3$floor_count,1,0.04,list=F)

s=unique(c(mt,pr,fc)) 

############модели
train.fit <- train3[s,] 

save(file="tmptmp.rdata",train.fit)
load("tmptmp.rdata")

train.class=train.fit %>% mutate(
  meter_reading=ifelse(meter_reading==0,0,1)
) %>% mutate(
  meter_reading=factor(meter_reading,levels = c("0","1"))
)


if(F){
 fit=lm(
  sqrt(meter_reading)~
         (meter+
           primary_use+
            poly(square_feet,3)  +
            log(square_feet)+
            sqrt(square_feet)+
            air_temperature+
            I(air_temperature^2)+
            sigmoid(air_temperature)+
            cloud_coverage+
            floor_count+
            dew_temperature+
            sigmoid(dew_temperature)+
            abs(air_temperature-dew_temperature)+
            sea_level_pressure+
            sqrt(sea_level_pressure)+
            log(sea_level_pressure)+
            wind_direction+
            #sin(wind_direction)+
           # cos(wind_direction)+
            wind_speed+
            sqrt(wind_speed)+
            precip_depth_1_hr+
            sigmoid(precip_depth_1_hr)+
            how_old+
            sigmoid(how_old)
          )^2,
       train3)
summary(fit)#0.5437
Score(fit,train3)#1.551641

save(file="fitlm.rdata",fit)




fit2 <- train(sqrt(meter_reading)~
                meter+
                primary_use+
                poly(square_feet,2)  +
                log(square_feet)+
                sqrt(square_feet)+
                sigmoid(air_temperature)+
                #cloud_coverage+
                floor_count+
                sigmoid(dew_temperature)+
                sqrt(sea_level_pressure)+
                log(sea_level_pressure)+
                wind_direction+
                #sin(wind_direction)+
                # cos(wind_direction)+
                wind_speed+
                sqrt(wind_speed)+
                sigmoid(precip_depth_1_hr)+
                sigmoid(how_old/10),
              data= train3, method = "ctree")
summary(fit2)

Score(fit2,train3) #1.218939
save(file = "fitCtree.rdata",fit2)

methods=c(
#'sda','lda',
#'lda2', 
'glm','bayesglm','Mlda','vglmCumulative'      
          )
results=list()
i=0
for(met in methods){
  
  cat("---------METHOD",met ," \n")
  time= system.time(gcFirst = TRUE,
                    expr = (  
                      fit3 <- train(meter_reading~.^2,
                                    data= train.class, method = met)
                      
                      
                      )
  )

  sc=ScoreBinary(fit3,train.class)
  cat("***************************score",sc,"\n")
  i=i+1
  results[[i]]=list(
    in.score=sc,
    # all.score=sc2,
    method=met,
   time=time
  ) 
  print(results)
}

}


if(F){
    t=function(vec){
    v1=vec[1]
    v2=vec[2]
    v3=vec[3]
    ndf=cbind(
                    train.fit,
                             v1=rep(v1,nrow(train.fit)),
                             v2=rep(v2,nrow(train.fit)),
                             v3=rep(v3,nrow(train.fit)))
    fit3 <- train(meter_reading^v1~
                    meter+
                    primary_use+
                    poly(square_feet,4)  +
                    primary_use:square_feet+
                    sigmoid(air_temperature)+
                    floor_count+
                    tanh(dew_temperature)+
                    sigmoid(air_temperature):tanh(dew_temperature)+
                    I(sea_level_pressure^v2)+
                   I(wind_direction/3.1415926535898)  +
                    wind_speed+
                    tanh(precip_depth_1_hr)+
                    sigmoid(how_old/v3),
                  data= ndf,
                  method = "ppr")
    summary(fit3)
    
    Score(fit3,ndf,1/v1) #1.864324
  }
  
  # Find minimum and show trace
  tt= ucminf(par = c(0.5,0.5,10), fn = t, control = list(trace = 1,grtol=1e-12,maxeval=500))
  print(tt)
}


fit.class <- train(meter_reading~.,
              data= train.class, method = "ranger",trControl=trainControl(
                method = "oob",
                number = 6  ))

ScoreBinary(fit.class ,train.class)

save(file="rangerclass.rdata",fit.class)

ScoreBinary(fit.class ,train3[sample(1:2000000,5000),]%>% mutate(
  meter_reading=ifelse(meter_reading==0,0,1)
) %>% mutate(
  meter_reading=factor(meter_reading,levels = c("0","1"))
))





fit.nonzero <- train(
  log(meter_reading)~
                meter+
                primary_use+
                poly(log(square_feet),3)  +
                primary_use:square_feet+
                sigmoid(air_temperature)+
                floor_count+
                tanh(dew_temperature)+
                sigmoid(air_temperature):tanh(dew_temperature)+
                I((sea_level_pressure^2-1)/20000)+
                wind_direction +
                I(sqrt(wind_speed+1))+
                tanh(precip_depth_1_hr)+
                sigmoid(how_old/10),
              data= train3 %>% filter(meter_reading>0)
  ,  method = "ranger",
  trControl=trainControl(
    method = "oob",
    number = 5  ) 
  )

ScoreLog(fit.nonzero,train3%>% filter(meter_reading>0)) #0.3443867


Combpredict=function(df,deg=2){
 exp(predict(fit.nonzero,df)) * (as.numeric(predict(fit.class,df))-1)
}

score(train3$meter_reading,Combpredict(train3[,-2]))#0.2521707

save(file = "fitRanger.rdata",fit.class,fit.nonzero,Combpredict)




ggplot(train3,aes(x=log(meter_reading+1)))+
  geom_histogram(bins = 50)







#1.218939


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



