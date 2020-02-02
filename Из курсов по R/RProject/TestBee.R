
#bee
library(dplyr)
s = read.table("bee.txt", dec = ",")
x = s[[1]]
y = s[[2]]
n = 1:length(x)
df = data_frame(pair = n, error = (x - y) / y * 100,result=factor( ifelse(error>0,"more","less")))


png(filename = "bee.png",width = 800, height = 850)
library(ggplot2)
ggplot(df, aes(x = pair, y = error, col = result)) +
    geom_point(size = 3) +
    geom_line(y = 0, col = "red", size = 1.2, linetype = "dashed") +
    scale_y_continuous(name = "Error percent", breaks = round(seq(min(df$error), max(df$error), length.out = 15))) +
    scale_x_continuous(name = "Every cup", breaks = c()) +
    scale_color_manual(values = c("black", "green"), name = "Position",
                       labels = c(paste("less =", sum(df$error < 0)), paste("more =", sum(df$error >= 0)))) +
                       theme_bw()

dev.off()