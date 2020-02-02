library(data.table)

w = getwd()

eps1 = read.table(paste0(w, "/ArrayA.txt"), dec = ".", header = FALSE)[[1]]
eps2 = read.table(paste0(w, "/ArrayB.txt"), dec = ".", header = FALSE)[[1]]
eps3 = read.table(paste0(w, "/ArrayC.txt"), dec = ".", header = FALSE)[[1]]
eps4 = read.table(paste0(w, "/ArrayD.txt"), dec = ".", header = FALSE)[[1]]


tb = cbind(eps1, eps2,eps3,eps4)
limss = c(min(tb), max(tb))

arr = c("ArrayA", "ArrayB", "ArrayC", "ArrayD")

pdf(file = paste0("ArrayABCD", ".pdf"),width = 12)

plot(eps1, type = "l", lty = 30, lwd = 0.5, pch = 16, col = "green", ann = FALSE, ylim = limss)
lines(eps2, type = "l", lty = 30, lwd = 0.5, pch = 16, col = "red", ann = FALSE)
lines(eps3, type = "l", lty = 30, lwd = 0.5, pch = 16, col = "blue", ann = FALSE)
lines(eps4, type = "l", lty = 30, lwd = 0.5, pch = 16, col = "black", ann = FALSE)
title(main = "Arrays A, B, C, D", xlab = "n", ylab = "arrays")

legend("bottomright", inset = 0.05, title = "Arrays", c("A", "B","C","D"), col = c("green", "red","blue","black"), lty = c(2, 2,2,2), pch = c(16, 16,16,16))

dev.off()