#include <fstream>
#include <iostream>
#include <cmath>

using namespace std;


double **aa,*a,*c,*b,*f,*y,eps,*alp,*bet,T,h,k=1;
int n;

double u(double x)
{
	return cos(k*x);
}

double D2u(double x)
{
	return -k*k*cos(k*x);
}

void read()
{ifstream fin("input2.txt");
char v[50];
//fin>>n;
fin>>v>>T>>v>>h;
n=int(T/h+1);


  aa = new double *[n+1];
    for (int i = 0; i <= n; i++)
        aa[i] = new double [n+1];

y=new double[n+1];
f=new double[n+1];

a=new double[n+1];
b=new double[n+1];
c=new double[n+1];


alp=new double[n+1];
bet=new double[n+1];

for(int i=1;i<=n;i++)
for(int j=1;j<=n;j++)
{aa[i][j]=0;
if(abs(i-j)<=1) aa[i][j]=i%2+j%2; 
} 

//fin>>aa[i][j];

for(int i=1;i<=n;i++) f[i]=D2u(0+i*h);
//f[i]=i%5+i%2;
//fin>>f[i];
//f[n/2]=10000;

b[1]=aa[1][1];
c[1]=aa[1][2];
for(int i=2;i<n;i++)
{
	b[i]=aa[i][i];
	c[i]=aa[i][i+1];
	a[i]=aa[i][i-1];
}
b[n]=aa[n][n];
a[n]=aa[n][n-1];



//вывод
/*
for(int i=1;i<=n;i++)
{for(int j=1;j<=n;j++) cout<<aa[i][j]<<" ";
cout<<endl;
}
cout<<endl;
for(int i=1;i<=n;i++) cout<<f[i]<<endl;
for(int i=2;i<=n;i++) cout<<a[i]<<' ';cout<<endl;
for(int i=1;i<=n;i++) cout<<b[i]<<' ';cout<<endl;
for(int i=1;i<n;i++) cout<<c[i]<<' ';cout<<endl;
*/
fin.close();
}

void search()
{   //прямой ход
	alp[2]=-c[1]/b[1];
	bet[2]=f[1]/b[1];
	for(int i=2;i<n;i++)
	{
		double val=b[i]+a[i]*alp[i];
		//cout<<b[i]<<' '<<a[i]<<' '<<alp[i]<<' '<<val<<endl;
		alp[i+1]=-c[i]/val;
		bet[i+1]=(-a[i]*bet[i]+f[i])/val;
	}
	
    /*	for(int i=2;i<=n;i++)
	{
		cout<<alp[i]<<" "<<bet[i]<<endl;
	}*/
	
	//обратный ход
	y[n]=(-a[n]*bet[n]+f[n])/(b[n]+a[n]*alp[n]);
	for(int i=n-1; i>=1;i--)
	{
		y[i]=alp[i+1]*y[i+1]+bet[i+1];
	}
}
//невязка
/*double Nev()
{int t;
double q;
double s=0;
double norm[n];
double c[n];
for (int i=1;i<=n;i++)
{c[i]=0;
for (int t=1;t<=n;t++)
{
	c[i]+= (aa[i][t]*y[t]);
}

norm[i]= c[i]-f[i];
s+=(norm[i])*(norm[i]);
}

q=sqrt(s);
return q;
}*/

double maximum()
{double m=abs(f[1]-u(1*h));

for(int i=2;i<=n;i++) if(abs(f[i]-u(i*h))>m) m=abs(f[i]-u(i*h));	

	return m;
}

//вывод
void write()
{
ofstream fout("output2.txt");

fout<<"вектор решения:"<<endl;
for(int i=1;i<=n;i++) fout<<y[i]<<endl;fout<<" "<<endl;

fout<<"погрешность= "<<maximum();
//fout<<"невязка = "<<Nev();

fout.close();
}

int main() 
{
	read();
   search();
    write();
	return 0;
}
