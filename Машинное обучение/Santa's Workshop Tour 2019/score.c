
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>

#define NF 5000
int cost[NF][101];
int fs[NF];

int cf[NF][10];

int loaded=0;

float acc[301][301];

void precompute_acc() {
    
for(int i=125;i<=300;i++) 
    for(int j=125;j<=300;j++)
      acc[i][j] = (i-125.0)/400.0 * pow(i , 0.5 + fabs(i-j) / 50 );    
}

void read_fam() {
  FILE *f;
  char s[1000];
  int d[101],fid,n;
  int *c;

  f=fopen("family_data.csv","r");
  if (fgets(s,1000,f)==NULL)
    exit(-1);

  for(int i=0;i<5000;i++) {
    c = &cf[i][0];
    if (fscanf(f,"%d,%d,%d,%d,%d,%d,%d,%d,%d,%d,%d,%d",
               &fid,&c[0],&c[1],&c[2],&c[3],&c[4],&c[5],&c[6],&c[7],&c[8],&c[9],&fs[i])!=12)
      exit(-1);

    //    printf("%d,%d,%d,%d,%d,%d,%d,%d,%d,%d,%d,%d\n",
    //fid,c[0],c[1],c[2],c[3],c[4],c[5],c[6],c[7],c[8],c[9],fs[i]);
    n = fs[i];

    for(int j=1;j<=100;j++) {
      if (j==c[0]) cost[i][j]=0;
      else if (j==c[1]) cost[i][j]=50;
      else if (j==c[2]) cost[i][j]=50 + 9 * n;
      else if (j==c[3]) cost[i][j]=100 + 9 * n;
      else if (j==c[4]) cost[i][j]=200 + 9 * n;
      else if (j==c[5]) cost[i][j]=200 + 18 * n;
      else if (j==c[6]) cost[i][j]=300 + 18 * n;
      else if (j==c[7]) cost[i][j]=300 + 36 * n;
      else if (j==c[8]) cost[i][j]=400 + 36 * n;
      else if (j==c[9]) cost[i][j]=500 + 36 * n + 199 * n;
      else cost[i][j]=500 + 36 * n + 398 * n;
    }
  }

}

float max_cost=1000000000;

int day_occ[102];

static inline int day_occ_ok(int d) {
  return !(d <125 || d>300);
}

float score(int *pred) {
  float r=0;
    
  if (!loaded) {
      read_fam();
      precompute_acc();
      loaded = 1;
  }

  // validate day occupancy
  memset(day_occ,0,101*sizeof(int));

  for(int i=0;i<NF;i++) {
    day_occ[pred[i]]+=fs[i];
    r+=cost[i][pred[i]];
  }
       
  day_occ[101]=day_occ[100];

  for (int d=1;d<=100;d++) {
    if (day_occ[d]<125)                                                       
      r += 100000 * (125 - day_occ[d]);                                      
    else if (day_occ[d] > 300)                                               
      r += 100000 * (day_occ[d] - 300);      
    r += acc[day_occ[d]][day_occ[d+1]];
  }
  return r;
} 






