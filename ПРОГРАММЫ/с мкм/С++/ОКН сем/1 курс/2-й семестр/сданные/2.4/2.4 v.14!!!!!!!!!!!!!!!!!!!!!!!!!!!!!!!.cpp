#include <cstdlib>
#include <iostream>
#include <stdio.h>
#include "ReadImage.h"
#include <math.h>
using namespace std;

double	a,b,c,d;
float sc,sv,sb;
int h,w,n,i,j;
double q[100];
double r[100],g[100],t[100];
char *R,*G,*B;
char pust[100];
	float  scan;
	FILE *fp=fopen("in.txt","r");	
   	FILE *pf=fopen("out.txt","w");

void Vvod();									
void grafic();									
double f(double x,double y);



int main()
{
	Vvod();
	GetSize("input.bmp", &w, &h);	// (w -- ширина, h -- высота)
	R = new char[h*w];
	G = new char[h*w];
	B = new char[h*w];
	ReadImageFromFile("input.bmp",B,G,R);
			

	grafic();


	WriteImage("output.bmp",B,G,R);	
			
}



void Vvod()
{
						


	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);fscanf(fp,"%f",&scan);
	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);fscanf(fp,"%f",&scan);b=scan;
	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);fscanf(fp,"%f",&scan);c=scan;
	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);fscanf(fp,"%f",&scan);d=scan;
	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);fscanf(fp,"%d",&n); 
	fscanf(fp,"%s",pust);

	for(i=0;i<n;i++)
	{
		fscanf(fp,"%f",&sc);
		q[i]=sc;
		fprintf(pf,"%d " ,i);
		fprintf(pf,"%f \n",q[i]);
	}


	fscanf(fp,"%s",pust);

      for(i=0;i<n+1;i++)
	{
		fscanf(fp,"%f %f %f",&sc,&sv,&sb);
        r[i]=sc;
        g[i]=sv;
        t[i]=sb;
	}

	fclose(fp);
    fclose(pf);								
}




void grafic()
{ 
	double dx=(b-a)/w, dy=(d-c)/h;
	int m=0;
	int ur;

	for (double y=c;y<d;y+=dy)
	{	
		for (double x=a;x<b;x+=dx)
		{ 
  
             int p=n;
    
            if(f(x,y)>=q[n]) 
			{
			R[m]=r[n+1];
			G[m]=g[n+1];
			B[m]=t[n+1];
            }
            else
            {
			while(f(x,y)<q[p]) p--;
			R[m]=r[p];
			G[m]=g[p];
			B[m]=t[p];
            }
			if(f(x,y)<=q[0]) 
			{
			R[m]=r[0];
			G[m]=g[0];
			B[m]=t[0];
            }
			m++;
		}
	}
}

double f(double x,double y)
{
	return x*x-y*y;
}
