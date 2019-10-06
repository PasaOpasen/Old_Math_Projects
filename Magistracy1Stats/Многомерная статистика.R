#номер варианта
nv=7


#чтение данных и чистка
library(readxl)

datacrude =data.frame(read_excel("Таблица 1.xlsx")) 
data=datacrude[5:nrow(datacrude),-c(1)]
data=data[-nv,]
colnames(data)=c("Country","Doctors","Deaths","GDP","Costs")

data[,2:5]=apply(data[,2:5],2,function(x)scale( as.numeric(x)))#тут переменные из текста преобразуются в числа и стандартизируются
data[,1]=factor(data[,1])

#####################################Задание 1

d = dist(data[,2:5], method = "euclidean")#матрица расстояний
fit <- hclust(d, method = "ward.D")
plot(fit, labels = data$Country,xlab = "Countries")

plot(fit$height, xlab = "step",ylab="dist",type="b",col="blue",lwd=1,main="Расстояния при объединении кластеров")



####################################Задание 2

it=1:8
sums=sapply(it, function(k) kmeans(data[,2:5], k)$tot.withinss)

plot(it,sums,type = "b",col="red")

getimage=function(k){
  fit=kmeans(data[,2:5],k)
print(fit$withinss)#внутригрупповые суммы
print(fit$betweenss)  
print(dist(fit$centers))#матрица расстояний

newdata=as_data_frame(data)%>%mutate(cluster=factor(fit$cluster))

library(dplyr)
means=newdata[,2:6]%>%group_by(cluster)%>%summarise(
  meanCosts=mean(Costs),sdCosts=sd(Costs),
  meanDoctors=mean(Doctors),sdDoctors=sd(Doctors),
  meanGDP=mean(GDP),sdGDP=sd(GDP),
  meanDeaths=mean(Deaths),sdDeaths=sd(Deaths)
  )
print(means)

means=means[,c(1,2,4,6,8)]

cs=c("red","green","blue","black","yellow")
rg=range(means[-1])*1.05
plot(as.numeric(means[1,2:5]),type="b",
     col=cs[1],ylim = rg,ylab = "values of means")
for(i in 2:k){
  lines(as.numeric(means[i,2:5]),type="b",col=cs[i])
}


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

getimage(3)



###################################Задание 3

datacrude =data.frame(read_excel("Приложение 1.xlsx")) 
data=datacrude[,-c(1)]
data=data[,-c(1,2,16,17)]

library(corrplot)
corrplot(cor(data))

library(psych)
principal(data[,-1],nfactors = 8,rotate = "none")

fa.parallel(data[,-1],fa="pc",show.legend = T,main="Диаграмма каменистой осыпи с параллельным анализом")

#варимакс с нормализацией
(vm=principal(apply(data[,-1],2,scale),nfactors = 6,rotate = "varimax"))
#коэффициенты
round(unclass(vm$weights),2)

cor(vm$scores)



###################################Задание 4
data =data.frame(read_excel("Приложение 2.xlsx")) 
data$CLASS=factor(data$CLASS)

library(MASS)

ldadat <- lda(CLASS~.,data,method="t")
ldadat$means#групповые средние
(mat=ldadat$scaling)
#matrix(nrow=1,as.numeric(data[65,1:7]))%*%as.matrix(mat)

#функция для оценки ошибки 
misclass <- function(pred, obs) {
   tbl <- table(pred, obs)
   sum <- colSums(tbl)
   dia <- diag(tbl)
   msc <- (sum - dia)/sum * 100
   m.m <- mean(msc)
   cat("Classification table:", "\n")
   print(tbl)
   cat("Misclassification errors:", "\n")
   print(round(msc, 1))
  }

misclass(predict(ldadat, data[,1:7])$class, data[,8])

#дерево классификации
library(tree)
datatree <- tree(data[,8] ~ ., data[,-8])
plot(datatree)
text(datatree) 

#то же, что и раньше, только методом randomForest
library(randomForest)
rf <- randomForest(data[,8] ~ ., data=data[,1:7])
rfp <- predict(rf, data[,1:7])
table(rfp, data[,8])
MDSplot(randomForest(data[,-8]), data[,8])
misclass(rfp, data[,8])


###################################Задание 5











