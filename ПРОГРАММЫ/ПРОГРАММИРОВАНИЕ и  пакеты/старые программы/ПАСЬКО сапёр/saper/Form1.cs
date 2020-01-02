//игра САПЁР, сделал Пасько Д. А., группа 33.1
//в последний раз программа редактировалась 11.11.2017

//код первой формы

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    enum game_status { Beginng, Process, Endgame }

    public partial class Form1 : Form
    {
        private const int
        amount_vert = 10, // количество клеток по вертикали
        amount_horiz = amount_vert, // кол-во клеток по горизонтали

        cell_width = 50,  // ширина клетки
        cell_length = cell_width;  // высота клетки

        private int amount_mines = amount_vert;// кол-во мин

        private string game_result;/// результат игры

        // массив - минное поле
        private int[,] Pole = new int[amount_vert + 2, amount_horiz + 2];
        // значение элемента массива:
        //  0..8 - количество мин в соседних клетках (соседних клеток всего не больше 8)
        //  9 - в данной клетке мина
        //  100..109 - клетка открыта и рядом с ней 0..8 мин или мина в ней
        //  200..209 - в клетку поставлен флаг (и рядом с ней 0..8 мин или мина в ней)

        private int number_mines;  // кол-во найденных мин
        private int number_flags; // кол-во поставленных флагов

        // статус игры: 0 - начало игры, 1 - игра, 2 – результат
        private game_status status;

        // графическая поверхность формы, поверхность рисования; прежде чем начать рисовать, нужно создать такую поверхность, чтобы ссылаться на неё в функциях и пр.
        private System.Drawing.Graphics g;

        //конструктор формы
        public Form1()
        {
            InitializeComponent();//метод инициализации компонентов, генерируется автоматически студией при работе с дизайном формы

            // в неотображаемые эл-ты массива (по краям), соответствующие клеткам границы поля пишется число -1. Это значение потом используется процедурой Open_cells() для завершения процесса открытия соседних пустых клеток
            for (int row = 0; row <= amount_vert + 1; row++)
            {
                Pole[row, 0] = -1;
                Pole[row, amount_horiz + 1] = -1;
            }

            for (int col = 0; col <= amount_horiz + 1; col++)
            {
                Pole[0, col] = -1;
                Pole[amount_vert + 1, col] = -1;
            }

            // устанавливаем размер формы в соответствии с размером игрового поля (во вотором аргументе к общей мере добавляется мера самого меню)
            this.ClientSize = new Size(cell_width * amount_horiz + 1, cell_length * amount_vert + menuStrip1.Height + 1);

            New_game(); // новая игра

            // создание графической поверхности, получение ссылки; используется именно этот способ, потому что рисование происходит на уже существующей форме
            g = panel1.CreateGraphics();
        }

        // новая игра
        private void New_game()
        {
            int row, col,    // индексы клетки
            n = 0,      // количество поставленных мин
            k;           // кол-во мин в соседних клетках

            // очистить поле
            for (row = 1; row <= amount_vert; row++)
                for (col = 1; col <= amount_horiz; col++)
                    Pole[row, col] = 0; //в каждой клетке мины нет

            // инициализация генератора случайных чисел через текущее время
            Random rnd = new Random();

            // расставим мины случайным образом
            do
            {
                row = rnd.Next(amount_vert) + 1;
                col = rnd.Next(amount_horiz) + 1;

                if (Pole[row, col] != 9)//если в этой клекте уже нет мины, то поставить
                {
                    Pole[row, col] = 9;
                    n++;
                }
            }
            while (n != amount_mines); //выполнять, пока не будет поставлено максимальное количество мин

            // для каждой клетки вычислим кол-во мин в соседних клетках
            for (row = 1; row <= amount_vert; row++)
                for (col = 1; col <= amount_horiz; col++)
                    if (Pole[row, col] != 9)//если в этой клетке нет мины, то в ней всё считается
                    {
                        k = 0;

                        for (int i = row - 1; i <= row + 1; i++)
                            for (int j = col - 1; j <= col + 1; j++)
                                if (Pole[i, j] == 9) k++;

                        /*if (Pole[row - 1, col - 1] == 9) k++;
                        if (Pole[row - 1, col] == 9) k++;
                        if (Pole[row - 1, col + 1] == 9) k++;
                        if (Pole[row, col - 1] == 9) k++;
                        if (Pole[row, col + 1] == 9) k++;
                        if (Pole[row + 1, col - 1] == 9) k++;
                        if (Pole[row + 1, col] == 9) k++;
                        if (Pole[row + 1, col + 1] == 9) k++;*/

                        Pole[row, col] = k;
                    }

            status = game_status.Beginng;      // начало игры
            label1.Text = "Щёлкайте по полю"; label2.Text = "";

            number_mines = 0;      // нет обнаруженных мин
            number_flags = 0;      // нет поставленных флагов
        }

        // рисует поле
        private void Show_Pole0(Graphics g, game_status status)
        {
            for (int row = 1; row <= amount_vert; row++)
                for (int col = 1; col <= amount_horiz; col++)
                    this.Draw_a_cell(g, row, col, status);
        }

        private void Show_Pole(Graphics g, game_status status)
        {
            //System.Threading.Thread.Sleep(500);/*приостановить работу на секунду*/
            //await Task.Delay(5000);

            //задержка
            int i = 0;
            while (i < 150000000) i++;

            for (int row = 1; row <= amount_vert; row++)
                for (int col = 1; col <= amount_horiz; col++)
                    this.Draw_a_cell(g, row, col, status);
        }

        // рисует клетку (включая флаг в клетке и открытие всех свободных клеток по близости)
        private void Draw_a_cell(Graphics g, int row, int col, game_status status)
        {

            int x, y;// декартовы координаты левого верхнего угла клетки

            x = (col - 1) * cell_width + 1;
            y = (row - 1) * cell_length + 1;

            // если не открытые клетки - будут серые
            if (Pole[row, col] < 100) g.FillRectangle(SystemBrushes.ControlDark, x - 1, y - 1, cell_width, cell_length);

            // открытые или помеченные клетки
            if (Pole[row, col] >= 100)
            {
                // открываем клетку, открытые - светло-зелёные
                if (Pole[row, col] != 109) g.FillRectangle(Brushes.LawnGreen, x - 1, y - 1, cell_width, cell_length);
                else
                {
                    // если в клетке была бомба, то на этой мине взрыв -> конец игры
                    g.FillRectangle(Brushes.Red, x - 1, y - 1, cell_width, cell_length);
                    status = game_status.Endgame;
                    game_result = "ПОТРАЧЕНО";
                    label2.Text = game_result;
                    label1.Text = "Игра завершена";

                }
                // если в соседних клетках есть мины, указываем их количество
                if ((Pole[row, col] >= 101) && (Pole[row, col] <= 108)) g.DrawString((Pole[row, col] - 100).ToString(), new Font("Tahoma", 15, System.Drawing.FontStyle.Regular), Brushes.DarkBlue, x + 5, y + 5);
            }

            // в клетке поставлен флаг
            if (Pole[row, col] >= 200) this.Draw_flag(g, x, y);

            // рисуем границу клетки
            g.DrawRectangle(Pens.Black, x - 1, y - 1, cell_width, cell_length);

            // если игра завершена (status = 2), показываем мины
            if ((status == game_status.Endgame) && ((Pole[row, col] % 10) == 9))
            {
                this.Draw_bomb(g, x, y);
                //this.Draw_flag(g, x, y);
            }

        }

        // открывает текущую и все соседние с ней клетки, в которых нет мин
        private void Open_cells(int row, int col)
        {
            // координаты области вывода
            int x = (col - 1) * cell_width + 1, y = (row - 1) * cell_length + 1;

            if (Pole[row, col] == 0)//если в этой и соседних клетках нет мин, открыть все 9
            {
                Pole[row, col] = 100;

                // отобразить содержимое клетки
                this.Draw_a_cell(g, row, col, status);

                /*
                // открыть примыкающие клетки слева, справа, сверху, снизу
                this.Open_cells(row, col - 1);
                this.Open_cells(row - 1, col);
                this.Open_cells(row, col + 1);
                this.Open_cells(row + 1, col);

                //примыкающие диагонально
                this.Open_cells(row - 1, col - 1);
                this.Open_cells(row - 1, col + 1);
                this.Open_cells(row + 1, col - 1);
                this.Open_cells(row + 1, col + 1);
                */

                // открыть примыкающие клетки
                for (int i = row - 1; i <= row + 1; i++)
                    for (int j = col - 1; j <= col + 1; j++)
                        if ((i != row) || (j != col)) this.Open_cells(i, j);

            }
            else if ((Pole[row, col] < 100) && (Pole[row, col] != -1)) //иначе всё равно открыть, если только это не самые граничные клетки
            {
                Pole[row, col] += 100;

                // отобразить содержимое клетки
                this.Draw_a_cell(g, row, col, status);
            }
        }



        // рисует мину
        private void Draw_bomb(Graphics g, int x, int y)
        {
            // корпус
            g.DrawPie(Pens.Black, x + 6, y + 20, 28, 16, 0, -180);
            g.FillPie(Brushes.Green, x + 6, y + 20, 28, 16, 0, -180);

            // вертикальный ус
            g.DrawLine(Pens.Black, x + 20, y + 15, x + 20, y + 26);

            //на мину похоже

        }

        // рисует флаг
        private void Draw_flag(Graphics g, int x, int y)
        {
            Point[] p = new Point[3];
            // флажок
            p[0].X = x + 4; p[0].Y = y + 4;
            p[1].X = x + 30; p[1].Y = y + 12;
            p[2].X = x + 4; p[2].Y = y + 20;
            g.FillPolygon(Brushes.Red, p);

            // древко
            g.DrawLine(Pens.Black, x + 4, y + 4, x + 4, y + 35);

            Point[] m = new Point[5];
            // буква M на флажке
            m[0].X = x + 8; m[0].Y = y + 14;
            m[1].X = x + 8; m[1].Y = y + 8;
            m[2].X = x + 10; m[2].Y = y + 10;
            m[3].X = x + 12; m[3].Y = y + 8;
            m[4].X = x + 12; m[4].Y = y + 14;
            g.DrawLines(Pens.White, m);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void минToolStripMenuItem_Click(object sender, EventArgs e)
        {
            amount_mines = 10;
            New_game();
            Show_Pole0(g, status);
        }

        // щелчок кнопкой в клетке игрового поля
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            // игра завершена -> вписать в метку об этом + сделать так, чтобы последующие щелчки ничего не делали
            if (status == game_status.Endgame) label1.Text = "Игра завершена";
            else
            {
                // первый щелчок -> начать игру
                if (status == game_status.Beginng)
                {
                    //New_game(); // новая игра
                    status = game_status.Process;
                    label1.Text = "Игра началась";
                    label2.Text = "";
                }
                // преобразуем координаты мыши в индексы клетки поля, в которой был сделан щелчок; (e.X, e.Y) - координаты точки формы, в которой была нажата кнопка мыши;            
                int row = e.Y / cell_length + 1, col = e.X / cell_width + 1;

                // координаты области вывода
                int x = (col - 1) * cell_width + 1, y = (row - 1) * cell_length + 1;

                // щелчок левой кнопки мыши
                if (e.Button == MouseButtons.Left)
                {
                    // открыта клетка, в которой есть мина                   
                    if (Pole[row, col] == 9)
                    {
                        Pole[row, col] += 100;

                        // игра закончена
                        status = game_status.Endgame;

                        // перерисовать форму
                        this.panel1.Invalidate();

                    }
                    else
                        if (Pole[row, col] < 9) this.Open_cells(row, col);//если в клетке нет мины, заделать процесс открытия
                }

                // щелчок правой кнопки мыши
                if (e.Button == MouseButtons.Right)
                {

                    // в клетке не было флага/клетка не открыта, ставим его
                    if (Pole[row, col] <= 9)
                    {
                        number_flags += 1;

                        if (Pole[row, col] == 9) number_mines += 1;

                        Pole[row, col] += 200;

                        if ((number_mines == amount_mines) && (number_flags == amount_mines))//если открыты все мины и поставлено столько же флагов, завершить игру
                        {
                            this.Draw_a_cell(g, row, col, status);
                            // игра закончена
                            status = game_status.Endgame;
                            label1.Text = "Игра завершена";
                            game_result = "МОЁ УВАЖЕНИЕ";
                            label2.Text = game_result;

                            // перерисовываем все игровое поле ()
                            //this.Invalidate();
                            this.panel1.Invalidate();
                        }
                        else
                            // перерисовываем только клетку
                            this.Draw_a_cell(g, row, col, status);
                    }
                    else
                        // в клетке был поставлен флаг, повторный щелчок правой кнопки мыши убирает его и закрывает клетку
                        if (Pole[row, col] >= 200)
                    {
                        number_flags -= 1;
                        Pole[row, col] -= 200;

                        // перерисовываем клетку                
                        this.Draw_a_cell(g, row, col, status);
                    }
                }
            }
        }

        private void минToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            amount_mines = 15;
            New_game();
            Show_Pole0(g, status);
        }

        private void миноченьСложноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            amount_mines = 20;
            New_game();
            Show_Pole0(g, status);
        }

        private void минневозможноПоЗаконамВероятностиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            amount_mines = 30;
            New_game();
            Show_Pole0(g, status);
        }

        // команда Новая игра
        private void новаяИграToolStripMenuItem_Click(object sender, EventArgs e)
        {
            New_game();
            Show_Pole0(g, status);
        }

        // обработка события Paint панели, происходит при перерисовке
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Show_Pole(g, status);
        }

        // выбор в меню Справка команды О программе
        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 aboutBox = new Form2();
            aboutBox.ShowDialog();//отображает форму как модальное диалоговое окно

        }
    }
}
