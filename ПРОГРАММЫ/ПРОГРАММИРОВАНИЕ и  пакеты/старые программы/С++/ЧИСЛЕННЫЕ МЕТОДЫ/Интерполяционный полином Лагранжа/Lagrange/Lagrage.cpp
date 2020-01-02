#include "net.cpp"
#include "polynom.cpp"

Polynom CreatePhi(unsigned i, Net nn)
  {Polynom mult;
   double a[2];
   double xi;
   double * pxj = nn.NodPtr();    
   Polynom prod(1.);

   xi = nn[i];
   for (unsigned j = 0; j <= nn.Size(); j++, pxj++)
    {if(j == i) continue;
     a[0] = -nn[j]/(xi - *pxj);
     a[1] = 1/(xi - *pxj);
     mult = Polynom(1, a);
     prod = prod * mult;   
    }    
   return prod;
  }

