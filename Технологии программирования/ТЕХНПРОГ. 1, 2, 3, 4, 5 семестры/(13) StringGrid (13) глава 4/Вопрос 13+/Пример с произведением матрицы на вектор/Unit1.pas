unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, Grids, StdCtrls, ExtCtrls;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    Edit1: TEdit;
    Button1: TButton;
    StringGrid1: TStringGrid;
    StringGrid2: TStringGrid;
    StringGrid3: TStringGrid;
    Button2: TButton;
    Label3: TLabel;
    RadioGroup1: TRadioGroup;
    RadioButton1: TRadioButton;
    RadioButton2: TRadioButton;
    RadioButton3: TRadioButton;
    Button3: TButton;
    RadioGroup2: TRadioGroup;
    RadioButton4: TRadioButton;
    RadioButton5: TRadioButton;
    RadioButton6: TRadioButton;
    RadioButton7: TRadioButton;
    RadioButton8: TRadioButton;
    RadioButton9: TRadioButton;
    procedure FormCreate(Sender: TObject);
    procedure Button1Click(Sender: TObject);
    procedure Button2Click(Sender: TObject);
    procedure RadioButton1Click(Sender: TObject);
    procedure RadioButton2Click(Sender: TObject);
    procedure RadioButton3Click(Sender: TObject);
    procedure Button3Click(Sender: TObject);
    procedure RadioButton4Click(Sender: TObject);
    procedure RadioButton5Click(Sender: TObject);
    procedure RadioButton6Click(Sender: TObject);
    procedure RadioButton7Click(Sender: TObject);
    procedure RadioButton8Click(Sender: TObject);
    procedure RadioButton9Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;
type
    Matr = Array[1..10,1..10] of Real;
    Vct = Array[1..10] of Real;
var
  Form1: TForm1;
implementation

{$R *.dfm}
//задание начальных значений при запуске программы
Procedure TForm1.FormCreate(Sender: TObject);
var i,n: Integer;
begin
    n:=3;
    Edit1.Text:=FloatToStr(n);
    //задание числа столбцов и строк в таблицах
    StringGrid1.ColCount:= n+1;       // столбцы
    StringGrid1.RowCount:= n+1;       //строки
    StringGrid2.RowCount:= n+1;
    StringGrid3.RowCount:= n+1;
    //заполнение названиями
    StringGrid1.Cells[0,0]:='Матрица А:';
    StringGrid2.Cells[0,0]:='Вектор b:';
    StringGrid3.Cells[0,0]:='Вектор y:';
    //заполнение первых столбца и строки матрицы поясняющими надписями
    //i=1,2,...,n, j=1,2,...,n
    For i:=1 to n do
    begin
        StringGrid1.Cells[0,i]:='i= '+IntToStr(i); //целый в строковый формат
        StringGrid1.Cells[i,0]:='j= '+IntToStr(i);
    end;
end;

//обработка клавиши "задать размерность"

procedure TForm1.Button1Click(Sender: TObject);
var n,i,j: Integer;
begin
    TRY                                               //защищенный блок
      n:=StrToInt(Edit1.Text);  //строковый в целый формат
      IF n<=0 THEN except ShowMessage ('Ошибка');
                          Exit;
    end;
    //задание числа строк и столбцов в таблицах
    StringGrid1.ColCount:=n+1;        //столбцы
    StringGrid1.RowCount:=n+1;        //строки
    StringGrid2.RowCount:=n+1;
    StringGrid3.RowCount:=n+1;
    //запонение первых столбца и строки поясняющими надписями
    FOR i:=1 TO n DO
    begin
        StringGrid1.Cells[0,i]:='i= '+IntToStr(i);  //целый в строковый формат
        StringGrid1.Cells[i,0]:='j= '+IntToStr(i);
    end;
    //очистить ячейки
    FOR i:=1 TO n DO
       FOR j:=1 TO n DO StringGrid1.Cells[i,j]:='';
    FOR j:=1 TO n DO StringGrid2.Cells[0,j]:='';
end;


//обработка нажатия клавиши "A*b" с учетом того, что все ячейки матрицы А
//и вектора b должны быть заполнены пользователем вещественными числами
procedure TForm1.Button2Click(Sender: TObject);
var A:Matr;
    y,b:Vct;
    n,i,j:Integer;
begin
    n:=StrToInt(Edit1.Text);
    //заполнение массива А элементами из таблицы №1
    FOR i:=1 TO n DO
      FOR j:=1 TO n DO

      TRY                                              //защищенный блок
         A[i,j]:= StrToFloat(StringGrid1.Cells[j,i]);  //строковый в вещественый формат
         except ShowMessage ('Ошибка ввода эементов матрицы');
                Exit;
    end;

    //заполнение массива В элементами из таблицы №2
    FOR i:=1 TO n DO

      TRY                                            //защищенный блок
         b[i]:= StrToFloat(StringGrid2.Cells[0,i]); //строковый в вещественый формат
         except ShowMessage ('Ошибка ввода элементов вектора');
                Exit;
      end;

    //Умножение A на В
    FOR i:=1 TO n DO
    begin
        y[i]:=0;
        FOR j:=1 TO n DO y[i]:= y[i]+A[i,j]*b[j];
        //Вывод результата в таблицу №3
        StringGrid3.Cells[0,i]:= FloatToStrf(y[i],fffixed,6,2); //вещественный в строковый
    end;                                                        //формат с фиксированным
                                                                // количеством символов-6,
                                                                // и количеством символов-4
end;                                                                                                                            // после точки-2

 //3х3
procedure TForm1.RadioButton1Click(Sender: TObject);
var f:textfile;
    fName: String[80];
    x:integer;
    i,j,n:Integer;
begin
    fName:= Edit1.Text;
    AssignFile(f, '3.txt'); //связь логического и физического файла
    Reset(f);  //открытие для чтения, текущая 1-ая позиция
    n:=3;
    Edit1.Text:=FloatToStr(n); //вещественный в строковый формат
    //задание числа столбцов и строк в таблицах
    StringGrid1.ColCount:= n+1;  //столбцы
    StringGrid1.RowCount:= n+1;  //строки
    StringGrid2.RowCount:= n+1;
    StringGrid3.RowCount:= n+1;
    //запонение первых столбца и строки поясняющими надписями i=1,2,...,n, j=,2,...,n
    FOR i:=1 TO n DO
    begin
        StringGrid1.Cells[0,i]:='i= '+IntToStr(i);
        StringGrid1.Cells[i,0]:='j= '+IntToStr(i);
    end;
    FOR j:=1 TO n DO
      FOR i:=1 TO n DO
      begin
        read(f, x); //чтение элемента из файла
        StringGrid1.Cells[i,j]:= FloatToStrf(x,fffixed,1,0); //вещественный в строковый
                                                             //формат с фиксированным
                                                             // количеством символов-1,
                                                             // и количеством символов-0
      end;
      Reset(f);   //открытие для чтения, текущая 1-ая позиция
      FOR i:=1 TO n DO
        begin
          readln(f,x); //переход на следующую строку в файле
          StringGrid2.Cells[0,i]:=FloatToStrf(x,fffixed,1,0); //вещественный в строковый
                                                             //формат с фиксированным
                                                             // количеством символов-1,
                                                             // и количеством символов-0
        end;
      closeFile(f); //закрытие файла
end;

//4х4
procedure TForm1.RadioButton2Click(Sender: TObject);
var f:textfile;
    fName: String[80];
    x:integer;
    i,j,n:Integer;
begin
    fName:= Edit1.Text;
    AssignFile(f, '4.txt'); //связь логического и физического файла
    Reset(f); //открытие для чтения, текущая 1-ая позиция
    n:=4;
    Edit1.Text:=FloatToStr(n);  //вещественный в строковый формат
    //задание числа столбцов и строк в таблицах
    StringGrid1.ColCount:= n+1;  //столбцы
    StringGrid1.RowCount:= n+1;  //строки
    StringGrid2.RowCount:= n+1;
    StringGrid3.RowCount:= n+1;
    //запонение первых столбца и строки поясняющими надписями i=1,2,...,n, j=,2,...,n
    FOR i:=1 TO n DO
    begin
        StringGrid1.Cells[0,i]:='i= '+IntToStr(i);
        StringGrid1.Cells[i,0]:='j= '+IntToStr(i);
    end;
    FOR j:=1 TO n DO
      FOR i:=1 TO n DO
      begin
        read(f, x);//чтение элемента из файла
        StringGrid1.Cells[i,j]:= FloatToStrf(x,fffixed,1,0);//вещественный в строковый
                                                             //формат с фиксированным
                                                             // количеством символов-1,
                                                             // и количеством символов-0
      end;
      Reset(f); //открытие для чтения, текущая 1-ая позиция
      FOR i:=1 TO n DO
        begin
          read(f,x); //чтение элемента из файла
          StringGrid2.Cells[0,i]:=FloatToStrf(x,fffixed,1,0); //вещественный в строковый
                                                             //формат с фиксированным
                                                             // количеством символов-1,
                                                             // и количеством символов-0
        end;
      closeFile(f); //закрываем файл
end;

//5х5
procedure TForm1.RadioButton3Click(Sender: TObject);
var f:textfile;
    fName: String[80];
    x:integer;
    i,j,n:Integer;
begin
    fName:= Edit1.Text;
    AssignFile(f, '5.txt');//связь логического и физического файла
    Reset(f);  //открытие для чтения, текущая 1-ая позиция
    n:=5;
    Edit1.Text:=FloatToStr(n); //вещественный в строковый формат
    //задание числа столбцов и строк в таблицах
    StringGrid1.ColCount:= n+1;
    StringGrid1.RowCount:= n+1;
    StringGrid2.RowCount:= n+1;
    StringGrid3.RowCount:= n+1;
    //запонение первых столбца и строки поясняющими надписями i=1,2,...,n, j=,2,...,n
    FOR i:=1 TO n DO
    begin
        StringGrid1.Cells[0,i]:='i= '+IntToStr(i);
        StringGrid1.Cells[i,0]:='j= '+IntToStr(i);
    end;
    FOR j:=1 TO n DO
      FOR i:=1 TO n DO
      begin
        read(f, x); //чтение элемента из файла
        StringGrid1.Cells[i,j]:= FloatToStrf(x,fffixed,1,0); //вещественный в строковый
                                                             //формат с фиксированным
                                                             // количеством символов-1,
                                                             // и количеством символов-0
      end;
      Reset(f);  //открытие для чтения, текущая 1-ая позиция
      FOR i:=1 TO n DO
        begin
          read(f,x); //чтение элемента из файла
          StringGrid2.Cells[0,i]:=FloatToStrf(x,fffixed,1,0); //вещественный в строковый
                                                             //формат с фиксированным
                                                             // количеством символов-1,
                                                             // и количеством символов-0
        end;
      closeFile(f);// закрытие файла
end;

procedure TForm1.Button3Click(Sender: TObject);
begin
  close;
end;

procedure TForm1.RadioButton4Click(Sender: TObject);
begin
    Form1.Color:=clAqua;
end;

procedure TForm1.RadioButton5Click(Sender: TObject);
begin
    Form1.Color:=clTeal;
end;

procedure TForm1.RadioButton6Click(Sender: TObject);
begin
    Form1.Color:=clYellow;
end;

procedure TForm1.RadioButton7Click(Sender: TObject);
begin
    Form1.Color:=clLime;
end;

procedure TForm1.RadioButton8Click(Sender: TObject);
begin
    Form1.Color:=clPurple;
end;

procedure TForm1.RadioButton9Click(Sender: TObject);
begin
    Form1.Color:=clWhite;
end;

END.
