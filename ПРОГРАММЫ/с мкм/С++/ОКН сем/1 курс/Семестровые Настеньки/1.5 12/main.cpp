#include <cstdlib>
#include <iostream>
#include <stdio.h>
#include <math.h>
//#include <cmath>
using namespace std;


FILE *fp=fopen("in.txt","r");FILE *fpp=fopen("out.txt","w");
double m[101];char pust[100];
float scan;
double a,b,c;
double f(double x);

int main()
{
	fprintf(fpp,"моя         библиотека          погрешность\n");
	for (int i=0;i<10;i++) 
	{fscanf(fp,"%f",&scan); a=scan;
	fprintf(fpp,"x=  ");fprintf(fpp,"%f  \n",a);
	
	fprintf(fpp,"%f      %f          %f \n",f(a), atanh(a), f(a)-atanh(a));fprintf(fpp,"  \n");
	}
  

	fclose(fp); 
	fclose(fpp); 
	return 0;
}


double f(double x)
{double t=0;
for (int i=1; i< 10;i++) t+=pow(x,2*i-1)/(2*i-1);
return t;
}

