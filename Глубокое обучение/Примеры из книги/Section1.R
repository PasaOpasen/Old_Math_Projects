

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







