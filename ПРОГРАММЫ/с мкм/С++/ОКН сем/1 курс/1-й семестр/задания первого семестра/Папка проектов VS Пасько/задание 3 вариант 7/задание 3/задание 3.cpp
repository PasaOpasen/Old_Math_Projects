#include <fstream>
using namespace std;

int main()
{
	setlocale(LC_ALL, "rus");

	int b[10];
	int i=0,f,k=0;

	ifstream fin("ввод.txt");

	for(i=0;i<10;i++) fin>>b[i];
	i=0;
	while(b[i]>=0)i++;

	f=b[i];
	for(i=1;i<10;i++) {if((b[i]<0)&&(b[i]>f)) f=b[i];}

	ofstream fout("вывод.txt"); 
	fout<<"элементы массива:   ";
	for(i=0;i<10;i++) fout<<b[i]<<"   ";
	fout<<endl;
	fout <<"наибольший отрицательный элемент=   "<<f<<endl;

	fout.close(); 

return 0;
}
