#include <cstdlib>
#include <iostream>
#include <stdio.h>
#include <math.h>
using namespace std;

double	a,b,c,d;
int h,w,n,i,j,k;
double *A,*y,*f,*A1,*f1,*f0,*A0;
double m;
char pust[100];
float  scan;
FILE *fp=fopen("in.txt","r");	
FILE *pf=fopen("out.txt","w");
void Input();
void GJ1();void GJ2();
void Output();

#define A(i,j) A[n*(i-1)+j]
#define A1(i,j) A1[n*(i-1)+j]

int main()
{
	Input();
	GJ1();GJ2();
	Output();	


	fclose(fp);
	fclose(pf);		
}



void Input()
{				
fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
fscanf(fp,"%d",&n);
A = new double [n*n+1];A1 = new double [n*n+1];A0 = new double [n*n+1];
y = new double [n+1];
f = new double [n+1];f1 = new double [n+1];f0 = new double [n+1];

fscanf(fp,"%s",pust);
for (i=1;i<=n*n;i++)
{
	fscanf(fp,"%f",&scan);
	A[i]=scan;
	A1[i]=scan;
}

fscanf(fp,"%s",pust);
for (i=1;i<=n;i++)
{
	fscanf(fp,"%f",&scan);
	f[i]=scan;f1[i]=scan;
}


int k=0;
fprintf(pf,"матрица А: \n");
for (i=1;i<=n*n;i++)
{k++;
fprintf(pf,"%f ",A[i]);
if(k==10){
	fprintf(pf," \n");k=0;
}
}
fprintf(pf," \n");



fprintf(pf," \n");
fprintf(pf,"столбец значений: \n");
for (i=1;i<=n;i++)
{
	fprintf(pf,"%f \n",f[i]);
}
fprintf(pf," \n");fprintf(pf," \n");
}



void GJ1()
{ 

for(j=1;j<=n;j++)
	{
		double max = fabs(A1(j,j));
	    int k=j;
	    
	    for(i=j+1;i<n;i++)
	    if (fabs(A1(i,j))>max)
	    {k=i;
	     max =fabs(A1(i,j));
	    }
	    
	    
	    for(h=1;h<n;h++)
	    {c = A1(j,h);
	    A1(j,h)=A1(k,h);
	    A1(k,h)=c;
	    }
	    c=f1[j];
	    f1[j]=f1[k];
	    f1[k]=c;	
    
	for(i=j+1;i<=n;i++)
	{m=A1(i,j)/A1(j,j);
	f1[i]=f1[i]-(f1[j])*m;
	for(h=j;h<=n;h++)A1(i,h)=A1(i,h)-(A1(j,h))*A1(i,j)/A1(j,j);
	 //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! m
	
	} 
	
    }
	


	fprintf(pf," \n");fprintf(pf," \n");
	k=0;
	fprintf(pf,"матрица ступенчатая (первый ход): \n");
	for (i=1;i<=n*n;i++)
	{k++;
	fprintf(pf,"%f ",A1[i]);
	if(k==10){
		fprintf(pf," \n");k=0;
	}
	}
	fprintf(pf," \n");
	fprintf(pf," \n");
	fprintf(pf,"её столбец значений: \n");
	for (i=1;i<=n;i++)
	{
		fprintf(pf,"%f\n",f1[i]);
	}
	fprintf(pf," \n");fprintf(pf," \n");
}

void GJ2()
{

	for(j=n;j>=1;j--)
	{  for(i=j-1;i>=1;i--)
		{m=A1(i,j)/A1(j,j);
		 f1[i]=f1[i]-f1[j]*m;
		 A1(i,j)=A1(i,j)-m*(A1(j,j));
         
		}

	}


	fprintf(pf," \n");fprintf(pf," \n");
	k=0;
	fprintf(pf,"матрица конечная (второй ход): \n");
	for (i=1;i<=n*n;i++)
	{k++;
	fprintf(pf,"%f ",A1[i]);
	if(k==10){
		fprintf(pf," \n");k=0;
	}
	}
	fprintf(pf," \n");
	fprintf(pf," \n");
	fprintf(pf,"её столбец значений: \n");
	for (i=1;i<=n;i++)
	{
		fprintf(pf,"%f\n",f1[i]);
	}
	fprintf(pf," \n");fprintf(pf," \n");


	for(i=1;i<=n;i++) y[i]=f1[i]/A1(i,i);

	fprintf(pf,"столбец решений: \n");
	for (i=1;i<=n;i++)
	{
		fprintf(pf,"%f\n",y[i]);
	}
} 

//whhy
void Output()
{
fprintf(pf," \n");

int t;
double q;
double s=0;
double norm[n];
double c[n];

for (i=1;i<=n;i++)
{c[i]=0;
for (t=1;t<=n;t++)
{
	c[i]+= (A1(i,t))*y[t];
}

norm[i]= c[i]-f1[i];
s+=(norm[i])*(norm[i]);
}

q=sqrt(s);
fprintf(pf," \n");
fprintf(pf,"невязка: \n");	
fprintf(pf,"%f ",q);
	}
