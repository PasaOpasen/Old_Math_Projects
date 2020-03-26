using System;
using System.Collections.Generic;
using МатКлассы;
using static МатКлассы.FuncMethods;
using Complex = МатКлассы.Number.Complex;
using System.IO;
using System.Linq;
using JR.Utils.GUI.Forms;

namespace MullerConsole
{
    class Program
    {
        static double xmin, xmax, ymin, ymax, eps;
        static int count;
        static string[] funcs;
        static List<string> result = new List<string>(50);
        static StreamWriter wr = new StreamWriter($"{DateTime.Now.ToString().Replace(':',' ')}.txt");

        static void Main(string[] args)
        {
            ReadData();
            
            Expendator.EmptyLine(1);
            Calculate();
         
            Expendator.EmptyLine(4);

            wr.Close();

            "ВЫЧИСЛЕНИЯ ЗАВЕРШЕНЫ".Show();
            Console.ReadKey();
        }

        static void ReadData()
        {
            //File.Copy(Expendator.GetResource("Файл с данными.txt"), Environment.CurrentDirectory);
            var arr = Expendator.GetStringArrayFromFile("Файл с данными.txt", true).Where(s => !s.StartsWith('#')).ToArray()[1..];

            double getVal(string s) => s.Split(' ')[1].ToDouble();
            var ind = Array.IndexOf(arr, arr.First(p=>p.StartsWith("xmin")))-1;

            funcs = arr[..ind];

            xmin = getVal(arr[++ind]);
            xmax = getVal(arr[++ind]);
            ymin = getVal(arr[++ind]);
            ymax = getVal(arr[++ind]);
            eps= getVal(arr[++ind]);
            count = (int)getVal(arr[++ind]);

        }
    
        static void Calculate()
        {
            foreach(var fc in funcs)
            {
                Func<Complex, Complex> f = ParserComplex.GetDelegate(fc, out var fm);
                result.Add(""); result.Add("");
                result.Add($"Формула {fc} была прочитана как {fm}");
                result.Add("-----------Корни (округлённые):");

                var r = Optimization.MullerTryMany((c)=>f(c), xmin, xmax, ymin, ymax, out var rs, eps, count);
                if (r)
                {
                    // это просто божественное решение проблемы кластеризации корней с учётом погрешностей, но оно ещё как работает!
                    result.AddRange(rs.OrderBy(c=>f(c).Abs)
                        .GroupBy(t=>t.Round(4)).Select(group=>group.First())
                        .Select(t => $"----> root = {t.Round(4)}   \t|f(root)| = {f(t).Abs}"));
                }
                else result.Add($"Для функции {fm} корней не найдено. Попробуйте изменить гиперпараметры и начать снова");


                foreach (var st in result)
                {
                    st.Show();
                    wr.WriteLine(st);
                }


                result = new List<string>(50);
            }
        }
    }
}
