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


z4 = matrix(z[[1]], nrow = length(x), ncol = length(y), byrow = TRUE)
z5 = matrix(z[[2]], nrow = length(x), ncol = length(y), byrow = TRUE)
z6 = matrix(abs(z4 - z5), nrow = length(x), ncol = length(y), byrow = TRUE)

#tit = scan("tmp.txt", what = text())
#tit

urmin = -20
urmax = 20
levels = 15


dt = readLines("info.txt")

pdf(file = dt[[1]], height = 12, paper = "letter")
#par(mfrow = c(3, 1))

split.screen(c(2, 1))
split.screen(c(1, 2), screen = 1)
screen(3)
persp3D(z = z4, x = x, y = y, scale = FALSE, zlab = "z",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(-40, max(uzIm, na.rm = TRUE) * 0.7),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "Density")

screen(4)
persp3D(z = z5, x = x, y = y, scale = FALSE, zlab = "z",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(-20, max(urAbs, na.rm = TRUE) * 0.3),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "Density approx.")

screen(2)
persp3D(z = z6, x = x, y = y, scale = FALSE, zlab = "z",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(-40, max(uzAbs, na.rm = TRUE) * 0.7),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "Difference")

close.screen(all = TRUE)

dev.off()