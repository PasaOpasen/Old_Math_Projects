#define _CRT_SECURE_NO_WARNINGS
#include <stdlib.h>
#include <stdio.h>
#include <iostream>
#include <fstream>
#include <conio.h>

using namespace std;

     
class Polynom
{ unsigned int deg;
  double * coef;

static
  Polynom Reminder;   

Polynom MultMonom(double a, unsigned d);

public:
// Constructors
 Polynom()
  {deg = 0;
   coef = NULL;
  }

 Polynom(unsigned n)
   {deg = n;
    coef = (double *)malloc((n+1) * sizeof(double));
   }

 Polynom(double a)
   {deg = 0;
    coef = (double *)malloc(sizeof(double));
    *coef = a;
   }

 Polynom(unsigned n, double * p)
   {deg = n;
    coef = (double *)malloc((n+1) * sizeof(double));
    
    memcpy(coef, p, (n+1) * sizeof(double));
  }

 Polynom(const Polynom& A)
   {deg = A.deg;
    coef = (double *)malloc((deg+1) * sizeof(double));
    memcpy(coef, A.coef, (deg+1) * sizeof(double));
   }

 Polynom(FILE * fp)
   {fscanf(fp, "%d", &deg);
    coef = (double *)malloc((deg+1) * sizeof(double));
    double *p = coef;
    for (unsigned i = 0; i <= deg; i++, p++)
      fscanf(fp, "%lf", p);
   }
        
 // Destructor
 ~Polynom()
   {free(coef);
    coef = NULL;
    deg = 0;
   }

// Info functions
 double * CoefPtr()
   {return coef;
   }

 unsigned Degree()
   {return deg;
   }    

 double GetCoef(unsigned i)
   {return *(coef + i);
   }
 
// SetValue functions
 void SetCoef(unsigned i, double a)
   {double * p = coef;
    p += i;
    *p = a;
   }

//GetValue function
 double Value(double x);

 void operator=(const Polynom& M)
  {if(coef != NULL)
     free(coef); 
   deg = M.deg;
   coef = (double *)malloc((deg+1) * sizeof(double));
   memcpy(coef, M.coef, (deg+1) * sizeof(double));
   }

// Debug operations
 void Display();

        
// !!!!!!!!!!!!!! Algebraic operations
 Polynom operator+(const Polynom& B);
 void operator+=(const Polynom& B);
 Polynom operator-(const Polynom& B);
 void operator-=(const Polynom& B);

 void operator+=(double a)
   {*coef += a;
   }

 Polynom operator*(Polynom& Q);  // Z = P * Q

 Polynom Substitute(Polynom& Q); // P(Q(x))

//Analitic functions
 Polynom Diff();
 
}; // end of  class Polynom
 

