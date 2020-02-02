x <- 1:100
y <- 1:100
z <- x %o% y
z <- z*sin(z) + - .1 * z



urt <- data.frame(ur.abs = c(abs(z$ur)), x = rep(x, len), y = rep(y, each = len))
p1 = ggplot(urt, aes(x, y, fill = ur.abs)) +
    geom_raster(interpolate = TRUE) +
    coord_fixed(expand = FALSE) +
    scale_fill_viridis()

urz <- data.frame(uz.abs = c(abs(z$uz)), x = rep(x, len), y = rep(y, each = len))
p2 = ggplot(urz, aes(x, y, fill = uz.abs)) +
    geom_raster(interpolate = TRUE) +
    coord_fixed(expand = FALSE) +
    scale_fill_viridis()

grid.arrange(p1, p2, newpage = FALSE)


image.plot(x, y, z = abs(ur),
           ylab = "y", xlab = "x", maintainer = "|ur|", zlim = c(0, max(abs(rlim))))



###############################################
library(data.table)

d = read.table("ufomega.dat", dec = ".", header = FALSE)
w = d[[1]]
uxr = d[[2]]
uxi = d[[3]]
uxa = d[[4]]
uzr = d[[5]]
uzi = d[[6]]
uza = d[[7]]

pdf( "ufomega.pdf",width = 20)
par(mfrow = c(1, 2))

df=rbind(uxa,uza)

plot(w, uxa, type = "l", lty = 30, lwd = 3, pch = 16, col = "green", ann = FALSE, ylim = c(min(df),max(df)))
lines(w, uza, type = "l", lty = 30, lwd = 3, pch = 16, col = "red", ann = FALSE)
title(main = "Functions |ux|, |uz|", xlab = "w", ylab = "u")

legend("topright", inset = 0.05, title = "Function", c("|ux|", "|uz|"), col = c("green", "red"), lty = c(2, 2), pch = c(16, 16))


df=rbind(uxr,uxi,uzr,uzi)
plot(w, uxr, type = "l", lty = 1, lwd = 1, pch = 16, col = "green", ann = FALSE, ylim = c(min(df), max(df)))
lines(w, uxi, type = "l", lty = 2, lwd = 0.7, pch = 16, col = "red", ann = FALSE)
lines(w, uzr, type = "l", lty = 1, lwd = 1, pch = 16, col = "blue", ann = FALSE)
lines(w, uzi, type = "l", lty = 2, lwd = 0.7, pch = 16, col = "black", ann = FALSE)
title(main = "Functions ux, ux", xlab = "w", ylab = "u")

legend("topright", inset = 0.05, title = "Function", c("Re ux", " Im ux","Re uz","Im uz"), col = c("green", "red","blue","black"), lty = c(1, 2,1,2), pch = c(1, 2,1,2))


dev.off()
