library(rgl)
library(plot3D)
library(data.table)

xx = fread("3D ur, uz(x).txt", header = TRUE, dec = ",")
yy = fread("3D ur, uz(y).txt", header = TRUE, dec = ",")
z = fread("3D ur, uz.txt", header = TRUE, dec = ",")
x = xx$x#;xx
y = yy$y

urRe = matrix(z$urRe, nrow = length(x), ncol = length(y), byrow = TRUE)
urIm = matrix(z$urIm, nrow = length(x), ncol = length(y), byrow = TRUE)
urAbs = matrix(z$urAbs, nrow = length(x), ncol = length(y), byrow = TRUE)
uzRe = matrix(z$uzRe, nrow = length(x), ncol = length(y), byrow = TRUE)
uzIm = matrix(z$uzIm, nrow = length(x), ncol = length(y), byrow = TRUE)
uzAbs = matrix(z$uzAbs, nrow = length(x), ncol = length(y), byrow = TRUE)

#tit = scan("tmp.txt", what = text())
#tit

urmin = -20
urmax = 20
levels = 15

pdf(file = "3D ur, uz.pdf", paper = "a4")
par(mfrow = c(3, 2))

pp=persp3D(z = urRe, x = x, y = y, scale = FALSE, zlab = "Re ur(x,y)",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(urmin, max(urRe, na.rm = TRUE) * 0.3),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "Re ur")

persp3D(z = uzRe, x = x, y = y, scale = FALSE, zlab = "Re uz(x,y)",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(-40, max(uzRe, na.rm = TRUE) * 0.7),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "Re uz")

persp3D(z = urIm, x = x, y = y, scale = FALSE, zlab = "Im ur(x,y)",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(-20, max(urIm, na.rm = TRUE) * 0.3),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "Im ur")

persp3D(z = uzIm, x = x, y = y, scale = FALSE, zlab = "Im uz(x,y)",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(-40, max(uzIm, na.rm = TRUE) * 0.7),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "Im uz")

persp3D(z = urAbs, x = x, y = y, scale = FALSE, zlab = "Abs ur(x,y)",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(-20, max(urAbs, na.rm = TRUE) * 0.3),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "Abs ur")

persp3D(z = uzAbs, x = x, y = y, scale = FALSE, zlab = "Abs uz(x,y)",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(-40, max(uzAbs, na.rm = TRUE) * 0.7),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "Abs uz")

#savePlot("3D ur, uz.pdf", type = pdf)
#dev.print(pdf, file = "3D ur, uz.pdf");
par(mfrow = c(1, 2))

persp3D(z = urRe, x = x, y = y, scale = FALSE, zlab = "Re ur(x,y)",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(urmin, max(urRe, na.rm = TRUE) * 0.3),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "Re ur")

persp3D(z = uzRe, x = x, y = y, scale = FALSE, zlab = "Re uz(x,y)",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(-40, max(uzRe, na.rm = TRUE) * 0.7),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "Re uz")

persp3D(z = urIm, x = x, y = y, scale = FALSE, zlab = "Im ur(x,y)",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(-20, max(urIm, na.rm = TRUE) * 0.3),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "Im ur")

persp3D(z = uzIm, x = x, y = y, scale = FALSE, zlab = "Im uz(x,y)",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(-40, max(uzIm, na.rm = TRUE) * 0.7),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "Im uz")

persp3D(z = urAbs, x = x, y = y, scale = FALSE, zlab = "Abs ur(x,y)",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(-20, max(urAbs, na.rm = TRUE) * 0.3),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "Abs ur")

persp3D(z = uzAbs, x = x, y = y, scale = FALSE, zlab = "Abs uz(x,y)",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(-40, max(uzAbs, na.rm = TRUE) * 0.7),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "Abs uz")

par(mfrow = c(1, 1))

persp3D(z = urRe, x = x, y = y, scale = FALSE, zlab = "Re ur(x,y)",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(urmin, max(urRe, na.rm = TRUE) * 0.3),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "Re ur")

persp3D(z = uzRe, x = x, y = y, scale = FALSE, zlab = "Re uz(x,y)",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(-40, max(uzRe, na.rm = TRUE) * 0.7),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "Re uz")

persp3D(z = urIm, x = x, y = y, scale = FALSE, zlab = "Im ur(x,y)",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(-20, max(urIm, na.rm = TRUE) * 0.3),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "Im ur")

persp3D(z = uzIm, x = x, y = y, scale = FALSE, zlab = "Im uz(x,y)",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(-40, max(uzIm, na.rm = TRUE) * 0.7),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "Im uz")

persp3D(z = urAbs, x = x, y = y, scale = FALSE, zlab = "Abs ur(x,y)",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(-20, max(urAbs, na.rm = TRUE) * 0.3),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "Abs ur")

persp3D(z = uzAbs, x = x, y = y, scale = FALSE, zlab = "Abs uz(x,y)",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(-40, max(uzAbs, na.rm = TRUE) * 0.7),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "Abs uz")

dev.off()

library(plotly)
p1 = plot_ly(x = x, y = y, z = ~urRe, type = "surface", contours = list(
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
      ))
p2 = plot_ly(x = x, y = y, z = ~urIm, type = "surface", contours = list(
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
      ))
p3 = plot_ly(x = x, y = y, z = ~urAbs, type = "surface", contours = list(
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
      ))
p4=plot_ly(x = x, y = y, z = ~uzRe, type = "surface", contours = list(
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
      ))
p5 = plot_ly(x = x, y = y, z = ~uzIm, type = "surface", contours = list(
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
      ))
p6 = plot_ly(x = x, y = y, z = ~uzAbs, type = "surface", contours = list(
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
      ))

library(htmlwidgets)

saveWidget(as.widget(p1), "urRe.html",FALSE)
saveWidget(as.widget(p2), "urIm.html", FALSE)
saveWidget(as.widget(p3), "urAbs.html", FALSE)
saveWidget(as.widget(p4), "uzRe.html", FALSE)
saveWidget(as.widget(p5), "uzIm.html", FALSE)
saveWidget(as.widget(p6), "uzAbs.html", FALSE)

#plot_ly(showscale = FALSE) %>%
#    add_surface(x = x, y = y, z = urAbs) %>%
#    add_surface(x = x, y = y, z = uzAbs, opacity = 0.98)