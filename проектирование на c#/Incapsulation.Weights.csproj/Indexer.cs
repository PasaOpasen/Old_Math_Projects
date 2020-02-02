using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.Weights
{
    public class Indexer
    {
        double[] w;
        int start, len;

        public int Length => len;

        private void exep(int i)
        {
            if (start + i > len ||i<0)
                throw new IndexOutOfRangeException();
        }

        public double this[int i]
        {
            get
            {
                exep(i);
                return w[start + i];
            }
            set
            {
                exep(i);
                w[start + i] = value;
            }
        }

        public Indexer(double[] ar, int start = 0, int length = 1)
        {
            if (start < 0 || length < 0 || start + length > ar.Length)
                throw new ArgumentException();
            w = ar;
            this.start = start;
            len = length;

        }
    }
}
