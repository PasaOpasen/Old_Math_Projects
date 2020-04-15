#include <iostream>
#include <ctime>
using namespace std;

const int n=100000;

int main()
{
	double b,x,c[n];
int i,k,h,y;
double BEGIN,END;
FILE *fp=fopen("in.txt","r");FILE *fpp=fopen("out.txt","w");


fscanf(fp,"%d",&c[0],&b);
for(i=1;i<n;i++) c[i]=c[i-1]+b;


BEGIN=clock();
x=c[0];
k=0;
for(i=1;i<n;i++)
{
	if(c[i]>x)
	{y=c[k];
	c[k]=c[i];}

	for(h=i;h>k+1;h--)
	{
		c[h]=c[h-1];
	}
	c[k+1]=y;
	k++;

}
END=clock();

i=0;fprintf(fpp,"20 elements");
while(c[i]!=x) i++;
for(h=i-5;h<i+5;h++) fprintf(fpp,"%d",c[h]);

fprintf(fpp,"time");
fprintf(fpp,"%d",END-BEGIN);

fclose(fp);
fclose(fpp);
return 0;
}