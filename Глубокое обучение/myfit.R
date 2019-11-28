
score=function(target,predict){
  s=sum((log(1+predict)-log(1+target))^2)/length(target)
  sqrt(s)
}
predpred=function(model,df) round(abs(predict(model,df[,-2])),2)


library(data.table)

build=fread("building_metadata.csv")

train=fread("train.csv")
weather=fread("weather_train.csv")
weather$timestamp=as.factor(weather$timestamp)
lev=levels(weather$timestamp)
train$timestamp=factor(train$timestamp,levels = lev)

train2=merge(train,build,by="building_id")
library(dplyr)
library(mice)
library(magrittr)
train2=full_join(train2,weather)

#удаляю наблюдения с пропущенными значениями
#train2=na.omit(train2)
train2=train2 %>% mice() %>% complete()

#узнаю, на что делится число строк
vc=4:23
number=(nrow(train2)%%vc ==0)[1]+3
step=nrow(train2)/number


#train2=merge(train2,weather,by.x="site_id",by.y = "timestamp")

colnames(train2)
train2=tbl_df(train2) %>% 
  mutate(
    building_id=factor(building_id),
    meter=factor(meter),
    site_id=factor(site_id),
    floor_count=factor(floor_count),
    primary_use=factor(primary_use),
    cloud_coverage=factor(cloud_coverage),
    how_old=as.POSIXlt(substr(as.character(timestamp), 1, 10))$year-year_built+1900,
    wind_direction=wind_direction/180*pi
  )

meter.lev=levels(train2$meter)
floor_count.lev=levels(train2$floor_count)
primary_use.lev=levels(train2$primary_use)
cloud_coverage.lev=levels(train2$cloud_coverage)
l.lev=list(meter.lev,floor_count.lev,primary_use.lev,cloud_coverage.lev)
save(file="levels.rdata",l.lev)

#удалить дату и год постройки
train2=train2[,-c(3,8)]

#удалить номер здания и район, так как объединение по ним уже произошло
train2=train2[,-c(1,4)]

#удаляю переменную, которая хз чо такое (глубина осадков)
train2=train2[,-9]

(NAMES=colnames(train2))


fit=lm(meter_reading~
       (primary_use+
            square_feet+
         I(square_feet^2)+
          I(square_feet^3)+
         floor_count+
         air_temperature+
         cloud_coverage+
         dew_temperature+
           abs(air_temperature-dew_temperature)+
         I( abs(air_temperature-dew_temperature)^2)+
         sea_level_pressure+
         wind_direction+
         wind_speed+
       #  I(wind_direction*wind_speed)  +
       #   precip_depth_1_hr+
         sqrt(how_old)+
         how_old)^2,
       train2)
summary(fit)
#97 блять процентов!!!!
#plot(fit)

tg=train2$meter_reading
res1=predpred(fit,train2) #0.5883174

score(tg,res1)

score(tg,runif(length(tg),0,20))


#library(MASS)

#step.model <- stepAIC(fit, direction = "backward",  trace = 5)
#summary(step.model)


#вторая модель
library(caret)
set.seed(1998)
rc=nrow(train2)
p.80=sample(1:rc,round(rc*0.1))
p.20=(1:rc)[-p.80]

#p.80=1:400

data.80=train2[p.80,]#обучение
data.20=train2[p.20,]#тест, который добавлю к обучению

Lm_model <- train(meter_reading~
                    primary_use+
                    square_feet+
                    I(square_feet^2)+
                    floor_count+
                    air_temperature+
                    cloud_coverage+
                    dew_temperature+
                   sea_level_pressure+
                    wind_direction+
                    wind_speed+
                    sqrt(how_old)+
                    #I(how_old^2)
                    how_old,
                  data= train2, method = "lm")
summary(Lm_model)

res2=predpred(Lm_model,train2) #1.171259
score(tg,res2)




CT_model <- train(meter_reading~
                    primary_use+
                    square_feet+
                    I(square_feet^2)+
                    floor_count+
                    air_temperature+
                    cloud_coverage+
                    dew_temperature+
                    sea_level_pressure+
                    wind_direction+
                    wind_speed+
                    sqrt(how_old)+
                    #I(how_old^2)
                    how_old,
                  data= data.80, method = "ctree")
summary(CT_model)

res3=predpred(CT_model,train2) #0.3253814 при 0.1 от обучающей
score(tg,res3)



rc=nrow(train2)
p.80=sample(1:rc,round(rc*0.05))
p.20=(1:rc)[-p.80]
data.80=train2[p.80,]#обучение
data.20=train2[p.20,]#тест, который добавлю к обучению
R_model <- train(meter_reading~
                    primary_use+
                    square_feet+
                    I(square_feet^2)+
                    floor_count+
                    air_temperature+
                    cloud_coverage+
                    dew_temperature+
                    sea_level_pressure+
                    wind_direction+
                    wind_speed+
                    sqrt(how_old)+
                    #I(how_old^2)
                    how_old,
                  data= data.80, method = "ranger")
summary(R_model)

res4=predpred(R_model,train2) #0.3069017 при 0.05 от обучающей
score(tg,res4)


p.80=sample(1:rc,round(rc*0.05))
p.20=(1:rc)[-p.80]
data.80=train2[p.80,]#обучение
data.20=train2[p.20,]#тест, который добавлю к обучению
G_model <- train(meter_reading~
                   primary_use+
                   square_feet+
                   I(square_feet^2)+
                   floor_count+
                   air_temperature+
                   cloud_coverage+
                   dew_temperature+
                   sea_level_pressure+
                   wind_direction+
                   wind_speed+
                   sqrt(how_old)+
                   #I(how_old^2)
                   how_old,
                 data= data.80, method = "ppr")
summary(G_model)

res5=predpred(G_model,train2) #0.8881283 при 0.05 от обучающей
score(tg,res5)


if(F){
  #модели-кандидаты
methods=c(
       #  'rpart1SE',
          'lm'#,
       #   'ppr',
        #  'gbm',
         #'ctree'#,
          #'ranger'
         )


results=list()
for(i in seq(methods)){
  cat("---------METHOD",methods[i] ," \n")
 time= system.time(
  Lm_model <- train(meter_reading~
                      primary_use+
                      square_feet+
                      I(square_feet^2)+
                      floor_count+
                      air_temperature+
                      cloud_coverage+
                      dew_temperature+
                      sea_level_pressure+
                      wind_direction+
                      wind_speed+
                      sqrt(how_old)+
                      how_old, 
                    data= data.80, method = methods[i])
  )
  #summary(Lm_model)
  
  res2=round(abs(predict(Lm_model,data.80[,-2])),2) 
  sc=score(tg[p.80],res2)
  cat("***************************score",sc,"\n")
  sc2=score(tg[p.20],round(abs(predict(Lm_model,data.20[,-2])),2))
  cat("***************************score",sc2,"\n")
  
  results[[i]]=list(
    in.score=sc,
    all.score=sc2,
    method=methods[i],
    time=time
  ) 
}



res2=round(abs(predict(Lm_model,train2[,-2])),2) 
score(tg,res2)
}


#функция, которая оптимизует комбинацию моделей
library(ucminf)
coefs=function(best,others){
  n=nrow(others)
  res=rep(1,n)
  t=function(vec){
    s=best
    vec=abs(vec)
    for(i in seq(n)) s=s+vec[i]*others[i,]
   score(tg,s/(1+sum(vec))) 
  }
  
  # Find minimum and show trace
  tt= ucminf(par = res, fn = t, control = list(trace = 1,grtol=1e-12,maxeval=500))
  print(tt)
  tt$par
}



mycoefs=coefs(res4,
      rbind(
        res1,
        res2,
        res3,
        res5
        ))
save(file="mycoefs.RData",mycoefs)


files=paste0(c("fit","Lm_model","CT_model","R_model","G_model","mycoefs","levels"),".RData")
for(fl in files) load(fl)

fits=list(fit=list(fit,Lm_model,CT_model,R_model,G_model),
          vec=c(1,mycoefs))

library(data.table)
library(dplyr)
library(magrittr)

remove(data.80,data.20,train2,train,lev,p.20,p.80,tg,res1,res2,res3,res4,res5);gc()

Spred=function(df){
  s=predpred(fits$fit[[1]],df)
  for(i in 2:5) s=s+predpred(fits$fit[[i]],df)*fits$vec[i-1]
  s
}


#файл предсказаний
weather.test=fread("weather_test.csv")
weather.test$timestamp=as.factor(weather.test$timestamp)

predpred=function(model,df) round(abs(predict(model,df)),2)

V=1000

test=fread("test.csv",nrows = V,skip=0)
test$timestamp=factor(test$timestamp,levels = levels(weather.test$timestamp))

test2=merge(test,build,by="building_id")
test2=inner_join(test2,weather.test)

colnames(test2)
test2=tbl_df(test2) %>% 
  mutate(
    building_id=factor(building_id),
    meter=factor(meter,levels =  l.lev[[1]]),
    site_id=factor(site_id),
    floor_count=factor(floor_count,levels =  l.lev[[2]]),
    primary_use=factor(primary_use,levels =  l.lev[[3]]),
    cloud_coverage=factor(cloud_coverage,levels =  l.lev[[4]]),
    how_old=as.POSIXlt(substr(as.character(timestamp), 1, 10))$year-year_built+1900,
    wind_direction=wind_direction/180*pi,
    row_id=as.numeric(row_id)
  ) %>% arrange(row_id)
itrows=test2$row_id

test2=test2[,-2]#убрать row_id

#удалить дату и год постройки
test2=test2[,-c(3,7)]

#-----------------------------------------------------------------
#удалить номер здания и район, так как объединение по ним уже произошло
test2=test2[,-c(1,3)]

#удаляю переменную, которая хз чо такое (глубина осадков)
test2=test2[,-8]


Spred(test2)








