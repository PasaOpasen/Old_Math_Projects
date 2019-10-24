str(AirPassengers) # структура данных

vec = as.vector(AirPassengers)
good_months = c()
for (i in 2:length(vec)) {
    if (vec[i] > vec[i - 1]) {
        good_months = c(good_months, vec[i])
    }
}
good_months



vec = as.vector(AirPassengers)
moving_average <- numeric(135)
for (i in 1:135) {
    moving_average[i]=mean(vec[i:(i+9)])
}

print(moving_average)


descriptions_stat = aggregate(cbind(hp, disp) ~ am, mtcars, sd)
print(descriptions_stat)



qv = subset(airquality,Month %in% c(7, 8, 9))

result = aggregate(Ozone ~ Month, qv, FUN = length)
print(result)


library(psych)
tmp = describeBy(airquality[, c(1, 2, 3, 4)], group = airquality$Month)
print(tmp)

str(iris)
print(describeBy(iris,group = iris$Species)$virginica)


my_vector <- rnorm(17)
my_vector[sample(1:17, 10)] <- NA # на десять случайных позиций поместим NA

m = mean(my_vector, na.rm = T)
fixed_vector = my_vector
fixed_vector[is.na(fixed_vector)] = m
print(my_vector)
print(fixed_vector)


dimnames(HairEyeColor)
tmp = HairEyeColor[,, 1]
red_men <- prop.table(tmp,2)[3,2]

print(red_men)

ch = sum(HairEyeColor[, "Green", 2])
print(ch)




library("ggplot2")
mydata <- as.data.frame(HairEyeColor[,,"Female"])
print(mydata)

obj <- ggplot(data = mydata, aes(x = Hair, y = Freq, fill = Eye)) +
    geom_bar(stat = "identity", position = position_dodge()) +
    scale_fill_manual(values = c("Brown", "Blue", "Darkgrey", "Darkgreen"))
obj



dat = HairEyeColor["Brown",, "Female"]
print(dat)

chisq.test(dat)


library("ggplot2")

main_stat =chisq.test(x = diamonds$cut, y = diamonds$color)[[1]]

print(main_stat)


library("ggplot2")
d = diamonds
price.mean = mean(d$price)
carat.mean = mean(d$carat)
d$price = factor(ifelse(d$price >= price.mean, 1, 0))
d$carat = factor(ifelse(d$carat >= carat.mean, 1, 0))
main_stat = chisq.test(x = d$price, y = d$carat)[[1]]
print(main_stat)



fisher_test = fisher.test(mtcars$am, mtcars$vs)[[1]]
print(fisher_test)

s = ToothGrowth
t_stat=t.test(s$len[s$supp == "OJ" & s$dose == 0.5], s$len[s$supp == "VC" & s$dose == 2])$statistic

str(t.test(s$len[s$supp == "OJ" & s$dose == 0.5], s$len[s$supp == "VC" & s$dose == 2]))



df <- read.csv(url('https://stepic.org/media/attachments/lesson/11504/lekarstva.csv'))
t.test(df$Pressure_before, df$Pressure_after, paired = T)



df = read.table("dataset_11504_15 (1).txt", dec = ".")
df[[2]] = factor(df[[2]])
df=data.frame(df)
print(df)

bartlett.test(df[[1]] ~ df[[2]])
wilcox.test(df[[1]] ~ df[[2]])



df = read.table("dataset_11504_16 (1).txt", dec = ".")
df = data.frame(df)
print(df)

t.test(df[[1]], df[[2]], var.equal = F)



g = npk
t = aov(yield ~ N * P, g)
summary(t)

g = npk
t = aov(yield ~ N + P+K, g)
summary(t)

g = iris
t = aov(Sepal.Width ~ Species, g)
TukeyHSD(t)




df <- read.csv(url('https://stepic.org/media/attachments/lesson/11505/Pillulkin.csv'))
df$patient = factor(df$patient)
df$doctor = factor(df$doctor)
an = aov(temperature ~ pill*doctor ++Error(patient/(doctor+pill)),df)
summary(an)


library(Hmisc)
library(ggplot2)
obj <- ggplot(ToothGrowth, aes(x = as.factor(dose), y = len,group=supp, col = supp)) +
    stat_summary(fun.data = mean_cl_boot, geom = 'errorbar', width = 0.1, position = position_dodge(0.2)) +
    stat_summary(fun.data = mean_cl_boot, geom = 'point', size = 3, position = position_dodge(0.2)) +
    stat_summary(fun.data = mean_cl_boot, geom = 'line', position = position_dodge(0.2))
obj



NA.position <- function(x) {
    t = is.na(x)
    p=1:length(x)
    return(p[t])
}
NA.counter <- function(x) {
    # put your code here  
    t = is.na(x)
    p = 1:length(x)
    return(length(p[t]))
}
v = c(1, 2, 3, NA, 5, 4, 3)
NA.position(v)


filtered.sum <- function(x) {
   return(sum(x[!is.na(x)&x>0]) ) 
}


outliers.rm <- function(x) {
   q = IQR(x)*1.5
    ran = quantile(x, probs = c(0.25, 0.75))
    return(x[(x>ran[1]-q)&(x<ran[2]+q)])

}




corr.calc <- function(x) {
    t = cor.test(x = x[[1]], y = x[[2]])
    return(c(t$estimate, t$p.value))

}

corr.calc(iris[, 1:2])



filtered.cor <- function(x) {
    df = x[, sapply(x, is.numeric)]
    mat=cor(df)
    diag(mat) <- 0

    mx = max(abs(mat))
    if (length(which(as.array( mat) %in% mx)) > 0) {
        return(mx)
    }
    return(-mx)
}

v = c(1, 2, 3, 4)
which(v %in% 8)


test_data <- as.data.frame(list(V3 = c(-0.4, 1.7, 0, -1, 0.5, -0.1, 0.4, 0.4), V2 = c(-0.4, 1.7, 0, -1, 0.5, -0.1, 0.4, 0.4), V1 = c(-0.5, 1.4, -0.1, 0.1, -0.3, -1, 0.5, 1.6), V5 = c("t", "t", "t", "t", "t", "t", "t", "t"), V4 = c("k", "k", "k", "k", "k", "k", "k", "k")))
str(test_data)

filtered.cor(test_data)




smart_cor <- function(x) {
    p1 = shapiro.test(x[[1]])$p.value
    p2 = shapiro.test(x[[2]])$p.value
    print(c(p1,p2))
    if (p1 < 0.05 | p2 < 0.05) {
        s = "spearman"
    } else {
        s="pearson"
    }
    return(cor.test(x = x[[1]], y = x[[2]],method=s)$estimate)
}

test_data <- as.data.frame(list(col1 = c(0.06, 0.68, 1.15, 1.17, -1.66, -0.13, 0.06, 0.24, 1.49, 0.21, 1.51, 1.96, 0.61, -0.81, 0.89, -0.91, -0.88, -0.61, -1.4, 1.4, 1.66, 0.65, -1.18, -0.21, 1.69, 0.73, -1.22, 1.83, -0.57, 1.44), col2 = c(-0.59, -0.65, -0.74, 1.39, 0.91, -1.42, 0.31, -1.71, -0.21, -0.43, -1.68, 0.13, -0.75, 0.37, -0.04, -0.03, -2.01, 0.11, 0.08, -1.29, -1.08, -0.67, 0.72, 1.23, -1.81, 1.65, -0.14, -1.66, 0.05, -0.67)))

smart_cor(test_data)




df = read.table("dataset_11508_12.txt", dec = ".")
df = data.frame(df)
t = lm(df[[1]]~ df[[2]])
summary(t)


library(ggplot2)
df = subset(diamonds, cut == "Ideal" & carat == 0.46)
fit_coef = lm(price ~ depth, df)$coefficients



regr.calc <- function(x) {
    p = cor.test(x[[1]], x[[2]])$p.value
    if (p >= 0.05) {
        return("There is no sense in prediction")
    }

    t = lm(x[[1]] ~ x[[2]])
    fit = t$fitted.values
    x$fit = fit
    return(x)
}


library(ggplot2)
my_plot = ggplot(iris, aes(x = Sepal.Width, y = Petal.Width, col = Species)) +
    geom_smooth(method = "lm") +
    geom_point()
my_plot



library(ggplot2)
ggplot(mtcars, aes(mpg, disp, col = factor(am)))+
  geom_point()+
  geom_smooth()

ggplot(mtcars, aes(mpg, disp)) +
    geom_point(aes(col = factor(am))) +
    geom_smooth()

ggplot(mtcars, aes(mpg, disp)) +
    geom_point() +
    geom_smooth(aes(col = factor(am)))




fill_na <- function(x) {
    t = lm(y ~ x_1 + x_2, x,na.action = na.omit)
    y_full = ifelse(is.na(x$y), predict(t,x), x$y)
    x$y_full = y_full
    return(x)
}

test_data <- read.csv(url('https://stepic.org/media/attachments/course/129/fill_na_test.csv'))
fill_na(test_data)




df = data.frame(mtcars$wt, mtcars$mpg, mtcars$disp, mtcars$drat, mtcars$hp)
colnames(df)=c("wt", "mpg", "disp", "drat", "hp")
df

model = lm(wt ~ disp+mpg+hp, df)
summary(model)



summary(lm(rating ~ complaints*critical, attitude))



mtcars$am <- factor(mtcars$am, labels = c('Automatic', 'Manual'))
summary(lm(mpg ~ wt * am, mtcars))


library(ggplot2)
# сначала переведем переменную am в фактор
mtcars$am <- factor(mtcars$am)
# теперь строим график
my_plot <- ggplot(mtcars, aes(wt, mpg, col = factor(am))) +
    geom_smooth(method="lm")
my_plot



model_full <- lm(rating ~ ., data = attitude)
model_null <- lm(rating ~ 1, data = attitude)
scope = list(lower = model_null, upper = model_full)
ideal_model <-step(model_null,scope = scope,direction = "forward")
summary( ideal_model)

anova(model_full,ideal_model)


str(LifeCycleSavings)
model <- lm(sr ~ (.)^2, LifeCycleSavings)# . значит сумму по всем переменным, квадрат делает ещё и взаимодействия до второго уровня
summary(model)





high.corr <- function(x) {
    mat =abs( cor(x))
    diag(mat) = 0
    mx = which.max(mat)
    len1 = nrow(mat)
    len2=ncol(mat)
    i = mx %/% len2
    j = mx - (i - 1) * len2
    return(c(rownames(mat)[i],colnames(mat)[j]))
}

x1 <- rnorm(30) # создадим случайную выборку
x2 <- rnorm(30) # создадим случайную выборку
x3 <- x2 + 5 # теперь коэффициент корреляции x1 и x3 равен единице
my_df <- data.frame(var1 = x1, var2 = x2, var3 = x3)
high.corr(my_df)



df = mtcars
df$am = factor(df$am)
t = glm(am ~ disp + vs + mpg, df, family = "binomial")
#summary(t)
log_coef=t$coefficients


library("ggplot2")
obj <- ggplot(data = ToothGrowth, aes(x = supp, y = len, fill = factor(dose))) +
    geom_boxplot()
obj




test_data <- read.csv(url('https://stepic.org/media/attachments/lesson/11478/data.csv'))
test_data = data.frame(test_data)
test_data$admit = factor(test_data$admit)
test_data$rank=factor(test_data$rank)

t = glm(admit ~ (gre+gpa +rank)^2,test_data, family = "binomial", na.action = na.omit)
y_full = ifelse(is.na(test_data$admit), predict(t, test_data, type = "response",newdata=T), 0)
y_full=y_full[y_full >= 0.4]
length(y_full)




normality.test <- function(x) {
    df = x[, sapply(x, is.numeric)]
    v=c()
    for (i in 1:ncol(df)) {
        p = shapiro.test(df[[i]])$p.value
        v=cbind(v,p)
    }
    v=as.vector( v)
    names(v)=colnames(df)
    return(v)
}
normality.test(mtcars[, 1:6])



########################################вторая часть курса

library(dplyr)
library(lazyeval)

find_outliers <- function(t) {
   
    s = t %>% group_by_if(is.factor)
    mutate_call = lazyeval::interp(~ifelse(abs(a-mean(a)) <= 2 * sd(a),0,1), a = as.name(colnames(s)[sapply(s, is.numeric)]))
    k = mutate_(s, .dots = setNames(list(mutate_call), 'is_outlier'))

    return(as.data.frame( k))
}
ToothGrowth$dose <- factor(ToothGrowth$dose)
find_outliers(ToothGrowth)




library(data.table)
filter.expensive.available <- function(products, brands) {
    return(products[price/100>=5000 &available==T & brand %in% brands,])
}

sample.products <- data.table(price = c(10000, 600000, 700000, 1000000),
                              brand = c("a", "b", "c", "d"),
                              available = c(T, T, F, T))
filter.expensive.available(sample.products, c("a", "c", "d"))




ordered.short.purchase.data <- function(purchases) {
    return(purchases[order(-price)][quantity>=0, c("ordernumber", "product_id")])
}

sample.purchases <- data.table(price = c(100000, 6000, 7000, 5000000),
                               ordernumber = 1:4,
                               quantity = c(1, 2, 1, -1),
                               product_id = 1:4)
ordered.short.purchase.data(sample.purchases)



library(dplyr)
purchases.median.order.price <- function(purchases) {

    SUMARIZE = purchases[quantity > 0, .(pr = quantity * price), by = ordernumber]
    s=SUMARIZE%>%as.data.frame()%>%group_by(ordernumber)%>%summarise(sp=sum(pr))
    return(median(s$sp))
}
sample.purchases <- data.table(price = c(100000, 6000, 7000, 5000000),
                               ordernumber = c(1, 2, 2, 3),
                               quantity = c(1, 2, 1, -1),
                               product_id = 1:4)
purchases.median.order.price(sample.purchases)




library(ggplot2)
x_cut_density <- qplot(x = x, data = diamonds, color = cut, geom = "density")
 x_cut_density

price_violin <- qplot(x=color,y=price,data = diamonds,geom = "violin")


my_plot <- ggplot(mtcars, aes(x = factor(am), y = mpg)) +
                  geom_violin()+
geom_boxplot(width = 0.2)
my_plot



sales = read.csv("https://stepic.org/media/attachments/course/724/sales.csv")

my_plot <- ggplot(sales, aes(x = income, y = sale)) +
    geom_point(aes(col = shop)) +
    geom_smooth()
my_plot


my_plot <- ggplot(sales, aes(x = shop, y = income, col = season)) +
    stat_summary(fun.data = mean_cl_boot, geom = "pointrange",
                 position = position_dodge(0.2))
my_plot


my_plot <- ggplot(sales, aes(x = date, y = sale, col = shop)) +
    stat_summary(fun.data = mean_cl_boot, geom = "errorbar",
                 position = position_dodge(0.2)) + # добавим стандартную ошибку
                 stat_summary(fun.data = mean_cl_boot, geom = "point",
                 position = position_dodge(0.2)) + # добавим точки
                 stat_summary(fun.data = mean_cl_boot, geom = "line",
                 position = position_dodge(0.2)) # соединим линиями
my_plot


library(ggplot2)
mpg_facet <- ggplot(mtcars, aes(x = mpg)) +
    facet_grid(am ~ vs) +
    geom_dotplot()
mpg_facet



sl_wrap <- ggplot(iris, aes(x = Sepal.Length)) +
    geom_density() +
    facet_wrap(~Species)
sl_wrap


my_plot <- ggplot(iris, aes(x = Sepal.Length, y = Sepal.Width)) +
    geom_point() +
    geom_smooth()+
    facet_wrap(~Species)
my_plot


myMovieData = read.csv("myMovieData.csv")
str(myMovieData)

my_plot <- ggplot(myMovieData, aes(x = Type, y = Budget)) +
    geom_boxplot() +
    facet_grid(.~Year)+
theme(axis.text.x = element_text(angle = 90, hjust = 1))

my_plot


iris_plot <- ggplot(iris, aes(x = Sepal.Length, y = Petal.Length, col = Species)) +
    geom_point() +
    geom_smooth(method = "lm") +
    scale_color_discrete(name = "Вид цветка",
                       labels = c("Ирис щетинистый", "Ирис разноцветный", "Ирис виргинский")) +
                       scale_x_continuous(name = "Длина чашелистика", breaks = c(4, 5, 6, 7, 8),limits = c(4,8)) +
                       scale_y_continuous(name = "Длина лепестка", breaks = c(1, 2, 3, 4, 5, 6, 7)) 
iris_plot







library(data.table)
library(plotly)
make.fancy.teapot <- function(teapot.coords) {
    len=nrow(teapot.coords)
    i.s <- seq(0, len - 2, 3)
    j.s <- seq(1, len - 1, 3)
    k.s <- seq(2, len , 3)
    plot_ly(teapot.coords, x = ~x, y = ~y, z = ~z, i = ~i.s, j = ~j.s, k = ~k.s, type = "mesh3d")
}

df=data.table(read.csv("teapot2.csv",sep = ";",dec = "."))
head(df,14)
make.fancy.teapot(df)




library(dplyr)
mark.position.portion <- function(purchases) {

    SUMARIZE = purchases[quantity > 0, .(pr = quantity * price), by = ordernumber]
    s = SUMARIZE %>% as.data.frame() %>% group_by(ordernumber) %>% mutate(price.portion = formatC(round(pr / sum(pr) * 100, 2), format = 'f', digits = 2))
    s=as.data.frame(s)
    return(as.data.table(cbind(purchases[quantity > 0], s["price.portion"])))
}
sample.purchases <- data.table(price = c(100, 300, 50, 700, 30),
                               ordernumber = c(1, 1, 1, 2, 3),
                               quantity = c(1, 1, 2, 1, -1),
                               product_id = 1:5)



library(data.table)
sample.purchases = data.table(
product_id = c(1739, 1948, 1055, 285, 291, 147),
price = c(7840592.95, 5958649.13, 529825.34, 4607180.89, 1681484.37, 176796683.66),
quantity = c(1, 1, 1, 1, 1, 1),
ordernumber = c(3, 3, 5, 6, 6, 9))

mark.position.portion <- function(purchases) {
    S = purchases[quantity > 0, price.portion := formatC(round((quantity * price) / sum((quantity * price)) * 100, 2), format = 'f', digits = 2), by = ordernumber]
   # print(S)
    return(as.data.table(S[quantity > 0]))
}

mark.position.portion(sample.purchases)