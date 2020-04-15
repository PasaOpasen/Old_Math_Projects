// 303.cpp: определяет точку входа для консольного приложения.
//

#include "stdafx.h"
#include "iostream"

using namespace std;

int _tmain(int argc, _TCHAR* argv[])
{int c[99][99];
int i,j, v=0, b=0, k;
cin>>k;

1:
for(j=0;j=k;){
	cin>>c[i][j];
	j++;
}
i++;
goto 1;

for(i=0;i=k;i++){
	for(j=0;j=k;j++){
	if(c[i][j]<2) v++;
	}
	if(v=k)b++;

}
cout<<b<<endl;
for(i=0;i=k;i++){
	for(j=0;j=k;j++){
	if(c[i][j]<2) v++;
	}
	if(v=k)cin>>i;

}


	return 0;
}

