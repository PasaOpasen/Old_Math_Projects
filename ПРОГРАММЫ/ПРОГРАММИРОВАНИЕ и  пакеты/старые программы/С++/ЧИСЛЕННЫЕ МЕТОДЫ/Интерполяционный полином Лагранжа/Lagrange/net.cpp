#include <stdio.h>
#include <iostream.h>
#include <stdlib.h>

class Net
 {unsigned n;
  double * pt; // pointer to x0

public:
// constructors
 Net()
   {n = 0;
    pt = NULL;   
   }  

 Net(unsigned q, double * a)
   {n = q;
    pt = (double *)malloc((n+1) * sizeof(double));
    memcpy(pt, a,(n+1) * sizeof(double));   
   }

   
//destructor
 ~Net()
  {free(pt);
   pt = NULL;
   n = 0;   
  }

//info
 unsigned Size()
   {return n;
   }
 double * NodPtr()
   {return pt;
   }
   
 double operator[](unsigned i)
   {return  *(pt + i);
   }            
 }; // end of class Net
