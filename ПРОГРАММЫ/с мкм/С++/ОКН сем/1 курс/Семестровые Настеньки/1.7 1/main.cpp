#include <ctime>
#include <iostream>
#include <stdlib.h>
using namespace std;

    FILE *fp=fopen("in.txt","r");
	FILE *fpp=fopen("out.txt","w");
	const int u=1000;
	float q,w,g;
int i,k,h,b;
int n[u];
double y;
	int max1=0,min1=0,cred=0;

double BEGIN,END;
	char pust[100];
	#define min(a,b) ((a) < (b) ? (a) : (b))
	

void makingmass()
{
	
	fscanf(fp,"%s",pust);
	fscanf(fp,"%f",&w);
	fscanf(fp,"%s",pust);
	fscanf(fp,"%f",&q);
	b=(int)q;
	n[0]=(int)w;
	for(i=1;i<u;i++) n[i]=n[i-1]+b;
    
	fscanf(fp,"%s",pust);
	fscanf(fp,"%f",&w);
	fscanf(fp,"%f",&q);
	fscanf(fp,"%f",&g);
	n[5]=(int)w;
	n[6]=(int)q;
	n[120]=(int)g;
    
}


void lookingfor3()
{    max1=n[1];
	 min1=n[0];
	 cred=n[3];
	BEGIN=clock();
	for(int q=0;q<=10000;q++)
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
	fprintf(fpp," %1d ",max1);
	fprintf(fpp," =");
	fprintf(fpp," %1d ",min1+cred+max1);
 
	fprintf(fpp,"   time= ");
	fprintf(fpp," %0.10f \n",END-BEGIN);
	
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
	
	for(int q=0;q<=10000;q++)
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
 
	fprintf(fpp,"  time= ");
	fprintf(fpp," %0.10f \n",END-BEGIN);
	
}

void lookingfor1()
{    max1=n[1];
	 min1=n[0];
	 cred=n[3];
	//BEGIN=clock();

//    
//		for(i=3;i<u;i++)
//		{
//			if((n[i]!=min1)&&(n[i]!=max1)&&(n[i]!=cred))
//            {                                      
//                                                        
//				if (n[i]> min1)
//				{
//					if(n[i]>cred)
//					{
//						if(n[i]>max1) max1=n[i];
//						else cred=n[i];
//					}
//					else min1=n[i];
//				}
//				}
//        }
//		
//	END=clock();
	
	
		BEGIN=clock();
	for(int q=0;q<=1000;q++)
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
 
	fprintf(fpp,"  time=");
	fprintf(fpp," %0.10f \n",END-BEGIN);
	
}

int main()
{
	
makingmass();

lookingfor4();
lookingfor3();
lookingfor1();

	fclose(fp);
	fclose(fpp);
	return 0;
}

