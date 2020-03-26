#include <fstream>
#include <iostream>
using namespace std;

const long int y=500000;
const long int t=1000000;

void main()
{
	setlocale(LC_ALL, "rus");
	int m[y],n[y],z[1000000];
	int a0,a1,b0,b1,i,k,l;
	m[0]=a0;
	n[0]=a1;

ifstream fin("ввод.txt");
ofstream fout("вывод.txt"); 



fin>>a0>>a1>>b0>>b1;
for(i=1;i<=y;i++)
{
	m[i]=m[i-1]+b0;
    n[i]=n[i-1]+b1;
}



k=0;
l=0;
for(i=0;i<=2*y;i++)
{
if(m[k]>=n[l]) {z[i]=n[l]; l++;}
else {z[i]=m[k]; k++;}
}

for(i=0;i<10;i++){fout<<z[i]<<" ";}
for(i=10;i<20;i++){fout<<z[i]<<" ";}



	fout.close(); 


}