#include "ReadImage.h"

int h,w;
char *R, *G,*B;
//
// ---------- Глобальные переменные
//
int  xWindow,yWindow;                       // ширина и высота окна
int  rColor,gColor,bColor;                  // цвет
int  xPoint,yPoint;                         // текущее положение курсора
int  x0Point,y0Point;                       // координвты "начала координат"
double aWindow,bWindow,cWindow,dWindow;     // координаты горизонт-х и вертик-х границ окна
double xMastab,yMastab;                     // масштабы по x и y
double kWindow=0.9;                         // к-т для отступа коорд. линии от границ окна

//
// ---------- Функции
//
void SetWindow(double a,double b,double c,double d);
                                            // создание окна по bmp-файлу
                                            // (размеры w и h -- определяются размерами
                                            // файла input.bmp);
                                            // устанавливаются параметры окна:
                                            // a,b -- горизонтальные пределы
                                            // c,d -- вертикальные пределы;
                                            // вычисляются необходимые параметры 
                                            // (масштабы, "начало координат")
                                            // и происходит цветовая заливка
void ReadWindow(char* FileName,double a,double b,double c,double d);
                                            // то же, что и SetWindow но без заливки окна
void CloseWindow();                         // закрытие окна
void SetColor(int r,int g,int b);           // задание цвета
void SetPoint(double x,double y);           // задание текущее положения курсора
void xyLine(double x0,double y0,double delx,double dely);
                                            // рисование осей координат:
                                            // (x0,y0) -- точка их пересечения,
                                            // delx,dely -- цена деления
                                            // горизонтальной и вертикальной оси
void Line(double x,double y);               // отрезок, соединяющий текущую точку с точкой (x,y)
void Line2(double x,double y);              // толстая Line
void Line3(double x,double y);              // очкнь толстая Line
// -----------------------------------------
void SetWindow(double a,double b,double c,double d)
{
     GetSize("input.bmp", &h, &w);
	 R = new char[h*w];
	 G = new char[h*w];
	 B = new char[h*w];
	 ReadImageFromFile("input.bmp",B,G,R);
     xWindow=w;
     yWindow=h;
                                            // параметры окна:
                                            // a,b -- горизонтальные пределы
                                            // c,d -- вертикальные пределы
                                            //
                                            // и вычисление необходимых параметров 
                                            // (масштабы, "начало координат")
     aWindow=a;
     bWindow=b;
     xMastab=xWindow*kWindow/(bWindow-aWindow);
     cWindow=c;
     dWindow=d;
     yMastab=yWindow*kWindow/(dWindow-cWindow);
     x0Point=(int)(xWindow*(1-kWindow)/2);
     y0Point=(int)(yWindow*(1-kWindow)/2);

     for (int i=0;i<xWindow*yWindow;i++) 
     {
         R[i]=rColor;
         G[i]=gColor;
         B[i]=bColor;
     }
}
// -----------------------------------------
void ReadWindow(char* FileName,double a,double b,double c,double d)
{
     GetSize(FileName, &h, &w);
	 R = new char[h*w];
	 G = new char[h*w];
	 B = new char[h*w];
	 ReadImageFromFile(FileName,B,G,R);
     xWindow=w;
     yWindow=h;
                                            // параметры окна:
                                            // a,b -- горизонтальные пределы
                                            // c,d -- вертикальные пределы
                                            //
                                            // и вычисление необходимых параметров 
                                            // (масштабы, "начало координат")
     aWindow=a;
     bWindow=b;
     xMastab=xWindow*kWindow/(bWindow-aWindow);
     cWindow=c;
     dWindow=d;
     yMastab=yWindow*kWindow/(dWindow-cWindow);
     x0Point=(int)(xWindow*(1-kWindow)/2);
     y0Point=(int)(yWindow*(1-kWindow)/2);
}
// -----------------------------------------
void CloseWindow()
{
     WriteImage("output.bmp", B,G,R);
}
// -----------------------------------------
void SetColor(int r,int g,int b)
{
     rColor=r;
     gColor=g;
     bColor=b;
}
// -----------------------------------------
void xyLine(double x0,double y0,double delx,double dely)
{
                                            // рисуем горизонтальную ось
    int yP=(int)((y0-cWindow)*yMastab)+y0Point;
    for (int j=x0Point/2;j<xWindow-x0Point/2;j++) 
    {
        int Point=yP*xWindow+j;
        R[Point]=rColor;
        G[Point]=gColor;
        B[Point]=bColor;
    }
                                            // рисуем стрелки
    int jl=5;
    int iN=yP-jl,iV=yP+jl;
    for (int j=xWindow-x0Point/2-jl;j<=xWindow-x0Point/2;j++)
        {
            int Point=iV*xWindow+j;
            R[Point]=rColor;
            G[Point]=gColor;
            B[Point]=bColor;
            Point=iN*xWindow+j;
            R[Point]=rColor;
            G[Point]=gColor;
            B[Point]=bColor;
            iV--;iN++;
        }
                                            // рисуем деления на оси
    for (double x=x0+delx;x<=bWindow;x+=delx)
    {
        int X=(int)((x-aWindow)*xMastab)+x0Point;
        for (int i=yP-jl;i<yP+jl;i++)
        {
            int Point=i*xWindow+X;
            R[Point]=rColor;
            G[Point]=gColor;
            B[Point]=bColor;
        }
    }
    for (double x=x0-delx;x>=aWindow;x-=delx)
    {
        int X=(int)((x-aWindow)*xMastab)+x0Point;
        for (int i=yP-jl;i<yP+jl;i++)
        {
            int Point=i*xWindow+X;
            R[Point]=rColor;
            G[Point]=gColor;
            B[Point]=bColor;
        }
    }
                                            // рисуем вертикальную ось
    int xP=(int)((x0-aWindow)*xMastab)+x0Point;
    for (int i=y0Point/2;i<yWindow-y0Point/2;i++) 
    {
        int Point=i*xWindow+xP;
        R[Point]=rColor;
        G[Point]=gColor;
        B[Point]=bColor;
    }
                                            // рисуем стрелки
    int il=5;
    int iL=xP-il,iP=xP+il;
    for (int i=yWindow-y0Point/2-il;i<=yWindow-y0Point/2;i++)
        {
            int Point=i*xWindow+iP;
            R[Point]=rColor;
            G[Point]=gColor;
            B[Point]=bColor;
            Point=i*xWindow+iL;
            R[Point]=rColor;
            G[Point]=gColor;
            B[Point]=bColor;
            iP--;iL++;
 
       }
                                            // рисуем деления на оси
    for (double y=y0+dely;y<=dWindow;y+=dely)
    {
        int Y=(int)((y-cWindow)*yMastab)+y0Point;
        for (int j=xP-il;j<xP+il;j++)
        {
            int Point=Y*xWindow+j;
            R[Point]=rColor;
            G[Point]=gColor;
            B[Point]=bColor;
        }
    }
    
    for (double y=y0-dely;y>=cWindow;y-=dely)
    {
        int Y=(int)((y-cWindow)*yMastab)+y0Point;
        for (int j=xP-il;j<xP+il;j++)
        {
            int Point=Y*xWindow+j;
            R[Point]=rColor;
            G[Point]=gColor;
            B[Point]=bColor;
        }
    }
}
// -----------------------------------------
void SetPoint(double x,double y)
{
     xPoint=(int)((x-aWindow)*xMastab)+x0Point;
     if (y>dWindow) y=dWindow;
     if (y<cWindow) y=cWindow;
     yPoint=(int)((y-cWindow)*yMastab)+y0Point;
}
// -----------------------------------------
void Line(double x,double y)
{
    if (x>bWindow) x=bWindow;
    if (x<aWindow) x=aWindow;
    if (y>dWindow) y=dWindow;
    if (y<cWindow) y=cWindow;
    int xP=(int)((x-aWindow)*xMastab)+x0Point;
    int yP=(int)((y-cWindow)*yMastab)+y0Point;
    int a,b,c,d;
    if (xP>=xPoint)
    {
        a=xPoint;c=yPoint;
        b=xP;d=yP;
    }
    else
    {
        a=xP;c=yP;
        b=xPoint;d=yPoint;
    }
   	xPoint=xP;                              // запоминаем новые координаты курсора
    yPoint=yP;
				                            // рисуем отрезок [(a,c),(b,d)]
   	double s=0,ds;                          // сдвиг и сдвиг по y за один шаг по x
    if (a==b) ds=d-c;
    else ds=((double)(d-c))/(b-a);
    int i=a,j1=c,j2;
    do
    {
        s+=ds;
        j2=c+(int)s;
        if (j2>=j1)							// если график не убывает
            for (int j=j1;j<=j2;j++)		// рисуем точку (i,j)
            {
                int Point=j*xWindow+i;
                R[Point]=rColor;
                G[Point]=gColor;
                B[Point]=bColor;
            }
        else								// если график убывает
            for (int j=j1;j>=j2;j--)
            {
                int Point=j*xWindow+i;
                R[Point]=rColor;
                G[Point]=gColor;
                B[Point]=bColor;
            }
        j1=j2;
        i++;
    }
    while (i<b);
}
// -----------------------------------------
void Line2(double x,double y)
{
    if (x>bWindow) x=bWindow;
    if (x<aWindow) x=aWindow;
    if (y>dWindow) y=dWindow;
    if (y<cWindow) y=cWindow;
    int xP=(int)((x-aWindow)*xMastab)+x0Point;
    int yP=(int)((y-cWindow)*yMastab)+y0Point;
    int a,b,c,d;
    if (xP>=xPoint)
    {
        a=xPoint;c=yPoint;
        b=xP;d=yP;
    }
    else
    {
        a=xP;c=yP;
        b=xPoint;d=yPoint;
    }
   	xPoint=xP;                              // запоминаем новые координаты курсора
    yPoint=yP;
				                            // рисуем отрезок [(a,c),(b,d)]
   	double s=0,ds;                          // сдвиг и сдвиг по y за один шаг по x
    if (a==b) ds=d-c;
    else ds=((double)(d-c))/(b-a);
    int i=a,j1=c,j2;
    do
    {
        s+=ds;
        j2=c+(int)s;
        if (j2>=j1)							// если график не убывает
            for (int j=j1;j<=j2;j++)		// рисуем точку (i,j)
            {
                int Point=j*xWindow+i;
                R[Point]=R[Point+1]=R[Point+xWindow]=R[Point+xWindow+1]=rColor;
                G[Point]=G[Point+1]=G[Point+xWindow]=G[Point+xWindow+1]=gColor;
                B[Point]=B[Point+1]=B[Point+xWindow]=B[Point+xWindow+1]=bColor;
            }
        else								// если график убывает
            for (int j=j1;j>=j2;j--)
            {
                int Point=j*xWindow+i;
                R[Point]=R[Point+1]=R[Point+xWindow]=R[Point+xWindow+1]=rColor;
                G[Point]=G[Point+1]=G[Point+xWindow]=G[Point+xWindow+1]=gColor;
                B[Point]=B[Point+1]=B[Point+xWindow]=B[Point+xWindow+1]=bColor;
            }
        j1=j2;
        i++;
    }
    while (i<b);
}
// -----------------------------------------
void Line3(double x,double y)
{
    if (x>bWindow) x=bWindow;
    if (x<aWindow) x=aWindow;
    if (y>dWindow) y=dWindow;
    if (y<cWindow) y=cWindow;
    int xP=(int)((x-aWindow)*xMastab)+x0Point;
    int yP=(int)((y-cWindow)*yMastab)+y0Point;
    int a,b,c,d;
    if (xP>=xPoint)
    {
        a=xPoint;c=yPoint;
        b=xP;d=yP;
    }
    else
    {
        a=xP;c=yP;
        b=xPoint;d=yPoint;
    }
   	xPoint=xP;                              // запоминаем новые координаты курсора
    yPoint=yP;
				                            // рисуем отрезок [(a,c),(b,d)]
   	double s=0,ds;                          // сдвиг и сдвиг по y за один шаг по x
    if (a==b) ds=d-c;
    else ds=((double)(d-c))/(b-a);
    int i=a,j1=c,j2;
    do
    {
        s+=ds;
        j2=c+(int)s;
        if (j2>=j1)							// если график не убывает
            for (int j=j1;j<=j2;j++)		// рисуем точку (i,j)
            {
                int Point=j*xWindow+i;
                R[Point]=R[Point+1]=R[Point+2]=
                R[Point+xWindow]=R[Point+xWindow+1]=R[Point+xWindow+2]=
                R[Point+2*xWindow]=R[Point+2*xWindow+1]=R[Point+2*xWindow+2]=rColor;
                G[Point]=G[Point+1]=G[Point+2]=
                G[Point+xWindow]=G[Point+xWindow+1]=G[Point+xWindow+2]=
                G[Point+2*xWindow]=G[Point+2*xWindow+1]=G[Point+2*xWindow+2]=gColor;
                B[Point]=B[Point+1]=B[Point+2]=
                B[Point+xWindow]=B[Point+xWindow+1]=B[Point+xWindow+2]=
                B[Point+2*xWindow]=B[Point+2*xWindow+1]=B[Point+2*xWindow+2]=bColor;
            }
        else								// если график убывает
            for (int j=j1;j>=j2;j--)
            {
                int Point=j*xWindow+i;
                R[Point]=R[Point+1]=R[Point+2]=
                R[Point+xWindow]=R[Point+xWindow+1]=R[Point+xWindow+2]=
                R[Point+2*xWindow]=R[Point+2*xWindow+1]=R[Point+2*xWindow+2]=rColor;
                G[Point]=G[Point+1]=G[Point+2]=
                G[Point+xWindow]=G[Point+xWindow+1]=G[Point+xWindow+2]=
                G[Point+2*xWindow]=G[Point+2*xWindow+1]=G[Point+2*xWindow+2]=gColor;
                B[Point]=B[Point+1]=B[Point+2]=
                B[Point+xWindow]=B[Point+xWindow+1]=B[Point+xWindow+2]=
                B[Point+2*xWindow]=B[Point+2*xWindow+1]=B[Point+2*xWindow+2]=bColor;
            }
        j1=j2;
        i++;
    }
    while (i<b);
}
