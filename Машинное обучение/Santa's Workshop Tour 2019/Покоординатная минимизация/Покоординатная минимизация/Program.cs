using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Покоординатная_минимизация
{
    class Program
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
        static Func<byte[], double> preference_costs = (byte[] arr) =>
            {
                byte[] sum = new byte[5000];
                int s = 0;

                for (int i = 0; i < arr.Length; i++)
                    if (arr[i] == choice_0[i])
                    {
                        sum[i]++;
                    }
                for (int i = 0; i < arr.Length; i++)
                    if (arr[i] == choice_1[i])
                    {
                        sum[i]++;
                        s += 50;
                    }
                for (int i = 0; i < arr.Length; i++)
                    if (arr[i] == choice_2[i])
                    {
                        sum[i]++;
                        s += 50 + 9 * n_people[i];
                    }
                for (int i = 0; i < arr.Length; i++)
                    if (arr[i] == choice_3[i])
                    {
                        sum[i]++;
                        s += 100 + 9 * n_people[i];
                    }
                for (int i = 0; i < arr.Length; i++)
                    if (arr[i] == choice_4[i])
                    {
                        sum[i]++;
                        s += 200 + 9 * n_people[i];
                    }
                for (int i = 0; i < arr.Length; i++)
                    if (arr[i] == choice_5[i])
                    {
                        sum[i]++;
                        s += 200 + 18 * n_people[i];
                    }
                for (int i = 0; i < arr.Length; i++)
                    if (arr[i] == choice_6[i])
                    {
                        sum[i]++;
                        s += 300 + 18 * n_people[i];
                    }
                for (int i = 0; i < arr.Length; i++)
                    if (arr[i] == choice_7[i])
                    {
                        sum[i]++;
                        s += 300 + 36 * n_people[i];
                    }
                for (int i = 0; i < arr.Length; i++)
                    if (arr[i] == choice_8[i])
                    {
                        sum[i]++;
                        s += 400 + 36 * n_people[i];
                    }
                for (int i = 0; i < arr.Length; i++)
                    if (arr[i] == choice_9[i])
                    {
                        sum[i]++;
                        s += 500 + (36 + 199) * n_people[i];
                    }
                for (int i = 0; i < sum.Length; i++)
                    if (sum[i] == 0)
                        s += (500 + (36 + 398) * n_people[i]);

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
        static Func<byte[], double> score = (byte[] arr) => preference_costs(arr) + accounting_penalty(arr);

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

            bool existprogress = false;

            byte[][] mat = GetNresCopy();

            var numbers = GetRange(); //GetRandom();
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
                    Console.WriteLine($"best score = {best}; iter = {nb}");
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

        static readonly int countbegins = 1000;
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

        static void Main(string[] args)
        {
            ReadData();
            //ReadRES();

           // MakeResult2();

            // var t = preference_costs(res);
            // var s = accounting_penalty(res);
            ReadBegins();
            for(int y = 0; y < countbegins; y++)
            {
                ReadRES();
                //res = begins[y];

                Console.WriteLine($"----------------ITERATION {y+1}. Begin score = {best}");
                //best = preference_costs(res);
                //MakeResult2(preference_costs, "prf");
                best = score(res);
                MakeResult2(score);
               // MakeResult3(score);
            }
            
           
        }
    }
}
