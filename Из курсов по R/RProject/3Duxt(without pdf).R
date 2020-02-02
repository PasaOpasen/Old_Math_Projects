library(data.table)

x = fread("3D ur, uz(x).txt", header = TRUE, dec = ",")$x
y = fread("3D ur, uz(y).txt", header = TRUE, dec = ",")$y
z = fread("3D ur, uz.txt", header = TRUE, dec = ",")


ur = matrix(z$ur, nrow = length(x), ncol = length(y), byrow = TRUE)
uz = matrix(z$uz, nrow = length(x), ncol = length(y), byrow = TRUE)

st = readLines("3D ur, uz(title).txt")[[1]]

sink(paste(s, "(ur).txt"))
cat(ur)
sink()
sink(paste(s, "(uz).txt"))
cat(uz)
sink()
