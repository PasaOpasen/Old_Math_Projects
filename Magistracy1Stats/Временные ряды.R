
#Вариант 7

p1 = nchar("Дмитрий")
p2 = nchar("Пасько")


#Задание 1

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
print(paste("Коэффициент вариации равен",vart,"%"))



#Задание 3
library(ggplot2)

yt=c(1133+ p1,	1222,	1354+ p1,	1389,	1342+ p2,	1377,	1491,	1684+ p2)
data=data.frame(time=1:length(yt),values=yt)
plot(data,type="b")

fit=lm(values~time,data)
ggplot(data,aes(x=time,y=values))+
  geom_point()+
  geom_smooth(method=lm)

summary(fit)
cf=fit$coefficients

#прогноз среднего
predict(fit,data.frame(time = c(9)), se.fit=TRUE, interval="confidence", level=0.95)$fit
#прогноз индивидуального
predict(fit,data.frame(time = c(9)), se.fit=TRUE, interval="prediction", level=0.95)$fit









#Задание 4

