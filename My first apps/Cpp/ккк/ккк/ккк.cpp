// ккк.cpp: определяет точку входа для консольного приложения.
//

#include "stdafx.h"




#include <iostream>
using namespace std;void read(double x);
void write(double t);

int _tmain(int argc, _TCHAR* argv[])
{

	double e;

	read(e);
	write(e);



	return 0;
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