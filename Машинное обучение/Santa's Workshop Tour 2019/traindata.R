
library(tidyverse)
library(magrittr)
library(compiler)
#options(digits = 17)

data=read_csv("family_data.csv")
peop=data$n_people
choises=data[,2:11] %>% tbl_df()


preference.cost=function(dayvec){
  tmp=apply(choises,2,function(x) ifelse(x==dayvec,1,0))
  sums=apply(tmp, 1, sum)
  sums=ifelse(sums>0,0,1)
  
 ( 
     tmp[,2]*50+
     tmp[,3]*(50+9*peop)+
    tmp[,4]*(100+9*peop)+ 
    tmp[,5]*(200+9*peop)+
    tmp[,6]*(200+18*peop)+
    tmp[,7]*(300+18*peop)+
    tmp[,8]*(300+36*peop)+
    tmp[,9]*(400+36*peop)+
    tmp[,10]*(500+(36+199)*peop)+
     sums*(500+(36+398)*peop)
   ) %>% sum()
}

accounting.penalty=function(dayvec){
  N=cbind(peop,d=dayvec) %>% tbl_df() %>% mutate(d=factor(d))%>% 
    group_by(d) %>% summarise(sums=sum(peop)) %$%sums 
  
  if(sum(N<125|N>300)>0) {
    return(1e20)
    }
  
  N2=c(N,N[100])[2:101]
 ( (N-125)/400*N^(0.5+0.02*abs(N-N2))) %>% sum()
}


score=function(dayvec) preference.cost(dayvec)+accounting.penalty(dayvec)

#######################################

ct=0
repeat{
  
  dayvec=sample(1:100,5000,replace = T)
  sc=score(dayvec)
  if(sc<1e20){
    write_csv(dayvec %>% tbl_df(),"begins.csv",append = T)
    ct=ct+1
    cat(sc,'\n')
  }
  
 if(ct==1000)  break
}


best.res=dayvec
x=1:100
one=function(x){
  mat=rep(best.res,100) %>% matrix(nrow = 5000,byrow = F)
    mat[1,]=x
  num=apply(mat, 2, score)
  num[num>=1e19]=median(num[num<1e19])
  num
}

s=one(1:100)
ggplot(data.frame(x=1:100,y=s),aes(x,y))+geom_line(col="blue")




