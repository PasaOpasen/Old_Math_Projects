#include <cstdlib>
#include <iostream>
#include <stdio.h>
#include <math.h>
#include "Graph.h"
using namespace std;

double eps;int	nRis,n;
int i,j,k,maxI,num=0;
double *A,*y,*f,*func,*p;
char pust[100];
float  scan;
FILE *fp=fopen("in.txt","r");	
FILE *pf=fopen("out.txt","w");
void Input();
void Jak();double Nev();
void Output();
void Ris();double ip(double x);

#define A(i,j) A[n*(i-1)+j]

int main()
{
	Input();
	Jak();
   Output();	
   Ris();	


	fclose(fp);
	fclose(pf);		
}



void Input()
{				
fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
fscanf(fp,"%d",&n);
A = new double [n*n+1];
y = new double [n+1];
f = new double [n+1];

fscanf(fp,"%s",pust);
for (i=1;i<=n*n;i++)
{
	fscanf(fp,"%f",&scan);
	A[i]=scan;
}

fscanf(fp,"%s",pust);
for (i=1;i<=n;i++)
{
	fscanf(fp,"%f",&scan);
	f[i]=scan;
}

fscanf(fp,"%s",pust);
for (i=1;i<=n;i++)
{
	fscanf(fp,"%f",&scan);
	y[i]=scan;
}

fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
fscanf(fp,"%f",&scan); eps = scan;

fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
fscanf(fp,"%f",&scan); maxI = (int)scan;

func = new double [maxI+1];p = new double [maxI+1];

int k=0;
fprintf(pf,"матрица ј: \n");
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


fprintf(pf," \n");
fprintf(pf,"столбец приближени€: \n");
for (i=1;i<=n;i++)
{
	fprintf(pf,"%f \n",y[i]);
}
fprintf(pf," \n");fprintf(pf," \n");

fprintf(pf,"точность %f \n",eps);
fprintf(pf,"изначальна€ нев€зка %f \n",Nev());
//fprintf(pf,"точность %d \n",maxI);
}



void Jak()
{double E;
	while((Nev()>eps)&(num <maxI))
	{func[num]=Nev();
	for(i=1;i<=n;i++)
	 {E=0;
	   for(j=1;j<=n;j++)
	   {z1:
	     if(j==i)
	      {j++;
	      goto z1;
	      }
	     else
		 {E+=A(i,j)*y[j];
		 } 
	   }	
	 y[i]=(f[i]-E)/A(i,i);
	 }
	 num++;
	 
	}
}




double Nev()
{int t;
double q;
double s=0;
double norm[n];
double c[n];
for (i=1;i<=n;i++)
{c[i]=0;
for (t=1;t<=n;t++)
{
	c[i]+= (A(i,t))*y[t];
}

norm[i]= c[i]-f[i];
s+=(norm[i])*(norm[i]);
}

q=sqrt(s);
return q;
}


void Output()
{
fprintf(pf," \n");fprintf(pf," \n");
fprintf(pf," \n");fprintf(pf," \n");fprintf(pf," \n");
fprintf(pf,"столбец решений: \n");
for (i=1;i<=n;i++)
{
	fprintf(pf,"%f \n",y[i]);
}
fprintf(pf," \n");fprintf(pf," \n");
fprintf(pf,"при номере итерации %d \n",num);
fprintf(pf,"и нев€зке  %f \n",Nev());

}



void Ris()
{  double a=0, b=num,c=0,d=200;
	SetColor(250,250,250);						// задаем фоновый цвет окна
	SetWindow(a,b,c,d);	
	nRis = 250;						// создаЄм окно (создаем массивы R,G,B)
	// с пределами [a,b]x[c,d]
	SetColor(0,0,0);							// задаем цвет координатных осей
	xyLine(0,0,1,10);							// строим оси, пересекающиес€ в т. (a,0)
	// с шагом делений по х равным Nx
	// и Ny -- по у
	
	
	for (int i=0; i<=num;i++) p[i]=i;

	SetColor(0,255,0);    			            
	double step=(b-a)/nRis;
	double x=a;
	SetPoint(x,ip(x));							
	for (int i=0; i<nRis; i++)
	{ 
		x+=step;
		Line2(x,ip(x));							
											
	}

	CloseWindow();	
}

double ip(double x)
{
	n=num;
	double ttt=0;
	double B;
	for (int i=0;i<=n;i++)
	{    B=1;
	     for(int j=0;j<=n;j++)
	     {z1:
	        if(j==i)
	         {j++;
	          goto z1;
	         }
	     else B*=(x-p[j])/(p[i]-p[j]);
	     }
	ttt+=func[i]*B;
	//fprintf(pf,"%f \n",B);
	}

	return ttt;
}

