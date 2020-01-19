using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MathNet.Numerics.Random;
//using Accord.Math;
//using Alea.Parallel;
using МатКлассы;
using System.Diagnostics;

namespace Покоординатная_минимизация
{
    static class Program
    {
        static readonly SystemRandomSource randomgen = new SystemRandomSource();
        static readonly int[] Range = Enumerable.Range(0, 5000).ToArray();
        static readonly int[] family_id = Enumerable.Range(0, 5000).ToArray();

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
        static readonly (byte, byte, byte)[] samples3 = new (byte, byte, byte)[1000000];
        static readonly (byte, byte)[] samples2 = new (byte, byte)[10000];

        static double[] lastN = new double[176];
        static int[][] prCosts = new int[5000][];
        static double[][] Ntonumber = new double[176][];

        /// <summary>
        /// Всевозможные комбинации пар от 1 до 100
        /// </summary>
        static (byte, byte)[] combinations2;

        static byte[][] Tops = new byte[5000][];

        static Program()
        {
            ReadData();

            combinations2 = new (byte, byte)[10000];
            for (int n1 = 0; n1 < 100; n1++)
                for (int n2 = 0; n2 < 100; n2++)
                    combinations2[(n1 * 100) + n2] = ((byte)(n1 + 1), (byte)(n2 + 1));

            for (int k = 0; k < 5000; k++)
            {
                prCosts[k] = new int[100];
                int arri;
                for (int i = 0; i < 100; i++)
                {
                    arri = i + 1;

                    if (arri == choice_0[k])
                    {
                        prCosts[k][i] = 0;
                    }
                    else if (arri == choice_1[k])
                    {
                        prCosts[k][i] = 50;
                    }
                    else if (arri == choice_2[k])
                    {
                        prCosts[k][i] = ifchoice2[k];
                    }
                    else if (arri == choice_3[k])
                    {
                        prCosts[k][i] = ifchoice3[k];
                    }
                    else if (arri == choice_4[k])
                    {
                        prCosts[k][i] = ifchoice4[k];
                    }
                    else if (arri == choice_5[k])
                    {
                        prCosts[k][i] = ifchoice5[k];
                    }
                    else if (arri == choice_6[k])
                    {
                        prCosts[k][i] = ifchoice6[k];
                    }
                    else if (arri == choice_7[k])
                    {
                        prCosts[k][i] = ifchoice7[k];
                    }
                    else if (arri == choice_8[k])
                    {
                        prCosts[k][i] = ifchoice8[k];
                    }
                    else if (arri == choice_9[k])
                    {
                        prCosts[k][i] = ifchoice9[k];
                    }
                    else
                        prCosts[k][i] = ifnochoice[k];
                }

                Tops[k] = new byte[] { choice_0[k], choice_1[k], choice_2[k], choice_3[k], choice_4[k], choice_5[k], choice_6[k], choice_7[k], choice_8[k], choice_9[k] };
            }

            int n;
            for (int i = 0; i < lastN.Length; i++)
            {
                n = i + 125;
                lastN[i] = ((n - 125.0) / 400.0 * Math.Sqrt(n)) ;
            }

            for (short i = 125; i <= 300; i++)
            {
                Ntonumber[i - 125] = new double[176];
                for (short j = 125; j <= 300; j++)
                    Ntonumber[i - 125][j - 125] = ((i - 125.0) / 400.0 * Math.Pow(i, 0.5 + 0.02 * Math.Abs(i - j))) ;
            }

            int u = 0, v = 0;
            for (byte i = 1; i <= 100; i++)
                for (byte j = 1; j <= 100; j++)
                {
                    samples2[v++] = (i, j);
                    for (byte h = 1; h <= 100; h++)
                        samples3[u++] = (i, j, h);
                }

        }


        static byte RandVal(int index, int top = 10)
        {
            if (top == 1)
                return choice_0[index];
            return Tops[index][randomgen.Next(0, top - 1)];
            //var p = new byte[] { choice_0[index], choice_1[index], choice_2[index], choice_3[index], choice_4[index], choice_5[index], choice_6[index], choice_7[index], choice_8[index], choice_9[index] };
            //return p[randomgen.Next(0, top - 1)];
        }

        static byte[] Top(int index, int count) => Tops[index].Take(count).ToArray();
        //{
        //    var p = new byte[] { choice_0[index], choice_1[index], choice_2[index], choice_3[index], choice_4[index], choice_5[index], choice_6[index], choice_7[index], choice_8[index], choice_9[index] };
        //    return p.Take(count).ToArray();
        //}

        static byte[] Randomize(byte[] arr, int count)
        {
            int k1, k2;
            byte tmp;
            for (int i = 0; i < count; i++)
            {
                k1 = randomgen.Next(0, 4999);
                k2 = randomgen.Next(0, 4999);
                tmp = arr[k1];
                arr[k1] = arr[k2];
                arr[k2] = tmp;
            }
            return arr;
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
                if (tmp < 125 || tmp > 300)
                    return 1e20f;
            }

            double sum = 0;
            for (int i = 98; i >= 0; i--)
            {
                tmp = count[i];
                sum += ((tmp - 125.0) / 400.0 * Math.Pow(tmp, 0.5 + 0.02 * Math.Abs(tmp - count[i + 1]))) ;
            }

            return sum + ((count[99] - 125.0) / 400.0 * Math.Sqrt(count[99])) ;
        };
        static Func<byte[], double> preference_costs2 = (byte[] arr) =>
        {
            if (wall(arr))
                return 1e20f;
            return preference_costs(arr);
        };
        static Func<byte[], double> accounting_penaltyMemoized = (byte[] arr) =>
        {
            short[] count = new short[100];
            for (int i = 0; i < arr.Length; i++)
            {
                count[arr[i] - 1] += n_people[i];
            }

            short tmp;
            for (int i = 0; i < count.Length; i++)
            {
                tmp = count[i];
                if (tmp < 125 || tmp > 300)
                    return 1e20f;
            }

            double sum = lastN[count[99] - 125];
            for (int i = 98; i >= 0; i--)
            {
                sum += Ntonumber[count[i] - 125][count[i + 1] - 125];
            }

            return sum;
        };
        static Func<byte[], int> preference_costsMemoized = (byte[] arr) =>
        {
            int S = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                S += prCosts[i][arr[i] - 1];
            }

            return S;
        };

        static Func<short[], double> accounting_penalty2 = (short[] count) =>
        {
            short tmp;
            for (int i = 0; i < count.Length; i++)
            {
                tmp = count[i];
                if (tmp < 125 || tmp > 300)
                    return 1e20f;
            }
            return accounting_penalty3(count);
        };
        /// <summary>
        /// То же самое, что и 2, но без проверки
        /// </summary>
        static Func<short[], double> accounting_penalty3 = (short[] count) =>
        {
            double sum = lastN[count[99] - 125];
            for (int i = 98; i >= 0; i--)
            {
                sum += Ntonumber[count[i] - 125][count[i + 1] - 125];
            }

            return sum;
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
            //    return 1e20f;

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
            if (acc >= 1e20f) return acc;
            return acc + preference_costs(arr);
        };

        static Func<byte[], double> scoreMemoized = (byte[] arr) =>
        {
            int S = 0, arrm1;
            short[] count = new short[100];
            for (int i = 0; i < arr.Length; i++)
            {
                arrm1 = arr[i] - 1;
                count[arrm1] += n_people[i];
                S += prCosts[i][arrm1];
            }

            short tmp;
            for (int i = 0; i < count.Length; i++)
            {
                tmp = count[i];
                if (tmp < 125 || tmp > 300)
                    return 1e20f;
            }

            double sum = lastN[count[99] - 1];
            for (int i = 98; i >= 0; i--)
            {
                sum += Ntonumber[count[i] - 125][count[i + 1] - 125];
            }

            return sum + S;

        };
        static Func<byte[], double> scoreMemoized2 = (byte[] arr) =>
        {
            double acc = accounting_penaltyMemoized(arr);
            if (acc >= 1e20f) return acc;
            return acc + preference_costsMemoized(arr);
        };


        static void ReadRES()
        {
            using (var s = new StreamReader("res.csv"))
            {
                s.ReadLine();
                for (int i = 0; i < 5000; i++)
                    res[i] = Convert.ToByte(s.ReadLine().Split(',')[1]);
            }
            best = scoreMemoized2(res);
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
        static void WriteData(byte[] rs, string path)
        {
            using (var s = new StreamWriter(Path.Combine(path, $"res {scoreMemoized2(rs)}.csv")))
            {
                s.WriteLine("family_id,assigned_day");
                for (int i = 0; i < rs.Length; i++)
                    s.WriteLine($"{i},{rs[i]}");
            }
        }

        static int[] GetRange() => Enumerable.Range(0, 5000).ToArray();
        static int[] GetRandom(int deep = 5000)
        {
            var s = GetRange();
            int tmp1, tmp2;
            int tmp;
            for (int i = 0; i < deep; i++)
            {
                tmp1 = randomgen.Next(4999);
                tmp2 = randomgen.Next(4999);
                tmp = s[tmp1];
                s[tmp1] = s[tmp2];
                s[tmp2] = tmp;
            }
            return s;
        }
        static int[] GetRandom(int[] minimum, int deep = 5000)
        {
            var s = GetRange();
            int tmp1, tmp2;
            int tmp;
            for (int i = 0; i < deep; i++)
            {
                tmp1 = randomgen.Next(4999);
                tmp2 = randomgen.Next(4999);
                tmp = s[tmp1];
                s[tmp1] = s[tmp2];
                s[tmp2] = tmp;
            }


            for (int i = 0; i < minimum.Length; i++)
            {
                int index = Array.IndexOf(s, minimum[i]);
                tmp = s[index];
                s[index] = s[i];
                s[i] = tmp;
            }



            return s;
        }
        static int[] GetRandomIndexes(int count = 100)
        {
            return Enumerable.Range(0, count).Select(s => randomgen.Next(0, 4999)).ToArray();
        }
        static int[] GetNotZeroChoises()
        {
            List<int> rs = new List<int>(4000);
            for (int i = 0; i < res.Length; i++)
                if (res[i] != choice_0[i])
                    rs.Add(i);
            return rs.ToArray();
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
        static bool MakeCoordMin(Func<byte[], double> fun, int count = 5000)
        {
            best = fun(res);

            bool existprogress = false;

            byte[][] mat = GetNresCopy();

            var numbers = GetRandom().Take(count).ToArray(); //GetRange();
            double[] results = new double[100];

            foreach (var nb in numbers)
            {
                //for (byte i = 0; i < 100; i++)
                //    mat[i][nb] = (byte)(i + 1);

                //results = mat.AsParallel().Select(arr => fun(arr)).ToArray();
                //double bst = results.Min();
                //int n = Array.IndexOf(results, bst) + 1;

                //byte bn = (byte)n;
                //for (byte i = 0; i < 100; i++)
                //    mat[i][nb] = bn;
                var sm = MinByOne(nb);

                if (best > sm.res)
                {
                    best = sm.res;
                    existprogress = true;
                    res[nb] = sm.val;
                    Console.WriteLine($"best score = {Math.Round(best, 3)}; iter = {nb}");
                }

            }

            return existprogress;
        }

        static void MakeCoordMinPr(byte[] res2, int count = 5000)
        {
            bool existprogress = false;
            double bst;

            int[] numbers; //GetRange();
            int pr;
            short[] acr;

        iti:
            acr = GetMap(res2);
            pr = preference_costsMemoized(res2);
            bst = pr;// + accounting_penalty3(acr);
            numbers = GetRandomIndexes(count); //GetRandom().Take(count).ToArray();

            foreach (var nb in numbers)
            {
                var sm = MinByOnePr(nb, res2, pr, acr);

                if (bst > sm.res)
                {
                    bst = sm.res;
                    existprogress = true;
                    res2[nb] = sm.val;
                    acr = GetMap(res2);
                    pr = preference_costsMemoized(res2);
                    //Console.WriteLine($"best score = {Math.Round(best, 3)}; iter = {nb}");
                }

            }
            if (!existprogress)
                goto iti;

            //Console.WriteLine($"pr = {preference_costsMemoized(res2)}  acc = {accounting_penaltyMemoized(res2)}  sum = {scoreMemoized2(res2)}");
        }
        static void MakeCoordMinAcc(byte[] res2, int count = 5000)
        {
            bool existprogress = false;
            double bst;

            int[] numbers; //GetRange();
            int pr;
            short[] acr;

        iti:
            acr = GetMap(res2);
            pr = preference_costsMemoized(res2);
            bst = accounting_penalty3(acr);
            numbers = GetRandomIndexes(count); //GetRandom().Take(count).ToArray();

            foreach (var nb in numbers)
            {
                var sm = MinByOneAcc(nb, res2, pr, acr);

                if (bst > sm.res)
                {
                    bst = sm.res;
                    existprogress = true;
                    res2[nb] = sm.val;
                    acr = GetMap(res2);
                    pr = preference_costsMemoized(res2);
                    //Console.WriteLine($"best score = {Math.Round(best, 3)}; iter = {nb}");
                }

            }
            if (!existprogress)
                goto iti;

            Console.WriteLine($"pr = {preference_costsMemoized(res2)}  acc = {accounting_penaltyMemoized(res2)}  sum = {scoreMemoized2(res2)}");
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

            //byte[][] mat = GetNresCopy();

            var numbers = GetRange();
            var result = new (double, byte, int)[5000];
            int index = 0;
            double bst;

            double[] results = new double[100];
            foreach (var nb in numbers)
            {
                //for (byte i = 0; i < 100; i++)
                //    mat[i][nb] = (byte)(i + 1);

                //results = mat.AsParallel().Select(arr => fun(arr)).ToArray();

                //bst = results.Min();
                //int n = Array.IndexOf(results, bst) + 1;

                //result[index++] = (bst, mat[n - 1][nb], nb);

                //byte bn = (byte)n;
                //for (byte i = 0; i < 100; i++)
                //    mat[i][nb] = res[nb];

                var sm = MinByOne(nb);
                result[index++] = (sm.res, sm.val, nb);

            }

            bst = result.Min(t => t.Item1);

            if (best > bst)
            {
                var pi = result.First(t => t.Item1 == bst);
                Console.WriteLine($"best score = {Math.Round(bst, 3)} (from {Math.Round(best, 3)}); iter = {pi.Item3}");
                best = bst;
                existprogress = true;
                res[pi.Item3] = pi.Item2;
            }


            return existprogress;
        }

        /// <summary>
        /// Делает покоординатный спуск по лучшим координатам до тех пор, пока спуск возможен
        /// </summary>
        /// <param name="sample_res"></param>
        /// <returns></returns>
        static (double, byte[]) MakeCoordMinSlow(byte[] sample_res)
        {
            double locbest = scoreMemoized2(sample_res);
            bool existprogress;
            var numbers = family_id;
            var result = new (double, byte, int)[5000];
            int index, pr;
            double bst;
            short[] acr;

        begin1:
            index = 0;
            existprogress = false;
            pr = preference_costsMemoized(sample_res);
            acr = GetMap(sample_res);
            foreach (var nb in numbers)
            {
                var sm = MinByOneChoise(nb, sample_res, pr, acr);//--------------------------------------------------------------------------
                result[index++] = (sm.res, sm.val, nb);
            }

            bst = result.Min(t => t.Item1);

            if (locbest > bst)
            {
                var pi = result.First(t => t.Item1 == bst);
                //Console.WriteLine($"best score = {Math.Round(bst, 3)} (from {Math.Round(best, 3)}); iter = {pi.Item3}");           
                existprogress = true;
                locbest = bst;
                sample_res[pi.Item3] = pi.Item2;
            }

            if (locbest == 1e20f)
                return (locbest, sample_res);

            if (existprogress && locbest != best)
                goto begin1;
            else
                return (locbest, sample_res);
        }
        static (double, byte[]) MakeCoordMinSlow2(byte[] sample_res)
        {

            return MinByTwoChoise(sample_res, preference_costsMemoized(sample_res), GetMap(sample_res));
        }
        static (double, byte[]) MakeCoordMinSlow2(byte[] sample_res, int[] bt)
        {
            return MinByTwoChoise(sample_res, bt, preference_costsMemoized(sample_res), GetMap(sample_res));
        }
        static (double, byte[]) MakeCoordMinSlow2Parallel(byte[] sample_res, int[] bt)
        {
            return MinByTwoChoiseParallel(sample_res, bt, preference_costsMemoized(sample_res), GetMap(sample_res));
        }
        static (double, byte[]) MakeCoordMinSlowPr(byte[] sample_res, int count = 15)
        {
            double locbest = preference_costsMemoized(sample_res);
            bool existprogress;
            var numbers = family_id;
            var result = new (double, byte, int)[5000];
            int index, pr;
            double bst;
            short[] acr;

        begin1:
            index = 0;
            existprogress = false;
            pr = preference_costsMemoized(sample_res);
            acr = GetMap(sample_res);
            foreach (var nb in numbers)
            {
                var sm = MinByOnePr(nb, sample_res, pr, acr);//--------------------------------------------------------------------------
                result[index++] = (sm.res, sm.val, nb);
            }

            bst = result.Min(t => t.Item1);

            if (locbest > bst)
            {
                var pi = result.First(t => t.Item1 == bst);
                //Console.WriteLine($"best score = {Math.Round(bst, 3)} (from {Math.Round(best, 3)}); iter = {pi.Item3}");           
                existprogress = true;
                locbest = bst;
                sample_res[pi.Item3] = pi.Item2;
            }

            if (existprogress && locbest != best && count > 0)
            {
                count--;
                goto begin1;
            }
            else
                return (locbest, sample_res);
        }

        static (double, byte[]) MakeCoordMinRand(byte[] sample_res)
        {
            double locbest = scoreMemoized2(sample_res);
            bool existprogress;
            var numbers = family_id;
            var result = new (double, byte, int)[5000];
            int pr;
            short[] acr;

        begin1:
            existprogress = false;
            pr = preference_costsMemoized(sample_res);
            acr = GetMap(sample_res);
            foreach (var nb in GetRandom())
            {
                var sm = MinByOneChoise(nb, sample_res, pr, acr);//--------------------------------------------------------------------------

                if (sm.res < locbest)
                {
                    existprogress = true;
                    locbest = sm.res;
                    pr -= prCosts[nb][sample_res[nb] - 1];
                    acr[sample_res[nb] - 1] -= n_people[nb];

                    sample_res[nb] = sm.val;

                    pr += prCosts[nb][sample_res[nb] - 1];
                    acr[sample_res[nb] - 1] += n_people[nb];
                }
            }

            if (existprogress && locbest != best)
                goto begin1;
            else
                return (locbest, sample_res);
        }


        static (double, byte[]) Make2CoordMinSlow(byte[] sample_res)
        {
            double locbest = scoreMemoized2(sample_res);
            bool existprogress;
            var numbers = family_id;
            var result = new (double, byte, byte, int)[4999];
            int index, pr;
            double bst;
            short[] acr;

        begin1:
            index = 0;
            existprogress = false;
            pr = preference_costsMemoized(sample_res);
            acr = GetMap(sample_res);
            for (int nb = 0; nb < 4999; nb++)
            {
                var sm = MinByOneChoise2(nb, sample_res, pr, acr);//--------------------------------------------------------------------------
                result[index++] = (sm.res, sm.val1, sm.val2, nb);
            }

            bst = result.Min(t => t.Item1);

            if (locbest > bst)
            {
                var pi = result.First(t => t.Item1 == bst);
                //Console.WriteLine($"best score = {Math.Round(bst, 3)} (from {Math.Round(best, 3)}); iter = {pi.Item3}");           
                existprogress = true;
                locbest = bst;
                sample_res[pi.Item4] = pi.Item2;
                sample_res[pi.Item4 + 1] = pi.Item3;
            }

            if (locbest == 1e20f)
                return (locbest, sample_res);

            if (existprogress && locbest != best)
                goto begin1;
            else
                return (locbest, sample_res);
        }


        /// <summary>
        /// Показывает, сколько дней из 100 выходят за условие
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        static int IndexOfArray(byte[] arr)
        {
            int S = 0;
            short tmp;
            short[] count = new short[100];
            for (int i = 0; i < arr.Length; i++)
            {
                count[arr[i] - 1] += n_people[i];
            }
            for (int i = 0; i < count.Length; i++)
            {
                tmp = count[i];
                if (tmp < 125 || tmp > 300)
                    S++;
            }
            return S;
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
        static void MakeResult6(string acc = "")
        {
            while (MinByRandomize())
            {
                Console.WriteLine("Записывается в файл");
                WriteData(best, acc);
                MakeResult2(scoreMemoized2);
            }
        }
        static void MakeResult7(string acc = "")
        {
            byte[][] mat = GetNresCopy();
            double[] results = new double[100];

            for (int i = 115; i < 4999; i++)
            {
                // if (i % 2 == 0)
                Console.WriteLine($"i = {i}");
                for (int j = i + 1; j < 5000; j++)
                {
                    if (j % 500 == 0)
                        Console.WriteLine($"j = {j}");

                    if (MinByTwo(i, j, ref mat, ref results))
                    {
                        best = scoreMemoized2(res);
                        Console.WriteLine($"Записывается в файл (улучшено до {best})");
                        WriteData(best, acc);
                        // i = -1;
                        i--;
                        MakeResult5(scoreMemoized2, "");
                        //MakeResult2(scoreMemoized2, "");
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// Минимизация по всем парам координат
        /// </summary>
        /// <param name="acc"></param>
        static void MakeResult8(string acc = "")
        {
            double[] results = new double[100];
            int[] pres = new int[100];
            short[][] accs = new short[100][];

            for (int i = 0; i < 4999; i++)
            {
                // if (i % 2 == 0)
                Console.WriteLine($"i = {i}");
                for (int j = i + 1; j < 5000; j++)
                {
                    if (j % 500 == 0)
                        Console.WriteLine($"j = {j}");

                    if (MinByTwo2(i, j, ref pres, ref accs, ref results))
                    {
                        best = scoreMemoized2(res);
                        Console.WriteLine($"Записывается в файл (улучшено до {best})");
                        WriteData(best, acc);
                        // i = -1;
                        i--;
                        MakeResult5(scoreMemoized2, "");
                        //MakeResult2(scoreMemoized2, "");
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Минимизация по случайной замене некоторых компонент
        /// </summary>
        /// <param name="acc"></param>
        /// <param name="dim"></param>
        /// <param name="count"></param>
        /// <param name="iter"></param>
        static void MakeResult9(string acc = "", int dim = 10, int count = 2000, int iter = 10_000)
        {
            for (int i = 0; i < iter; i++)
                if (MinByRandomize2(dim, count))
                {
                    //Console.WriteLine("Записывается в файл");
                    //WriteData(best, acc);
                    MakeResult5(scoreMemoized2);
                }
        }

        static void MakeResult10(string acc = "", int istep = 1, int jstep = 1, int pstep = 1)
        {
            for (int i = 0; i < 4998; i += istep)
            {
                Console.WriteLine($"i = {i}");
                for (int j = i + 1; j < 4999; j += jstep)
                {
                    Console.WriteLine($"j = {j}");
                    for (int p = j + 1; p < 5000; p += pstep)
                    {
                        if (p % 11 == 0)
                            Console.WriteLine($"k = {p}");
                        if (MinByThree(i, j, p, samples3))
                        {
                            best = scoreMemoized2(res);
                            Console.WriteLine($"Записывается в файл (улучшено до {best})");
                            WriteData(best, acc);
                            // i = -1;
                            p--;
                            MakeResult5(scoreMemoized2, "");
                            //MakeResult2(scoreMemoized2, "");
                            break;
                        }
                    }

                }
            }
        }

        static void MakeResult11(string acc = "", int iter = 100000)
        {
            int i, j, k;
            for (int index = 0; index < iter; index++)
            {
                if (index % 100 == 0)
                    Console.WriteLine(index);

                beg:
                i = randomgen.Next(0, 4999);
                j = randomgen.Next(0, 4999);
                k = randomgen.Next(0, 4999);
                if (i == j || i == k || j == k)
                    goto beg;

                if (MinByThree(i, j, k, samples3))
                {
                    best = scoreMemoized2(res);
                    Console.WriteLine($"Записывается в файл (улучшено до {best})");
                    WriteData(best, acc);
                    // i = -1;
                    MakeResult5(scoreMemoized2, "");
                    //MakeResult2(scoreMemoized2, "");
                    break;
                }
            }
        }

        static void MakeResult12(string acc = "", int istep = 1, int jstep = 1)
        {

            for (int i = 0; i < 4998; i += istep)
            {
                Console.WriteLine($"i = {i}");
                for (int j = i + 1; j < 4999; j += jstep)
                {
                    Console.WriteLine($"j = {j}");
                    var vals = new (double, (int, int, int), (byte, byte, byte))[4999 - j];
                    Parallel.For(j + 1, 5000, (int p) => vals[p - j - 1] = MinByThreeForParallel(i, j, p, samples3));

                    var bt = vals.Min(v => v.Item1);
                    var rs = vals.First(v => v.Item1 == bt);

                    if (bt < best)
                    {
                        var ind = rs.Item2;
                        var s = rs.Item3;
                        res[ind.Item1] = s.Item1;
                        res[ind.Item2] = s.Item2;
                        res[ind.Item3] = s.Item3;

                        best = scoreMemoized2(res);
                        Console.WriteLine($"Записывается в файл (улучшено до {best})");
                        WriteData(best, acc);
                        j = -1;
                        MakeResult5(scoreMemoized2, "");
                        //MakeResult2(scoreMemoized2, "");
                        //break;
                    }


                }
            }
        }

        static void MakeResult13(string acc = "", int istep = 1, int jstep = 1)
        {
            for (int i = 0; i < 4999; i += istep)
            {
                Console.WriteLine($"i = {i}");
                var vals = new (double, (int, int), (byte, byte))[4999 - i];

                Parallel.For(i + 1, 5000, (int j) => vals[j - i - 1] = MinByTwoForParallel(i, j));

                var bt = vals.Min(v => v.Item1);
                var rs = vals.First(v => v.Item1 == bt);

                if (bt < best)
                {
                    var ind = rs.Item2;
                    var s = rs.Item3;
                    res[ind.Item1] = s.Item1;
                    res[ind.Item2] = s.Item2;

                    best = scoreMemoized2(res);
                    Console.WriteLine($"Записывается в файл (улучшено до {best})");
                    WriteData(best, acc);
                    i = -1;
                    MakeResult5(scoreMemoized2, "");
                    //MakeResult2(scoreMemoized2, "");
                    //break;
                }
            }
        }

        static void MakeResult14(string acc = "")
        {
            List<int> ch0 = new List<int>(4000), ch1 = new List<int>(1050);
            for (int i = 0; i < res.Length; i++)
                if (res[i] == choice_0[i])
                    ch0.Add(i);
                else ch1.Add(i);


            var vals = new (double, (int, int), (byte, byte))[ch0.Count];
            foreach (var i in ch1)
            {
                Console.WriteLine($"i = {i}");

                Parallel.For(0, ch0.Count, (int j) => vals[j] = MinByTwoForParallel(i, j));

                var bt = vals.Min(v => v.Item1);
                var rs = vals.First(v => v.Item1 == bt);

                if (bt < best)
                {
                    var ind = rs.Item2;
                    var s = rs.Item3;
                    res[ind.Item1] = s.Item1;
                    res[ind.Item2] = s.Item2;

                    best = scoreMemoized2(res);
                    Console.WriteLine($"Записывается в файл (улучшено до {best})");
                    WriteData(best, acc);
                    MakeResult5(scoreMemoized2, "");
                    //MakeResult2(scoreMemoized2, "");
                    //break;
                }
            }
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
                    if (score(res) >= 1e20f)
                    {
                        res[number] = tmp;
                        i--;
                    }
                }
                else
                    i--;

            }

        }
        static void Randomize2(int count = 15)
        {
            int number1, number2;
            byte tmp;
            for (int i = 0; i < count; i++)
            {
                number1 = randomgen.Next(0, 4999);
                number2 = randomgen.Next(0, 4999);
                tmp = res[number1];
                res[number1] = res[number2];
                res[number2] = tmp;
            }
        }
        /// <summary>
        /// Попытка уменьшить preferenc3, мало изменяя accounting, делая перестановку пар
        /// </summary>
        /// <returns></returns>
        static bool MinByRandomize()
        {
            byte tmp;
            double bst = scoreMemoized2(res);
            for (int i = 0; i < 4999; i++)
            {
                if (i % 25 == 0)
                    Console.WriteLine($"i = {i}");
                for (int j = i + 1; j < 5000; j++)
                {
                    tmp = res[i];
                    res[i] = res[j];
                    res[j] = tmp;

                    if (scoreMemoized2(res) < bst)
                    {
                        best = scoreMemoized2(res);
                        Console.WriteLine($"new best = {best}");
                        return true;
                    }

                    //tmp = res[j];
                    res[j] = res[i];
                    res[i] = tmp;

                }
            }
            return false;
        }

        static bool MinByRandomize2(int dim = 10, int count = 2000, int top = 5)
        {
            bool existprogress = false;
            double bsttmp, copybest = scoreMemoized2(res);
            best = copybest;

            double[] results = new double[count];
            byte[][] samples = new byte[count][];
            var copy = GetNresCopy(1)[0];

            int[] indexes = new int[dim];
            for (int i = 0; i < dim; i++)
                indexes[i] = randomgen.Next(0, 4999);
            indexes = indexes.Distinct().ToArray();
            dim = indexes.Length;

            int pr = preference_costsMemoized(res);
            var acr = GetMap();
            for (int i = 0; i < dim; i++)
            {
                pr -= prCosts[indexes[i]][res[indexes[i]] - 1];
                acr[res[indexes[i]] - 1] -= n_people[indexes[i]];
            }


            Parallel.For(0, count, (int i) =>
            //for(int i = 0; i < count; i++)
            {
                byte tmp;
                byte[] sample = new byte[dim];
                int pr2 = pr;
                var acr2 = acr.Dup();
                for (int j = 0; j < sample.Length; j++)
                {
                    tmp = RandVal(indexes[j], top);//(byte)randomgen.Next(1, 100);
                    sample[j] = tmp;
                    pr2 += prCosts[indexes[j]][tmp - 1];
                    acr2[tmp - 1] += n_people[indexes[j]];
                }

                if (pr2 >= best || acr2.Any(s => s < 125 || s > 300))
                    results[i] = 1e20f;
                else
                    results[i] = accounting_penalty2(acr2) + pr2;
                samples[i] = sample;
            }
            );

            bsttmp = results.Min();
            int n = Array.IndexOf(results, bsttmp);
            for (int i = 0; i < dim; i++)
            {
                res[indexes[i]] = samples[n][i];
            }
            MakeResult5(scoreMemoized2, "");

            if (best < copybest)
            {
                existprogress = true;

                Console.WriteLine($"Success. best score = {Math.Round(best, 3)}");
            }
            else
            {
                res = copy;
                Console.WriteLine($"Fail. Best score {Math.Round(best, 3)} >= {Math.Round(copybest, 3)}");
                best = copybest;
            }

            return existprogress;
        }

        static (double, byte[]) MinByRandomizeBy10(int top = 5)
        {
            const int dim = 10;

            // var indexes = GetRandom().Take(dim).ToArray();
            var indexes = GetRandomInArray(Range.Where(c => n_people[c] < 6 && n_people[c] > 2).ToArray(), 20).Take(dim).ToArray();
            var peop = indexes.Select(i => n_people[i]).ToArray();
            var copy = res.Dup();
            int pr = preference_costsMemoized(copy);
            var acr = GetMap(copy);
            var fval = pr + accounting_penalty3(acr);
            double first = fval;
            byte[][] TP = new byte[dim][];
            int[][] Costs = new int[dim][];
            byte[] vals = new byte[dim];

            for (int i = 0; i < dim; i++)
            {
                vals[i] = (byte)(copy[indexes[i]] - 1);
                Costs[i] = prCosts[indexes[i]];
                pr -= Costs[i][vals[i]];
                acr[vals[i]] -= peop[i];
                TP[i] = Top(indexes[i], top);
            }

            bool bad(short val) => val < 125 || val > 300;
            bool gut(params int[] I)
            {
                for (int i = 0; i < dim; i++)
                    if (bad(acr[vals[i]]) || bad(acr[I[i] - 1]))
                        return false;
                return true;
            }

            foreach (var i1 in TP[0])
            {
                pr += Costs[0][i1 - 1];
                acr[i1 - 1] += peop[0];

                foreach (var i2 in TP[1])
                {
                    pr += Costs[1][i2 - 1];
                    acr[i2 - 1] += peop[1];

                    foreach (var i3 in TP[2])
                    {
                        pr += Costs[2][i3 - 1];
                        acr[i3 - 1] += peop[2];

                        foreach (var i4 in TP[3])
                        {
                            pr += Costs[3][i4 - 1];
                            acr[i4 - 1] += peop[3];

                            foreach (var i5 in TP[4])
                            {
                                pr += Costs[4][i5 - 1];
                                acr[i5 - 1] += peop[4];

                                foreach (var i6 in TP[5])
                                {
                                    pr += Costs[5][i6 - 1];
                                    acr[i6 - 1] += peop[5];

                                    foreach (var i7 in TP[6])
                                    {
                                        pr += Costs[6][i7 - 1];
                                        acr[i7 - 1] += peop[6];

                                        foreach (var i8 in TP[7])
                                        {
                                            pr += Costs[7][i8 - 1];
                                            acr[i8 - 1] += peop[7];

                                            foreach (var i9 in TP[8])
                                            {
                                                pr += Costs[8][i9 - 1];
                                                acr[i9 - 1] += peop[8];

                                                foreach (var i10 in TP[9])
                                                {
                                                    pr += Costs[9][i10 - 1];
                                                    acr[i10 - 1] += peop[9];

                                                    if (pr < first && gut(i1, i2, i3, i4, i5, i6, i7, i8, i9, i10))
                                                    {
                                                        fval = pr + accounting_penalty3(acr);
                                                        if (fval < first)
                                                        {
                                                            Console.WriteLine($"NEW MINIMUM {fval} ON {new Vectors(indexes)} AND vals {new Vectors(i1, i2, i3, i4, i5, i6, i7, i8, i9, i10)}");
                                                            first = fval;
                                                            copy[indexes[0]] = i1;
                                                            copy[indexes[1]] = i2;
                                                            copy[indexes[2]] = i3;
                                                            copy[indexes[3]] = i4;
                                                            copy[indexes[4]] = i5;
                                                            copy[indexes[5]] = i6;
                                                            copy[indexes[6]] = i7;
                                                            copy[indexes[7]] = i8;
                                                            copy[indexes[8]] = i9;
                                                            copy[indexes[9]] = i10;
                                                            return (first, copy);
                                                        }
                                                    }


                                                    pr -= Costs[9][i10 - 1];
                                                    acr[i10 - 1] -= peop[9];
                                                }

                                                pr -= Costs[8][i9 - 1];
                                                acr[i9 - 1] -= peop[8];
                                            }

                                            pr -= Costs[7][i8 - 1];
                                            acr[i8 - 1] -= peop[7];
                                        }

                                        pr -= Costs[6][i7 - 1];
                                        acr[i7 - 1] -= peop[6];
                                    }

                                    pr -= Costs[5][i6 - 1];
                                    acr[i6 - 1] -= peop[5];
                                }

                                pr -= Costs[4][i5 - 1];
                                acr[i5 - 1] -= peop[4];
                            }

                            pr -= Costs[3][i4 - 1];
                            acr[i4 - 1] -= peop[3];
                        }

                        pr -= Costs[2][i3 - 1];
                        acr[i3 - 1] -= peop[2];
                    }

                    pr -= Costs[1][i2 - 1];
                    acr[i2 - 1] -= peop[1];
                }

                pr -= Costs[0][i1 - 1];
                acr[i1 - 1] -= peop[0];
            }


            // Console.WriteLine($"{indexes[0]} {indexes[1]} {indexes[2]} {indexes[3]} {indexes[4]}");

            return (first, copy);
        }
        static (double, byte[]) MinByRandomizeBy12(int top = 5)
        {
            const int dim = 12;

            //var indexes = GetRandom(Range.Where(c=>res[c]==choice_4[c]).ToArray(),7000).Take(dim).ToArray();
            var indexes = GetRandomInArray(Range, 20).Take(dim).ToArray();
            var peop = indexes.Select(i => n_people[i]).ToArray();
            var copy = res.Dup();
            int pr = preference_costsMemoized(copy);
            var acr = GetMap(copy);
            var fval = pr + accounting_penalty3(acr);
            double first = fval;
            byte[][] TP = new byte[dim][];
            int[][] Costs = new int[dim][];

            for (int i = 0; i < dim; i++)
            {
                pr -= prCosts[indexes[i]][copy[indexes[i]] - 1];
                acr[copy[indexes[i]] - 1] -= peop[i];
                TP[i] = Top(indexes[i], top);
                Costs[i] = prCosts[indexes[i]];
            }

            bool bad(short val) => val < 125 || val > 300;
            bool gut(params int[] I)
            {
                for (int i = 0; i < dim; i++)
                    if (bad(acr[copy[indexes[i]] - 1]) || bad(acr[I[i] - 1]))
                        return false;
                return true;
            }

            foreach (var i1 in TP[0])
            {
                pr += Costs[0][i1 - 1];
                acr[i1 - 1] += peop[0];

                foreach (var i2 in TP[1])
                {
                    pr += Costs[1][i2 - 1];
                    acr[i2 - 1] += peop[1];

                    foreach (var i3 in TP[2])
                    {
                        pr += Costs[2][i3 - 1];
                        acr[i3 - 1] += peop[2];

                        foreach (var i4 in TP[3])
                        {
                            pr += Costs[3][i4 - 1];
                            acr[i4 - 1] += peop[3];

                            foreach (var i5 in TP[4])
                            {
                                pr += Costs[4][i5 - 1];
                                acr[i5 - 1] += peop[4];

                                foreach (var i6 in TP[5])
                                {
                                    pr += Costs[5][i6 - 1];
                                    acr[i6 - 1] += peop[5];

                                    foreach (var i7 in TP[6])
                                    {
                                        pr += Costs[6][i7 - 1];
                                        acr[i7 - 1] += peop[6];

                                        foreach (var i8 in TP[7])
                                        {
                                            pr += Costs[7][i8 - 1];
                                            acr[i8 - 1] += peop[7];

                                            foreach (var i9 in TP[8])
                                            {
                                                pr += Costs[8][i9 - 1];
                                                acr[i9 - 1] += peop[8];

                                                foreach (var i10 in TP[9])
                                                {
                                                    pr += Costs[9][i10 - 1];
                                                    acr[i10 - 1] += peop[9];

                                                    foreach (var i11 in TP[10])
                                                    {
                                                        pr += Costs[10][i11 - 1];
                                                        acr[i11 - 1] += peop[10];
                                                        foreach (var i12 in TP[11])
                                                        {
                                                            pr += Costs[11][i12 - 1];
                                                            acr[i12 - 1] += peop[11];

                                                            if (pr < first && gut(i1, i2, i3, i4, i5, i6, i7, i8, i9, i10, i11, i12))
                                                            {
                                                                fval = pr + accounting_penalty3(acr);
                                                                if (fval < first)
                                                                {
                                                                    Console.WriteLine($"NEW MINIMUM {first} ON {new Vectors(indexes).ToString()}");
                                                                    first = fval;
                                                                    copy[indexes[0]] = i1;
                                                                    copy[indexes[1]] = i2;
                                                                    copy[indexes[2]] = i3;
                                                                    copy[indexes[3]] = i4;
                                                                    copy[indexes[4]] = i5;
                                                                    copy[indexes[5]] = i6;
                                                                    copy[indexes[6]] = i7;
                                                                    copy[indexes[7]] = i8;
                                                                    copy[indexes[8]] = i9;
                                                                    copy[indexes[9]] = i10;
                                                                    copy[indexes[10]] = i11;
                                                                    copy[indexes[11]] = i12;

                                                                    return (first, copy);
                                                                }
                                                            }

                                                            pr -= Costs[11][i12 - 1];
                                                            acr[i12 - 1] -= peop[11];
                                                        }

                                                        pr -= Costs[10][i11 - 1];
                                                        acr[i11 - 1] -= peop[10];
                                                    }

                                                    pr -= Costs[9][i10 - 1];
                                                    acr[i10 - 1] -= peop[9];
                                                }

                                                pr -= Costs[8][i9 - 1];
                                                acr[i9 - 1] -= peop[8];
                                            }

                                            pr -= Costs[7][i8 - 1];
                                            acr[i8 - 1] -= peop[7];
                                        }

                                        pr -= Costs[6][i7 - 1];
                                        acr[i7 - 1] -= peop[6];
                                    }

                                    pr -= Costs[5][i6 - 1];
                                    acr[i6 - 1] -= peop[5];
                                }

                                pr -= Costs[4][i5 - 1];
                                acr[i5 - 1] -= peop[4];
                            }

                            pr -= Costs[3][i4 - 1];
                            acr[i4 - 1] -= peop[3];
                        }

                        pr -= Costs[2][i3 - 1];
                        acr[i3 - 1] -= peop[2];
                    }

                    pr -= Costs[1][i2 - 1];
                    acr[i2 - 1] -= peop[1];
                }

                pr -= Costs[0][i1 - 1];
                acr[i1 - 1] -= peop[0];
            }


            Console.WriteLine($"{indexes[0]} {indexes[1]} {indexes[2]} {indexes[3]} {indexes[4]}");

            return (first, copy);
        }
        static (double, byte[]) MinByRandomizeBy20(int top = 5)
        {
            const int dim = 20;

            var indexes = GetRandom().Take(dim).ToArray();
            var peop = indexes.Select(i => n_people[i]).ToArray();
            var copy = res.Dup();
            int pr = preference_costsMemoized(copy);
            var acr = GetMap(copy);
            var fval = pr + accounting_penalty3(acr);
            double first = fval;
            byte[][] TP = new byte[dim][];
            int[][] Costs = new int[dim][];

            for (int i = 0; i < dim; i++)
            {
                pr -= prCosts[indexes[i]][copy[indexes[i]] - 1];
                acr[copy[indexes[i]] - 1] -= peop[i];
                TP[i] = Top(indexes[i], top);
                Costs[i] = prCosts[indexes[i]];
            }

            bool bad(short val) => val < 125 || val > 300;
            bool gut(params int[] I)
            {
                for (int i = 0; i < dim; i++)
                    if (bad(acr[copy[indexes[i]] - 1]) || bad(acr[I[i] - 1]))
                        return false;
                return true;
            }

            foreach (var i1 in TP[0])
            {
                pr += Costs[0][i1 - 1];
                acr[i1 - 1] += peop[0];

                foreach (var i2 in TP[1])
                {
                    pr += Costs[1][i2 - 1];
                    acr[i2 - 1] += peop[1];

                    foreach (var i3 in TP[2])
                    {
                        pr += Costs[2][i3 - 1];
                        acr[i3 - 1] += peop[2];

                        foreach (var i4 in TP[3])
                        {
                            pr += Costs[3][i4 - 1];
                            acr[i4 - 1] += peop[3];

                            foreach (var i5 in TP[4])
                            {
                                pr += Costs[4][i5 - 1];
                                acr[i5 - 1] += peop[4];

                                foreach (var i6 in TP[5])
                                {
                                    pr += Costs[5][i6 - 1];
                                    acr[i6 - 1] += peop[5];

                                    foreach (var i7 in TP[6])
                                    {
                                        pr += Costs[6][i7 - 1];
                                        acr[i7 - 1] += peop[6];

                                        foreach (var i8 in TP[7])
                                        {
                                            pr += Costs[7][i8 - 1];
                                            acr[i8 - 1] += peop[7];

                                            foreach (var i9 in TP[8])
                                            {
                                                pr += Costs[8][i9 - 1];
                                                acr[i9 - 1] += peop[8];

                                                foreach (var i10 in TP[9])
                                                {
                                                    pr += Costs[9][i10 - 1];
                                                    acr[i10 - 1] += peop[9];

                                                    foreach (var i11 in TP[10])
                                                    {
                                                        pr += Costs[10][i11 - 1];
                                                        acr[i11 - 1] += peop[10];
                                                        foreach (var i12 in TP[11])
                                                        {
                                                            pr += Costs[11][i12 - 1];
                                                            acr[i12 - 1] += peop[11];
                                                            foreach (var i13 in TP[12])
                                                            {
                                                                pr += Costs[12][i13 - 1];
                                                                acr[i13 - 1] += peop[12];
                                                                foreach (var i14 in TP[13])
                                                                {
                                                                    pr += Costs[13][i14 - 1];
                                                                    acr[i14 - 1] += peop[13];
                                                                    foreach (var i15 in TP[14])
                                                                    {
                                                                        pr += Costs[14][i15 - 1];
                                                                        acr[i15 - 1] += peop[14];
                                                                        foreach (var i16 in TP[15])
                                                                        {
                                                                            pr += Costs[15][i16 - 1];
                                                                            acr[i16 - 1] += peop[15];
                                                                            foreach (var i17 in TP[16])
                                                                            {
                                                                                pr += Costs[16][i17 - 1];
                                                                                acr[i17 - 1] += peop[16];
                                                                                foreach (var i18 in TP[17])
                                                                                {
                                                                                    pr += Costs[17][i18 - 1];
                                                                                    acr[i18 - 1] += peop[17];
                                                                                    foreach (var i19 in TP[18])
                                                                                    {
                                                                                        pr += Costs[18][i19 - 1];
                                                                                        acr[i19 - 1] += peop[18];
                                                                                        foreach (var i20 in TP[19])
                                                                                        {
                                                                                            pr += Costs[19][i20 - 1];
                                                                                            acr[i20 - 1] += peop[19];


                                                                                            if (pr < first && gut(i1, i2, i3, i4, i5, i6, i7, i8, i9, i10, i11, i12, i13, i14, i15, i16, i17, i18, i19, i20))
                                                                                            {
                                                                                                fval = pr + accounting_penalty3(acr);
                                                                                                if (fval < first)
                                                                                                {
                                                                                                    Console.WriteLine($"NEW MINIMUM {first} ON {new Vectors(indexes).ToString()}");
                                                                                                    first = fval;
                                                                                                    copy[indexes[0]] = i1;
                                                                                                    copy[indexes[1]] = i2;
                                                                                                    copy[indexes[2]] = i3;
                                                                                                    copy[indexes[3]] = i4;
                                                                                                    copy[indexes[4]] = i5;
                                                                                                    copy[indexes[5]] = i6;
                                                                                                    copy[indexes[6]] = i7;
                                                                                                    copy[indexes[7]] = i8;
                                                                                                    copy[indexes[8]] = i9;
                                                                                                    copy[indexes[9]] = i10;
                                                                                                    copy[indexes[10]] = i11;
                                                                                                    copy[indexes[11]] = i12;
                                                                                                    copy[indexes[12]] = i13;
                                                                                                    copy[indexes[13]] = i14;
                                                                                                    copy[indexes[14]] = i15;
                                                                                                    copy[indexes[15]] = i16;
                                                                                                    copy[indexes[16]] = i17;
                                                                                                    copy[indexes[17]] = i18;
                                                                                                    copy[indexes[18]] = i19;
                                                                                                    copy[indexes[19]] = i20;
                                                                                                    return (first, copy);
                                                                                                }
                                                                                            }


                                                                                            pr -= Costs[19][i20 - 1];
                                                                                            acr[i20 - 1] -= peop[19];
                                                                                        }

                                                                                        pr -= Costs[18][i19 - 1];
                                                                                        acr[i19 - 1] -= peop[18];
                                                                                    }

                                                                                    pr -= Costs[17][i18 - 1];
                                                                                    acr[i18 - 1] -= peop[17];
                                                                                }

                                                                                pr -= Costs[16][i17 - 1];
                                                                                acr[i17 - 1] -= peop[16];
                                                                            }

                                                                            pr -= Costs[15][i16 - 1];
                                                                            acr[i16 - 1] -= peop[15];
                                                                        }

                                                                        pr -= Costs[14][i15 - 1];
                                                                        acr[i15 - 1] -= peop[14];
                                                                    }

                                                                    pr -= Costs[13][i14 - 1];
                                                                    acr[i14 - 1] -= peop[13];
                                                                }

                                                                pr -= Costs[12][i13 - 1];
                                                                acr[i13 - 1] -= peop[12];
                                                            }

                                                            pr -= Costs[11][i12 - 1];
                                                            acr[i12 - 1] -= peop[11];
                                                        }

                                                        pr -= Costs[10][i11 - 1];
                                                        acr[i11 - 1] -= peop[10];
                                                    }

                                                    pr -= Costs[9][i10 - 1];
                                                    acr[i10 - 1] -= peop[9];
                                                }

                                                pr -= Costs[8][i9 - 1];
                                                acr[i9 - 1] -= peop[8];
                                            }

                                            pr -= Costs[7][i8 - 1];
                                            acr[i8 - 1] -= peop[7];
                                        }

                                        pr -= Costs[6][i7 - 1];
                                        acr[i7 - 1] -= peop[6];
                                    }

                                    pr -= Costs[5][i6 - 1];
                                    acr[i6 - 1] -= peop[5];
                                }

                                pr -= Costs[4][i5 - 1];
                                acr[i5 - 1] -= peop[4];
                            }

                            pr -= Costs[3][i4 - 1];
                            acr[i4 - 1] -= peop[3];
                        }

                        pr -= Costs[2][i3 - 1];
                        acr[i3 - 1] -= peop[2];
                    }

                    pr -= Costs[1][i2 - 1];
                    acr[i2 - 1] -= peop[1];
                }

                pr -= Costs[0][i1 - 1];
                acr[i1 - 1] -= peop[0];
            }

          //  Console.WriteLine($"{indexes[0]} {indexes[1]} {indexes[2]} {indexes[3]} {indexes[4]}");

            return (first, copy);
        }
        static (double, byte[]) MinByRandomizeBy10(int[] indexes, int top = 5)
        {
            const int dim = 10;

            var peop = indexes.Select(i => n_people[i]).ToArray();
            var copy = res.Dup();
            int pr = preference_costsMemoized(copy);
            var acr = GetMap(copy);
            var fval = pr + accounting_penalty3(acr);
            double first = fval;
            byte[][] TP = new byte[dim][];
            int[][] Costs = new int[dim][];
            byte[] vals = new byte[dim];

            for (int i = 0; i < dim; i++)
            {
                vals[i] = (byte)(copy[indexes[i]] - 1);
                Costs[i] = prCosts[indexes[i]];
                pr -= Costs[i][vals[i]];
                acr[vals[i]] -= peop[i];
                TP[i] = Top(indexes[i], top);
            }

            bool bad(short val) => val < 125 || val > 300;
            bool gut(params int[] I)
            {
                for (int i = 0; i < dim; i++)
                    if (bad(acr[vals[i]]) || bad(acr[I[i] - 1]))
                        return false;
                return true;
            }

            foreach (var i1 in TP[0])
            {
                pr += Costs[0][i1 - 1];
                acr[i1 - 1] += peop[0];

                foreach (var i2 in TP[1])
                {
                    pr += Costs[1][i2 - 1];
                    acr[i2 - 1] += peop[1];

                    foreach (var i3 in TP[2])
                    {
                        pr += Costs[2][i3 - 1];
                        acr[i3 - 1] += peop[2];

                        foreach (var i4 in TP[3])
                        {
                            pr += Costs[3][i4 - 1];
                            acr[i4 - 1] += peop[3];

                            foreach (var i5 in TP[4])
                            {
                                pr += Costs[4][i5 - 1];
                                acr[i5 - 1] += peop[4];

                                foreach (var i6 in TP[5])
                                {
                                    pr += Costs[5][i6 - 1];
                                    acr[i6 - 1] += peop[5];

                                    foreach (var i7 in TP[6])
                                    {
                                        pr += Costs[6][i7 - 1];
                                        acr[i7 - 1] += peop[6];

                                        foreach (var i8 in TP[7])
                                        {
                                            pr += Costs[7][i8 - 1];
                                            acr[i8 - 1] += peop[7];

                                            foreach (var i9 in TP[8])
                                            {
                                                pr += Costs[8][i9 - 1];
                                                acr[i9 - 1] += peop[8];

                                                foreach (var i10 in TP[9])
                                                {
                                                    pr += Costs[9][i10 - 1];
                                                    acr[i10 - 1] += peop[9];

                                                    if (pr < first && gut(i1, i2, i3, i4, i5, i6, i7, i8, i9, i10))
                                                    {
                                                        fval = pr + accounting_penalty3(acr);
                                                        if (fval < first)
                                                        {
                                                            Console.WriteLine($"NEW MINIMUM {fval} ON {new Vectors(indexes)} AND vals {new Vectors(i1, i2, i3, i4, i5, i6, i7, i8, i9, i10)}");
                                                            first = fval;
                                                            copy[indexes[0]] = i1;
                                                            copy[indexes[1]] = i2;
                                                            copy[indexes[2]] = i3;
                                                            copy[indexes[3]] = i4;
                                                            copy[indexes[4]] = i5;
                                                            copy[indexes[5]] = i6;
                                                            copy[indexes[6]] = i7;
                                                            copy[indexes[7]] = i8;
                                                            copy[indexes[8]] = i9;
                                                            copy[indexes[9]] = i10;
                                                            return (first, copy);
                                                        }
                                                    }


                                                    pr -= Costs[9][i10 - 1];
                                                    acr[i10 - 1] -= peop[9];
                                                }

                                                pr -= Costs[8][i9 - 1];
                                                acr[i9 - 1] -= peop[8];
                                            }

                                            pr -= Costs[7][i8 - 1];
                                            acr[i8 - 1] -= peop[7];
                                        }

                                        pr -= Costs[6][i7 - 1];
                                        acr[i7 - 1] -= peop[6];
                                    }

                                    pr -= Costs[5][i6 - 1];
                                    acr[i6 - 1] -= peop[5];
                                }

                                pr -= Costs[4][i5 - 1];
                                acr[i5 - 1] -= peop[4];
                            }

                            pr -= Costs[3][i4 - 1];
                            acr[i4 - 1] -= peop[3];
                        }

                        pr -= Costs[2][i3 - 1];
                        acr[i3 - 1] -= peop[2];
                    }

                    pr -= Costs[1][i2 - 1];
                    acr[i2 - 1] -= peop[1];
                }

                pr -= Costs[0][i1 - 1];
                acr[i1 - 1] -= peop[0];
            }


            // Console.WriteLine($"{indexes[0]} {indexes[1]} {indexes[2]} {indexes[3]} {indexes[4]}");

            return (first, copy);
        }
        static (double, byte[]) MinByRandomizeBy10(byte[] arr, int[] indexes, int top = 5)
        {
            const int dim = 10;

            var peop = indexes.Select(i => n_people[i]).ToArray();
            var copy = arr.Dup();
            int pr = preference_costsMemoized(copy);
            var acr = GetMap(copy);
            var fval = pr + accounting_penalty3(acr);
            double first = fval;
            byte[][] TP = new byte[dim][];
            int[][] Costs = new int[dim][];
            byte[] vals = new byte[dim];

            for (int i = 0; i < dim; i++)
            {
                vals[i] = (byte)(copy[indexes[i]] - 1);
                Costs[i] = prCosts[indexes[i]];
                pr -= Costs[i][vals[i]];
                acr[vals[i]] -= peop[i];
                TP[i] = Top(indexes[i], top);
            }

            bool bad(short val) => val < 125 || val > 300;
            bool gut(params int[] I)
            {
                for (int i = 0; i < dim; i++)
                    if (bad(acr[vals[i]]) || bad(acr[I[i] - 1]))
                        return false;
                return true;
            }

            foreach (var i1 in TP[0])
            {
                pr += Costs[0][i1 - 1];
                acr[i1 - 1] += peop[0];

                foreach (var i2 in TP[1])
                {
                    pr += Costs[1][i2 - 1];
                    acr[i2 - 1] += peop[1];

                    foreach (var i3 in TP[2])
                    {
                        pr += Costs[2][i3 - 1];
                        acr[i3 - 1] += peop[2];

                        foreach (var i4 in TP[3])
                        {
                            pr += Costs[3][i4 - 1];
                            acr[i4 - 1] += peop[3];

                            foreach (var i5 in TP[4])
                            {
                                pr += Costs[4][i5 - 1];
                                acr[i5 - 1] += peop[4];

                                foreach (var i6 in TP[5])
                                {
                                    pr += Costs[5][i6 - 1];
                                    acr[i6 - 1] += peop[5];

                                    foreach (var i7 in TP[6])
                                    {
                                        pr += Costs[6][i7 - 1];
                                        acr[i7 - 1] += peop[6];

                                        foreach (var i8 in TP[7])
                                        {
                                            pr += Costs[7][i8 - 1];
                                            acr[i8 - 1] += peop[7];

                                            foreach (var i9 in TP[8])
                                            {
                                                pr += Costs[8][i9 - 1];
                                                acr[i9 - 1] += peop[8];

                                                foreach (var i10 in TP[9])
                                                {
                                                    pr += Costs[9][i10 - 1];
                                                    acr[i10 - 1] += peop[9];

                                                    if (pr < first && gut(i1, i2, i3, i4, i5, i6, i7, i8, i9, i10))
                                                    {
                                                        fval = pr + accounting_penalty3(acr);
                                                        if (fval < first)
                                                        {
                                                            Console.WriteLine($"NEW MINIMUM {fval} ON {new Vectors(indexes)} AND vals {new Vectors(i1, i2, i3, i4, i5, i6, i7, i8, i9, i10)}");
                                                            first = fval;
                                                            copy[indexes[0]] = i1;
                                                            copy[indexes[1]] = i2;
                                                            copy[indexes[2]] = i3;
                                                            copy[indexes[3]] = i4;
                                                            copy[indexes[4]] = i5;
                                                            copy[indexes[5]] = i6;
                                                            copy[indexes[6]] = i7;
                                                            copy[indexes[7]] = i8;
                                                            copy[indexes[8]] = i9;
                                                            copy[indexes[9]] = i10;
                                                            return (first, copy);
                                                        }
                                                    }


                                                    pr -= Costs[9][i10 - 1];
                                                    acr[i10 - 1] -= peop[9];
                                                }

                                                pr -= Costs[8][i9 - 1];
                                                acr[i9 - 1] -= peop[8];
                                            }

                                            pr -= Costs[7][i8 - 1];
                                            acr[i8 - 1] -= peop[7];
                                        }

                                        pr -= Costs[6][i7 - 1];
                                        acr[i7 - 1] -= peop[6];
                                    }

                                    pr -= Costs[5][i6 - 1];
                                    acr[i6 - 1] -= peop[5];
                                }

                                pr -= Costs[4][i5 - 1];
                                acr[i5 - 1] -= peop[4];
                            }

                            pr -= Costs[3][i4 - 1];
                            acr[i4 - 1] -= peop[3];
                        }

                        pr -= Costs[2][i3 - 1];
                        acr[i3 - 1] -= peop[2];
                    }

                    pr -= Costs[1][i2 - 1];
                    acr[i2 - 1] -= peop[1];
                }

                pr -= Costs[0][i1 - 1];
                acr[i1 - 1] -= peop[0];
            }


            // Console.WriteLine($"{indexes[0]} {indexes[1]} {indexes[2]} {indexes[3]} {indexes[4]}");

            return (first, copy);
        }

        static void MinRandomSeries(int count = 1000, int top = 5)
        {
            //var was = Enumerable.Range(0, count).AsParallel().Select(i => MinByRandomizeBy20(top)).ToArray();

            var was = new (double, byte[])[count];
            Parallel.For(0, count, (int i) => was[i] = MinByRandomizeBy10(top));

            var min = was.Min(t => t.Item1);
            var el = was.First(s => s.Item1 == min);
            res = el.Item2;
            WriteData(min);
        }
        static void MinRandomSeries2(int top = 5)
        {
            //var was = Enumerable.Range(0, count).AsParallel().Select(i => MinByRandomizeBy20(top)).ToArray();

            var was = new (double, byte[])[4991];
            Parallel.For(0, 4991, (int i) => was[i] = MinByRandomizeBy10(new int[] { i, i + 1, i + 2, i + 3, i + 4, i + 5, i + 6, i + 7, i + 8, i + 9 }, top));

            var min = was.Min(t => t.Item1);
            var el = was.First(s => s.Item1 == min);
            res = el.Item2;
            WriteData(min);
        }

        static (double res, byte val) MinByOne(int ind)
        {
            double resultbest = 1e20f, result = 1e20f;
            int index = 0;

            int pr = preference_costsMemoized(res);
            var acr = GetMap();

            pr -= prCosts[ind][res[ind] - 1];
            acr[res[ind] - 1] -= n_people[ind];

            for (int i = 1; i <= 100; i++)
            {
                int pr2 = pr;
                var acr2 = acr.Dup();

                pr2 += prCosts[ind][i - 1];
                acr2[i - 1] += n_people[ind];

                if (!(pr2 >= best || acr2.Any(s => s < 125 || s > 300)))
                {
                    result = accounting_penalty2(acr2) + pr2;
                    if (result < resultbest)
                    {
                        index = i;
                        resultbest = result;
                    }
                }
            }

            return (res: resultbest, val: (byte)index);
        }
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double res, byte val) MinByOne(int ind, byte[] sample_res, int pr, short[] acr)
        {
            double result_best = 1e20f, result;
            int index = 1;
            byte np = n_people[ind], nd = (byte)(sample_res[ind] - 1);

            pr -= prCosts[ind][nd];
            acr[nd] -= np;

            for (int i = 1; i <= 100; i++)
            {
                pr += prCosts[ind][i - 1];
                acr[i - 1] += np;

                if (pr < best && acr[nd] >= 125 && acr[i - 1] >= 125 && acr[i - 1] <= 300/*acr.All(s => s >= 125&& s <= 300)*/)
                {
                    result = accounting_penalty2(acr) + pr;
                    if (result < result_best)
                    {
                        index = i;
                        result_best = result;
                    }
                }

                pr -= prCosts[ind][i - 1];
                acr[i - 1] -= np;
            }
            acr[nd] += np;

            return (res: result_best, val: (byte)index);
        }

        static (double res, byte val) MinByOneChoise(int ind, byte[] sample_res, int pr, short[] acr, int top = 8)
        {
            double result_best = 1e20f, result;
            byte index = 1;
            byte np = n_people[ind], nd = (byte)(sample_res[ind] - 1);

            pr -= prCosts[ind][nd];
            acr[nd] -= np;

            foreach (byte i in Top(ind, top))
            //for (int i = 1; i <= 100; i++)
            {
                pr += prCosts[ind][i - 1];
                acr[i - 1] += np;

                if (pr < best && acr[nd] >= 125 && acr[i - 1] >= 125 && acr[i - 1] <= 300/*acr.All(s => s >= 125&& s <= 300)*/)
                {
                    result = accounting_penalty2(acr) + pr;
                    if (result < result_best)
                    {
                        index = i;
                        result_best = result;
                    }
                }

                pr -= prCosts[ind][i - 1];
                acr[i - 1] -= np;
            }
            acr[nd] += np;

            return (res: result_best, val: index);
        }
        static (double res, byte val1, byte val2) MinByOneChoise2(int ind, byte[] sample_res, int pr, short[] acr, int top = 8)
        {
            double result_best = 1e20f, result;
            int ind2 = ind + 1;
            byte index = 1, index2 = 2;
            byte np = n_people[ind], nd = (byte)(sample_res[ind] - 1), np2 = n_people[ind2], nd2 = (byte)(sample_res[ind2] - 1);

            pr -= prCosts[ind][nd];
            acr[nd] -= np;
            pr -= prCosts[ind2][nd2];
            acr[nd2] -= np2;

            var t2 = Top(ind2, top);
            bool good(short v) => v >= 125 && v <= 300;

            foreach (byte i in Top(ind, top))
            //for (int i = 1; i <= 100; i++)
            {
                pr += prCosts[ind][i - 1];
                acr[i - 1] += np;

                foreach (byte j in t2)
                {
                    pr += prCosts[ind2][j - 1];
                    acr[j - 1] += np2;
                    if (pr < best && acr[nd] >= 125 && acr[nd2] > 124 && good(acr[i - 1]) && good(acr[j - 1]))
                    {
                        result = accounting_penalty2(acr) + pr;
                        if (result < result_best)
                        {
                            index = i;
                            index2 = j;
                            result_best = result;
                        }
                    }
                    pr -= prCosts[ind2][j - 1];
                    acr[j - 1] -= np2;
                }


                pr -= prCosts[ind][i - 1];
                acr[i - 1] -= np;
            }
            acr[nd] += np;
            acr[nd2] += np2;
            return (res: result_best, val1: index, val2: index2);
        }

        static (double, byte[]) MinByTwoChoise(byte[] sample_res, int pr, short[] acr, int top = 8)
        {
            double first = scoreMemoized2(sample_res), min;
            (int, int) inds = (0, 0);
            (byte, byte) rs = (0, 0);
            byte[] a1, a2;
            byte ni, nj;
            bool gut(short val) => val >= 125 && val <= 300;

        algol:
            //progress = false;
            for (int i = 0; i < 4999; i++)
            {
                ni = n_people[i];
                pr -= prCosts[i][sample_res[i] - 1];
                acr[sample_res[i] - 1] -= ni;
                a1 = Top(i, top);
                //i.Show();

                for (int j = i + 1; j < 5000; j++)
                {
                    nj = n_people[j];
                    pr -= prCosts[j][sample_res[j] - 1];
                    acr[sample_res[j] - 1] -= nj;
                    a2 = Top(j, top);


                    (double, (byte, byte))[] tops = new (double, (byte, byte))[top];

                    for (int ii = 0; ii < top; ii++)
                    //Parallel.For(0, top, ii =>
                    {
                        int pr2 = pr + prCosts[i][a1[ii] - 1];
                        //var acr2 = acr.Dup();
                        double f = first, tmp = 1e20f;
                        acr[a1[ii] - 1] += ni;

                        foreach (var i2 in a2)
                        {
                            pr2 += prCosts[j][i2 - 1];
                            acr[i2 - 1] += nj;

                            if (pr2 < f && gut(acr[sample_res[i] - 1]) && gut(acr[sample_res[j] - 1]) && gut(acr[a1[ii] - 1]) && gut(acr[i2 - 1]))
                            {
                                tmp = pr2 + accounting_penalty3(acr);
                                if (tmp < f)
                                {
                                    f = tmp;
                                    tops[ii] = (f, (a1[ii], i2));
                                }
                            }

                            pr2 -= prCosts[j][i2 - 1];
                            acr[i2 - 1] -= nj;
                        }
                        acr[a1[ii] - 1] -= ni;
                    }//);

                    var ps = tops.Where(s => s.Item1 > 0).ToArray();
                    if (ps.Length == 0)
                        min = 1e20f;
                    else
                        min = ps.Min(s => s.Item1);

                    if (min < first)
                    {
                        first = min;
                        inds = (i, j);
                        rs = tops.First(s => s.Item1 == min).Item2;
                        //Console.WriteLine($"New best val is {first}");

                        //sample_res[inds.Item1] = rs.Item1;

                        sample_res[inds.Item1] = rs.Item1;
                        sample_res[inds.Item2] = rs.Item2;
                        //WriteData(first);

                        pr += prCosts[j][sample_res[j] - 1];
                        acr[sample_res[j] - 1] += nj;
                        pr += prCosts[i][sample_res[i] - 1];
                        acr[sample_res[i] - 1] += ni;
                        goto algol;
                    }

                    pr += prCosts[j][sample_res[j] - 1];
                    acr[sample_res[j] - 1] += nj;
                }

                pr += prCosts[i][sample_res[i] - 1];
                acr[sample_res[i] - 1] += ni;
            }
            Console.WriteLine($"New result {first}");
            return (first, sample_res);
        }

        static (double, byte[]) MinByTwoChoise(byte[] sample_res, int[] dx, int pr, short[] acr, int top = 6)
        {
            double first = scoreMemoized2(sample_res), min;
            (int, int) inds = (0, 0);
            (byte, byte) rs = (0, 0);
            byte[] a1, a2;
            byte ni, nj;
            bool gut(short val) => val >= 125 && val <= 300;
            int howmuch = -1;

        algol:
            howmuch++;
            (first, sample_res) = LineDown(sample_res);
            (first, sample_res) = MakeCoordMinSlow(sample_res);

            if (howmuch == 20)
            {
                Console.WriteLine($"Limit!  New result {first} (count = {howmuch})");
                return (first, sample_res);
            }

            //progress = false;
            for (int i = 0; i < dx.Length - 1; i++)
            {
                ni = n_people[dx[i]];
                pr -= prCosts[dx[i]][sample_res[dx[i]] - 1];
                acr[sample_res[dx[i]] - 1] -= ni;
                a1 = Top(dx[i], top);
                //dx[i].Show();

                for (int j = i + 1; j < dx.Length; j++)
                {
                    nj = n_people[dx[j]];
                    pr -= prCosts[dx[j]][sample_res[dx[j]] - 1];
                    acr[sample_res[dx[j]] - 1] -= nj;
                    a2 = Top(dx[j], top);


                    (double, (byte, byte))[] tops = new (double, (byte, byte))[top];

                    for (int ii = 0; ii < top; ii++)
                    //Parallel.For(0, top, ii =>
                    {
                        int pr2 = pr + prCosts[dx[i]][a1[ii] - 1];
                        //var acr2 = acr.Dup();
                        double f = first, tmp = 1e20f;
                        acr[a1[ii] - 1] += ni;

                        foreach (var i2 in a2)
                        {
                            pr2 += prCosts[dx[j]][i2 - 1];
                            acr[i2 - 1] += nj;

                            if (pr2 < f && gut(acr[sample_res[dx[i]] - 1]) && gut(acr[sample_res[dx[j]] - 1]) && gut(acr[a1[ii] - 1]) && gut(acr[i2 - 1]))
                            {
                                tmp = pr2 + accounting_penalty3(acr);
                                if (tmp < f)
                                {
                                    f = tmp;
                                    tops[ii] = (f, (a1[ii], i2));
                                }
                            }

                            pr2 -= prCosts[dx[j]][i2 - 1];
                            acr[i2 - 1] -= nj;
                        }
                        acr[a1[ii] - 1] -= ni;
                    }//);

                    var ps = tops.Where(s => s.Item1 > 0).ToArray();
                    if (ps.Length == 0)
                        min = 1e20f;
                    else
                        min = ps.Min(s => s.Item1);

                    if (min < first)
                    {
                        first = min;
                        inds = (dx[i], dx[j]);
                        rs = tops.First(s => s.Item1 == min).Item2;
                        //Console.WriteLine($"New best val is {first}");

                        //sample_res[inds.Item1] = rs.Item1;

                        sample_res[inds.Item1] = rs.Item1;
                        sample_res[inds.Item2] = rs.Item2;
                        //WriteData(first);

                        pr += prCosts[dx[j]][sample_res[dx[j]] - 1];
                        acr[sample_res[dx[j]] - 1] += nj;
                        pr += prCosts[dx[i]][sample_res[dx[i]] - 1];
                        acr[sample_res[dx[i]] - 1] += ni;

                        if (first < best)
                            Console.WriteLine($"WoW! {first} < {best}");

                        goto algol;
                    }

                    pr += prCosts[dx[j]][sample_res[dx[j]] - 1];
                    acr[sample_res[dx[j]] - 1] += nj;
                }

                pr += prCosts[dx[i]][sample_res[dx[i]] - 1];
                acr[sample_res[dx[i]] - 1] += ni;
            }
            Console.WriteLine($"New result {first} (count = {howmuch})");
            return (first, sample_res);
        }
        static (double, byte[]) MinByTwoChoiseParallel(byte[] sample_res, int[] dx, int pr, short[] acr, int top = 6)
        {
            double first = scoreMemoized2(sample_res), min;
            (int, int) inds = (0, 0);
            (byte, byte) rs = (0, 0);
            byte[] a1, a2;
            byte ni, nj;
            bool gut(short val) => val >= 125 && val <= 300;
            int howmuch = -1;

        algol:
            howmuch++;
            (first, sample_res) = LineDown(sample_res);
            (first, sample_res) = MakeCoordMinSlow(sample_res);
            if (howmuch == 20)
            {
                Console.WriteLine($"Limit!  New result {first} (count = {howmuch})");
                return (first, sample_res);
            }

            //progress = false;
            for (int i = 0; i < dx.Length - 1; i++)
            {
                ni = n_people[dx[i]];
                pr -= prCosts[dx[i]][sample_res[dx[i]] - 1];
                acr[sample_res[dx[i]] - 1] -= ni;
                a1 = Top(dx[i], top);
                //dx[i].Show();

                for (int j = i + 1; j < dx.Length; j++)
                {
                    nj = n_people[dx[j]];
                    pr -= prCosts[dx[j]][sample_res[dx[j]] - 1];
                    acr[sample_res[dx[j]] - 1] -= nj;
                    a2 = Top(dx[j], top);


                    (double, (byte, byte))[] tops = new (double, (byte, byte))[top];

                    //for (int ii = 0; ii < top; ii++)
                    Parallel.For(0, top, ii =>
                    {
                        int pr2 = pr + prCosts[dx[i]][a1[ii] - 1];
                        var acr2 = acr.Dup();
                        double f = first, tmp = 1e20f;
                        acr2[a1[ii] - 1] += ni;

                        foreach (var i2 in a2)
                        {
                            pr2 += prCosts[dx[j]][i2 - 1];
                            acr2[i2 - 1] += nj;

                            if (pr2 < f && gut(acr2[sample_res[dx[i]] - 1]) && gut(acr2[sample_res[dx[j]] - 1]) && gut(acr2[a1[ii] - 1]) && gut(acr2[i2 - 1]))
                            {
                                tmp = pr2 + accounting_penalty3(acr2);
                                if (tmp < f)
                                {
                                    f = tmp;
                                    tops[ii] = (f, (a1[ii], i2));
                                }
                            }

                            pr2 -= prCosts[dx[j]][i2 - 1];
                            acr2[i2 - 1] -= nj;
                        }
                        acr2[a1[ii] - 1] -= ni;
                    });

                    var ps = tops.Where(s => s.Item1 > 0).ToArray();
                    if (ps.Length == 0)
                        min = 1e20f;
                    else
                        min = ps.Min(s => s.Item1);

                    if (min < first)
                    {
                        first = min;
                        inds = (dx[i], dx[j]);
                        rs = tops.First(s => s.Item1 == min).Item2;
                        //Console.WriteLine($"New best val is {first}");

                        //sample_res[inds.Item1] = rs.Item1;

                        sample_res[inds.Item1] = rs.Item1;
                        sample_res[inds.Item2] = rs.Item2;
                        //WriteData(first);

                        pr += prCosts[dx[j]][sample_res[dx[j]] - 1];
                        acr[sample_res[dx[j]] - 1] += nj;
                        pr += prCosts[dx[i]][sample_res[dx[i]] - 1];
                        acr[sample_res[dx[i]] - 1] += ni;

                        if (first < best)
                            Console.WriteLine($"WoW! {first} < {best}");

                        goto algol;
                    }

                    pr += prCosts[dx[j]][sample_res[dx[j]] - 1];
                    acr[sample_res[dx[j]] - 1] += nj;
                }

                pr += prCosts[dx[i]][sample_res[dx[i]] - 1];
                acr[sample_res[dx[i]] - 1] += ni;
            }
            Console.WriteLine($"New result {first} (count = {howmuch})");
            return (first, sample_res);
        }

        static (double res, byte val) MinByOnePr(int ind, byte[] sample_res, int pr, short[] acr)
        {
            double result_best = pr, result;
            int index = 1;
            int sr = sample_res[ind] - 1;
            byte nd = n_people[ind];

            pr -= prCosts[ind][sr];
            acr[sr] -= nd;

            for (int i = 1; i <= 100; i++)
            {
                pr += prCosts[ind][i - 1];
                acr[i - 1] += nd;

                if (acr[sr] >= 125 && acr[sr] <= 300 && acr[i - 1] >= 125 && acr[i - 1] <= 300)
                {
                    result = pr;
                    if (result < result_best)
                    {
                        index = i;
                        result_best = result;
                    }
                }

                pr -= prCosts[ind][i - 1];
                acr[i - 1] -= nd;
            }
            acr[sr] += nd;

            return (res: result_best, val: (byte)index);
        }
        static (double res, byte val) MinByOneAcc(int ind, byte[] sample_res, int pr, short[] acr)
        {
            double result_best = accounting_penalty3(acr), result;
            int index = 1;
            int sr = sample_res[ind] - 1;
            byte nd = n_people[ind];

            pr -= prCosts[ind][sr];
            acr[sr] -= nd;

            for (int i = 1; i <= 100; i++)
            {
                pr += prCosts[ind][i - 1];
                acr[i - 1] += nd;

                if (acr[sr] >= 125 && acr[sr] <= 300 && acr[i - 1] >= 125 && acr[i - 1] <= 300)
                {
                    result = accounting_penalty3(acr);
                    if (result < result_best)
                    {
                        index = i;
                        result_best = result;
                    }
                }

                pr -= prCosts[ind][i - 1];
                acr[i - 1] -= nd;
            }
            acr[sr] += nd;

            return (res: result_best, val: (byte)index);
        }

        static byte[] MinPre(byte[] ss, int deep = 20)
        {
            int pr = preference_costsMemoized(ss);
            var acr = GetMap(ss);
            double bst = pr;

            foreach (int ind in GetRandom().Take(deep))
            {
                var t = MinByOnePr(ind, ss, pr, acr);
                if (t.res < bst)
                {
                    bst = t.res;
                    ss[ind] = t.val;
                    pr = preference_costsMemoized(ss);
                    acr = GetMap(ss);
                }
            }
            return ss;
        }
        static byte[] MinAcc(byte[] ss, int deep = 20)
        {
            int pr = preference_costsMemoized(ss);
            var acr = GetMap(ss);
            double bst = accounting_penalty3(acr);

            foreach (int ind in GetRandom().Take(deep))
            {
                var t = MinByOneAcc(ind, ss, pr, acr);
                if (t.res < bst)
                {
                    bst = t.res;
                    ss[ind] = t.val;
                    pr = preference_costsMemoized(ss);
                    acr = GetMap(ss);
                }
            }
            return ss;
        }

        static bool MinByTwo(int ind1, int ind2, ref byte[][] mat, ref double[] results)
        {
            bool existprogress = false;
            double bst = scoreMemoized2(res), bsttmp;
            //byte[][] mat = GetNresCopy();
            //double[] results = new double[100];
            byte k1 = res[ind1], k2 = res[ind2], s1 = k1, s2 = k2;
            int n;

            for (int k = 0; k < 100; k++)
            {
                mat[k][ind2] = (byte)(k + 1);
            }

            for (byte i = 1; i <= 100; i++)
            {
                // if (i % 5 == 0) Console.WriteLine($"i_inner = {i}");

                for (int k = 0; k < 100; k++)
                {
                    mat[k][ind1] = i;
                }
                if (IndexOfArray(mat[0]) >= 3)
                    continue;

                results = mat.AsParallel().Select(arr => scoreMemoized2(arr)).ToArray();
                bsttmp = results.Min();
                n = Array.IndexOf(results, bsttmp) + 1;

                if (bst > bsttmp)
                {
                    bst = bsttmp;
                    existprogress = true;
                    s1 = i;
                    s2 = (byte)n;
                    Console.WriteLine($"best score = {Math.Round(bst, 3)}; (i,j)=({ind1},{ind2})  s1 = {s1}, s2 = {s2}");
                }
            }


            if (existprogress)
            {
                res[ind1] = s1;
                res[ind2] = s2;
                for (int k = 0; k < 100; k++)
                {
                    mat[k][ind1] = s1;
                    mat[k][ind2] = s2;
                }
            }
            else
                for (int k = 0; k < 100; k++)
                {
                    mat[k][ind1] = k1;
                    mat[k][ind2] = k2;
                }
            // else
            //  Console.WriteLine($"bad score = {bst} >= {scoreMemoized2(res)}");

            return existprogress;
        }

        static bool MinByThree(int i1, int i2, int i3, (byte, byte, byte)[] samples)
        {
            const int count = 100_00_00, dim = 3;

            bool existprogress = false;
            double bsttmp, copybest = scoreMemoized2(res);
            best = copybest;

            double[] results = new double[count];
            var copy = GetNresCopy(1)[0];

            int pr = preference_costsMemoized(res);
            var acr = GetMap();

            pr -= prCosts[i1][res[i1] - 1];
            acr[res[i1] - 1] -= n_people[i1];
            pr -= prCosts[i2][res[i2] - 1];
            acr[res[i2] - 1] -= n_people[i2];
            pr -= prCosts[i3][res[i3] - 1];
            acr[res[i3] - 1] -= n_people[i3];



            Parallel.For(0, count, (int i) =>
            //for(int i = 0; i < count; i++)
            {
                var sample = samples[i];
                int pr2 = pr;
                var acr2 = acr.Dup();

                pr2 += prCosts[i1][sample.Item1 - 1];
                acr2[sample.Item1 - 1] += n_people[i1];

                pr2 += prCosts[i2][sample.Item2 - 1];
                acr2[sample.Item2 - 1] += n_people[i2];

                pr2 += prCosts[i3][sample.Item3 - 1];
                acr2[sample.Item3 - 1] += n_people[i3];



                if (pr2 >= best || acr2.Any(s => s < 125 || s > 300))
                    results[i] = 1e20f;
                else
                    results[i] = accounting_penalty2(acr2) + pr2;
            }
            );

            bsttmp = results.Min();
            int n = Array.IndexOf(results, bsttmp);


            if (bsttmp < best)
            {
                existprogress = true;

                res[i1] = samples[n].Item1;
                res[i2] = samples[n].Item2;
                res[i3] = samples[n].Item3;


                Console.WriteLine($"Success. best score = {Math.Round(bsttmp, 3)} < {Math.Round(best, 3)}");
            }

            return existprogress;
        }

        static (double res, (int x, int y), (byte s1, byte s2)) MinByTwoForParallel(int i1, int i2)
        {
            const int count = 100_00;

            double resultbest = 1e20f, result = 1e20f;
            int index = 0;

            int pr = preference_costsMemoized(res);
            var acr = GetMap();

            pr -= prCosts[i1][res[i1] - 1];
            acr[res[i1] - 1] -= n_people[i1];
            pr -= prCosts[i2][res[i2] - 1];
            acr[res[i2] - 1] -= n_people[i2];

            for (int i = 0; i < count; i++)
            {
                var sample = samples2[i];
                int pr2 = pr;
                var acr2 = acr.Dup();

                pr2 += prCosts[i1][sample.Item1 - 1];
                acr2[sample.Item1 - 1] += n_people[i1];

                pr2 += prCosts[i2][sample.Item2 - 1];
                acr2[sample.Item2 - 1] += n_people[i2];

                if (!(pr2 >= best || acr2.Any(s => s < 125 || s > 300)))
                {
                    result = accounting_penalty2(acr2) + pr2;
                    if (result < resultbest)
                    {
                        index = i;
                        resultbest = result;
                    }
                }
            }

            return (res: resultbest, (x: i1, y: i2), samples2[index]);
        }
        static (double res, (int x, int y, int z), (byte s1, byte s2, byte s3)) MinByThreeForParallel(int i1, int i2, int i3, (byte, byte, byte)[] samples)
        {
            const int count = 100_00_00;

            double resultbest = 1e20f, result = 1e20f;
            int index = 0;

            int pr = preference_costsMemoized(res);
            var acr = GetMap();

            pr -= prCosts[i1][res[i1] - 1];
            acr[res[i1] - 1] -= n_people[i1];
            pr -= prCosts[i2][res[i2] - 1];
            acr[res[i2] - 1] -= n_people[i2];
            pr -= prCosts[i3][res[i3] - 1];
            acr[res[i3] - 1] -= n_people[i3];

            for (int i = 0; i < count; i++)
            {
                var sample = samples[i];
                int pr2 = pr;
                var acr2 = acr.Dup();

                pr2 += prCosts[i1][sample.Item1 - 1];
                acr2[sample.Item1 - 1] += n_people[i1];

                pr2 += prCosts[i2][sample.Item2 - 1];
                acr2[sample.Item2 - 1] += n_people[i2];

                pr2 += prCosts[i3][sample.Item3 - 1];
                acr2[sample.Item3 - 1] += n_people[i3];

                if (!(pr2 >= best || acr2.Any(s => s < 125 || s > 300)))
                {
                    result = accounting_penalty2(acr2) + pr2;
                    if (result < resultbest)
                    {
                        index = i;
                        resultbest = result;
                    }
                }
            }

            return (res: resultbest, (x: i1, y: i2, z: i3), samples[index]);
        }


        static byte[] GetResWithout(int i1, int i2)
        {
            byte[] t = new byte[4998];
            int i = 0;
            for (; i < i1; i++)
                t[i] = res[i];
            i = i1 + 1;
            for (; i < i2; i++)
                t[i - 1] = res[i];
            i = i2 + 1;
            for (; i < res.Length; i++)
                t[i - 2] = res[i];

            return t;
        }
        static byte[] GetPeopWithout(int i1, int i2)
        {
            byte[] t = new byte[4998];
            int i = 0;
            for (; i < i1; i++)
                t[i] = n_people[i];
            i = i1 + 1;
            for (; i < i2; i++)
                t[i - 1] = n_people[i];
            i = i2 + 1;
            for (; i < n_people.Length; i++)
                t[i - 2] = n_people[i];

            return t;
        }

        static int PrefCostsWithout(int i1, int i2)
        {
            int S = 0;
            int i = 0;
            for (; i < i1; i++)
                S += prCosts[i][res[i] - 1];
            i = i1 + 1;
            for (; i < i2; i++)
                S += prCosts[i][res[i] - 1];
            i = i2 + 1;
            for (; i < res.Length; i++)
                S += prCosts[i][res[i] - 1];
            return S;
        }

        /// <summary>
        /// Возвращает нужный для acc массив с количествами людей на день
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        static short[] GetMap(int i1, int i2)
        {
            short[] count = new short[100];

            int i = 0;
            for (; i < i1; i++)
                count[res[i] - 1] += n_people[i];
            i = i1 + 1;
            for (; i < i2; i++)
                count[res[i] - 1] += n_people[i];
            i = i2 + 1;
            for (; i < res.Length; i++)
                count[res[i] - 1] += n_people[i];
            return count;
        }
        static short[] GetMap()
        {
            short[] count = new short[100];

            int i = 0;
            for (; i < res.Length; i++)
                count[res[i] - 1] += n_people[i];
            return count;
        }
        static short[] GetMap(byte[] sres)
        {
            short[] count = new short[100];

            int i = 0;
            for (; i < sres.Length; i++)
                count[sres[i] - 1] += n_people[i];
            return count;
        }

        static bool MinByTwo2(int ind1, int ind2, ref int[] pres, ref short[][] accs, ref double[] results)
        {
            bool existprogress = false;
            double bst = scoreMemoized2(res), bsttmp;

            //int[] pres = new int[100];
            //short[][] accs = new short[100][];

            int pr = PrefCostsWithout(ind1, ind2);

            for (int i = 0; i < 100; i++)
            {
                pres[i] = pr + prCosts[ind2][i];
                accs[i] = GetMap(ind1, ind2);
                accs[i][i] += n_people[ind2];
            }

            if (accs[0].Count(s => s < 125 || s > 300) >= 4)
                return false;

            byte k1 = res[ind1], k2 = res[ind2], s1 = k1, s2 = k2;
            int n, tmppr;
            byte peop = n_people[ind1];

            for (byte i = 0; i < 100; i++)
            {
                tmppr = prCosts[ind1][i];

                for (int j = 0; j < 100; j++)
                {
                    accs[j][i] += peop;
                }

                results = accs.AsParallel().Select(arr => accounting_penalty2(arr)).ToArray();

                //Parallel.For(0, 100, (int j) => results[j] = pres[j] + tmppr);

                for (int j = 0; j < 100; j++)
                {
                    results[j] += pres[j] + tmppr;// + accounting_penalty2(accs[i]);
                }

                bsttmp = results.Min();
                n = Array.IndexOf(results, bsttmp) + 1;

                if (bst > bsttmp)
                {
                    bst = bsttmp;
                    existprogress = true;
                    s1 = (byte)(i + 1);
                    s2 = (byte)n;
                    Console.WriteLine($"best score = {Math.Round(bst, 3)}; (i,j)=({ind1},{ind2})  s1 = {s1}, s2 = {s2}");
                }


                for (int j = 0; j < 100; j++)
                {
                    accs[j][i] -= peop;
                }
            }


            if (existprogress)
            {
                res[ind1] = s1;
                res[ind2] = s2;
            }
            // else
            //  Console.WriteLine($"bad score = {bst} >= {scoreMemoized2(res)}");

            return existprogress;
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
                if (bst >= 1e20f)
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

        static int down_t, up_t;
        static byte[] peops1, peops2;
        static void ReadUpDown()
        {
            string[] s = Expendator.GetWordFromFile("границы.txt").Split(' ');
            down_t = Convert.ToInt32(s[0]); up_t = Convert.ToInt32(s[1]);

            Console.WriteLine($"down = {down_t}");
            Console.WriteLine($"up = {up_t}");

            var w = Expendator.GetStringArrayFromFile("peops.txt");
            peops1 = w[0].Split(' ').Select(p => Convert.ToByte(p)).ToArray();
            peops2 = w[1].Split(' ').Select(p => Convert.ToByte(p)).ToArray();
        }

        static void Replace()
        {
            //for (int i = 0; i < res.Length; i++)
            //    if (res[i] == 31)
            //        res[i] = 32;
            //else if(res[i] == 32)
            //        res[i] = 31;

            for (int i = 0; i < res.Length; i++)
            {
                if (res[i] == choice_0[i] && n_people[i] == 8)
                    res[i] = choice_1[i];
                if (wall(res))
                    res[i] = choice_0[i];
            }


            double tt = scoreMemoized2(res);
            tt.Show();
            (tt, res) = MakeCoordMinSlow(res);
            tt.Show();
            (tt, res) = MakeCoordMinSlow2(res, Enumerable.Range(0, 4999).ToArray());
            tt.Show();
        }

        static int[] Filter(int proc = 6)
        {
            //return Enumerable.Range(0, 5000).Where(c => res[c] >1 && res[c] <6).ToArray();

            //var map = GetMap(res);
            //var t = Enumerable.Range(0, 99).Where(c => Ntonumber[ map[c]-125][map[c+1]-125] >= 90 &&res[c]!=choice_0[c]&&res[c]!=choice_1[c]).Select(c => c + 1).ToArray();
            //return Enumerable.Range(0, 5000).Where(c => t.Contains(res[c])).ToArray();

            //var map = GetMap(res);
            //var t = Enumerable.Range(0, 99).Where(c => Ntonumber[map[c] - 125][map[c + 1] - 125] >= 100).Select(c => c + 1).ToArray();
            //var inds = Enumerable.Range(0, 5000).Where(c => t.Contains(res[c])).ToArray();

            //return inds.Select(i => (i, prCosts[i][res[i] - 1])).OrderByDescending(k => k.Item2).Select(p => p.Item1).Take(1000).ToArray();

            var ts = new int[5000 / proc];
            for (int i = 0; i < ts.Length; i++)
                ts[i] = randomgen.Next(0, 4999);

            var l = new List<int>();
            l.AddRange(ts);
            l.AddRange(Enumerable.Range(0, 5000).Where(c => res[c] == choice_3[c] || res[c] == choice_4[c] || res[c] == choice_2[c]));

            return l.Distinct().ToArray();


            //return Enumerable.Range(0, 5000).Where(c => res[c] != choice_0[c]).ToArray();

        }

        static void OtherTries()
        {
            BigRandomTopsTry(res, 50, 10000, 10000, 4);
            var map = DaysOfMaxPrice(GetMap(res), 100);
            var inds = IndexOfDays(res, r => map.Contains(r));
            TopDown3(/*Enumerable.Range(0,4999).Where(p=>n_people[p]==2).ToArray()*/inds, 5);

            TopDown3(Enumerable.Range(0, 4999).Where(p => n_people[p] < 4 || n_people[p] > 5).ToArray(), 5);
            MigratTest();
            Swap_3();
            Swap_6();
            WriteData(scoreMemoized2(res));
        }

        static void RunWithLogs()
        {
            void Log(string message)
            {
                File.AppendAllText("log.txt", message);
            }
            DateTime past;
            TimeSpan now;


            ReadRES();
            ReadUpDown();
            past = DateTime.Now;
            foreach (var p in new int[3])
                for (int count = 3; count <= 30; count += 5)
                    for (int top = 5; top >= 1; top--)
                    {
                        $"_____________________________count = {count} top = {top}".Show(); "".Show();
                        double b = best;
                        int q = 0;
                        while (q < 10)
                        {
                            RandomDown4(count, top, 500);
                            if (b == best)
                            {
                                q++;
                            }
                            else b = best;
                        }

                    }
            now = DateTime.Now - past;

            Log($"count top + -    time: {now}   res: {scoreMemoized2(res)}");



            ReadRES();
            ReadUpDown();
            past = DateTime.Now;
            foreach (var p in new int[2])
                for (int count = 28; count >= 5; count -= 5)
                    for (int top = 5; top >= 1; top--)
                    {
                        $"_____________________________count = {count} top = {top}".Show(); "".Show();
                        double b = best;
                        int q = 0;
                        while (q < 10)
                        {
                            RandomDown4(count, top, 500);
                            if (b == best)
                            {
                                q++;
                            }
                            else b = best;
                        }

                    }
            now = DateTime.Now - past;

            Log($"count top - -    time: {now}   res: {scoreMemoized2(res)}");



            ReadRES();
            ReadUpDown();
            past = DateTime.Now;
            foreach (var p in new int[2])
                for (int count = 28; count >= 5; count -= 5)
                    for (int top = 1; top <= 5; top++)
                    {
                        $"_____________________________count = {count} top = {top}".Show(); "".Show();
                        double b = best;
                        int q = 0;
                        while (q < 10)
                        {
                            RandomDown4(count, top, 500);
                            if (b == best)
                            {
                                q++;
                            }
                            else b = best;
                        }

                    }
            now = DateTime.Now - past;

            Log($"count top - +    time: {now}   res: {scoreMemoized2(res)}");



            ReadRES();
            ReadUpDown();
            past = DateTime.Now;
            foreach (var p in new int[2])
                for (int count = 5; count <= 28; count += 5)
                    for (int top = 1; top <= 5; top++)
                    {
                        $"_____________________________count = {count} top = {top}".Show(); "".Show();
                        double b = best;
                        int q = 0;
                        while (q < 10)
                        {
                            RandomDown4(count, top, 500);
                            if (b == best)
                            {
                                q++;
                            }
                            else b = best;
                        }

                    }
            now = DateTime.Now - past;

            Log($"count top + +    time: {now}   res: {scoreMemoized2(res)}");



            ReadRES();
            ReadUpDown();
            past = DateTime.Now;
            foreach (var p in new int[2])
                for (int top = 5; top >= 1; top--)
                    for (int count = 3; count <= 30; count += 5)
                    {
                        $"_____________________________count = {count} top = {top}".Show(); "".Show();
                        double b = best;
                        int q = 0;
                        while (q < 10)
                        {
                            RandomDown4(count, top, 500);
                            if (b == best)
                            {
                                q++;
                            }
                            else b = best;
                        }

                    }
            now = DateTime.Now - past;

            Log($"top count + -    time: {now}   res: {scoreMemoized2(res)}");



            ReadRES();
            ReadUpDown();
            past = DateTime.Now;
            foreach (var p in new int[2])
                for (int top = 5; top >= 1; top--)
                    for (int count = 28; count >= 5; count -= 5)
                    {
                        $"_____________________________count = {count} top = {top}".Show(); "".Show();
                        double b = best;
                        int q = 0;
                        while (q < 10)
                        {
                            RandomDown4(count, top, 500);
                            if (b == best)
                            {
                                q++;
                            }
                            else b = best;
                        }

                    }
            now = DateTime.Now - past;

            Log($"top count - -    time: {now}   res: {scoreMemoized2(res)}");



            ReadRES();
            ReadUpDown();
            past = DateTime.Now;
            foreach (var p in new int[2])
                for (int top = 1; top <= 5; top++)
                    for (int count = 28; count >= 5; count -= 5)
                    {
                        $"_____________________________count = {count} top = {top}".Show(); "".Show();
                        double b = best;
                        int q = 0;
                        while (q < 10)
                        {
                            RandomDown4(count, top, 500);
                            if (b == best)
                            {
                                q++;
                            }
                            else b = best;
                        }

                    }
            now = DateTime.Now - past;

            Log($"top count - +    time: {now}   res: {scoreMemoized2(res)}");



            ReadRES();
            ReadUpDown();
            past = DateTime.Now;
            foreach (var p in new int[2])
                for (int top = 1; top <= 5; top++)
                    for (int count = 5; count <= 28; count += 5)
                    {
                        $"_____________________________count = {count} top = {top}".Show(); "".Show();
                        double b = best;
                        int q = 0;
                        while (q < 10)
                        {
                            RandomDown4(count, top, 500);
                            if (b == best)
                            {
                                q++;
                            }
                            else b = best;
                        }

                    }
            now = DateTime.Now - past;

            Log($"top count + +    time: {now}   res: {scoreMemoized2(res)}");

        }

        static int GetIndexInChoise(int row, byte val) => Array.IndexOf(Tops[row], val);
        static void DayDown(int deep = 20)
        {
            bool gut(short v) => v >= 125 && v <= 300;

            var map = GetMap(res);
            var pr = GetProcArray();
            var days = Enumerable.Range(0, 100).Select(d => (d + 1, pr[d])).OrderByDescending(p => p.Item2).Select(p => p.Item1).ToArray();

            foreach (var day in days.Take(deep))
            {
                var peop = Enumerable.Range(0, 5000).Where(c => res[c] == day && choice_0[c] != day)
                    .Select(c => (c, n_people[c], GetIndexInChoise(c, res[c])))
                    .OrderByDescending(p => p.Item2).ThenByDescending(p => p.Item3).ToArray();
                if (peop.Length > 0)
                {
                    if (map[day - 1] > 150)
                    {
                        foreach (var p in peop)
                        {
                            var (ind, pp, chs) = p;

                            map[res[ind] - 1] -= pp;
                            int choice = 0;
                            bool result = false;
                            while (choice < chs)
                            {
                                map[Tops[ind][choice] - 1] += p.Item2;
                                if (gut(map[Tops[ind][choice] - 1]) && gut(map[res[ind] - 1]))
                                {
                                    result = true;
                                    break;
                                }

                                map[Tops[ind][choice] - 1] -= p.Item2;
                                choice++;
                            }
                            res[ind] = Tops[ind][choice];
                            map[res[ind] - 1] += pp;

                            if (result)
                                ShowStructure();
                        }
                    }
                    else
                    {

                    }
                }
            }

        }


        static void CycleDown()
        {
           // var map = GetMap(res);
           // var pr = GetProcArray();
            // var days = Enumerable.Range(0, 100).Select(d => (d + 1, pr[d])).OrderBy(h => h.Item2).Select(h => h.Item1).Take(25).ToArray();
            //var ines = Enumerable.Range(0, 5000).Where(c => res[c] == choice_3[c] || res[c] == choice_4[c] || /*days*/peops1.Contains(res[c])).ToArray();

            foreach (var p in new int[2])
                for (int count = 5; count <= 40; count += 5)
                    for (int top = 5; top >= 1; top--)
                    {
                        $"_____________________________count = {count} top = {top}".Show(); "".Show();
                        double b = best;
                        int q = 1;
                        // inds = GetSub2();
                        while (q <= 3)
                        {
                            // RandomDown8(ines,count, top, 1000);
                            // RandomDown4dot2(count, top, 1000);
                            //RandomDown12(count, top, 500);
                            // RandomDown3(count, 500);
                            RandomDown9(count, top, 1000*q);
                            //RandomDown4(count / 8, top, 500);
                            //RandomDown5(count / 4, top, 500);
                            // RandomDown7(count / 10, 500);
                            if (b == best)
                            {
                                q++;

                                //inds = GetSub2();
                            }
                            else b = best;
                        }

                    }
            return;
        }

        static void Main(string[] args)
        {
            ReadRES();
            ReadUpDown();
            ShowStructure();

            //for(int i = 1; i <= 100; i++)
            //{
            //    var p = new (double, byte[])[15];
            //    Parallel.For(0,15,(int k)=>//)
            //    //for (int k = 0; k < 15; k++)
            //    {
            //        var d = GetRandom().Where(c => res[c] == i).Take(10).ToArray();
            //        p[k] = MinByRandomizeBy10(res,d, 6);
            //    });
            //  res=  p.First(s => s.Item1 == p.Min(r => r.Item1)).Item2;
            //    WriteData(scoreMemoized2(res));

            //}

           // var inds = Range.Where(c => n_people[c] >6 && (res[c] == choice_3[c]|| res[c] == choice_4[c])).ToArray();
           //// var inds = Range.Where(c => n_people[c] > 6 && 1 == choice_0[c]).ToArray();
           // for (int i = 0; i < inds.Length; i++)
           //     res[inds[i]] = choice_2[inds[i]];
           // res = EchoDown(res);
            CycleDown();


            //for(int i=0;i<50;i++)
            //MinRandomSeries(1000, 2);
            //MinRandomSeries2( 5);

            //TopDown4(Range.Where(c=>res[c]==choice_4[c]||res[c]==choice_3[c]|| res[c] == choice_2[c]).ToArray(),5);
            //DayDown(12);double d;
            //best = scoreMemoized2(res);
            //(d, res) = MakeCoordMinSlow2Parallel(res, Enumerable.Range(0, 5000).ToArray());
            //WriteData(d);

            //int g = 0;
            //while(g++<100)
            //MinByRandomize2(100, 2000, 5);

            //for(byte i=1;i<100;i++)
            //Swap_6(i);

            // TopDown3(Enumerable.Range(0, 5000).Where(c => res[c] != choice_0[c]).ToArray(), 5);

            //RunWithLogs();
            //Bee2();
            //Bee(800);
            //MakeResult2(accounting_penaltyMemoized);
            //OtherTries();
            // Replace();

            //GetNotZeroChoises();

            //TopDown2(4093, 6);
            //TopDown2(5);
            // var p = MinByTwoChoise(res,preference_costsMemoized(res),GetMap(res));
            // int o=0;
            //TopDown3(5);
            //Swap();
            //Swap3();
            // Swap_2();

            //  Swap_4();
            //Swap_5();

            // Accord();
            //for (byte b = 0; b < 100;b++)
            //Swap_6(b);
            // MakeResult8();
            //NotRandomDown(3);
            //WriteContributions("conts.csv");
            // int[] inds;


            // var t = preference_costs(res);
            // var s = accounting_penalty(res);

            //ReadBegins();

            //for (int i = 0; i < res.Length; i++)
            //    if(res[i]>1&&res[i]<100)
            //    res[i] = (byte)(res[i]+randomgen.Next(-1, 1));
            //MakeResult5(scoreMemoized2);

            //MakeResult14("");
            //MakeResult9("", 30, 3_000_000, 10000);

            int ct = 1000, min = down_t, max = up_t;
            for (int i = 1; i <= 300; i++)
            {
                //Console.WriteLine($"Осталось итераций: {300 - i}");
                double b = best;
                //RandomDown2(max, ct, min: min);
                //RandomDown3(max, ct);
                RandomDown4(max, min, ct);
                if (b == best)
                {
                    //ct += 50;
                    //min++;
                    //max += 2;
                }
                else b = best;

                "".Show();

                //if (i % 25 == 0)
                //MakeResult11("", 500);
            }

            //ReadRES();
            if (false)
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

                    SuperMinimizingPreferenceCosts(30);
                    Console.WriteLine("Next step prf...");
                    //MakeCoordMinSlow(accounting_penalty);
                    //Console.WriteLine("Next step acc...");
                    //MakeCoordMinSlow(score);
                    // MakeResult2(score);
                    MakeResult5(score);
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

            //Bee();

            //MakeResult8("");
            //MakeResult10("", 5, 3, 2);
            // MakeResult11("",100000);
            //MakeResult12("",3,5);
            //MakeResult13("");



            for (int u = 0; u < 10; u++)
            {
                //MakeResult6("");

                //MakeResult2(scoreMemoized2);

                //RandomDown(550, down_t, up_t);
                //NotRandomDown();

                NotRandomDown(3);

                MakeResult7("");
            }

            System.Diagnostics.Process.Start("cmd", "/c shutdown -s -f -t 00");
        }

        static void RandomDown(int iter = 15, int down_t = 20, int up_t = 60)
        {
            double super = score(res);
            var res0 = res.Take(5000).ToArray();

            int i = 0, count;

            while (i < iter)
            {
                //SuperMinimizingPreferenceCosts(deep);
                //MakeCoordMinSlow(score);
                // Console.WriteLine("Next step...");
                MakeResult5(scoreMemoized2);

                best = scoreMemoized2(res);
                if (best >= super)
                {
                    res = res0.Take(5000).ToArray();
                    count = randomgen.Next(down_t, up_t);
                    Console.WriteLine($"Randomize... count = {count}... bad score {best} >= {super} ");
                    Randomize(count);
                    //i--;
                }
                else
                {
                    res0 = res.Take(5000).ToArray();
                    //i = 0;
                    super = best;
                }
                i++;
            }
        }

        /// <summary>
        /// Меняет dim случайных координат в сторону choice_0, затем градиентная покоординатная минимизация, всё это делается параллельно для iter векторов, в ответе получаем наилучший результат
        /// </summary>
        /// <param name="dim"></param>
        /// <param name="iter"></param>
        static void RandomDown2(int dim = 10, int iter = 10000, int min = 5)
        {
            var mas = new int[dim];
            var obs = GetNresCopy(iter);
            int index, j;

            var inds = GetNotZeroChoises();
            var icount = inds.Length;
            Console.WriteLine($"Not zero families count is {icount} ({Expendator.GetProcent(icount, 5000)}%)");

            for (int i = 0; i < iter; i++)
            {
            ss:
                for (j = 0; j < dim; j++)
                {
                    mas[j] = randomgen.Next(0, 4999/*icount*/);
                }
                if (mas.Distinct().Count() != dim /*|| mas.Count(tt => inds.Contains(tt)) < min*/)
                    goto ss;

                for (j = 0; j < dim; j++)
                {
                    index = mas[j];//inds[mas[j]];
                    //if (obs[i][index] == choice_0[index])
                    //{
                    //    if(index%2==0)
                    //        obs[i][index] = choice_1[index];
                    //    else
                    //        obs[i][index] = choice_2[index];
                    //}                        
                    //else
                    obs[i][index] = choice_0[index]; //RandVal(index) ;// choice_0[index];//LevelDown(index, obs[i][index]);//choice_0[index];//choice_0[index];//
                }
            }

            (double, byte[])[] links = new (double, byte[])[iter];
            //"Start".Show();
            Parallel.For(0, iter, (int i) => links[i] = MakeCoordMinSlow(obs[i]));
            //for (int i = 0; i < iter; i++) links[i] = MakeCoordMinSlow(obs[i]);

            var bs = links.Min(p => p.Item1);
            var t = links.First(p => p.Item1 == bs);
            if (best > bs)
            {
                res = t.Item2;
                best = bs;

                Console.WriteLine($"Записывается в файл {bs}");
                WriteData(bs, "");
            }
        }

        static void RandomDown3(int deep = 10, int iter = 10000)
        {
            var obs = GetNresCopy(iter);

            //Parallel.For(0, iter, (int i) => MakeCoordMinPr(obs[i], deep));
            Parallel.For(0, iter, (int i) => MinAcc(obs[i], deep));
            //Parallel.For(0, iter, (int i) => MinPre(obs[i], deep));

            MakeDown(obs);
        }

        /// <summary>
        /// Меняет в iter копиях случайные dim элементов на случайные choise от 0 до top-1
        /// </summary>
        /// <param name="dim"></param>
        /// <param name="top"></param>
        /// <param name="iter"></param>
        static void RandomDown4(int dim = 10, int top = 5, int iter = 10000)
        {
            var mas = new int[dim];
            var copy = new byte[dim];
            var obs = GetNresCopy(iter);
            int index, j;

            //var map =Days( GetMap(res), s => s >= 200);
            //var map = DaysOfMaxPrice(GetMap(res), 100);
            var inds = Enumerable.Range(0, 4999).Where(c => peops1.Contains(n_people[c])  /*<= 6 && n_people[c] >= 3*/).ToArray(); //IndexOfDays(res, r => r>1&&r<70/*map.Contains(r)*/); //GetNotZeroChoises();
                                                                                                                                   // var inds = Filter();
                                                                                                                                   //var inds = Enumerable.Range(0, 4999).Where(c => res[c]!=choice_5[c]&&res[c]!=choice_4[c]  /*<= 6 && n_people[c] >= 3*/).ToArray();

            var icount = inds.Length;
            //Console.WriteLine($"Not zero families count is {icount} ({Expendator.GetProcent(icount, 5000)}%)");

            //var inds = IndexOfFamilies(f => f <4);
            //var icount = inds.Length;

            //Parallel.For(0, iter, (int i) => 
            for (int i = 0; i < iter; i++)
            {
                ref var ob = ref obs[i];
                //ob = MinPreOrAcc(ob, dim);
                while (true)
                {
                    for (j = 0; j < dim; j++)
                    {
                        mas[j] = randomgen.Next(0, /*4999*/icount);
                        mas[j] = inds[mas[j]];
                    }
                    if (mas.Distinct().Count() != dim /*|| mas.Count(tt => inds.Contains(tt)) < min*/)
                        continue;

                    for (j = 0; j < dim; j++)
                    {
                        index = mas[j];
                        copy[j] = ob[index];
                        ob[index] = RandVal(index, top);
                        // if(n_people[index]>5)
                        //   ob[index] = RandVal(index, 3);
                        // else ob[index] = RandVal(index, top);//choice_0[index]; //RandVal(index) ;// choice_0[index];//LevelDown(index, obs[i][index]);//choice_0[index];//choice_0[index];//
                    }
                    if (GetMap(ob).Any(p => p < 125 || p > 300))
                    {
                        for (j = 0; j < dim; j++)
                            ob[mas[j]] = copy[j];
                    }
                    else break;
                }

                //WriteData(ob, @"C:\Users\крендель\Desktop\MagicCode\Машинное обучение\Santa's Workshop Tour 2019\samples");
                //links[i] = MakeCoordMinSlow(ob);
            }
            //);

            MakeDown(obs);
        }
        /// <summary>
        /// Меняет в iter копиях случайные dim элементов на случайные choise от 0 до top-1
        /// </summary>
        /// <param name="dim"></param>
        /// <param name="top"></param>
        /// <param name="iter"></param>
        static void RandomDown4dot2(int dim = 10, int top = 5, int iter = 10000)
        {
            var mas = new int[dim];
            //var copy = new byte[dim];
            var obs = GetNresCopy(iter);
            int index, j;

            var inds = Enumerable.Range(0, 4999).Where(c => peops1.Contains(n_people[c])  ).ToArray(); 
            var icount = inds.Length;

            Parallel.For(0, iter, (int i) => 
            //for (int i = 0; i < iter; i++)
            {
                ref var ob = ref obs[i];

                    for (j = 0; j < dim; j++)
                        mas[j] = inds[randomgen.Next(0, icount)];
                    
                    for (j = 0; j < dim; j++)
                    {
                        index = mas[j];
                    // copy[j] = ob[index];
                    ob[index] = LevelDown(index, ob[index]); //RandVal(index, top);
                    }
                ob = EchoDown(ob);

            }
            );

            MakeDown(obs);
        }

        static byte[] DaysOfMaxPrice(short[] map, double morethan = 100)
        {
            var res = new (double, byte)[99];
            for (int i = 0; i < 99; i++)
                res[i] = (Ntonumber[map[i] - 125][map[i + 1] - 125], (byte)(i + 1));

            return res.Where(r => r.Item1 > 100).Select(r => r.Item2).ToArray();

        }
        /// <summary>
        /// То же самое, что и 4, но в каждой копии случайно меняются одни и те же координаты
        /// </summary>
        /// <param name="dim"></param>
        /// <param name="top"></param>
        /// <param name="iter"></param>
        static void RandomDown5(int dim = 10, int top = 5, int iter = 10000)
        {
            var mas = new int[dim];
            var copy = new byte[dim];
            var obs = GetNresCopy(iter);
            int index, j;
            (double, byte[])[] links = new (double, byte[])[iter];

            //var inds = GetNotZeroChoises();
            //var icount = inds.Length;
            //Console.WriteLine($"Not zero families count is {icount} ({Expendator.GetProcent(icount, 5000)}%)");
            while (true)
            {
                for (j = 0; j < dim; j++)
                {
                    mas[j] = randomgen.Next(0, 4999/*icount*/);
                }
                if (mas.Distinct().Count() == dim /*|| mas.Count(tt => inds.Contains(tt)) < min*/)
                    break;
            }
            //new Vectors(mas).Show();

            //Parallel.For(0, iter, (int i) => 
            for (int i = 0; i < iter; i++)
            {
                ref var ob = ref obs[i];

                while (true)
                {
                    for (j = 0; j < dim; j++)
                    {
                        index = mas[j];
                        copy[j] = ob[index];
                        ob[index] = RandVal(index, top);//choice_0[index]; //RandVal(index) ;// choice_0[index];//LevelDown(index, obs[i][index]);//choice_0[index];//choice_0[index];//
                    }
                    if (GetMap(ob).Any(p => p < 125 || p > 300))
                    {
                        for (j = 0; j < dim; j++)
                            ob[mas[j]] = copy[j];
                    }
                    else break;
                }

                //links[i] = MakeCoordMinSlow(ob);
            }
            //);

            MakeDown(obs);
        }
        /// <summary>
        /// По копиям сначала делается градиентный спуск по первой функции
        /// </summary>
        /// <param name="dim"></param>
        /// <param name="top"></param>
        /// <param name="iter"></param>
        static void RandomDown6(int dim = 10, int iter = 10000)
        {
            var obs = GetNresCopy(iter);
            (double, byte[])[] links = new (double, byte[])[iter];

            Parallel.For(0, iter, (int i) => links[i] = MakeCoordMinSlowPr(obs[i], dim + 2 * i));
            obs = links.Select(u => u.Item2).ToArray();

            MakeDown(obs);
        }
        /// <summary>
        /// В копиях делается dim swaps
        /// </summary>
        /// <param name="dim"></param>
        /// <param name="iter"></param>
        static void RandomDown7(int dim = 10, int iter = 10000)
        {
            var mas = new int[dim];
            var copy = new byte[dim];
            var obs = GetNresCopy(iter);
            int index, j;
            (double, byte[])[] links = new (double, byte[])[iter];

            //Parallel.For(0, iter, (int i) => 
            for (int i = 0; i < iter; i++)
            {
                ref var ob = ref obs[i];

                while (true)
                {
                    ob.Swap(dim);
                    if (GetMap(ob).Any(p => p < 125 || p > 300))
                    {
                        ob = res.Dup();
                    }
                    else break;
                }
                // accounting_penaltyMemoized(ob).Show();
                //links[i] = MakeCoordMinSlow(ob);
            }
            //);

            "--->Start".Show();
            Parallel.For(0, iter, (int i) => links[i] = MakeCoordMinSlow(obs[i]));
            //for (int i = 0; i < iter; i++) links[i] = MakeCoordMinSlow(obs[i]);

            var bs = links.Min(p => p.Item1);
            var t = links.First(p => p.Item1 == bs);
            if (best > bs)
            {
                res = t.Item2;
                best = bs;

                Console.WriteLine($"Записывается в файл {bs}");
                WriteData(bs, "");
                ShowStructure();
            }
            else
            {
                Console.WriteLine($"-------> to best: {links.Count(p => p.Item1 == best)} ; to 1e20f: {links.Count(p => p.Item1 == 1e20f)}");
                "".Show();
            }
        }

        static void RandomDown8(int[] inds, int dim = 10, int top = 5, int iter = 10000)
        {
            var mas = new int[dim];
            var copy = new byte[dim];
            var obs = GetNresCopy(iter);
            int index, j;
            (double, byte[])[] links = new (double, byte[])[iter];

            var icount = inds.Length;

            //Parallel.For(0, iter, (int i) => 
            for (int i = 0; i < iter; i++)
            {
                ref var ob = ref obs[i];
                //ob = MinPreOrAcc(ob, dim);
                while (true)
                {
                    for (j = 0; j < dim; j++)
                    {
                        mas[j] = randomgen.Next(0, /*4999*/icount);
                        mas[j] = inds[mas[j]];
                    }
                    if (mas.Distinct().Count() != dim /*|| mas.Count(tt => inds.Contains(tt)) < min*/)
                        continue;

                    for (j = 0; j < dim; j++)
                    {
                        index = mas[j];
                        copy[j] = ob[index];
                        ob[index] = RandVal(index, top);
                        // if(n_people[index]>5)
                        //   ob[index] = RandVal(index, 3);
                        // else ob[index] = RandVal(index, top);//choice_0[index]; //RandVal(index) ;// choice_0[index];//LevelDown(index, obs[i][index]);//choice_0[index];//choice_0[index];//
                    }
                    if (GetMap(ob).Any(p => p < 125 || p > 300))
                    {
                        for (j = 0; j < dim; j++)
                            ob[mas[j]] = copy[j];
                    }
                    else break;
                }

                //WriteData(ob, @"C:\Users\крендель\Desktop\MagicCode\Машинное обучение\Santa's Workshop Tour 2019\samples");
                //links[i] = MakeCoordMinSlow(ob);
            }
            //);

            "--->Start".Show();
            Parallel.For(0, iter, (int i) => links[i] = MakeCoordMinSlow(obs[i]));
            //for (int i = 0; i < iter; i++) links[i] = MakeCoordMinSlow(obs[i]);
            "--->Start2".Show();
            Parallel.ForEach(Enumerable.Range(0, iter).Where(s => links[s].Item1 < best), (int i) => links[i] = MakeCoordMinSlow2(obs[i], Enumerable.Range(0, 4999).Where(c => peops2.Contains(n_people[c]) /*n_people[c] ==4*/).ToArray()));

            var bs = links.Min(p => p.Item1);
            var t = links.First(p => p.Item1 == bs);
            if (best > bs)
            {
                res = t.Item2;
                best = bs;

                Console.WriteLine($"Записывается в файл {bs}");
                WriteData(bs, "");
                ShowStructure();
            }
            else
            {
                Console.WriteLine($"-------> to best: {links.Count(p => p.Item1 == best)} ; to 1e20f: {links.Count(p => p.Item1 == 1e20f)}");
                "".Show();
            }
        }

        /// <summary>
        /// По сути то же самое, что и RandomDown4, но чуть иначе меняются копии
        /// </summary>
        /// <param name="dim"></param>
        /// <param name="top"></param>
        /// <param name="iter"></param>
        static void RandomDown9(int dim = 10, int top = 5, int iter = 10000)
        {
            var obs = GetSuperRandom(iter, dim, top);
            //var obs = GetStochacticRandom(iter, top, 3);
            MakeDown(obs);
        }

        static void RandomDown10(int dim = 10, int top = 5, int iter = 10000)
        {
            var obs = GetSuperRandom(iter, dim, top, false)/*.Select(s => MinPre(s, dim)).ToArray()*/;
            //var obs = GetStochacticRandom(iter, top, 3);
            MakeDown(obs);
        }

        static void RandomDown11(int dim = 10, int top = 5, int iter = 10000)
        {
            var obs = GetNresCopy(iter).Select(s => MinAcc(s, dim / 5)).Select(s => MinPre(s, dim)).ToArray();
            //var obs = GetStochacticRandom(iter, top, 3);
            MakeDown(obs);
        }

        static void RandomDown12(int dim = 10, int top = 5, int iter = 10000)
        {
            var range = Enumerable.Range(0, 5000);
            int[] st = range.Where(i => res[i] == choice_4[i]).ToArray();
            if (dim > 50)
                st = range.Where(i => res[i] == choice_4[i] || res[i] == choice_3[i]).ToArray();

            var obs = GetSuperRandom(st, iter, dim, top, false)/*.Select(s => MinPre(s, dim)).ToArray()*/;
            //var obs = GetStochacticRandom(iter, top, 3);
            MakeDown(obs);
        }

        static void MakeDown(byte[][] obs)
        {
            var iter = obs.GetLength(0);
            (double, byte[])[] links = new (double, byte[])[iter];

            "--->Start".Show();
            Parallel.For(0, iter, (int i) => links[i] = MakeCoordMinSlow(obs[i]));
            //for (int i = 0; i < iter; i++) links[i] = MakeCoordMinSlow(obs[i]);
            "--->Start2".Show();
            var en = Enumerable.Range(0, iter).Where(s => links[s].Item1 < best).ToArray();
            if (en.Length > 0)
            {
                if (en.Length == 1)
                    links[en[0]] = MakeCoordMinSlow2Parallel(obs[en[0]], Enumerable.Range(0, 4999).Where(c => peops2.Contains(n_people[c]) /*n_people[c] ==4*/).ToArray());
                else
                    Parallel.ForEach(en, (int i) => links[i] = MakeCoordMinSlow2(obs[i], Enumerable.Range(0, 4999).Where(c => peops2.Contains(n_people[c]) /*n_people[c] ==4*/).ToArray()));
            }

            var bs = links.Min(p => p.Item1);
            var t = links.First(p => p.Item1 == bs);
            if (best > bs)
            {
                res = t.Item2;
                best = bs;

                Console.WriteLine($"Записывается в файл {bs}");
                WriteData(bs, "");
                ShowStructure();
            }
            else
            {
                Console.WriteLine($"-------> to best: {links.Count(p => p.Item1 == best)} ; to 1e20f: {links.Count(p => p.Item1 == 1e20f)}");
                "".Show();
            }
        }


        /// <summary>
        /// Показывает разброс текущих данных по choises
        /// </summary>
        static void ShowStructure()
        {
            int c0, c1, c2, c3, c4, c5, c6, c7, c8, c9, no;
            c0 = c1 = c2 = c3 = c4 = c5 = c6 = c7 = c8 = c9 = no = 0;
            byte t;
            for (int i = 0; i < res.Length; i++)
            {
                t = res[i];
                if (t == choice_0[i])
                    c0++;
                else if (t == choice_1[i])
                    c1++;
                else if (t == choice_2[i])
                    c2++;
                else if (t == choice_3[i])
                    c3++;
                else if (t == choice_4[i])
                    c4++;
                else if (t == choice_5[i])
                    c5++;
                else if (t == choice_6[i])
                    c6++;
                else if (t == choice_7[i])
                    c7++;
                else if (t == choice_8[i])
                    c8++;
                else if (t == choice_9[i])
                    c9++;
                else no++;

            }

            if (c5 + c6 + c7 + c8 + c9 + no == 0)
                Console.WriteLine($"0: {c0}  1: {c1}  2: {c2}  3: {c3}  4: {c4}");
            else
                Console.WriteLine($"0: {c0}  1: {c1}  2: {c2}  3: {c3}  4: {c4}  5: {c5}  6: {c6}  7: {c7}  8: {c8}  9: {c9}  other: {no}");

            int pr = preference_costsMemoized(res);
            double acc = accounting_penaltyMemoized(res);
            Console.WriteLine($"pref = {pr}   acc = {acc}   sum = {pr + acc}");
            Console.WriteLine($"sub length = {GetSub2().Length}"); "".Show();
        }

        static int[] GetStructure(int top = 5)
        {
            int c0, c1, c2, c3, c4, c5, c6, c7, c8, c9, no;
            c0 = c1 = c2 = c3 = c4 = c5 = c6 = c7 = c8 = c9 = no = 0;
            byte t;
            for (int i = 0; i < res.Length; i++)
            {
                t = res[i];
                if (t == choice_0[i])
                    c0++;
                else if (t == choice_1[i])
                    c1++;
                else if (t == choice_2[i])
                    c2++;
                else if (t == choice_3[i])
                    c3++;
                else if (t == choice_4[i])
                    c4++;
                else if (t == choice_5[i])
                    c5++;
                else if (t == choice_6[i])
                    c6++;
                else if (t == choice_7[i])
                    c7++;
                else if (t == choice_8[i])
                    c8++;
                else if (t == choice_9[i])
                    c9++;
                else no++;

            }

            return new int[] { c0, c1, c2, c3, c4, c5, c6, c7, c8, c9, no }.Take(top).ToArray();
        }

        static void TopDown2(int top = 5)
        {
            int pr = preference_costsMemoized(res);
            short[] acr = GetMap();
            double first = scoreMemoized2(res), min;
            (int, int) inds = (0, 0);
            (byte, byte) rs = (0, 0);
            byte[] a1, a2;
            bool progress;
            byte ni, nj;

        algol:
            progress = false;
            for (int i = 0; i < 4999; i++)
            {
                ni = n_people[i];
                pr -= prCosts[i][res[i] - 1];
                acr[res[i] - 1] -= ni;
                a1 = Top(i, top);
                i.Show();

                for (int j = i + 1; j < 5000; j++)
                {
                    nj = n_people[j];
                    pr -= prCosts[j][res[j] - 1];
                    acr[res[j] - 1] -= nj;
                    a2 = Top(j, top);


                    (double, (byte, byte))[] tops = new (double, (byte, byte))[top];

                    Parallel.For(0, top, ii =>
                    {
                        int pr2 = pr + prCosts[i][a1[ii] - 1];
                        var acr2 = acr.Dup();
                        double f = first, tmp = 1e20f;
                        acr2[a1[ii] - 1] += ni;

                        foreach (var i2 in a2)
                        {
                            pr2 += prCosts[j][i2 - 1];
                            acr2[i2 - 1] += nj;

                            if (pr2 < f && acr2.All(s => s >= 125 && s <= 300))
                            {
                                tmp = pr2 + accounting_penalty3(acr2);
                                if (tmp < f)
                                {
                                    f = tmp;
                                    tops[ii] = (f, (a1[ii], i2));
                                }
                            }

                            pr2 -= prCosts[j][i2 - 1];
                            acr2[i2 - 1] -= nj;
                        }

                    });

                    var ps = tops.Where(s => s.Item1 > 0).ToArray();
                    if (ps.Length == 0)
                        min = 1e20f;
                    else
                        min = ps.Min(s => s.Item1);
                    if (min < first)
                    {
                        first = min;
                        inds = (i, j);
                        rs = tops.First(s => s.Item1 == min).Item2;
                        progress = true;
                        Console.WriteLine($"New best val is {first}");

                        //res[inds.Item1] = rs.Item1;

                        res[inds.Item1] = rs.Item1;
                        res[inds.Item2] = rs.Item2;


                        WriteData(first);

                        pr += prCosts[j][res[j] - 1];
                        acr[res[j] - 1] += nj;
                        pr += prCosts[i][res[i] - 1];
                        acr[res[i] - 1] += ni;
                        goto algol;
                    }

                    pr += prCosts[j][res[j] - 1];
                    acr[res[j] - 1] += nj;
                }

                pr += prCosts[i][res[i] - 1];
                acr[res[i] - 1] += ni;
            }

            //if (progress)
            //{
            //    res[inds.Item1] = rs.Item1;
            //    res[inds.Item2] = rs.Item2;
            //    res[inds.Item3] = rs.Item3;
            //    res[inds.Item4] = rs.Item4;
            //    res[inds.Item5] = rs.Item5;
            //}
        }
        static void TopDown2(int by, int top = 5)
        {
            int pr = preference_costsMemoized(res);
            short[] acr = GetMap();
            double first = scoreMemoized2(res), min;
            (int, int, int) inds = (0, 0, 0);
            (byte, byte, byte) rs = (0, 0, 0);
            byte[] a1, a2;
            bool progress;
            byte ni, nj, nb = n_people[by];
            var a0 = Top(by, top);

        algol:
            progress = false;
            for (int i = 0; i < 4999; i++)
            {
                ni = n_people[i];
                pr -= prCosts[i][res[i] - 1];
                acr[res[i] - 1] -= ni;
                a1 = Top(i, top);
                i.Show();

                for (int j = i + 1; j < 5000; j++)
                {
                    nj = n_people[j];
                    pr -= prCosts[j][res[j] - 1];
                    acr[res[j] - 1] -= nj;
                    a2 = Top(j, top);


                    (double, (byte, byte, byte))[] tops = new (double, (byte, byte, byte))[top];

                    Parallel.For(0, top, ii =>
                    {
                        int pr2 = pr + prCosts[i][a1[ii] - 1];
                        var acr2 = acr.Dup();
                        double f = first, tmp = 1e20f;
                        acr2[a1[ii] - 1] += ni;

                        foreach (var i0 in a0)
                        {
                            pr2 += prCosts[by][i0 - 1];
                            acr2[i0 - 1] += nb;
                            foreach (var i2 in a2)
                            {
                                pr2 += prCosts[j][i2 - 1];
                                acr2[i2 - 1] += nj;

                                if (pr2 < f && acr2.All(s => s >= 125 && s <= 300))
                                {
                                    tmp = pr2 + accounting_penalty3(acr2);
                                    if (tmp < f)
                                    {
                                        f = tmp;
                                        tops[ii] = (f, (i0, a1[ii], i2));
                                    }
                                }

                                pr2 -= prCosts[j][i2 - 1];
                                acr2[i2 - 1] -= nj;
                            }
                            pr2 -= prCosts[by][i0 - 1];
                            acr2[i0 - 1] -= nb;
                        }


                    });

                    var ps = tops.Where(s => s.Item1 > 0).ToArray();
                    if (ps.Length == 0)
                        min = 1e20f;
                    else
                        min = ps.Min(s => s.Item1);
                    if (min < first)
                    {
                        first = min;
                        inds = (by, i, j);
                        rs = tops.First(s => s.Item1 == min).Item2;
                        progress = true;
                        Console.WriteLine($"New best val is {first}");

                        //res[inds.Item1] = rs.Item1;

                        res[inds.Item1] = rs.Item1;
                        res[inds.Item2] = rs.Item2;
                        res[inds.Item3] = rs.Item3;

                        WriteData(first);

                        pr += prCosts[j][res[j] - 1];
                        acr[res[j] - 1] += nj;
                        pr += prCosts[i][res[i] - 1];
                        acr[res[i] - 1] += ni;
                        pr += prCosts[i][res[by] - 1];
                        acr[res[by] - 1] += nb;
                        goto algol;
                    }

                    pr += prCosts[j][res[j] - 1];
                    acr[res[j] - 1] += nj;
                }

                pr += prCosts[i][res[i] - 1];
                acr[res[i] - 1] += ni;
            }

            //if (progress)
            //{
            //    res[inds.Item1] = rs.Item1;
            //    res[inds.Item2] = rs.Item2;
            //    res[inds.Item3] = rs.Item3;
            //    res[inds.Item4] = rs.Item4;
            //    res[inds.Item5] = rs.Item5;
            //}
        }
        static void TopDown3(int top = 5)
        {
            int pr = preference_costsMemoized(res);
            short[] acr = GetMap();
            double first = scoreMemoized2(res), min;
            (int, int, int) inds = (0, 0, 0);
            (byte, byte, byte) rs = (0, 0, 0);
            byte[] a1, a2, a3;
            bool progress;
            byte ni, nj, nk;

        algol:
            progress = false;
            for (int i = 0; i < 4998; i++)
            {
                ni = n_people[i];
                pr -= prCosts[i][res[i] - 1];
                acr[res[i] - 1] -= ni;
                a1 = Top(i, top);
                i.Show();

                for (int j = i + 1; j < 4999; j++)
                {
                    nj = n_people[j];
                    pr -= prCosts[j][res[j] - 1];
                    acr[res[j] - 1] -= nj;
                    a2 = Top(j, top);
                    Console.WriteLine($"j = {j}");

                    for (int k = j + 1; k < 5000; k++)
                    {
                        nk = n_people[k];
                        pr -= prCosts[k][res[k] - 1];
                        acr[res[k] - 1] -= nk;
                        a3 = Top(k, top);

                        (double, (byte, byte, byte))[] tops = new (double, (byte, byte, byte))[top];

                        Parallel.For(0, top, ii =>
                        {
                            int pr2 = pr + prCosts[i][a1[ii] - 1];
                            var acr2 = acr.Dup();
                            double f = first, tmp = 1e20f;
                            acr2[a1[ii] - 1] += ni;


                            foreach (var i2 in a2)
                            {
                                pr2 += prCosts[j][i2 - 1];
                                acr2[i2 - 1] += nj;
                                foreach (var i3 in a3)
                                {
                                    pr2 += prCosts[k][i3 - 1];
                                    acr2[i3 - 1] += nk;


                                    if (pr2 < f && acr2.All(s => s >= 125 && s <= 300))
                                    {
                                        tmp = pr2 + accounting_penalty3(acr2);
                                        if (tmp < f)
                                        {
                                            f = tmp;
                                            tops[ii] = (f, (a1[ii], i2, i3));
                                        }
                                    }

                                    pr2 -= prCosts[k][i3 - 1];
                                    acr2[i3 - 1] -= nk;
                                }
                                pr2 -= prCosts[j][i2 - 1];
                                acr2[i2 - 1] -= nj;
                            }


                        });

                        var ps = tops.Where(s => s.Item1 > 0).ToArray();
                        if (ps.Length == 0)
                            min = 1e20f;
                        else
                            min = ps.Min(s => s.Item1);
                        if (min < first)
                        {
                            first = min;
                            inds = (i, j, k);
                            rs = tops.First(s => s.Item1 == min).Item2;
                            progress = true;
                            Console.WriteLine($"New best val is {first}");

                            //res[inds.Item1] = rs.Item1;

                            res[inds.Item1] = rs.Item1;
                            res[inds.Item2] = rs.Item2;
                            res[inds.Item3] = rs.Item3;


                            WriteData(first);

                            pr += prCosts[k][res[k] - 1];
                            acr[res[k] - 1] += nk;
                            pr += prCosts[j][res[j] - 1];
                            acr[res[j] - 1] += nj;
                            pr += prCosts[i][res[i] - 1];
                            acr[res[i] - 1] += ni;
                            goto algol;
                        }

                        pr += prCosts[k][res[k] - 1];
                        acr[res[k] - 1] += nk;
                    }
                    pr += prCosts[j][res[j] - 1];
                    acr[res[j] - 1] += nj;
                }

                pr += prCosts[i][res[i] - 1];
                acr[res[i] - 1] += ni;
            }

            //if (progress)
            //{
            //    res[inds.Item1] = rs.Item1;
            //    res[inds.Item2] = rs.Item2;
            //    res[inds.Item3] = rs.Item3;
            //    res[inds.Item4] = rs.Item4;
            //    res[inds.Item5] = rs.Item5;
            //}
        }
        static void TopDown3(int[] indexes, int top = 5)
        {
            int pr = preference_costsMemoized(res);
            short[] acr = GetMap();
            double first = scoreMemoized2(res), min;
            (int, int, int) inds = (0, 0, 0);
            (byte, byte, byte) rs = (0, 0, 0);
            byte[] a1, a2, a3;
            byte ni, nj, nk;

        algol:
            // progress = false;
            for (int i = 0; i < indexes.Length - 2; i++)
            {
                ni = n_people[indexes[i]];
                pr -= prCosts[indexes[i]][res[indexes[i]] - 1];
                acr[res[indexes[i]] - 1] -= ni;
                a1 = Top(indexes[i], top);
                i.Show();

                for (int j = i + 1; j < indexes.Length - 1; j++)
                {
                    nj = n_people[indexes[j]];
                    pr -= prCosts[indexes[j]][res[indexes[j]] - 1];
                    acr[res[indexes[j]] - 1] -= nj;
                    a2 = Top(indexes[j], top);
                    if (j % 15 == 0)
                        Console.WriteLine($"j = {j}");

                    for (int k = j + 1; k < indexes.Length; k++)
                    {
                        nk = n_people[indexes[k]];
                        pr -= prCosts[indexes[k]][res[indexes[k]] - 1];
                        acr[res[indexes[k]] - 1] -= nk;
                        a3 = Top(indexes[k], top);

                        (double, (byte, byte, byte))[] tops = new (double, (byte, byte, byte))[top];

                        Parallel.For(0, top, ii =>
                        {
                            int pr2 = pr + prCosts[indexes[i]][a1[ii] - 1];
                            var acr2 = acr.Dup();
                            double f = first, tmp = 1e20f;
                            acr2[a1[ii] - 1] += ni;


                            foreach (var i2 in a2)
                            {
                                pr2 += prCosts[indexes[j]][i2 - 1];
                                acr2[i2 - 1] += nj;
                                foreach (var i3 in a3)
                                {
                                    pr2 += prCosts[indexes[k]][i3 - 1];
                                    acr2[i3 - 1] += nk;


                                    if (pr2 < f && acr2.All(s => s >= 125 && s <= 300))
                                    {
                                        tmp = pr2 + accounting_penalty3(acr2);
                                        if (tmp < f)
                                        {
                                            f = tmp;
                                            tops[ii] = (f, (a1[ii], i2, i3));
                                        }
                                    }

                                    pr2 -= prCosts[indexes[k]][i3 - 1];
                                    acr2[i3 - 1] -= nk;
                                }
                                pr2 -= prCosts[indexes[j]][i2 - 1];
                                acr2[i2 - 1] -= nj;
                            }


                        });

                        var ps = tops.Where(s => s.Item1 > 0).ToArray();
                        if (ps.Length == 0)
                            min = 1e20f;
                        else
                            min = ps.Min(s => s.Item1);
                        if (min < first)
                        {
                            first = min;
                            inds = (indexes[i], indexes[j], indexes[k]);
                            rs = tops.First(s => s.Item1 == min).Item2;
                            //progress = true;
                            Console.WriteLine($"New best val is {first}");

                            //res[inds.Item1] = rs.Item1;

                            res[inds.Item1] = rs.Item1;
                            res[inds.Item2] = rs.Item2;
                            res[inds.Item3] = rs.Item3;


                            WriteData(first);

                            pr += prCosts[indexes[k]][res[indexes[k]] - 1];
                            acr[res[indexes[k]] - 1] += nk;
                            pr += prCosts[indexes[j]][res[indexes[j]] - 1];
                            acr[res[indexes[j]] - 1] += nj;
                            pr += prCosts[indexes[i]][res[indexes[i]] - 1];
                            acr[res[indexes[i]] - 1] += ni;
                            goto algol;
                        }

                        pr += prCosts[indexes[k]][res[indexes[k]] - 1];
                        acr[res[indexes[k]] - 1] += nk;
                    }
                    pr += prCosts[indexes[j]][res[indexes[j]] - 1];
                    acr[res[indexes[j]] - 1] += nj;
                }

                pr += prCosts[indexes[i]][res[indexes[i]] - 1];
                acr[res[indexes[i]] - 1] += ni;
            }

            //if (progress)
            //{
            //    res[inds.Item1] = rs.Item1;
            //    res[inds.Item2] = rs.Item2;
            //    res[inds.Item3] = rs.Item3;
            //    res[inds.Item4] = rs.Item4;
            //    res[inds.Item5] = rs.Item5;
            //}
        }
        static void TopDown5(int top = 5)
        {
            int pr = preference_costsMemoized(res);
            short[] acr = GetMap();
            double first = scoreMemoized2(res), min;
            (int, int, int, int, int) inds = (0, 0, 0, 0, 0);
            (byte, byte, byte, byte, byte) rs = (0, 0, 0, 0, 0);
            byte[] a1, a2, a3, a4, a5;
            bool progress;
            byte ni, nj, nk, np, nt;

        algol:
            progress = false;
            for (int i = 0; i < 4996; i++)
            {
                ni = n_people[i];
                pr -= prCosts[i][res[i] - 1];
                acr[res[i] - 1] -= ni;
                a1 = Top(i, top);
                i.Show();

                for (int j = i + 1; j < 4997; j++)
                {
                    nj = n_people[j];
                    pr -= prCosts[j][res[j] - 1];
                    acr[res[j] - 1] -= nj;
                    a2 = Top(j, top);
                    Console.WriteLine($"j = {j}");

                    for (int k = j + 1; k < 4998; k++)
                    {
                        nk = n_people[k];
                        pr -= prCosts[k][res[k] - 1];
                        acr[res[k] - 1] -= nk;
                        a3 = Top(k, top);
                        Console.WriteLine($"k = {k}");

                        for (int p = k + 1; p < 4999; p++)
                        {
                            np = n_people[p];
                            pr -= prCosts[p][res[p] - 1];
                            acr[res[p] - 1] -= np;
                            a4 = Top(p, top);
                            Console.WriteLine($"p = {p}");

                            for (int t = p + 1; t < 5000; t++)
                            {
                                nt = n_people[t];
                                pr -= prCosts[t][res[t] - 1];
                                acr[res[t] - 1] -= nt;
                                a5 = Top(t, top);

                                (double, (byte, byte, byte, byte, byte))[] tops = new (double, (byte, byte, byte, byte, byte))[top];

                                Parallel.For(0, top, ii =>
                                 {
                                     int pr2 = pr + prCosts[i][a1[ii] - 1];
                                     var acr2 = acr.Dup();
                                     double f = first, tmp = 1e20f;
                                     acr2[a1[ii] - 1] += ni;


                                     foreach (var i2 in a2)
                                     {
                                         pr2 += prCosts[j][i2 - 1];
                                         acr2[i2 - 1] += nj;
                                         foreach (var i3 in a3)
                                         {
                                             pr2 += prCosts[k][i3 - 1];
                                             acr2[i3 - 1] += nk;
                                             foreach (var i4 in a4)
                                             {
                                                 pr2 += prCosts[p][i4 - 1];
                                                 acr2[i4 - 1] += np;
                                                 foreach (var i5 in a5)
                                                 {
                                                     pr2 += prCosts[t][i5 - 1];
                                                     acr2[i5 - 1] += nt;

                                                     if (pr2 < f && acr2.All(s => s >= 125 && s <= 300))
                                                     {
                                                         tmp = pr2 + accounting_penalty3(acr2);
                                                         if (tmp < f)
                                                         {
                                                             f = tmp;
                                                             tops[ii] = (f, (a1[ii], i2, i3, i4, i5));
                                                         }
                                                     }

                                                     pr2 -= prCosts[t][i5 - 1];
                                                     acr2[i5 - 1] -= nt;
                                                 }
                                                 pr2 -= prCosts[p][i4 - 1];
                                                 acr2[i4 - 1] -= np;
                                             }
                                             pr2 -= prCosts[k][i3 - 1];
                                             acr2[i3 - 1] -= nk;
                                         }
                                         pr2 -= prCosts[j][i2 - 1];
                                         acr2[i2 - 1] -= nj;
                                     }


                                 });

                                var ps = tops.Where(s => s.Item1 > 0).ToArray();
                                if (ps.Length == 0)
                                    min = 1e20f;
                                else
                                    min = ps.Min(s => s.Item1);
                                if (min < first)
                                {
                                    first = min;
                                    inds = (i, j, k, p, t);
                                    rs = tops.First(s => s.Item1 == min).Item2;
                                    progress = true;
                                    Console.WriteLine($"New best val is {first}");

                                    //res[inds.Item1] = rs.Item1;

                                    res[inds.Item1] = rs.Item1;
                                    res[inds.Item2] = rs.Item2;
                                    res[inds.Item3] = rs.Item3;
                                    res[inds.Item4] = rs.Item4;
                                    res[inds.Item5] = rs.Item5;

                                    WriteData(first);
                                    pr += prCosts[t][res[t] - 1];
                                    acr[res[t] - 1] += nt;
                                    pr += prCosts[p][res[p] - 1];
                                    acr[res[p] - 1] += np;
                                    pr += prCosts[k][res[k] - 1];
                                    acr[res[k] - 1] += nk;
                                    pr += prCosts[j][res[j] - 1];
                                    acr[res[j] - 1] += nj;
                                    pr += prCosts[i][res[i] - 1];
                                    acr[res[i] - 1] += ni;
                                    goto algol;
                                }

                                pr += prCosts[t][res[t] - 1];
                                acr[res[t] - 1] += nt;
                            }
                            pr += prCosts[p][res[p] - 1];
                            acr[res[p] - 1] += np;
                        }
                        pr += prCosts[k][res[k] - 1];
                        acr[res[k] - 1] += nk;
                    }
                    pr += prCosts[j][res[j] - 1];
                    acr[res[j] - 1] += nj;
                }

                pr += prCosts[i][res[i] - 1];
                acr[res[i] - 1] += ni;
            }

            //if (progress)
            //{
            //    res[inds.Item1] = rs.Item1;
            //    res[inds.Item2] = rs.Item2;
            //    res[inds.Item3] = rs.Item3;
            //    res[inds.Item4] = rs.Item4;
            //    res[inds.Item5] = rs.Item5;
            //}
        }
        static void TopDown5(int[] indexes, int top = 5)
        {
            int pr = preference_costsMemoized(res);
            short[] acr = GetMap();
            double first = scoreMemoized2(res), min;
            (int, int, int, int, int) inds = (0, 0, 0, 0, 0);
            (byte, byte, byte, byte, byte) rs = (0, 0, 0, 0, 0);
            byte[] a1, a2, a3, a4, a5;
            bool progress;
            byte ni, nj, nk, np, nt;

        algol:
            progress = false;
            for (int i = 0; i < indexes.Length - 4; i++)
            {
                ni = n_people[indexes[i]];
                pr -= prCosts[indexes[i]][res[indexes[i]] - 1];
                acr[res[indexes[i]] - 1] -= ni;
                a1 = Top(indexes[i], top);
                indexes[i].Show();

                for (int j = i + 1; j < indexes.Length - 3; j++)
                {
                    nj = n_people[indexes[j]];
                    pr -= prCosts[indexes[j]][res[indexes[j]] - 1];
                    acr[res[indexes[j]] - 1] -= nj;
                    a2 = Top(indexes[j], top);
                    // Console.WriteLine($"j = {indexes[j]}");

                    for (int k = j + 1; k < indexes.Length - 2; k++)
                    {
                        nk = n_people[indexes[k]];
                        pr -= prCosts[indexes[k]][res[indexes[k]] - 1];
                        acr[res[indexes[k]] - 1] -= nk;
                        a3 = Top(indexes[k], top);
                        // Console.WriteLine($"k = {indexes[k]}");

                        for (int p = k + 1; p < indexes.Length - 1; p++)
                        {
                            np = n_people[indexes[p]];
                            pr -= prCosts[indexes[p]][res[indexes[p]] - 1];
                            acr[res[indexes[p]] - 1] -= np;
                            a4 = Top(indexes[p], top);
                            //Console.WriteLine($"p = {indexes[p]}");

                            for (int t = p + 1; t < indexes.Length; t++)
                            {
                                nt = n_people[indexes[t]];
                                pr -= prCosts[indexes[t]][res[indexes[t]] - 1];
                                acr[res[indexes[t]] - 1] -= nt;
                                a5 = Top(indexes[t], top);

                                (double, (byte, byte, byte, byte, byte))[] tops = new (double, (byte, byte, byte, byte, byte))[top];

                                Parallel.For(0, top, ii =>
                                {
                                    int pr2 = pr + prCosts[indexes[i]][a1[ii] - 1];
                                    var acr2 = acr.Dup();
                                    double f = first, tmp = 1e20f;
                                    acr2[a1[ii] - 1] += ni;


                                    foreach (var i2 in a2)
                                    {
                                        pr2 += prCosts[indexes[j]][i2 - 1];
                                        acr2[i2 - 1] += nj;
                                        foreach (var i3 in a3)
                                        {
                                            pr2 += prCosts[indexes[k]][i3 - 1];
                                            acr2[i3 - 1] += nk;
                                            foreach (var i4 in a4)
                                            {
                                                pr2 += prCosts[indexes[p]][i4 - 1];
                                                acr2[i4 - 1] += np;
                                                foreach (var i5 in a5)
                                                {
                                                    pr2 += prCosts[indexes[t]][i5 - 1];
                                                    acr2[i5 - 1] += nt;

                                                    if (pr2 < f && acr2.All(s => s >= 125 && s <= 300))
                                                    {
                                                        tmp = pr2 + accounting_penalty3(acr2);
                                                        if (tmp < f)
                                                        {
                                                            f = tmp;
                                                            tops[ii] = (f, (a1[ii], i2, i3, i4, i5));
                                                        }
                                                    }

                                                    pr2 -= prCosts[indexes[t]][i5 - 1];
                                                    acr2[i5 - 1] -= nt;
                                                }
                                                pr2 -= prCosts[indexes[p]][i4 - 1];
                                                acr2[i4 - 1] -= np;
                                            }
                                            pr2 -= prCosts[indexes[k]][i3 - 1];
                                            acr2[i3 - 1] -= nk;
                                        }
                                        pr2 -= prCosts[indexes[j]][i2 - 1];
                                        acr2[i2 - 1] -= nj;
                                    }


                                });

                                var ps = tops.Where(s => s.Item1 > 0).ToArray();
                                if (ps.Length == 0)
                                    min = 1e20f;
                                else
                                    min = ps.Min(s => s.Item1);
                                if (min < first)
                                {
                                    first = min;
                                    inds = (indexes[i], indexes[j], indexes[k], indexes[p], indexes[t]);
                                    rs = tops.First(s => s.Item1 == min).Item2;
                                    progress = true;
                                    Console.WriteLine($"New best val is {first}");

                                    //res[inds.Item1] = rs.Item1;

                                    res[inds.Item1] = rs.Item1;
                                    res[inds.Item2] = rs.Item2;
                                    res[inds.Item3] = rs.Item3;
                                    res[inds.Item4] = rs.Item4;
                                    res[inds.Item5] = rs.Item5;

                                    WriteData(first);
                                    pr += prCosts[indexes[t]][res[indexes[t]] - 1];
                                    acr[res[indexes[t]] - 1] += nt;
                                    pr += prCosts[indexes[p]][res[indexes[p]] - 1];
                                    acr[res[indexes[p]] - 1] += np;
                                    pr += prCosts[indexes[k]][res[indexes[k]] - 1];
                                    acr[res[indexes[k]] - 1] += nk;
                                    pr += prCosts[indexes[j]][res[indexes[j]] - 1];
                                    acr[res[indexes[j]] - 1] += nj;
                                    pr += prCosts[indexes[i]][res[indexes[i]] - 1];
                                    acr[res[indexes[i]] - 1] += ni;

                                    goto algol;
                                }

                                pr += prCosts[indexes[t]][res[indexes[t]] - 1];
                                acr[res[indexes[t]] - 1] += nt;
                            }
                            pr += prCosts[indexes[p]][res[indexes[p]] - 1];
                            acr[res[indexes[p]] - 1] += np;
                        }
                        pr += prCosts[indexes[k]][res[indexes[k]] - 1];
                        acr[res[indexes[k]] - 1] += nk;
                    }
                    pr += prCosts[indexes[j]][res[indexes[j]] - 1];
                    acr[res[indexes[j]] - 1] += nj;
                }

                pr += prCosts[indexes[i]][res[indexes[i]] - 1];
                acr[res[indexes[i]] - 1] += ni;
            }

            //if (progress)
            //{
            //    res[inds.Item1] = rs.Item1;
            //    res[inds.Item2] = rs.Item2;
            //    res[inds.Item3] = rs.Item3;
            //    res[inds.Item4] = rs.Item4;
            //    res[inds.Item5] = rs.Item5;
            //}
        }

        static void TopDown4(int[] indexes, int top = 5)
        {
            int pr = preference_costsMemoized(res);
            short[] acr = GetMap();
            double first = scoreMemoized2(res), min;
            (int, int, int, int) inds = (0, 0, 0, 0);
            (byte, byte, byte, byte) rs = (0, 0, 0, 0);
            byte[] a1, a2, a3, a4, a5;
            bool progress;
            byte ni, nj, nk, np, nt;

            bool gut(short v) => v >= 125 && v <= 300;

        algol:
            progress = false;
            for (int i = 0; i < indexes.Length - 3; i++)
            {
                ni = n_people[indexes[i]];
                pr -= prCosts[indexes[i]][res[indexes[i]] - 1];
                acr[res[indexes[i]] - 1] -= ni;
                a1 = Top(indexes[i], top);
                indexes[i].Show();

                for (int j = i + 1; j < indexes.Length - 2; j++)
                {
                    nj = n_people[indexes[j]];
                    pr -= prCosts[indexes[j]][res[indexes[j]] - 1];
                    acr[res[indexes[j]] - 1] -= nj;
                    a2 = Top(indexes[j], top);
                    // Console.WriteLine($"j = {indexes[j]}");

                    for (int k = j + 1; k < indexes.Length - 1; k++)
                    {
                        nk = n_people[indexes[k]];
                        pr -= prCosts[indexes[k]][res[indexes[k]] - 1];
                        acr[res[indexes[k]] - 1] -= nk;
                        a3 = Top(indexes[k], top);
                        // Console.WriteLine($"k = {indexes[k]}");

                        for (int p = k + 1; p < indexes.Length; p++)
                        {
                            np = n_people[indexes[p]];
                            pr -= prCosts[indexes[p]][res[indexes[p]] - 1];
                            acr[res[indexes[p]] - 1] -= np;
                            a4 = Top(indexes[p], top);
                            //Console.WriteLine($"p = {indexes[p]}");


                            (double, (byte, byte, byte, byte))[] tops = new (double, (byte, byte, byte, byte))[top];

                            Parallel.For(0, top, ii =>
                            {
                                int pr2 = pr + prCosts[indexes[i]][a1[ii] - 1];
                                var acr2 = acr.Dup();
                                double f = first, tmp = 1e20f;
                                acr2[a1[ii] - 1] += ni;


                                foreach (var i2 in a2)
                                {
                                    pr2 += prCosts[indexes[j]][i2 - 1];
                                    acr2[i2 - 1] += nj;
                                    foreach (var i3 in a3)
                                    {
                                        pr2 += prCosts[indexes[k]][i3 - 1];
                                        acr2[i3 - 1] += nk;
                                        foreach (var i4 in a4)
                                        {
                                            pr2 += prCosts[indexes[p]][i4 - 1];
                                            acr2[i4 - 1] += np;


                                            if (pr2 < f &&
                                            gut(acr2[i4 - 1]) &&
                                            gut(acr2[i3 - 1]) &&
                                            gut(acr2[i2 - 1]) &&
                                            gut(acr2[a1[ii] - 1]) &&
                                            gut(acr2[res[indexes[p]] - 1]) &&
                                            gut(acr2[res[indexes[k]] - 1]) &&
                                            gut(acr2[res[indexes[j]] - 1]) &&
                                            gut(acr2[res[indexes[i]] - 1]))
                                            {
                                                tmp = pr2 + accounting_penalty3(acr2);
                                                if (tmp < f)
                                                {
                                                    f = tmp;
                                                    tops[ii] = (f, (a1[ii], i2, i3, i4));
                                                }
                                            }

                                            pr2 -= prCosts[indexes[p]][i4 - 1];
                                            acr2[i4 - 1] -= np;
                                        }
                                        pr2 -= prCosts[indexes[k]][i3 - 1];
                                        acr2[i3 - 1] -= nk;
                                    }
                                    pr2 -= prCosts[indexes[j]][i2 - 1];
                                    acr2[i2 - 1] -= nj;
                                }


                            });

                            var ps = tops.Where(s => s.Item1 > 0).ToArray();
                            if (ps.Length == 0)
                                min = 1e20f;
                            else
                                min = ps.Min(s => s.Item1);
                            if (min < first)
                            {
                                first = min;
                                inds = (indexes[i], indexes[j], indexes[k], indexes[p]);
                                rs = tops.First(s => s.Item1 == min).Item2;
                                progress = true;
                                Console.WriteLine($"New best val is {first}");

                                //res[inds.Item1] = rs.Item1;

                                res[inds.Item1] = rs.Item1;
                                res[inds.Item2] = rs.Item2;
                                res[inds.Item3] = rs.Item3;
                                res[inds.Item4] = rs.Item4;

                                WriteData(first);

                                pr += prCosts[indexes[p]][res[indexes[p]] - 1];
                                acr[res[indexes[p]] - 1] += np;
                                pr += prCosts[indexes[k]][res[indexes[k]] - 1];
                                acr[res[indexes[k]] - 1] += nk;
                                pr += prCosts[indexes[j]][res[indexes[j]] - 1];
                                acr[res[indexes[j]] - 1] += nj;
                                pr += prCosts[indexes[i]][res[indexes[i]] - 1];
                                acr[res[indexes[i]] - 1] += ni;

                                goto algol;
                            }

                            pr += prCosts[indexes[p]][res[indexes[p]] - 1];
                            acr[res[indexes[p]] - 1] += np;
                        }
                        pr += prCosts[indexes[k]][res[indexes[k]] - 1];
                        acr[res[indexes[k]] - 1] += nk;
                    }
                    pr += prCosts[indexes[j]][res[indexes[j]] - 1];
                    acr[res[indexes[j]] - 1] += nj;
                }

                pr += prCosts[indexes[i]][res[indexes[i]] - 1];
                acr[res[indexes[i]] - 1] += ni;
            }

            //if (progress)
            //{
            //    res[inds.Item1] = rs.Item1;
            //    res[inds.Item2] = rs.Item2;
            //    res[inds.Item3] = rs.Item3;
            //    res[inds.Item4] = rs.Item4;
            //    res[inds.Item5] = rs.Item5;
            //}
        }

        static byte LevelDown(int index, byte val)
        {
            byte ch0 = choice_0[index], ch1 = choice_1[index], ch2 = choice_2[index], ch3 = choice_3[index], ch4 = choice_4[index], ch5 = choice_5[index], ch6 = choice_6[index], ch7 = choice_7[index], ch8 = choice_8[index], ch9 = choice_9[index];
            if (val == ch0)
                return ch0;
            if (val == ch1)
                return ch0;
            if (val == ch2)
                return ch1;
            if (val == ch3)
                return ch2;
            if (val == ch4)
                return ch3;
            if (val == ch5)
                return ch4;
            if (val == ch6)
                return ch5;
            if (val == ch7)
                return ch6;
            if (val == ch8)
                return ch7;
            if (val == ch9)
                return ch8;
            return ch9;

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
                while (i < deeps.Length - 1)
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
                }

            }
        }

        static byte[][] GetSuperRandom(int count, int take = 100, int top = 5, bool allpeople = true)
        {
            var gv = GetNresCopy(count);
            var Map = GetMap(res);
            bool gut(short v) => v >= 125 && v <= 300;
            Parallel.For(0, count, p =>
            {
                ref var ob = ref gv[p];
                var map = Map.Dup();

                var ar = GetRandom();
                if (!allpeople)
                    ar = ar.Where(c => peops1.Contains(n_people[c]) || res[c] == choice_4[c] || res[c] == choice_3[c] || res[c] == choice_2[c]).ToArray();

                foreach (var ind in ar.Take(take))
                {
                    var pi = ob[ind] - 1;
                    var peop = n_people[ind];
                    map[pi] -= peop;
                    var arr = Top(ind, peop > 5 ? Math.Min(3, top) : top).ToArray(); arr.Swap(top);
                    foreach (var vl in arr)
                    {
                        map[vl - 1] += peop;
                        if (gut(map[pi]) && gut(map[vl - 1]))
                        {
                            ob[ind] = vl;
                            break;
                        }
                        else
                            map[vl - 1] -= peop;
                    }
                }

            });

            return gv;
        }
        static byte[][] GetStochacticRandom(int count, int top = 5, int coef = 3)
        {
            var pr = GetStructure(top);

            var gv = GetNresCopy(count);
            var Map = GetMap(res);
            bool gut(short v) => v >= 125 && v <= 300;
            Parallel.For(0, count, p =>
            {
                ref var ob = ref gv[p];
                var map = Map.Dup();

                foreach (var ind in GetRandom())
                {
                    var pi = ob[ind] - 1;
                    var peop = n_people[ind];
                    map[pi] -= peop;

                    var tp = Top(ind, top).ToArray();
                    var d = Array.IndexOf(tp, ob[ind]);
                    var prob = pr.Dup();
                    prob[d] *= coef;
                    var vl = Expendator.GetRandomElementFromArrayWithProbabilities(tp, prob);

                    map[vl - 1] += peop;
                    if (gut(map[pi]) && gut(map[vl - 1]))
                    {
                        ob[ind] = vl;
                    }
                    else
                        map[vl - 1] -= peop;

                }

            });

            return gv;
        }
        static byte[][] GetSuperRandom(int[] minimum, int count, int take = 100, int top = 5, bool allpeople = true)
        {
            var gv = GetNresCopy(count);
            var Map = GetMap(res);
            bool gut(short v) => v >= 125 && v <= 300;
            Parallel.For(0, count, p =>
            {
                ref var ob = ref gv[p];
                var map = Map.Dup();

                var ar = GetRandom(minimum);
                if (!allpeople)
                    ar = ar.Where(c => peops1.Contains(n_people[c]) || res[c] == choice_4[c] || res[c] == choice_3[c]).ToArray();

                foreach (var ind in ar.Take(take))
                {
                    var pi = ob[ind] - 1;
                    var peop = n_people[ind];
                    map[pi] -= peop;
                    var arr = Top(ind, top).ToArray(); arr.Swap(top);
                    foreach (var vl in arr)
                    {
                        map[vl - 1] += peop;
                        if (gut(map[pi]) && gut(map[vl - 1]))
                        {
                            ob[ind] = vl;
                            break;
                        }
                        else
                            map[vl - 1] -= peop;
                    }
                }

            });

            return gv;
        }

        static byte[] ToByteArr(double[] arr)
        {
            byte[] g = new byte[arr.Length];
            double b;
            for (int i = 0; i < arr.Length; i++)
            {
                b = arr[i];
                if (b < 0) b = 0;
                else if (b > 4) b = 4;
                g[i] = Tops[i][(int)Math.Round(b)];
            }
            return g;
        }
        static double[] ToDouble(byte[] arr)
        {
            var k = new double[arr.Length];
            for (int i = 0; i < arr.Length; i++)
                k[i] = Array.IndexOf(Tops[i], arr[i]);
            return k;
        }
        static void Bee(int dim = 25)
        {
            const int ct = 400;
            var mas = new int[dim];
            var copy = new byte[dim];

            var obs = GetSuperRandom(ct, dim, 5);
            obs[0] = res;

            //var obs = GetNresCopy(ct);
            //int index, j;
            //var inds = Enumerable.Range(0, 5000).ToArray();
            //var icount = inds.Length;

            //for (int i = 1; i < ct; i++)
            //{
            //    ref var ob = ref obs[i];
            //    //ob = MinPreOrAcc(ob, dim);
            //    while (true)
            //    {
            //        for (j = 0; j < dim; j++)
            //        {
            //            mas[j] = randomgen.Next(0, /*4999*/icount);
            //            mas[j] = inds[mas[j]];
            //        }
            //        if (mas.Distinct().Count() != dim /*|| mas.Count(tt => inds.Contains(tt)) < min*/)
            //            continue;

            //        for (j = 0; j < dim; j++)
            //        {
            //            index = mas[j];
            //            copy[j] = ob[index];
            //            ob[index] = RandVal(index, 4);
            //        }
            //        if (GetMap(ob).Any(p => p < 125 || p > 300))
            //        {
            //            for (j = 0; j < dim; j++)
            //                ob[mas[j]] = copy[j];
            //        }
            //        else break;
            //    }
            //}

            BeeHiveAlgorithm.w = 0.3;
            BeeHiveAlgorithm.fp = 1;
            BeeHiveAlgorithm.fg = 5;

            var t = BeeHiveAlgorithm.GetGlobalMin(
                obs.Select(o => new Vectors(ToDouble(o))).ToArray(),
                (Vectors v) => scoreMemoized2(ToByteArr(v.DoubleMas)),
                n: 5000,
               min: 0, max: 4, eps: 1e-10, countpoints: ct, maxcountstep: 2000, maxiter: 50000);

            best = t.Item2 ;
            res = ToByteArr(t.Item1.DoubleMas);
        }
        static void Bee2()
        {
            const int ct = 400;

            BeeHiveAlgorithm.w = 0.3;
            BeeHiveAlgorithm.fp = 2;
            BeeHiveAlgorithm.fg = 5;

            var t = BeeHiveAlgorithm.GetGlobalMin(
                (Vectors v) => scoreMemoized2(ToByteArr(v.DoubleMas)),
                n: 5000,
               min: 0, max: 4, eps: 1e-10, countpoints: ct, maxcountstep: 2000, center: new Vectors(ToDouble(res)), maxiter: 50000);

            best = t.Item2 ;
            res = ToByteArr(t.Item1.DoubleMas);
        }


        static void RandomTopsTry(byte[] arr, int[] indexes, int count = 10000, int top = 5)
        {
            byte[] vals = indexes.Select(i => arr[i]).ToArray();
            byte[] vals2 = new byte[indexes.Length];
            byte[] peop = indexes.Select(i => n_people[i]).ToArray();

            int pr = preference_costsMemoized(arr), tmp;
            var acr = GetMap(arr);
            double bst = pr + accounting_penalty3(acr), super = bst, f;
            bool yes;
            bool bad(short v) => v < 125 || v > 300;

            for (int i = 0; i < indexes.Length; i++)
            {
                tmp = indexes[i];
                pr -= prCosts[tmp][vals[i] - 1];
                acr[vals[i] - 1] -= peop[i];

                //vals2[i] = RandVal(tmp, top);
            }

            for (int c = 0; c < count; c++)
            {
                yes = true;
                for (int i = 0; i < indexes.Length; i++)
                {
                    tmp = indexes[i];
                    vals2[i] = RandVal(tmp, top);
                    pr += prCosts[tmp][vals2[i] - 1];
                    acr[vals2[i] - 1] += peop[i];
                }
                if (pr < bst)
                {
                    for (int i = 0; i < indexes.Length; i++)
                        if (bad(acr[vals2[i] - 1]) || bad(acr[vals[i] - 1]))
                        {
                            yes = false;
                            break;
                        }
                    if (yes)
                    {
                        f = pr + accounting_penalty3(acr);
                        if (f < bst)
                        {
                            bst = f;
                            for (int i = 0; i < indexes.Length; i++)
                                vals[i] = vals2[i];
                        }
                    }
                }
                for (int i = 0; i < indexes.Length; i++)
                {
                    tmp = indexes[i];
                    pr -= prCosts[tmp][vals2[i] - 1];
                    acr[vals2[i] - 1] -= peop[i];
                }
            }

            if (bst < super)
            {
                for (int i = 0; i < indexes.Length; i++)
                    arr[indexes[i]] = vals[i];
                Console.WriteLine($"new best = {bst}");
            }
        }
        static void BigRandomTopsTry(byte[] arr, int size, int repeat = 1000, int count = 10000, int top = 5)
        {
            var map = GetMap(arr);
            var days = Enumerable.Range(0, 100).Where(c => map[c] < 300 && map[c] > 125).Select(c => c + 1).ToArray();
            var inds = Enumerable.Range(0, 5000).Where(c => days.Contains(arr[c])).ToArray();

            for (int i = 0; i < repeat; i++)
                // RandomTopsTry(arr, GetRandomInArray(0, 4999, size), count, top);
                RandomTopsTry(arr, GetRandomInArray(/*inds*/Filter(), size), count, top);
        }

        static int[][] PerDays()
        {
            var b = new int[100][];
            List<int>[] list = new List<int>[100];
            for (int i = 0; i < 100; i++)
                list[i] = new List<int>(60);

            for (int i = 0; i < 5000; i++)
                list[res[i] - 1].Add(i);

            for (int i = 0; i < 100; i++)
                b[i] = list[i].ToArray();

            return b;
        }
        static int[] WhichPeopIs(int[] inds, byte people)
        {
            List<int> t = new List<int>(inds.Length / 2);
            for (int i = 0; i < inds.Length; i++)
                if (n_people[inds[i]] == people)
                    t.Add(inds[i]);
            return t.ToArray();
        }
        static int[] Mprice(int[] inds, byte from, byte to)
        {
            //from--;to--;

            if (inds.Length == 0)
                return inds;

            List<int> t = new List<int>(inds.Length / 2);
            for (int i = 0; i < inds.Length; i++)
                if (prCosts[inds[i]][from] >= prCosts[inds[i]][to])
                    t.Add(inds[i]);
            return t.ToArray();
        }
        static void Swap()
        {
            var per = PerDays();
            int[] d1, d2, p1, p2, r1, r2;
            best = scoreMemoized2(res);
            double bst;
            int min;

            for (byte day1 = 0; day1 < 99; day1++)
            {
                d1 = per[day1];
                for (byte day2 = (byte)(day1 + 1); day2 < 100; day2++)
                {
                    d2 = per[day2];


                    //r1 = Mprice(d1, day1, day2);
                    //r2 = Mprice(d2, day2, day1);
                    //if (r1.Length + r2.Length != 0)
                    //    Debug.WriteLine($"{r1.Length} {r2.Length}");
                    //if (r2.Length > 0 && r1.Length >= r2.Length)
                    //{
                    //    for (int i = 0; i < r1.Length; i++)
                    //        res[r1[i]] = (byte)(day2+1);
                    //    for (int i = 0; i < r2.Length; i++)
                    //        res[r2[i]] = (byte)(day1+1);
                    //}

                    for (byte cd = 8; cd >= 2; cd--)
                    {
                        p1 = WhichPeopIs(d1, cd);
                        p2 = WhichPeopIs(d2, cd);
                        r1 = Mprice(p1, day1, day2);
                        r2 = Mprice(p2, day2, day1);
                        if (r1.Length + r2.Length != 0)
                            Debug.WriteLine($"{r1.Length} {r2.Length}");
                        if (r2.Length > 0 && r1.Length >= r2.Length)
                        {
                            min = Math.Min(r1.Length, r2.Length);

                            for (int i = 0; i < min; i++)
                                res[r1[i]] = (byte)(day2 + 1);
                            for (int i = 0; i < min; i++)
                                res[r2[i]] = (byte)(day1 + 1);
                        }
                    }

                    bst = scoreMemoized2(res);
                    if (bst < best)
                    {
                        best = bst;
                        Console.WriteLine($"New score: {bst}");
                        WriteData(best);
                    }
                }
            }

        }

        static void Swap3()
        {
            var per = PerDays();
            int[] d1, d2, d3, p1, p2, p3, r1, r2, r3;
            best = scoreMemoized2(res);
            double bst;
            int min;

            for (byte day1 = 0; day1 < 98; day1++)
            {
                d1 = per[day1];
                for (byte day2 = (byte)(day1 + 1); day2 < 99; day2++)
                {
                    d2 = per[day2];
                    for (byte day3 = (byte)(day2 + 1); day3 < 100; day3++)
                    {
                        d3 = per[day3];
                        for (byte cd = 8; cd >= 2; cd--)
                        {
                            p1 = WhichPeopIs(d1, cd);
                            p2 = WhichPeopIs(d2, cd);
                            p3 = WhichPeopIs(d3, cd);
                            r1 = Mprice(p1, day1, day2);
                            r2 = Mprice(p2, day2, day3);
                            r3 = Mprice(p3, day3, day1);
                            if (r1.Length + r2.Length + r3.Length != 0)
                                Debug.WriteLine($"{r1.Length} {r2.Length} {r3.Length}");
                            if (r2.Length > 0 && r1.Length >= r2.Length && r3.Length > r2.Length)
                            {
                                min = Expendator.Min(r1.Length, r2.Length, r3.Length);

                                for (int i = 0; i < min; i++)
                                    res[r1[i]] = (byte)(day2 + 1);
                                for (int i = 0; i < min; i++)
                                    res[r2[i]] = (byte)(day3 + 1);
                                for (int i = 0; i < min; i++)
                                    res[r3[i]] = (byte)(day1 + 1);
                            }
                        }
                    }


                    bst = scoreMemoized2(res);
                    if (bst < best)
                    {
                        best = bst;
                        Console.WriteLine($"New score: {bst}");
                        WriteData(best);
                    }
                }
            }

        }

        static void Swap_2()
        {
            var per = PerDays();
            int[] d1, d2, r1, r2;
            best = scoreMemoized2(res);
            double bst;

            for (byte day1 = 0; day1 < 99; day1++)
            {
                d1 = per[day1];
                for (byte day2 = (byte)(day1 + 1); day2 < 100; day2++)
                {

                    r1 = Mprice(d1, day1, day2);
                    if (r1.Length == 0)
                        continue;

                    d2 = per[day2];
                    r2 = Mprice(d2, day2, day1);

                    if (r2.Length != 0)
                    {
                        Debug.WriteLine($"{r1.Length} {r2.Length}");

                        var cho = Gut(new int[][] { r1, r2 });
                        r1 = cho[0];
                        r2 = cho[1];

                        if (NotEmpty(cho))
                        {
                            for (int i = 0; i < r1.Length; i++)
                                res[r1[i]] = (byte)(day2 + 1);
                            for (int i = 0; i < r2.Length; i++)
                                res[r2[i]] = (byte)(day1 + 1);
                            bst = scoreMemoized2(res);
                            if (bst < best)
                            {
                                best = bst;
                                Console.WriteLine($"New score: {bst}");
                                WriteData(best);
                            }
                            return;
                        }
                    }



                }
            }

        }
        static void Swap_3()
        {
            var per = PerDays();
            int[] d1, d2, d3, r1, r2, r3;
            best = scoreMemoized2(res);
            double bst;

            for (byte day1 = 0; day1 < 100; day1++)
            {
                d1 = per[day1];
                for (byte day2 = 0; day2 < 100; day2++)
                {
                    if (day2 == day1)
                        continue;

                    d2 = per[day2];
                    for (byte day3 = 0; day3 < 100; day3++)
                    {
                        if (day3 == day2 || day3 == day1)
                            continue;

                        d3 = per[day3];

                        r1 = Mprice(d1, day1, day2);
                        r2 = Mprice(d2, day2, day3);
                        r3 = Mprice(d3, day3, day1);
                        if (r1.Length != 0 && r2.Length != 0 && r3.Length != 0)
                        {
                            Debug.WriteLine($"{r1.Length} {r2.Length} {r3.Length}");

                            var cho = Gut(new int[][] { r1, r2, r3 });
                            r1 = cho[0];
                            r2 = cho[1];
                            r3 = cho[2];

                            if (r1.Length != 0 && r2.Length != 0 && r3.Length != 0)
                            {
                                for (int i = 0; i < r1.Length; i++)
                                    res[r1[i]] = (byte)(day2 + 1);
                                for (int i = 0; i < r2.Length; i++)
                                    res[r2[i]] = (byte)(day3 + 1);
                                for (int i = 0; i < r3.Length; i++)
                                    res[r3[i]] = (byte)(day1 + 1);
                            }
                        }

                    }


                    bst = scoreMemoized2(res);
                    if (bst < best)
                    {
                        best = bst;
                        Console.WriteLine($"New score: {bst}");
                        WriteData(best);
                    }
                }
            }

        }
        static void Swap_4()
        {
            var per = PerDays();
            int[] d1, d2, d3, d4, r1, r2, r3, r4, p1, p2, p3, p4;
            best = scoreMemoized2(res);
            double bst;

            for (byte day1 = 0; day1 < 100; day1++)
            {
                d1 = per[day1];
                for (byte day2 = 0; day2 < 100; day2++)
                {
                    if (day1 == day2)
                        continue;
                    d2 = per[day2];
                    r1 = Mprice(d1, day1, day2);
                    if (r1.Length == 0)
                        continue;

                    for (byte day3 = 0; day3 < 100; day3++)
                    {
                        if (day3 == day2 || day3 == day1)
                            continue;
                        d3 = per[day3];
                        r2 = Mprice(d2, day2, day3);
                        if (r2.Length == 0)
                            continue;

                        for (byte day4 = 0; day4 < 100; day4++)
                        {
                            if (day4 == day2 || day4 == day1 || day4 == day3)
                                continue;

                            d4 = per[day4];

                            r3 = Mprice(d3, day3, day4);

                            if (r3.Length == 0)
                                continue;

                            r4 = Mprice(d4, day4, day1);

                            // Debug.WriteLine($"{r1.Length} {r2.Length} {r3.Length} {r4.Length}");

                            if (r4.Length != 0)
                            {
                                Debug.WriteLine($"{r1.Length} {r2.Length} {r3.Length} {r4.Length}");

                                var cho = Gut(new int[][] { r1, r2, r3, r4 });

                                if (NotEmpty(cho))
                                {
                                    p1 = cho[0];//new Vectors(p1).Show();
                                    p2 = cho[1]; //new Vectors(p2).Show();
                                    p3 = cho[2]; //new Vectors(p3).Show();
                                    p4 = cho[3]; //new Vectors(p4).Show();

                                    //var pr = preference_costsMemoized(res);pr.Show();
                                    // var ac = accounting_penaltyMemoized(res);ac.Show();

                                    for (int i = 0; i < p1.Length; i++)
                                        res[p1[i]] = (byte)(day2 + 1);
                                    for (int i = 0; i < p2.Length; i++)
                                        res[p2[i]] = (byte)(day3 + 1);
                                    for (int i = 0; i < p3.Length; i++)
                                        res[p3[i]] = (byte)(day4 + 1);
                                    for (int i = 0; i < p4.Length; i++)
                                        res[p4[i]] = (byte)(day1 + 1);

                                    //preference_costsMemoized(res).Show();
                                    //accounting_penaltyMemoized(res).Show();

                                    bst = scoreMemoized2(res);
                                    Console.WriteLine($"New score: {bst}");
                                    WriteData(bst);

                                }
                            }

                        }

                    }

                }
            }

        }
        static void Swap_5()
        {
            var per = PerDays();
            int[] d1, d2, d3, d4, d5, r1, r2, r3, r4, r5, p1, p2, p3, p4, p5;
            best = scoreMemoized2(res);
            double bst;

            for (byte day1 = 0; day1 < 100; day1++)
            {
                //begs:
                d1 = per[day1];
                for (byte day2 = 0; day2 < 100; day2++)
                {
                    if (day1 == day2)
                        continue;
                    d2 = per[day2];
                    r1 = Mprice(d1, day1, day2);
                    if (r1.Length == 0)
                        continue;

                    for (byte day3 = 0; day3 < 100; day3++)
                    {
                        if (day3 == day2 || day3 == day1)
                            continue;
                        d3 = per[day3];
                        r2 = Mprice(d2, day2, day3);
                        if (r2.Length == 0)
                            continue;

                        for (byte day4 = 0; day4 < 100; day4++)
                        {
                            if (day4 == day2 || day4 == day1 || day4 == day3)
                                continue;

                            d4 = per[day4];

                            r3 = Mprice(d3, day3, day4);

                            if (r3.Length == 0)
                                continue;

                            for (byte day5 = 0; day5 < 100; day5++)
                            {
                                if (day5 == day2 || day5 == day1 || day5 == day3 || day5 == day4)
                                    continue;

                                r4 = Mprice(d4, day4, day5);
                                if (r4.Length == 0)
                                    continue;
                                d5 = per[day5];
                                r5 = Mprice(d5, day5, day1);

                                if (r5.Length != 0)
                                {
                                    Debug.WriteLine($"{r1.Length} {r2.Length} {r3.Length} {r4.Length} {r5.Length}");

                                    var cho = Gut(new int[][] { r1, r2, r3, r4, r5 });

                                    if (NotEmpty(cho))
                                    {
                                        p1 = cho[0];
                                        p2 = cho[1];
                                        p3 = cho[2];
                                        p4 = cho[3];
                                        p5 = cho[4];

                                        //var pr = preference_costsMemoized(res);pr.Show();
                                        // var ac = accounting_penaltyMemoized(res);ac.Show();

                                        for (int i = 0; i < p1.Length; i++)
                                            res[p1[i]] = (byte)(day2 + 1);
                                        for (int i = 0; i < p2.Length; i++)
                                            res[p2[i]] = (byte)(day3 + 1);
                                        for (int i = 0; i < p3.Length; i++)
                                            res[p3[i]] = (byte)(day4 + 1);
                                        for (int i = 0; i < p4.Length; i++)
                                            res[p4[i]] = (byte)(day5 + 1);
                                        for (int i = 0; i < p5.Length; i++)
                                            res[p5[i]] = (byte)(day1 + 1);

                                        //preference_costsMemoized(res).Show();
                                        //accounting_penaltyMemoized(res).Show();

                                        bst = scoreMemoized2(res);
                                        Console.WriteLine($"New score: {bst}");
                                        WriteData(bst);

                                    }
                                }
                                //else
                                //{
                                //    day1++;
                                //    goto begs;
                                //}

                            }
                        }
                    }
                }
            }
        }
        static void Swap_6()
        {
            var per = PerDays();
            int[] d1, d2, d3, d4, d5, d6, r1, r2, r3, r4, r5, r6, p1, p2, p3, p4, p5, p6;
            best = scoreMemoized2(res);
            double bst;

            for (byte day1 = 0; day1 < 100; day1++)
            {
                d1 = per[day1];
                for (byte day2 = 0; day2 < 100; day2++)
                {
                    if (day1 == day2)
                        continue;
                    d2 = per[day2];
                    r1 = Mprice(d1, day1, day2);
                    if (r1.Length == 0)
                        continue;

                    for (byte day3 = 0; day3 < 100; day3++)
                    {
                        if (day3 == day2 || day3 == day1)
                            continue;
                        d3 = per[day3];
                        r2 = Mprice(d2, day2, day3);
                        if (r2.Length == 0)
                            continue;

                        for (byte day4 = 0; day4 < 100; day4++)
                        {
                            if (day4 == day2 || day4 == day1 || day4 == day3)
                                continue;

                            d4 = per[day4];

                            r3 = Mprice(d3, day3, day4);

                            if (r3.Length == 0)
                                continue;

                            for (byte day5 = 0; day5 < 100; day5++)
                            {
                                if (day5 == day2 || day5 == day1 || day5 == day3 || day5 == day4)
                                    continue;

                                r4 = Mprice(d4, day4, day5);
                                if (r4.Length == 0)
                                    continue;

                                for (byte day6 = 0; day6 < 100; day6++)
                                {
                                    if (day6 == day2 || day6 == day1 || day6 == day3 || day6 == day4 || day6 == day5)
                                        continue;

                                    d5 = per[day5];
                                    r5 = Mprice(d5, day5, day6);
                                    if (r5.Length == 0)
                                        continue;

                                    d6 = per[day6];
                                    r6 = Mprice(d6, day6, day1);

                                    if (r6.Length != 0)
                                    {
                                        Console.WriteLine($"{r1.Length} {r2.Length} {r3.Length} {r4.Length} {r5.Length}");

                                        var cho = Gut(new int[][] { r1, r2, r3, r4, r5, r6 });

                                        if (NotEmpty(cho))
                                        {
                                            p1 = cho[0];
                                            p2 = cho[1];
                                            p3 = cho[2];
                                            p4 = cho[3];
                                            p5 = cho[4];
                                            p6 = cho[5];

                                            //var pr = preference_costsMemoized(res);pr.Show();
                                            // var ac = accounting_penaltyMemoized(res);ac.Show();

                                            for (int i = 0; i < p1.Length; i++)
                                                res[p1[i]] = (byte)(day2 + 1);
                                            for (int i = 0; i < p2.Length; i++)
                                                res[p2[i]] = (byte)(day3 + 1);
                                            for (int i = 0; i < p3.Length; i++)
                                                res[p3[i]] = (byte)(day4 + 1);
                                            for (int i = 0; i < p4.Length; i++)
                                                res[p4[i]] = (byte)(day5 + 1);
                                            for (int i = 0; i < p5.Length; i++)
                                                res[p5[i]] = (byte)(day6 + 1);
                                            for (int i = 0; i < p6.Length; i++)
                                                res[p6[i]] = (byte)(day1 + 1);

                                            //preference_costsMemoized(res).Show();
                                            //accounting_penaltyMemoized(res).Show();

                                            bst = scoreMemoized2(res);
                                            Console.WriteLine($"New score: {bst}");
                                            WriteData(bst);
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        static void Swap_6(byte target)
        {
            var per = PerDays();
            int[] d1, d2, d3, d4, d5, d6, r1, r2, r3, r4, r5, r6, p1, p2, p3, p4, p5, p6;
            best = scoreMemoized2(res);
            double bst;

            for (byte day1 = 0; day1 < 100; day1++)
            {
                d1 = per[day1];
                for (byte day2 = 0; day2 < 100; day2++)
                {
                    if (day1 == day2)
                        continue;
                    d2 = per[day2];
                    r1 = Mprice(d1, day1, day2);
                    if (r1.Length == 0)
                        continue;

                    for (byte day3 = 0; day3 < 100; day3++)
                    {
                        if (day3 == day2 || day3 == day1)
                            continue;
                        d3 = per[day3];
                        r2 = Mprice(d2, day2, day3);
                        if (r2.Length == 0)
                            continue;

                        for (byte day4 = 0; day4 < 100; day4++)
                        {
                            if (day4 == day2 || day4 == day1 || day4 == day3)
                                continue;

                            d4 = per[day4];

                            r3 = Mprice(d3, day3, day4);

                            if (r3.Length == 0)
                                continue;

                            for (byte day5 = 0; day5 < 100; day5++)
                            {
                                if (day5 == day2 || day5 == day1 || day5 == day3 || day5 == day4)
                                    continue;

                                r4 = Mprice(d4, day4, day5);
                                if (r4.Length == 0)
                                    continue;

                                for (byte day6 = 0; day6 < 100; day6++)
                                {
                                    if (day6 == day2 || day6 == day1 || day6 == day3 || day6 == day4 || day6 == day5)
                                        continue;

                                    d5 = per[day5];
                                    r5 = Mprice(d5, day5, day6);
                                    if (r5.Length == 0)
                                        continue;

                                    d6 = per[day6];
                                    r6 = Mprice(d6, day6, target);

                                    if (r6.Length != 0)
                                    {
                                        Debug.WriteLine($"{r1.Length} {r2.Length} {r3.Length} {r4.Length} {r5.Length}");

                                        var cho = /*Gut(*/new int[][] { r1, r2, r3, r4, r5, r6 }/*)*/;

                                        if (NotEmpty(cho))
                                        {
                                            p1 = cho[0];
                                            p2 = cho[1];
                                            p3 = cho[2];
                                            p4 = cho[3];
                                            p5 = cho[4];
                                            p6 = cho[5];

                                            var pr = preference_costsMemoized(res); pr.Show();
                                            var ac = accounting_penaltyMemoized(res); ac.Show();

                                            for (int i = 0; i < p1.Length; i++)
                                                res[p1[i]] = (byte)(day2 + 1);
                                            for (int i = 0; i < p2.Length; i++)
                                                res[p2[i]] = (byte)(day3 + 1);
                                            for (int i = 0; i < p3.Length; i++)
                                                res[p3[i]] = (byte)(day4 + 1);
                                            for (int i = 0; i < p4.Length; i++)
                                                res[p4[i]] = (byte)(day5 + 1);
                                            for (int i = 0; i < p5.Length; i++)
                                                res[p5[i]] = (byte)(day6 + 1);
                                            for (int i = 0; i < /*6*/p1.Length; i++)
                                                res[p6[i]] = (byte)(target + 1);

                                            preference_costsMemoized(res).Show();
                                            accounting_penaltyMemoized(res).Show();

                                            bst = scoreMemoized2(res);
                                            bst.Show();
                                            if (bst < best)
                                            {
                                                Console.WriteLine($"New score: {bst}");
                                                WriteData(bst);
                                            }

                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        static int[][] Gut(int[][] it)
        {
            int[][] t = new int[it.GetLength(0)][];
            int[][] rs = new int[it.GetLength(0)][];
            for (int i = 0; i < t.GetLength(0); i++)
            {
                t[i] = new int[it[i].Length];
                for (int j = 0; j < t[i].Length; j++)
                    t[i][j] = n_people[it[i][j]];
            }

            int min = t.Select(s => s.Sum()).Min();
            for (int count = min; count >= 2; count--)
            {
                bool gut = true;
                for (int i = 0; i < t.GetLength(0); i++)
                {
                    rs[i] = Expendator.IndexesWhichGetSum(t[i], count);
                    if (rs[i].Length == 0)
                    {
                        gut = false;
                        break;
                    }
                }
                if (gut)
                {
                    for (int i = 0; i < t.GetLength(0); i++)
                        for (int j = 0; j < rs[i].Length; j++)
                            rs[i][j] = it[i][rs[i][j]];
                    return rs;
                }
            }
            return new int[it.GetLength(0)][];

        }
        static bool NotEmpty(int[][] arr)
        {
            for (int i = 0; i < arr.GetLength(0); i++)
                if (arr[i] == null || arr[i].Length == 0)
                    return false;

            return true;
        }

        static byte[] Migration(byte[] arr, int more = 250, int less = 150, int batch = 15)
        {
            var map = GetMap(arr);
            var up = Days(map, s => s >= more);
            var down = Days(map, s => s <= less);

            List<(int[], byte)> inds = new List<(int[], byte)>();

            foreach (var m in up)
            {
                var mas = IndexOfDays(arr, m);
                foreach (var l in down)
                {
                    var t = Mprice(mas, m, l);
                    if (t.Length > 0)
                        inds.Add((t, l));
                }
            }

            List<(int, byte)> tmp = new List<(int, byte)>(inds.Capacity);
            for (int i = 0; i < inds.Count; i++)
                for (int j = 0; j < inds[i].Item1.Length; j++)
                    tmp.Add((inds[i].Item1[j], inds[i].Item2));

            int q;
            (int, byte) d;
            batch = Math.Min(batch, tmp.Count);
            for (int i = 0; i < batch; i++)
            {
                q = randomgen.Next(0, tmp.Count - 1);
                d = tmp[q];
                arr[d.Item1] = d.Item2;
            }

            return arr;
        }

        static void MigratTest()
        {
            Console.WriteLine($"before: pref = {preference_costsMemoized(res)}  acc = {accounting_penaltyMemoized(res)}");

            for (int batch = 1; batch < 40; batch += 1)
            {
                var res2 = Migration(res.Dup(), 240, 160, batch);
                Console.WriteLine($"after batch {batch}: pref = {preference_costsMemoized(res2)}  acc = {accounting_penaltyMemoized(res2)}");
                if (scoreMemoized2(res2) < best)
                {
                    res = res2;
                    WriteData(scoreMemoized2(res2));
                }
            }

        }

        static int[] IndexOfDays(byte[] arr, int day)
        {
            List<int> inds = new List<int>(125);
            for (int i = 0; i < arr.Length; i++)
                if (arr[i] == day)
                    inds.Add(i);
            return inds.ToArray();
        }
        static int[] IndexOfDays(byte[] arr, Func<byte, bool> pred)
        {
            List<int> inds = new List<int>(125);
            for (int i = 0; i < arr.Length; i++)
                if (pred(arr[i]))
                    inds.Add(i);
            return inds.ToArray();
        }
        static int[] IndexOfFamilies(Func<byte, bool> pred)
        {
            List<int> inds = new List<int>(200);
            for (int i = 0; i < n_people.Length; i++)
                if (pred(n_people[i]))
                    inds.Add(i);
            return inds.ToArray();
        }

        /// <summary>
        /// Возвращает дни, удовлетворяющие предикату
        /// </summary>
        /// <param name="map"></param>
        /// <param name="pred"></param>
        /// <returns></returns>
        static byte[] Days(short[] map, Func<short, bool> pred)
        {
            List<byte> inds = new List<byte>(15);
            for (byte i = 0; i < (byte)map.Length; i++)
                if (pred(map[i]))
                    inds.Add((byte)(i + 1));
            return inds.ToArray();
        }


        /// <summary>
        /// Перемешивает элементы массива указанное число раз
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="count"></param>
        public static void Swap<T>(this T[] arr, int count)
        {
            T tmp;
            int i1, i2;
            for (int i = 0; i < count; i++)
            {
                i1 = randomgen.Next(0, arr.Length);
                tmp = arr[i1];

                i2 = randomgen.Next(0, arr.Length);
                arr[i1] = arr[i2];
                arr[i2] = tmp;
            }
        }

        /// <summary>
        /// Возвращает массив случайных чисел от min до max
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static int[] GetRandomInArray(int min, int max, int count)
        {
            int[] r = new int[count];
            for (int i = 0; i < count; i++)
                r[i] = randomgen.Next(min, max);
            return r.Distinct().ToArray();
        }

        /// <summary>
        /// Возвращает случайную подвыборку указанного массива
        /// </summary>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static int[] GetRandomInArray(int[] from, int count)
        {
            int[] r = new int[count];
            for (int i = 0; i < count; i++)
                r[i] = from[randomgen.Next(0, from.Length)];
            return r.Distinct().ToArray();
        }


        static double[] ContributionPr()
        {
            double pr = preference_costsMemoized(res);
            var t = new double[5000];
            for (int i = 0; i < t.Length; i++)
                t[i] = prCosts[i][res[i] - 1] / pr;
            return t;
        }

        static double[] ContributionAcc()
        {
            var map = GetMap(res);
            double r = accounting_penalty3(map);
            double[] proc = new double[100];
            for (int i = 0; i < 99; i++)
                proc[i] = Ntonumber[map[i] - 125][map[i + 1] - 125] / r;
            proc[99] = lastN[map[99] - 125] / r;

            double[] t = new double[5000];
            for (int i = 0; i < t.Length; i++)
                t[i] = proc[res[i] - 1] * n_people[i] / map[res[i] - 1];
            return t;
        }
        static double[] GetProcArray()
        {
            var map = GetMap(res);
            double r = accounting_penalty3(map);
            double[] proc = new double[100];
            for (int i = 0; i < 99; i++)
                proc[i] = Ntonumber[map[i] - 125][map[i + 1] - 125] / r;
            proc[99] = lastN[map[99] - 125] / r;
            return proc;
        }

        static void WriteContributions(string filemane)
        {
            var pr = ContributionPr();
            var acc = ContributionAcc();
            var s = Enumerable.Range(0, 5000).Select(i => $"{i + 1},{pr[i].ToString().Replace(',', '.')},{acc[i].ToString().Replace(',', '.')}").ToList();
            s.Insert(0, "id,pr,acc");
            Expendator.WriteInFile(filemane, s.ToArray());
        }
        static int[] GetSub()
        {
            WriteContributions(@"C:\Users\крендель\Desktop\MagicCode\Машинное обучение\Santa's Workshop Tour 2019\conts.csv");
            Expendator.StartProcessOnly("BuildSubset.r",
                false,
                @"C:\Users\крендель\Desktop\MagicCode\Машинное обучение\Santa's Workshop Tour 2019");
            return Expendator.GetStringArrayFromFile(@"C:\Users\крендель\Desktop\MagicCode\Машинное обучение\Santa's Workshop Tour 2019\subset.txt").Select(i => Convert.ToInt32(i)).ToArray();
        }
        static int[] GetSub2()
        {
            var p = ContributionAcc();
            var t = ContributionPr();
            return Enumerable.Range(0, 5000).Where(i => p[i] + t[i] > 0).ToArray();
        }


        static byte[] EchoDown(byte[] arr)
        {
            //зашумить
            //var inds = Range.Where(c => n_people[c] == 7&&choice_0[c]==1).ToArray();

          //  var inds = Range.Where(c => /*(n_people[c] ==8|| n_people[c] == 7) &&*/ arr[c] == choice_4[c]).ToArray();
           // for (int i = 0; i < inds.Length; i++)
           //     arr[inds[i]] = choice_0[inds[i]];
            //WriteData(arr, Environment.CurrentDirectory);
while (true)
            {
                var map = GetMap(arr);
                var mp = Enumerable.Range(0, 100).Where(c => map[c] >300).Select(c => (byte)(c + 1)).ToArray();
                var mp2 = Enumerable.Range(0, 100).Where(c => map[c] <285).Select(c => (byte)(c + 1)).ToArray();
                if (mp.Length == 0)
                    break;
                //else Console.WriteLine($"{mp.Length} {preference_costsMemoized(arr)}");
                var not = Range.Where(c => mp.Contains(arr[c])).ToArray();
                var not2 = Range.Where(c => !mp2.Contains(arr[c])).ToArray();
                int[] mins = new int[5000];
                int[][] vals = new int[5000][];
                for (int i = 0; i < 5000; i++)
                {
                    mins[i] = 1000000;
                    vals[i] = new int[] { 10000000 };
                }

                foreach (var i in not)
                {
                    int price = prCosts[i][arr[i] - 1];
                    vals[i] = new int[mp2.Length];
                    for (int j = 0; j < mp2.Length; j++)
                        vals[i][j] = prCosts[i][mp2[j] - 1] - price;
                    mins[i] = vals[i].Min();
                }
                var minrow = Array.IndexOf(mins, mins.Min());
                var mincol = Array.IndexOf(vals[minrow], mins.Min());
                arr[minrow] = mp2[mincol];

            }
            //заполнить дни, меньшие 125
            while (true)
            {
                var map = GetMap(arr);
                var mp = Enumerable.Range(0, 100).Where(c => map[c] < 125).Select(c => (byte)(c + 1)).ToArray();
                var mp2 = Enumerable.Range(0, 100).Where(c => map[c] < 140).Select(c => (byte)(c + 1)).ToArray();
                if (mp.Length == 0)
                    break;
                //else Console.WriteLine($"{mp.Length} {preference_costsMemoized(arr)}");
                var not = Range.Where(c => !mp.Contains(arr[c])).ToArray();
                var not2 = Range.Where(c => !mp2.Contains(arr[c])).ToArray();
                int[] mins = new int[5000];
                int[][] vals = new int[5000][];
                for (int i = 0; i < 5000; i++)
                {
                    mins[i] = 1000000;
                    vals[i] = new int[] { 10000000 };
                }

                foreach (var i in not2)
                {
                    int price = prCosts[i][arr[i] - 1];
                    vals[i] = new int[mp.Length];
                    for (int j = 0; j < mp.Length; j++)
                        vals[i][j] = prCosts[i][mp[j] - 1] - price;
                    mins[i] = vals[i].Min();
                }
                var minrow = Array.IndexOf(mins, mins.Min());
                var mincol = Array.IndexOf(vals[minrow], mins.Min());
                arr[minrow] = mp[mincol];

            }
            //WriteData(arr, Environment.CurrentDirectory);


            //раскидать с дней, больших 300
            


            WriteData(arr, Environment.CurrentDirectory);
            //ShowStructure();
            (best, arr) = MakeCoordMinSlow2Parallel(arr, Range);
            ShowStructure();
             WriteData(arr, Environment.CurrentDirectory);
            //for(int i=0;i<500;i++)
            //MinRandomSeries(50, 7);
            return arr;
        }

        static (double,byte[]) LineDown(byte[] arr,int badcount=30)
        {
            var inds = Range.Where(c => res[c] != arr[c]).ToArray();
            if (inds.Length == 10)
                return MinByRandomizeBy10(arr,inds, 5);
            else if (inds.Length < 10)
            {              
                var pp = new (double, byte[])[badcount];
                var t = new int[10];
                for (int i = 0; i < badcount; i++)
                {
                    t = GetRandom().Where(r => !inds.Contains(r)).Take(10-inds.Length).Union(inds).ToArray();
                    pp[i] = MinByRandomizeBy10(arr, t, 5);
                }
                return pp.First(s => s.Item1 == pp.Min(r => r.Item1));
            }
            else
            {
                int k,l,tmp;
                var pp = new (double, byte[])[badcount];
                for(int i = 0; i < badcount; i++)
                {
                    for(int j = 0; j < inds.Length; j++)
                    {
                        k = randomgen.Next(inds.Length - 1);
                        l = randomgen.Next(inds.Length - 1);
                        tmp = inds[k];
                        inds[k] = inds[l];
                        inds[l] = tmp;
                    }

                    pp[i]= MinByRandomizeBy10(arr,inds.Take(10).ToArray(), 5);
                }
                return pp.First(s => s.Item1 == pp.Min(r => r.Item1));
            }
        }
    }


}

