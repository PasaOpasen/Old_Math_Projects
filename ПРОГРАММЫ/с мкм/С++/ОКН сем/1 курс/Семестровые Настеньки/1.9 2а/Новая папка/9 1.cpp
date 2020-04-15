#include <iostream>
#include <ctime>
using namespace std;

const int n=1000;
double b,x,c[n];
float q,w,u;
int i,k,h=n;
double y;
float BEGIN,END;
FILE *fp=fopen("in.txt","r");FILE *fpp=fopen("out.txt","w");


void make()
{
	fscanf(fp,"%f",&w);
	fscanf(fp,"%f",&q);
	c[0]=w;
    b=q;
	for(i=1;i<=n;i++) c[i]=c[i-1]+b;

	fscanf(fp,"%f",&w);
	fscanf(fp,"%f",&q);
	fscanf(fp,"%f",&u);
	c[5]=w;
	c[6]=q;
	c[120]=u;
}


void go(){
	BEGIN=clock();
	
	for(int g=1; g<999;g++)
	{
	
	double x=c[0];
	int h=n-1; i=1;

	while(i+(n-h)<=n)
	{
		if(c[i]>x) i++;

		else
        {
			y=c[h];
			c[h]=c[i];
			c[i]=y;
			h--;
		}

	 }
     }
          y=c[0];
		  c[0]=c[i-1];
		  c[i-1]=y;
     
	END=clock();
} 

void vivod()
{
fprintf(fpp,"20 elements :  ");

fprintf(fpp,"%f\n ",c[1]);

if (i<10)
{for(int h=0;h<=0+20;h++) fprintf(fpp,"%f ",c[h]);}
else if (i>n-10)
{for(int h=n-20;h<=n;h++) fprintf(fpp,"%f ",c[h]);}
else
{for(int h=i-10;h<=i+10;h++) fprintf(fpp,"%f ",c[h]);}


fprintf(fpp,"\n  time =  ");
fprintf(fpp,"%0.6f",END-BEGIN);
} 


int main()
{
	make();
	go();
	vivod();

	fclose(fp);
	fclose(fpp);
	return 0;
}
