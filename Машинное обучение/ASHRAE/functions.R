source("libraries.r", local=T)

#####функции
score=function(target,predict){
  s=sum((log(1+predict)-log(1+target[seq(predict)]))^2)
  sqrt(s/length(predict))
}
predpred=function(model,df) round(abs(predict(model,df[,-2])),2)

Score=function(model,df,deg=2){
pr=predpred(model,df)
score(df$meter_reading,pr^deg)
}
ScoreLog=function(model,df){
  pr=predpred(model,df)
  score(df$meter_reading,exp(pr))
}

ScoreBinary=function(model,df){
  s=predict(model,df[,-2])
  sum(s==df$meter_reading)/nrow(df)
}

sigmoid=function(vec) 1/(1+exp(-vec))

tanh=function(vec) 2*sigmoid(2*vec)-1

relu=function(vec) ifelse(vec<0,0,vec)

coefs=function(best,others,tg){
  n=nrow(others)
  res=rep(1,n)
  t=function(vec){
    s=best
    vec=abs(vec)
    for(i in seq(n)) s=s+vec[i]*others[i,]
    score(tg,s/(1+sum(vec))) 
  }
  
  # Find minimum and show trace
  tt= ucminf(par = res, fn = t, control = list(trace = 1,grtol=1e-12,maxeval=500))
  print(tt)
  tt$par
}