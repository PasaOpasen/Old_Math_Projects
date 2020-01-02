#include <iostream>
#include <ctime>
using namespace std;

const long int y=50;

int main()
{
	FILE *fp=fopen("in.txt","r");FILE *fpp=fopen("out.txt","w");
	long int m[y],n[y],z[2*y];
	int a0,a1,b0,b1,i,k=0,l=0;


	fcanf(fp,"%d",&a0,&a1,&b0,&b1);
	m[0]=a0;
	n[0]=a1;
	for(i=1;i<y;i++)
	{
		m[i]=m[i-1]+b0;
		n[i]=n[i-1]+b1;
	}



	
	for(i=0;i<(2*y);i++)
	{
		if(m[k]>=n[l]) {z[i]=n[l]; l++;}
		else {z[i]=m[k]; k++;}
	}


	for(i=0;i<20;i++)fprintf(fpp,"%4d",z[i]);




	fclose(fp); 
	fclose(fpp); 
	return 0;
}