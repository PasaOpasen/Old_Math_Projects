#include <ctime>
#include <iostream>
#include <stdlib.h>
using namespace std;

    FILE *fp=fopen("in.txt","r");
	FILE *fpp=fopen("out.txt","w");
	int i,d,e,k;
	int max1=0,min1=0,cred=0;
	const int u=10000000;
	int n[u];
	clock_t BEGIN,END;
	
	#define min(a,b) ((a) < (b) ? (a) : (b))
	

void makingmass()
{
	fscanf(fp,"%d",&n[0]);
    fscanf(fp,"%d",&d);
	for(i=1;i<u;i++) n[i]=n[i-1]+d;
	n[38]=rand()%150;
	n[15]=rand()%150;
	n[56]=rand()%150;
    
}


void lookingfor3()
{    max1=n[1];
	 min1=n[0];
	 cred=n[3]
	BEGIN=clock();
	for(int q=0;q<=3;q++)
    {
		for(i=3;i<u;i++)
		{
			if((n[i]!=min1)&&(n[i]!=max1)&&(n[i]!=cred))
            {                                      
                                                        
				if (n[i]> min1)
				{
					if(n[i]>cred)
					{
						if(n[i]>max1) max1=n[i];
						else cred=n[i];
					}
					else min1=n[i];
				}}
}
		}
	END=clock();
	
	fprintf(fpp,"%1d",min1);
	fprintf(fpp," +");
	fprintf(fpp," %1d",cred);
	fprintf(fpp," +");
	fprintf(fpp," %1d",max1);
	fprintf(fpp," =");
	fprintf(fpp," %1d",min1+cred+max1);
 
	fprintf(fpp,"   time=");
	fprintf(fpp," %0.10d",END-BEGIN);
	
}

void lookingfor4()
{    max1=n[1];
	 min1=n[0];
	 cred=n[3];
	 double k;
	BEGIN=clock();
	k=n[0];
	for(i=3;i<u;i++)
		{if(n[i]<k) k=n[i];
			
        }
	
	for(int q=0;q<=3;q++)
    {
		for(i=3;i<u;i++)
		{
			if((n[i]!=min1)&&(n[i]!=max1)&&(n[i]!=cred))
            {                                      
                                                        
				if (n[i]> min1)
				{
					if(n[i]>cred)
					{
						if(n[i]>max1) max1=n[i];
						else cred=n[i];
					}
					else min1=n[i];
				}}
}
		}
	END=clock();
	
	fprintf(fpp,"%1d",min1);
	fprintf(fpp," +");
	fprintf(fpp," %1d",cred);
	fprintf(fpp," +");
	fprintf(fpp," %1d",max1);
	fprintf(fpp," =");
	fprintf(fpp," %1d",min1+cred+max1);
 
	fprintf(fpp,"   time=");
	fprintf(fpp," %0.10d",END-BEGIN);
	
}

void lookingfor3()
{    max1=n[1];
	 min1=n[0];
	 cred=n[3]
	BEGIN=clock();

    
		for(i=3;i<u;i++)
		{
			if((n[i]!=min1)&&(n[i]!=max1)&&(n[i]!=cred))
            {                                      
                                                        
				if (n[i]> min1)
				{
					if(n[i]>cred)
					{
						if(n[i]>max1) max1=n[i];
						else cred=n[i];
					}
					else min1=n[i];
				}
				}
        }
}
		
	END=clock();
	
	fprintf(fpp,"%1d",min1);
	fprintf(fpp," +");
	fprintf(fpp," %1d",cred);
	fprintf(fpp," +");
	fprintf(fpp," %1d",max1);
	fprintf(fpp," =");
	fprintf(fpp," %1d",min1+cred+max1);
 
	fprintf(fpp,"   time=");
	fprintf(fpp," %0.10d",END-BEGIN);
	
}

int main()
{
	
makingmass();

lookingfor3();



	fclose(fp);
	fclose(fpp);
	return 0;
}

