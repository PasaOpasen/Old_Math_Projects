//программа, которая находит приближённое решение задачи Дирихле для уравнения Лапласа
//написал ПАСЬКО Д. А.
//в последний раз программа редактировалась 19.10.2017
//при организации кода я пытался разбить программу на как можно большее число независимых фрагментов
//программа ещё не готова

//set_locale

//стандартные директивы препроцессора
#include <iostream>
#include <fstream>
#include <algorithm>
#include <cstdlib>
#include <cmath>
//стандартное пространство имён
using namespace std;



//структура точки на плоскости
struct basp {
	double x;//координаты точки в двумерном пространстве
	double y;
	//функция базисного потенциала в точке z, соответствующая исходной базисной точке
	double potfunc(basp z) {
		return log(1 / sqrt((z.x - x)*(z.x - x) + (z.y - y)*(z.y - y)));
	}
};
//расстояние между точками
double eudistance(basp z, basp w) {
	return sqrt((z.x - w.x)*(z.x - w.x) + (z.y - w.y)*(z.y - w.y));
}
double fpow(int i, int j, basp z);



int M0 = 1000; //количество шагов при интегрировании

//класс плоских кривых 
class curve {
private:
	double a;//начальное значение параметра
	double b;//конечное значение параметра
	double(*u)(double);
	double(*v)(double);
	double h;//значение шага для этой кривой
	int M;
	void geth(int MM) {
		M = MM;
		h = (b - a) / M;//присвоение шагу конкретного значения
	}
	basp transfer(double t) {//возврат точки на кривой по значению параметра
		basp point;
		point.x = u(t);
		point.y = v(t);
		return point;
	}

public:

	curve(double a0, double b0, double(*uu)(double), double(*vv)(double)) {//конструктор
		a = a0; b = b0;
		u = uu; v = vv;
	}
	curve() {
		a = 0; b = 0;
	}

	double firstkind(double(*f)(basp)) {//вычисление криволинейного интеграла первого рода по этой кривой от функции f
		geth(M0);
		double value = 0;
		for (int k = 0; k <= M - 1; k++) value += f(transfer((a + (k + 1)*h + a + (k)*h) / 2))*eudistance(transfer(a + (k + 1)*h), transfer(a + (k)*h));
		return value;
	}

	double firstkind(int i, int j) {//вычисление криволинейного интеграла первого рода по этой кривой от функции fpow(int i,int j,basp z)
		geth(M0);
		double value = 0;
		for (int k = 0; k <= M - 1; k++) value += fpow(i, j, transfer((a + (k + 1)*h + a + (k)*h) / 2))*eudistance(transfer(a + (k + 1)*h), transfer(a + (k)*h));
		return value;
	}
};

//класс СЛАУ с методами их решения
class syst {
private:
	int n;//размерность системы
public:
	double **a, *b, *x;//указатели на матрицу и векторы

	void make(int k) {}//создание двумерного и одномерных динамических массиво
		n = k;
		a = new double *[n];
		for (int i = 0; i < n; i++)
			a[i] = new double[n];
		b = new double[n];
		x = new double[n];
	}
	
	
	void Gauss(){
		
	}
	
	void Holets() {} //решение уравнения Ах=b методом Холецкого, присвоение вектору х значений решени
	  //создание вспомогательной матрицы
		double **t = new double *[n];
		for (int i = 0; i < n; i++)
			t[i] = new double[n];

		//прямой ход метода
		t[0][0] = sqrt(a[0][0]);
		for (int j = 1; j < n; j++)t[0][j] = sqrt(a[0][j]) / t[0][0];

		for (int j = 1; j < n; j++) {
			double sum = 0;
			for (int k = 0; k <= j - 1; k++) sum += t[k][j] * t[k][j];
			t[j][j] = sqrt(a[j][j] - sum);
		}

		for (int i = 0; i < n; i++)
			for (int j = 0; j < n; j++)
			{
				if (i < j) {
					double sum = 0;
					for (int k = 0; k <= i - 1; k++) sum += t[k][i] * t[k][j];
					t[i][j] = (a[i][j] - sum) / t[i][i];
				}
				else if (j < i) {
					t[i][j] = 0;
				}
			}

//
		double **c = new double *[n];
		for (int i = 0; i < n; i++)
			c[i] = new double[n];
		for (int i = 0; i < n; i++)for (int j = 0; j < n; j++) {
			double s = 0;
			for (int k = 0; k < n; k++)s += t[k][i] * t[k][j];
			c[i][j] = s;
		}

		cout << "матрица c:" << endl;
		for (int i = 0; i < n; i++)
		{
			cout << endl;
			for (int j = 0; j < n; j++)cout << c[i][j] << " ";
		}


		cout << "матрица t:" << endl;
		for (int i = 0; i < n; i++)
		{
			cout << endl;
			for (int j = 0; j < n; j++)cout << t[i][j] << " ";
		}

//

		//обратный ход метода     
		double *y = new double[n];
		y[0] = b[0] / t[0][0];
		for (int i = 1; i < n; i++) {
			double sum = 0;
			for (int k = 0; k <= i - 1; k++) sum += t[k][i] * y[k];
			y[i] = (b[i] - sum) / t[i][i];
		}
		x[n - 1] = y[n - 1] / t[n - 1][n - 1];
		for (int i = n - 2; i > 0; i--)
		{
			double sum = 0;
			for (int k = i + 1; k < n; k++) sum += t[i][k] * x[k];
			x[i] = (y[i] - sum) / t[i][i];
		}
	}


};


//глобальные переменные
basp *mas;//указатель на массив точек плоскости
curve mycurve;//граница области, в которой решается диф. уравнение
syst mysystem;//система, которую придётся решить
double(*fi)(basp);//указатель на граничную функцию
int circle, gf, N;//номер области, граничной функции и количество базисных точек

const int kgf = 2;//количество граничных функций на одну кривую





//возможные параметризации для области
	//круг радиуса 3
double u1(double t) {
	return 3 * cos(t);
}
double v1(double t) {
	return 3 * cos(t);
}
//равносторонний треугольник со стороной 2
double u2(double t) {
	if ((t >= 0) && (t <= 4))return t;
	if ((t >= 4) && (t <= 6))return 12 - 2 * t;
}
double v2(double t) {
	if ((t >= 0) && (t <= 2))return t*sqrt(3);
	if ((t >= 2) && (t <= 4))return -sqrt(3)*t + 4 * sqrt(3);
	if ((t >= 4) && (t <= 6))return 0;
}
//квадрат со стороной 4
double u3(double t) {
	if ((t >= 0) && (t <= 4))return t;
	if ((t >= 4) && (t <= 8))return 4;
	if ((t >= 8) && (t <= 12))return 12 - t;
	if ((t >= 12) && (t <= 16))return 0;
}
double v3(double t) {
	if ((t >= 0) && (t <= 4))return 0;
	if ((t >= 4) && (t <= 8))return t - 4;
	if ((t >= 8) && (t <= 12))return 4;
	if ((t >= 12) && (t <= 16))return 16 - t;
}

//граничные функции и массив граничных функций
double g11(basp point) {
	return cos(point.x)*cos(point.y);
}
double g12(basp point) {
	return sin(point.y);
}
double g21(basp point) {
	return point.x*point.x + 4;
}
double g22(basp point) {
	return pow(point.x, 5) + point.x*point.y*point.y;
}
double g31(basp point) {
	return log(abs(point.x*point.y) + 2 * point.x);
}
double g32(basp point) {
	return cos(2 * point.x) / cos(point.x*point.y); //возможна проблема из-за особых точек
}
double(*gfunctions[3][kgf])(basp) = { g11,g12,g21,g22,g31,g32 };

//функция произведений функций
double fpow(int i, int j, basp z) {
	if ((i == N) && (j == N)) return fi(z)*fi(z);
	if (i == N) return mas[j].potfunc(z)*fi(z);
	if (j == N) return mas[i].potfunc(z)*fi(z);
	return mas[i].potfunc(z)*mas[j].potfunc(z);
}

//перечисление прототипов вспомогательных функций
void building();
void read();
void screening();
bool comp(basp a, basp b);
void exceptionmas(basp *mas);
void excep(int i, basp *a);
void display();
void writeAerror();
void writeerror(double eps);
void matrixpower(double *Ax, double **a, double *x);
void error();
void search();


//основная функция
int main()
{
	 setlocale( LC_ALL,"Russian" );
	
	building();//чтение и работа с данными
	search();//поиск решения
	return 0;
}



void building()
{
	read();//чтение данных из файла
	display();//сопоставление первым двум числам из файла - области и граничной функции
	screening();//отсеивание из массива точек одинаковых точек

   /*cout<<circle<<" "<<gf<<endl;
   for (int i = 0; i < N; i++) cout<<mas[i].x <<" "<< mas[i].y<<endl;*/
}

//вспомогательные для "чтения данных и работы с данными"
void read()
{
	char buff[150];//создание вспомогательного символьного массива
	int k = 0; //счётчик
	double r;//вспомогательное действительное число	
	ifstream fin("input.txt"); //объявление объекта для чтения из файла
	fin >> buff;//чтение ненужной текстовой информации
	fin >> circle;//чтение номера области
	fin >> buff;
	fin >> gf;//чтение номера граничной функции
	fin >> buff;

	while (fin >> r) k++;//пока координаты точек считываются, прибавлять к счётчику 
	N = k / 2;//вычисление мощности множества базисных точек
	mas = new basp[N];//создание массива точек

	fin.clear();
	fin.seekg(0, ios::beg);//поставить указатель на начало файла, чтобы считать значения заново
	fin >> buff; fin >> circle; fin >> buff; fin >> gf; fin >> buff;
	for (int i = 0; i < N; i++) fin >> mas[i].x >> mas[i].y;//заполнение массива точек
	fin.close();
}

void screening()
{
	sort(mas, mas + N, comp);//сортировка точек по компаратору
	exceptionmas(mas);//отсеивание повторяющихся точек
}

bool comp(basp a, basp b)//функция компоратора
{
	if (a.y < b.y)return true;//cравнение по второй координате
	else if (a.y > b.y) return false;
	else return a.x < b.x;//если вторые координаты равны, сравнение по первой координате
}

void exceptionmas(basp *mas)//отсеивание из массива повторяющихся элементов
{
	for (int i = 1; i < N; i++)
	{
		if (mas[i - 1].x == mas[i].x && mas[i - 1].y == mas[i].y) { excep(i, mas); N--; i--; }//удаление из массива точек повторяющихся элементов
	}
}

void excep(int i, basp *a)//удаление из массива элемента i
{
	for (int j = i; j < N - 1; j++) a[j] = a[j + 1];
}

//сообщение об ошибке
void writeAerror()
{
	ofstream fout("output.txt");
	fout << "НЕСУЩЕСТВУЮЩИЙ НОМЕР ГРАНИЧНОЙ ФУНКЦИИ ИЛИ ОБЛАСТИ" << endl;
	exit(0);
	fout.close();
}


void display() {}//сопоставление первым двум числам из файла области и граничной функци
	curve c1(0, 2 * 3.1415, u1, v1);
	curve c2(0, 6, u2, v2);
	curve c3(0, 16, u3, v3);

	switch (circle) {
	case 1:
		mycurve = c1;
		break;
	case 2:
		mycurve = c2;
		break;
	case 3:
		mycurve = c3;
		break;
	default:
		writeAerror();
	}

	if (gf > kgf)writeAerror();//если выбранный номер больше числа граничных функций на кривую
	else fi = gfunctions[circle - 1][gf - 1];//граничная функция - функция с номером gf для кривой с номером circle
}

//вывод точности
void writeerror(double *x, double eps) {
	ofstream fout("output.txt");

	fout << "Вектор решения:" << endl;
	for (int i = 0; i < N; i++) fout << x[i] << endl;
	fout << "Разница приближённого решения с граничной функцией при числе базисных функций " << N << " равна " << eps << endl;
	fout.close();
}

//присвоение вектору Ax произведения a*x матрицы на вектор
void matrixpower(double *Ax, double **a, double *x) {
	for (int i = 0; i < N; i++) {
		double sum = 0;
		for (int j = 0; j < N; j++) sum += a[i][j] * x[j];
		Ax[i] = sum;
	}
}

//нахождение и вывод точности по известному решению
void error() {

	double p = mycurve.firstkind(N, N), sum = 0;

	double *Ax = new double[N];
	matrixpower(Ax, mysystem.a, mysystem.x);

	for (int i = 0; i < N; i++) sum += mysystem.x[i] * Ax[i];//mycurve.firstkind(i,N);
	double eps = abs(p - sum);
	writeerror(mysystem.x, eps);
}

void search()
{
	mysystem.make(N);//создать систему порядка, равного порядку базисных точек

			//заполнить систему
	for (int i = 0; i < N; i++) {
		mysystem.b[i] = mycurve.firstkind(i, N);
		mysystem.a[i][i] = 2;//mycurve.firstkind(i,i);
		for (int j = i + 1; j < N; j++) {
			double tmp = i % 5 + j % 8;//mycurve.firstkind(i,j);
			mysystem.a[i][j] = tmp;
			mysystem.a[j][i] = tmp;
		}
	}



	cout << "матрица системы и свободный вектор: " << endl;
	for (int i = 0; i < N; i++)
	{
		cout << endl;
		for (int j = 0; j < N; j++)cout << mysystem.a[i][j] << " ";
	}
	cout << endl; cout << endl;
	for (int i = 0; i < N; i++) cout << mysystem.b[i] << endl; cout << endl;


	//решить систему
	//mysystem.Holets();
mysystem.Gauss();

	/*	double *Ax = new double[N];
		matrixpower(Ax, mysystem.a, mysystem.x);
		double s = 0;
		for (int i = 0; i < N; i++) s += sqrt((Ax[i] - mysystem.b[i])*(Ax[i] - mysystem.b[i]));
		cout << endl;
		cout << "невязка = " << s << endl;cout << "произведение матрицы системы на найденное решение " << endl;
		for (int i = 0; i < N; i++) cout << Ax[i] << endl; cout << endl;*/




		//вывести решение, найти и вывести погрешность		
	error();
}

