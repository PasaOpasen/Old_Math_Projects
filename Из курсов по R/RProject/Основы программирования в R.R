
decorate_string <- function(pattern, ...) { 
  res=paste(...)
  pt=intToUtf8(rev(utf8ToInt(pattern)))#интересный реверс строки!
  return(paste0(pattern,res,pt))
}

decorate_string(pattern = "123", "abc")            # "123abc321"
decorate_string(pattern = "123", "abc", "def")     # "123abc def321"
decorate_string(pattern = "123", c("abc", "def"))  # "123abc321" "123def321" (вектор длины 2)  

decorate_string(pattern = "123", "abc", "def", sep = "+")    # "123abc+def321"
decorate_string(pattern = "!", c("x", "x"), collapse = "_")  # "!x_x!"
decorate_string(pattern = ".:", 1:2, 3:4, 5:6, sep = "&")    # ".:1&3&5:." ".:2&4&6:." (вектор длины 2)




generator<- function(set, prob = rep(1/length(set), length(set)))
  function(n) sample(set, n, replace=T, prob) 

roulette_values <- c("Zero!", 1:36)
roulette_values
a<-c(rep(2/(length(roulette_values)+1),1),rep(1/(length(roulette_values) +1), (length(roulette_values)-1)))

print(sum(a))
print(a) 

fair_roulette<- generator(roulette_values) 
fair_roulette(37)

rigged_roulette <- generator(roulette_values, prob=a) 
rigged_roulette(37)



df1=read.csv(url("https://github.com/tonytonov/Rcourse/raw/master/R%20programming/avianHabitat.csv"))
df2=read.csv(url("https://github.com/tonytonov/Rcourse/raw/master/R%20programming/avianHabitat2.csv"),skip=5,sep = ";",dec=".",na.strings = "Don't remember")
names(df2)=names(df1[,-2])
df=rbind(df1[,-2],df2)
df[,ncol(df)]=as.numeric(df[,ncol(df)])

cv=names(df)[-(1:3)][c(T,F)]
df$total=rowSums(df[,cv])
summary(df$total)


sort(sapply(df[,endsWith(names(df),"Ht")], max),decreasing = T) 



df=cbind(df1[,2],df1[,endsWith(names(df1),"Ht")]) 


vc=sapply(df[,-1], max)
for(i in 2:ncol(df)){
  k= which(df[,i]==vc[i-1])
  print(df[k,c(1,i)])
}



cat_temper <- c("задиристый", "игривый", "спокойный", "ленивый")
cat_color <- c("белый", "серый", "чёрный", "рыжий")
cat_age <- c("кот", "котёнок")
cat_trait <- c("с умными глазами", "с острыми когтями", "с длинными усами")

library(stringi)
#всевозможные комбинации
d=expand.grid(cat_temper,cat_color,cat_age,cat_trait)
m=mapply(paste, d[[1]],d[[2]],d[[3]],d[[4]])
sort(m)



df=cbind(df1[,1:2],df1[,endsWith(names(df1),"Ht")]) 
names(df)[1:2]=c("P","S")
df=transform(df,P=factor(stri_replace(df$P,regex="[:digit:]+","")) )

#df[,1]=sapply(df[,1],function(t)stri_sub(t,length = nchar(t)-2))          
library(dplyr)
df %>% group_by(P,S) %>% summarise_each(funs = function(x)sum(x>0))




