#include <iostream>
using namespace std;

void main()
{
int a[100],b[100];
int n,p=0,o=0, t=0,u=o,k=0,w;
int m=0;

for(int q=0;q<=99;q++) {a[q]=0; b[q]=0;}

cin>>n;

for(w=0;w<=n-1;w++) {cin>>a[w]; cin>>b[w];}

for(w=0;w<=n-1;w++)
{if(a[w]<0) {p+=b[w]; t++;}
else {o+=b[w]; u++;}
}

if(u==t)cout<<p+o;
else 
{
if(t>u)

      { o=0;
       while(k<=t+1){
	      m++;
	for(w=0;w<=n-1;w++)
	{
		if(a[w]==m) {o+=b[w];k++;}

	                }
	   }}

if(t<u)
{ p=0;
while(k<=u+1){
	m++;
	for(w=0;w<=n-1;w++){
		if(a[w]==m) {p+=b[w];k++;}

	}}}

cout<<p+o;
}

}