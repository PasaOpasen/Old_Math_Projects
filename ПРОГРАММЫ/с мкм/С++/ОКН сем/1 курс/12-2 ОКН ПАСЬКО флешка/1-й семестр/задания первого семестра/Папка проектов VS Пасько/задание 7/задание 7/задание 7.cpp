#include <fstream>
#include <iostream>
using namespace std;

void main()
{
	setlocale(LC_ALL, "rus");
	ifstream fin("ввод.txt");
	ofstream fout("вывод.txt");

	int i,d,e,k;
	float max=0,min=0,cred=0;
	long long int n[1000];

	fin>>n[0]>>d;
	for(i=1;i<1000;i++) n[i]=n[i-1]+d;
	n[38]=rand()%150;
	n[15]=rand()%150;
	n[56]=rand()%150;


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

cout<<n[0]<<" "<<d<<endl;
cout<<min<<"+"<<cred<<"+"<<max<<"="<<min+cred+max<<endl;


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
cout<<min<<"+"<<cred<<"+"<<max<<"="<<min+cred+max;

fout<<min<<"+"<<cred<<"+"<<max<<"="<<min+cred+max;


	fout.close();

}
