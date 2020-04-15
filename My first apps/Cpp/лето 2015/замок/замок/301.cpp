


#include "stdafx.h"
#include "iostream"
#include <cstring>





int main()
{
char k[1000], d[1000];
int c,t=0,q,w;
cin>> c;
for(int a=o; a=c; a++) cin>> k[a];
for(a=o; a=c; a++) cin>> d[a];

q=atoi(d);
w=atoi(k);
for(int g=o;g=c;g++)
{
	if((q%10-w%10)>=((10-q%10)+w%10)){
	t=t+((10-q%10)+w%10);
	}
	else t=t+(q%10-w%10);
	q=(q-q%10)/10;
	w=(w-w%10)/10;

}
cout<<t;

	return 0;
}
