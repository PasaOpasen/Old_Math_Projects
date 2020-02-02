library(rgl)
library(plot3D)
library(data.table)

xx = fread("3D ur, uz(x).txt", header = TRUE, dec = ",")
yy = fread("3D ur, uz(y).txt", header = TRUE, dec = ",")
x = xx$x
y = yy$y
xy =(max(x) - min(x))/(max(y) - min(y))  

s = readLines("SurfaceMain.txt")[[1]]
cat(paste(s, "\n"))
ur = scan(paste0(s, " (ur).txt"))
uz = scan(paste0(s, " (uz).txt"))

if (FALSE) {
    coeff = 0.8
    ur = ur / (max(ur))
    ur[ur < coeff] = 0
    uz = uz / (max(uz))
    uz[uz < coeff] = 0

for (i in 1:3) {
    #coeff = 0.5
    ur = ur / (max(ur))
    #ur[ur < coeff] = 0
    uz = uz / (max(uz))
    #uz[uz < coeff] = 0
    ur = ur ^ 2
    uz = uz ^ 2
}
}


zm = max(ur, na.rm = TRUE)
x1 = x / (max(x) - min(x)) * zm
y1 = y / (max(y) - min(y)) * zm
zm = max(uz, na.rm = TRUE)
x2 = x / (max(x) - min(x)) * zm
y2 = y / (max(y) - min(y)) * zm

urr = matrix(ur, nrow = length(x), ncol = length(y), byrow = FALSE)
uzz = matrix(uz, nrow = length(x), ncol = length(y), byrow = FALSE)

levels = 30


cat(paste("Pdf...", "\n"))
ss = sub(", (xmin", " \n(xmin", s, fixed = TRUE)
ss = strsplit(ss, "\n")
s1 = ss[[1]][1]
s2 = ss[[1]][2]

pdf(file = paste(s, ".pdf"), width = 15, height = 12)
par(mfrow = c(1, 2), cex = 1.1, cex.sub = 1.2, col.sub = "blue")
layout(matrix(c(1, 2, 3, 4), 2, 2, byrow = FALSE), widths = c(2.5, 1))

pp = persp3D(z = urr, x = x, y = y, scale = TRUE, zlab = "ur(x,t)",
       contour = list(nlevels = levels, col = "red"),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "ur-Surface", sub = s1)

persp3D(z = uzz, x = x, y = y, scale = TRUE, zlab = "uz(x,t)",
       contour = list(nlevels = levels, col = "red"),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "uz-Surface", sub = s2)

image(x, y, abs(urr), col = heat.colors(20), main = "|ur|")
image(x, y, abs(uzz), col = topo.colors(20), main = "|uz|")

dev.off()



library(ggplot2)
library(viridis)
library(gridExtra)
library(fields)
library(plotly)

lenx = length(x)
leny = length(y)
cat(paste("Maps...", "\n"))

height = 500;
width = height * xy

if (lenx == leny) {
#png(filename = paste(s, "(heatmap).png"), height = height, width = width)
#par(cex = 1.0, cex.sub = 1.3, col.sub = "blue")
urt <- data.frame(ur.abs = c(abs(urr)), x = rep(x, lenx), y = rep(y, each = leny))

    ggplot(urt, aes(x, y, fill = ur.abs)) +
    scale_x_continuous(breaks = seq(min(x), max(x), length.out = 9)) +
    #scale_y_continuous(breaks = seq(max(y), min(y), length.out = 4)) +
    geom_raster(interpolate = TRUE) +
    coord_fixed(expand = FALSE) +
    scale_fill_viridis(option = "A", name = "|ur|") +
    theme(axis.title.x = element_text(size = 22), axis.title.y = element_text(size = 22), text = element_text(size = 19)) +
    scale_y_reverse()
    ggsave(paste(s, "(heatmap).png"))
#dev.off()
}


ur.Abs = abs(urr)
#print(ur.Abs)

p1 = plot_ly(x = x, y = y, z = ~ur.Abs, type = "surface", contours = list(
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

if (lenx == leny) {
#png(filename = paste(s, "(heatmap_uz).png"), height = height, width = width)
#par(cex = 1.0, cex.sub = 1.3, col.sub = "blue")
urt <- data.frame(uz.abs = c(abs(uzz)), x = rep(x, lenx), y = rep(y, each = leny))

ggplot(urt, aes(x, y, fill = uz.abs)) +
    scale_x_continuous(breaks = seq(min(x), max(x), length.out = 9)) +
    # scale_y_continuous(breaks = seq(max(y), min(y), length.out = 10)) +
    geom_raster(interpolate = TRUE) +
    coord_fixed(expand = FALSE) +
    scale_fill_viridis(option = "D", name = "|uz|") +
    theme(axis.title.x = element_text(size = 22), axis.title.y = element_text(size = 22), text = element_text(size = 19)) +
    scale_y_reverse()
    #dev.off()
    ggsave(paste(s, "(heatmap_uz).png"))
}

uz.Abs = abs(uzz)
p2 = plot_ly(x = x, y = y, z = ~uz.Abs, type = "surface", contours = list(
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

saveWidget(as.widget(p1),paste(s, "(ur).html"), FALSE)
saveWidget(as.widget(p2), paste(s, "(uz).html"), FALSE)

