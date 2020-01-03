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

df=data.frame(fam=bes$family_id,ch=factor(choise,levels = c("0","1","2","3","4","5","6","7","8","9","no"))) %>% tbl_df()

counts=df %>% group_by(ch) %>% summarise(ct=n())

ggplot(counts,aes(x=ch,y=ct))+
  geom_col()

counts

qplot(x=factor(res))


ds=data.frame(p=peop,dc=factor(res)) %>% group_by(dc) %>% summarise(val=sum(p))

ggplot(ds,aes(x=dc,y=val))+
  geom_col()+
  labs(title=paste("acc =",accounting.penalty(res),"  pre =",preference.cost(res)))+
  geom_hline(yintercept =c( 125,300),col="red",size=1.5)+
  theme_bw()


ds=data.frame(p=peop,dc=factor(res),np=factor(peop)) %>% group_by(dc,np) %>% summarise(val=sum(p))

ggplot(ds,aes(x=dc,y=val))+
  geom_col()+
  facet_wrap(vars(np))+
  labs(title=paste("acc =",accounting.penalty(res),"  pre =",preference.cost(res)))+
 geom_hline(yintercept =c( 125,300),col="red",size=1.5)+
  theme_bw()


N=ds$val[1:99]
N2=c(N,N[99])[2:100]
rs=( (N-125)/400*N^(0.5+0.02*abs(N-N2)))



