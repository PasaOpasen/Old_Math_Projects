

#Listing 2.1. Loading the MNIST dataset in Keras
library(keras)

mnist <- dataset_mnist()
train_images <- mnist$train$x
train_labels <- mnist$train$y
test_images <- mnist$test$x
test_labels <- mnist$test$y




#Listing 2.2. The network architecture
network <- keras_model_sequential() %>%
  layer_dense(units = 512, activation = "relu", input_shape = c(28 * 28)) %>%
  layer_dense(units = 10, activation = "softmax")



#Listing 2.3. The compilation step
network %>% compile(
  optimizer = "rmsprop",
  loss = "categorical_crossentropy",
  metrics = c("accuracy")
)


#Listing 2.4. Preparing the image data
train_images <- array_reshape(train_images, c(60000, 28 * 28))
train_images <- train_images / 255

test_images <- array_reshape(test_images, c(10000, 28 * 28))
test_images <- test_images / 255


#Listing 2.5. Preparing the labels
train_labels <- to_categorical(train_labels)
test_labels <- to_categorical(test_labels)



network %>% fit(train_images, train_labels, epochs = 5, batch_size = 128)



metrics <- network %>% evaluate(test_images, test_labels)
metrics

network %>% predict_classes(test_images[1:10,])



digit <- train_images[5,,]
plot(as.raster(digit, max = 255))



#Listing 3.1. Loading the IMDB dataset
library(keras)

imdb <- dataset_imdb(num_words = 10000)
c(c(train_data, train_labels), c(test_data, test_labels)) %<-% imdb


#Listing 3.2. Encoding the integer sequences into a binary matrix
vectorize_sequences <- function(sequences, dimension = 10000) {
  results <- matrix(0, nrow = length(sequences), ncol = dimension)    
  for (i in 1:length(sequences))
    results[i, sequences[[i]]] <- 1                                   
  results
}

x_train <- vectorize_sequences(train_data)
x_test <- vectorize_sequences(test_data)




#Listing 3.3. The model definition
library(keras)

model <- keras_model_sequential() %>%
  layer_dense(units = 16, activation = "relu", input_shape = c(10000)) %>%
  layer_dense(units = 16, activation = "relu") %>%
  layer_dense(units = 1, activation = "sigmoid")



#Listing 3.4. Compiling the model
model %>% compile(
  optimizer = "rmsprop",
  loss = "binary_crossentropy",
  metrics = c("accuracy")
)



#Listing 3.5. Configuring the optimizer
model %>% compile(
  optimizer = optimizer_rmsprop(lr=0.001),
  loss = "binary_crossentropy",
  metrics = c("accuracy")
)


#Listing 3.6. Using custom losses and metrics
model %>% compile(
  optimizer = optimizer_rmsprop(lr = 0.001),
  loss = loss_binary_crossentropy,
  metrics = metric_binary_accuracy
)


#Listing 3.7. Setting aside a validation set
val_indices <- 1:10000

x_val <- x_train[val_indices,]
partial_x_train <- x_train[-val_indices,]
y_val <- y_train[val_indices]
partial_y_train <- y_train[-val_indices]



#Listing 3.8. Training your model
model %>% compile(
  optimizer = "rmsprop",
  loss = "binary_crossentropy",
  metrics = c("accuracy")
)

history <- model %>% fit(
  partial_x_train,
  partial_y_train,
  epochs = 20,
  batch_size = 512,
  validation_data = list(x_val, y_val)
)


plot(history)


#Listing 3.9. Retraining a model from scratch
model <- keras_model_sequential() %>%
  layer_dense(units = 16, activation = "relu", input_shape = c(10000)) %>%
  layer_dense(units = 16, activation = "relu") %>%
  layer_dense(units = 1, activation = "sigmoid")

model %>% compile(
  optimizer = "rmsprop",
  loss = "binary_crossentropy",
  metrics = c("accuracy")
)

model %>% fit(x_train, y_train, epochs = 4, batch_size = 512)
results <- model %>% evaluate(x_test, y_test)



#Listing 3.10. Loading the Reuters dataset
library(keras)

reuters <- dataset_reuters(num_words = 10000)
c(c(train_data, train_labels), c(test_data, test_labels)) %<-% reuters



#Listing 3.11. Decoding newswires back to text
word_index <- dataset_reuters_word_index()
reverse_word_index <- names(word_index)
names(reverse_word_index) <- word_index
decoded_newswire <- sapply(train_data[[1]], function(index) {
  word <- if (index >= 3) reverse_word_index[[as.character(index - 3)]]    
  if (!is.null(word)) word else "?"
})



#Listing 3.12. Encoding the data
vectorize_sequences <- function(sequences, dimension = 10000) {
  results <- matrix(0, nrow = length(sequences), ncol = dimension)
  for (i in 1:length(sequences))
    results[i, sequences[[i]]] <- 1
  results
}

x_train <- vectorize_sequences(train_data)          
x_test <- vectorize_sequences(test_data)              



#Listing 3.13. Model definition
model <- keras_model_sequential() %>%
  layer_dense(units = 64, activation = "relu", input_shape = c(10000)) %>%
  layer_dense(units = 64, activation = "relu") %>%
  layer_dense(units = 46, activation = "softmax")



#Listing 3.14. Compiling the model
model %>% compile(
  optimizer = "rmsprop",
  loss = "categorical_crossentropy",
  metrics = c("accuracy")
)



#Listing 3.15. Setting aside a validation set
val_indices <- 1:1000

x_val <- x_train[val_indices,]
partial_x_train <- x_train[-val_indices,]

y_val <- one_hot_train_labels[val_indices,]
partial_y_train = one_hot_train_labels[-val_indices,]



#Listing 3.16. Training the model
history <- model %>% fit(
  partial_x_train,
  partial_y_train,
  epochs = 20,
  batch_size = 512,
  validation_data = list(x_val, y_val)
)


plot(history)



#Listing 3.18. Retraining a model from scratch
model <- keras_model_sequential() %>%
  layer_dense(units = 64, activation = "relu", input_shape = c(10000)) %>%
  layer_dense(units = 64, activation = "relu") %>%
  layer_dense(units = 46, activation = "softmax")

model %>% compile(
  optimizer = "rmsprop",
  loss = "categorical_crossentropy",
  metrics = c("accuracy")
)

history <- model %>% fit(
  partial_x_train,
  partial_y_train,
  epochs = 9,
  batch_size = 512,
  validation_data = list(x_val, y_val)
)

results <- model %>% evaluate(x_test, one_hot_test_labels)



#Listing 3.19. Generating predictions for new data
predictions <- model %>% predict(x_test)



#Listing 3.20. A model with an information bottleneck
model <- keras_model_sequential() %>%
  layer_dense(units = 64, activation = "relu", input_shape = c(10000)) %>%
  layer_dense(units = 4, activation = "relu") %>%
  layer_dense(units = 46, activation = "softmax")

model %>% compile(
  optimizer = "rmsprop",
  loss = "categorical_crossentropy",
  metrics = c("accuracy")
)

model %>% fit(
  partial_x_train,
  partial_y_train,
  epochs = 20,
  batch_size = 128,
  validation_data = list(x_val, y_val)
)



#Listing 3.21. Loading the Boston housing dataset
library(keras)

dataset <- dataset_boston_housing()
c(c(train_data, train_targets), c(test_data, test_targets)) %<-% dataset



#Listing 3.22. Normalizing the data
mean <- apply(train_data, 2, mean)                                  
std <- apply(train_data, 2, sd)
train_data <- scale(train_data, center = mean, scale = std)         
test_data <- scale(test_data, center = mean, scale = std)



#Listing 3.23. Model definition
build_model <- function() {                           
  model <- keras_model_sequential() %>%
    layer_dense(units = 64, activation = "relu",
                input_shape = dim(train_data)[[2]]) %>%
    layer_dense(units = 64, activation = "relu") %>%
    layer_dense(units = 1)
  model %>% compile(
    optimizer = "rmsprop",
    loss = "mse",
    metrics = c("mae")
  )
}




#Listing 3.24. K-fold validation
k <- 4
indices <- sample(1:nrow(train_data))
folds <- cut(indices, breaks = k, labels = FALSE)

num_epochs <- 100
all_scores <- c()
for (i in 1:k) {
  cat("processing fold #", i, "\n")
  
  val_indices <- which(folds == i, arr.ind = TRUE)                     
  val_data <- train_data[val_indices,]
  val_targets <- train_targets[val_indices]
  partial_train_data <- train_data[-val_indices,]                      
  partial_train_targets <- train_targets[-val_indices]
  
  model <- build_model()                                               
  
  model %>% fit(partial_train_data, partial_train_targets,             
                epochs = num_epochs, batch_size = 1, verbose = 0)
  
  results <- model %>% evaluate(val_data, val_targets, verbose = 0)    
  all_scores <- c(all_scores, results$mean_absolute_error)
}


#Listing 3.25. Saving the validation logs at each fold
num_epochs <- 500
all_mae_histories <- NULL
for (i in 1:k) {
  cat("processing fold #", i, "\n")
  
  val_indices <- which(folds == i, arr.ind = TRUE)              
  val_data <- train_data[val_indices,]
  val_targets <- train_targets[val_indices]
  
  partial_train_data <- train_data[-val_indices,]               
  partial_train_targets <- train_targets[-val_indices]
  
  model <- build_model()                                        
  
  history <- model %>% fit(                                     
                                                                partial_train_data, partial_train_targets,
                                                                validation_data = list(val_data, val_targets),
                                                                epochs = num_epochs, batch_size = 1, verbose = 0
  )
  mae_history <- history$metrics$val_mean_absolute_error
  all_mae_histories <- rbind(all_mae_histories, mae_history)
}




#Listing 3.26. Building the history of successive mean K-fold validation scores
average_mae_history <- data.frame(
  epoch = seq(1:ncol(all_mae_histories)),
  validation_mae = apply(all_mae_histories, 2, mean)
)


#Listing 3.27. Plotting validation scores
library(ggplot2)
ggplot(average_mae_history, aes(x = epoch, y = validation_mae)) + geom_line()



#Listing 3.28. Plotting validation scores with geom_smooth()
ggplot(average_mae_history, aes(x = epoch, y = validation_mae)) + geom_smooth()



#Listing 3.29. Training the final model
model <- build_model()
model %>% fit(train_data, train_targets,                  
              epochs = 80, batch_size = 16, verbose = 0)
result <- model %>% evaluate(test_data, test_targets)





#Listing 4.1. Hold-out validation
indices <- sample(1:nrow(data), size = 0.80 * nrow(data))               
evaluation_data  <- data[-indices, ]                                    
training_data <- data[indices, ]                                        

model <- get_model()                                                    
model %>% train(training_data)                                          
validation_score <- model %>% evaluate(validation_data)                 

model <- get_model()                                                    
model %>% train(data)                                                   
test_score <- model %>% evaluate(test_data)                             



#Listing 4.2. K-fold cross-validation
k <- 4
indices <- sample(1:nrow(data))
folds <- cut(indices, breaks = k, labels = FALSE)

validation_scores <- c()
for (i in 1:k) {
  
  validation_indices <- which(folds == i, arr.ind = TRUE)
  validation_data <- data[validation_indices,]                          
  training_data <- data[-validation_indices,]                           
  
  model <- get_model()                                                  
  model %>% train(training_data)
  results <- model %>% evaluate(validation_data)
  validation_scores <- c(validation_scores, results$accuracy)
}

validation_score <- mean(validation_scores)                             

model <- get_model()                                                    
model %>% train(data)                                                   
results <- model %>% evaluate(test_data)                                



#Listing 4.3. Original model
library(keras)

model <- keras_model_sequential() %>%
  layer_dense(units = 16, activation = "relu", input_shape = c(10000)) %>%
  layer_dense(units = 16, activation = "relu") %>%
  layer_dense(units = 1, activation = "sigmoid")


#Listing 4.4. Version of the model with lower capacity
model <- keras_model_sequential() %>%
  layer_dense(units = 4, activation = "relu", input_shape = c(10000)) %>%
  layer_dense(units = 4, activation = "relu") %>%
  layer_dense(units = 1, activation = "sigmoid")


#Listing 4.5. Version of the model with higher capacity
model <- keras_model_sequential() %>%
  layer_dense(units = 512, activation = "relu", input_shape = c(10000)) %>%
  layer_dense(units = 512, activation = "relu") %>%
  layer_dense(units = 1, activation = "sigmoid")



#Listing 4.6. Adding L2 weight regularization to the model
model <- keras_model_sequential() %>%
  layer_dense(units = 16, kernel_regularizer = regularizer_l2(0.001),
              activation = "relu", input_shape = c(10000)) %>%
  layer_dense(units = 16, kernel_regularizer = regularizer_l2(0.001),
              activation = "relu") %>%
  layer_dense(units = 1, activation = "sigmoid")



#Listing 4.7. Different weight regularizers available in Keras
regularizer_l1(0.001)
regularizer_l1_l2(l1 = 0.001, l2 = 0.001)



#Listing 4.8. Adding dropout to the IMDB network
model <- keras_model_sequential() %>%
  layer_dense(units = 16, activation = "relu", input_shape = c(10000)) %>%
  layer_dropout(rate = 0.5) %>%
  layer_dense(units = 16, activation = "relu") %>%
  layer_dropout(rate = 0.5) %>%
  layer_dense(units = 1, activation = "sigmoid")




































































