#include <cstdlib>
#include <iostream>
#include <stdio.h>
#include "ReadImage.h"
#include <math.h>

using namespace std;

double	a, b, c, d;
int h, w, n, i, j;
double q[100];
int r[100], g[100], t[100];
char *R, *G, *B;

void Vvod();
void grafic();
double f(double x, double y);




int main()
{
	Vvod();
	GetSize("input1.bmp", &h, &w);
	// (w -- ширина, h -- высота)
	R = new char[h*w];
	G = new char[h*w];
	B = new char[h*w];
	ReadImageFromFile("input.bmp", B, G, R);

	grafic();

	WriteImage("output.bmp", B, G, R);
}






void Vvod()
{
	char pust[100];
	float  scan;
	FILE *fp;
	fp = fopen("in(2).txt", "r");

	fscanf(fp, "%s", pust); fscanf(fp, "%s", pust);
	fscanf(fp, "%s", pust); fscanf(fp, "%d", &scan); a = scan;
	fscanf(fp, "%s", pust); fscanf(fp, "%d", &scan); b = scan;
	fscanf(fp, "%s", pust); fscanf(fp, "%d", &scan); c = scan;
	fscanf(fp, "%s", pust); fscanf(fp, "%d", &scan); d = scan;
	fscanf(fp, "%s", pust); fscanf(fp, "%d", &scan); n = (int)scan;
	fscanf(fp, "%s", pust);

	for (i = 0; i<n; i++)
	{
		fscanf(fp, "%d", &j);
		q[i] = j;
	}

	fscanf(fp, "%s", pust);

	for (i = 0; i <= n; i++)
	{
		fscanf(fp, "%d", r[i], g[i], t[i]);
	}


	fclose(fp);
}




void grafic()
{
	double dx = (b - a) / w, dy = (d - c) / h;
	int m = 0;
	int ur;

	for (double y = c + dy / 2; y<d; y += dy)
	{
		for (double x = a + dx / 2; x<b; x += dx)
		{
			ur = n;
			i = 1;

			while (i <= n)
			{
				if (f(x, y)<q[i]) i++;
				else { ur = i - 1; break; }
			}

			R[m] = r[ur];
			G[m] = g[ur];
			B[m] = t[ur];
			m++;
		}
	}
}

double f(double x, double y)
{
	return 2 * x + 3 * y + 2;
}
