#include <fstream>
#include <iostream>
#include <cmath>
#include <ctime>
#include <iomanip>
using namespace std;

double h, k, x, y,**a,**b,eps,mtime,beg;
int kk;
double timeBegin,timeEnd;

void read()
{ifstream fin("input.txt");

fin>>h>>k>>x>>y;
if (3/h-int(3/h)<0.0001 )kk=int(3/h);//от 0+ до 3 
  else kk=int(3/h)+1;

a = new double*[kk+1];//массив от 0 до 3, включая 0
b = new double*[kk+1];
for(int i=0;i<kk+1;i++) 
{
 a[i] = new double[kk+1];
 b[i] = new double[kk+1]; 	
}
fin.close();
}

void write(double eps,double time)
{ofstream fout("output.txt");

fout<<eps<<' '<<setprecision(6)<<time<<endl;

fout.close();
}

void z()
{
	//заполнение граничных узлов
	for(int j=0;j<=kk;j++) 
	{//внешние
		b[j][0]=j*j*h*h-0*0;
		b[j][kk]=j*j*h*h-3*3;
		b[0][j]=0*0-j*j*h*h;
		b[kk][j]=3*3-j*j*h*h;
	}
	for(int j=kk/3;j<=2*kk/3;j++)  
	{//внутренние
		b[j][kk/3]=j*j*h*h-1*1;
		b[j][2*kk/3]=j*j*h*h-2*2;
		b[kk/3][j]=1*1-j*j*h*h;
		b[2*kk/3][j]=2*2-j*j*h*h;
	}
	
	beg = 0;
		
	for(int j=1;j<kk;j++)
	{
		for(int i=1;i<kk;i++)
		{
			if(i==2*kk/3 || i==kk/3 || j==2*kk/3 || j==kk/3) continue;//если точка не попадает на внутренний квадрат
			b[i][j]= beg;// начальные значения
		}
	}

    //вывод
    
//    ofstream fout("output.txt");
//   	for(int j=kk;j>=0;j--)
//	{
//		for(int i=0;i<=kk;i++)
//		{
//			fout<</*"("<<setprecision(2)<<i*h<<","<<setprecision(2)<<j*h<<")= "<<*/setprecision(1)<<b[i][j]<<" \t";
//		}
//		fout<<endl;
//	}fout<<endl;

}

void zz()
{
	for(int n=1;n<=k;n++)//по итерациям
   {
   	
   	for(int j=1;j<kk;j++)
	{
		for(int i=1;i<kk;i++)
		{
			if(i>=kk/3 && i<=2*kk/3 && j>=kk/3 && j<=2*kk/3) continue;//если точка не попадает в внутренний квадрат 
			a[i][j]=(b[i-1][j]+b[i+1][j]+b[i][j-1]+b[i][j+1])/4;//заполнение новой этерации
		}
	}
		
	//заполнить старый массив
	for(int j=1;j<kk;j++)
	{
		for(int i=1;i<kk;i++)
		{
			if(i>=kk/3 && i<=2*kk/3 && j>=kk/3 && j<=2*kk/3) continue;//если точка не попадает в внутренний квадрат 
			b[i][j]=a[i][j];
		}
	}
  }
  
  //вывод
//	ofstream fout("output.txt");
//   	for(int j=kk;j>=0;j--)
//	{
//		for(int i=0;i<=kk;i++)
//		{
//			fout<</*"("<<setprecision(2)<<i*h<<","<<setprecision(2)<<j*h<<")= "<<*/setprecision(1)<<b[i][j]<<" \t";
//		}
//		fout<<endl;
//	}fout<<endl;
  
}

int main() {
	read();
	
	z();//заполнение
	 
	timeBegin = clock();
		
	zz();//выполнение
	
	timeEnd = clock(); 
	
	mtime = timeEnd-timeBegin; 
	eps=fabs(a[int(x/h)][int(y/h)]-x*x+y*y);
	
	cout<<a[int(x/h)][int(y/h)]<<' '<<x*x-y*y<<' '<<eps;
		
	write(eps,mtime);
	return 0;
}
