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

fout<<eps<<endl;;

fout.close();
}

int main() {
	
	read();
	
	double y=-3+9*tau,eps;
	for(double i=tau;i<=T;i+=tau) 
	{
	y*=(1+y*tau);
	}
	
	eps=fabs(y+1/(T+1./3));
	
	write(eps);
	//write(y);
	
	return 0;
}
