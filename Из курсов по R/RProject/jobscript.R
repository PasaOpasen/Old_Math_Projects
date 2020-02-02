#äëÿ ãðàôèêîâ ur, uz â ñðàâíåíèè èíòåãðàëîâ è âû÷åòîâ

library(data.table)
library(ggplot2)
library(reshape2)

files <- c("uruz.txt")
titles <- c("uz(x,y,0)","ur(x,y,0)")

df = fread(files, header = F, dec = ",")
df2 = fread(files, header = F, dec = ",")
df3 = fread(files, header = F, dec = ",")
df4 = fread(files, header = F, dec = ",")

x = df[[1]]

#df = data.frame(x, df[[4+1]], df[[5+1]], df[[10+1]], df[[11+1]]) #,f4,f5)
#df2 <- melt(data = df, id.vars = "x")
df = data.frame(x, df[[4 + 1]], df[[5 + 1]])
df2 = data.frame(x, df2[[10 + 1]], df2[[11 + 1]])
df3 = data.frame(x, df3[[1 + 1]], df3[[2 + 1]])
df4 = data.frame(x, df4[[7 + 1]], df4[[8 + 1]])


d3 <- melt(data = df, id.vars = "x")
d4 <- melt(data = df2, id.vars = "x")
d1 <- melt(data = df3, id.vars = "x")
d2 <- melt(data = df4, id.vars = "x")

#ggplot(data = df2, aes(x = x, y = value, colour = variable)) + geom_line(lwd = 1.2, aes(linetype = variable)) + # + geom_line(lwd = 1) + geom_line(lwd = 1) + geom_line(lwd = 1) + geom_line(lwd = 1) + geom_line(lwd = 1) +
   # ylab("êîìïîíåíòû uz") + xlab("îòðåçîê íà ëó÷å îò ãðàíèöû ïüåçîýëåìåíòà") + theme_bw() + ggtitle(titles[1]) +
     # scale_colour_discrete(name = "", labels = c("Re uz", "Im uz", "Re uz (res)", "Im uz (res)"))

ggplot(data = d3, aes(x = x, y = value, colour = variable)) + geom_line(lwd = 1.2) +
    geom_line(data = d4, aes(x = x, y = value, colour = variable), lwd = 1.2, linetype = "dashed") +
    ylab("êîìïîíåíòû uz") + xlab("îòðåçîê íà ëó÷å îò ãðàíèöû ïüåçîýëåìåíòà") + theme_bw() + ggtitle(titles[1]) +
      scale_colour_discrete(name = "", labels = c("Re uz", "Im uz", "Re uz (res)", "Im uz (res)")) + theme(legend.text = element_text(size = 16), axis.text = element_text(size = 14), axis.title = element_text(size = 14))

ggplot(data = d1, aes(x = x, y = value, colour = variable)) + geom_line(lwd = 1.2) +
    geom_line(data = d2, aes(x = x, y = value, colour = variable), lwd = 1.2, linetype = "dashed") +
    ylab("êîìïîíåíòû ur") + xlab("îòðåçîê íà ëó÷å îò ãðàíèöû ïüåçîýëåìåíòà") + theme_bw() + ggtitle(titles[2]) +
      scale_colour_discrete(name = "", labels = c("Re ur", "Im ur", "Re ur (res)", "Im ur (res)")) + theme(legend.text = element_text(size = 16), axis.text = element_text(size = 14), axis.title = element_text(size = 14))


#â òî÷êå ïðè ìåíÿþùåéñÿ ÷àñòîòå
library(data.table)
library(ggplot2)
library(reshape2)

files <- c("uruzw.txt")
titles <- c("Im ur, Re uz, Im uz")

df = fread(files, header = F, dec = ",")
df2 = fread(files, header = F, dec = ",")

x = df[[1]]
for (i in 2:13) {
    df[[i]][abs(df[[i]]) >0.01] = NA
    df2[[i]][abs(df2[[i]]) > 0.01] = NA
}

df = data.frame(x,df[[2+1]], df[[4 + 1]], df[[5 + 1]])
df2 = data.frame(x,df2[[8+1]],df2[[10 + 1]], df2[[11 + 1]] )

d3 <- melt(data = df, id.vars = "x")
d4 <- melt(data = df2, id.vars = "x")

ggplot(data = d3, aes(x = x, y = value, colour = variable)) + geom_line(lwd = 1.2) +
    geom_line(data = d4, aes(x = x, y = value, colour = variable), lwd = 1.6, linetype = "longdash") +
    ylab("êîìïîíåíòû ur, uz") + xlab("omega") + theme_bw() + ggtitle(titles[1]) +
      scale_colour_discrete(name = "", labels = c("Im ur","Re uz", "Im uz", "Re uz (res)","Im uz (res)", "Im ur (res)")) + theme(legend.text = element_text(size = 16), axis.text = element_text(size = 14), axis.title = element_text(size = 14))


#Hann window
library(data.table)
fc = 1e5
pfc = pi * fc
n = c(2, 3, 5, 8)
par(mfrow = c(2, 2))
for (i in seq_along(n)) {
    x = seq(0, n[i] / fc, length.out= 100)
    f = 10 * sin(2 * pfc * x) * sin(pfc * x/ n[i]) ^ 2
    plot(x, f, type = "l", lty = "solid", xlab = "time, 100usec", ylab = "voltage 1A", main = paste("Hann window, n =",n[i]), col = "red", lwd = 3)
}







#test by 23.04
library(data.table)
library(ggplot2)
library(reshape2)

files <- c("spec.txt")
titles <- c("угол = 0","угол = pi/2", "угол = -pi", "угол = -pi/2")

df = fread(files, header = F, dec = ",")
pi0 = fread("spectrum0.dat", header = F, dec = ".")
pi2p = fread("spectrum+pi-2.dat", header = F, dec = ".")
pipm = fread("spectrum+pi.dat", header = F, dec = ".")
pi2m = fread("spectrum-pi-2.dat", header = F, dec = ".")

listfiles=list(pi0,pi2p,pipm,pi2m)
li = list()

for (i in 1:4)
    {
    df = fread(files, header = F, dec = ",")
x1 = df[[2]]
#x3 = pi0[[1]]
x3=listfiles[[i]][[1]]
y1 = df[[3+i-1]]
y2 = df[[7 + i - 1]]
    y3 =listfiles[[i]][[2]]
#y3=pi0[[2]]

y1=y1/max(y1)
y2 = y2 / max(y2)
y3 = y3 / max(y3)

#for (i in 2:13) {
    #df[[i]][abs(df[[i]]) > 0.01] = NA
    #df2[[i]][abs(df2[[i]]) > 0.01] = NA
#}

df = data.frame(x1,y1,y2)
df2 = data.frame(x3,y3)

d3 <- melt(data = df, id.vars = "x1")
d4 <- melt(data = df2, id.vars = "x3")

    li[[i]] = ggplot(data = d3, aes(x = x1, y = value, colour = variable)) + geom_line(lwd = 1.2, linetype = "longdash") +
    geom_line(data = d4, aes(x = x3, y = value, colour = variable), lwd = 1.6, linetype = "solid") +
    ylab("|v|") + xlab("f, КГц") + theme_bw() + ggtitle(titles[i]) +
     scale_colour_discrete(name = "", labels = c("circle", "dcircle", "data"))+
    theme(legend.text = element_text(size = 16), axis.text = element_text(size = 14), axis.title = element_text(size = 14))

}

library("cowplot")
plot_grid(li[[1]], li[[2]], li[[3]],
          li[[4]],
          ncol = 2, nrow = 2)






#vg
s = read.table("Vg.txt", dec = ",")
x = s[[1]]
y = s[[2]]
plot(x, y, "b", col = "red")