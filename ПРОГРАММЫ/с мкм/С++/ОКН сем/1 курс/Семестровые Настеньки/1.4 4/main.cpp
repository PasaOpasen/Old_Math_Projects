#include <cstdlib>
#include <iostream>
#include <stdio.h>
#include <math.h>
using namespace std;


FILE *fp=fopen("in.txt","r");FILE *fpp=fopen("out.txt","w");
int m[101];char pust[100];
int i;
float e;
double a,b,c;
void input();
	void go();
	void output();
double f(double x);

int main()
{

	input();
	go();
	output();


	fclose(fp); 
	fclose(fpp); 
	return 0;
}

void input()
{

	fscanf(fp,"%s",pust);	fscanf(fp,"%s",pust);

    fscanf(fp,"%f",&e);a=e;
	fscanf(fp,"%s",pust);
    fscanf(fp,"%f",&e);b=e;fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
    fscanf(fp,"%f",&e);
}

void go()
{
while(b-a >e)
{c=b-(b-a)/2;
if(f(c)*f(a)<0) b=c;
else a=c;
}

//c/=2;
}



void output()
{
	//fprintf(fpp,"%f ",a);fprintf(fpp,"%f ",b);fprintf(fpp,"%f ",c);fprintf(fpp,"%f ",e);
  fprintf(fpp,"корень \n");
  fprintf(fpp,"%f \n",c);
  fprintf(fpp,"значение функции \n");
    fprintf(fpp,"%f ",f(c));
  
}

double f(double x)
{return x*x-cos(x);
}

