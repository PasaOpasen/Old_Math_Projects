
y=rnorm(100)
x=rnorm(100)/1000+0.2*y
c=factor(rep(c("A","B","C","D"),25))

summary(lm(y~x+c))



cars=DAAGbio::toycars 
# Полная модель
toy_mod <- 
  # Значимо ли взаимодействие? Ответ - результат drop1()
  test_interaction <- 
  # Упрощенная модель (NA, если нельзя удалить взаимодействие)
  simple_mod <-















