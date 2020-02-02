data = data.frame(
    x = rep(c(0.1, 0.2, 0.3, 0.4, 0.5), each = 5),
    y = rep(c(1, 2, 3, 4, 5), 5)
)




data$z = runif(
    25,
    min = (sin (data$x) * sqrt( data$y) - 0.1 * (data$x * data$y)),
    max = (sin(data$x) * sqrt(data$y) + 0.1 * (data$x * data$y))
)
###################################
library(akima)
im <- with(data, interp(x, y, z))
with(im, image(x, y, z))

##############################
p <- wireframe(z ~ x * y, data = data)
npanel <- c(4, 2)
rotx <- c(-50, -80)
rotz <- seq(30, 300, length = npanel[1] + 1)
update(p[rep(1, prod(npanel))], layout = npanel,
    panel = function(..., screen) {
        panel.wireframe(..., screen = list(z = rotz[current.column()],
                                           x = rotx[current.row()]))
    })

######################################################äåëàåò ôîðìó

x <- 0:20
y <- 0:20
z <- x %o% y
z <- exp(3-sin(z/20))

library(rgl)
mycut = function(x, breaks) as.numeric(cut(x = x, breaks = breaks)) # to combine different factors
zcol2 = as.numeric(apply(z, 2, mycut, breaks = nbcol))
persp3d(x, y, z, theta = 50, phi = 25,col="red",zlab="zlabs")

###################################â áðàóçåð
x <- seq(from = 0.2, to = 5, by = 0.2)
y <- seq(from = 0.2, to = 5, by = 0.2)
z <- x %o% y
z <- z*cos(z) + .1 * z

library(plotly)
plot_ly(x = x, y = y, z = z, type = "surface")
#plot_ly(z = volcano, type = "surface")

######################################òîæå äåëàåò ôîðìó
library(rgl);
data(volcano)
dim(volcano)

peak.height <- volcano;
ppm.index <- (1:nrow(volcano));
sample.index <- (1:ncol(volcano));

zlim <- range(peak.height)
zlen <- zlim[2] - zlim[1] + 1
colorlut <- terrain.colors(zlen) # height color lookup table
col <- colorlut[(peak.height - zlim[1] + 1)] # assign colors to heights for each point
open3d()

ppm.index1 <- ppm.index * zlim[2] / max(ppm.index);
sample.index1 <- sample.index * zlim[2] / max(sample.index)

title.name <- paste("plot3d ", "volcano", sep = "");
surface3d(ppm.index1, sample.index1, peak.height, color = col, back = "lines", main = title.name);
grid3d(c("x", "y+", "z"), n = 20)

sample.name <- paste("col.", 1:ncol(volcano), sep = "");
sample.label <- as.integer(seq(1, length(sample.name), length = 5));

axis3d('y+', at = sample.index1[sample.label], sample.name[sample.label], cex = 0.3);
axis3d('y', at = sample.index1[sample.label], sample.name[sample.label], cex = 0.3)
axis3d('z', pos = c(0, 0, NA))

ppm.label <- as.integer(seq(1, length(ppm.index), length = 10));
axes3d('x', at = c(ppm.index1[ppm.label], 0, 0), abs(round(ppm.index[ppm.label], 2)), cex = 0.3);

title3d(main = title.name, sub = "test", xlab = "ppm", ylab = "samples", zlab = "peak")
rgl.bringtotop();

########################################



library(plotly)
x <- seq(-2, 2, 0.05)
y1 <- pnorm(x)
y2 <- pnorm(x, 1, 1)

plot_ly(x = x) %>%
    add_lines(y = y1, color = I("red"), name = "Red") %>%
    add_lines(y = y2, color = I("green"), name = "Green")





# save plotting parameters
pm <- par("mfrow")

## =======================================================================
## Ribbon, persp, color keys, facets
## =======================================================================
library(rgl)
library(plot3D)
par(mfrow = c(2, 2))
# simple, no scaling, use breaks to set colors
persp3D(z = volcano, main = "volcano", clab = c("height", "m"),
   breaks = seq(80, 200, by = 10))

# keep ratios between x- and y (scale = FALSE) 
# change ratio between x- and z (expand)
persp3D(z = volcano, x = 1:nrow(volcano), y = 1:ncol(volcano),
       expand = 0.3, main = "volcano", facets = FALSE, scale = FALSE,
       clab = "height, m", colkey = list(side = 1, length = 0.5))

# ribbon, in x--direction
V <- volcano[, seq(1, ncol(volcano), by = 3)] # lower resolution
ribbon3D(z = V, colkey = list(width = 0.5, length = 0.5,
          cex.axis = 0.8, side = 2), clab = "m")

# ribbon, in y-direction
Vy <- volcano[seq(1, nrow(volcano), by = 3),]
ribbon3D(z = Vy, expand = 0.3, space = 0.3, along = "y",
          colkey = list(width = 0.5, length = 0.5, cex.axis = 0.8))

## =======================================================================
## Several ways to visualise 3-D data
## =======================================================================
library(rgl)
library(plot3D)
x <- seq(-pi, pi, by = 0.2)
y <- seq(-pi, pi, by = 0.3)
grid <- mesh(x, y)

z <- with(grid, cos(x) * sin(y))

par(mfrow = c(2, 2))

persp3D(z = z, x = x, y = y)

persp3D(z = z, x = x, y = y, facets = FALSE, curtain = TRUE)

# ribbons in two directions and larger spaces
ribbon3D(z = z, x = x, y = y, along = "xy", space = 0.3)

hist3D(z = z, x = x, y = y, border = "black")

## =======================================================================
## Contours and images added
## =======================================================================
library(rgl)
library(plot3D)
par(mfrow = c(2, 2))
x <- seq(1, nrow(volcano), by = 3)
y <- seq(1, ncol(volcano), by = 3)

Volcano <- volcano[x, y]
ribbon3D(z = Volcano, contour = TRUE, zlim = c(-100, 200),
          image = TRUE)

persp3D(z = Volcano, contour = TRUE, zlim = c(-200, 200), image = FALSE)

persp3D(z = Volcano, x = x, y = y, scale = FALSE,
       contour = list(nlevels = 20, col = "red"),
       zlim = c(-200, 200), expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))))

persp3D(z = Volcano, contour = list(side = c("zmin", "z", "350")),
       zlim = c(-100, 400), phi = 20, image = list(side = 350))

## =======================================================================
## Use of inttype
## =======================================================================
library(rgl)
library(plot3D)
par(mfrow = c(2, 2))
persp3D(z = Volcano, shade = 0.5, colkey = FALSE)
persp3D(z = Volcano, inttype = 2, shade = 0.5, colkey = FALSE)

x <- y <- seq(0, 2 * pi, length.out = 10)
z <- with(mesh(x, y), cos(x) * sin(y)) + runif(100)
cv <- matrix(nrow = 10, 0.5 * runif(100))
persp3D(x, y, z, colvar = cv) # takes averages of z
persp3D(x, y, z, colvar = cv, inttype = 2) # takes averages of colvar

## =======================================================================
## Use of inttype with NAs
## =======================================================================
library(rgl)
library(plot3D)
par(mfrow = c(2, 2))
VV <- V2 <- volcano[10:15, 10:15]
V2[3:4, 3:4] <- NA
V2[4, 5] <- NA

image2D(V2, border = "black") # shows true NA region

# averages of V2, including NAs, NA region larger
persp3D(z = VV, colvar = V2, inttype = 1, theta = 0,
       phi = 20, border = "black", main = "inttype = 1")

# extension of VV; NAs unaffected
persp3D(z = VV, colvar = V2, inttype = 2, theta = 0,
       phi = 20, border = "black", main = "inttype = 2")

# average of V2, ignoring NA; NA region smaller
persp3D(z = VV, colvar = V2, inttype = 3, theta = 0,
       phi = 20, border = "black", main = "inttype = 3")

## =======================================================================
## Use of panel.first
## =======================================================================
library(rgl)
library(plot3D)
par(mfrow = c(1, 1))

# A function that is called after the axes were drawn
panelfirst <- function(trans) {
    zticks <- seq(100, 180, by = 20)
    len <- length(zticks)
    XY0 <- trans3D(x = rep(1, len), y = rep(1, len), z = zticks,
                   pmat = trans)
    XY1 <- trans3D(x = rep(1, len), y = rep(61, len), z = zticks,
                   pmat = trans)
    segments(XY0$x, XY0$y, XY1$x, XY1$y, lty = 2)

    rm <- rowMeans(volcano)
    XY <- trans3D(x = 1:87, y = rep(ncol(volcano), 87),
                  z = rm, pmat = trans)
    lines(XY, col = "blue", lwd = 2)
}
persp3D(z = volcano, x = 1:87, y = 1:61, scale = FALSE, theta = 10,
       expand = 0.2, panel.first = panelfirst, colkey = FALSE)

## =======================================================================
## with / without colvar / facets 
## =======================================================================
library(rgl)
library(plot3D)
par(mfrow = c(2, 2))
persp3D(z = volcano, shade = 0.3, col = gg.col(100))

# shiny colors - set lphi for more brightness
persp3D(z = volcano, lighting = TRUE, lphi = 90)

persp3D(z = volcano, col = "lightblue", colvar = NULL,
   shade = 0.3, bty = "b2")

# this also works:
#  persp3D(z = volcano, col = "grey", shade = 0.3)

# tilted x- and y-coordinates of 'volcano'
volcx <- matrix(nrow = 87, ncol = 61, data = 1:87)
volcx <- volcx + matrix(nrow = 87, ncol = 61,
        byrow = TRUE, data = seq(0., 15, length.out = 61))

volcy <- matrix(ncol = 87, nrow = 61, data = 1:61)
volcy <- t(volcy + matrix(ncol = 87, nrow = 61,
        byrow = TRUE, data = seq(0., 15, length.out = 87)))

persp3D(volcano, x = volcx, y = volcy, phi = 80)

## =======================================================================
## Several persps on one plot
## =======================================================================
library(rgl)
library(plot3D)
par(mfrow = c(1, 1))
clim <- range(volcano)
persp3D(z = volcano, zlim = c(100, 600), clim = clim,
   box = FALSE, plot = FALSE)

persp3D(z = volcano + 200, clim = clim, colvar = volcano,
       add = TRUE, colkey = FALSE, plot = FALSE)

persp3D(z = volcano + 400, clim = clim, colvar = volcano,
       add = TRUE, colkey = FALSE) # plot = TRUE by default

## =======================================================================
## hist3D
## =======================================================================
library(rgl)
library(plot3D)
par(mfrow = c(2, 2))
VV <- volcano[seq(1, 87, 15), seq(1, 61, 15)]
hist3D(z = VV, scale = FALSE, expand = 0.01, border = "black")

# transparent colors
hist3D(z = VV, scale = FALSE, expand = 0.01,
   alpha = 0.5, opaque.top = TRUE, border = "black")

hist3D(z = VV, scale = FALSE, expand = 0.01, facets = FALSE, lwd = 2)

hist3D(z = VV, scale = FALSE, expand = 0.01, facets = NA)

## =======================================================================
## hist3D and ribbon3D with greyish background, rotated, rescaled,...
## =======================================================================
library(rgl)
library(plot3D)
par(mfrow = c(2, 2))
hist3D(z = VV, scale = FALSE, expand = 0.01, bty = "g", phi = 20,
        col = "#0072B2", border = "black", shade = 0.2, ltheta = 90,
        space = 0.3, ticktype = "detailed", d = 2)

# extending the ranges
plotdev(xlim = c(-0.2, 1.2), ylim = c(-0.2, 1.2), theta = 45)

ribbon3D(z = VV, scale = FALSE, expand = 0.01, bty = "g", phi = 20,
        col = "lightblue", border = "black", shade = 0.2, ltheta = 90,
        space = 0.3, ticktype = "detailed", d = 2, curtain = TRUE)

ribbon3D(z = VV, scale = FALSE, expand = 0.01, bty = "g", phi = 20, zlim = c(95, 183),
        col = "lightblue", lighting = TRUE, ltheta = 50, along = "y",
        space = 0.7, ticktype = "detailed", d = 2, curtain = TRUE)

## =======================================================================
## hist3D for a 1-D data set
## =======================================================================
library(rgl)
library(plot3D)
par(mfrow = c(2, 1))
x <- rchisq(1000, df = 4)
hs <- hist(x, breaks = 15)

hist3D(x = hs$mids, y = 1, z = matrix(ncol = 1, data = hs$density),
  bty = "g", ylim = c(0., 2.0), scale = FALSE, expand = 20,
  border = "black", col = "white", shade = 0.3, space = 0.1,
  theta = 20, phi = 20, main = "3-D perspective")


# reset plotting parameters
par(mfrow = pm)
