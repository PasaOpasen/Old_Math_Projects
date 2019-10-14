#include <iostream> 
#include <math.h> 
#include <ctime> 
using namespace std; 
int main () 
{ 
double Sum,SumP,SumSinP; 
int timeSum,timeSumP,timeSumSinP; 
int n; 
float scan; 
double a[100000]; 
double b[100000]; 
double timeBegin,timeEnd;

FILE *po = fopen("in.txt", "r"); 



fscanf(po, "%d", &n); 
for (int i = 0; i < n; i++) fscanf(po, "%3d", &a[i]);
for (int i = 0; i < n; i++) fscanf(po, "%3d", &b[i]); 


FILE *pi = fopen("out.txt", "w"); 

fprintf(pi, "искомые значения : "); 
fprintf(pi, "\n a + b = "); 
fprintf(pi, "\n"); 

for (int i = 0; i <n; i++) fprintf(pi, "%6d", a[i] + b[i]); 
 

fprintf(pi, "\n a*b= "); 
fprintf(pi, "\n"); 
for (int i = 0; i <n; i++) fprintf(pi, "%5d", a[i] * b[i]); 
 

fprintf(pi, "\n arccos(a * b) = "); 
fprintf(pi, "\n"); 
for (int i = 0; i <n; i++) fprintf(pi, "%5.1f", acos(a[i]*b[i])); 
 
 
timeBegin = clock(); 
Sum=0; 
for (int i=0;i<n;i++) 
Sum+=(a[i]+b[i]); 
timeEnd = clock(); 
timeSum=timeEnd-timeBegin; 

timeBegin = clock(); 
SumP=0; 
for (int i=0;i<n;i++) 
SumP+=(a[i]*b[i]); 
timeEnd = clock(); 
timeSumP=timeEnd-timeBegin; 

timeBegin = clock(); 
SumSinP=0; 
for (int i=0;i<n;i++) 
SumSinP+=acos(a[i]*b[i]); 
timeEnd = clock(); 
timeSumSinP=timeEnd-timeBegin; 




fprintf(pi," sum of sumes %12.2f %4d\n",Sum,timeSum); 
fprintf(pi," sum of powers %12.2f %4d\n",SumP,timeSumP); 
fprintf(pi," sum of functions %12.2f %4d\n",SumSinP,timeSumSinP); 
fclose(po); 
fclose(pi); 
return 0; 
}
