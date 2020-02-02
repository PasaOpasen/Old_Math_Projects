
arr = c("rgl", "plot3D", "data.table", "ggplot2", "viridis", "gridExtra", "fields", "plotly", "foreach", "htmlwidgets", "reshape2", "cowplot","psych","Hmisc","coin","stargazer","dplyr","rmarkdown","lmtest")

foo <- function(x) {
    for (i in x) {
        #  require returns TRUE invisibly if it was able to load package
        if (!require(i, character.only = TRUE)) {
            #  If package was not able to be loaded then re-install
            install.packages(i, dependencies = TRUE)
            #  Load package after installing
            require(i, character.only = TRUE)
        }
    }
}
#  Then try/install packages...
foo(arr)