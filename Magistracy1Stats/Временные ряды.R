
#Вариант 7

p1 = nchar("Дмитрий")
p2 = nchar("Пасько")


#Задание 1

library(readxl)
library(dplyr)
tab=data.frame(t(read_xlsx("Рожь18век.xlsx")))
names(tab)=sapply( tab[1,], as.character)#поставить правильные названия
tab=tab[-1,]#удалить строку с именами
tab=data.frame(Year=sapply(rownames(tab),as.numeric),tab)
tab=sapply(tab, as.numeric)#факторы перевести в числа
head(as_data_frame(tab),10)

tmptab=tab[,-1]
means=list(rowMeans(tmptab,na.rm = T))
for(i in 1:((ncol(tab)-1)/2)){
  means[[i+1]]=rowMeans(tmptab[,c(i,i+1)],na.rm = T)
}

means=sapply(means,function(col)sapply(col, function(row) ifelse(is.nan(row),NA,row)))#Заменить все NaN на NA
means=data.frame(tab[,1],means)
names(means)=c("Год","ПоСтране","Район1","Район2","Район3","Район4","Район5","Район6","Район7","Район8","Район9")
head(means)

library(ggplot2)
p1=  ggplot(means,aes(x=Год))+
  geom_line(aes(y=ПоСтране),size=1)
p2=  ggplot(means,aes(x=Год))+
  geom_line(aes(y=Район1),size=1,col=2)
p3=  ggplot(means,aes(x=Год))+
geom_line(aes(y=Район2),size=1,col=3)
p4=  ggplot(means,aes(x=Год))+
geom_line(aes(y=Район3),size=1,col=5)
p5=  ggplot(means,aes(x=Год))+
geom_line(aes(y=Район4),size=1,col=6)
p6= ggplot(means,aes(x=Год))+
geom_line(aes(y=Район5),size=1,col=7)
p7= ggplot(means,aes(x=Год))+
geom_line(aes(y=Район6),size=1,col=8)
p8=  ggplot(means,aes(x=Год))+
geom_line(aes(y=Район7),size=1,col=9)
p9=  ggplot(means,aes(x=Год))+
geom_line(aes(y=Район8),size=1,col=10)
p10=  ggplot(means,aes(x=Год))+
  geom_line(aes(y=Район9),size=1,col=11)
library(ggpubr)
ggarrange(p1,p2,p3,p4,p5,p6,p7,p8,p9,p10,nrow=5,ncol=2)


if(FALSE){
  
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
}

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

t.test(means[,2],price1)


#Задание 3
library(ggplot2)

yt=c(1133+p1,1222,1354+p1,1389,1342+p2,1377,1491,1684+p2)
data=data.frame(time=1:length(yt),values=yt)
plot(data,type="b")

fit=lm(values~time,data)#создание модели
ggplot(data,aes(x=time,y=values))+
  geom_point()+
  geom_smooth(method=lm)

summary(fit)#информация о модели
cf=fit$coefficients#коэффициенты

anova(fit,levels(0.05))


#прогноз среднего
predict(fit,data.frame(time = c(9)), se.fit=TRUE, interval="confidence", level=0.95)$fit
#прогноз индивидуального
predict(fit,data.frame(time = c(9)), se.fit=TRUE, interval="prediction", level=0.95)$fit


forma=ts(yt, start = 1,frequency = 1)
plot(stl(forma,s.window = "periodic")$time.series,main="")



#Задание 4

library(readxl)
data=data.frame(read_xlsx("РожьВсеГода.xlsx"))
data[,-1]=apply(data[,-1], 2, as.numeric)#перевести в числа все строки
y=t(data[,-1])#транспонирование для удобства

#получить массив лет
ns=rownames(y)
x=sapply(ns, function(s) as.numeric(substr(s,2,nchar(s))))

library(mice)#обработать пустые значения
imp=mice(y,seed=11)
y=complete(imp,action = 1)

df=data.frame(x=x,y=y[,2])#объединить данные в фрейм

print(df[sort(sample(1:nrow(df),13)),])

library(ggplot2)
ggplot(df,aes(x=x,y=y))+
  geom_line(col="green")+
  geom_point(size=2)+
  geom_smooth(method = lm)+
  geom_smooth(se=F,col="red")

mt=lm(y~x,df)
summary(mt)
mt=lm(log(y)~x,df)
summary(mt)
mt=lm(log(y)~log(x),df)
summary(mt)
mt=lm(sqrt(y)~x,df)
summary(mt)
mt=lm(log(y)~log(x)+I(log(x)^2),df)
summary(mt)

x=x[!is.na(df$y)]
mt=lm(log(y)~log(x),df)
res=mt$residuals
#скользящее среднее
library(caTools)
k=c(3,5,9)
plot(x,res,type="l",col="grey")
for(i in 1:length(k)){
  lines(x,runmean(res,k[i]),col=i,lwd=2)
}

legend("topleft",c(paste("k =", k)),col=1:length(k),bty="n",lwd=2)



library(corrgram)
nn=15

for(i in seq(length(x)-nn*3,length(x)-nn,nn)){
  tmp=i:(i+nn-1)
  cat("Times:",x[tmp],"\n")
  data=y[tmp,]#транспонирование, чтобы строки стали переменными
  cormatrix=cor(data)
  lower=abs(cormatrix[lower.tri(cormatrix)])
  cat("Статистика по треугольнику корреляционной матрицы \n")
  print(summary(lower[lower!=0])) 
  corrgram(cormatrix,order=FALSE, lower.panel=panel.shade,
           upper.panel=panel.pie)
}









