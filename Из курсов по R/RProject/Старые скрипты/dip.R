
library(data.table)

DrawPars = function(filename) {
    dat = fread(filename, header = FALSE, dec = ",")
    xt = dat[[1]]
    yt = dat[[2]]
    return(list(xt,yt))
}

par(mfrow = c(1, 2))

p = DrawPars("Rect.txt")
px = p[[1]]
py = p[[2]]
plot(px,py,type="l",lty="solid",xlab="x",ylab="y",main="Квадрат c параметром a = 4",col="red",lwd=3)

p = DrawPars("Circle.txt")
px = p[[1]]
py = p[[2]]
plot(px, py, type = "l", lty = "solid", xlab = "x", ylab = "y", main = "Окружность c параметром r = 2", col = "green", lwd = 3)

p = DrawPars("Tria.txt")
px = p[[1]]
py = p[[2]]
plot(px, py, type = "l", lty = "solid", xlab = "x", ylab = "y", main = "Треугольник c параметром a = 2", col = "blue", lwd = 3)

p = DrawPars("Os.txt")
px = p[[1]]
py = p[[2]]
plot(px, py, type = "l", lty = "solid", xlab = "x", ylab = "y", main = "Острие c параметром a = 2", col = "purple", lwd = 3)



library(data.table)
library(ggplot2)
library(reshape2)

files <- c("Monoms 1.txt", "Monoms 2.txt", "Monoms 3.txt", "Monoms 4.txt", "Monoms 5.txt", "Monoms 6.txt")
titles<-c("exp(x/(|x|+1)) на [0;5]","sin(x)cos(2x) на [-2;2]","|x|exp(cos(x)) на [-2;4]","ln(1+|x|)sh(x/2) на [-2;2]","sin(x)-7/(2x+6) на [-1;1]","|x|+ln(0.01+x^2) на [-1;1]")

#split.screen(c(2,1))

#df2 = fread(files[1], header = TRUE, dec = ",")

li = list() #list(ggplot(data = df2), ggplot(data = df2), ggplot(data = df2), ggplot(data = df2), ggplot(data = df2), ggplot(data = df2))

for (i in 1:6) {
    #i=2
df = fread(files[i], header = TRUE, dec = ",")

# data from the example you were referring to, in a 'wide' format.
x <- df[[1]]
ocean <- df[[2]]
soil <- df[[3]]
df <- data.frame(x, ocean, soil)
df2 <- melt(data = df, id.vars = "x")

# plot, using the aesthetics argument 'colour'
#ggplot(data = df2, aes(x = x, y = value, colour = variable)) + geom_line()
   li[[i]] <- ggplot(data = df2, aes(x = x, y = value, colour = variable)) + geom_line(lwd = 1) +
    ylab("Логарифм от точности аппроксимации") + xlab("Число функций") + theme_bw() + ggtitle(titles[i])
    if (i==5) {
  li[[i]]=li[[i]]+ scale_colour_discrete(name = "Метод", labels = c("Только решение СЛАУ методом Гаусса", "Ультра-гибридный метод"))
    }
    else {
        li[[i]] = li[[i]] + scale_colour_discrete(name = "", labels = c("", ""))
    }
    
}

library("cowplot")
plot_grid(li[[1]], li[[2]], li[[3]],
          li[[4]],
          li[[5]],
          #li[[6]],
          #labels = c("A", "B", "C"),
          ncol = 2, nrow = 3)




library(data.table)
library(ggplot2)
library(reshape2)

files <- c("Monoms vs.txt")
titles <- c("cos(2x)(exp(|x|/3)-4ln(1+|x|)) на [-1;5]")

li = list() #list(ggplot(data = df2), ggplot(data = df2), ggplot(data = df2), ggplot(data = df2), ggplot(data = df2), ggplot(data = df2))

    i=1
df = fread(files[i], header = F, dec = ",")

x = df[[1]]
f = df[[2]]
fl = df[[3]]
f1 = df[[4]]
f2 = df[[5]]
f3 = df[[6]]
#f4 = df[[7]]
#f5 = df[[8]]

df=data.frame(x,f,fl,f1,f2,f3)#,f4,f5)
df2 <- melt(data = df, id.vars = "x")

ggplot(data = df2, aes(x = x, y = value, colour = variable)) + geom_line(lwd = 1.2)+# + geom_line(lwd = 1) + geom_line(lwd = 1) + geom_line(lwd = 1) + geom_line(lwd = 1) + geom_line(lwd = 1) +
   ylab("y") + xlab("x") + theme_bw() + ggtitle(titles[i])+
      scale_colour_discrete(name = "", labels = c("Исходная функция", "Гаусс при count = 34", "Ультра-гибрид при count = 9", "Ультра-гибрид при count = 16", "Ультра-гибрид при count = 34"))




#для круга!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
library(data.table)
library(ggplot2)
library(reshape2)

files <- c("eps.txt","epstime.txt")
titles <- c("Точность (меньше - лучше)","Точность x Время вычислений (меньше - лучше)")

df1 = fread(files[1], header = F, dec = ",")
x = df1[[1]]
f1 = df1[[2]]
f2 = df1[[3]]
f3 = df1[[4]]
f4 = df1[[5]]
f5=df1[[6]]


df1 = data.frame(x, f1,f2,f3,f4,f5) #,f4,f5)
df2 <- melt(data = df1, id.vars = "x")

p1 = ggplot(data = df2, aes(x = x, y = value, colour = variable)) + geom_point() + geom_line(lwd = 1.2) + # + geom_line(lwd = 1) + geom_line(lwd = 1) + geom_line(lwd = 1) + geom_line(lwd = 1) + geom_line(lwd = 1) +
    ylab("log10(точность)") + xlab("Число колец") + theme_bw() + ggtitle(titles[1]) +
      scale_colour_discrete(name = "", labels = c("ʃln(1+x^2+y^2)", "ʃsqrt(x^2+y^2)", "ʃxy", "ʃexp(-x^2-y^2)", "ʃ(3x-2y)/(x^2+y^2)"))


df1 = fread(files[2], header = F, dec = ",")
x = df1[[1]]
f1 = df1[[2]]
f2 = df1[[3]]
f3 = df1[[4]]
f4 = df1[[5]]
f5 = df1[[6]]


df1 = data.frame(x, f1, f2, f3, f4, f5) #,f4,f5)
df2 <- melt(data = df1, id.vars = "x")

p2 = ggplot(data = df2, aes(x = x, y = value, colour = variable)) + geom_point() + 
    ylab("log10(точность х время)") + xlab("Число колец") + theme_bw() + ggtitle(titles[2]) +
      scale_colour_discrete(name = "", labels = c("ʃln(1+x^2+y^2)", "ʃsqrt(x^2+y^2)", "ʃxy", "ʃexp(-x^2-y^2)", "ʃ(3x-2y)/(x^2+y^2)"))
#p2


library("cowplot")
plot_grid(p1,p2,
#li[[6]],
#labels = c("A", "B", "C"),
          ncol = 1, nrow = 2)




#для полукруга!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
library(data.table)
library(ggplot2)
library(reshape2)

files <- c("eps.txt", "epstime.txt")
titles <- c("Точность (меньше - лучше)", "Точность x Время вычислений (меньше - лучше)")

df1 = fread(files[1], header = F, dec = ",")
x = df1[[1]]
f1 = df1[[2]]
f2 = df1[[3]]
f3 = df1[[4]]
f4 = df1[[5]]
f5 = df1[[6]]


df1 = data.frame(x, f1, f2, f3, f4, f5) #,f4,f5)
df2 <- melt(data = df1, id.vars = "x")

p1 = ggplot(data = df2, aes(x = x, y = value, colour = variable)) + geom_point() + geom_line(lwd = 1.2) + # + geom_line(lwd = 1) + geom_line(lwd = 1) + geom_line(lwd = 1) + geom_line(lwd = 1) + geom_line(lwd = 1) +
    ylab("log10(точность)") + xlab("Число колец") + theme_bw() + ggtitle(titles[1]) +
      scale_colour_discrete(name = "", labels = c("ʃ1", "ʃxy^2+1", "ʃexp(x^2+y^2-10)", "ʃln(1+sqrt(x^2+y^2))", "ʃx+y"))


df1 = fread(files[2], header = F, dec = ",")
x = df1[[1]]
f1 = df1[[2]]
f2 = df1[[3]]
f3 = df1[[4]]
f4 = df1[[5]]
f5 = df1[[6]]


df1 = data.frame(x, f1, f2, f3, f4, f5) #,f4,f5)
df2 <- melt(data = df1, id.vars = "x")

p2 = ggplot(data = df2, aes(x = x, y = value, colour = variable)) + geom_point() +
    ylab("log10(точность х время)") + xlab("Число колец") + theme_bw() + ggtitle(titles[2]) +
      scale_colour_discrete(name = "", labels = c("ʃ1", "ʃxy^2+1", "ʃexp(x^2+y^2-10)", "ʃln(1+sqrt(x^2+y^2))", "ʃx+y"))
#p2


library("cowplot")
plot_grid(p1, p2,
#li[[6]],
#labels = c("A", "B", "C"),
          ncol = 1, nrow = 2)







#---------проверка оценки
#
#
#
#
library(rgl)
library(plot3D)
library(data.table)

xx = fread("arg.txt", header = F, dec = ",")
z = fread("vals.txt", header = F, dec = ",")
x = xx[[1]] #;xx
y = xx[[2]]


z1 = matrix(z[[1]], nrow = length(x), ncol = length(y), byrow = TRUE)
z2 = matrix(z[[2]], nrow = length(x), ncol = length(y), byrow = TRUE)
z3 = matrix(z[[3]], nrow = length(x), ncol = length(y), byrow = TRUE)
z4 = matrix(z[[4]], nrow = length(x), ncol = length(y), byrow = TRUE)
z5 = matrix(z[[5]], nrow = length(x), ncol = length(y), byrow = TRUE)
z6 = matrix(z[[6]], nrow = length(x), ncol = length(y), byrow = TRUE)

#tit = scan("tmp.txt", what = text())
#tit

urmin = -20
urmax = 20
levels = 15

pdf(file = "valargs.pdf",15)
par(mfrow = c(2, 3))

pp = persp3D(z = z1, x = x, y = y, scale = FALSE, zlab = "z",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(urmin, max(urRe, na.rm = TRUE) * 0.3),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "k = 3")

persp3D(z = z2, x = x, y = y, scale = FALSE, zlab = "z",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(-40, max(uzRe, na.rm = TRUE) * 0.7),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "k = 5")

persp3D(z = z3, x = x, y = y, scale = FALSE, zlab = "z",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(-20, max(urIm, na.rm = TRUE) * 0.3),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "k = 25")

persp3D(z = z4, x = x, y = y, scale = FALSE, zlab = "z",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(-40, max(uzIm, na.rm = TRUE) * 0.7),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "k = 50")

persp3D(z = z5, x = x, y = y, scale = FALSE, zlab = "z",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(-20, max(urAbs, na.rm = TRUE) * 0.3),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "k = 200")

persp3D(z =z6, x = x, y = y, scale = FALSE, zlab = "z",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(-40, max(uzAbs, na.rm = TRUE) * 0.7),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "k = 500")

dev.off()



#---------Сравнение аппроксимаций
#
#
#
#
library(rgl)
library(plot3D)
library(data.table)

xx = fread("(arg).txt", header = F, dec = ",")
z = fread("(vals).txt", header = F, dec = ",")
x = xx[[1]] #;xx
y = xx[[2]]


z4 = matrix(z[[1]], nrow = length(x), ncol = length(y), byrow = TRUE)
z5 = matrix(z[[2]], nrow = length(x), ncol = length(y), byrow = TRUE)
z6 =abs( z4-z5)
#tit = scan("tmp.txt", what = text())
#tit

urmin = -20
urmax = 20
levels = 15


dt = readLines("info.txt")

pdf(file = dt[[1]],height = 12,paper = "letter" )
#par(mfrow = c(3, 1))

split.screen(c(2, 1))
split.screen(c(1, 2), screen = 1)
screen(3)
persp3D(z = z4, x = x, y = y, scale = FALSE, zlab = "z",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(-40, max(uzIm, na.rm = TRUE) * 0.7),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "Density")

screen(4)
persp3D(z = z5, x = x, y = y, scale = FALSE, zlab = "z",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(-20, max(urAbs, na.rm = TRUE) * 0.3),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "Density approx.")

screen(2)
persp3D(z = z6, x = x, y = y, scale = FALSE, zlab = "z",
       contour = list(nlevels = levels, col = "red"),
#zlim = c(-40, max(uzAbs, na.rm = TRUE) * 0.7),
        expand = 0.2,
       image = list(col = grey(seq(0, 1, length.out = 100))), main = "Difference")

close.screen(all = TRUE)

dev.off()

library(plotly)
p1 = plot_ly(x = x, y = y, z = z6, type = "surface", contours = list(
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

saveWidget(as.widget(p1), dt[[2]], FALSE)


#устойчивость аппроксимации новым методом
library(data.table)
library(ggplot2)
library(reshape2)

cir = readLines("Circles.txt")
fir=readLines("Functions.txt")

files=list()

for (i in 1:length(fir)) {
    files[[i]]=paste0("C=",cir[[3]], " f=", fir[[i]], ".txt")
}


titles <- c("Решение бигармонической задачи", "Решение задачи оптимизации")


df1 = fread(files[[1]], header = F, dec = ",")
x = df1[[1]]
f1 = list()
f2 = list()

for (i in 1:length(files)) {
    df1 = fread(files[[i]], header = F, dec = ",")
    f2[[i]] = log10(df1[[2]]) #*df1[[4]])
    f1[[i]] = log10(df1[[3]]) #*df1[[4]])
}

df1 = data.frame(x, f1) #,f4,f5)
df2 <- melt(data = df1, id.vars = "x")

ystring = "log10(точность)"

p1 = ggplot(data = df2, aes(x = x, y = value, colour = variable)) + geom_point(size = 3) + geom_line(lwd = 1.0, linetype = "dashed") + # + geom_line(lwd = 1) + geom_line(lwd = 1) + geom_line(lwd = 1) + geom_line(lwd = 1) + geom_line(lwd = 1) +
    ylab(ystring) + xlab("Число функций") + theme_bw() + ggtitle(titles[1]) +
      scale_colour_discrete(name = "", labels = as.vector(fir))


df1 = data.frame(x, f2) #,f4,f5)
df2 <- melt(data = df1, id.vars = "x")

p2 = ggplot(data = df2, aes(x = x, y = value, colour = variable)) + geom_point(size = 3) + geom_line(lwd = 1.0, linetype = "dashed") + # + geom_line(lwd = 1) + geom_line(lwd = 1) + geom_line(lwd = 1) + geom_line(lwd = 1) + geom_line(lwd = 1) +
    ylab(ystring) + xlab("Число функций") + theme_bw() + ggtitle(titles[2]) +
      scale_colour_discrete(name = "", labels = as.vector(fir))
#p2


library("cowplot")
plot_grid(p1, p2,
#li[[6]],
#labels = c("A", "B", "C"),
          ncol = 1, nrow = 2)


#для примера Рунге
d = read.table("Runge.txt", dec = ",")
n = d[[1]]
eps1 = log10(d[[2]])
eps2 = log10(d[[3]])
plot(n, eps1, type = "b", lty = 3, lwd = 3, pch = 16, col = "green", ann = FALSE,ylim=c(-1,4.5))
lines(n, eps2, type = "b", lty = 3, lwd = 3, pch = 16, col = "red", ann = FALSE)
title(main = "Погрешность аппроксимации в зависимости от числа наблюдений", xlab = "Число наблюдений", ylab = "log10 от погрешности")

legend("topleft",inset = 0.05,title = "Норма",c("Равномерная","Среднеквадратичная"),col = c("green","red"),lty = c(2,2),pch = c(16,16))
