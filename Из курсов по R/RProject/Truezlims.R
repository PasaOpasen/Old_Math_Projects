library(rgl)
library(plot3D)
library(data.table)

w = getwd()

cur = c(0, 0)
cuz=cur

stt = readLines("textnames.txt")
for (i in 1:(length(stt))) {
    s = stt[[i]]
    s1 = sub(".txt", " (ur).txt", s, fixed = TRUE)
    s2 = sub(".txt", " (uz).txt", s, fixed = TRUE)
    ur = scan(s1)
    uz = scan(s2)

    cur = c(min(cur, ur, na.rm = TRUE), max(cur, ur, na.rm = TRUE))
    cuz = c(min(cuz, uz, na.rm = TRUE), max(cuz, uz, na.rm = TRUE))
}

    l = list(cur, cuz)
    names(l) = c("ur", "uz")
#fwrite(l, "zlims(real).txt", sep = " ", dec = ",", col.names = TRUE)
write.table(l, file = "zlims(real).txt", row.names = FALSE)
cat("zlab lims are calculated")
