
res=read_csv("sample_submission.csv")
res$assigned_day=best.res
write_csv(res,"res.csv")
