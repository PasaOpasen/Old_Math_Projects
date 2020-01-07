
library(tidyverse)
library(caret)

data=read.csv("conts.csv") %>% tbl_df()
colnames(data)=c("id","pr","acc")

data= data%>%filter(pr+acc>0) 

#i=createDataPartition(y=data[,-1],p=0.8,list=F)

i=createDataPartition(y=data$pr+data$acc,p=0.7,list=F)

write(data$id[i]-1,"subset.txt",ncolumns = 1)
