using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using МатКлассы2018;
using static МатКлассы2018.Graphs;

namespace SystAnalys_lr1
{
    public partial class Form1 : Form
    {
        DrawGraph G;
        List<Vertex> V;
        List<Edge> E;
        List<Vertex> Vtmp;
        List<Edge> Etmp;
        int[,] AMatrix; //матрица смежности
        int[,] IMatrix; //матрица инцидентности
        public Graphs g;//исходный граф

        int selected1; //выбранные вершины, для соединения линиями
        int selected2;
        bool save = false;

        public Form1()
        {
            InitializeComponent();
            textBox1.Hide();
            V = new List<Vertex>();
            G = new DrawGraph(sheet.Width, sheet.Height);
            E = new List<Edge>();
            Vtmp = V;
            Etmp = E;
            sheet.Image = G.GetBitmap();
            //label2.RenderTransform = new RotateTransform(90);

            ToolTip toolTip1 = new ToolTip();

            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 400;
            toolTip1.ShowAlways = true;

            toolTip1.SetToolTip(this.button1, "Узнать основные характеристики графа");
            toolTip1.SetToolTip(this.selectButton, "Узнать степень вершины графа, указав на неё");
            toolTip1.SetToolTip(this.drawVertexButton, "Рисовать вершины графа");
            toolTip1.SetToolTip(this.drawEdgeButton, "Рисовать рёбра графа");
            toolTip1.SetToolTip(this.deleteButton, "Выбрать компонент графа, который требуется удалить");
            toolTip1.SetToolTip(this.deleteALLButton, "Удалить граф и очистить все панели");
            toolTip1.SetToolTip(this.button3, "Найти хроматический полином графа");
            toolTip1.SetToolTip(this.button6, "Вывести метрические характеристики графа");
            toolTip1.SetToolTip(this.buttonAdj, "Вывести матрицу смежности");
            toolTip1.SetToolTip(this.buttonInc, "Вывести матрицу инцидентности");
            toolTip1.SetToolTip(this.chainButton, "Вывести все простые цепи");
            toolTip1.SetToolTip(this.cycleButton, "Вывести все циклы");
        }

        //кнопка - выбрать вершину
        private void selectButton_Click(object sender, EventArgs e)
        {
            selectButton.Enabled = false;
            drawVertexButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            deleteButton.Enabled = true;
            //G.clearSheet();
            //G.drawALLGraph(V, E);
            //sheet.Image = G.GetBitmap();
            selected1 = -1;
        }

        //кнопка - рисовать вершину
        private void drawVertexButton_Click(object sender, EventArgs e)
        {
            drawVertexButton.Enabled = false;
            selectButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            deleteButton.Enabled = true;
            //G.clearSheet();
            //G.drawALLGraph(V, E);
            //sheet.Image = G.GetBitmap();
            save = false;
        }

        //кнопка - рисовать ребро
        private void drawEdgeButton_Click(object sender, EventArgs e)
        {
            drawEdgeButton.Enabled = false;
            selectButton.Enabled = true;
            drawVertexButton.Enabled = true;
            deleteButton.Enabled = true;
            //G.clearSheet();
            //G.drawALLGraph(V, E);
            //sheet.Image = G.GetBitmap();
            selected1 = -1;
            selected2 = -1;
            save = false;
        }

        //кнопка - удалить элемент
        private void deleteButton_Click(object sender, EventArgs e)
        {
            deleteButton.Enabled = false;
            selectButton.Enabled = true;
            drawVertexButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            G.clearSheet();
            G.drawALLGraph(V, E);
            sheet.Image = G.GetBitmap();
            save = false;
        }

        //кнопка - удалить граф
        private void deleteALLButton_Click(object sender, EventArgs e)
        {
            selectButton.Enabled = true;
            drawVertexButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            deleteButton.Enabled = true;
            const string message = "Вы действительно хотите полностью удалить граф?";
            const string caption = "Удаление";
            var MBSave = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (MBSave == DialogResult.Yes)
            {
                V.Clear();
                E.Clear();
                G.clearSheet();
                sheet.Image = G.GetBitmap();
                g = new Graphs(0);
            }
            listBoxMatrix.Items.Clear();
            save = false;
        }

        //кнопка - матрица смежности
        private void buttonAdj_Click(object sender, EventArgs e)
        {
            listBoxMatrix.Items.Clear();
            
            createAdjAndOut();
        }

        //кнопка - матрица инцидентности 
        private void buttonInc_Click(object sender, EventArgs e)
        {
            listBoxMatrix.Items.Clear();
            createIncAndOut();
        }

        private void sheet_MouseClick(object sender, MouseEventArgs e)
        {
            //нажата кнопка "выбрать вершину", ищем степень вершины
            if (selectButton.Enabled == false)
            {
                for (int i = 0; i < V.Count; i++)
                {
                    if (Math.Pow((V[i].x - e.X), 2) + Math.Pow((V[i].y - e.Y), 2) <= G.R * G.R)
                    {
                        if (selected1 != -1)
                        {
                            selected1 = -1;
                            G.clearSheet();
                            G.drawALLGraph(V, E);
                            sheet.Image = G.GetBitmap();
                        }
                        if (selected1 == -1)
                        {
                            G.drawSelectedVertex(V[i].x, V[i].y);
                            selected1 = i;
                            sheet.Image = G.GetBitmap();
                            createAdjAndOut();
                            listBoxMatrix.Items.Clear();
                            int degree = 0;
                            for (int j = 0; j < V.Count; j++)
                                degree += AMatrix[selected1, j];
                            listBoxMatrix.Items.Add("Степень вершины №" + (selected1 + 1) + " равна " + degree);
                            break;
                        }
                    }
                }
            }
            //нажата кнопка "рисовать вершину"
            if (drawVertexButton.Enabled == false)
            {
                V.Add(new Vertex(e.X, e.Y));
                G.drawVertex(e.X, e.Y, V.Count.ToString());
                sheet.Image = G.GetBitmap();
            }
            //нажата кнопка "рисовать ребро"
            if (drawEdgeButton.Enabled == false)
            {
                if (e.Button == MouseButtons.Left)
                {
                    for (int i = 0; i < V.Count; i++)
                    {
                        if (Math.Pow((V[i].x - e.X), 2) + Math.Pow((V[i].y - e.Y), 2) <= G.R * G.R)
                        {
                            if (selected1 == -1)
                            {
                                G.drawSelectedVertex(V[i].x, V[i].y);
                                selected1 = i;
                                sheet.Image = G.GetBitmap();
                                break;
                            }
                            if (selected2 == -1)
                            {
                                G.drawSelectedVertex(V[i].x, V[i].y);
                                selected2 = i;
                                E.Add(new Edge(selected1, selected2));
                                G.drawEdge(V[selected1], V[selected2], E[E.Count - 1], E.Count - 1);
                                selected1 = -1;
                                selected2 = -1;
                                sheet.Image = G.GetBitmap();
                                break;
                            }
                        }
                    }
                }
                if (e.Button == MouseButtons.Right)
                {
                    if ((selected1 != -1) &&
                        (Math.Pow((V[selected1].x - e.X), 2) + Math.Pow((V[selected1].y - e.Y), 2) <= G.R * G.R))
                    {
                        G.drawVertex(V[selected1].x, V[selected1].y, (selected1 + 1).ToString());
                        selected1 = -1;
                        sheet.Image = G.GetBitmap();
                    }
                }
            }
            //нажата кнопка "удалить элемент"
            if (deleteButton.Enabled == false)
            {
                bool flag = false; //удалили ли что-нибудь по ЭТОМУ клику
                //ищем, возможно была нажата вершина
                for (int i = 0; i < V.Count; i++)
                {
                    if (Math.Pow((V[i].x - e.X), 2) + Math.Pow((V[i].y - e.Y), 2) <= G.R * G.R)
                    {
                        for (int j = 0; j < E.Count; j++)
                        {
                            if ((E[j].v1 == i) || (E[j].v2 == i))
                            {
                                E.RemoveAt(j);
                                j--;
                            }
                            else
                            {
                                if (E[j].v1 > i) E[j].v1--;
                                if (E[j].v2 > i) E[j].v2--;
                            }
                        }
                        V.RemoveAt(i);
                        flag = true;
                        break;
                    }
                }
                //ищем, возможно было нажато ребро
                if (!flag)
                {
                    for (int i = 0; i < E.Count; i++)
                    {
                        if (E[i].v1 == E[i].v2) //если это петля
                        {
                            if ((Math.Pow((V[E[i].v1].x - G.R - e.X), 2) + Math.Pow((V[E[i].v1].y - G.R - e.Y), 2) <= ((G.R + 2) * (G.R + 2))) &&
                                (Math.Pow((V[E[i].v1].x - G.R - e.X), 2) + Math.Pow((V[E[i].v1].y - G.R - e.Y), 2) >= ((G.R - 2) * (G.R - 2))))
                            {
                                E.RemoveAt(i);
                                flag = true;
                                break;
                            }
                        }
                        else //не петля
                        {
                            if (((e.X - V[E[i].v1].x) * (V[E[i].v2].y - V[E[i].v1].y) / (V[E[i].v2].x - V[E[i].v1].x) + V[E[i].v1].y) <= (e.Y + 4) &&
                                ((e.X - V[E[i].v1].x) * (V[E[i].v2].y - V[E[i].v1].y) / (V[E[i].v2].x - V[E[i].v1].x) + V[E[i].v1].y) >= (e.Y - 4))
                            {
                                if ((V[E[i].v1].x <= V[E[i].v2].x && V[E[i].v1].x <= e.X && e.X <= V[E[i].v2].x) ||
                                    (V[E[i].v1].x >= V[E[i].v2].x && V[E[i].v1].x >= e.X && e.X >= V[E[i].v2].x))
                                {
                                    E.RemoveAt(i);
                                    flag = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                //если что-то было удалено, то обновляем граф на экране
                if (flag)
                {
                    G.clearSheet();
                    G.drawALLGraph(V, E);
                    sheet.Image = G.GetBitmap();
                }
            }
        }

        //создание матрицы смежности и вывод в листбокс
        private void createAdjAndOut()
        {
            AMatrix = new int[V.Count, V.Count];
            G.fillAdjacencyMatrix(V.Count, E, AMatrix);
            listBoxMatrix.Items.Clear();
            Show0();
            string sOut = "    ";
            for (int i = 0; i < V.Count; i++)
                sOut += (i + 1) + " ";
            listBoxMatrix.Items.Add(sOut);
            for (int i = 0; i < V.Count; i++)
            {
                sOut = (i + 1) + " | ";
                for (int j = 0; j < V.Count; j++)
                    sOut += AMatrix[i, j] + " ";
                listBoxMatrix.Items.Add(sOut);
            }
        }

        //создание матрицы инцидентности и вывод в листбокс
        private void createIncAndOut()
        {
            if (E.Count > 0)
            {
                IMatrix = new int[V.Count, E.Count];
                G.fillIncidenceMatrix(V.Count, E, IMatrix);
                listBoxMatrix.Items.Clear();
                Show0();
                string sOut = "    ";
                for (int i = 0; i < E.Count; i++)
                    sOut += (char)('a' + i) + " ";
                listBoxMatrix.Items.Add(sOut);
                for (int i = 0; i < V.Count; i++)
                {
                    sOut = (i + 1) + " | ";
                    for (int j = 0; j < E.Count; j++)
                        sOut += IMatrix[i, j] + " ";
                    listBoxMatrix.Items.Add(sOut);
                }
            }
            else
                listBoxMatrix.Items.Clear();
        }

        //поиск элементарных цепей
        private void chainButton_Click(object sender, EventArgs e)
        {
            listBoxMatrix.Items.Clear();
            Show0();
            listBoxMatrix.Items.Add("-----------Простые цепи в графе:");
            //1-white 2-black
            int[] color = new int[V.Count];
            for (int i = 0; i < V.Count - 1; i++)
                for (int j = i + 1; j < V.Count; j++)
                {
                    for (int k = 0; k < V.Count; k++)
                        color[k] = 1;
                    DFSchain(i, j, E, color, (i + 1).ToString());
                }
        }

        //обход в глубину. поиск элементарных цепей. (1-white 2-black)
        private void DFSchain(int u, int endV, List<Edge> E, int[] color, string s)
        {
            //вершину не следует перекрашивать, если u == endV (возможно в нее есть несколько путей)
            if (u != endV)  
                color[u] = 2;
            else
            {
                listBoxMatrix.Items.Add(s);
                return;
            }
            for (int w = 0; w < E.Count; w++)
            {
                if (color[E[w].v2] == 1 && E[w].v1 == u)
                {
                    DFSchain(E[w].v2, endV, E, color, s + "-" + (E[w].v2 + 1).ToString());
                    color[E[w].v2] = 1;
                }
                else if (color[E[w].v1] == 1 && E[w].v2 == u)
                {
                    DFSchain(E[w].v1, endV, E, color, s + "-" + (E[w].v1 + 1).ToString());
                    color[E[w].v1] = 1;
                }
            }
        }

        private void Show0()
        {
            listBoxMatrix.Items.Add("ДЕМОНСТРАЦИЯ РАБОТЫ БИБЛИОТЕКИ ГРАФОВ (Дм. ПА.). ВЕРШИНЫ ГРАФА НУМЕРУЮТСЯ, НАЧИНАЯ С 1");
            listBoxMatrix.Items.Add("ОБОЗНАЧЕНИЯ: p - число вершин, q - число рёбер, k - число компонент связности");
            listBoxMatrix.Items.Add("");
            //listBoxMatrix.Items.Add("");
        }

        //поиск элементарных циклов
        private void cycleButton_Click(object sender, EventArgs e)
        {
            listBoxMatrix.Items.Clear();
            Show0();
            listBoxMatrix.Items.Add("-----------Простые циклы в графе:");
            //1-white 2-black
            int[] color = new int[V.Count];
            for (int i = 0; i < V.Count; i++)
            {
                for (int k = 0; k < V.Count; k++)
                    color[k] = 1;
                List<int> cycle = new List<int>();
                cycle.Add(i + 1);
                DFScycle(i, i, E, color, -1, cycle);
            }
        }

        //обход в глубину. поиск элементарных циклов. (1-white 2-black)
        //Вершину, для которой ищем цикл, перекрашивать в черный не будем. Поэтому, для избежания неправильной
        //работы программы, введем переменную unavailableEdge, в которой будет хранится номер ребра, исключаемый
        //из рассмотрения при обходе графа. В действительности это необходимо только на первом уровне рекурсии,
        //чтобы избежать вывода некорректных циклов вида: 1-2-1, при наличии, например, всего двух вершин.

        private void DFScycle(int u, int endV, List<Edge> E, int[] color, int unavailableEdge, List<int> cycle)
        {
            //если u == endV, то эту вершину перекрашивать не нужно, иначе мы в нее не вернемся, а вернуться необходимо
            if (u != endV)
                color[u] = 2;
            else
            {
                if (cycle.Count >= 2)
                {
                    cycle.Reverse();
                    string s = cycle[0].ToString();
                    for (int i = 1; i < cycle.Count; i++)
                        s += "-" + cycle[i].ToString();
                    bool flag = false; //есть ли палиндром для этого цикла графа в листбоксе?
                    for (int i = 0; i < listBoxMatrix.Items.Count; i++)
                        if (listBoxMatrix.Items[i].ToString() == s)
                        {
                            flag = true;
                            break;
                        }
                    if (!flag)
                    {
                        cycle.Reverse();
                        s = cycle[0].ToString();
                        for (int i = 1; i < cycle.Count; i++)
                            s += "-" + cycle[i].ToString();
                        listBoxMatrix.Items.Add(s);
                    }
                    return;
                }
            }
            for (int w = 0; w < E.Count; w++)
            {
                if (w == unavailableEdge)
                    continue;
                if (color[E[w].v2] == 1 && E[w].v1 == u)                {
                    List<int> cycleNEW = new List<int>(cycle);
                    cycleNEW.Add(E[w].v2 + 1);
                    DFScycle(E[w].v2, endV, E, color, w, cycleNEW);
                    color[E[w].v2] = 1;
                }
                else if (color[E[w].v1] == 1 && E[w].v2 == u)
                {
                    List<int> cycleNEW = new List<int>(cycle);
                    cycleNEW.Add(E[w].v1 + 1);
                    DFScycle(E[w].v1, endV, E, color, w, cycleNEW);
                    color[E[w].v1] = 1;
                }
            }
        }


        private void saveButton_Click(object sender, EventArgs e)
        {
            if (sheet.Image != null)
            {
                SaveFileDialog savedialog = new SaveFileDialog();
                savedialog.Title = "Сохранить картинку как...";
                savedialog.OverwritePrompt = true;
                savedialog.CheckPathExists = true;
                savedialog.Filter = "Image Files(*.BMP)|*.BMP|Image Files(*.JPG)|*.JPG|Image Files(*.GIF)|*.GIF|Image Files(*.PNG)|*.PNG|All files (*.*)|*.*";
                savedialog.ShowHelp = true;
                if (savedialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        sheet.Image.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        StreamWriter sr = new StreamWriter(/*savedialog.FileName+*/"Информация о графе.txt");
                        string s = "";
                        for(int i=0;i< listBoxMatrix.Items.Count;i++)
                        {
                            s = (string)listBoxMatrix.Items[i];
                            sr.WriteLine(s);
                        }
                        sr.Close();
                    }
                    catch
                    {
                        MessageBox.Show("Невозможно сохранить изображение", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.FORM.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (V.Count != 0)
            {
            Program.FORM.listBoxMatrix.Items.Clear();
            Program.FORM.listBoxMatrix.Items.Add("...Операция может занять несколько секунд... или не несколько..."); Program.FORM.listBoxMatrix.Refresh();

            AMatrix = new int[V.Count, V.Count];
            G.fillAdjacencyMatrix(V.Count, E, AMatrix);
            SqMatrix M = new SqMatrix(AMatrix);
            g = new Graphs(M);

            //----------------------------------

            Program.CHECK.ShowDialog();


                //----------------------------------

                Program.FORM.listBoxMatrix.Items.Clear();
                StreamReader sr = new StreamReader("Информация о графе.txt");
                string s = "";
                while (s != null)
                {
                    listBoxMatrix.Items.Add(s);
s = sr.ReadLine();
                }
                sr.Close();
                textBox1.Hide();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void расположитьВершиныНаОкружностиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AMatrix = new int[V.Count, V.Count];
            G.fillAdjacencyMatrix(V.Count, E, AMatrix);
            selectButton.Enabled = true;
            drawVertexButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            deleteButton.Enabled = true;
            //Vtmp = new List<Vertex>(V);
            //Etmp = new List<Edge>(E);
            V.Clear();
            E.Clear();
            G.clearSheet();
            sheet.Image = G.GetBitmap();

            SqMatrix M = new SqMatrix(AMatrix);
            g = new Graphs(M);
            this.V = new List<Vertex>(g.Ver);
            this.E = new List<Edge>(g.Ed);
            G.drawThisGraph(g);
            
        }

        private void нарисоватьДополнениеГрафаToolStripMenuItem_Click(object sender, EventArgs e)
        {

            AMatrix = new int[V.Count, V.Count];
            G.fillAdjacencyMatrix(V.Count, E, AMatrix);
            selectButton.Enabled = true;
            drawVertexButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            deleteButton.Enabled = true;
            //Vtmp = new List<Vertex>(V);
            //Etmp = new List<Edge>(E);
            V.Clear();
            E.Clear();
            G.clearSheet();
            sheet.Image = G.GetBitmap();

            SqMatrix M = new SqMatrix(AMatrix);
            g = new Graphs(M);
            Graphs dg = g.Addition;
            this.V = new List<Vertex>(g.Ver);
            this.E = new List<Edge>(g.Ed);
            G.drawThisGraph(dg);

        }

        private void очиститьТекстовуюПанельToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBoxMatrix.Items.Clear();
        }

        private void вернутьИсходноеИзображениеГрафаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AMatrix = new int[V.Count, V.Count];
            G.fillAdjacencyMatrix(V.Count, E, AMatrix);
            //SqMatrix M = new SqMatrix(AMatrix);
            //g = new Graphs(M);
            selectButton.Enabled = true;
            drawVertexButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            deleteButton.Enabled = true;
            //Vtmp = new List<Vertex>(V);
            //Etmp = new List<Edge>(E);
            V.Clear();
            E.Clear();
            G.clearSheet();
            sheet.Image = G.GetBitmap();

            this.E = new List<Edge>(Etmp);
            this.V = new List<Vertex>(Vtmp);
            for (int i = 0; i < V.Count; i++) V[i].color = 0;
            G.drawALLGraph(V, E);
        }

        private void какПользоватьсяПрограммойToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Опрограмме FR = new Опрограмме();
            FR.ShowDialog();
        }

        private void нарисоватьПодграфПорождённыйВершинами1234ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //V.Clear();
            //E.Clear();
            G.clearSheet();
            sheet.Image = G.GetBitmap();

            Graphs o = g.SubGraph(1, 2, 3, 4);
            G.drawThisGraph(o);
        }

        private void нарисоватьОдинИзОстововToolStripMenuItem_Click(object sender, EventArgs e)
        {
            G.clearSheet();
            sheet.Image = G.GetBitmap();

            Graphs o = g.GetSpanningTree();
            G.drawThisGraph(o);
        }

        private void изобразитьПримерГомеоморфногоГрафаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            G.clearSheet();
            sheet.Image = G.GetBitmap();

            Graphs o = g.GomeoExample();
            G.drawThisGraph(o);
        }

        private void изобразитьПримерПервообразногоГрафаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            G.clearSheet();
            sheet.Image = G.GetBitmap();

            Graphs o = g.OrigExample();
            G.drawThisGraph(o);
        }

        private void показатьРаскраскуГрафаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //пока расположить вершины на окружности
            AMatrix = new int[V.Count, V.Count];
            G.fillAdjacencyMatrix(V.Count, E, AMatrix);
            selectButton.Enabled = true;
            drawVertexButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            deleteButton.Enabled = true;
            //Vtmp = new List<Vertex>(V);
            //Etmp = new List<Edge>(E);
            //V.Clear();
            //E.Clear();
            G.clearSheet();
            sheet.Image = G.GetBitmap();

            SqMatrix M = new SqMatrix(AMatrix);
            g = new Graphs(M);
            this.V = new List<Vertex>(g.Ver);

            Vectors v = g.GetColouring();
            for (int i = 0; i < V.Count; i++) V[i].color = (int)v[i];

            this.E = new List<Edge>(g.Ed);
            G.drawThisGraph(g);
        }

        private void особыеДействияСИзображениемToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AMatrix = new int[V.Count, V.Count];
            G.fillAdjacencyMatrix(V.Count, E, AMatrix);
            SqMatrix M = new SqMatrix(AMatrix);
            g = new Graphs(M);
            if (!save)
            {
            Vtmp = new List<Vertex>(V);
            Etmp = new List<Edge>(E);
                save = true;
            }

        }

        private void показатьРаскраскуГрафаНаОригинальномИзображенииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AMatrix = new int[V.Count, V.Count];
            G.fillAdjacencyMatrix(V.Count, E, AMatrix);
            selectButton.Enabled = true;
            drawVertexButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            deleteButton.Enabled = true;
            //Vtmp = new List<Vertex>(V);
            //Etmp = new List<Edge>(E);
            V.Clear();
            E.Clear();
            G.clearSheet();
            sheet.Image = G.GetBitmap();

            this.E = new List<Edge>(Etmp);
            this.V = new List<Vertex>(Vtmp);

            SqMatrix M = new SqMatrix(AMatrix);
            g = new Graphs(M);
            Vectors v = g.GetColouring();
            for (int i = 0; i < V.Count; i++) V[i].color = (int)v[i];
            G.drawALLGraph(V, E);
        }

        private void изобразитьОбъединениеРёберныхБлоковToolStripMenuItem_Click(object sender, EventArgs e)
        {
            G.clearSheet();
            sheet.Image = G.GetBitmap();

            Graphs o = g.BridgeBlocks();
            G.drawThisGraph(o);
        }

        private void изобразитьБлокСвязанныйСТочкамиСочлененияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            G.clearSheet();
            sheet.Image = G.GetBitmap();

            Graphs o = g.JointBlock();
            G.drawThisGraph(o);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Program.FORM.listBoxMatrix.Items.Clear();

            AMatrix = new int[V.Count, V.Count];
            G.fillAdjacencyMatrix(V.Count, E, AMatrix);
            SqMatrix M = new SqMatrix(AMatrix);
            g = new Graphs(M);

            //g.ShowInfoFile();

            FileStream fs = new FileStream("Информация о графе.txt", FileMode.Create);
            TextWriter tmp = Console.Out;
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);

            //Polynom pol = g.Xpolymon();
            //Console.Write("Хроматический полином: "); pol.Show();
            //Console.WriteLine("Тогда хроматическое число X = {0}", g.ChromaticNumber);
            g.ShowCheck0();
            try
            {
                g.ShowCheck11();
        }
            catch (Exception y) { Console.Write("----------Исключение! "); Console.WriteLine(y.Message); }

    sw.Close();
            Console.SetOut(tmp);
            Console.WriteLine("Запись завершена!");

            StreamReader sr = new StreamReader("Информация о графе.txt");
            string s = "";
            while (s != null)
            {

                listBoxMatrix.Items.Add(s);
                s = sr.ReadLine();
            }
            sr.Close();
        }

        private void показатьПримерМаксимальногоНезависимогоПодмножестваToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.FORM.listBoxMatrix.Items.Clear();

            AMatrix = new int[V.Count, V.Count];
            G.fillAdjacencyMatrix(V.Count, E, AMatrix);
            selectButton.Enabled = true;
            drawVertexButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            deleteButton.Enabled = true;
            V.Clear();
            E.Clear();
            G.clearSheet();
            sheet.Image = G.GetBitmap();

            this.E = new List<Edge>(Etmp);
            this.V = new List<Vertex>(Vtmp); for (int i = 0; i < V.Count; i++) V[i].color = 0;

            SqMatrix M = new SqMatrix(AMatrix);
            g = new Graphs(M);

            FileStream fs = new FileStream("Информация о графе.txt", FileMode.Create);
            TextWriter tmp = Console.Out;
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);

            Vectors p;
            Console.WriteLine();
            Console.WriteLine("Независимые (внутренне устойчивые) подмножества вершин графа (подмножества наибольшей длины - максимальные, любые подмножества этих подмножеств - тоже независимые подмножества):"); g.ShowIndepSubSets();
            Console.WriteLine("Наибольшие независимые подмножества (^наибольшие^ значит, что каждая вершина графа вне этого подмножества смежна вершине в подмножестве):"); g.ShowGreatestIndepSubSets();
            Console.Write("-----------> Число независимости графа = {0}. Вершины максимального множества: ", g.IndependenceNumber(out p)); 
            p.Show();
            //Vectors v = new Vectors(g.GreatestIndepSubsets[0]);
            for (int i = 0; i < p.n; i++) V[(int)p[i]-1].color = 1;
            G.drawALLGraph(V, E);

            sw.Close();
            Console.SetOut(tmp);
            Console.WriteLine("Запись завершена!");

            StreamReader sr = new StreamReader("Информация о графе.txt");
            string s = "";
            while (s != null)
            {

                listBoxMatrix.Items.Add(s);
                s = sr.ReadLine();
            }
            sr.Close();
        }

        private void показатьПримерМинимальногоДоминирующегоПодмножестваToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.FORM.listBoxMatrix.Items.Clear();

            AMatrix = new int[V.Count, V.Count];
            G.fillAdjacencyMatrix(V.Count, E, AMatrix);
            selectButton.Enabled = true;
            drawVertexButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            deleteButton.Enabled = true;
            V.Clear();
            E.Clear();
            G.clearSheet();
            sheet.Image = G.GetBitmap();

            this.E = new List<Edge>(Etmp);
            this.V = new List<Vertex>(Vtmp); for (int i = 0; i < V.Count; i++) V[i].color = 0;

            SqMatrix M = new SqMatrix(AMatrix);
            g = new Graphs(M);

            FileStream fs = new FileStream("Информация о графе.txt", FileMode.Create);
            TextWriter tmp = Console.Out;
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);

            g.DominSub();
            Console.WriteLine("Доминирующие (внешне устойчивые) множества (записано каждое третье):"); g.ShowDominSub(3);
            Console.WriteLine("Минимальные (не содержащие в себе других) доминирующие множества:"); g.ShowMinDominSub();
            Console.WriteLine("Наименьшие (по мощности) доминирующие  множества:"); g.ShowSmallestDominSub();
            Console.WriteLine("-----------> Число доминирования равно {0}", g.DominationNumber);
            Vectors v = new Vectors(g.MinimalDominSubsets[0]);
            for (int i = 0; i < v.n; i++) V[(int)v[i] - 1].color = 1;
            G.drawALLGraph(V, E);

            sw.Close();
            Console.SetOut(tmp);
            Console.WriteLine("Запись завершена!");

            StreamReader sr = new StreamReader("Информация о графе.txt");
            string s = "";
            while (s != null)
            {

                listBoxMatrix.Items.Add(s);
                s = sr.ReadLine();
            }
            sr.Close();
        }

        private void показатьПримерМинимальногоВершинногоПокрытияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.FORM.listBoxMatrix.Items.Clear();
            //Program.FORM.listBoxMatrix.Items.Add("...Операция может занять несколько секунд... или не несколько...");

            AMatrix = new int[V.Count, V.Count];
            G.fillAdjacencyMatrix(V.Count, E, AMatrix);
            selectButton.Enabled = true;
            drawVertexButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            deleteButton.Enabled = true;
            V.Clear();
            E.Clear();
            G.clearSheet();
            sheet.Image = G.GetBitmap();

            this.E = new List<Edge>(Etmp);
            this.V = new List<Vertex>(Vtmp); for (int i = 0; i < V.Count; i++) V[i].color = 0;

            SqMatrix M = new SqMatrix(AMatrix);
            g = new Graphs(M);

            FileStream fs = new FileStream("Информация о графе.txt", FileMode.Create);
            TextWriter tmp = Console.Out;
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);
            Program.FORM.listBoxMatrix.Items.Add("...Операция может занять несколько секунд... или не несколько..."); Program.FORM.listBoxMatrix.Refresh();
            g.VCoatingSub();
            Console.WriteLine("Вершинные покрытия графа (записано каждое третье):"); Graphs.ShowVectorsList(g.VCoatingSubsets, 3);
            Console.WriteLine("Минимальные (не содержащие в себе других) вершинные покрытия:"); Graphs.ShowVectorsList(g.MinimalVCoatingSubsets);
            Console.WriteLine("Наименьшие (по мощности) вершинные покрытия:"); g.ShowSmallestVCoatingSub();
            Console.WriteLine("-----------> Число вершинного покрытия равно {0}", g.VCoatingNumber);
            Vectors v = new Vectors(g.MinimalVCoatingSubsets[0]);
            for (int i=0;i<g.MinimalVCoatingSubsets.Count;i++)
                if(g.MinimalVCoatingSubsets[i].n==g.VCoatingNumber)
                {
                    v = new Vectors(g.MinimalVCoatingSubsets[i]);
                    break;
                }
            
            for (int i = 0; i < v.n; i++) V[(int)v[i] - 1].color = 1;
            G.drawALLGraph(V, E);

            sw.Close();
            Console.SetOut(tmp);
            Console.WriteLine("Запись завершена!");

            Program.FORM.listBoxMatrix.Items.Clear();
            StreamReader sr = new StreamReader("Информация о графе.txt");
            string s = "";
            while (s != null)
            {

                listBoxMatrix.Items.Add(s);
                s = sr.ReadLine();
            }
            sr.Close();
        }

        private void показатьПримерНаибольшегоКликаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.FORM.listBoxMatrix.Items.Clear();
            //Program.FORM.listBoxMatrix.Items.Add("...Операция может занять несколько секунд... или не несколько...");

            AMatrix = new int[V.Count, V.Count];
            G.fillAdjacencyMatrix(V.Count, E, AMatrix);
            selectButton.Enabled = true;
            drawVertexButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            deleteButton.Enabled = true;
            V.Clear();
            E.Clear();
            G.clearSheet();
            sheet.Image = G.GetBitmap();

            this.E = new List<Edge>(Etmp);
            this.V = new List<Vertex>(Vtmp); for (int i = 0; i < V.Count; i++) V[i].color = 0;

            SqMatrix M = new SqMatrix(AMatrix);
            g = new Graphs(M);

            FileStream fs = new FileStream("Информация о графе.txt", FileMode.Create);
            TextWriter tmp = Console.Out;
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);
            Program.FORM.listBoxMatrix.Items.Add("...Операция может занять несколько секунд... или не несколько..."); Program.FORM.listBoxMatrix.Refresh();
            g.CliquesSub();
            Console.WriteLine("Клики графа (вершины, дающие полные подграфы; записан каждый второй):"); Graphs.ShowVectorsList(g.CliquesSubsets, 2);
            Console.WriteLine("Макcимальные (не содержащиеся в других) клики:"); Graphs.ShowVectorsList(g.MaximalCliquesSubsets);
            Console.WriteLine("Наибольшие (по мощности) клики:"); Graphs.ShowVectorsList(g.GreatestCliquesSubsets);
            Console.WriteLine("-----------> Число кликового покрытия равно {0}", g.CliquesNumber);
            //Console.WriteLine("-----------> Рёберная плотность графа равна 2q/(p(p-1) = 2*{0}/({1}*{2}) = {3}", g.Edges, g.p, g.p - 1, g.Density);
            Console.WriteLine("Матрица кликов графа:"); g.CliquesMatrix.PrintMatrix();
            //Console.WriteLine("Матрица смежности графа клик:"); g.CliquesGraph.A.PrintMatrix();
            Vectors v = new Vectors(g.GreatestCliquesSubsets[0]);

            for (int i = 0; i < v.n; i++) V[(int)v[i] - 1].color = 1;
            G.drawALLGraph(V, E);

            sw.Close();
            Console.SetOut(tmp);
            Console.WriteLine("Запись завершена!");

            Program.FORM.listBoxMatrix.Items.Clear();
            StreamReader sr = new StreamReader("Информация о графе.txt");
            string s = "";
            while (s != null)
            {

                listBoxMatrix.Items.Add(s);
                s = sr.ReadLine();
            }
            sr.Close();
        }

        private void изобразитьГрафКликToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Program.FORM.listBoxMatrix.Items.Clear();
            //Program.FORM.listBoxMatrix.Items.Add("...Операция может занять несколько секунд... или не несколько...");

            AMatrix = new int[V.Count, V.Count];
            G.fillAdjacencyMatrix(V.Count, E, AMatrix);
            selectButton.Enabled = true;
            drawVertexButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            deleteButton.Enabled = true;
            V.Clear();
            E.Clear();
            G.clearSheet();
            sheet.Image = G.GetBitmap();

            this.E = new List<Edge>(Etmp);
            this.V = new List<Vertex>(Vtmp); for (int i = 0; i < V.Count; i++) V[i].color = 0;

            SqMatrix M = new SqMatrix(AMatrix);
            g = new Graphs(M);

            FileStream fs = new FileStream("Информация о графе.txt", FileMode.Create);
            TextWriter tmp = Console.Out;
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);
            Program.FORM.listBoxMatrix.Items.Add("...Операция может занять несколько секунд... или не несколько..."); Program.FORM.listBoxMatrix.Refresh();
            g.CliquesSub();
            Graphs o = g.CliquesGraph;
            G.drawThisGraph(o);
            Console.WriteLine("Клики графа (вершины, дающие полные подграфы; записан каждый второй):"); Graphs.ShowVectorsList(g.CliquesSubsets, 2);
            Console.WriteLine("Макcимальные (не содержащиеся в других) клики:"); Graphs.ShowVectorsList(g.MaximalCliquesSubsets);
            Console.WriteLine("Наибольшие (по мощности) клики:"); Graphs.ShowVectorsList(g.GreatestCliquesSubsets);
            Console.WriteLine("-----------> Число кликового покрытия равно {0}", g.CliquesNumber);
            //Console.WriteLine("-----------> Рёберная плотность графа равна 2q/(p(p-1) = 2*{0}/({1}*{2}) = {3}", g.Edges, g.p, g.p - 1, g.Density);
            Console.WriteLine("Матрица кликов графа:"); g.CliquesMatrix.PrintMatrix();
            //Console.WriteLine("Матрица смежности графа клик:"); g.CliquesGraph.A.PrintMatrix();
            Vectors v = new Vectors(g.GreatestCliquesSubsets[0]);

            for (int i = 0; i < v.n; i++) V[(int)v[i] - 1].color = 1;
            //G.drawALLGraph(V, E);

            sw.Close();
            Console.SetOut(tmp);
            Console.WriteLine("Запись завершена!");

            Program.FORM.listBoxMatrix.Items.Clear();
            StreamReader sr = new StreamReader("Информация о графе.txt");
            string s = "";
            while (s != null)
            {

                listBoxMatrix.Items.Add(s);
                s = sr.ReadLine();
            }
            sr.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Restart();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Program.MATRIX.ShowDialog();

            selectButton.Enabled = true;
            drawVertexButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            deleteButton.Enabled = true;
            //Vtmp = new List<Vertex>(V);
            //Etmp = new List<Edge>(E);
            V.Clear();
            E.Clear();
            G.clearSheet();
            sheet.Image = G.GetBitmap();

            this.V = new List<Vertex>(g.Ver);
            this.E = new List<Edge>(g.Ed);
            G.drawThisGraph(g);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Program.FORM.listBoxMatrix.Items.Clear();

            AMatrix = new int[V.Count, V.Count];
            G.fillAdjacencyMatrix(V.Count, E, AMatrix);
            SqMatrix M = new SqMatrix(AMatrix);
            g = new Graphs(M);

            //g.ShowInfoFile();

            FileStream fs = new FileStream("Информация о графе.txt", FileMode.Create);
            TextWriter tmp = Console.Out;
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);

            g.ShowCheck0();
            try { g.ShowCheck5(); }
            catch (Exception y) { Console.WriteLine(y.Message); }

            sw.Close();
            Console.SetOut(tmp);
            Console.WriteLine("Запись завершена!");

            StreamReader sr = new StreamReader("Информация о графе.txt");
            string s = "";
            while (s != null)
            {

                listBoxMatrix.Items.Add(s);
                s = sr.ReadLine();
            }
            sr.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            G.clearSheet();
            G.drawALLGraph(V, E);
            sheet.Image = G.GetBitmap();
        }
    }
}
