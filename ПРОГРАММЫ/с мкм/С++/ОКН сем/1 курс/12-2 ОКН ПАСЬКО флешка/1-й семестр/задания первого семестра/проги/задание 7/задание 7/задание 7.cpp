#include <ctime>
#include <iostream>
using namespace std;

FILE *fp=fopen("in.txt","r");
	FILE *fpp=fopen("out.txt","w");
	int i,d,e,k;
	float max=0,min=0,cred=0;
	float n[1000];
	double BEGIN,END;



void makingmass()
{
	fscanf(fp,"%d",n[0],&d);
	for(i=1;i<1000;i++) n[i]=n[i-1]+d;
	n[38]=rand()%150;
	n[15]=rand()%150;
	n[56]=rand()%150;
}

void lookingfor()
{
	BEGIN =clock();
	for(int q=0;q<=3;q++){
		for(i=0;i<1000;i++)
		{
			if((n[i]!=min)&&(n[i]!=max)&&(n[i]!=cred)){
				if (n[i]>min)
				{
					if(n[i]>cred)
					{
						if(n[i]>max) max=n[i];
						else cred=n[i];
					}
					else min=n[i];
				}}

		}}
	END=clock();
}

void first()
{
    min=n[1];
	max=n[0];
	for(i=0;i<3;i++)
	{
		if(n[i]>max) {max=n[i]; e=i;}
		if(n[i]<min) {min=n[i]; k=i;}

	}

	for(i=0;i<3;i++)
	{
		if((i!=e)&&(i!=k)) cred=n[i];
	}
}	


void vivod()
{
	fprintf(fpp,"%d",min);
	fprintf(fpp,"+");
	fprintf(fpp,"%d",cred);
	fprintf(fpp,"+");
	fprintf(fpp,"%d",max);
	fprintf(fpp,"=");
	fprintf(fpp,"%d",min+cred+max);

	fprintf(fpp,"time=");
	fprintf(fpp,"%d",END-BEGIN);
}


int main()
{
	
makingmass();
first()
lookingfor()
vivod();


	fclose(fp);
	fclose(fpp);
	return 0;
}
