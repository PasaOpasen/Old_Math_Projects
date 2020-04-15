#include <iostream>
using namespace std;

void main()
{int a[100];
int n,i,c=0,k=0;
for(i=0;i<=99;i++) a[i]=0;


cin>>n;
for(i=0;i<=n-1;i++) cin>>a[i];

z1:
c=0;
for(i=0;i<=n-1;i++){if(a[i]>=c) c=a[i];} 

if(a[1]==c) cout<<k;
else for(i=0;i<=n-1;i++){if(a[i]==c) {a[i]--;k++;a[1]++;if(a[1]==c) cout<<k; break;}}
goto z1;

}