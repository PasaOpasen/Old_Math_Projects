#include <iostream>
using namespace std;
void read(double x);
void write(double t);

void main()
{
	double e;

	read(e);
	write(e);


}
void read(double x)
{
	FILE *qw=fopen("vvod.txt","r");

	fscanf(qw,"%d",&x);
	fclose(qw);
}

void write(double t)
{
	FILE *ww=fopen("vivod.txt","w");

	fprintf(ww,"%d",&t);
	fclose(ww);
}

