#include <fstream>
#include <iostream>
#include <cmath>
using namespace std;

double tau, T;

void read()
{ifstream fin("input.txt");

fin>>tau>>T;

fin.close();
}

void write(double eps)
{ofstream fout("output.txt");

fout<<eps;

fout.close();
}

int main() {
	
	read();
	
	double y1=1,y2=-tau,eps;
	for(double i=tau;i<=T;i+=tau) 
	{
	  y2+=y1*(2-tau)-2*(y1+=tau*y2);
	}
	
	eps=fabs(y1-(1+T)*exp(-T));
	
	write(eps);
	
	return 0;
}
