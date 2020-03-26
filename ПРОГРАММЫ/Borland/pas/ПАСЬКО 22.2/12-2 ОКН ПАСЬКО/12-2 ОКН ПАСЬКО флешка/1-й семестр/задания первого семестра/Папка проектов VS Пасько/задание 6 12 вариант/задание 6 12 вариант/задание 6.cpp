#include <fstream>
using namespace std;

void main()
{
	setlocale(LC_ALL, "rus");
	ifstream fin("ввод.txt");
	ofstream fout("вывод.txt");

	int a[10][10],i,k,j,sum1=0,sum2=0;
	for(i=0;i<10;i++)
	{
		for(j=0;j<10;j++) fin>>a[i][j];
	}

	for(k=1;k<=10;k++){
		for(i=k;i<10;i++)
		{sum1+=a[k-1][i]*a[k-1][i];
		sum2+=a[i][k-1]*a[i][k-1];
		}
	}


	if(sum1==sum2) fout<<sum1<<" = "<<sum2;
	else fout<<sum1<<" != "<<sum2;
	fout.close();

}
