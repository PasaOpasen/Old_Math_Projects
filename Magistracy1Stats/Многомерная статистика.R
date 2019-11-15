#номер варианта
nv=7


#чтение данных и чистка
library(readxl)

datacrude =data.frame(read_excel("Таблица 1.xlsx")) #считывание таблицы
data=datacrude[5:nrow(datacrude),-1]#удаление лишних строк и столбцов
data=data[-nv,]#удаление строки в соответствиии с номером варианта
colnames(data)=c("Country","Doctors","Deaths","GDP","Costs")#переименование столбцов

data[,2:5]=apply(data[,2:5],2,function(x)scale(as.numeric(x)))#тут переменные из текста преобразуются в числа и стандартизируются
data[,1]=factor(data[,1])#первая переменная из количественной преобразуется в номенативную


#####################################Задание 1

d = dist(data[,2:5], method = "euclidean")#матрица расстояний
fit <- hclust(d, method = "ward.D")
plot(fit, labels = data$Country,xlab = "Countries")

plot(fit$height, xlab = "step",ylab="dist",type="b",col="blue",lwd=1,main="Расстояния при объединении кластеров")


mat=fit$merge
resu=list()
countries=as.character(data$Country)
for(i in 1:nrow(mat)){
  
  if(mat[i,1]<0){
    a=countries[-mat[i,1]]
  }else{
    a=as.character(resu[[mat[i,1]]])
  }
  
  if(mat[i,2]<0){
    b=countries[-mat[i,2]]
  }else{
    b=as.character(resu[[mat[i,2]]])
  }

  resu[[i]]=c(a,b)
}
names(resu)=paste("Шаг",1:nrow(mat),"расстояние", fit$height)
print(resu)





####################################Задание 2

it=1:8
sums=sapply(it, function(k) kmeans(data[,2:5], k)$tot.withinss)

plot(it,sums,type = "b",col="red",main = "Суммы внутригрупповых расстояний при разном числе кластеров")

#функция, проводящая некоторый анализ и строящая графики для заданного числа кластеров
getimage=function(k){
  
fit=kmeans(data[,2:5],k)#строится модель

cat("Внутригрупповые суммы:",fit$withinss,"\n")#внутригрупповые суммы
cat("Общая сумма:", fit$betweenss,"\n") 
cat("Матрица расстояний:\n")
#print(dist(fit$centers))#матрица расстояний



#Добавляем кластер к фрейму данных
library(dplyr)
newdata=as_data_frame(data)%>%mutate(cluster=factor(fit$cluster))
#print(summary(aov(Costs~cluster,newdata)))

#рисуются кластеры через главные компоненты
library(cluster) 
clusplot(newdata, newdata$cluster, color=TRUE, shade=TRUE, labels=2, lines=0)

#агрегирование данных по группам
means=newdata[,2:6]%>%group_by(cluster)%>%summarise(
  meanCosts=mean(Costs),sdCosts=sd(Costs),
  meanDoctors=mean(Doctors),sdDoctors=sd(Doctors),
  meanGDP=mean(GDP),sdGDP=sd(GDP),
  meanDeaths=mean(Deaths),sdDeaths=sd(Deaths)
  )
print(means)


means=means[,c(1,2,4,6,8)]#берётся сабсет только из значений для средних

lbs=c("cluster1","cluster2","cluster3","cluster4","cluster5")

library(ggplot2)
library(ggpubr)

#здесь создаётся таблица со средними по каждой переменной и каждому классу в том виде, в каком удобней рисовать
tmpdata=data.frame(x=1:4,means=as.numeric(means[1,2:5]),cluster=rep(lbs[1],4))
for(i in 2:k){
  tmpdata=rbind(tmpdata,data.frame(x=1:4,means=as.numeric(means[i,2:5]),cluster=rep(lbs[i],4)))
}
tmpdata$cluster=factor(tmpdata$cluster)


ppp=ggplot(tmpdata,aes(x=x,y=means,col=cluster))+
  geom_line()+
  geom_point(size=4)


pl1=ggplot(newdata, aes(x=Doctors, y=Deaths, col = cluster))+
  geom_point(size = 3)+
  theme_bw() 

pl2=ggplot(newdata, aes(x=GDP, y=Costs, col = cluster))+
  geom_point(size = 3)+
  theme_bw()

pl3=ggplot(newdata, aes(x=GDP, y=Deaths, col = cluster))+
  geom_point(size = 3)+
  theme_bw()
pl4=ggplot(newdata, aes(x=GDP, y=Doctors, col = cluster))+
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

p1 <- ggarrange(pl1, pl2,pl3,pl4,
                ncol = 2, nrow = 2)
p2 <- ggarrange(costs, deaths, doctors, gdp,
                ncol = 2, nrow = 2)
ggarrange(ppp,p1, p2, ncol = 1, nrow = 3,heights=c(1.3,2,3))
}
getimage2=function(k){
  fit=kmeans(data[,2:5],k)#строится модель
  
  #Добавляем кластер к фрейму данных
  library(dplyr)
  newdata=as_data_frame(data)%>%mutate(cluster=factor(fit$cluster))
  cat("Дисперсионный анализ для каждой переменной,",k,"кластеров \n")
  cat("Затраты: \n")
  print(summary(aov(Costs~cluster,newdata)))
  cat("Смерти: \n")
  print(summary(aov(Deaths~cluster,newdata)))
  cat("Врачи: \n")
  print(summary(aov(Doctors~cluster,newdata)))
  cat("ВВП: \n")
  print(summary(aov(GDP~cluster,newdata)))
  
  #рисуются кластеры через главные компоненты
  #library(cluster) 
  #print(clusplot(newdata, newdata$cluster, color=TRUE, shade=TRUE, labels=2, lines=0))
  library(factoextra)
  print( fviz_cluster(fit, data[, -1], ellipse.type = "norm"))
}

getimage(3)
getimage2(3)


getfaces=function(dataset){
  means=data_frame(dataset)%>%group_by_if(is.factor)
 
  means=sapply(select_if(means,is.numeric), mean)
  print(means)
  library(TeachingDemos)
  faces(means)
}
pp=mtcars
pp$cyl=factor(pp$cyl)
getfaces(pp[2:8,2:6])

#функция делает анализ dataset по методу k-means с k кластерами, затем добавляет результаты к датасету
getbykmeans=function(dataset,k){
  fit=kmeans(dataset,k)#строится модель
  #Добавляем кластер к фрейму данных
  library(dplyr)
  newdata=as_data_frame(dataset)%>%mutate(cluster=factor(fit$cluster))
}
#функция считает средние и рисует лица
getfaces=function(k){
  #создаем матрицу средних
means=getbykmeans(data[,2:5],k)%>%group_by(cluster)%>%
  summarise_all(funs(mean))

library(TeachingDemos)
faces(means[,2:5])#рисуем лица
}
getfaces(2)

###################################Задание 3

datacrude =data.frame(read_excel("Приложение 1.xlsx")) 
data=datacrude[,-c(1)]
data=data[,-c(1,2,16,17)]

library(corrplot)
corrplot(cor(data))

library(psych)
principal(data[,-1],nfactors = 8,rotate = "none")#Создание модели

fa.parallel(data[,-1],fa="pc",show.legend = T,main="Диаграмма каменистой осыпи с параллельным анализом")

#варимакс с нормализацией
(vm=principal(apply(data[,-1],2,scale),nfactors = 6,rotate = "varimax"))
#коэффициенты
round(unclass(vm$weights),2)

cor(vm$scores)



###################################Задание 4
data =data.frame(read_excel("Приложение 2.xlsx")) 
data$CLASS=factor(data$CLASS)
pairs(data[,1:7],col=data$CLASS,pch=16)

#лица Чернова
showfaces=function(){
  newdata=as_data_frame(data)%>%group_by(CLASS)%>%
    summarise_all(funs(mean))
  print(faces(newdata[,2:8]))#рисуем лица
}
showfaces()
#визуализация кластеров через главные компоненты
showimage=function(){
  library(factoextra)
  print( fviz_cluster(list(data=data[,1:7],cluster=data[,8]), ellipse.type = "norm"))
}

  
#проверка многомерного нормального распределения по каждому классу
tmp=numeric()
library(mvnormtest) 
  
for(i in 1:length(levels(data$CLASS))){
  tmp[i]=mshapiro.test(t(data[data$CLASS == i, 1:7]))$p.value 
} 
  

library(MASS)
# Функция вывода результатов классификации
Out_CTab <- function(model, group) {
  cat("Таблица неточностей \"Факт/Прогноз\" по обучающей выборке: \n") 
    classified <- predict(model)$class  
    t1 <- table(group, classified) 
    print(t1)
    # Точность классификации и расстояние Махалонобиса
    Err_S <- mean(group != classified)
    mahDist <- dist(model$means %*% model$scaling) 
    cat("Точность классификации:",1-Err_S[1],'\n')
    cat("Расстояния Махалонобиса:\n")
    print(mahDist)
    
    # Таблица "Факт/Прогноз" и ошибка при скользящем контроле
    t2 <-  table(group, update(model, CV = T)$class -> LDA.cv) 
    Err_CV <- mean(group != LDA.cv) 
    cat("Ошибка при скользящем контроле:",Err_CV[1],'\n')
    Err_S.MahD <- c(Err_S, mahDist) 
    Err_CV.N <- c(Err_CV, length(group)) 
    cbind(t1, Err_S.MahD, t2, Err_CV.N)
    
    cat("Результаты многомерного дисперсионного анализа: \n")
    ldam <- manova(as.matrix( data[,1:7]) ~ data$CLASS)
    print(summary(ldam, test="Wilks"))
  }   
  

#линейный дискриминантный анализ
ldadat <- lda(CLASS~.,data,method="moment")

Out_CTab(ldadat,data$CLASS)

ldadat$means#групповые средние
(mat=ldadat$scaling)#матрица дискриминантных функций
plot(ldadat)


#матрицы, обратные матрицам ковариации для каждого класса
covinv=function(df){
  res=list()
  for(i in 1:length(levels(df$CLASS)))
    res[[i]]= tryCatch({df[df$CLASS==i,1:7] %>% as.matrix() %>% cov() %>% solve},error=function(r) NA)
    #res[[i]]=df[df$CLASS==i,1:7] %>% as.matrix() %>% cov() %>% solve
  res
}

#расстояния Махаланобиса от элемента до каждого из классов
distance=function(means,covs, elem){
  res=c()
  for(i in 1:nrow(means)){
    vec=(means[i,]-elem)
     res[i]=(vec%*%covs[[i]])%*%vec
  }
   
  return(sqrt(res))
}

#поиск номера элемента в датафрейме
find.number=function(df,elem){
  sm=0
  i=0
  len=length(elem)
  while (sm!=len) {
    i=i+1
    v=ifelse(df[i,]==elem,T,F)
    sm=sum(v)
  }
  return(i)
}

acc=0#точность
repeat{ 
#for(k in 1:40){
  
#showfaces()
  showimage()
    
ldadat <- lda(CLASS~.,data,method="moment")
means=ldadat$means
cov.mat=covinv(data)

#для всех неправильно найденных найти расстояния до кластеров, отнесённых экспертами
prclass=predict(ldadat, data[,1:7])$class
st=data[data$CLASS!=prclass,]
acc=1-nrow(st)/nrow(data)
cat("Точность классификации:",acc,'\n')
if(near(acc,1)) break

if(nrow(st)==1){
  number.of.max.distance=1
}else{
  distances=c()
for(i in 1:nrow(st)){
  cls=as.numeric(st[i,8])
  vec=(means[cls,]-as.numeric(st[i,1:7]))
  if(is.na(cov.mat[[cls]])){
    distances[i]=NA
  }else{
  distances[i]=(vec%*%cov.mat[[cls]])%*%vec    
  }
}
distances=sqrt(distances)
#номер элемента (в таблице неверно отнесённых) с максимальным расстоянием для своего кластера
number.of.max.distance=which.max(distances)
}

tt=st[number.of.max.distance,]#сам элемент
cat("Неправильно отнесённый элемент с максимальным расстоянием (",max(distances,na.rm = T),")\n")
#номер того же элемента, но в исходном фрейме
number.of.max.distance.new=find.number(data,tt)
print(data[number.of.max.distance.new,])
#сделать замену на кластер с минимальным расстоянием
data[number.of.max.distance.new,8]=predict(ldadat, tt[,1:7])$class#levels(data$CLASS)[which.min(distance(ldadat$means,cov.mat,as.numeric(tt[,-8])))]  
cat("Заменяется на\n")
print(data[number.of.max.distance.new,])
}


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

data2 =data.frame(read_excel("Приложение 3.xlsx")) 
data2= apply(data2,2,as.numeric) %>% as_data_frame()
data2=data2[31:80,]
(cluster=predict(rf, data2)$class)

data2=data.frame(cbind(data2,cluster))
data2$cluster=factor(data2$cluster)

library(ggplot2)
library(ggpubr)

ggarrange(
  ggplot(data2,aes(x=X1,fill=cluster))+
    geom_density(alpha=0.6),
  ggplot(data2,aes(x=X2,fill=cluster))+
    geom_density(alpha=0.6),
  ggplot(data2,aes(x=X3,fill=cluster))+
    geom_density(alpha=0.6),
  ggplot(data2,aes(x=X4,fill=cluster))+
    geom_density(alpha=0.6),
  ggplot(data2,aes(x=X5,fill=cluster))+
    geom_density(alpha=0.6),
  ggplot(data2,aes(x=X6,fill=cluster))+
    geom_density(alpha=0.6),
  ggplot(data2,aes(x=X7,fill=cluster))+
    geom_density(alpha=0.6),
          ncol = 2, nrow = 4)

ggarrange(
  ggplot(data2,aes(x=cluster,y=X1))+
    geom_boxplot(),
  ggplot(data2,aes(x=cluster,y=X2))+
    geom_boxplot(),
  ggplot(data2,aes(x=cluster,y=X3))+
    geom_boxplot(),
  ggplot(data2,aes(x=cluster,y=X4))+
    geom_boxplot(),
  ggplot(data2,aes(x=cluster,y=X5))+
    geom_boxplot(),
  ggplot(data2,aes(x=cluster,y=X6))+
    geom_boxplot(),
  ggplot(data2,aes(x=cluster,y=X7))+
    geom_boxplot(),
  ncol = 2, nrow = 4)

newdata=as_data_frame(data2)%>%group_by(cluster)%>%
  summarise_all(funs(mean))
faces(newdata[,2:8])#рисуем лица

if(FALSE){
  ggplot(data2,aes(x=X6,fill=cluster))+
  geom_density(position = "stack")

ggplot(data2,aes(x=X5,fill=cluster))+
  geom_density(position = "fill")

ggplot(data2,aes(x=cluster,y=X7))+
  geom_boxplot()

ggplot(data2,aes(x=cluster,y=X4))+
  geom_boxplot()+ coord_flip()

ggplot(data2,aes(x=X5,y=X1,col=cluster))+
  geom_point(size=3)

ggplot(data2, aes(x = X2, y = X3,col=cluster)) +
 stat_density_2d(aes(fill = stat(level)), geom = "polygon")
}







