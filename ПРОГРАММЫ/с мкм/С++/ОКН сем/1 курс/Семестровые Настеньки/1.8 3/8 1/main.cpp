#include <iostream>
#include <ctime>
using namespace std;

const long int y=5000000, o=50000;
FILE *fp=fopen("in.txt","r");FILE *fpp=fopen("out.txt","w");
int m[o],n[y],z[o+y];
int a0,a1,b0,b1;
int i,k=0,l=0,c;
clock_t BEGIN,END;

void made()
{

	fscanf(fp,"%d",&a0);
	fscanf(fp,"%d",&a1);
	fscanf(fp,"%d",&b0);
	fscanf(fp,"%d",&b1);


	m[0]=a0;
	n[0]=a1;
	for(i=1;i<=y;i++)
	{
		n[i]=n[i-1]+b1;
	}
	for(i=1;i<=o;i++)
	{
		m[i]=m[i-1]+b0;
	}
	m[0]=a0;
    n[0]=a1;
    
}

void go()
{
	BEGIN=clock();

	if ((b0<0)&&(b1<0))
	{
         
		
		int x=0,c=0;
         for(i=0;i<(o+y);i++)
	{
	if (n[x]>=m[c]) 
    {z[i]=n[x];x++;}
   else {z[i]=m[c];c++;}

	
    if (c==o+1)
	{for(k=i+1;k<o+y;k++) {z[k]=n[x]; x++; }goto z1;}
	if (x==y+1)
	{for(k=i+1;k<o+y;k++) {z[k]=m[c]; c++; }goto z1;}
	}
		
	}
	else {int x=0,c=0;
         for(i=0;i<(o+y);i++)
	{
	if (n[x]<=m[c]) 
    {z[i]=n[x];x++;}
   else {z[i]=m[c];c++;}

	
    if (c==o+1)
	{for(k=i+1;k<o+y;k++) {z[k]=n[x]; x++; }goto z1;}
	if (x==y+1)
	{for(k=i+1;k<o+y;k++) {z[k]=m[c]; c++; }goto z1;}
	}
	}

z1:
	END=clock();
	fprintf(fpp,"  time = ");
	fprintf(fpp," %0.10d",END-BEGIN);
}



void vivod()
{
  //   for(i=0;i<20;i++)fprintf(fpp," %2d",m[i]);
//for(i=0;i<20;i++)fprintf(fpp," %2d",n[i]);
	for(i=0;i<20;i++)fprintf(fpp," %2d",z[i]);
}


int main()
{

	made();
	go();
	vivod();


	fclose(fp); 
	fclose(fpp); 
	return 0;
}
