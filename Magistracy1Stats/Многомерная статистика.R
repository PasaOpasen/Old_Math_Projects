

#чтение данных и чистка
library(readxl)

datacrude =data.frame(read_excel("Таблица 1.xlsx")) 
data=datacrude[5:nrow(datacrude),-c(1)]
colnames(data)=c("Country","Doctors","Deaths","GDP","Costs")

data[,2:5]=apply(data[,2:5],2,function(x)scale( as.numeric(x)))#тут переменные из текста преобразуются в числа и стандартизируются
data[,1]=factor(data[,1])

#задание 1

d = dist(data[,2:5], method = "euclidean")#матрица расстояний
fit <- hclust(d, method = "ward.D")
plot(fit, labels = data$Country,xlab = "Countries")



#Задание 2

it=1:8
sums=sapply(it, function(k) kmeans(data[,2:5], k)$tot.withinss)

plot(it,sums,type = "b",col="red")

getimage=function(k){
  fit=kmeans(data[,2:5],k)

newdata=data
newdata$cluster=factor(fit$cluster)
library(ggplot2)
library(ggpubr)
pl1=ggplot(newdata, aes(x=Doctors, y=Deaths, col = cluster))+
  geom_point(size = 3)+
  theme_bw() 

pl2=ggplot(newdata, aes(x=GDP, y=Costs, col = cluster))+
  geom_point(size = 3)+
  theme_bw()

pl3=ggplot(newdata, aes(x=GDP, y=Deaths, col = cluster))+
  geom_point(size = 3)+
  theme_bw()

costs = ggplot(newdata, aes(x=cluster, y=Costs))+
  geom_boxplot()+
  theme_bw()
deaths = ggplot(newdata, aes(x=cluster, y=Deaths))+
  geom_boxplot()+
  theme_bw()
doctors = ggplot(newdata, aes(x=cluster, y=Doctors))+
  geom_boxplot()+
  theme_bw()
gdp = ggplot(newdata, aes(x=cluster, y=GDP))+
  geom_boxplot()+
  theme_bw()

p1 <- ggarrange(pl1, pl2,pl3,
                ncol = 3, nrow = 1)
p2 <- ggarrange(costs, deaths, doctors, gdp,
                ncol = 2, nrow = 2)
ggarrange(p1, p2, ncol = 1, nrow = 2,heights=c(1,3))
}

getimage(2)



