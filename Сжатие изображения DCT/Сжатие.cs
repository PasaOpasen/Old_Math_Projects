using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using МатКлассы;
using System.IO;
using System.Drawing;

namespace Сжатие_изображения_DCT
{
    class Сжатие
    {
        [STAThreadAttribute]
        static void Main(string[] args)
        {

            new Compress().ShowDialog();
            //GlobalMembers.BitMap = new Bitmap("test.bmp");
            //МатКлассы.Point size = new МатКлассы.Point(GlobalMembers.BitMap.Width,GlobalMembers.BitMap.Height);

            //--------------------------------------------------------------------------

            //DCT.Transform dct = new DCT.Transform();
            //for (int k = 3; k <= 8; k++)
            //{
            //   GlobalMembers.frec = new Frecuency(10.0, -1000.0, 1000.0, $"frec {k}.txt");
            //    Bitmap B = new Bitmap(GlobalMembers.BitMap);
            //    for (int i = 0; i < size.x - 8; i += 8)
            //    {
            //        for (int j = 0; j < size.y - 8; j += 8)
            //        {
            //            dct.CopyToMatrix(ref B, i, j, (int)size.y);
            //            dct.DCT(k);
            //            dct.IDCT();
            //            dct.CopyOutOfMatrix(ref B, i, j, (int)size.y);
            //        }
            //    }
            //    GlobalMembers.frec.file.Dispose();

            //    GlobalMembers.CompressBitMap = new Bitmap(B);
            //    string FileName = "weightfile";
            //    B.Save(FileName);
            //    FileInfo file = new FileInfo(FileName);
            //    string weight = file.Length.ToString();
            //    GlobalMembers.CompressBitMap.Save($"Итог (k={k} weight = {weight}).bmp");
            //}



            //DCT.Transform dct = new DCT.Transform();
            //for (int k = 3; k <= 8; k++)
            //{
            //    GlobalMembers.frec = new Frecuency(10.0, -1000.0, 1000.0, $"frec {k}.txt");
            //    Bitmap B = new Bitmap(GlobalMembers.BitMap);
            //    GlobalMembers.frec.file.WriteLine($"{size.x} {size.y}");
            //    for (int i = 0; i < size.x - 8; i += 8)
            //    {
            //        for (int j = 0; j < size.y - 8; j += 8)
            //        {
            //            dct.CopyToMatrix(ref B, i, j, (int)size.y);
            //            dct.DCT(k);
            //        }
            //    }
            //    GlobalMembers.frec.file.Dispose();


            //    GlobalMembers.frec = new Frecuency($"frec {k}.txt");
            //    string[] st = GlobalMembers.frec.reader.ReadLine().Split(' ');
            //     size = new МатКлассы.Point(Convert.ToDouble(st[0]), Convert.ToDouble(st[1]));
            //    Bitmap B2 = new Bitmap((int)size.x, (int)size.y);
            //    for (int i = 0; i < size.x - 8; i += 8)
            //    {
            //        for (int j = 0; j < size.y - 8; j += 8)
            //        {
            //            dct.MatrixFromFrec();
            //            dct.IDCT();
            //            dct.CopyOutOfMatrix(ref B2, i, j, (int)size.y);
            //        }
            //    }

            //    GlobalMembers.CompressBitMap = new Bitmap(B2);
            //    string FileName = "weightfile";
            //    B2.Save(FileName);
            //    FileInfo file = new FileInfo(FileName);
            //    string weight = file.Length.ToString();
            //    GlobalMembers.CompressBitMap.Save($"Итог (k={k} weight = {weight}).bmp");
            //}

        }

        public enum Method { DCT,SVD};
        private static Bitmap Cut(Bitmap a) => new Bitmap(a, new Size(a.Width - a.Width % 8, a.Height - a.Height % 8));
        

        public static long WeightBitmap(Bitmap B)
        {
            string FileName = "weightfile";
            B.Save(FileName);
            FileInfo file = new FileInfo(FileName);
            return file.Length;
        }

        public static void Compression(Bitmap Image, int begk = 3, int endk = 10)
        {
            GlobalMembers.BitMap = new Bitmap(Cut(Image));
            МатКлассы.Point size = new МатКлассы.Point(GlobalMembers.BitMap.Width, GlobalMembers.BitMap.Height);

            DCT.Transform dct = new DCT.Transform();
            for (int k = begk; k <= endk; k++)
            {
                GlobalMembers.frec = new Frecuency(10.0, -1000.0, 1000.0, $"frec {k}.txt");
                Bitmap B = new Bitmap(GlobalMembers.BitMap);
                for (int i = 0; i < size.x - 8; i += 8)
                {
                    for (int j = 0; j < size.y - 8; j += 8)
                    {
                        dct.CopyToMatrix(ref B, i, j, (int)size.y);
                        dct.DCT(k);
                        dct.IDCT();
                        dct.CopyOutOfMatrix(ref B, i, j, (int)size.y);
                    }
                }
                GlobalMembers.frec.file.Dispose();

                GlobalMembers.CompressBitMap = new Bitmap(B);
                string FileName = "weightfile";
                B.Save(FileName);
                FileInfo file = new FileInfo(FileName);
                string weight = file.Length.ToString();
                GlobalMembers.CompressBitMap.Save($"Итог (k={k} weight = {weight}).bmp");
            }
        }
        public static Bitmap Compression(Bitmap Image, int k = 5)
        {
            GlobalMembers.BitMap = new Bitmap(Cut(Image));
            МатКлассы.Point size = new МатКлассы.Point(GlobalMembers.BitMap.Width, GlobalMembers.BitMap.Height);

            DCT.Transform dct = new DCT.Transform();
            GlobalMembers.frec = new Frecuency(10.0, -1000.0, 1000.0, $"frec {k}.txt");
            Bitmap B = new Bitmap(GlobalMembers.BitMap);
            for (int i = 0; i < size.x - 8; i += 8)
            {
                for (int j = 0; j < size.y - 8; j += 8)
                {
                    dct.CopyToMatrix(ref B, i, j, (int)size.y);
                    dct.DCT(k);
                    dct.IDCT();
                    dct.CopyOutOfMatrix(ref B, i, j, (int)size.y);
                }
            }
            GlobalMembers.frec.file.Dispose();

            GlobalMembers.CompressBitMap = new Bitmap(B);
            string FileName = "weightfile";
            B.Save(FileName);
            FileInfo file = new FileInfo(FileName);
            string weight = file.Length.ToString();
            GlobalMembers.CompressBitMap.Save($"Итог (k={k} weight = {weight}).bmp");
            return GlobalMembers.CompressBitMap;

        }

        public static void BitmapToFile(Bitmap bitmap, string filename, int k = 5,bool lines=false)
        {
            GlobalMembers.BitMap = new Bitmap(Cut(bitmap));
            МатКлассы.Point size = new МатКлассы.Point(GlobalMembers.BitMap.Width, GlobalMembers.BitMap.Height);
            DCT.Transform dct = new DCT.Transform();
            GlobalMembers.frec = new Frecuency(10.0, -1000.0, 1000.0, filename);
            Bitmap B = new Bitmap(GlobalMembers.BitMap);
            GlobalMembers.frec.file.WriteLine($"{size.x} {size.y}");
            if(!lines)
            for (int i = 0; i < size.x - 8; i += 8)
            {
                for (int j = 0; j < size.y - 8; j += 8)
                {
                    dct.CopyToMatrix(ref B, i, j, (int)size.y);
                    dct.DCT(k);
                }
            }
            else
            {
                int wi, h;
                var mas = Vectors.ToDoubleMas(new Vectors(ImageToFileWithHoffmanAndBack.ImageToMasArray(B,out wi,out h))*255);
                for(int i=0;i<wi*h;i+=64)
                {
                    double[] tmp = new double[64];
                    for (int j = 0; j < 64; j++)
                        tmp[j] = mas[i + j];
                    dct.Matrix = new SqMatrix(tmp);
                    dct.DCT(k);
                }
            }

            GlobalMembers.frec.file.Dispose();
        }

        public static Bitmap FileToBitmap(string filename, bool lines = false)
        {
            DCT.Transform dct = new DCT.Transform();
            GlobalMembers.frec = new Frecuency(filename);
            string[] st = GlobalMembers.frec.reader.ReadLine().Split(' ');
            МатКлассы.Point size = new МатКлассы.Point(Convert.ToDouble(st[0]), Convert.ToDouble(st[1]));
            Bitmap B2 = new Bitmap((int)size.x, (int)size.y);
            if(!lines)
            for (int i = 0; i < size.x - 8; i += 8)
            {
                for (int j = 0; j < size.y - 8; j += 8)
                {
                    dct.MatrixFromFrec();
                    dct.IDCT();
                    dct.CopyOutOfMatrix(ref B2, i, j, (int)size.y);
                }
            }
            else
            {
                List<double> tmp = new List<double>();
                for(int i=0;i<size.x*size.y;i+=64)
                {
                    dct.MatrixFromFrec();
                    dct.IDCT();
                    double[] mas = SqMatrix.ToDoubleMas(dct.Matrix);
                    tmp.AddRange(mas);
                }
                int tmpp = 0;
                for (int i = 0; i < B2.Height; i++)
                    for (int j = 0; j < B2.Width; j++)
                    {
                        int s = (int)Math.Floor(tmp[tmpp++]); s = Math.Max(s, 0); s = Math.Min(s, 255);
                        Color r = Color.FromArgb(s, Color.White);
                        B2.SetPixel(j, i, Color.FromArgb(s, r));
                    }
                        
            }
            GlobalMembers.CompressBitMap = new Bitmap(B2);
            string FileName = "weightfile";
            B2.Save(FileName);
            FileInfo file = new FileInfo(FileName);
            string weight = file.Length.ToString();
            GlobalMembers.CompressBitMap.Save($"Итог (weight = {weight}).bmp");
            GlobalMembers.frec.reader.Close();
            return GlobalMembers.CompressBitMap;
        }

        public static void SVDBitmapToFile(Bitmap bitmap, string filename, bool lines=false)
        {
            GlobalMembers.BitMap = new Bitmap(Cut(bitmap));
            МатКлассы.Point size = new МатКлассы.Point(GlobalMembers.BitMap.Width, GlobalMembers.BitMap.Height);
            DCT.Transform dct = new DCT.Transform();
            Bitmap B = new Bitmap(GlobalMembers.BitMap);

            StreamWriter uf = new StreamWriter("U.txt"), wf = new StreamWriter(filename), vtf = new StreamWriter("VT.txt");
            Matrix A, U, VT;double[] w;

            wf.WriteLine($"{size.x} {size.y}");


            if (!lines)
                for (int i = 0; i < size.x - 8; i += 8)
                {
                    for (int j = 0; j < size.y - 8; j += 8)
                    {
                        dct.CopyToMatrix(ref B, i, j, (int)size.y);
                        A = dct.Matrix;
                        Matrix.SVD(A, out U, out w, out VT);
                        MasInFile(wf,w);SquareMatrixInFile(uf, U);SquareMatrixInFile(vtf, VT);
                    }
                }
            else
            {
                int wi, h;
                var mas = Vectors.ToDoubleMas(new Vectors(ImageToFileWithHoffmanAndBack.ImageToMasArray(B, out wi, out h)) * 255);
                for (int i = 0; i < wi * h ; i += 64)
                {
                    double[] tmp = new double[64];
                    for (int j = 0; j < 64; j++)
                        tmp[j] = mas[i + j];
                    dct.Matrix = new SqMatrix(tmp);
                    A = dct.Matrix;
                    Matrix.SVD(A, out U, out w, out VT);
                    MasInFile(wf, w); SquareMatrixInFile(uf, U); SquareMatrixInFile(vtf, VT);
                }
            }

            wf.Close();uf.Close();vtf.Close();
        }
        public static Bitmap SVDFileToBitmap(string filename, bool lines = false)
        {
            DCT.Transform dct = new DCT.Transform();
            StreamReader wf = new StreamReader(filename), uf = new StreamReader("U.txt"), vtf = new StreamReader("VT.txt");
            string[] st = wf.ReadLine().Split(' ');
            МатКлассы.Point size = new МатКлассы.Point(Convert.ToDouble(st[0]), Convert.ToDouble(st[1]));
            Bitmap B2 = new Bitmap((int)size.x, (int)size.y);
            if (!lines)
                for (int i = 0; i < size.x - 8; i += 8)
                {
                    for (int j = 0; j < size.y - 8; j += 8)
                    {
                        Matrix U = MatrixFromFile(uf), VT = MatrixFromFile(vtf),E=new Matrix(MasFromFile(wf),U.RowCount,VT.RowCount);
                        dct.Matrix=new SqMatrix(U*E*VT);
                        for (int t = 0; t < 8; t++)
                            for (int y = 0; y < 8; y++)
                                if (dct.Matrix[t, y] < 0) dct.Matrix[t, y] = 0;

                        dct.CopyOutOfMatrix(ref B2, i, j, (int)size.y);
                    }
                }
            else
            {
                List<double> tmp = new List<double>();
                for (int i = 0; i < size.x * size.y; i += 64)
                {
                    Matrix U = MatrixFromFile(uf), VT = MatrixFromFile(vtf), E = new Matrix(MasFromFile(wf), U.RowCount, VT.RowCount);
                    dct.Matrix = new SqMatrix(U * E * VT);
                    double[] mas = SqMatrix.ToDoubleMas(dct.Matrix);
                    tmp.AddRange(mas);
                }
                int tmpp = 0;
                for (int i = 0; i < B2.Height; i++)
                    for (int j = 0; j < B2.Width; j++)
                    {
                        int s = (int)Math.Floor(tmp[tmpp++]); s = Math.Max(s, 0); s = Math.Min(s, 255);
                        Color r = Color.FromArgb(s, Color.White);
                        B2.SetPixel(j, i, Color.FromArgb(s, r));
                    }

            }
            GlobalMembers.CompressBitMap = new Bitmap(B2);
            string FileName = "weightfile";
            B2.Save(FileName);
            FileInfo file = new FileInfo(FileName);
            string weight = file.Length.ToString();
            GlobalMembers.CompressBitMap.Save($"Итог (weight = {weight}).bmp");

            wf.Close();uf.Close();vtf.Close();
            return GlobalMembers.CompressBitMap;
        }

        private static  void MasInFile(StreamWriter s,double[] mas)
        {
            for (int i = 0; i < mas.Length; i++)
                s.Write(mas[i] + " ");
            s.WriteLine();
        }
        private static void SquareMatrixInFile(StreamWriter s,Matrix m)
        {
            for(int i=0;i<m.RowCount;i++)
            {
                for (int j = 0; j < m.ColCount; j++)
                    s.Write(m[i, j] + " ");
                s.WriteLine();
            }
        }
        private static double[] MasFromFile(StreamReader s)
        {
            string[] st = s.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            double[] res = new double[st.Length];
            for (int i = 0; i < res.Length; i++)
                res[i] = Convert.ToDouble(st[i]);
            return res;
        }
        private static Matrix MatrixFromFile(StreamReader s)
        {
            string[] st = s.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Matrix res = new Matrix(st.Length, st.Length);
            for (int i = 0; i < st.Length; i++)
                res[0,i] = Convert.ToDouble(st[i]);
            for(int i=1;i<st.Length;i++)
            {
                st = s.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for(int j=0;j<st.Length;j++)
                    res[i,j] = Convert.ToDouble(st[i]);
            }

            return res;     
        }

        public static long FILEweight(string filename)
        {
            FileInfo f= new FileInfo(filename);
            return f.Length;
        }
    }

    namespace DCT
    {
        public class Transform
        {
            public SqMatrix Matrix = new SqMatrix(8);
            public void DCT(int par = 4)
            {
                //Matrix.PrintMatrix();
                //MyMath.matrix_mul(MatrixForDCT.M, Matrix, MatrixForDCT.TEMP, 8, 8, 8);
                // MyMath.matrix_mul(MatrixForDCT.TEMP, MatrixForDCT.MT, Matrix, 8, 8, 8);
                Matrix = Matrix.ConvertToSimilar(MatrixForDCT.M, true); //Matrix.PrintMatrix();
                Kvant(par); //Matrix.PrintMatrix();              
            }
            public void IDCT()
            {
                //MyMath.matrix_mul(MatrixForDCT.MT, Matrix, MatrixForDCT.TEMP, 8, 8, 8);
                //MyMath.matrix_mul(MatrixForDCT.TEMP, MatrixForDCT.M, Matrix, 8, 8, 8);
                Matrix = Matrix.ConvertToSimilar(MatrixForDCT.M, false);
                //Matrix.PrintMatrix();"".Show();
            }

            public void MatrixFromFrec()
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Matrix[i, j] = Convert.ToDouble(GlobalMembers.frec.reader.ReadLine());
                    }
                }
            }

            public void CopyToMatrix(ref Bitmap BitMap, int x, int y, int N)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Matrix[i, j] = 255 * BitMap.GetPixel(x + i, y + j).GetBrightness();
                    }
                }
                //Matrix.PrintMatrix();"".Show();
            }
            public void CopyOutOfMatrix(ref Bitmap BitMap, int x, int y, int N)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        //int s = (int)(255*Math.Min(Matrix[i, j].Abs(),1));//s.Show();

                        //double m = Matrix.Min;
                        //double tmp = (m<0)? m.Abs() : 0;
                        //int s = (int)(255 * Math.Min(Matrix[i, j]+tmp, 1));//s.Show()

                        //double m = Matrix.Min;
                        //double tmp = (m < 0) ? m.Abs() : 0;
                        //double l = 1 + tmp;
                        //int s = (int)(255 * Math.Min(1 - (1 - Matrix[i, j]) / l, 1));//s.Show()

                        //double m = Matrix.Min;
                        //double tmp = (m < 0) ? m.Abs() : 0;
                        //double l = Matrix.Max+tmp;
                        //int s = (int)(255 *Math.Min((Matrix[i,j]+tmp)/l, 1));//s.Show()

                        //int s = (int)(255 * Matrix[i, j]);
                        int s = (int)Math.Floor(Matrix[i, j]); s = Math.Max(s, 0); s = Math.Min(s, 255);
                        Color r = Color.FromArgb(s, Color.White);
                        BitMap.SetPixel((x + i), y + j, r);
                        //*(BitMap +  * N + ) = ;
                    }
                }

            }
            private void Kvant(int par = 4)
            {
                //Matrix *= MatrixForDCT.KvantMatrix;

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Matrix[i, j] = MyMath.round(Matrix[i, j], 10.0);
                        if ((i + j) > par)
                        {
                            Matrix[i, j] = 0.0;
                        }

                        //GlobalMembers.frec.list.Add(Matrix[i,j]);
                        GlobalMembers.frec.file.WriteLine(Matrix[i, j]);
                    }
                }
            }
        }

        public static class MatrixForDCT
        {
            public static SqMatrix M = new SqMatrix(8);
            public static SqMatrix MT = new SqMatrix(8);
            public static SqMatrix TEMP = new SqMatrix(8);
            public static SqMatrix KvantMatrix = new SqMatrix(8);
            public static double KvantR = 4;

            static MatrixForDCT()
            {
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                        KvantMatrix[i, j] = 1 + KvantR * (i + j);

                for (int i = 0; i < 8; i++)
                {
                    M[0, i] = 1 / Math.Sqrt(8.0);
                }
                for (int i = 1; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        M[i, j] = 0.5 * Math.Cos(Math.PI / 16.0 * i * (2 * j + 1));
                    }
                }

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        MT[i, j] = M[j, i];
                    }
                }
            }
        }

    }


    public class Frecuency
    {
        public Frecuency(double dKvant, double dLow, double dHigh, string pcOutputFileName)
        {
            this.dKvant = dKvant;
            this.dLow = dLow;
            this.size = (int)((dHigh - dLow) / dKvant) + 1;
            this.pdFrec = new double[size];
            for (int i = 0; i < size; i++)
            {
                pdFrec[i] = 0.0;
            }

            file = new StreamWriter(pcOutputFileName);
        }
        public Frecuency(string s)
        {
            this.reader = new StreamReader(s);
        }

        //public void Dispose()
        //{
        //    double dSum = 0.0;
        //    double dMat = 0.0;

        //    for (int i = 0; i < size; i++)
        //    {
        //        file.WriteLine(pdFrec[i]);
        //        dSum += pdFrec[i];
        //    }
        //    for (int i = 0; i < size; i++)
        //    {
        //        pdFrec[i] /= dSum;
        //        if (pdFrec[i] != 0)
        //        {
        //            dMat -= pdFrec[i] * Math.Log(pdFrec[i]);
        //        }
        //    }
        //    file.WriteLine("\n\n%f", dMat);
        //    file.Close();
        //}

        //public void AddElement(double x)
        //{
        //    int ix = (int)((x - dLow) / dKvant);
        //    pdFrec[ix] += 1.0;
        //}

        private double dKvant;
        private double dLow;
        private int size;
        public StreamWriter file;
        public StreamReader reader;
        private double[] pdFrec;
        public List<double> list = new List<double>();
    }

    public static class GlobalMembers
    {
        public static Frecuency frec = new Frecuency(10.0, -1000.0, 1000.0, "frec.txt");
        public static Bitmap BitMap, CompressBitMap; //Рисунок
    }

    public static class MyMath
    {
        public static double round(double x, double k = 0.0)
        {
            //return ((int)(x / k)) * k;
            return Math.Floor(x /** 255*/ / k) * k /*/ 250*/;
        }

        public static void matrix_mul(double[] A, double[] B, double[] RES, int N, int M, int K)
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < K; j++)
                {
                    RES[i * K + j] = 0.0;
                    for (int t = 0; t < M; t++)
                    {
                        RES[i * K + j] += A[i * M + t] * B[t * K + j];
                    }
                }
            }
        }

    }

    /// <summary>
    /// Для преобразования изображения в файл через кодирование Хаффмана и обратно
    /// </summary>
    public static class ImageToFileWithHoffmanAndBack
    {
        private static long bitweight;

        /// <summary>
        /// Перевод изображения в массив яркостей
        /// </summary>
        /// <param name="im"></param>
        /// <returns></returns>
        public static double[] ImageToMasArray(Bitmap im, out int width, out int hei)
        {
            width = im.Width; hei = im.Height;
            double[] res = new double[width * hei];
            for (int i = 0; i < hei; i++)
                for (int j = 0; j < width; j++)
                    res[(i) * width + j] = im.GetPixel(j, i).GetBrightness();
            return res;
        }
        /// <summary>
        /// Кодирование массива яркости в файл с параллельной записью нужных для расшифровки таблиц
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="masimage"></param>
        private static void CreatFileWithCodeDataImage(string filename, double[] masimage, int wid, int hei)
        {
            string filein = Coding.HoffmanNumberEncode(masimage);
            var TableWordToByte = Coding.HofTable;
            var TableDoubleToChar = Coding.DoubleCharTable;

            StreamWriter size = new StreamWriter("Size.txt");
            size.WriteLine($"{wid} {hei}");
            size.Close();

            //var bits = Coding.StringToBitMas(filein);
            bitweight = filein.Where(e => e != ' ').Count();

            StreamWriter fs = new StreamWriter(filename /*+ ".txt"*/);
            fs.Write(filein);
            //for (int i = 0; i < bits.Length; i++)
            //{
            //    for(int j=0;j<bits[i].Length;j++)
            //    fs.Write(bits[i][j]);
            //    fs.WriteLine();
            //}
            fs.Close();
            //забить в файл таблицы

            StreamWriter wtb = new StreamWriter("CharString.txt");
            for (int i = 0; i < TableWordToByte.Count; i++)
                wtb.WriteLine($"{TableWordToByte.SymbolList[i].Item1} -> {TableWordToByte.SymbolList[i].Item2}");
            wtb.Close();

            StreamWriter dct = new StreamWriter("DoubleChar.txt");
            for (int i = 0; i < TableDoubleToChar.Count; i++)
                dct.WriteLine($"{TableDoubleToChar[i].Item1} -> {TableDoubleToChar[i].Item2}");
            dct.Close();
        }

        /// <summary>
        /// Воссоздание изображения по файлу кодированных яркостей и файлам таблиц
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="HofTablefile"></param>
        /// <param name="DoubleCharfile"></param>
        /// <returns></returns>
        private static Bitmap CreatImageByFiles(string filename)
        {
            int width, height;

            StreamReader size = new StreamReader("Size.txt");
            string[] st = size.ReadLine().Split(' ');
            width = Convert.ToInt32(st[0]); height = Convert.ToInt32(st[1]);
            size.Close();

            //считывание кодированных яркостей
            StreamReader fs = new StreamReader(filename);
            string code = fs.ReadLine();
            fs.Close();

            //считывание таблицы Hof
            StreamReader hf = new StreamReader("CharString.txt");
            Coding.HofTable.SymbolList = new List<Tuple<char, string>>();
            string s = hf.ReadLine();
            while (s != null)
            {
                st = s.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries);
                Coding.HofTable.SymbolList.Add(new Tuple<char, string>(st[0][0], st[1]));
                s = hf.ReadLine();//if (s == "") break;
            }
            hf.Close();

            //считывание второй таблицы кодирования
            StreamReader dc = new StreamReader("DoubleChar.txt");
            Coding.DoubleCharTable = new List<Tuple<double, char>>();
            s = dc.ReadLine();
            while (s != null)
            {
                st = s.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries);
                Coding.DoubleCharTable.Add(new Tuple<double, char>(Convert.ToDouble(st[0]), st[1][0]));
                s = dc.ReadLine();
            }
            dc.Close();
            "+Нужные файлы считаны".Show();

            //расшифровка и генерация изображения
            double[] mas = Coding.HoffmanNunderDecode(code); "+Расшифровка выполнена".Show();
            int tmp = 0;
            Bitmap res = new Bitmap(width, height);
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    res.SetPixel(j, i, Color.FromArgb((int)(mas[tmp++] * 255), Color.White));

            return res;
        }

        /// <summary>
        /// Создание по изображению файла данных
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="filename"></param>
        public static void CompressionEncode(Bitmap bitmap, string filename)
        {
            int w, h;
            double[] mas = ImageToMasArray(bitmap, out w, out h); "+Перевод изображения в массив чисел".Show();
            CreatFileWithCodeDataImage(filename, mas, w, h); "+Создан файл с кодировкой".Show();

            LastBitmapWeight = Сжатие.WeightBitmap(bitmap);
            FileInfo f1 = new FileInfo(filename), f2 = new FileInfo("CharString.txt"), f3 = new FileInfo("DoubleChar.txt"), f4 = new FileInfo("Size.txt");
            LastFilesWeight1 = f1.Length + f2.Length + f3.Length + f4.Length;
            LastFilesWeight2 = bitweight / 8 + f2.Length + f3.Length + f4.Length;
            "Посчитаны веса файлов".Show();
        }
        /// <summary>
        /// Воссоздание изображения по файлу данных
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static Bitmap CompressionDecode(string filename) => CreatImageByFiles(filename);

        /// <summary>
        /// Размер последнего изображения
        /// </summary>
        public static long LastBitmapWeight;
        /// <summary>
        /// Суммарный размер нужных для воссоздания изображения файлов (для последнего изображения)
        /// </summary>
        public static long LastFilesWeight1, LastFilesWeight2;
    }
}

