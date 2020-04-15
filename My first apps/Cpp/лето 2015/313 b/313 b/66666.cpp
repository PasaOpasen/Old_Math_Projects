#include <iostream>
using namespace std;

void main()
{
	int n,i,b=0;
	int a[1000];

	for(i=0;i<=999;i++) a[i]=0;

	cin>>n;
	for(i=0;i<=n-1;i++) cin>>a[i];



	for(i=0;i<=n-1;i++) {if(a[i]==1) {b=1;cout<<'-1';  break;}}
	

	if(b==0) cout<<'1';



}