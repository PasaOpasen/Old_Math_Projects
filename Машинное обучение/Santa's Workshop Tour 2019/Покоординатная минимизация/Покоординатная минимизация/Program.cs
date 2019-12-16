using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Покоординатная_минимизация
{
   static class Program
    {

        static int[] family_id = Enumerable.Range(0, 5000).ToArray();
        static byte[]
             choice_0 = new byte[5000],
             choice_1 = new byte[5000],
             choice_2 = new byte[5000],
             choice_3 = new byte[5000],
             choice_4 = new byte[5000],
             choice_5 = new byte[5000],
             choice_6 = new byte[5000],
             choice_7 = new byte[5000],
             choice_8 = new byte[5000],
             choice_9 = new byte[5000],
             n_people = new byte[5000],
             res = new byte[5000];
        static double best;
        static (int, int)[] Fs = new (int, int)[5000];

        /// <summary>
        /// Всевозможные комбинации пар от 1 до 100
        /// </summary>
        static (byte, byte)[] combinations2;
        static Program()
        {
            combinations2 = new (byte, byte)[10000];
            for (int n1 = 0; n1 < 100; n1++)
                for (int n2 = 0; n2 < 100; n2++)
                    combinations2[n1 * 100 + n2] = ((byte)(n1+1), (byte)(n2+1));
        }


        static void ReadData()
        {
            short[] tmp;
            using (var s = new StreamReader("family_data.csv"))
            {
                s.ReadLine();
                for (int i = 0; i < 5000; i++)
                {
                    tmp = s.ReadLine().Split(',').Select(n => Convert.ToInt16(n)).ToArray();
                    choice_0[i] = Convert.ToByte(tmp[1]);
                    choice_1[i] = Convert.ToByte(tmp[2]);
                    choice_2[i] = Convert.ToByte(tmp[3]);
                    choice_3[i] = Convert.ToByte(tmp[4]);
                    choice_4[i] = Convert.ToByte(tmp[5]);
                    choice_5[i] = Convert.ToByte(tmp[6]);
                    choice_6[i] = Convert.ToByte(tmp[7]);
                    choice_7[i] = Convert.ToByte(tmp[8]);
                    choice_8[i] = Convert.ToByte(tmp[9]);
                    choice_9[i] = Convert.ToByte(tmp[10]);
                    n_people[i] = Convert.ToByte(tmp[11]);
                }
            }
        }

        static Func<byte[], bool> wall = (byte[] arr) =>
          {
                //******************
                short[] count = new short[100];
                for (int i = 0; i < arr.Length; i++)
                    count[arr[i] - 1] += n_people[i];

                for (int i = 0; i < count.Length; i++)
                    if (count[i] < 125 || count[i] > 300)
                        return true;
              //**************
              return false;
          };

        static Func<byte[], double> preference_costs = (byte[] arr) =>
            {
                byte[] sum = new byte[5000];
                int s = 0;
                for (int i = 0; i < arr.Length; i++)
                {
                    bool sm = false;

                    if (arr[i] == choice_0[i])
                    {
                        sm = true;
                    }
                    if (arr[i] == choice_1[i])
                    {
                        sm = true;
                        s += 50;
                    }
                    if (arr[i] == choice_2[i])
                    {
                        sm = true;
                        s += 50 + 9 * n_people[i];
                    }
                    if (arr[i] == choice_3[i])
                    {
                        sm = true;
                        s += 100 + 9 * n_people[i];
                    }
                    if (arr[i] == choice_4[i])
                    {
                        sm = true;
                        s += 200 + 9 * n_people[i];
                    }
                    if (arr[i] == choice_5[i])
                    {
                        sm = true;
                        s += 200 + 18 * n_people[i];
                    }
                    if (arr[i] == choice_6[i])
                    {
                        sm = true;
                        s += 300 + 18 * n_people[i];
                    }
                    if (arr[i] == choice_7[i])
                    {
                        sm = true;
                        s += 300 + 36 * n_people[i];
                    }
                    if (arr[i] == choice_8[i])
                    {
                        sm = true;
                        s += 400 + 36 * n_people[i];
                    }
                    if (arr[i] == choice_9[i])
                    {
                        sm = true;
                        s+= 500 + (36 + 199) * n_people[i];
                    }
                    if (!sm)
                        s += 500 + (36 + 398) * n_people[i];
                }           

                return s;
            };
        static Func<byte[], double> accounting_penalty = (byte[] arr) =>
           {
               short[] count = new short[100];
               for (int i = 0; i < arr.Length; i++)
                   count[arr[i] - 1] += n_people[i];

               for (int i = 0; i < count.Length; i++)
                   if (count[i] < 125 || count[i] > 300)
                       return 1e20;

               double sum = 0;
               for (int i = 98; i >= 0; i--)
                   sum += (count[i] - 125) / 400.0 * Math.Pow(count[i], 0.5 + 0.02 * Math.Abs(count[i] - count[i + 1]));
               return sum + (count[99] - 125) / 400.0 * Math.Sqrt(count[99]);
           };
        static Func<byte[], double> preference_costs2 = (byte[] arr) =>
        {
            if (wall(arr))
                return 1e20;
            return preference_costs(arr);
        };
        static Func<byte[], double> accounting_penalty2 = (byte[] arr) =>
        {
            if (wall(arr))
                return 1e20;
            return accounting_penalty(arr);
        };

        /// <summary>
        /// Функция, делающая то же самое, что и preference_costs2, но возвращающая ещё массив индексов, упорядоченных по убыванию вклада элементов входного массива
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        static double PreferenceCosts(byte[] arr,ref int[] order)
        {
            //if (wall(arr))
            //    return 1e20;

            int[] sums = new int[5000];

            for (int i = 0; i < arr.Length; i++)
            {
                bool sm=false;

                if (arr[i] == choice_0[i])
                {
                    sm=true;
                }
                if (arr[i] == choice_1[i])
                {
                    sm = true;
                    sums[i] += 50;
                }
                if (arr[i] == choice_2[i])
                {
                    sm = true;
                    sums[i] += 50 + 9 * n_people[i];
                }
                if (arr[i] == choice_3[i])
                {
                    sm = true;
                    sums[i] += 100 + 9 * n_people[i];
                }
                if (arr[i] == choice_4[i])
                {
                    sm = true;
                    sums[i] += 200 + 9 * n_people[i];
                }
                if (arr[i] == choice_5[i])
                {
                    sm = true;
                    sums[i] += 200 + 18 * n_people[i];
                }
                if (arr[i] == choice_6[i])
                {
                    sm = true;
                    sums[i] += 300 + 18 * n_people[i];
                }
                if (arr[i] == choice_7[i])
                {
                    sm = true;
                    sums[i] += 300 + 36 * n_people[i];
                }
                if (arr[i] == choice_8[i])
                {
                    sm = true;
                    sums[i] += 400 + 36 * n_people[i];
                }
                if (arr[i] == choice_9[i])
                {
                    sm = true;
                    sums[i] += 500 + (36 + 199) * n_people[i];
                }
                if (!sm)
                    sums[i] += 500 + (36 + 398) * n_people[i];

                Fs[i] = (i, sums[i]);
            }

            order = Fs.Where(t=>t.Item2>0).OrderByDescending(t => t.Item2).Select(t => t.Item1).ToArray();

            return sums.Sum();
        }
        static void SuperMinimizingPreferenceCosts(int maxdeep=-1)
        {
            int md = (maxdeep < 1) ? 5000 : maxdeep;

            int[] order=new int[1];
            bool existresult;
            double bst;
            byte[][] mat = GetNresCopy();
            double[] results = new double[100];
            int iter;

            do
            {
                existresult = false;
                best = PreferenceCosts(res, ref order);
                iter = 0;

                order = order.Take(Math.Min(order.Length, md)).ToArray();

                foreach(int nb in order)
                {
                    iter++;
                    for (byte i = 0; i < 100; i++)
                        mat[i][nb] = (byte)(i + 1);
                    results = mat.AsParallel().Select(arr => preference_costs2(arr)).ToArray();
                    bst = results.Min();
                    int n = 0;
                    for (int i = 0; i < results.Length; i++)
                        if (results[i] == bst)
                        {
                            n = i + 1;
                            break;
                        }

                    byte bn = (byte)n;
                    for (byte i = 0; i < 100; i++)
                        mat[i][nb] = bn;

                    if (bst < best)
                    {
                        res = mat[0];
                        best = bst;
                        existresult = true;
                        Console.WriteLine($"best score = {Math.Round(best, 3)}; index = {nb}; iteration = {iter}");

                        break;
                    }
                }


            } while (existresult);

            Console.WriteLine("Записывается в файл");
            WriteData(best, "prfmod");
        }



        static Func<byte[], double> score = (byte[] arr) => {
            //if (wall(arr))
            //    return 1e20;
           return preference_costs(arr) + accounting_penalty(arr);
        };

        static void ReadRES()
        {
            using (var s = new StreamReader("res.csv"))
            {
                s.ReadLine();
                for (int i = 0; i < 5000; i++)
                    res[i] = Convert.ToByte(s.ReadLine().Split(',')[1]);
            }
            best = score(res);
        }
        static void WriteData(double reslt,string acc="")
        {
            using (var s = new StreamWriter($"res {acc} {reslt}.csv"))
            {
                s.WriteLine("family_id,assigned_day");
                for (int i = 0; i < res.Length; i++)
                    s.WriteLine($"{i},{res[i]}");
            }
        }

        static int[] GetRange()=> Enumerable.Range(0, 5000).ToArray();
        static int[] GetRandom()
        {
            var s = GetRange();
            Random r = new Random();
            int tmp1,tmp2;
            int tmp;
            for (int i=0;i<5000;i++)
            {
                tmp1 = r.Next(4999);
                tmp2= r.Next(4999);
                tmp = s[tmp1];
                s[tmp1] = s[tmp2];
                s[tmp2] = tmp;
            }
            return s;
        }

        /// <summary>
        /// Копирует массив res указанное число раз
        /// </summary>
        /// <param name="samplecount"></param>
        /// <returns></returns>
        static byte[][] GetNresCopy(int samplecount=100)
        {
            byte[][] mat = new byte[samplecount][];
            for (int i = 0; i < samplecount; i++)
            {
                ref var m = ref mat[i];
                m = new byte[5000];
                for (int j = 0; j < m.Length; j++)
                    m[j] = res[j];
            }
            return mat;
        }
        static bool MakeCoordMin(Func<byte[], double> fun)
        {
            best = fun(res);

            bool existprogress = false;

            byte[][] mat = GetNresCopy();

            var numbers = GetRandom(); //GetRange();
            double[] results = new double[100];
            foreach(var nb in numbers)
            {
                for (byte i = 0; i < 100; i++)
                    mat[i][nb] = (byte)(i + 1);
                results = mat.AsParallel().Select(arr => fun(arr)).ToArray();
                double bst=results.Min();
                int n = 0;
                for(int i=0;i<results.Length;i++)
                    if(results[i]==bst)
                    {
                        n = i + 1;
                        break;
                    }

                byte bn = (byte)n;
                for (byte i = 0; i < 100; i++)
                    mat[i][nb] = bn;

                if (best > bst)
                {
                    best = bst;
                    existprogress = true;
                    res = mat[0];
                    Console.WriteLine($"best score = {Math.Round(best,3)}; iter = {nb}");
                }
                                          
            }

            return existprogress;
        }

        static bool MakeCoordMin2(Func<byte[], double> fun)
        {
            best = fun(res);

            bool existprogress = false;

            const int howmany= 100 * 100;

            byte[][] mat = GetNresCopy(howmany);

            var numbers = GetRandom(); //GetRange();
            Random r = new Random();

            double[] results = new double[howmany];
            int ind2;
            (byte, byte) vals;

            foreach (var nb in numbers)
            {
                    ind2 = r.Next(0, 4999);
                    ind2 = (ind2 == nb) ? nb / 2 : ind2;

                for (int i = 0; i < howmany; i++)
                {
                    vals = combinations2[i];

                    mat[i][nb] = vals.Item1;
                    mat[i][ind2] = vals.Item2;
                }
                    
                results = mat.AsParallel().Select(arr => fun(arr)).ToArray();
                double bst = results.Min();
                int n = 0;
                for (int i = 0; i < results.Length; i++)
                    if (results[i] == bst)
                    {
                        n = i + 1;
                        break;
                    }

                byte s1 = mat[n][nb], s2 = mat[n][ind2];
                for (int i = 0; i < howmany; i++)
                {
                       mat[i][nb] = s1;
                       mat[i][ind2] = s2;
                }
                    

                if (best > bst)
                {
                    best = bst;
                    existprogress = true;
                    res = mat[0];
                    Console.WriteLine($"best score = {Math.Round(best, 3)}; iter = {nb}");
                }
                else
                {
                    Console.WriteLine($"FAILURE on {nb}-{ind2}");
                }

            }

            return existprogress;
        }

        /// <summary>
        /// Минимицация по случайной выборке
        /// </summary>
        /// <param name="fun">Минимизируемая функция</param>
        /// <param name="samplecount">Число образцов, размерность множетсва примеров, из которого будет искаться лучший</param>
        /// <param name="rows">В скольки строках за раз меняются значения</param>
        /// <param name="range">Максимальный по модулю сдвиг, то есть к координатам вектора будут прибавляться целые числа от -range до range (включая 0, но он маловероятен)</param>
        /// <returns></returns>
        static bool MakeSampleBest(Func<byte[], double> fun,int samplecount=100,int rows = 10,int range=5)
        {
            bool exist = false;

            var mat = GetNresCopy(samplecount);

            var numbers = GetRandom();
            double[] results = new double[samplecount];
            Random r = new Random();
            for(int tt=0;tt<numbers.Length;tt+=rows)
            {     
                for (byte i = 0; i < samplecount; i++)
                {
                    ref var row = ref mat[i];
                    int add;
                    for (int h = 0; h < rows; h++)
                    {
                        add=row[numbers[tt + h]]+(Math.Sign(r.NextDouble() - 0.5) * r.Next(1, range));
                        if (add < 1) add = 1;
                        else if (add > 100) add = 100;
                        row[numbers[tt + h]] = (byte)add;
                    }
                        
                }
                    
                results = mat.AsParallel().Select(arr => fun(arr)).ToArray();
                double bst = results.Min();
                int n = 0;
                for (int i = 0; i < results.Length; i++)
                    if (results[i] == bst)
                    {
                        n = i + 1;
                        break;
                    }

                if (best > bst)
                {
                    best = bst;
                    exist = true;
                    res = mat[n];
                    Console.WriteLine($"best score = {best}; iter = {(tt+1)/rows}");
                }

                for (byte i = 0; i < samplecount; i++)
                {
                    ref var row = ref mat[i];
                    for (int h = 0; h < rows; h++)
                        row[numbers[tt + h]] = res[numbers[tt + h]];
                }
            }

            return exist;
        }

        static void MakeResult2( Func<byte[],double> fun,string acc="")
        {
            while (MakeCoordMin(fun))
            {
                Console.WriteLine("Записывается в файл");
                WriteData(best,acc);
            }
            return;
        }
        static void MakeResult3(Func<byte[], double> fun, string acc = "")
        {
            while (MakeSampleBest(fun,200,5,1))
            {
                Console.WriteLine("Записывается в файл");
                WriteData(best, acc);
            }
            return;
        }
        static void MakeResult4(string acc = "")
        {
            while (MakeCoordMin(preference_costs2)||MakeCoordMin(accounting_penalty))
            {
                Console.WriteLine("Записывается в файл");
                WriteData(best, acc);
                MakeResult2(score);
            }
            return;
        }


        const int countbegins = 1000;
        static byte[][] begins = new byte[countbegins][];
        static void ReadBegins()
        {
            using(var s=new StreamReader("begins.csv"))
            {
                for(int i=0;i< countbegins; i++)
                {
                    ref var bg = ref begins[i];
                    bg = new byte[5000];
                    for (int j = 0; j < bg.Length; j++)
                        bg[j] = Convert.ToByte(s.ReadLine());
                }
            }
        }

        static void BatchMethod(Func<byte[],double> fun,int size=20,int count = 400)
        {
            best = fun(res);
            byte[][] mat = GetNresCopy();

            var numbers = Enumerable.Range(0,count);
            double[] results = new double[100];

            var rand = new Random();

            foreach (var nb in numbers)
            {
                for (byte i = 0; i < 100; i++)
                    for(byte s=0;s<size;s++)
                    mat[i][rand.Next(0,4999)] = (byte)rand.Next(1,100);
                results = mat.AsParallel().Select(arr => fun(arr)).ToArray();
                double bst = results.Min();
                if (bst >= 1e20)
                    continue;
                int n = 0;
                for (int i = 0; i < results.Length; i++)
                    if (results[i] == bst)
                    {
                        n = i + 1;
                        break;
                    }

                byte bn = (byte)n;
                for (byte i = 0; i < 100; i++)
                    for(int s=0;s<5000;s++)
                    mat[i][s] = mat[n][s];

                if (best > bst)
                {
                    best = bst;
                    res = mat[0];
                    Console.WriteLine($"best score = {Math.Round(best, 3)}; iter = {nb}");
                }

            }
        }

        static void Batch2(Func<byte[], double> fun, int size = 20, int count = 40)
        {
            byte[] res2 = new byte[5000];
            for (int i = 0; i < res.Length; i++)
                res2[i] = res[i];
            var r = new Random();

            for(int tmp = 0; tmp < count; tmp++)
            {
                for (byte s = 0; s < size; s++)
                    res[r.Next(0, 4999)] = (byte)r.Next(1, 100);
                MakeResult2(fun, "batch");
                if(fun(res)<fun(res2))
                {
                    for (int i = 0; i < res.Length; i++)
                        res2[i] = res[i];
                }
                else
                {
                    for (int i = 0; i < res.Length; i++)
                        res[i] = res2[i];
                }
                Console.WriteLine($"BATCH {tmp + 1} complited");
            }


        }

        static void Main(string[] args)
        {
            ReadData();
            //ReadRES();

           // MakeResult2();

            // var t = preference_costs(res);
            // var s = accounting_penalty(res);
            ReadBegins();

             ReadRES();
            for(int y = 0; y < countbegins; y++)
            {
                
               // res = begins[y];

                Console.WriteLine($"----------------ITERATION {y+1}. Begin score = {best}");

                //best = accounting_penalty(res);
                //Console.WriteLine("Пробуем минимизировать вторую функцию");
                //MakeCoordMin(accounting_penalty);
                //Console.WriteLine("Записывается в файл");
                //WriteData(best, "acc");

                //MakeResult4("all");

                //best = preference_costs2(res);
                //MakeResult2(preference_costs2, "prf");

                //best = score(res);
                //MakeCoordMin2(score);
                //WriteData(best, "lev2");

               // double beg = score(res);

                SuperMinimizingPreferenceCosts(100);

                best = score(res);        
                MakeResult2(score);
                best = score(res);

                // BatchMethod(score, 2, 400);

                //Batch2(score, 10, 10);

                //if (beg <= best)
                //{
                //    Console.WriteLine("Пробуем минимизировать вторую функцию");
                //MakeCoordMin(accounting_penalty);
                //Console.WriteLine("Записывается в файл");
                //WriteData(best, "acc");
                //}

                // MakeResult3(score);

                //best = accounting_penalty(res);
                //MakeResult2(accounting_penalty, "acc");

                //best = score(res);
                //MakeResult2(score);
            }
            
           
        }
    }
}
