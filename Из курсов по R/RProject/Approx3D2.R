#---------Сравнение аппроксимаций
#
#
#
#
library(rgl)
library(plot3D)
library(data.table)

xx = fread("(arg).txt", header = F, dec = ",")
z = fread("(vals).txt", header = F, dec = ",")
x = xx[[1]] #;xx
y = xx[[2]]


z1 = matrix(z[[1]], nrow = length(x), ncol = length(y), byrow = TRUE)
z2 = matrix(z[[2]], nrow = length(x), ncol = length(y), byrow = TRUE)
z3 = matrix(z[[3]], nrow = length(x), ncol = length(y), byrow = TRUE)
z4 = matrix(z[[4]], nrow = length(x), ncol = length(y), byrow = TRUE)
z5 = matrix(z[[5]], nrow = length(x), ncol = length(y), byrow = TRUE)
#z6 = matrix(z[[6]], nrow = length(x), ncol = length(y), byrow = TRUE)

p1 = matrix(abs(z1 - z2), nrow = length(x), ncol = length(y), byrow = TRUE)
p2 = matrix(abs(z1 - z3), nrow = length(x), ncol = length(y), byrow = TRUE)
p3 = matrix(abs(z1 - z4), nrow = length(x), ncol = length(y), byrow = TRUE)
p4 = matrix(abs(z1 - z5), nrow = length(x), ncol = length(y), byrow = TRUE)
#p5 = matrix(abs(z1 - z6), nrow = length(x), ncol = length(y), byrow = TRUE)

#tit = scan("tmp.txt", what = text())
#tit

urmin = -20
urmax = 20
levels = 15

dt = readLines("info.txt")
meth = readLines("methods.txt");


pdf(file = dt[[1]], height = 12, paper = "letter")
#par(mfrow = c(3, 1))

split.screen(c(2, 1))
split.screen(c(1, 2), screen = 1)
split.screen(c(1, 2), screen = 2)
screen(6)
persp3D(z = p1, x = x, y = y, scale = FALSE, zlab = "z",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(-40, max(uzIm, na.rm = TRUE) * 0.7),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = meth[[1]])

screen(3)
persp3D(z = p2, x = x, y = y, scale = FALSE, zlab = "z",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(-20, max(urAbs, na.rm = TRUE) * 0.3),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = meth[[2]])

screen(4)
persp3D(z = p3, x = x, y = y, scale = FALSE, zlab = "z",
       contour = list(nlevels = levels, col = "red"),
##zlim = c(-40, max(uzIm, na.rm = TRUE) * 0.7),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = meth[[3]])

screen(5)
persp3D(z = p4, x = x, y = y, scale = FALSE, zlab = "z",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(-20, max(urAbs, na.rm = TRUE) * 0.3),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = meth[[4]])

close.screen(all = TRUE)

dev.off()