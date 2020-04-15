// 307.cpp: определяет точку входа для консольного приложения.
//

#include "stdafx.h"
#include <iostream>

using namespace std;

int _tmain(int argc, _TCHAR* argv[])
{int n,q,e,t=0;
int a[2001],b[2001];
cin>>n;
for(e=1;e=n;e++) cin>>a[e];

for(e=1;e=n;e++)
{for(q=1;q=n;q++) 
{if(a[q]>a[e]) t++;}
b[e]=t+1;
t=0;
}


for(e=1;e=n;e++) cout<<b[e];







	return 0;
}

