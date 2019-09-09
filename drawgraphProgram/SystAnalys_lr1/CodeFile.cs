using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using МатКлассы;
using static МатКлассы.Graphs;
using static МатКлассы.Point;

namespace SystAnalys_lr1
{
    //class Vertex
    //{
    //    public int x, y;

    //    public Vertex(int x, int y)
    //    {
    //        this.x = x;
    //        this.y = y;
    //    }
    //}

    //class Edge
    //{
    //    public int v1, v2;

    //    public Edge(int v1, int v2)
    //    {
    //        this.v1 = v1;
    //        this.v2 = v2;
    //    }
    //}

    class DrawGraph
    {
        Bitmap bitmap;
        Pen blackPen;
        Pen redPen;
        Pen darkGoldPen;
        Graphics gr;
        Font fo;
        Brush br;
        PointF point;
        public int R = 20; //радиус окружности вершины
        

        public DrawGraph(int width, int height)
        {
            bitmap = new Bitmap(width, height);
            gr = Graphics.FromImage(bitmap);
            clearSheet();
            blackPen = new Pen(Color.Black);
            blackPen.Width = 2;
            redPen = new Pen(Color.Red);
            redPen.Width = 2;
            darkGoldPen = new Pen(Color.DarkGoldenrod);
            darkGoldPen.Width = 2;
            fo = new Font("Consolas", 16);
            br = Brushes.Black;
        }

        public Bitmap GetBitmap()
        {
            return bitmap;
        }

        public void clearSheet()
        {
            gr.Clear(Color.White);
        }

        public void drawVertex(int x, int y, string number, int color=0)
        {
            Brush col= Brushes.LawnGreen;
            if (color != 0)
            {
                //Color c = Color.FromArgb(color+10,Color.Green);
                int r = Math.Abs(250 - 70 * color)%250;
                int g = Math.Abs(60 * color) % 250;
                int b = Math.Abs(50 + 20 * color) % 250;

                Color c = Color.FromArgb(r,g,b);
                SolidBrush myBrush = new SolidBrush(c);
                col = myBrush;
            }

            //gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.GammaCorrected;
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            gr.FillEllipse(col, (x - R), (y - R), 2 * R, 2 * R);
            gr.DrawEllipse(blackPen, (x - R), (y - R), 2 * R, 2 * R);
            point = new PointF(x - ((int.Parse(number)<10)?10:14), y - 12);
            gr.DrawString(number, fo, br, point);
        }

        public void drawSelectedVertex(int x, int y)
        {
            gr.DrawEllipse(redPen, (x - R), (y - R), 2 * R, 2 * R);
        }

        public void drawEdge(Vertex V1, Vertex V2, Edge E, int numberE)
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            
            if (E.v1 == E.v2)
            {
                int radius = 2 * R;
                int tmp = r.Next((int)(1.7*R), 2*R);
                int h =(int) Math.Sqrt(radius * radius - tmp * tmp);
                gr.DrawArc(darkGoldPen, (V1.x - tmp), (V1.y - h), radius, radius, 90, /*270*/360);
                point = new PointF(V1.x - (int)(2.75 * R), V1.y - (int)(2.75 * R));
                string s = (numberE + 1).ToString() + "(" + ((char)('a' + numberE)).ToString() + ")";
                gr.DrawString(s, fo, Brushes.Red, point);
                drawVertex(V1.x, V1.y, (E.v1 + 1).ToString());
            }
            else
            {
                PointF[] p = new PointF[3];
                double d = Math.Sqrt((V1.x - V2.x) * (V1.x - V2.x) + (V1.y - V2.y) * (V1.y - V2.y));
                int hx =(int)d/4, hy =(d<10*R)? (int)d / 4:(int)R / 7;
                
                p[0] = new PointF(V1.x, V1.y);
                p[1] = new PointF((V1.x + V2.x) / 2+r.Next(-hx,hx ), (V1.y + V2.y) / 2+r.Next(-hy, hy));
                p[2] = new PointF(V2.x, V2.y);

                gr.DrawCurve(darkGoldPen,p);
                //gr.DrawLine(darkGoldPen, V1.x, V1.y, V2.x, V2.y);
                //point = new PointF(/*(V1.x + V2.x) / 2, (V1.y + V2.y) / 2*/);
                string s = (numberE + 1).ToString() + "(" + ((char)('a' + numberE)).ToString() + ")";
                gr.DrawString(s, fo, Brushes.Red, /*point*/p[1]);
                drawVertex(V1.x, V1.y, (E.v1 + 1).ToString());
                drawVertex(V2.x, V2.y, (E.v2 + 1).ToString());
            }
        }

        /// <summary>
        /// Изобразить этот граф
        /// </summary>
        /// <param name="r"></param>
        public void drawThisGraph(Graphs r)
        {
            //List<Vertex> V = new List<Vertex>(r.Ver);
            //List<Edge> E = new List<Edge>(r.Ed);
            drawALLGraph(r.Ver, r.Ed);
        }

        public void drawALLGraph(List<Vertex> V, List<Edge> E)
        {
            //рисуем ребра
            for (int i = 0; i < E.Count; i++)
            {
                if (E[i].v1 == E[i].v2)
                {
                    gr.DrawArc(darkGoldPen, (V[E[i].v1].x - 2 * R), (V[E[i].v1].y - 2 * R), 2 * R, 2 * R, 90, 270);
                    point = new PointF(V[E[i].v1].x - (int)(2.75 * R), V[E[i].v1].y - (int)(2.75 * R));
                    gr.DrawString(((char)('a' + i)).ToString(), fo, br, point);
                }
                else
                {
                    gr.DrawLine(darkGoldPen, V[E[i].v1].x, V[E[i].v1].y, V[E[i].v2].x, V[E[i].v2].y);
                    point = new PointF((V[E[i].v1].x + V[E[i].v2].x) / 2, (V[E[i].v1].y + V[E[i].v2].y) / 2);
                    gr.DrawString(((char)('a' + i)).ToString(), fo, br, point);
                }
            }
            //рисуем вершины
            for (int i = 0; i < V.Count; i++)
            {
                drawVertex(V[i].x, V[i].y, (i + 1).ToString(),V[i].color);
            }
        }

        //заполняет матрицу смежности
        public void fillAdjacencyMatrix(int numberV, List<Edge> E, int[,] matrix)
        {
            for (int i = 0; i < numberV; i++)
                for (int j = 0; j < numberV; j++)
                    matrix[i, j] = 0;
            for (int i = 0; i < E.Count; i++)
            {
                matrix[E[i].v1, E[i].v2] = 1;
                matrix[E[i].v2, E[i].v1] = 1;
            }
        }

        //заполняет матрицу инцидентности
        public void fillIncidenceMatrix(int numberV, List<Edge> E, int[,] matrix)
        {
            for (int i = 0; i < numberV; i++)
                for (int j = 0; j < E.Count; j++)
                    matrix[i, j] = 0;
            for (int i = 0; i < E.Count; i++)
            {
                matrix[E[i].v1, i] = 1;
                matrix[E[i].v2, i] = 1;
            }
        }

        
    }
}