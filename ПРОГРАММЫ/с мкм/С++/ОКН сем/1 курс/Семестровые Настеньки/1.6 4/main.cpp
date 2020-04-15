#include <cstdlib>
#include <iostream>
#include <stdio.h>
using namespace std;


FILE *fp=fopen("in.txt","r");FILE *fpp=fopen("out.txt","w");
int m[101];char pust[100];
int a0,a1,b0,b1;
int i,k=1,l=0,c;
void input();
	void go();
	void output();
#define m(c,i) m[(c-1)*10+i]

int main()
{

	input();
	go();
	output();


	fclose(fp); 
	fclose(fpp); 
	return 0;
}

void input()
{

	fscanf(fp,"%s",pust);

    for(i=1;i<=100;i++) fscanf(fp,"%d",&m[i]);
    
    fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
    fscanf(fp,"%d",&c);
}

void go()
{
 for(i=1;i<=10;i++)
 {k=m(c,i);
 m(c,i)=m(i,c);
 m(i,c)=k;
 } 

}



void output()
{
//fprintf(fpp,"%d ",c);
  for(i=1;i<=100;i++) 
  {fprintf(fpp,"%d ",m[i]);
  if(i%10==0) fprintf(fpp," \n");
  }
}



