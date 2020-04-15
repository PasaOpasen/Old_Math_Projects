// 308.cpp: определяет точку входа для консольного приложения.
//

#include "stdafx.h"
#include <iostream>

using namespace std;

int _tmain(int argc, _TCHAR* argv[])
{int b=0,k;
char a;
while(a!=' ')
{
	cin>>a;
b++;
}
k=26*(b+1)-b;
cout<<k;




	return 0;
}

