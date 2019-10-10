
#Вариант 7

p1 = nchar("Дмитрий")
p2 = nchar("Пасько")


#Задание 1
library(readxl)
tab=data.frame(read_xlsx("Псевдоцены.xlsx")) 
colnames(tab)=c("Price","Year","City","Distrinct")
tab$Year=factor(tab$Year)
tab$City=factor(tab$City)
tab$Distrinct=factor(tab$Distrinct)

library(dplyr)

tib=data_frame(tab)[[1]]

# по городам
tb=tib%>%select(Price,Year,City)%>%group_by(Year,City)%>%
  summarise(mean=mean(Price))
ms=matrix(tb$mean,nrow=length(levels(tb$Year)))
ms= data.frame(ms)
colnames(ms)=levels(tb$City)
rownames(ms)=levels(tb$Year)

# по районам
tb=tib%>%select(Price,Year,Distrinct)%>%group_by(Year,Distrinct)%>%
  summarise(mean=mean(Price))
ms=matrix(tb$mean,nrow=length(levels(tb$Year)))
ms= data.frame(ms)
colnames(ms)=levels(tb$Distrinct)
rownames(ms)=levels(tb$Year)

# по стране
tb=tib%>%select(Price,Year)%>%group_by(Year)%>%
  summarise(mean=mean(Price))
ms=matrix(tb$mean,nrow=length(levels(tb$Year)))
ms= data.frame(ms)
colnames(ms)=c("mean")
rownames(ms)=levels(tb$Year)

library(ggplot2)
df=data.frame(time=as.numeric(levels(tb$Year)),price=ms$mean)
ggplot(df,
       aes(time,price))+
  geom_point(size=3)+
  geom_smooth(method = lm,se=F)+
  geom_smooth(method =loess,col="red")


#forma=ts(df$price,start = min( df$time),frequency = 1)
#plot(stl(forma, s.window = 21)$time.series,main="")


price1 = c(
  40 + p1,43 + p1,40,80,
  74,40 + p2,55 + p2,42 + p2,
  42,50,40 + p2,43,43,
  35 + p1,40 + p1,30,36 + p1,
  50,30 + p1,29,45 + p1,
  40,42,40,36,
  50,30 + p1,24 + p2,
  25 + p2,40,32 + p1,
  30,20,30,25,32 + p2
)

summary(price1)#минимальные характеристики
t.test(price1)#тест Стьюдента для среднего

vart=sd(price1)/mean(price1)*100
cat("Коэффициент вариации равен",vart,"%\n")
#так как коэффициент вариации < 30%, выборка достаточно однородная


#Задание 3
library(ggplot2)

yt=c(1133+ p1,	1222,	1354+ p1,	1389,	1342+ p2,	1377,	1491,	1684+ p2)
data=data.frame(time=1:length(yt),values=yt)
plot(data,type="b")

fit=lm(values~time,data)#создание модели
ggplot(data,aes(x=time,y=values))+
  geom_point()+
  geom_smooth(method=lm)

summary(fit)#информация о модели
cf=fit$coefficients#коэффициенты

#прогноз среднего
predict(fit,data.frame(time = c(9)), se.fit=TRUE, interval="confidence", level=0.95)$fit
#прогноз индивидуального
predict(fit,data.frame(time = c(9)), se.fit=TRUE, interval="prediction", level=0.95)$fit


forma=ts(yt, start = 1,frequency = 1)
plot(stl(forma,s.window = "periodic")$time.series,main="")



#Задание 4

library(readxl)
data=data.frame(read_xlsx("РожьВсеГодаПустыхЛет.xlsx"))
data[,-1]=apply(data[,-1], 2, as.numeric)#перевести в числа все строки
y=t(data[,-1])
ns=rownames(y)
#y=apply(y,2,function(s) ifelse(is.na(s),median(s,na.rm = T),s))
library(mice)
imp=mice(y,seed=11)
y=complete(imp,action = 1)

x=sapply(ns, function(s) as.numeric(substr(s,2,nchar(s))))
df=data.frame(x=x,y=y[,2])

library(ggplot2)
ggplot(df,aes(x=x,y=y))+
  geom_line(col="green")+
  geom_point(size=2,na.rm = TRUE)+
  geom_smooth(method = lm,na.rm = TRUE)

mt=lm(y~x,df,na.rm = TRUE)
summary(mt)


x=x[!is.na(df$y)]
res=mt$residuals[!is.na(df$y)]
#скользящее среднее
library(caTools)
k=c(3,5,9)
plot(x,res,type="l",col="grey")
for(i in 1:length(k)){
  lines(x,runmean(res,k[i]),col=i,lwd=2)
}

legend("topleft",c(paste("k =", k)),col=1:length(k),bty="n",lwd=2)



library(corrplot)
nn=20

for(i in seq(length(x)-80,length(x)-nn,nn)){
  tmp=i:(i+nn-1)
  cat("Times:",x[tmp],"\n")
  data=y[tmp,]#транспонирование, чтобы строки стали переменными
  cormatrix=cor(data)
  lower=abs(cormatrix[lower.tri(cormatrix)])
  cat("Статистика по треугольнику корреляционной матрицы \n")
  print(summary(lower[lower!=0])) 
  corrplot(cormatrix)
}









