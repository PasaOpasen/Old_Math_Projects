#include <iostream>
using namespace std;

void main()
{ 
    int *a,n;
	double min,max;
    double k,s;
    cin>>n;
    a = new int[n];
    
    for(int i=0;i<n;i++)
    {cin>>a[i];}
    
    min=max=a[0];
    for(int i=0;i<n;i++)
    {
        if (a[i]<min) min=a[i];
        if (a[i]>max) max=a[i];
    }
    
    k=(max-min)/2;
    s=min+k;
    
	//cout<<min<<' '<<max<<' '<<k<<' '<<s;

    for(int i=0;i<n;i++)
    {
        if((a[i]!=s)&&((a[i]+k)!=s)&&((a[i]-k)!=s)) {cout<<"NO";return;}
    }
    
    cout<<"YES";
}