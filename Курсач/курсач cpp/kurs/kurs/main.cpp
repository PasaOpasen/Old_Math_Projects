//программа, которая из дискретного множества точек находит такие две тройки, что треугольники, образованные этими тройками точек как вершинами, являются друг для друга внешним и внутренним соответственно; 
             //программа иллюстрирует решение, если оно найдено
//написал ПАСЬКО Д. А.
//в последний раз программа редактировалась 30.04.2017
//сложность могут вызвать функции search() и illustrating()
//при организации кода я пытался разбить программу на как можно большее число независимых фрагментов
//требуется подумать над иллюстрацией и исправить ошибки, связанные со структурами (ли?)


//директивы препроцессора
#include <iostream>
#include <fstream>
#include <algorithm>
#include <cmath>
#include "Graph.h"
//стандартное пространство имён
using namespace std;


struct Point {//структрура ТОЧКА на плоскости
	double x;//поле - координата по х
	double y;
};

//глобальные переменные
int m;//объявление мощности множества точек плоскости
int kt;//объявление мощности множества РАЗЛИЧНЫХ точек плоскости, то есть множества точек после отсеивания повторяющихся
Point z1[3], z2[3]; //два маленьких массива, в которых будут располагаться вершины найденных треугольников 
Point *mas;//указатель на массив точек плоскости
double a0, b0, c0, d0;//пределы окна
const double eps = 0.01;//погрешность
const int nRis = 250;//количество шагов (нужно при иллюстрировании)


//перечисление прототипов вспомогательных функций
void building();
void search();
void illustrating();
void read();
void screening();
double P(double a, double b, double c);
void write1();
void write2();
void exceptionmas(Point *mas);
void excep(int i, Point *a);
void Ris(Point *z, int i, int j);
double f(double x, Point *z, int i, int j);
double min3(double a, double b, double c);
double max3(double a, double b, double c);
double eudistance(Point a, Point b);
bool triangleanswer(Point mas1, Point mas2, Point mas3);
bool triangleanswer(Point *a, int k);
int compx(Point a, Point b);
int compy(Point a, Point b);




//основная функция
int main()
{
	building();//чтение и работа с данными
	search();//поиск решения

//окончание программы
	return 0;
}



//тела вспомогательных функций для main (основной функции)
void building()
{
	read();//чтение данных из файла
	screening();//отсеивание из массива точек одинаковых точек

}
void search()
{
	Point* internalpoints = new Point[kt - 3];//массив "внутренних точек", в который для каждого потенциально внешнего треугольника будут записываться внутренние точки, ибо не любая тройка из них будет составлять треугольник, так что нужна проверка

	for (int t11 = 0; t11 < (kt - 2); t11++)//цикл поиска точек первого треугольника
	{
		for (int t12 = t11++; t12 < (kt - 1); t12++)
			for (int t13 = t12++; t13 < (kt); t13++)
			{
				double a, b, c;//объявление сторон треугольника
				a = eudistance(mas[t11], mas[t12]);//расстояние между точками t11 и t12
				b = eudistance(mas[t13], mas[t12]);//расстояние между точками t13 и t12
				c = eudistance(mas[t11], mas[t13]);//расстояние между точками t11 и t13
				if (triangleanswer(mas[t11], mas[t12], mas[t13]))//проверка, составляют ли точки треугольник
				{
					z1[1] = mas[t11];
					z1[2] = mas[t12];
					z1[3] = mas[t13]; //фиксация точек внешнего треугольника в его массив

					double p1, p2, p3;//объявление расстояний до найденных вершин
					int k = -1; //индекс, равный количеству найденных точек внутри треугольника
					for (int s = 0; s < kt; s++)
					{
						if (triangleanswer(mas[s], mas[t12], mas[t13]) && triangleanswer(mas[t11], mas[s], mas[t13]) && triangleanswer(mas[t11], mas[t12], mas[s]))//если точка образует треугольники с вершинами уже найденного треугольника
						{

							p1 = eudistance(mas[t11], mas[s]);//расстояние между точками t11 и s
							p2 = eudistance(mas[t12], mas[s]);//расстояние между точками t12 и s
							p3 = eudistance(mas[t13], mas[s]);//расстояние между точками t13 и s

							if (max3(p1, p2, p3) >= max3(a, b, c)) continue; //проверка необходимого условия
							if (P(a, p1, p2) + P(b, p2, p3) + P(c, p1, p3) > P(a, b, c)) continue;//проверка достаточного условия (неравенство площадей Р)
							k++;//одна из точек внутреннего треугольника найдена, увеличивается счётчик
							internalpoints[k] = mas[s];//фиксация найденной точки
						}
					}
					if (triangleanswer(internalpoints, k))//если в массиве внутренних точек существует тройка, образующая треугольник
					{
						write1();//вывод точек
						//переход к иллюстрированию, ибо решение найдено	
						illustrating();//иллюстрирование решения
						return;//прекращение работы программы (завершение функции поиска)
					}

				}
			}
	}
	write2();//если, дойдя до этого момента, программа не нашла решения, сообщается о том, что решения не существует
	exit(0);//...и программа прекращает работу
}


//тела вспомогательных функций следующего порядка

   //вспомогательные для "чтения данных и работы с данными"
void read()
{
	char buff[50];//создание вспомогательного символьного массива
	int k = 0; //счётчик
	ifstream fin("input.txt"); //объявление объекта для чтения из файла
	fin >> buff;//чтение ненужной текстовой информации
	while (fin >> m) k++;//пока координаты точек считываются, прибавлять к счётчику 
	m = k / 2;//вычисление мощности множества
	if (m < 6) { ofstream fout("output.txt"); fout << "точек слишком мало"; fout.close(); exit(0); }//условие на мощность множества
	mas = new Point[m];//создание массива точек

	ifstream file("input.txt");//поставить указатель на начало файла, чтобы считать значения заново
	fin >> buff;
	for (int i = 0; i < m; i++) fin >> mas[i].x >> mas[i].y;//заполнение массива точек
	fin.close();
}
void screening()
{
	sort(mas, mas + m, compx);//сортировка точек по компаратору (по первой координате)
	int i = 0;
	for (int j = 1; j < m; j++)
	{
		if (mas[j - 1].x != mas[j].x)//если соседние элементы имеют разную координату по х
		{
			sort(mas + i, mas + j - 1, compy);//сортировать участок, где одинакова координата по х, по второй координате
			i = j;
		}
	}
	exceptionmas(mas);//отсеивание повторяющихся точек

	if (kt < 6) { ofstream fout("output.txt"); fout << "разных точек слишком мало"; fout.close(); exit(0); }//условие на мощность множества
}

  //вспомогательные для "поиска"
double P(double a, double b, double c)
{
	double p, s;
	p = (a + b + c) / 2;//полупериметр
	s = sqrt(p*(p - a)*(p - b)*(p - c));//площадь
	return s;
}

void write1()
{
	ofstream fout("output.txt");
	fout << "вершины первого треугольника" << endl;
	for (int i = 0; i < 3; i++)fout << z1[i].x << " " << z1[i].y << endl;
	fout << "вершины второго треугольника" << endl;
	for (int i = 0; i < 3; i++)fout << z2[i].x << " " << z2[i].y << endl;
	fout.close();
}

void write2()
{
	ofstream fout("output.txt"); fout << "точки не найдены"; fout.close();
}


int compx(Point a, Point b)//функция компоратора для первой координаты
{
	return a.x < b.x;//сравнение по первой координате
}

int compy(Point a, Point b)//функция компоратора для второй координаты
{
	return a.y < b.y;//сравнение по второй координате
}

void exceptionmas(Point *mas)//отсеивание из массива повторяющихся элементов
{
	kt = m;
	for (int i = 1; i < kt; i++)
	{
		if (mas[i - 1].x == mas[i].x && mas[i - 1].y == mas[i].y) { excep(i, mas); kt--; i--; }//удаление из массива точек повторяющихся элементов
	}
}

void excep(int i, Point *a)//удаление из массива элемента i
{
	for (int j = i; j < kt - 1; j++) a[j] = a[j++];
}  
double min3(double a, double b, double c)//минимум из тройки элементов
{
	return min(min(a, b), c);
}

double max3(double a, double b, double c)//максимум из тройки элементов
{
	return max(max(a, b), c);
}

double eudistance(Point a, Point b)//расстояние между точками
{
	return sqrt(pow((a.x - b.x), 2) + pow((a.y - b.y), 2));
}

bool triangleanswer(Point mas1, Point mas2, Point mas3)
{
	double a, b, c;//объявление сторон треугольника
	a = eudistance(mas1, mas2);//расстояние между точками mas1 и mas2
	b = eudistance(mas3, mas2);//расстояние между точками mas3 и mas2
	c = eudistance(mas1, mas3);//расстояние между точками mas1 и mas3

	if ((a + b < c) && (a + c < b) && (c + b < a))//проверка неравенств треугольника
		return true;
	else return false;
}

bool triangleanswer(Point *a, int k)
{
	for (int t1 = 0; t1 < (k + 1 - 2); t1++)//цикл поиска точек треугольника
		for (int t2 = t1++; t2 < (k + 1 - 1); t2++)
			for (int t3 = t2++; t3 < k + 1; t3++)
			{
				if (triangleanswer(a[t1], a[t2], a[t3]))//если точки составляют треугольник, то они сразу записываются в массив точек внутреннего треугольника
				{
					z2[1] = a[t1];
					z2[2] = a[t2];
					z2[3] = a[t3];
					return true;
				}
				else return false;
			}
}  

  //иллюстрирование
void illustrating()
{
	//инициализация пределов окна
	a0 = -10 + min3(z1[1].x, z1[2].x, z1[3].x);
	b0 = 20 + max3(z1[1].x, z1[2].x, z1[3].x);
	c0 = -10 + min3(z1[1].y, z1[2].y, z1[3].y);
	d0 = 20 + max3(z1[1].y, z1[2].y, z1[3].y);


	SetColor(250, 250, 250);	// задаем фоновый цвет окна (белый)
	SetWindow(a0, b0, c0, d0);	// создаём окно (создаем массивы R,G,B)
	// с пределами [a,b]x[c,d]
	SetColor(0, 0, 0);	// задаем цвет координатных осей (чёрный)
	xyLine(a0, 0, 1, 1);	// строим оси, пересекающиеся в т. (a,0)
	// с шагом делений по х равным 1
	// и 1 по у

	// рисуем график
	Ris(z1, 1, 2); //отрезок между первой и второй вершиной первого треугольника
	Ris(z1, 3, 2);//отрезок между третьей и второй вершиной первого треугольника
	Ris(z1, 1, 3);//отрезок между первой и третьей вершиной первого треугольника
	Ris(z2, 1, 2);
	Ris(z2, 3, 2);
	Ris(z2, 1, 3);//отрезок между первой и третьей вершиной второго треугольника

	CloseWindow();// закрываем окно (создаем bmp-файл)

}

void Ris(Point *z, int i, int j)//изображение отрезка между точками zi и zj
{
	SetColor(0, 255, 0); // задаем цвет линии
	double step = (b0 - a0) / nRis;
	double x = b0;
	SetPoint(x, f(x, z, i, j)); // устанавливаем курсор
	for (int i = 0; i < nRis; i++)
	{
		x -= step;
		Line2(x, f(x, z, i, j));// строим отрезок,соединяющий курсор с точкой (х,у), и перемещаем курсор в эту точку	
		if (f(x, z, i, j) <= c0 + eps) break;
	}
}

double f(double x, Point *z, int i, int j)//прямая между точками zi и zj
{
	if ((z[j].x - z[i].x) == 0) return 0;
	else return (x*(z[j].y - z[i].y) - z[i].x*z[j].y + z[j].x*z[i].y) / (z[j].x - z[i].x);
}


