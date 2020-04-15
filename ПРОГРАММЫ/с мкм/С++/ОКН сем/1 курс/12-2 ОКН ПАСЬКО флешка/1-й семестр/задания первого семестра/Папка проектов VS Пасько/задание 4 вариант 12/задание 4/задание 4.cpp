#include <fstream>

using namespace std;

void main()
{
	setlocale(LC_ALL, "rus");
	int n=0,x=1,y=1,z=1;
	



	for(x=1;x<=1000;x++)
	{
		for(y=1;y<=1000;y++)
		{
			for(z=1;z<=1000;z++)
			{
				if(x-y*y+2*z==100000)
				{
					if(x+20*y+z<1000)n++;
				}
			}
		}
	}

	ofstream fout("вывод.txt"); 

	fout <<"количество натуральных решений="<<n<<endl;
	
	

	fout.close(); 


}
