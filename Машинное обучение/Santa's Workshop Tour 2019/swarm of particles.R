
get.sample=function()sample(1:100,5000,replace = T)



w = 0.03; fp = 2; fg = 3
fi=fp+fg
coef=2 * w / abs(2 - fi - sqrt(fi * (fi - 4)))

count.point=20
count.iter=20

#заполнение 
hive=list()
pos=get.sample()
val=1e100
best=list(pos,val)
for(i in seq(count.point)){
  s=get.sample()
  N=cbind(peop,d=factor(s)) %>% tbl_df() %>% group_by(d) %>% summarise(sums=sum(peop)) %$%sums 
  while(sum(N<125|N>300)>0){
    s=get.sample()
    N=cbind(peop,d=factor(s)) %>% tbl_df() %>% group_by(d) %>% summarise(sums=sum(peop)) %$%sums 
  }
  
  v=get.sample()
  
  val=score(s)
  hive[[i]]=list(s,s,v,val)# x , p , v , val
  if(val<best[[2]]){
    best=list(pos=s,val)
    cat(val,"\n")
  }
}



#алгоритм
for(k in seq(count.iter)){
  
  cat("--iter",k,"\n")
  for(i in seq(count.point)){
   # cat("-----point",i,"\n")
    
    v=hive[[i]][[3]]
    v=(coef*(v+fp*sample(hive[[i]][[2]]-hive[[i]][[1]],5000,replace=T)+
                           fg*sample(best[[1]]-hive[[i]][[1]],5000,replace=T))) %>% round()
    
    s=hive[[i]][[1]]+v
    if(min(s)<1) s=(s-min(s))+1
    s=round(s*100/max(s))
    N=cbind(peop,d=factor(s)) %>% tbl_df() %>% group_by(d) %>% summarise(sums=sum(peop)) %$%sums 
    while(sum(N<125|N>300)>0
          ){
      v=hive[[i]][[3]]
      v=(coef*(v+fp*sample(hive[[i]][[2]]-hive[[i]][[1]],5000,replace=T)+
                 fg*sample(best[[1]]-hive[[i]][[1]],5000,replace=T))) %>% round()
     # print(v)
      s=hive[[i]][[1]]+v
      if(min(s)<0) s=(s-min(s))
      s=round(s*100/max(s))
      N=cbind(peop,d=factor(s)) %>% tbl_df() %>% group_by(d) %>% summarise(sums=sum(peop)) %$%sums 
    }
    hive[[i]][[1]]=s
    hive[[i]][[3]]=v
    
    val=score(s) 
    if(val<hive[[i]][[4]]){
      hive[[i]][[4]]=val
      hive[[i]][[2]]=s
      #cat(val,"\n")
    }
  }
  
  
  for(i in seq(count.point)){
    val=hive[[i]][[4]]
    if(val<best[[2]]){
      best=list(pos=hive[[i]][[2]],val)
      cat(val,"\n")
    }
  }
  
  
}
























