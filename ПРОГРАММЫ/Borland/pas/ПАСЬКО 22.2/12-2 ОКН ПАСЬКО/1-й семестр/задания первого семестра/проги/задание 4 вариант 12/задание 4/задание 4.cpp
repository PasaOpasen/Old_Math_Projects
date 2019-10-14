#include <fstream>
#include <ctime>
using namespace std;

int main()
{
	FILE *th=fopen("out.txt","w");
	int n=0,x=1,y=1, z=1;;
		double v;
	double be,en;
	


be=clock();
	for(x=1;x<=1000;x++)
	{
		for(y=1;y<=1000;y++)
		{
			for(z=1;z<=1000;z++)
			{
				if(x-y*y+2*z==100000)
				{
					if(x+20*y+z<1000) n++;
				}
			}
		}
	}
en=clock();

	
v=en-be;
	fprintf(th,"количество натуральных решений=");
	fprintf(th,"%d\n",n);
    fprintf(th,"time=");
	fprintf(th,"%f\n",v);
	
	

	
	
	fclose(th);

	return 0;

}
