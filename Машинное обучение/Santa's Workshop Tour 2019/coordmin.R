library(compiler)


best.res=sample(1:100,5000,replace = T)

best.val=score(best.res)

while(T){
  mat=rep(best.res,100) %>% matrix(nrow = 5000,byrow = F)
  seqs=sample(1:5000,5000)
for(i in seqs){
  mat[i,]=1:100 
  num=apply(mat, 2, score)
  mat[i,]=rep(which.min(num),100)
  
  if(min(num)<best.val){
    best.val=min(num)
    best.res=mat[,1]
    cat('iter',i,'score',best.val,'\n')  
  }
}
source("writeresult.r")
}


itr=function(i){
  mat[i,]=1:100 
  num=apply(mat, 2, score)
  mat[i,]=rep(which.min(num),100)
  
  if(min(num)<best.val){
    best.val=min(num)
    best.res=mat[,1]
    cat('iter',i,'score',best.val,'\n')  
  }
}

itr2=cmpfun(itr)

system.time(itr(4))
system.time(itr2(4))

