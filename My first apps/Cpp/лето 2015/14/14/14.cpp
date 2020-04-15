#include<iostream>
using namespace std;

void main()
{
	int n,i,j,k,t;
		int a[10000],b[10000];
	cin>>n;

	for(i=0;i<=n-1;i++) cin>>a[i];

	for(i=0;i<=n-1;i++)
	{
		k=2000000000;
		t=0;
		for(j=0;j<=n-1;j++) b[j]=abs(a[i]-a[j]);
		
		
			for(j=0;j<=n-1;j++)
			{if(b[j]!=0)
			{
				if(b[j]>t) t=b[j];
                if(b[j]<k) k=b[j];
			}
			}
			cout<<k<<' '<<t<<endl;
	}



}