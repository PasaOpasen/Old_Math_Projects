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
                lastN[i] = (n - 125.0) / 400.0 * Math.Sqrt(n);
            }

            for (short i = 125; i <= 300; i++)
            {
                Ntonumber[i - 125] = new double[176];
                for (short j = 125; j <= 300; j++)
                    Ntonumber[i - 125][j - 125] = (i - 125.0) / 400.0 * Math.Pow(i, 0.5 + 0.02 * Math.Abs(i - j));
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

            var p = new byte[] { choice_0[index], choice_1[index], choice_2[index], choice_3[index], choice_4[index], choice_5[index], choice_6[index], choice_7[index], choice_8[index], choice_9[index] };
            return p[randomgen.Next(0, top - 1)];
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
                    return 1e20;
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
                    return 1e20;
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
                    return 1e20;
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
            if (acc >= 1e20) return acc;
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

            if (locbest == 1e20)
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
        static (double, byte[]) MakeCoordMinSlow2(byte[] sample_res,int[] bt)
        {
            return MinByTwoChoise(sample_res,bt, preference_costsMemoized(sample_res), GetMap(sample_res));
        }
        static (double, byte[]) MakeCoordMinSlowPr(byte[] sample_res,int count=15)
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

            if (existprogress && locbest != best &&count>0)
            {
                count--;
                goto begin1;
            }
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
                    if (score(res) >= 1e20)
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

        static bool MinByRandomize2(int dim = 10, int count = 2000)
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
                    tmp = RandVal(indexes[j]);//(byte)randomgen.Next(1, 100);
                    sample[j] = tmp;
                    pr2 += prCosts[indexes[j]][tmp - 1];
                    acr2[tmp - 1] += n_people[indexes[j]];
                }

                if (pr2 >= best || acr2.Any(s => s < 125 || s > 300))
                    results[i] = 1e20;
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

        static (double res, byte val) MinByOne(int ind)
        {
            double resultbest = 1e20, result = 1e20;
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
            double result_best = 1e20, result;
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
            double result_best = 1e20, result;
            int index = 1;
            byte np = n_people[ind], nd = (byte)(sample_res[ind] - 1);

            pr -= prCosts[ind][nd];
            acr[nd] -= np;

            foreach (int i in Top(ind, top))
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

            return (res: result_best, val: (byte)index);
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

                    for(int ii=0;ii<top;ii++)
                    //Parallel.For(0, top, ii =>
                    {
                        int pr2 = pr + prCosts[i][a1[ii] - 1];
                        //var acr2 = acr.Dup();
                        double f = first, tmp = 1e20;
                        acr[a1[ii] - 1] += ni;

                        foreach (var i2 in a2)
                        {
                            pr2 += prCosts[j][i2 - 1];
                            acr[i2 - 1] += nj;

                            if (pr2 < f&&gut(acr[sample_res[i] - 1])&&gut(acr[sample_res[j] - 1]) && gut(acr[a1[ii] - 1])&&gut(acr[i2 - 1]))
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
                        min = 1e20;
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
            return (first,sample_res);
        }

        static (double, byte[]) MinByTwoChoise(byte[] sample_res,int[] dx, int pr, short[] acr, int top = 8)
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
            (first, sample_res) = MakeCoordMinSlow(sample_res);
            if (howmuch == 40)
                return (first, sample_res);
            //progress = false;
            for (int i = 0; i < dx.Length-1; i++)
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
                        double f = first, tmp = 1e20;
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
                        min = 1e20;
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
            Console.WriteLine($"New result {first}");
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

                if (acr[sr]>=125&&acr[sr]<=300 && acr[i - 1]>=125&& acr[i - 1]<=300)
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
                    results[i] = 1e20;
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

            double resultbest = 1e20, result = 1e20;
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

            double resultbest = 1e20, result = 1e20;
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



        static void Main(string[] args)
        {
            ReadRES();
            ReadUpDown();

            //var map = DaysOfMaxPrice(GetMap(res), 100);
            //var inds = IndexOfDays(res, r => map.Contains(r));
            //TopDown3(Enumerable.Range(0,4999).Where(p=>n_people[p]>6).ToArray(), 5);
            
            //GetNotZeroChoises();
                                                               //MigratTest();

            //TopDown2(4093, 6);
            //TopDown2(5);
           // var p = MinByTwoChoise(res,preference_costsMemoized(res),GetMap(res));
           // int o=0;
            //TopDown3(5);
            //Swap();
            //Swap3();
            // Swap_2();
            // Swap_3();
            //  Swap_4();
            //Swap_5();
            //  Swap_6();
            // Accord();
            //for (byte b = 0; b < 100;b++)
            //Swap_6(b);
            // MakeResult8();
            //NotRandomDown(3);

            for (int count = 5; count <= 30; count += 5)
                for (int top = 5; top >= 4; top--)
                {
                    $"_____________________________count = {count} top = {top}".Show(); "".Show();
                    double b = best;
                    int q = 0;
                    while (q < 3)
                    {
                        RandomDown4(count, top, 250);
                        if (b == best)
                        {
                            q++;
                        }
                        else b = best;
                    }

                }


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

            var inds = GetNotZeroChoises();
            var icount = inds.Length;
            Console.WriteLine($"Not zero families count is {icount} ({Expendator.GetProcent(icount, 5000)}%)");

            Parallel.For(0, iter, (int i) => MakeCoordMinPr(obs[i], deep));

            //var v= obs.Select(p => scoreMemoized2(p)).ToArray();

            (double, byte[])[] links = new (double, byte[])[iter];
            //"Start".Show();
            Parallel.For(0, iter, (int i) => links[i] = MakeCoordMinSlow(obs[i]));
            //for (int i = 0; i < iter; i++) links[i] = MakeCoordMinSlow(obs[i]);

            best = scoreMemoized2(res);
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

        static void RandomDown4(int dim = 10, int top = 5, int iter = 10000)
        {
            var mas = new int[dim];
            var copy = new byte[dim];
            var obs = GetNresCopy(iter);
            int index, j;
            (double, byte[])[] links = new (double, byte[])[iter];

            //var map =Days( GetMap(res), s => s >= 200);
            //var map = DaysOfMaxPrice(GetMap(res), 100);
            //var inds = Enumerable.Range(0, 4999).Where(c => peops1.Contains(n_people[c])  /*<= 6 && n_people[c] >= 3*/).ToArray(); //IndexOfDays(res, r => r>1&&r<70/*map.Contains(r)*/); //GetNotZeroChoises();

            var inds = Enumerable.Range(0, 4999).Where(c => res[c]!=choice_0[c]  /*<= 6 && n_people[c] >= 3*/).ToArray();

            var icount = inds.Length;
            //Console.WriteLine($"Not zero families count is {icount} ({Expendator.GetProcent(icount, 5000)}%)");

            //var inds = IndexOfFamilies(f => f <4);
            //var icount = inds.Length;

            //Parallel.For(0, iter, (int i) => 
            for (int i = 0; i < iter; i++)
            {
                ref var ob = ref obs[i];

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
                        if(n_people[index]>5)
                            ob[index] = RandVal(index, 3);
                       else ob[index] = RandVal(index, top);//choice_0[index]; //RandVal(index) ;// choice_0[index];//LevelDown(index, obs[i][index]);//choice_0[index];//choice_0[index];//
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

            "--->Start".Show();
            Parallel.For(0, iter, (int i) => links[i] = MakeCoordMinSlow(obs[i]));
            //for (int i = 0; i < iter; i++) links[i] = MakeCoordMinSlow(obs[i]);
            "--->Start2".Show();
            Parallel.ForEach(Enumerable.Range(0, iter).Where(s => links[s].Item1 != best), (int i) => links[i] = MakeCoordMinSlow2(obs[i], Enumerable.Range(0, 4999).Where(c => peops2.Contains(n_people[c]) /*n_people[c] ==4*/).ToArray()));

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
                Console.WriteLine($"-------> to best: {links.Count(p => p.Item1 == best)} ; to 1e20: {links.Count(p => p.Item1 == 1e20)}");
                "".Show();
            }
        }

        static byte[] DaysOfMaxPrice(short[] map,double morethan=100)
        {
            var res = new (double, byte)[99];
            for (int i = 0; i < 99; i++)
                res[i] = (Ntonumber[map[i]-125][map[i+1]-125],(byte)(i+1));

            return res.Where(r => r.Item1 > 100).Select(r => r.Item2).ToArray();

        }

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
                Console.WriteLine($"-------> to best: {links.Count(p => p.Item1 == best)} ; to 1e20: {links.Count(p => p.Item1 == 1e20)}");
                "".Show();
            }
        }

        static void RandomDown6(int dim = 10, int top = 5, int iter = 10000)
        {
            var obs = GetNresCopy(iter);
            (double, byte[])[] links = new (double, byte[])[iter];


            Parallel.For(0, iter, (int i) => links[i] = MakeCoordMinSlowPr(obs[i],dim+2*i));
            obs = links.Select(u => u.Item2).ToArray();

            //for (int i = 0; i < iter; i++)
            //{
            //    MakeCoordMinPr(obs[i], dim);
            //}

            "--->Start".Show();
            Parallel.For(0, iter, (int i) => links[i] = MakeCoordMinSlow(obs[i]));

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
                Console.WriteLine($"-------> to best: {links.Count(p => p.Item1 == best)} ; to 1e20: {links.Count(p => p.Item1 == 1e20)}");
                "".Show();
            }
        }

        static void RandomDown7(int dim = 10, int top = 5, int iter = 10000)
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
                Console.WriteLine($"-------> to best: {links.Count(p => p.Item1 == best)} ; to 1e20: {links.Count(p => p.Item1 == 1e20)}");
                "".Show();
            }
        }

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

            if (c6 + c7 + c8 + c9 + no == 0)
                Console.WriteLine($"0: {c0}  1: {c1}  2: {c2}  3: {c3}  4: {c4}  5: {c5}");
            else
                Console.WriteLine($"0: {c0}  1: {c1}  2: {c2}  3: {c3}  4: {c4}  5: {c5}  6: {c6}  7: {c7}  8: {c8}  9: {c9}  other: {no}");
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
                        double f = first, tmp = 1e20;
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
                        min = 1e20;
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
                        double f = first, tmp = 1e20;
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
                        min = 1e20;
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
                            double f = first, tmp = 1e20;
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
                            min = 1e20;
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
        static void TopDown3(int[] indexes,int top = 5)
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
            for (int i = 0; i < indexes.Length-2; i++)
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
                            double f = first, tmp = 1e20;
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
                            min = 1e20;
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

                            for (int t = k + 1; t < 5000; t++)
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
                                     double f = first, tmp = 1e20;
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
                                    min = 1e20;
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

        static byte[] ToByteArr(double[] arr)
        {
            byte[] g = new byte[arr.Length];
            double b;
            for (int i = 0; i < arr.Length; i++)
            {
                b = arr[i];
                if (b < 1) b = 1;
                else if (b > 100) b = 100;
                g[i] = (byte)Math.Round(b);
            }
            return g;
        }
        static double[] ToDouble(byte[] arr)
        {
            var k = new double[arr.Length];
            for (int i = 0; i < arr.Length; i++)
                k[i] = arr[i];
            return k;
        }

        static void Bee()
        {
            var t = BeeHiveAlgorithm.GetGlobalMin(
                (Vectors v) => scoreMemoized2(ToByteArr(v.DoubleMas)),
                n: 5000,
               min: 1, max: 100, eps: 1e-10, countpoints: 300, maxcountstep: 200, center: new Vectors(ToDouble(res)), maxiter: 500);

            best = t.Item2;
            res = ToByteArr(t.Item1.DoubleMas);
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
                                        Debug.WriteLine($"{r1.Length} {r2.Length} {r3.Length} {r4.Length} {r5.Length}");

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
                                                res[p6[i]] = (byte)(target + 1);

                                            //preference_costsMemoized(res).Show();
                                            //accounting_penaltyMemoized(res).Show();

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

        static byte[] Migration(byte[] arr,int more=250,int less=150,int batch = 15)
        {
            var map = GetMap(arr);
            var up = Days(map,s => s >= more);
            var down =Days( map,s => s <= less);

            List<(int[],byte)> inds = new List<(int[], byte)>();

            foreach(var m in up)
            {
                var mas = IndexOfDays(arr, m);
                foreach(var l in down)
                {
                    var t = Mprice(mas, m, l);
                    if (t.Length > 0)
                        inds.Add((t, l));
                }
            }

            List<(int, byte)> tmp = new List<(int, byte)>(inds.Capacity);
            for (int i = 0; i < inds.Count; i++)
                for (int j = 0; j < inds[i].Item1.Length; j++)
                    tmp.Add((inds[i].Item1[j],inds[i].Item2));

            int q;
            (int, byte) d;
            batch = Math.Min(batch, tmp.Count);
            for(int i=0;i<batch;i++)
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

            for(int batch = 1; batch < 40; batch += 1)
            {
                var res2 =Migration( res.Dup(),240,160,batch);
                Console.WriteLine($"after batch {batch}: pref = {preference_costsMemoized(res2)}  acc = {accounting_penaltyMemoized(res2)}");
                if(scoreMemoized2(res2)<best)
                {
                    res = res2;
                    WriteData(scoreMemoized2(res2));
                }
            }

        }

        static int[] IndexOfDays(byte[] arr,int day)
        {
            List<int> inds = new List<int>(125);
            for (int i = 0; i < arr.Length; i++)
                if (arr[i] == day)
                    inds.Add(i);
            return inds.ToArray();
        }
        static int[] IndexOfDays(byte[] arr, Func<byte,bool> pred)
        {
            List<int> inds = new List<int>(125);
            for (int i = 0; i < arr.Length; i++)
                if (pred( arr[i]))
                    inds.Add(i);
            return inds.ToArray();
        }
        static int[] IndexOfFamilies(Func<byte,bool> pred)
        {
            List<int> inds = new List<int>(200);
            for (int i = 0; i < n_people.Length; i++)
                if (pred(n_people[i]))
                    inds.Add(i);
            return inds.ToArray();
        }
        static byte[] Days(short[] map,Func<short,bool> pred)
        {
            List<byte> inds = new List<byte>(15);
            for (byte i = 0; i < (byte)map.Length; i++)
                if (pred(map[i]))
                    inds.Add((byte)(i + 1));
            return inds.ToArray();
        }

        //static void Accord()
        //{
        //    byte[] ToByteArr(double[] arr)
        //    {
        //        byte[] g = new byte[arr.Length];
        //        byte b;
        //        for (int i = 0; i < arr.Length; i++)
        //        {
        //            b = (byte)Math.Round(arr[i]);
        //            if (b < 1) b = 1;
        //            else if (b > 100) b = 100;
        //            g[i] = b;
        //        }
        //        return g;
        //    }
        //    double[] ToDouble(byte[] arr)
        //    {
        //        var k = new double[arr.Length];
        //        for (int i = 0; i < arr.Length; i++)
        //            k[i] = arr[i];
        //        return k;
        //    }

        //    var S = new Accord.Math.Optimization.NelderMead(5000, (mas) => score(ToByteArr(mas)));
        //    Console.WriteLine(score(res));
        //    S.Minimize(ToDouble(res));

        //    res = ToByteArr(S.Solution);

        //    WriteData(S.Value, "sb");
        //}

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
    }


}

