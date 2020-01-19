bes=read_csv("res.csv")
res= bes$assigned_day

choise=-res
choise[res==choises[1]]=0
choise[res==choises[2]]=1
choise[res==choises[3]]=2
choise[res==choises[4]]=3
choise[res==choises[5]]=4
choise[res==choises[6]]=5
choise[res==choises[7]]=6
choise[res==choises[8]]=7
choise[res==choises[9]]=8
choise[res==choises[10]]=9
choise[choise<0]=-1

which(res==choises[6])

df=data.frame(fam=bes$family_id,ch=factor(choise,levels = c("0","1","2","3","4","5","6","7","8","9","no")),np=factor(peop)) %>% tbl_df()

counts=df %>% group_by(ch,np) %>% summarise(ct=n())

ggplot(counts,aes(x=ch,y=ct))+
  facet_wrap(vars(np))+
  geom_col()+
  theme_light()

counts

qplot(x=factor(res))


ds=data.frame(p=peop,dc=factor(res)) %>% group_by(dc) %>% summarise(val=sum(p))

ggplot(ds,aes(x=dc,y=val))+
  geom_col()+
  labs(title=paste("acc =",accounting.penalty(res),"  pre =",preference.cost(res),"  sum =",score(res)))+
  geom_hline(yintercept =c( 125,300),col="red",size=1.5)+
  theme_bw()


ds=data.frame(p=peop,dc=factor(res),np=factor(peop)) %>% group_by(dc,np) %>% summarise(val=sum(p))

ggplot(ds,aes(x=dc,y=val))+
  geom_col()+
  facet_wrap(vars(np))+
  labs(title=paste("acc =",accounting.penalty(res),"  pre =",preference.cost(res)))+
# geom_hline(yintercept =c( 125,300),col="red",size=1.5)+
  theme_bw()


N=ds$val[1:99]
N2=c(N,N[99])[2:100]
rs=( (N-125)/400*N^(0.5+0.02*abs(N-N2)))


########################################

resold= read_csv("resold.csv")$assigned_day

tt=cbind(i=1:5000,resold,res,peop,choises[,1:6]) %>% tbl_df()

pp=tt %>% filter(resold!=res)




###
alien=data.table::fread("alien3.txt",header = T,sep='\t')

best.res=alien$assigned_day

res=read_csv("res.csv")
res$assigned_day=best.res
#best.res=res$assigned_day
write_csv(res,"res.csv")


#####################
library(factoextra)  #графика по главным компонентам
print(fviz_cluster(list(data = choises[, 1:5], cluster =  res), ellipse.type = "norm"))



#######
ifelse(res==choises[1]&res==1,peop,0) %>% sum()

ifelse(choises[1]==1,peop,0) %>% factor()%>%  summary()


bes=read_csv("res.csv")
res= bes$assigned_day

ds=data.frame(p=peop,dc=factor(res)) %>% group_by(dc) %>% summarise(val=sum(p))

ggplot(ds,aes(x=dc,y=val))+
  geom_col()+
  labs(title=paste("acc =",accounting.penalty(res),"  pre =",preference.cost(res)))+
  geom_hline(yintercept =c( 125,300),col="red",size=1.5)+
  theme_bw()


res[choises[1]==1&peop==8]=1

res[(res==1&peop==2)]=choises[(res==1&peop==2),2]%>% unclass() %$%choice_1 

day=97
chs=4
inds=ifelse(choises[chs]==day,1,0)
sum((res==1)*inds)
res[(res==1)*inds]=day 


res[res==1&peop==4]=choises[res==1&peop==4,3] %>% unclass() %$%choice_2 
res[res==1&peop<=3]=choises[res==1&peop<=3,4] %>% unclass() %$%choice_3 


tt=cbind(i=1:5000,res,peop,choises[,1:6]) %>% tbl_df()

res[3249]=2



