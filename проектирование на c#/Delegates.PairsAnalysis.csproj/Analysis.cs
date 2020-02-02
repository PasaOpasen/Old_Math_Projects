using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.PairsAnalysis
{
    public static class Analysis
    {
        public static int FindMaxPeriodIndex(params DateTime[] data)
        {
             //return new MaxPauseFinder().Analyze(data);
            if (data == null || data.Length < 2)
                throw new ArgumentException();
            return data.Pairs().Select(i => Math.Abs((i.Item1 - i.Item2).Days)).MaxIndex();
        }

        public static double FindAverageRelativeDifference(params double[] data)
        {
            if (data == null || data.Length < 2)
                throw new ArgumentException();
           // return new AverageDifferenceFinder().Analyze(data);
            return data.Pairs().Select(i => (i.Item2 -i.Item1 )/i.Item1).Sum() / (data.Length-1);
        }

        public static IEnumerable<Tuple<T, T>> Pairs<T>(this IEnumerable<T> dates)
        {
            T last;
            var p = dates.GetEnumerator();
            p.MoveNext();
            last = p.Current;
            while (p.MoveNext())
            {
                yield return new Tuple<T, T>(last, p.Current);
                last = p.Current;
            }
        }
        public static int MaxIndex<T>(this IEnumerable<T> rawData) where T : IComparable
        { 
            if (rawData == null)
            throw new ArgumentException();
            var p = rawData.GetEnumerator();
            
            if(!p.MoveNext())
                throw new ArgumentException();

            int maxIndex = 0;
            T max = default(T);
            int i = 1;        
               
                while (p.MoveNext())
                {
                    if (max.CompareTo(p.Current) == -1)
                    {
                        max = p.Current;
                        maxIndex = i;
                    }
                    i++;
                }
           return maxIndex;
        }
    }
}
