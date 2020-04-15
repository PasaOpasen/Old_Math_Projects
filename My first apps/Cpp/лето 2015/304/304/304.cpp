// 304.cpp: определяет точку входа для консольного приложения.
//

#include "stdafx.h"
#include <iostream>

using namespace std;

int _tmain(int argc, _TCHAR* argv[])
{
	int k,w,n,e;
	int b=0;
	cin>>k>>w>>n;

	for(int i=1;i=n;i++)
	{
		b=b+i*k;
	}
e=b-w;
if(e>0) cout<<e;
else cout<<'0';









	return 0;
}

