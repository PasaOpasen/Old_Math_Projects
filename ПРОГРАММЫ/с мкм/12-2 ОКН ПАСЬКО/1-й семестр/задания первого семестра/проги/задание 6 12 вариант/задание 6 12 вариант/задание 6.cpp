#include <iostream>
using namespace std;

int main()
{
	FILE *fp=fopen("in.txt","w");
	FILE *pf=fopen("out.txt","r");

	int a[10][10],i,k,j,sum1=0,sum2=0;


	for(i=0;i<10;i++)
	{	
		for(j=0;j<10;j++) {fscanf(fp,"%d",&a[i][j]);fprintf(pf,"%d",a[i][j]);}
		fscanf(fp,"\n");fprintf(pf,"\n");// переход на новую строку
	}


	for(k=1;k<=10;k++)
	{
		for(i=k;i<10;i++)
		{
			sum1+=a[k-1][i]*a[k-1][i];
			sum2+=a[i][k-1]*a[i][k-1];
		}
	}


	if(sum1==sum2)
	{fprintf(pf,"%d",sum1);
	fprintf(pf," = ");
	fprintf(pf,"%d",sum2);}

	else {fprintf(pf,"%d",sum1);
	fprintf(pf," != ");
	fprintf(pf,"%d",sum2);}






	fclose(pf);
	fclose(fp);
	return 0;
}
