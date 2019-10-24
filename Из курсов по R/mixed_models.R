# Синтаксис для смешанных регрессионных моделей в пакете 'lme4'

lmer(DV ~ IV + (1 + IV | RV), data = my_data)

install.packages('lme4')
install.packages('mlmRev')
install.packages('lmerTest')

library(mlmRev)
library(lme4)


data("Exam")

str(Exam)
help(Exam)




ggplot(data = Exam, aes(x = standLRT, y = normexam)) + 
  geom_point()


ggplot(data = Exam, aes(x = standLRT, y = normexam, col = school)) + 
  geom_point()



# Один главный эффект


Model1 <- lm(normexam ~ standLRT, data=Exam)

Exam$Model1_pred <- predict(Model1)
ggplot(data = Exam, aes(x = standLRT, y = normexam)) + 
  geom_point() + 
  geom_line(data = Exam, aes(x = standLRT, y = Model1_pred), col = 'blue', size = 1)



Model1 <- lmer(normexam ~ standLRT, data=Exam)






# Главный эффект + случайный свободный член
Model2 <- lmer(normexam ~ standLRT + (1|school), data=Exam)
summary(Model2)

Exam$Model2_pred <- predict(Model2)
ggplot(data = Exam, aes(x = standLRT, y = normexam)) + 
  geom_point(alpha = 0.2) + 
  geom_line(data = Exam, aes(x = standLRT, y = Model2_pred, col = school))












# Главный эффект + случайный свободный член + случайный угловой коэффициент
Model3 <- lmer(normexam ~ standLRT + (1 + standLRT|school), data=Exam)
summary(Model3)

Exam$Model3_pred <- predict(Model3)
ggplot(data = Exam, aes(x = standLRT, y = normexam)) + 
  geom_point(alpha = 0.2) + 
  geom_line(data = Exam, aes(x = standLRT, y = Model3_pred, col = school))








# Главный эффект + случайный угловой коэффициент
Model4 <- lmer(normexam ~ standLRT + (0 + standLRT|school), data=Exam)
summary(Model4)

Exam$Model4_pred <- predict(Model4)
ggplot(data = Exam, aes(x = standLRT, y = normexam)) + 
  geom_point(alpha = 0.2) + 
  geom_line(data = Exam, aes(x = standLRT, y = Model4_pred, col = school))










# Нескоррелированные случайные эффекты
Model5 <- lmer(normexam ~ standLRT + (1|school) + (0 + standLRT|school), data=Exam)
summary(Model5)






###################################################################################

# Сравнение моделей


Model2 <- lmer(normexam ~ standLRT + (1|school), REML = FALSE, data=Exam)
summary(Model2)

Model0 <- lmer(normexam ~ 1 + (1|school), REML = FALSE, data = Exam)
summary(Model0)

anova(Model0, Model2)






# p-значения

library(lmerTest)

Model2 <- lmer(normexam ~ standLRT + (1|school), data=Exam)
summary(Model2)






# Обобщённые смешанные модели

Exam$school_type <- ifelse(Exam$type == 'Mxd', 1, 0)

Model5 <- glmer(school_type ~ normexam + (1|school), family = "binomial", data = Exam)

summary(Model5)






# Предсказания на новых датасетах


predict(Model2, Exam)


new_Exam <- Exam[sample(1:nrow(Exam), 100), ]
new_Exam$school <- sample(101:200)

predict(Model2, new_Exam, allow.new.levels = T)



# Исследование случайных эффектов

fixef(Model3)
ranef(Model3)
















