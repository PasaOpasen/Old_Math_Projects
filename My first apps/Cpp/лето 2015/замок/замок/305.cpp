// 304.cpp: определяет точку входа для консольного приложения.
//

#include "stdafx.h"
#include <iostream>

using namespace std;

int _tmain(int argc, _TCHAR* argv[])
{
	char a,b,c,d;
	bool c;
	int b=0;


	while(c==true)
	{cin>>a;
	cin>>b;
		c=((a=='A')&&(b=='B'))||((b=='A')&&(a=='B'));
		
	}
	b++;
if((a=='A')&&(b=='B'))
{while(c==true)
	{cin>>c;
	cin>>d;
		c=((d=='A')&&(c=='B'))
	}
b++
}

if((b=='A')&&(a=='B'))
	{while(c==true)
	{cin>>c;
	cin>>d;
		c=((c=='A')&&(d=='B'))
	}
b++;
}
if(b==2) cout<<"YES";
else cout<<"NO";





	return 0;
}

