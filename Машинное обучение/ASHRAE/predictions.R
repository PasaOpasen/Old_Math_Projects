source("libraries.r", local=T)
source("functions.r", local=T)

load("build.rdata")
load("weathertestMICED.rdata")
meterlevels=c("0","1","2","3")

#load("fitlm.rdata")
#load("fitCtree.rdata")
load("fitRanger.rdata")

weather.test %<>% tbl_df() %>% 
  mutate(
    cloud_coverage=factor(round(cloud_coverage)%%10),
    #precip_depth_1_hr=factor(sign(precip_depth_1_hr)),
    wind_direction=wind_direction/180*pi#,
  #  timestamp=as.factor(timestamp)
  ) 
save(file="weathertest.rdata",weather.test)
load("weathertest.rdata")

#load("weathermiced.rdata")


V=8400*34
ct=41697600/V

NAs=0
for(i in seq(ct)){
  cat(i,"from",ct,"\n")
test=fread("test.csv",nrow=V,skip=(i-1)*V)%>% tbl_df()
colnames(test)=c('row_id','building_id','meter','timestamp')

test2=merge(test,build,by="building_id")%>%na.omit()# %>% tbl_df() %>% arrange(site_id) #%>%mutate( timestamp=factor(timestamp,levels(weather.test$timestamp))) 
test2=left_join(test2,weather.test) 

if(is.na(mean(test2$air_temperature))){
  #test2=merge(test,build,by="building_id")%>%na.omit()
  #test2=left_join(test2,weather) 
  test2[,-4]%<>% mice(method = "sample",m=3) %>% complete()#у них жопа с данными, есть несостыковки в датах, ну и хуй с ними
 # test2[,-4]=  preProcess(test2[,-4],method= "knnImpute")
  test2$cloud_coverage=factor(test2$cloud_coverage,as.character(0:9))
}


test2%<>%  tbl_df() %>% 
  mutate(
    meter=factor(meter,levels=meterlevels),
    floor_count=factor(floor_count,levels(as.factor(build$floor_count))),
    primary_use=factor(primary_use,levels(factor(build$primary_use))),
    how_old=as.POSIXlt(substr(as.character(timestamp), 1, 10))$year-year_built+1900,
    wind_direction=wind_direction-pi) %>% 
  select(-building_id,-timestamp,-site_id,-year_built) %>% 
  arrange(row_id)

row_id=((i-1)*V):(i*V-1)#test2$row_id
test2=test2[,-1]#убрать row_id

meter_reading=round(Combpredict(test2),4)#round(predict(fit2,test2)^2,3)
s=sum(is.na(meter_reading))
if(s!=0){
  NAs=NAs+s
meter_reading=ifelse(is.na(meter_reading),runif(1,0,100),meter_reading)  
cat(s,"\n")
}

write_csv(data.frame(row_id,meter_reading),"results.csv",append = T,col_names = (i==1))

if(i%%4==0){
  rm(test2)
  gc()
}
}

cat("NAs = ",NAs,"\n")

