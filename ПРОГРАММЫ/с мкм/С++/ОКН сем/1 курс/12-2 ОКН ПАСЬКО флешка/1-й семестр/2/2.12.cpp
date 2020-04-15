#include <iostream> 
#include <math.h> 
#include <ctime> 
using namespace std;

char pust[100]; 
double Sum,SumP,SumSinP; 
float timeSum,timeSumP,timeSumSinP; 
int n; 
float scan; 
double a[10000000];
double a1,b1; 
double b[10000000]; 
double timeBegin,timeEnd;
FILE *po = fopen("in.txt", "r"); 
FILE *pi = fopen("out.txt", "w"); 

void input()
{


fscanf(po,"%s",pust);
fscanf(po,"%d", &n);

fscanf(po,"%s",pust);
fscanf(po, "%f", &scan);a[0]=scan;
fscanf(po, "%f", &scan);a1=scan;
for (int i = 1; i < n; i++) {a[i]=a[i-1]+a1;}
fscanf(po,"%s",pust);
fscanf(po, "%f", &scan);b[0]=scan;
fscanf(po, "%f", &scan);b1=scan;
for (int i = 1; i < n; i++) {b[i]=b[i-1]+b1;}

}


void output()
{

fprintf(pi, "искомые значения : "); 
fprintf(pi, "\n a + b = "); 
fprintf(pi, "\n"); 

for (int i = 0; i <10; i++) fprintf(pi, "%5f ", a[i] + b[i]); 
 

fprintf(pi, "\n a * b = "); 
fprintf(pi, "\n"); 
for (int i = 0; i <10; i++) fprintf(pi, "%5f ", a[i] * b[i]); 
 

fprintf(pi, "\n arccos(a * b) = "); 
fprintf(pi, "\n"); 
for (int i = 0; i < 10; i++) fprintf(pi, "%5f ", acos(a[i]*b[i]));  

} 



void time()
{

   
timeBegin = clock(); 
for(int j=1;j<=100000;j++)
{ Sum=0.0; 
for (int i=0;i<n;i++) 
Sum+=(a[i]+b[i]);
} 
timeEnd = clock(); 
timeSum = timeEnd-timeBegin; 


timeBegin = clock();
for(int j=1;j<=100000;j++)
{ 
SumP=0.0; 
for (int i=0;i<n;i++) 
SumP+=(a[i]*b[i]);
} 
timeEnd = clock(); 
timeSumP=timeEnd-timeBegin; 

timeBegin = clock();
for(int j=1;j<=100000;j++)
{ 
SumSinP=0.0; 
for (int i=0;i<n;i++) 
SumSinP+=acos(a[i]*b[i]);
} 
timeEnd = clock(); 
timeSumSinP=timeEnd-timeBegin; 


fprintf(pi,"time of sum of sumes %12.8f %4.14f   \n",Sum,timeSum); 
fprintf(pi,"time of sum of powers %12.8f %4.14f   \n",SumP,timeSumP); 
fprintf(pi,"time of sum of functions %12.8f %4.14f   \n",SumSinP,timeSumSinP); 
} 



int main () 
{
input();
time();
output();

fclose(po); 
fclose(pi);
return 0; 
}
