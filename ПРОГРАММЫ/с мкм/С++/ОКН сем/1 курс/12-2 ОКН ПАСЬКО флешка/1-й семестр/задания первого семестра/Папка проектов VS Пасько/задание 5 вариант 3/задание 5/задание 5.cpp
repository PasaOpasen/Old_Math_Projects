#include <fstream>
#include <cmath>
using namespace std;


float mycos(float x)
{
	int sum;
	float k=0,b=1;

	for(sum=2; sum<100000;sum+=2)
	{
k+=b;
b=b*(-1)*x*x/sum;
	}
	return k;

}

void main()
{
	setlocale(LC_ALL, "rus");
	int i;
	float x;
	ifstream fin("ввод.txt");

	ofstream fout("вывод.txt"); 

	for(i=0;i<10;i++)
	{
	fin>>x;

	fout <<"моя функция= "<<mycos(x)<<"        погрешность= "<<abs(cos(x)-mycos(x))<<endl;
	}


	fout.close(); 


}