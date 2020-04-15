#include <iostream>

using namespace std;

void main()
{int n,a,b,c;
int s[2],d[2],e[2];

cin>> n;
cin>> s[2];
cin>> d[2];
cin>> e[2];

  for(a=s[1];a>=s[0];)
            {
	  for(b=d[1];a>=d[0];)
  {
	  for(c=e[1];c>=e[0];)
	  {if(a+b+c!=n)c--;
	  else {cout<<a<<' '<<b<<' '<<c; break;}
	  }
  if(a+b+c!=n)b--;
	  else {cout<<a<<' '<<b<<' '<<c; break;}

  }
          if(a+b+c!=n)a--;
	  else {cout<<a<<' '<<b<<' '<<c; break;}  }

}