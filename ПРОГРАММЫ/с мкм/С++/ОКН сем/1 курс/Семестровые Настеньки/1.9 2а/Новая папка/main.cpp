#include <iostream>
#include <ctime>
using namespace std;

const long int y=5000000, o=5000000;
FILE *fp=fopen("in.txt","r");FILE *fpp=fopen("out.txt","w");
	int m[o],n[y],z[o+y];
	int a0,a1,b0,b1;
    int i,k=0,l=0;
    clock_t BEGIN,END;

void made()
{
	fscanf(fp,"%d",&a0);
	fscanf(fp,"%d",&a1);
	fscanf(fp,"%d",&b0);
	fscanf(fp,"%d",&b1);
	m[0]=a0;
	n[0]=a1;
	for(i=1;i<y;i++)
	{
		n[i]=n[i-1]+b1;
	}
	for(i=1;i<o;i++)
	{
     m[i]=m[i-1]+b0;
	}
 }

void go()
{
	BEGIN=clock();

	if ((b0<0)&&(b1<0))
	{l=o;
	k=y;
	for(i=(o+y);i>0;i--)
	{
		if ((l<0)&&(k<0))
		{
			if(m[k]>=n[l]) {z[i]=n[l]; l--;}
			else {z[i]=m[k]; k--;}
		}
		else if (l<0)z[i]=m[k];
		else z[i]=n[l];
	}
	}

	else for(i=0;i<(o+y);i++)
	{
		if ((l>o)&&(k>y))
		{
			if(m[k]>=n[l]) {z[i]=n[l]; l++;}
			else {z[i]=m[k]; k++;}
		}
		else if (l>o)z[i]=m[k];
		else z[i]=n[l]
	}

    END=clock();
    fprintf(fpp,"   time=");
	fprintf(fpp," %0.10d",END-BEGIN);
 }
 
void vivod()
{
     for(i=0;i<20;i++)fprintf(fpp,"  %7d",z[i]);
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
