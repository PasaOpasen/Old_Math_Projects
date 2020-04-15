
#include <cstdlib>
#include <iostream>
#include <stdio.h>
#include "ReadImage.h"
#include <math.h>

using namespace std;

int		h,w;
char		*R,*G,*B;
int		kr,kg,kb,kx,variant;

void Vvod();
void Ris1();									// изображение јндреевского флага
void Ris2();									// изображение эллипса
int f(int x,int y);
//---------------------------------------------------------------------------------------
int main()
{
	Vvod();
	GetSize("input1.bmp", &h, &w);				// читаем размеры файла в пиксел€х
											// (w -- ширина, h -- высота)
	R = new char[h*w];
	G = new char[h*w];
	B = new char[h*w];
	ReadImageFromFile("input1.bmp",B,G,R);			// считываем значени€ цветовых составл€ющих изображени€:
											// R -- красной, G -- зеленой, B -- синей
	switch (variant)
	{
		case 1:Ris1();break;
		case 2:Ris2();break;
	}
	WriteImage("output.bmp",B,G,R);				// создаем bmp-файл и записываем в него полученное "изображение"
}
//---------------------------------------------------------------------------------------
void Vvod()
{
	char		pust[100];
	float  scan;
	FILE *fp;									// указатель на файл
	fp=fopen("in.txt","r");						// открыть файл дл€ чтени€
											// чтение из файла
	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
	fscanf(fp,"%s",pust);
	fscanf(fp,"%s",pust);fscanf(fp,"%d",&kr);
	fscanf(fp,"%s",pust);fscanf(fp,"%d",&kg);
	fscanf(fp,"%s",pust);fscanf(fp,"%d",&kb);
	
	fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);fscanf(fp,"%s",pust);
	fscanf(fp,"%d",&kx);fscanf(fp,"%s",pust);
	fscanf(fp,"%s",pust);
	fscanf(fp,"%d",&kx);fscanf(fp,"%s",pust);
	fscanf(fp,"%s",pust);fscanf(fp,"%d",&variant);
	
	fclose(fp);								// закрыть файл дл€ чтени€
}
//---------------------------------------------------------------------------------------
void Ris1()
{
	double sd=1,								// начальное положение курсора
											// (эксперимент показал, что это лучше, чем =0)
	dsd=(double)(w-kx+1)/h;						// сдвиг по вертикали за один шаг по горизонтали
											// с учетом ширины полосы
	int sdv;
	for (int i=0;i<h;i++)
	{
		sdv=(int)sd;
		for (int j=sdv;j<sdv+kx;j++)
		{
			R[i*w+j]=kr;
			G[i*w+j]=kg;
			B[i*w+j]=kb;
		}
		sd+=dsd;
	}
	sd=1;
	for (int i=h-1;i>=0;i--)
	{
		sdv=(int)sd;
		for (int j=sdv;j<sdv+kx;j++)
		{
			R[i*w+j]=kr;
			G[i*w+j]=kg;
			B[i*w+j]=kb;
		}
		sd+=dsd;
	}   
}
//---------------------------------------------------------------------------------------
void Ris2()
{

	for (int y=0;y<750;y++)
	for (int x=0;x<1000;x++)
		{
			R[y*w+x]=kr;
			G[y*w+x]=kg;
			B[y*w+x]=kb;
		}
}

