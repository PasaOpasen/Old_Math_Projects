#---------Сравнение аппроксимаций
#
#
#
#
library(rgl)
library(plot3D)
library(data.table)

xx = fread("arg2.txt", header = F, dec = ",")
z = fread("val2.txt", header = F, dec = ",")
zl = fread("val2l.txt", header = F, dec = ",")
x = xx[[1]] #;xx
y = xx[[2]]

scalecoef = (max(y) - min(y)) / (max(x) - min(x))
x2=x*scalecoef

z4 = matrix(z[[1]], nrow = length(x), ncol = length(y), byrow = TRUE)
z4=log10(z4)
z4l = matrix(zl[[1]], nrow = length(x), ncol = length(y), byrow = TRUE)
z4l = log10(z4l)

urmin = -20
urmax = 20
levels = 15


dt = readLines("info2.txt")

pdf(file = dt[[1]], height = 12, paper = "letter")
par(mfrow = c(1, 2))


persp3D(z = z4, x = x2, y = y, scale = FALSE, zlab = "log10(eps)",xlab="Basis p. count",ylab="L radius",
       #contour = list(nlevels = levels, col = "red"),
#zlim = c(-40, max(uzIm, na.rm = TRUE) * 0.7),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "Approx. dependence of density on p. count and L radius")

persp3D(z = z4l, x = x2, y = y, scale = FALSE, zlab = "log10(eps)", xlab = "Basis p. count", ylab = "L radius",
#contour = list(nlevels = levels, col = "red"),
#zlim = c(-40, max(uzIm, na.rm = TRUE) * 0.7),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "Approx. dependence of potential on p. count and L radius")

dev.off()

L_radius = y
P.count = x
ln_norm=z4

library(plotly)
p1 = plot_ly(x = ~P.count, y = ~L_radius, z = ~ln_norm, type = "surface", contours = list(
    z = list(
      show = TRUE,
      usecolormap = TRUE,
      highlightcolor = "#ff0000",
      project = list(z = TRUE)
      )
    )
  ) %>%
  layout(
    scene = list(
      camera = list(
        eye = list(x = 1.87, y = 0.88, z = -0.64)
        )
      )
      )

ln_norm = z4l
p2 = plot_ly(x = ~P.count, y = ~L_radius, z = ~ln_norm, type = "surface", contours = list(
    z = list(
      show = TRUE,
      usecolormap = TRUE,
      highlightcolor = "#ff0000",
      project = list(z = TRUE)
      )
    )
  ) %>%
  layout(
    scene = list(
      camera = list(
        eye = list(x = 1.87, y = 0.88, z = -0.64)
        )
      )
      )

library(htmlwidgets)

saveWidget(as.widget(p1), dt[[2]], FALSE)
saveWidget(as.widget(p2), dt[[3]], FALSE)