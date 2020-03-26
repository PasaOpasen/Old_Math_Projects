#include <iostream>
#include <ctime>
using namespace std;

const int n=10000000;
double b,x,c[n];
float q,w,u;
int i,k,h;
double y;
double BEGIN,END;
FILE *fp=fopen("in.txt","r");FILE *fpp=fopen("out.txt","w");

void make()
{
	fscanf(fp,"%f",&w);
	fscanf(fp,"%f",&q);
	b=q;
	c[0]=w;
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
	x=c[0];
	k=0;
	int h=0;
	//for(i=1;i<=n;i++)
	//{z1:
	//	if(c[i]>x)
	//	{y=c[k];
	//	c[k]=c[i];
	//    c[i]=y;
	//    k++;
	//    }
	//
	//else{
	//     y=c[n];
	//     c[n]=c[i];
	//	 c[i]=y;
	//	 h++;
	//	 if(h<=n-i)
	//	 goto z1;
	//}

	i=1;
	while(i+h<=n)
	{
		if(c[i]>x)
		{y=c[i];
		c[i]=c[i-1];
		c[i-1]=y;
		i++;
		}

		else{
			y=c[n-h];
			c[n-h]=c[i];
			c[i]=y;
			h++;
		}

	}
	END=clock();
} 

void vivod()
{int s;
//i=0;while(c[i]!=x) i++;
fprintf(fpp,"20 elements -  ");


for(h=max((i-10),0);h<min(i+10,n);h++) fprintf(fpp,"%f ",c[h]);


fprintf(fpp,"\n  time= ");
fprintf(fpp,"%0.10d",END-BEGIN);
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
