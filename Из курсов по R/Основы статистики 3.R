

hetero_test <-  function(test_data){
  ns=colnames(test_data)
  m=lm(as.formula(paste(ns[1],'~', paste(ns[-1], collapse ='+'))),test_data)
  test_data[[1]]=m$residuals^2
  ns=colnames(test_data)
  tmp=summary(lm(as.formula(paste(ns[1],'~', paste(ns[-1], collapse ='+'))),test_data))$`r.squared`
 return(as.numeric(tmp) )
}

hetero_test(mtcars)




VIF <-  function(test_data){
  td=test_data[,2:ncol(test_data)]
  ns=colnames(td)
  tmp=numeric(0)
  for(i in 1:length(ns)){
    m=lm(as.formula(paste(ns[i],'~', paste(ns[-i], collapse ='+'))),td)
    R2=summary(m)$`r.squared`
    tmp=c(tmp,1/(1-R2))
  }
  names(tmp)=ns
  return(tmp)
}

VIF(mtcars)




#https://stepik.org/lesson/42032/step/4?unit=20306
smart_model <-  function(test_data){
  t=test_data
  vec=VIF(t)
  while(T){
    if( ncol(t)!=2){
      vec=VIF(t)
    }else{
      vec=numeric(0)
    }
    #print(vec)
    if(any(vec>10)){
      t=t[,-(1+which(vec==max(vec))[1])]
    }else{
      #print(t)
      ns=colnames(t)
      m=lm(as.formula(paste(ns[1],'~', paste(ns[-1], collapse ='+'))),t)
      return(m$coefficients)
    }
  }
  
}
smart_model(mtcars)



test_data <- as.data.frame(list(y = c(4.7, 5.24, 5.86, 6.6, 4.24, 4.17, 5.53, 2.22, 6.17, 3.98, 4.2, 4.81, 4.23, 5.74, 4.42, 5.72, 3.73, 2.86, 5.61, 6.34, 5.6, 6.14, 3.83, 4.09, 5.3, 5.38, 5.79, 3.73, 5.31, 5.16), 
                                x1 = c(6.33, 3.69, 4.56, 4.74, 4.94, 4.11, 4.74, 6.63, 6.22, 5.07, 5.79, 4.6, 4.26, 5.4, 5.03, 3.68, 5.64, 5.9, 4.55, 5.39, 5.61, 5.93, 4.71, 5.01, 5.23, 4.45, 7.33, 4.1, 6.67, 4.02), 
                                x2 = c(40.06, 13.61, 20.81, 22.5, 24.44, 16.87, 22.5, 43.96, 38.74, 25.67, 33.51, 21.2, 18.18, 29.15, 25.27, 13.58, 31.77, 34.77, 20.66, 29.07, 31.51, 35.12, 22.14, 25.1, 27.35, 19.79, 53.72, 16.79, 44.44, 16.13)))
smart_model(test_data)






transform_x <-  function(test_data){
  sq=seq(-2,2,0.1)
  fun= function(t,w){
    if(w==0){
      return(log(t))
    }
    if(w<0){
      return(-t^w)
    }
    return(t^w)
  }
  mt=numeric(length(sq))
  for(i in 1:length(sq)){
     x2=fun(test_data$x,sq[i])
     mt[i]=cor(test_data$y,x2)
  }
  ind=which.max(abs(mt))
  return(fun(test_data$x,sq[ind]))
}

set.seed(42)
test_data <- data.frame(y = rnorm(10, 10, 1), x = rnorm(10, 10, 1))  
transform_x(test_data)

cor(c(1,2,3,4,23,2,232,3,2),c(1,2,32,4,23,2,232,32,2))



exp_data <- read.csv("http://www.bodowinter.com/tutorial/politeness_data.csv")
str(exp_data)
library(ggplot2)
plot_1 <- ggplot(exp_data,aes(x=factor(scenario),y=frequency,fill=attitude))+
  geom_boxplot()
plot_1

plot_2 <-ggplot(exp_data,aes(x=frequency,fill=subject))+
  geom_density(alpha=0.2)+
  facet_grid(gender~.)
plot_2


library(lme4)
fit_1 <- lmer(frequency~attitude+(1|subject)+(1|scenario),exp_data)
summary(fit_1)
fit_2=lmer(frequency~attitude+gender+(1|subject)+(1|scenario),exp_data)
summary(fit_2)

fit_3=lmer(frequency~attitude+gender+(1+attitude|subject)+(1+attitude|scenario),exp_data)
summary(fit_3)






median_cl_boot <- function(x){
  mb=median(x)*2
  getv=function(t){
    s=round(runif(length(t),1,length(t)))
    return(t[s])
  }
  
  vb=sapply(1:1000, function(t) median(getv(x)))
  vb1=quantile(vb,0.05)
  vb2=quantile(vb,0.95)
  
  return(c(mb-vb2,mb-vb1))
  
}

vec=c(1,2,3,4,5,6,7,8,21,4,4,3,2,1,-5,9)
median_cl_boot(vec)



library(dplyr)
slope_cl_boot <- function(x){
  
  mb=cor(x$y,x$x)*2
  getv=function(t){
    s=round(runif(nrow(t),1,nrow(t)))
    return(t[s,])
  }
  
  vb=sapply(1:1000, function(t) {
    df=getv(x)
    cor(df$y,df$x)
        })
  vb1=quantile(vb,0.05)
  vb2=quantile(vb,0.95)
  
  return(c(mb-vb2,mb-vb1))
  
}

cor(c(1,1,2,3,42,2,2),c(-1,2,4,6,4,0,2))









