
library(rgl)
library(plot3D)
library(plotly)
library(magrittr)
library(htmlwidgets)


readvec=function(file){
  x=read.table(file)[[1]] %>% as.character()
x[1]=stringi::stri_sub(x[1],4)
x %<>%stringi::stri_replace_all_fixed(",","."  )%>% as.numeric() 
x
}

x=readvec("x.txt")
y=readvec("y.txt")

u=read.table("u.txt",dec=',')
u[[3]]=sqrt(u[[1]]^2+u[[2]]^2)
colnames(u)=c("Re","Im","Abs")

uRe=matrix(u$Re,nrow=length(x),byrow = T)
uIm=matrix(u$Im,nrow=length(x),byrow = T)
uAbs=matrix(u$Abs,nrow=length(x),byrow = T)

levels = 15
pdf(file ="u.pdf", width = 7, height = 14,paper = "special")

xs=x/length(x)
ys=y/length(y)
tmp = (max(uAbs) - min(uAbs)) / (max(xs) - min(xs))

layout(matrix(1:6, 3, 2, byrow = T), widths = c(1.8, 1))

persp3D(z = uAbs, x = xs * tmp, y = ys * tmp, scale = FALSE, zlab = "|u|", xlab = "x", ylab = "z",
        contour = list(nlevels = levels, col = "red"),
        expand = 0.2,
        image = list(col = grey(seq(0, 1, length.out = 100))), main = "|u(x,z)|",clab = "|u|")


image(x, -y, uAbs, col = topo.colors(20), main = "|u(x,z)|",xlab="x",ylab="-z")

persp3D(z = uRe, x = xs * tmp, y = ys * tmp, scale = FALSE, zlab = "Re u", xlab = "x", ylab = "z",
        contour = list(nlevels = levels, col = "red"),
        expand = 0.2,
        image = list(col = grey(seq(0, 1, length.out = 100))), main = "Re u(x,z)",clab = "Re u")


image(x, -y, uRe, col = topo.colors(20), main = "Re u(x,z)",xlab="x",ylab="-z")

persp3D(z = uIm, x = xs * tmp, y = ys * tmp, scale = FALSE, zlab = "Im u", xlab = "x", ylab = "z",
        contour = list(nlevels = levels, col = "red"),
        expand = 0.2,
        image = list(col = grey(seq(0, 1, length.out = 100))), main = "Im u(x,z)",clab = "Im u")


image(x, -y, uIm, col = topo.colors(20), main = "Im u(x,z)",xlab="x",ylab="-z")

dev.off()

uAbs %<>%t() 
uRe %<>%t() 
p1 = plot_ly(x = x, y = y, z = uAbs, type = "surface", contours = list(
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
p2 = plot_ly(x = x, y = y, z = uRe, type = "surface", contours = list(
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

saveWidget(as.widget(p1),  "Abs u.html", FALSE)
saveWidget(as.widget(p2),  "Re u.html", FALSE)




























