#include "net.cpp"
#include "Lagrange.cpp"
#include <conio.h>
#include <stdlib.h>

int main()
  {double ar[5] = {1, 2, 3, 4, 5};
   Net nn(4, ar);
   
   Polynom mumu;

   for(unsigned int i = 0; i <= nn.n; i++)
   mumu=CreatePhi(i,nn);
   
   for(unsigned int i = 0; i <= 4; i++)
     printf("%10.3lf", nn[i]);
   printf("\n");   
   getch(); 
  }

