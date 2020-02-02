library(data.table)

w = getwd()

d = read.table(paste0(w, "/OnePoint.txt"), dec = ",", header = TRUE)
s = readLines(paste0(w, "/OnePoint(info).txt"))

n = d[[1]]
eps1 = d[[2]] #ur
eps2 = d[[3]] #uz
title_main = s[1]
print(title_main)

limss = range(eps1, eps2)

len = (length(s) - 1) / 3 #count of sources

#draw u(x,t)
if (as.logical(readLines("MakeUxtByEvery.txt")[1])) {
    pdf(file = paste0(title_main, ".pdf"), width = 18, height = 22, pointsize = 20)
    layout(mat = matrix(c(1, 2, 3, 4), 4, 1), heights = c(1, 1, 1, 1))

    for (i in 1:len) {
        print(paste0("subgrafic ", i))
        d = read.table(paste0(w, "/", s[2 * len + 1 + i]), dec = ",", header = TRUE)
        e1 = d[[2]]
        e2 = d[[3]]
        lis = range(e1, e2)

        plot(n, e1, type = "l", lty = 1, lwd = 3, pch = 16, col = "green", ann = FALSE, ylim = lis)
        lines(n, e2, type = "l", lty = 1, lwd = 3, pch = 16, col = "red", ann = FALSE)
        title(main = paste0("Functions ur(x,t), uz(x,t) ", s[1 + i]), xlab = "t", ylab = "u")

        legend("topleft", inset = 0.05, title = "Function", c("ur", "uz"), col = c("green", "red"), lty = c(1, 1), pch = c(16, 16))
    }
} else {
    pdf(file = paste0(title_main, ".pdf"), width = 20)
}

#draw u(x,t) sum
plot(n, eps1, type = "l", lty = 30, lwd = 3, pch = 16, col = "green", ann = FALSE, ylim = limss)
lines(n, eps2, type = "l", lty = 30, lwd = 3, pch = 16, col = "red", ann = FALSE)
title(main = "Functions ur(x,t), uz(x,t) (sum)", xlab = "t", ylab = "u")

legend("topleft", inset = 0.05, title = "Function", c("ur", "uz"), col = c("green", "red"), lty = c(2, 2), pch = c(16, 16))

dev.off()


#draw other info
for (i in 2:(1 + len)) {
    cat(i - 1)

    pdf(file = paste0("center = ", s[i], "; ", title_main, ".pdf"), paper = "letter", width = 30, height = 25)
    layout(mat = matrix(c(1, 2, 3), 3, 1, byrow = TRUE), heights = c(1.7, 1, 1))

    d = read.table(paste0(w, "/f(w) from ", s[i], ".txt"), dec = ",", header = TRUE)
    n = d[[1]]

    eps1 = d[[2]]
    eps2 = d[[3]]
    eps3 = sqrt(eps1 ^ 2 + eps2 ^ 2)
    limss = range(eps1, eps2, eps3)

    plot(n, eps1, type = "l", lty = 6, lwd = 1, pch = 16, col = "green", ann = FALSE, ylim = limss)
    lines(n, eps2, type = "l", lty = 6, lwd = 1, pch = 16, col = "blue", ann = FALSE)
    lines(n, eps3, type = "l", lty = 1, lwd = 1, pch = 16, col = "red", ann = FALSE)
    title(main = "Input data f(w)", xlab = "w", ylab = "f(w)")

    legend("topright", inset = 0.05, title = "Function", c("Re", "Im", "Abs"), col = c("green", "blue", "red"), lty = c(6, 6, 1), pch = c(16, 16, 16))

    d = read.table(paste0(w, "/", s[i + len]), dec = ",", header = TRUE)
    n = d[[1]]
    eps1 = d[[2]]
    eps2 = d[[3]]
    eps3 = d[[4]]
    eps4 = d[[5]]

    eps_1 = sqrt(eps1 ^ 2 + eps2 ^ 2)
    eps_3 = sqrt(eps3 ^ 2 + eps4 ^ 2)

    limss = range(eps_1, eps_3)

    plot(n, eps_1, type = "l", lty = 1, lwd = 2, pch = 1, col = "green", ann = FALSE, ylim = limss)
    lines(n, eps_3, type = "l", lty = 1, lwd = 2, pch = 1, col = "blue", ann = FALSE)

    title(main = "Functions |u(x,w)|", xlab = "w", ylab = "u")

    legend("topright", inset = 0.05, title = "Function", c("|ur|", "|uz|"), col = c("green", "blue"), lty = c(1, 1, 1), pch = c(1, 1, 1))

    dt = rbind(eps1, eps2, eps3, eps4)
    limss = c(min(dt), max(dt))

    plot(n, eps1, type = "l", lty = 1, lwd = 1, pch = 1, col = "green", ann = FALSE, ylim = limss)
    lines(n, eps2, type = "l", lty = 2, lwd = 0.7, pch = 2, col = "red", ann = FALSE)
    lines(n, eps3, type = "l", lty = 1, lwd = 1, pch = 1, col = "blue", ann = FALSE)
    lines(n, eps4, type = "l", lty = 2, lwd = 0.7, pch = 2, col = "black", ann = FALSE)

    title(main = "Functions u(x,w)", xlab = "w", ylab = "u")

    legend("bottomright", inset = 0.05, title = "Function", c("Re ur", "Im ur", "Re uz", "Im uz"), col = c("green", "red", "blue", "violet"), lty = c(1, 2, 1, 2), pch = c(1, 2, 1, 2))

    dev.off()
}
