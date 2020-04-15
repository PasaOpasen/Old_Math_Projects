#include <iostream>
using namespace std;

void main()
{int a[100];
int n,i,c=0;
double k=0;
int e;
for(i=0;i<=99;i++) a[i]=0;


cin>>n;
for(i=0;i<=n-1;i++) cin>>a[i];


c=0;
for(i=0;i<=n-1;i++){if(a[i]>=c) c=a[i];} 

if(a[1]==c) cout<<'0';

else {for(i=1;i<=n-1;i++){if(a[i]>=a[1])k+=a[i];}
k=k/n;

e=k;
if(k==e)cout<<k;
else cout<<e++;

}


}