using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MathNet.Numerics.Random;

namespace Покоординатная_минимизация
{
    static class Program
    {
        static CryptoRandomSource randomgen = new CryptoRandomSource();

        static int[] family_id = Enumerable.Range(0, 5000).ToArray();
        static readonly byte[]
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
             n_people = new byte[5000];

        static byte[] res = new byte[5000];

        static readonly int[]
            ifchoice2 = new int[5000],
            ifchoice3 = new int[5000],
            ifchoice4 = new int[5000],
            ifchoice5 = new int[5000],
            ifchoice6 = new int[5000],
            ifchoice7 = new int[5000],
            ifchoice8 = new int[5000],
            ifchoice9 = new int[5000],
            ifnochoice = new int[5000];

        static double best;
        static (int, int)[] Fs = new (int, int)[5000];

        /// <summary>
        /// Всевозможные комбинации пар от 1 до 100
        /// </summary>
        static (byte, byte)[] combinations2;
        static Program()
        {
            ReadData();

            combinations2 = new (byte, byte)[10000];
            for (int n1 = 0; n1 < 100; n1++)
                for (int n2 = 0; n2 < 100; n2++)
                    combinations2[n1 * 100 + n2] = ((byte)(n1 + 1), (byte)(n2 + 1));
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

                    ifchoice2[i] = 50 + 9 * n_people[i];
                    ifchoice3[i] = 100 + 9 * n_people[i];
                    ifchoice4[i] = 200 + 9 * n_people[i];
                    ifchoice5[i] = 200 + 18 * n_people[i];
                    ifchoice6[i] = 300 + 18 * n_people[i];
                    ifchoice7[i] = 300 + 36 * n_people[i];
                    ifchoice8[i] = 400 + 36 * n_people[i];
                    ifchoice9[i] = 500 + (36 + 199) * n_people[i];
                    ifnochoice[i] = 500 + (36 + 398) * n_people[i];
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
                int s = 0;
                byte arri;
                for (int i = 0; i < arr.Length; i++)
                {
                    arri = arr[i];

                    if (arri == choice_0[i])
                    {
                    }
                    else if (arri == choice_1[i])
                    {
                        s += 50;
                    }
                    else if (arri == choice_2[i])
                    {
                        s += ifchoice2[i];
                    }
                    else if (arri == choice_3[i])
                    {
                        s += ifchoice3[i];
                    }
                    else if (arri == choice_4[i])
                    {
                        s += ifchoice4[i];
                    }
                    else if (arri == choice_5[i])
                    {
                        s += ifchoice5[i];
                    }
                    else if (arri == choice_6[i])
                    {
                        s += ifchoice6[i];
                    }
                    else if (arri == choice_7[i])
                    {
                        s += ifchoice7[i];
                    }
                    else if (arri == choice_8[i])
                    {
                        s += ifchoice8[i];
                    }
                    else if (arri == choice_9[i])
                    {
                        s += ifchoice9[i];
                    }
                    else
                        s += ifnochoice[i];
                }

                return s;
            };
        static Func<byte[], double> accounting_penalty = (byte[] arr) =>
           {
               short[] count = new short[100];
               for (int i = 0; i < arr.Length; i++)
                   count[arr[i] - 1] += n_people[i];

               short tmp;
               for (int i = 0; i < count.Length; i++)
               {
                   tmp = count[i];
                   if (tmp < 125 ||tmp > 300)
                       return 1e20;
               }

               double sum = 0;
               for (int i = 98; i >= 0; i--)
               {
                   tmp = count[i];
                   sum += (tmp - 125.0) / 400.0 * Math.Pow(tmp, 0.5 + 0.02 * Math.Abs(tmp - count[i + 1]));
               }
                   
               return sum + (count[99] - 125.0) / 400.0 * Math.Sqrt(count[99]);
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
        static double PreferenceCosts(byte[] arr, ref int[] order)
        {
            //if (wall(arr))
            //    return 1e20;

            int[] sums = new int[5000];

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

            order = Fs.Where(t => t.Item2 > 0).OrderByDescending(t => t.Item2).Select(t => t.Item1).ToArray();

            return sums.Sum();
        }
        static void SuperMinimizingPreferenceCosts(int maxdeep = -1)
        {
            int md = (maxdeep < 1) ? 5000 : maxdeep;
            int k = 0;

            int[] order = new int[1];
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

                //order = order.Take(Math.Min(order.Length, md)).ToArray();

                foreach (int nb in order)
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
                        k++;
                        if (k == md)
                        {
                            goto end;
                        }
                        break;
                    }
                }


            } while (existresult);

        end:
            Console.WriteLine("Записывается в файл");
            WriteData(best, "prfmod");
        }

        /// <summary>
        /// Функция счёта. Она так написана, чтоб быстрее работать в случаях, когда аргумент выходит за границы
        /// </summary>
        static Func<byte[], double> score = (byte[] arr) =>
        {
            double acc = accounting_penalty(arr);
            if (acc >= 1e20) return acc;
            return acc + preference_costs(arr);
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
        static void WriteData(double reslt, string acc = "")
        {
            using (var s = new StreamWriter($"res {acc} {reslt}.csv"))
            {
                s.WriteLine("family_id,assigned_day");
                for (int i = 0; i < res.Length; i++)
                    s.WriteLine($"{i},{res[i]}");
            }
        }

        static int[] GetRange() => Enumerable.Range(0, 5000).ToArray();
        static int[] GetRandom()
        {
            var s = GetRange();
            int tmp1, tmp2;
            int tmp;
            for (int i = 0; i < 5000; i++)
            {
                tmp1 = randomgen.Next(4999);
                tmp2 = randomgen.Next(4999);
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
        static byte[][] GetNresCopy(int samplecount = 100)
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
        static bool MakeCoordMin(Func<byte[], double> fun,int count=5000)
        {
            best = fun(res);

            bool existprogress = false;

            byte[][] mat = GetNresCopy();

            var numbers = GetRandom().Take(count).ToArray(); //GetRange();
            double[] results = new double[100];

            foreach (var nb in numbers)
            {
                for (byte i = 0; i < 100; i++)
                    mat[i][nb] = (byte)(i + 1);

                results = mat.AsParallel().Select(arr => fun(arr)).ToArray();
                double bst = results.Min();
                int n = Array.IndexOf(results,bst)+1;

                byte bn = (byte)n;
                for (byte i = 0; i < 100; i++)
                    mat[i][nb] = bn;

                if (best > bst)
                {
                    best = bst;
                    existprogress = true;
                    res = mat[0];
                    Console.WriteLine($"best score = {Math.Round(best, 3)}; iter = {nb}");
                }

            }

            return existprogress;
        }

        static bool MakeCoordMin2(Func<byte[], double> fun)
        {
            best = fun(res);

            bool existprogress = false;

            const int howmany = 100 * 100;

            byte[][] mat = GetNresCopy(howmany);

            var numbers = GetRandom(); //GetRange();

            double[] results = new double[howmany];
            int ind2;
            (byte, byte) vals;

            foreach (var nb in numbers)
            {
                ind2 = randomgen.Next(0, 4999);
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

        static bool MakeCoordMinSlow(Func<byte[], double> fun)
        {
            best = fun(res);

            bool existprogress = false;

            byte[][] mat = GetNresCopy();

            var numbers = GetRange();
            var result = new (double, byte,int)[5000];
            int index = 0;
            double bst;

            double[] results = new double[100];
            foreach (var nb in numbers)
            {
                for (byte i = 0; i < 100; i++)
                    mat[i][nb] = (byte)(i + 1);
                results = mat.AsParallel().Select(arr => fun(arr)).ToArray();
                bst = results.Min();
                int n = 0;
                for (int i = 0; i < results.Length; i++)
                    if (results[i] == bst)
                    {
                        n = i + 1;
                        break;
                    }

                result[index++] = (bst, mat[n-1][nb],nb);

                byte bn = (byte)n;
                for (byte i = 0; i < 100; i++)
                    mat[i][nb] = res[nb];
            }

            bst = result.Select(t => t.Item1).Min();

                if (best > bst)
                {
                var p = result.First(t => t.Item1 == bst);
                   Console.WriteLine($"best score = {Math.Round(bst, 3)} (from {Math.Round(best, 3)}); iter = {p.Item3}");
                    best = bst;
                existprogress = true;               
                    res[p.Item3] = p.Item2;                   
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
        static bool MakeSampleBest(Func<byte[], double> fun, int samplecount = 100, int rows = 10, int range = 5)
        {
            bool exist = false;

            var mat = GetNresCopy(samplecount);

            var numbers = GetRandom();
            double[] results = new double[samplecount];
            for (int tt = 0; tt < numbers.Length; tt += rows)
            {
                for (byte i = 0; i < samplecount; i++)
                {
                    ref var row = ref mat[i];
                    int add;
                    for (int h = 0; h < rows; h++)
                    {
                        add = row[numbers[tt + h]] + (Math.Sign(randomgen.NextDouble() - 0.5) * randomgen.Next(1, range));
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
                    Console.WriteLine($"best score = {best}; iter = {(tt + 1) / rows}");
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

        static void MakeResult2(Func<byte[], double> fun, string acc = "")
        {
            while (MakeCoordMin(fun))
            {
                Console.WriteLine("Записывается в файл");
                WriteData(best, acc);
            }
            return;
        }
        static void MakeResult3(Func<byte[], double> fun, string acc = "")
        {
            while (MakeSampleBest(fun, 200, 5, 1))
            {
                Console.WriteLine("Записывается в файл");
                WriteData(best, acc);
            }
            return;
        }
        static void MakeResult4(string acc = "")
        {
            while (MakeCoordMin(preference_costs2) || MakeCoordMin(accounting_penalty))
            {
                Console.WriteLine("Записывается в файл");
                WriteData(best, acc);
                MakeResult2(score);
            }
            return;
        }
        static void MakeResult5(Func<byte[], double> fun, string acc = "")
        {
            while (MakeCoordMinSlow(fun))
            {
                Console.WriteLine("Записывается в файл");
                WriteData(best, acc);
            }
            return;
        }

        static void Randomize(int count = 15)
        {
            int number;
            byte tmp;
            for (int i = 0; i < count; i++)
            {
                number = randomgen.Next(0, 4999);
                tmp = res[number];
                if (tmp != choice_0[number])
                {
                 res[number] = choice_0[number];
                if(score(res)>=1e20)
                {
                    res[number] = tmp;
                    i--;
                }
                }                 
                else
                    i--;

            }
              
        }

        const int countbegins = 1000;
        static byte[][] begins = new byte[countbegins][];
        static void ReadBegins()
        {
            using (var s = new StreamReader("begins.csv"))
            {
                for (int i = 0; i < countbegins; i++)
                {
                    ref var bg = ref begins[i];
                    bg = new byte[5000];
                    for (int j = 0; j < bg.Length; j++)
                        bg[j] = Convert.ToByte(s.ReadLine());
                }
            }
        }

        static void BatchMethod(Func<byte[], double> fun, int size = 20, int count = 400)
        {
            best = fun(res);
            byte[][] mat = GetNresCopy();

            var numbers = Enumerable.Range(0, count);
            double[] results = new double[100];

            foreach (var nb in numbers)
            {
                for (byte i = 0; i < 100; i++)
                    for (byte s = 0; s < size; s++)
                        mat[i][randomgen.Next(0, 4999)] = (byte)randomgen.Next(1, 100);
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
                    for (int s = 0; s < 5000; s++)
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

            for (int tmp = 0; tmp < count; tmp++)
            {
                for (byte s = 0; s < size; s++)
                    res[randomgen.Next(0, 4999)] = (byte)randomgen.Next(1, 100);
                MakeResult2(fun, "batch");
                if (fun(res) < fun(res2))
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
            ReadRES();

            // MakeResult2();

            // var t = preference_costs(res);
            // var s = accounting_penalty(res);

            //ReadBegins();

            //ReadRES();
            if(false)
            for (int y = 0; y < countbegins; y++)
            {

                // res = begins[y];

                Console.WriteLine($"----------------ITERATION {y + 1}. Begin score = {best}");

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

                SuperMinimizingPreferenceCosts(2);
                Console.WriteLine("Next step prf...");
                MakeCoordMinSlow(accounting_penalty);
                Console.WriteLine("Next step acc...");
                //MakeCoordMinSlow(score);
                MakeResult2(score);
                Console.WriteLine("Next step score...");


                //SuperMinimizingPreferenceCosts(int.MaxValue);

                ////best = accounting_penalty(res);
                ////MakeCoordMin(accounting_penalty,40);
                ////Console.WriteLine("All ");

                //best = score(res);
                //MakeResult2(score);
                //best = score(res);

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


            foreach (var t in Enumerable.Range(1, 10))
            {
                RandomDown();
                NotRandomDown();
            }

        }

        static void RandomDown(int iter = 15)
        {
                double super = score(res);
                var res0 = res.Take(5000).ToArray();

                int i = 0;

                while (i<iter)
                {
                    //SuperMinimizingPreferenceCosts(deep);
                    //MakeCoordMinSlow(score);
                    // Console.WriteLine("Next step...");
                    MakeResult2(score);

                    best = score(res);
                    if (best >= super)
                    {
                        res = res0.Take(5000).ToArray();
                        int count = new Random().Next(3, 14);
                        Console.WriteLine($"Randomize... count = {count}... bad score {best} >= {super} ");
                        Randomize(count);
                    }
                    else
                    {
                        res0 = res.Take(5000).ToArray();
                        i = 0;
                        super = best;
                    }
                i++;
                }           
        }
        static void NotRandomDown(int iter = 3)
        {

            var deeps = new int[]
            {
                1,2,3,5,  7 ,  11 ,   17     , 29 ,  37,
               53 ,   79 ,
 107,  131, 151,
 181,  223,
 263, 
 317 ,
  419 //,421 ,431, 433,
//439, 443, 449 ,457, 461 ,463 ,467, 479, 487, 491, 499 ,503,
//509, 521, 523 ,541, 547 ,557 ,563, 569 ,571, 577 ,587 ,593,
//599 ,601, 607, 613 ,617 ,619 ,631, 641 ,643, 647, 653 ,659,
//661, 673, 677 ,683 ,691, 701 ,709, 719, 727, 733, 739 ,743,
//751, 757, 761 ,769 ,773 ,787, 797, 809 ,811, 821 ,823 ,827,
//829, 839, 853 ,857 ,859 ,863, 877 ,881, 883, 887, 907, 911,
//919, 929, 937, 941 ,947, 953, 967 ,971, 977 ,983, 991 ,997
            };

            foreach (var t in Enumerable.Range(1, iter))
            {
                double super = score(res);
                var res0 = res.Take(5000).ToArray();

                int deep = deeps[0];
                int i = 0;
                while (true)
                {
                    SuperMinimizingPreferenceCosts(deep);
                    //MakeCoordMinSlow(score);
                    Console.WriteLine("Next step...");
                    MakeResult2(score);

                    best = score(res);
                    if (best >= super)
                    {
                        res = res0.Take(5000).ToArray();
                        deep = deeps[++i];//deeps[new Random().Next(0,deeps.Length-1)]; 
                    }
                    else
                    {
                        res0 = res.Take(5000).ToArray();
                        deep = deeps[0];
                        super = best;
                    }
                    if (i == deeps.Length - 1)
                        break;
                }

            }
        }
    }
}
