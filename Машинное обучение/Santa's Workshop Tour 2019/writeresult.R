
res=read_csv("res.csv")
res$assigned_day=best.res
#best.res=res$assigned_day
write_csv(res,"res.csv")
