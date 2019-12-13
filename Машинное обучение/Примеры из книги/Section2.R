

#Listing 5.1. Instantiating a small convnet
library(keras)

model <- keras_model_sequential() %>%
  layer_conv_2d(filters = 32, kernel_size = c(3, 3), activation = "relu",
                input_shape = c(28, 28, 1)) %>%
  layer_max_pooling_2d(pool_size = c(2, 2)) %>%
  layer_conv_2d(filters = 64, kernel_size = c(3, 3), activation = "relu") %>%
  layer_max_pooling_2d(pool_size = c(2, 2)) %>%
  layer_conv_2d(filters = 64, kernel_size = c(3, 3), activation = "relu")



model


#Listing 5.2. Adding a classifier on top of the convnet
model <- model %>%
  layer_flatten() %>%
  layer_dense(units = 64, activation = "relu") %>%
  layer_dense(units = 10, activation = "softmax")


#Listing 5.3. Training the convnet on MNIST images
mnist <- dataset_mnist()
c(c(train_images, train_labels), c(test_images, test_labels)) %<-% mnist
train_images <- array_reshape(train_images, c(60000, 28, 28, 1))
train_images <- train_images / 255
test_images <- array_reshape(test_images, c(10000, 28, 28, 1))
test_images <- test_images / 255
train_labels <- to_categorical(train_labels)
test_labels <- to_categorical(test_labels)
model %>% compile(
  optimizer = "rmsprop",
  loss = "categorical_crossentropy",
  metrics = c("accuracy")
)
model %>% fit(
  train_images, train_labels,
  epochs = 5, batch_size=64
)


results <- model %>% evaluate(test_images, test_labels)


#Listing 5.4. Copying images to training, validation, and test directories
original_dataset_dir <- "~/Downloads/kaggle_original_data"
base_dir <- "~/Downloads/cats_and_dogs_small"
dir.create(base_dir)
train_dir <- file.path(base_dir, "train")
dir.create(train_dir)
validation_dir <- file.path(base_dir, "validation")
dir.create(validation_dir)
test_dir <- file.path(base_dir, "test")
dir.create(test_dir)
train_cats_dir <- file.path(train_dir, "cats")
dir.create(train_cats_dir)
train_dogs_dir <- file.path(train_dir, "dogs")
dir.create(train_dogs_dir)
validation_cats_dir <- file.path(validation_dir, "cats")
dir.create(validation_cats_dir)
validation_dogs_dir <- file.path(validation_dir, "dogs")
dir.create(validation_dogs_dir)
test_cats_dir <- file.path(test_dir, "cats")
dir.create(test_cats_dir)
test_dogs_dir <- file.path(test_dir, "dogs")
dir.create(test_dogs_dir)
fnames <- paste0("cat.", 1:1000, ".jpg")
file.copy(file.path(original_dataset_dir, fnames),
          file.path(train_cats_dir))
fnames <- paste0("cat.", 1001:1500, ".jpg")
file.copy(file.path(original_dataset_dir, fnames),
          file.path(validation_cats_dir))
fnames <- paste0("cat.", 1501:2000, ".jpg")
file.copy(file.path(original_dataset_dir, fnames),
          file.path(test_cats_dir))
fnames <- paste0("dog.", 1:1000, ".jpg")
file.copy(file.path(original_dataset_dir, fnames),
          file.path(train_dogs_dir))
fnames <- paste0("dog.", 1001:1500, ".jpg")
file.copy(file.path(original_dataset_dir, fnames),
          file.path(validation_dogs_dir))
fnames <- paste0("dog.", 1501:2000, ".jpg")
file.copy(file.path(original_dataset_dir, fnames),
          file.path(test_dogs_dir))



#Listing 5.5. Instantiating a small convnet for dogs-versus-cats classification
library(keras)
model <- keras_model_sequential() %>%
  layer_conv_2d(filters = 32, kernel_size = c(3, 3), activation = "relu",
                input_shape = c(150, 150, 3)) %>%
  layer_max_pooling_2d(pool_size = c(2, 2)) %>%
  layer_conv_2d(filters = 64, kernel_size = c(3, 3), activation = "relu") %>%
  layer_max_pooling_2d(pool_size = c(2, 2)) %>%
  layer_conv_2d(filters = 128, kernel_size = c(3, 3), activation = "relu") %>%
  layer_max_pooling_2d(pool_size = c(2, 2)) %>%
  layer_conv_2d(filters = 128, kernel_size = c(3, 3), activation = "relu") %>%
  layer_max_pooling_2d(pool_size = c(2, 2)) %>%
  layer_flatten() %>%
  layer_dense(units = 512, activation = "relu") %>%
  layer_dense(units = 1, activation = "sigmoid")



#Listing 5.6. Configuring the model for training
model %>% compile(
  loss = "binary_crossentropy",
  optimizer = optimizer_rmsprop(lr = 1e-4),
  metrics = c("acc")
)




#Listing 5.7. Using image_data_generator to read images from directories
train_datagen <- image_data_generator(rescale = 1/255)             
validation_datagen <- image_data_generator(rescale = 1/255)        

train_generator <- flow_images_from_directory(
  train_dir,                                                       
  train_datagen,                                                   
  target_size = c(150, 150),                                       
  batch_size = 20,                                                 
  class_mode = "binary"
)

validation_generator <- flow_images_from_directory(
  validation_dir,
  validation_datagen,
  target_size = c(150, 150),
  batch_size = 20,
  class_mode = "binary"
)



#Listing 5.8. Fitting the model using a batch generator
history <- model %>% fit_generator(
  train_generator,
  steps_per_epoch = 100,
  epochs = 30,
  validation_data = validation_generator,
  validation_steps = 50
)



#Listing 5.9. Saving the model
model %>% save_model_hdf5("cats_and_dogs_small_1.h5")


#Listing 5.10. Displaying curves of loss and accuracy during training
plot(history)



#Listing 5.11. Setting up a data augmentation configuration via image_data_generator
datagen <- image_data_generator(
  rescale = 1/255,
  rotation_range = 40,
  width_shift_range = 0.2,
  height_shift_range = 0.2,
  shear_range = 0.2,
  zoom_range = 0.2,
  horizontal_flip = TRUE,
  fill_mode = "nearest"
)



#Listing 5.12. Displaying some randomly augmented training images
fnames <- list.files(train_cats_dir, full.names = TRUE)
img_path <- fnames[[3]]                                               

img <- image_load(img_path, target_size = c(150, 150))                
img_array <- image_to_array(img)                                      
img_array <- array_reshape(img_array, c(1, 150, 150, 3))              

augmentation_generator <- flow_images_from_data(           
    img_array,    
                generator = datagen,                                                
                  batch_size = 1
)

op <- par(mfrow = c(2, 2), pty = "s", mar = c(1, 0, 1, 0))            
for (i in 1:4) {                                                      
  batch <- generator_next(augmentation_generator)                     
  plot(as.raster(batch[1,,,]))                                        
}                                                                     
par(op)



#Listing 5.13. Defining a new convnet that includes dropout
model <- keras_model_sequential() %>%
  layer_conv_2d(filters = 32, kernel_size = c(3, 3), activation = "relu",
                input_shape = c(150, 150, 3)) %>%
  layer_max_pooling_2d(pool_size = c(2, 2)) %>%
  layer_conv_2d(filters = 64, kernel_size = c(3, 3), activation = "relu") %>%
  layer_max_pooling_2d(pool_size = c(2, 2)) %>%
  layer_conv_2d(filters = 128, kernel_size = c(3, 3), activation = "relu") %>%
  layer_max_pooling_2d(pool_size = c(2, 2)) %>%
  layer_conv_2d(filters = 128, kernel_size = c(3, 3), activation = "relu") %>%
  layer_max_pooling_2d(pool_size = c(2, 2)) %>%
  layer_flatten() %>%
  layer_dropout(rate = 0.5) %>%
  layer_dense(units = 512, activation = "relu") %>%
  layer_dense(units = 1, activation = "sigmoid")
model %>% compile(
  loss = "binary_crossentropy",
  optimizer = optimizer_rmsprop(lr = 1e-4),
  metrics = c("acc")
)




#Listing 5.14. Training the convnet using data-augmentation generators
datagen <- image_data_generator(
  rescale = 1/255,
  rotation_range = 40,
  width_shift_range = 0.2,
  height_shift_range = 0.2,
  shear_range = 0.2,
  zoom_range = 0.2,
  horizontal_flip = TRUE
)
test_datagen <- image_data_generator(rescale = 1/255)
train_generator <- flow_images_from_directory(        
 train_dir,                                            
datagen,                                             
 target_size = c(150, 150),                       
 batch_size = 32,
  class_mode = "binary"                                 
)
validation_generator <- flow_images_from_directory(
  validation_dir,
  test_datagen,
  target_size = c(150, 150),
  batch_size = 32,
  class_mode = "binary"
)
history <- model %>% fit_generator(
  train_generator,
  steps_per_epoch = 100,
  epochs = 100,
  validation_data = validation_generator,
  validation_steps = 50
)



#Listing 5.15. Saving the model
model %>% save_model_hdf5("cats_and_dogs_small_2.h5")


#Listing 5.16. Instantiating the VGG16 convolutional base
library(keras)

conv_base <- application_vgg16(
  weights = "imagenet",
  include_top = FALSE,
  input_shape = c(150, 150, 3)
)



#Listing 5.17. Extracting features using the pretrained convolutional base
base_dir <- "~/Downloads/cats_and_dogs_small"
train_dir <- file.path(base_dir, "train")
validation_dir <- file.path(base_dir, "validation")
test_dir <- file.path(base_dir, "test")
datagen <- image_data_generator(rescale = 1/255)
batch_size <- 20
extract_features <- function(directory, sample_count) {
  features <- array(0, dim = c(sample_count, 4, 4, 512))
  labels <- array(0, dim = c(sample_count))
  generator <- flow_images_from_directory(
    directory = directory,
    generator = datagen,
    target_size = c(150, 150),
    batch_size = batch_size,
    class_mode = "binary"
  )
  i <- 0
  while(TRUE) {
    batch <- generator_next(generator)
    inputs_batch <- batch[[1]]
    labels_batch <- batch[[2]]
    features_batch <- conv_base %>% predict(inputs_batch)
    index_range <- ((i * batch_size)+1):((i + 1) * batch_size)
    features[index_range,,,] <- features_batch
    labels[index_range] <- labels_batch
    i <- i + 1
    if (i * batch_size >= sample_count)
      break                                                
  }
  list(
    features = features,
    labels = labels
  )
}
train <- extract_features(train_dir, 2000)
validation <- extract_features(validation_dir, 1000)
test <- extract_features(test_dir, 1000)



#Listing 5.18. Defining and training the densely connected classifier
model <- keras_model_sequential() %>%
  layer_dense(units = 256, activation = "relu",
              input_shape = 4 * 4 * 512) %>%
  layer_dropout(rate = 0.5) %>%
  layer_dense(units = 1, activation = "sigmoid")

model %>% compile(
  optimizer = optimizer_rmsprop(lr = 2e-5),
  loss = "binary_crossentropy",
  metrics = c("accuracy")
)

history <- model %>% fit(
  train$features, train$labels,
  epochs = 30,
  batch_size = 20,
  validation_data = list(validation$features, validation$labels)
)



#Listing 5.19. Plotting the results
plot(history)




#Listing 5.20. Adding a densely connected classifier on top of the convolutional base
model <- keras_model_sequential() %>%
  conv_base %>%
  layer_flatten() %>%
  layer_dense(units = 256, activation = "relu") %>%
  layer_dense(units = 1, activation = "sigmoid")




#Listing 5.21. Training the model end to end with a frozen convolutional base
train_datagen = image_data_generator(
  rescale = 1/255,
  rotation_range = 40,
  width_shift_range = 0.2,
  height_shift_range = 0.2,
  shear_range = 0.2,
  zoom_range = 0.2,
  horizontal_flip = TRUE,
  fill_mode = "nearest"
)
test_datagen <- image_data_generator(rescale = 1/255)
train_generator <- flow_images_from_directory(            
 train_dir,                                               
 train_datagen,                                         
   target_size = c(150, 150),                               
           batch_size = 20,
            class_mode = "binary"                   
)
validation_generator <- flow_images_from_directory(
  validation_dir,
  test_datagen,
  target_size = c(150, 150),
  batch_size = 20,
  class_mode = "binary"
)
model %>% compile(
  loss = "binary_crossentropy",
  optimizer = optimizer_rmsprop(lr = 2e-5),
  metrics = c("accuracy")
)
history <- model %>% fit_generator(
  train_generator,
  steps_per_epoch = 100,
  epochs = 30,
  validation_data = validation_generator,
  validation_steps = 50
)


#Listing 5.22. Unfreezing previously frozen layers
unfreeze_weights(conv_base, from = "block3_conv1")



#Listing 5.23. Fine-tuning the model
model %>% compile(
  loss = "binary_crossentropy",
  optimizer = optimizer_rmsprop(lr = 1e-5),
  metrics = c("accuracy")
)
history <- model %>% fit_generator(
  train_generator,
  steps_per_epoch = 100,
  epochs = 100,
  validation_data = validation_generator,
  validation_steps = 50
)


#Listing 5.24. Preprocessing a single image
img_path <- "~/Downloads/cats_and_dogs_small/test/cats/cat.1700.jpg"
img <- image_load(img_path, target_size = c(150, 150))                 
img_tensor <- image_to_array(img)
img_tensor <- array_reshape(img_tensor, c(1, 150, 150, 3))
img_tensor <- img_tensor / 255                                         
dim(img_tensor)     



#Listing 5.25. Displaying the test picture
plot(as.raster(img_tensor[1,,,]))


#Listing 5.26. Instantiating a model from an input tensor and a list of output tensors
layer_outputs <- lapply(model$layers[1:8], function(layer) layer$output)      
activation_model <- keras_model(inputs = model$input, outputs = layer_outputs)



#Listing 5.27. Running the model in predict mode
activations <- activation_model %>% predict(img_tensor) 



#Listing 5.28. Function to plot a channel
plot_channel <- function(channel) {
  rotate <- function(x) t(apply(x, 2, rev))
  image(rotate(channel), axes = FALSE, asp = 1,
        col = terrain.colors(12))
}


#Listing 5.29. Plotting the second channel
plot_channel(first_layer_activation[1,,,2])


#Listing 5.30. Visualizing the seventh channel
plot_channel(first_layer_activation[1,,,7])



#Listing 5.31. Visualizing every channel in every intermediate activation
image_size <- 58
images_per_row <- 16

for (i in 1:8) {
  
  layer_activation <- activations[[i]]
  layer_name <- model$layers[[i]]$name
  
  n_features <- dim(layer_activation)[[4]]
  n_cols <- n_features %/% images_per_row
  
  png(paste0("cat_activations_", i, "_", layer_name, ".png"),
      width = image_size * images_per_row,
      height = image_size * n_cols)
  op <- par(mfrow = c(n_cols, images_per_row), mai = rep_len(0.02, 4))
  
  for (col in 0:(n_cols-1)) {
    for (row in 0:(images_per_row-1)) {
      channel_image <- layer_activation[1,,,(col*images_per_row) + row + 1]
      plot_channel(channel_image)
    }
  }
  
  par(op)
  dev.off()
}



#Listing 5.32. Defining the loss tensor for filter visualization
library(keras)
model <- application_vgg16(
  weights = "imagenet",
  include_top = FALSE
)
layer_name <- "block3_conv1"
filter_index <- 1
layer_output <- get_layer(model, layer_name)$output
loss <- k_mean(layer_output[,,,filter_index])


#Listing 5.33. Obtaining the gradient of the loss with regard to the input
grads <- k_gradients(loss, model$input)[[1]] 


#Listing 5.34. Gradient-normalization trick
grads <- grads / (k_sqrt(k_mean(k_square(grads))) + 1e-5) 



#Listing 5.35. Fetching output values given input values
iterate <- k_function(list(model$input), list(loss, grads))
c(loss_value, grads_value) %<-%
  iterate(list(array(0, dim = c(1, 150, 150, 3))))



#Listing 5.36. Loss maximization via stochastic gradient descent
input_img_data <-                                                     
array(runif(150 * 150 * 3), dim = c(1, 150, 150, 3)) * 20 + 128     
step <- 1
for (i in 1:40) {                                                     
  c(loss_value, grads_value) %<-% iterate(list(input_img_data))       
  input_img_data <- input_img_data + (grads_value * step)             
}


#Listing 5.37. Utility function to convert a tensor into a valid image
deprocess_image <- function(x) {
  dms <- dim(x)
  x <- x - mean(x)                        
  x <- x / (sd(x) + 1e-5)                 
  x <- x * 0.1                            
  x <- x + 0.5                            
  x <- pmax(0, pmin(x, 1))                
  array(x, dim = dms)                     
}


#Listing 5.38. Function to generate filter visualizations
generate_pattern <- function(layer_name, filter_index, size = 150) {
  layer_output <- model$get_layer(layer_name)$output                      
  loss <- k_mean(layer_output[,,,filter_index])                           
  grads <- k_gradients(loss, model$input)[[1]]                            
  grads <- grads / (k_sqrt(k_mean(k_square(grads))) + 1e-5)               
  iterate <- k_function(list(model$input), list(loss, grads))             
  input_img_data <-                                                       
  array(runif(size * size * 3), dim = c(1, size, size, 3)) * 20 + 128   
  step <- 1                                                               
  for (i in 1:40) {                                                       
    c(loss_value, grads_value) %<-% iterate(list(input_img_data))         
    input_img_data <- input_img_data + (grads_value * step)               
  }                                                                       
  img <- input_img_data[1,,,]
  deprocess_image(img)
}


#Listing 5.39. Generating a grid of all filter response patterns in a layer
library(grid)
library(gridExtra)
dir.create("vgg_filters")
for (layer_name in c("block1_conv1", "block2_conv1",
                     "block3_conv1", "block4_conv1")) {
  size <- 140
  
  png(paste0("vgg_filters/", layer_name, ".png"),
      width = 8 * size, height = 8 * size)
  
  grobs <- list()
  for (i in 0:7) {
    for (j in 0:7) {
      pattern <- generate_pattern(layer_name, i + (j*8) + 1, size = size)
      grob <- rasterGrob(pattern,
                         width = unit(0.9, "npc"),
                         height = unit(0.9, "npc"))
      grobs[[length(grobs)+1]] <- grob
    }
  }
  
  grid.arrange(grobs = grobs, ncol = 8)
  dev.off()
}



#Listing 5.40. Loading the VGG16 network with pretrained weights
model <- application_vgg16(weights = "imagenet")


#Listing 5.41. Preprocessing an input image for VGG16
img_path <- "~/Downloads/creative_commons_elephant.jpg"              
img <- image_load(img_path, target_size = c(224, 224)) %>%           
image_to_array() %>%                                               
array_reshape(dim = c(1, 224, 224, 3)) %>%                         
imagenet_preprocess_input()


#Listing 5.42. Setting up the Grad-CAM algorithm
african_elephant_output <- model$output[, 387]                             
last_conv_layer <- model %>% get_layer("block5_conv3")                     
grads <- k_gradients(african_elephant_output, last_conv_layer$output)[[1]] 
pooled_grads <- k_mean(grads, axis = c(1, 2, 3))                           
iterate <- k_function(list(model$input),                                   
                      list(pooled_grads, last_conv_layer$output[1,,,]))
c(pooled_grads_value, conv_layer_output_value) %<-% iterate(list(img))     
for (i in 1:512) {                                                         
  conv_layer_output_value[,,i] <-
    conv_layer_output_value[,,i] * pooled_grads_value[[i]]
}
heatmap <- apply(conv_layer_output_value, c(1,2), mean) 


#Listing 5.43. Heatmap post-processing
heatmap <- pmax(heatmap, 0)
heatmap <- heatmap / max(heatmap)                                          
write_heatmap <- function(heatmap, filename, width = 224, height = 224,    
                          bg = "white", col = terrain.colors(12)) {
  png(filename, width = width, height = height, bg = bg)
  op = par(mar = c(0,0,0,0))
  on.exit({par(op); dev.off()}, add = TRUE)
  rotate <- function(x) t(apply(x, 2, rev))
  image(rotate(heatmap), axes = FALSE, asp = 1, col = col)
}
write_heatmap(heatmap, "elephant_heatmap.png")



#Listing 5.44. Superimposing the heatmap with the original picture
library(magick)
library(viridis)
image <- image_read(img_path)                                      
info <- image_info(image)
geometry <- sprintf("%dx%d!", info$width, info$height)
pal <- col2rgb(viridis(20), alpha = TRUE)                          
alpha <- floor(seq(0, 255, length = ncol(pal)))
pal_col <- rgb(t(pal), alpha = alpha, maxColorValue = 255)
write_heatmap(heatmap, "elephant_overlay.png",
              width = 14, height = 14, bg = NA, col = pal_col)
image_read("elephant_overlay.png") %>%                             
image_resize(geometry, filter = "quadratic") %>%
  image_composite(image, operator = "blend", compose_args = "20") %>%
  plot()























































