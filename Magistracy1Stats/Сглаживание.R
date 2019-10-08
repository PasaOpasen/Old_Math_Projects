
y=read.table("ArrayE.txt")
y=y[[1]]
y=y[seq(1,length(y),by=8)]
x=1:length(y)

#n=1000
#x=seq(1770,2050,length.out = n)
#y=rchisq(n,14)

#полиномы
spanlist=c(0.01,0.015,0.02)
plot(x,y,type="l",col="grey")
for(i in 1:length(spanlist)){
  ls=loess(y~x,span=spanlist[i])
  lines(x,predict(ls),col=i,lwd=2)
}

legend("topleft",c(paste("span =", spanlist)),col=1:length(spanlist),bty="n",lwd=2)




#ядерное сглаживание
bandlist=c(80,120,160)
plot(x,y,type="l",col="grey")
for(i in 1:length(spanlist)){
  lines(ksmooth(x,y,"normal", bandwidth = bandlist[i]),col=i,lwd=2)
}

legend("topleft",c(paste("band =", bandlist)),col=1:length(bandlist),bty="n",lwd=2)




#сплайны
sparlist=c(-0.13)
plot(x,y,type="l",col="grey")
for(i in 1:length(sparlist)){
  lines(smooth.spline(x,y, spar = sparlist[i]),col=i,lwd=2)
}
#lines(smooth.spline(x,y, cv=TRUE),col=i,lwd=2)
legend("topleft",c(paste("spar =", sparlist)),col=1:length(sparlist),bty="n",lwd=2)


#скользящее среднее
library(caTools)
k=c(3,5,9)
plot(x,y,type="l",col="grey")
for(i in 1:length(k)){
  lines(x,runmean(y,k[i]),col=i,lwd=2)
}
legend("topleft",c(paste("k =", k)),col=1:length(k),bty="n",lwd=2)



