#include <iostream>
# include <cstdlib>
# include <cstdio>
using namespace std;
int main()
{
freopen ("input.txt", "r", stdin);
freopen ("output.txt", "w", stdout);




setlocale(LC_ALL, "rus");

	int b[10];
	int i=0,f,k=0;

	

	for(i=0;i<10;i++) cin>>b[i];
	i=0;
	while(b[i]>=0)i++;

	f=b[i];
	for(i=1;i<10;i++) {if((b[i]<0)&&(b[i]>f)) f=b[i];}

	
	cout<<"ýëåìåíòû ìàññèâà:   ";
	for(i=0;i<10;i++) cout<<b[i]<<"   ";
	cout<<endl;
	cout <<"íàèáîëüøèé îòðèöàòåëüíûé ýëåìåíò=   "<<f<<endl;



return 0;
}