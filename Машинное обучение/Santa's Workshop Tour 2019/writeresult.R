
res=read_csv("res.csv")
res$assigned_day=best.res
write_csv(res,"res.csv")
