unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, Grids, StdCtrls, ExtCtrls;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    edt1: TEdit;
    btn1: TButton;
    strngrd1: TStringGrid;
    strngrd2: TStringGrid;
    strngrd3: TStringGrid;
    btn2: TButton;
    RadioGroup1: TRadioGroup;
    Label2: TLabel;
    procedure FormCreate(Sender: TObject);
    procedure btn1Click(Sender: TObject);
    procedure btn2Click(Sender: TObject);
    procedure RadioGroup1Click(Sender: TObject);
    procedure btnFilleClick(Sender: TObject);
    procedure btnKeyboardClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;
type
  Matr = array[1..10,1..10] of real;
  Vct = array[1..10] of Real;
var
  Form1: TForm1;
  n : Integer;
  
implementation

{$R *.dfm}

procedure TForm1.FormCreate(Sender: TObject);
var i,j:Integer;
begin
  // задаем первоначальные данные
  n:=3; //Размерность массива
  edt1.Text:=FloatToStr(n); // выводим на форму
  for j:=1 to n do
  for i:=1 to n do
  begin
    strngrd1.Cells[i,j]:=intToStr(i+j);
    strngrd2.Cells[0,i]:=intToStr(i);
  end;
  // задание числа столбцов и строк в таблицах
  strngrd1.ColCount:=n+1;
  strngrd1.RowCount:=n+1;
  strngrd2.RowCount:=n+1;
  strngrd3.RowCount:=n+1;
  // Заполнение левой верхней ячейки таблиц названиями
  strngrd1.Cells[0,0]:=' А';
  strngrd2.Cells[0,0]:='Вектор B:';
  strngrd3.Cells[0,0]:='Вектор Y:';
  // Заполнение первых стoлбца и строки матрицы А поясняющими надписями i =1,2..n; j=1,2..n
  for i:=1 to n do
  begin
    strngrd1.Cells[0,i]:='i='+intToStr(i);
    strngrd1.Cells[i,0]:='j='+intToStr(i);
  end;
end;

// Кнопка : "Задать размерность!"
procedure TForm1.btn1Click(Sender: TObject);
var i,j:Integer;
begin
  try
    n:=StrToInt(edt1.Text);
    if (n<=0) or (n>10) then
      begin
        i:=0;
        i:=1 div i;
      end;
    except
    on EConvertError do
    ShowMessage('Не верно введена размерность матрицы и вектора');
    on EDivByZero do
    ShowMessage('Не возможно построить матрицy и вектор при n<0 или при n>10');
  end;
  

  //Задание числа строк и столбцов в таблицах
  strngrd1.ColCount:=n+1;
  strngrd1.RowCount:=n+1;
  strngrd2.RowCount:=n+1;
  strngrd3.RowCount:=n+1;
  // Заполнение первых столбца и строки поясняющими надписями i =1,2..n; j=1,2..n
  for i:=1 to n do
  begin
    strngrd1.Cells[0,i]:='i='+IntToStr(i);
    strngrd1.Cells[i,0]:='j='+IntToStr(i);
  end;
  // Очищение ячеек
  for i:=1 to n do for j:=1 to n do
  begin
   strngrd1.Cells[i,j]:='';
   strngrd2.Cells[0,i]:='';
   strngrd3.Cells[0,i]:='';
  end;
end;

// Кнопка: "A*b="
procedure TForm1.btn2Click(Sender: TObject);
var A:Matr; // объявление двумерного массива
    b,y:Vct; //  Объявление одномерных массивов
    i,j:Integer;
begin
   try

    // Заполненее массива А элементами из таблицы strngrd1
    for i:=1 to n do for j:=1 to n do
      A[j,i]:=StrToFloat(strngrd1.Cells[i,j]);

    // Заполненее массива b элементами из таблицы strngrd2
    for i:=1 to n do b[i]:=StrToFloat(strngrd2.Cells[0,i]);
   except ShowMessage('Не верно введены элементы матрицы или вектора');
           Exit;
   end;

   //Умножение матрицы A на вектор b
   for i:=1 to n do
   begin
     y[i]:=0;
     for j:=1 to n do y[i]:=y[i]+A[i,j]*b[j];
     //Вывод результата в таблицу strngrd3
     strngrd3.Cells[0,i]:=FloatToStrF(y[i],ffFixed,6,2);
   end;
end;

// считать матрицу из файла
procedure TForm1.RadioGroup1Click(Sender: TObject);
var f:TextFile;
    i,j: Byte;
    x:Integer;
begin
  n:=RadioGroup1.ItemIndex+3; //размерность матрицы
  strngrd1.ColCount:=n+1;
  strngrd1.RowCount:=n+1;
  strngrd2.RowCount:=n+1;
  strngrd3.RowCount:=n+1;

    //запрещаем ввод с клавиатуры
  strngrd1.Enabled:=True;
  strngrd2.Enabled:=True;
  // Очищение ячеек
  for i:=1 to n do for j:=1 to n do
  begin
   strngrd1.Cells[i,j]:='';
   strngrd2.Cells[0,i]:='';
   strngrd3.Cells[0,i]:='';
  end;

  for i:=1 to n+1 do
  begin
    strngrd1.Cells[0,i]:='i='+IntToStr(i);
    strngrd1.Cells[i,0]:='j='+IntToStr(i);
  end;

  AssignFile(f,IntToStr(n)+'.txt');
  Reset(f);

  // считали матрицу
  for i:=1 to n do
  begin
    for j:=1 to n do
    begin
      read(f,x);
      strngrd1.Cells[j,i]:=IntToStr(x);
    end;
  end;

  // считали вектор
  for j:=1 to n do
  begin
    read(f,x);
    strngrd2.Cells[0,j]:=IntToStr(x);
  end;
  
  CloseFile(f);
end;

procedure TForm1.btnFilleClick(Sender: TObject);
var i,j:Byte;
begin
  //запрещаем ввод с клавиатуры
  strngrd1.Enabled:=False;
  strngrd2.Enabled:=False;
  // Очищение ячеек
  for i:=1 to n do for j:=1 to n do
  begin
   strngrd1.Cells[i,j]:='';
   strngrd2.Cells[0,i]:='';
   strngrd3.Cells[0,i]:='';
  end;
end;

procedure TForm1.btnKeyboardClick(Sender: TObject);
var i,j:Byte;
begin
  //запрещаем ввод с клавиатуры
  strngrd1.Enabled:=True;
  strngrd2.Enabled:=True;
  // Очищение ячеек
  for i:=1 to n do for j:=1 to n do
  begin
   strngrd1.Cells[i,j]:='';
   strngrd2.Cells[0,i]:='';
   strngrd3.Cells[0,i]:='';
  end;
end;

end.
