#include <iostream>
using namespace std;

void main()
{
	int n,k=1000000,i,b=0;
	int a[1000];

	for(i=0;i<=999;i++) a[i]=0;

	cin>>n;
	for(i=0;i<=n-1;i++) cin>>a[i];



	for(i=0;i<=n-1;i++) {if(a[i]<k) k=a[i];}
	

	for(i=0;i<=n-1;i++) {if(a[i]%2==0)b++;}

	if(b>0) cout<<'-1';
	else cout<<k-1;


}
