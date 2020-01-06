
library(tidyverse)

fpath = 'family_data.csv'
data = read.csv(fpath)
rownames(data) <- data$family_id

fpath = 'sample_submission.csv'
submission = read.csv(fpath)
rownames(submission) <- submission$family_id

N_DAYS = 100
MAX_OCCUPANCY = 300
MIN_OCCUPANCY = 125


cost_function <- function(prediction){
  
  penalty = 0
  
  # We'll use this to count the number of people scheduled each day
  #daily_occupancy = rep(0, times = N_DAYS)
  
  pd <- prediction %>% inner_join(data, by = "family_id")
  
  for(fc in 0:9) pd[,paste0("choice_",fc)] <- pd[,paste0("choice_",fc)] == pd[,"assigned_day"]
  pd[,"choice_10"] <- rowSums(pd[,paste0("choice_",0:9)])==0
  pd %>% 
    rename(n = n_people) %>%
    mutate(c0 = 0, c1 = 50*choice_1, c2 = choice_2*(50+9*n), c3 = choice_3*(100+9*n), c4= choice_4*(200+9*n), c5 = choice_5*(200+18*n), c6 = choice_6*(300+18*n), c7 = choice_7*(300+36*n), c8=choice_8*(400+36*n),c9 =choice_9*(500+36*n+199*n), c10 = choice_10*(500+36*n+398*n)) %>%
    select(15:25) %>%
    summarize(res = sum(c0)+sum(c1)+sum(c2)+sum(c3)+sum(c4)+sum(c5)+sum(c6)+sum(c7)+sum(c8)+sum(c9)+sum(c10)) %>%
    pull -> penalty
  
  pd$assigned_day <- factor(prediction$assigned_day, levels = 1:100, labels = 1:100)
  
  penalty <- penalty + acc_cost(pd %>% group_by(assigned_day) %>% summarize(s = sum(n_people)) %>% select(s) %>% pull)
  return(penalty)
}


acc_cost <- function(daily_occupancy){
  # for each date, check total occupancy
  #  (using soft constraints instead of hard constraints)
  if (sum(daily_occupancy> MAX_OCCUPANCY)+ sum(daily_occupancy< MIN_OCCUPANCY))
    penalty2 <- 100000000
  else
    penalty2 <- 0
  
  # Calculate the accounting cost
  # The first day (day 100) is treated special
  accounting_cost <- (daily_occupancy[100]-125.0) / 400.0 * daily_occupancy[100]^(0.5)
  # using the max function because the soft constraints might allow occupancy to dip below 125
  accounting_cost <- max(0, accounting_cost)
  
  # Loop over the rest of the days, keeping track of previous count
  yesterday_count = daily_occupancy[100]
  for (day in 99:1){
    today_count <- daily_occupancy[day]
    diff <- abs(today_count - yesterday_count)
    accounting_cost <- accounting_cost + max(0, (daily_occupancy[day]-125.0) / 400.0 * daily_occupancy[day]^(0.5 + diff / 50.0))
    yesterday_count <- today_count
  }
  return(accounting_cost+penalty2)
}


sapply(2:8,function(n){
  sapply(0:10, function(ch){
    if(ch == 0) return(0)
    if(ch == 1) return(50)
    if(ch == 2) return(50+9*n)
    if(ch == 3) return(100+9*n)
    if(ch == 4) return(200+9*n)
    if(ch == 5) return(200+18*n)
    if(ch == 6) return(300+18*n)
    if(ch == 7) return(300+36*n)
    if(ch == 8) return(400+36*n)
    if(ch == 9) return(500+36*n+199*n)
    if(ch == 10) return(500+36*n+398*n)  
  })
}) -> nch
rownames(nch) <- 0:10
colnames(nch) <- 2:8



opt_pred <-function(cur_pred,ntimes = 1){
  #cur_pred <- submission
  # rownames(cur_pred) <- cur_pred$family_id
  
  cur_score <- cost_function(cur_pred)
  
  cur_pd <- cur_pred %>% inner_join(data, by = "family_id")
  for(fc in 0:9) cur_pd[,paste0("cchoice_",fc)] <- (cur_pd[,paste0("choice_",fc)] == cur_pd[,"assigned_day"])*fc
  cur_pd[,"cchoice_10"] <- (rowSums(cur_pd[,paste0("cchoice_",0:9)])==0)*10
  cur_pd[cur_pd[,paste0("choice_",0)] == cur_pd[,"assigned_day"],"cchoice_10"] <- 0
  cur_pd$cur_choice <- as.character(rowSums(cur_pd[,paste0("cchoice_",0:10)]))
  
  cur_pd$assigned_day <- factor(cur_pred$assigned_day, levels = 1:100,labels = 1:100)
  cur_daily_occ <- cur_pd %>% group_by(assigned_day) %>% summarize(s = sum(n_people)) %>% select(s) %>% pull
  cur_acc_cost <- acc_cost(cur_daily_occ)
  cur_penalty <- cur_score - cur_acc_cost
  
  ac <- as.character(0:9)
  # loop ntimes over each family 
  for(fn in 1:ntimes){
    cur_epoch_score <- cur_score
    cur_epoch_start <- Sys.time()
    for (fi in 1:nrow(cur_pred)){
      # loop over each family choice
      f <- as.character(cur_pred[fi,"family_id"])
      fcur_pd <- subset(cur_pd,family_id == f)
      fcur_choice <- fcur_pd[1,"cur_choice"]
      fcur_day <- fcur_pd[1,"assigned_day"]
      fcur_ni <- fcur_pd[1,"n_people"]
      fcur_n <- as.character(fcur_ni)
      fcur_penalty <- nch[fcur_choice,fcur_n]
      
      #temp_daily_occ
      tdo <- cur_daily_occ
      tdo[fcur_day] <- tdo[fcur_day]-fcur_ni
      
      f_scores <- sapply(ac,function(pick){
        #if(pick == fcur_choice) 
        #    {return(cur_score)}
        #else{
        
        temp_penalty <- cur_penalty - fcur_penalty + nch[pick,fcur_n]
        
        ftemp_day <- fcur_pd[1,paste0("choice_",pick)]
        tdo[ftemp_day] <- tdo[ftemp_day]+fcur_ni
        temp_acc_cost <- acc_cost(tdo)
        
        temp_score <- temp_penalty + temp_acc_cost 
        return(temp_score)
        #}
      })
      # update new choice
      
      
      fnew_choice <- names(which.min(f_scores))
      
      if(f_scores[fnew_choice] < cur_score){
        cpff <- which(cur_pd$family_id == f)
        cur_pd[cpff,"cur_choice"] <- fnew_choice
        
        fnew_day <- fcur_pd[1,paste0("choice_",fnew_choice)]
        cur_pd[cpff,"assigned_day"] <- fnew_day
        
        cur_daily_occ <- tdo
        cur_daily_occ[fnew_day] <- cur_daily_occ[fnew_day] + fcur_ni
        cur_score <- f_scores[fnew_choice]
        
        cur_penalty <- cur_penalty - fcur_penalty + nch[fnew_choice,fcur_n]
      }
      #if(cur_score!=cost_function(cur_pd %>% select(family_id,assigned_day))) break()
    }
    print(paste("epoch:",fn,"; score:",cur_score))
    print(Sys.time()-cur_epoch_start)
    if(cur_score == cur_epoch_score) break()
    cur_epoch_score <- cur_score
  }
  
  res <- list()
  res[[1]] <- cur_pd %>% select(family_id,assigned_day)
  res[[2]] <- cur_score
  return(res)
}

set.seed(123)
n <- 5000
m <- 5000
x <- 1:n
n_Fi = sample(x, m, replace=F)
opt_pred_2 <-function(cur_pred,ntimes = 1){
  
  cur_score <- cost_function(cur_pred)
  
  cur_pd <- cur_pred %>% inner_join(data, by = "family_id")
  for(fc in 0:9) cur_pd[,paste0("cchoice_",fc)] <- (cur_pd[,paste0("choice_",fc)] == cur_pd[,"assigned_day"])*fc
  cur_pd[,"cchoice_10"] <- (rowSums(cur_pd[,paste0("cchoice_",0:9)])==0)*10
  cur_pd[cur_pd[,paste0("choice_",0)] == cur_pd[,"assigned_day"],"cchoice_10"] <- 0
  cur_pd$cur_choice <- as.character(rowSums(cur_pd[,paste0("cchoice_",0:10)]))
  
  cur_pd$assigned_day <- factor(cur_pred$assigned_day, levels = 1:100,labels = 1:100)
  cur_daily_occ <- cur_pd %>% group_by(assigned_day) %>% summarize(s = sum(n_people)) %>% select(s) %>% pull
  cur_acc_cost <- acc_cost(cur_daily_occ)
  cur_penalty <- cur_score - cur_acc_cost
  
  ac <- as.character(0:9)
  
  # loop ntimes over each family 
  for(fn in 1:ntimes){
    cur_epoch_score <- cur_score
    cur_epoch_start <- Sys.time()
    for (fi in n_Fi){ 
      # use the random number created above
      # loop over each family choice
      f <- as.character(cur_pred[fi,"family_id"])
      fcur_pd <- subset(cur_pd,family_id == f)
      fcur_choice <- fcur_pd[1,"cur_choice"]
      fcur_day <- fcur_pd[1,"assigned_day"]
      fcur_ni <- fcur_pd[1,"n_people"]
      fcur_n <- as.character(fcur_ni)
      fcur_penalty <- nch[fcur_choice,fcur_n]
      
      #temp_daily_occ
      tdo <- cur_daily_occ
      tdo[fcur_day] <- tdo[fcur_day]-fcur_ni
      
      f_scores <- sapply(ac,function(pick){
        
        temp_penalty <- cur_penalty - fcur_penalty + nch[pick,fcur_n]
        
        ftemp_day <- fcur_pd[1,paste0("choice_",pick)]
        tdo[ftemp_day] <- tdo[ftemp_day]+fcur_ni
        temp_acc_cost <- acc_cost(tdo)
        
        temp_score <- temp_penalty + temp_acc_cost 
        return(temp_score)
        
      })
      
      # update new choice
      
      f_scores = f_scores[which(cur_score > f_scores)]
      fnew_choice <- names(which.max(f_scores))
      # optimise f_scores into one better choice at a time
      
      if(length(f_scores)==0){}
      else if(f_scores[fnew_choice] < cur_score){
        cpff <- which(cur_pd$family_id == f)
        cur_pd[cpff,"cur_choice"] <- fnew_choice
        
        fnew_day <- fcur_pd[1,paste0("choice_",fnew_choice)]
        cur_pd[cpff,"assigned_day"] <- fnew_day
        
        cur_daily_occ <- tdo
        cur_daily_occ[fnew_day] <- cur_daily_occ[fnew_day] + fcur_ni
        cur_score <- f_scores[fnew_choice]
        
        cur_penalty <- cur_penalty - fcur_penalty + nch[fnew_choice,fcur_n]
      }
      
    }
    print(paste("epoch:",fn,"; score:",cur_score))
    print(Sys.time()-cur_epoch_start)
    if(cur_score == cur_epoch_score) break()
    cur_epoch_score <- cur_score
  }
  
  res <- list()
  res[[1]] <- cur_pd %>% select(family_id,assigned_day)
  res[[2]] <- cur_score
  return(res)
}


paths=list.files("samples")
for(pth in paths){
  fpath = pth
gab = read.csv(paste0(getwd(),"/samples/",fpath))
#gab$assigned_day=sample(1:100,5000,replace = T) %>% matrix(ncol=1)
rownames(gab) <- submission$family_id

opt_gab <- opt_pred_2(gab,1000)
opt_score <- opt_gab[[2]]
print(opt_score)


if(opt_score<69467) write.csv(opt_gab[[1]],file =paste("submission",round(opt_score,2),".csv"),quote = F,row.names = F)
}

##################################################
    fpath = "res.csv"
    gab = read.csv(fpath)
    rownames(gab) <- submission$family_id
    best.result=gab$assigned_day

test=function(batch=100,count=100){
  for(pth in seq(count)){

    gab$assigned_day[sample(1:5000,batch)]=sample(1:100,batch,replace = T)
    while(score(gab$assigned_day)>=1e20){
      gab$assigned_day=best.result
      gab$assigned_day[sample(1:5000,batch)]=sample(1:100,batch,replace = T)
      cat("------minifail \n")
    }
    
    opt_gab <- opt_pred(gab,100)
    opt_score <- opt_gab[[2]]
    print(opt_score)
    
    write.csv(opt_gab[[1]],file =paste("submission",round(opt_score),".csv"),quote = F,row.names = F)
    
    if(score(best.result)>score(gab$assigned_day)){
      best.result=gab$assigned_day
      cat("----------LUCK \n")
    }else{
      gab$assigned_day=best.result
      cat("----------FAILURE \n")
    }
  }
  0
}

test(50,10)









