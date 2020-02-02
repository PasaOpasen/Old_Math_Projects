library(rgl)
library(plot3D)
library(data.table)
library(ggplot2)
library(viridis)
library(gridExtra)
library(fields)
library(foreach)


w = getwd()
print(w)
source(paste0(w, "/Truezlims.r"), echo = FALSE, print.eval = FALSE)

    zl = fread("zlims(real).txt", header = TRUE, dec = ".")
    rlim = c(zl[[1]][1], zl[[1]][2]) * 1.01
    zlim = c(zl[[2]][1], zl[[2]][2]) * 1.01
    levels = 30


    stt = readLines("textnames.txt")
    xx = fread("3D ur, uz(x).txt", header = TRUE, dec = ",")
    yy = fread("3D ur, uz(y).txt", header = TRUE, dec = ",")
    
    x = xx$x
    y = yy$y
    len = length(x)

    zm = max(abs(rlim))
    x1 = x / (max(x) - min(x)) * zm
    y1 = y / (max(y) - min(y)) * zm

    zm = max(abs(zlim))
    x2 = x / (max(x) - min(x)) * zm
    y2 = y / (max(y) - min(y)) * zm

bool=as.logical(readLines("MakePDFs.txt")[1])

anime <- function(i) {
    s = stt[[i]]
    s1 = sub(".txt", " (ur).txt", s, fixed = TRUE)
    s2 = sub(".txt", " (uz).txt", s, fixed = TRUE)
    ur = scan(s1)
    uz = scan(s2)

    ur = matrix(ur, nrow = length(x), ncol = length(y), byrow = FALSE)
    uz = matrix(uz, nrow = length(x), ncol = length(y), byrow = FALSE)

  
    s = sub(".txt", "", s)
    cat(paste(s, "\n"))
    ss = sub(", t =", ", \n t =", s)
    ss = strsplit(ss, "\n")
    s1 = ss[[1]][1]
    s2 = ss[[1]][2]


    if (bool) {
        pdf(file = paste("3D ur, uz(title ,", s, ").pdf"), width = 26)
        par(mfrow = c(1, 2), cex = 1.0, cex.sub = 1.3, col.sub = "blue")

        pp = persp3D(z = ur, x = x1, y = y1, scale = FALSE, zlab = "ur(x,t)",
       contour = list(nlevels = levels, col = "red"),
    #zlim = c(urmin, max(urRe, na.rm = TRUE) * 0.3),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "ur(x,t)", sub = s1, zlim = rlim)

        pp2 = persp3D(z = uz, x = x2, y = y2, scale = FALSE, zlab = "uz(x,t)",
       contour = list(nlevels = levels, col = "red"),
    #zlim = c(-40, max(uzRe, na.rm = TRUE) * 0.7),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "uz(x,t)", sub = s2, zlim = zlim)

        dev.off()
    }


    png(filename = paste("3D", s, ".png"), width = 1080, height = 810, units = "px", res = NA)
    par(mfrow = c(2, 2), cex = 1.0, cex.sub = 1.3, col.sub = "blue")
    layout(matrix(c(1, 2, 3, 4), 2, 2, byrow = FALSE), widths = c(1.3, 1))

    pp = persp3D(z = ur, x = x1, y = y1, scale = FALSE, zlab = "ur(x,t)",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(urmin, max(urRe, na.rm = TRUE) * 0.3),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "ur(x,t)", sub = s1, zlim = rlim, clim = rlim)

    persp3D(z = uz, x = x2, y = y2, scale = FALSE, zlab = "uz(x,t)",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(-40, max(uzRe, na.rm = TRUE) * 0.7),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "uz(x,t)", sub = s2, zlim = zlim, clim = zlim)
    image(x, y, abs(ur), col = heat.colors(levels), main = "|ur|")
    image(x, y, abs(uz), col = topo.colors(levels), main = "|uz|")

    dev.off()

}


clasters = as.numeric(readLines("ClastersCount.txt")[1])

if (clasters == 1) {
    for (i in 1:(length(stt))) {

        anime(i)
        cat(paste(i, "files from", length(stt), "are complited", "\n"))
    }
} else {
    library(doParallel) # Загружаем библиотеку в память
    cl <- makeCluster(clasters) # Создаем «кластер» на x потоков
    registerDoParallel(cl) # Регистрируем «кластер»

    cat(paste("Clusters selected:", clasters, "\n"))
    cat(paste("The window closes after execution...", "\n"))

    step = length(stt)%/%clasters

    for (k in 1:step) {
        foreach(i = ((k-1) * clasters+1):(k * clasters+1), .packages = c("rgl", "plot3D", "data.table", "gridExtra", "fields", "viridis")) %dopar% {
            anime(i)         
        }
        cat(paste0( round( (k * clasters)/length(stt)*100,digits = 3), "%", "\n"))
    }

    foreach(i = (step * clasters+1):length(stt), .packages = c("rgl", "plot3D", "data.table", "gridExtra", "fields", "viridis")) %dopar% {
        anime(i)
    }
    #foreach(i = 1:(length(stt)), .packages = c("rgl", "plot3D", "data.table", "gridExtra", "fields", "viridis")) %dopar% {
      #  anime(i)
    
}



