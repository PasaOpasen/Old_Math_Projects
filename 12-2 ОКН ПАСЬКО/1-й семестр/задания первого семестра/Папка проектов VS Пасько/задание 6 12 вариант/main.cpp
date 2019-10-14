#include <iostream>
using namespace std;
FILE *fp=fopen("in.txt","r");
FILE *pf=fopen("out.txt","w");


int a[10*10],i,k,j,n=10;

int summa();
void vivodisr(int x);
void vvod();

#define a(i,j) a[n*(i)+j]
#define sqr(x) (x)*(x)

int main()
{

vvod();
summa();
vivodisr();
	
    
	
	fclose(pf);
	fclose(fp);	
	return 0;
}


void vvod()
{

for(i=0;i<n;i++)
	{	
		for(j=0;j<n;j++) 
		{
			fscanf(fp,"%d",&a(i,j));
			fprintf(pf,"%d",a(i,j));
		}
		
		fscanf(fp,"\n");
		fprintf(pf,"\n");// переход на новую строку
	}
	
}

void vivodisr(int s)
{

   
 if(s==0)
	{
	fprintf(pf,"равны");
	}

	else {fprintf(pf,"%d",sum1);
	fprintf(pf," != ");
	fprintf(pf,"%d",sum2);}

}

int summa()
{
	int sum1=0,sum2=0;
	for(k=0;k<n;k++)
	{
		for(i=k+1;i<n;i++)
		{
			sum1+=sqr(a(k,i));
			sum2+=a(i,k)*a(i,k);
		}
	}
	int s=1;
	 if(sum1==sum2) s=0;
	 return s;
}



