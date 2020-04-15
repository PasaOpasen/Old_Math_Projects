
#include <fstream>
using namespace std;

void main()
{
   setlocale(LC_ALL, "rus");

	int x;

	ifstream fin("ввод.txt");
	fin>>x;

	ofstream fout("вывод.txt"); 

	if(x%12==0){fout << x<<"/12="<<x/12<<"  ƒмитрий ѕасько"<<endl;} 
	else fout <<" 8965193646 "<<endl;

		fout.close(); 


}
