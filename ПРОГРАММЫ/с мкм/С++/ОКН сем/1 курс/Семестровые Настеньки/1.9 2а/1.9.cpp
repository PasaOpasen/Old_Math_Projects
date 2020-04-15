#include <iostream>
#include <ctime>
using namespace std;

const int n=1000000;
int b,x,c[n],d[n];
float q,w,u;
int i,k,h;
double y;
double BEGIN,END;
char pust[100];
FILE *fp=fopen("in.txt","r");FILE *fpp=fopen("out.txt","w");

void make()
{
	fscanf(fp,"%s",pust);
	fscanf(fp,"%f",&w);
	fscanf(fp,"%s",pust);
	fscanf(fp,"%f",&q);
	b=(int)q;
	c[0]=(int)w;
	for(i=1;i<=n;i++) c[i]=c[i-1]+b;
    
	fscanf(fp,"%s",pust);
	fscanf(fp,"%f",&w);
	fscanf(fp,"%f",&q);
	fscanf(fp,"%f",&u);
	c[5]=(int)w;
	c[6]=(int)q;
	c[120]=(int)u;
}

void go(){
	BEGIN=clock();
	
	x=c[0];
	int h=0;

	i=1;
	while(i+h<n)
	{
		if(c[i]>x)
		{
	//	y=c[i];
//		c[i]=c[i-1];
//		c[i-1]=y;
		i++;
		}

		else{
			y=c[n-h];
			c[n-h]=c[i];
			c[i]=y;
			h++;
		}
     
	}
	     y=c[i];
		c[i]=c[0];
		c[0]=y;
	
	for(i=0;i<n;i++) d[i]=c[n-i];
	
	END=clock();
} 

void vivod()
{
fprintf(fpp,"20 elements -  ");
for(i=0;i<n;i++)
{
	if(d[i]==x) {
	h=i;break;}
}
for(i=h-10;i<h+10;i++) 
{if ((i>0)&(i<n))fprintf(fpp,"%d ",d[i]);
}



fprintf(fpp,"\n  time= ");
fprintf(fpp,"%0.7f",END-BEGIN);
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
