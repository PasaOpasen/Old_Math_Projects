library(rgl)
library(plot3D)
library(data.table)

#Sys.setlocale("LC_ALL", "Russian_Russia")
w=getwd()
print(w)
source(paste0(w, "/Truezlims.r"), echo = FALSE, print.eval = FALSE)

stt = readLines("textnames.txt")
for (i in 1:(length(stt))) {
    s = stt[[i]]
    s1 = sub(".txt", " (ur).txt", s, fixed = TRUE)
    s2 = sub(".txt", " (uz).txt", s, fixed = TRUE)
    ur = scan(s1)
    uz = scan(s2)

    sink("3D ur, uz(title).txt")
    cat(sub(".txt", "", s))
    sink()
    l=list(ur,uz)
    names(l)=c("ur","uz")
    fwrite(l,"3D ur, uz.txt",sep = " ",dec = ",",col.names = TRUE)
    write.table(l,file = "3D ur, uz.txt",row.names = FALSE,quote = FALSE,dec = ",")
    
    source(paste0(w, "/3Duxt(better).r"), echo = FALSE, print.eval = FALSE)
     print(paste(i,"files from",length(stt), "are complited"))
}

