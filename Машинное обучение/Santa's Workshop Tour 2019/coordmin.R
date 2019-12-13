


best.res=sample(1:100,5000,replace = T)

best.val=score(best.res)

while(T){
  mat=rep(best.res,100) %>% matrix(nrow = 5000,byrow = F)
for(i in seq(5000)){
 mat[i,]=1:100 
 num=apply(mat, 2, score)
 mat[i,]=rep(which.min(num),100)
 if(min(num)<best.val){
   best.val=min(num)
  cat('iter',i,'score',best.val,'\n')  
 }

}
best.res=mat[,1]
best.val=score(best.res)
source("writeresult.r")
}







