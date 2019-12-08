source("libraries.r", local=T)


build=fread("building_metadata.csv") %>% 
  tbl_df() %>%
  mutate(
    site_id=factor(site_id),
    primary_use=factor(primary_use)
    )
build[,-c(1,2)]%<>% mice(method = "rf",m=20,maxit = 30) %>% complete()

save(file="build.rdata",build)
#надо помнить, что среди года постройки половина пропущены, а среди числа этажей --
#две трети, так что в будущем надо бы это вообще вычеркнуть из прогноза
#



weather=fread("weather_train.csv") %>% tbl_df()%>%
  mutate(    
    site_id=factor(site_id)#,
    #timestamp=as.POSIXct(timestamp) 
  )
weather[,-2]%<>% mice(method = "norm.predict",m=10,maxit = 20) %>% complete() 

save(file="weathermiced.rdata",weather)
rm(weather)
#тут у переменной cloud_coverage половина пропущены
#а у precip треть


weather.test=fread("weather_test.csv")%>%
  tbl_df() %>%
  mutate(    
    site_id=factor(site_id)#,
    #timestamp=as.POSIXct(timestamp)
  )

weather.test[,-2] %<>% mice(method = "norm.predict",m=20,maxit = 20) %>% complete()

save(file="weathertestMICED.rdata",weather.test)
rm(weather.test)


