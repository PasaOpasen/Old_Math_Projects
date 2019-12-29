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

namespace OptimizeCore
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
                var sm = MinByOne(nb, sample_res, pr, acr);
                result[index++] = (sm.res, sm.val, nb);
            }

            bst = result.Min(t => t.Item1);

            if (locbest > bst)
            {
                var pi = result.First(t => t.Item1 == bst);
                //Console.WriteLine($"best score = {Math.Round(bst, 3)} (from {Math.Round(best, 3)}); iter = {pi.Item3}");
                locbest = bst;
                existprogress = true;
                sample_res[pi.Item3] = pi.Item2;
            }

            if (existprogress)
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
                    Console.WriteLine("Записывается в файл");
                    WriteData(best, acc);
                    MakeResult2(scoreMemoized2);
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
                    tmp = (byte)randomgen.Next(1, 100);
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

            pr -= prCosts[ind][sample_res[ind] - 1];
            acr[sample_res[ind] - 1] -= n_people[ind];

            for (int i = 1; i <= 100; i++)
            {
                pr += prCosts[ind][i - 1];
                acr[i - 1] += n_people[ind];

                if (pr < best && !acr.Any(s => s < 125 || s > 300))
                {
                    result = accounting_penalty2(acr) + pr;
                    if (result < result_best)
                    {
                        index = i;
                        result_best = result;
                    }
                }

                pr -= prCosts[ind][i - 1];
                acr[i - 1] -= n_people[ind];
            }
            acr[sample_res[ind] - 1] += n_people[ind];

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

        static void Main(string[] args)
        {
            ReadRES();

            // Accord();

            // MakeResult2();

            // var t = preference_costs(res);
            // var s = accounting_penalty(res);

            //ReadBegins();

            for (int i = 1; i <= 200; i++)
            {
                Console.WriteLine($"Осталось итераций: {200 - i}");

                RandomDown2(12, 600);

                if (i % 35 == 0)
                    MakeResult8();
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
            // MakeResult9("",10,1_000_000,1000);
            //MakeResult8("");
            //MakeResult10("", 5, 3, 2);
            // MakeResult11("",100000);
            //MakeResult12("",3,5);
            //MakeResult13("");

            string[] s = Expendator.GetWordFromFile("границы.txt").Split(' ');
            int down_t = Convert.ToInt32(s[0]), up_t = Convert.ToInt32(s[1]);

            Console.WriteLine($"down = {down_t}");
            Console.WriteLine($"up = {up_t}");

            for (int u = 0; u < 10; u++)
            {
                //MakeResult6("");

                //MakeResult2(scoreMemoized2);

                RandomDown(550, down_t, up_t);
                //NotRandomDown();

                NotRandomDown(10);

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
        static void RandomDown2(int dim = 10, int iter = 10000)
        {
            var mas = new int[dim];
            var obs = GetNresCopy(iter);

            int index, j;

            for (int i = 0; i < iter; i++)
            {
            ss:
                for (j = 0; j < dim; j++)
                {
                    mas[j] = randomgen.Next(0, 4999);
                }
                if (mas.Distinct().Count() != dim)
                    goto ss;

                for (j = 0; j < dim; j++)
                {
                    index = mas[j];
                    obs[i][index] = choice_0[index];
                }
            }


            (double, byte[])[] links = new (double, byte[])[iter];
            "Start".Show();
            Parallel.For(0, iter, (int i) => links[i] = MakeCoordMinSlow(obs[i]));
            //for (int i = 0; i < iter; i++) links[i] = MakeCoordMinSlow(obs[i]);

            var bs = links.Min(p => p.Item1);
            var t = links.First(p => p.Item1 == bs);
            res = t.Item2;
            best = bs;

            Console.WriteLine($"Записывается в файл {bs}");
            WriteData(bs, "");
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
    }
}
