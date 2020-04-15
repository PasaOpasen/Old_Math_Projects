unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls, Grids, Menus;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    Edit1: TEdit;
    Edit2: TEdit;
    Button1: TButton;
    A: TStringGrid;
    RadioGroup1: TRadioGroup;
    RB1: TRadioButton;
    RB2: TRadioButton;
    RB3: TRadioButton;
    RB4: TRadioButton;
    Button2: TButton;
    B: TStringGrid;
    Label2: TLabel;
    Label3: TLabel;
    Label4: TLabel;
    Label5: TLabel;
    OpenDialog1: TOpenDialog;
    SaveDialog1: TSaveDialog;
    MainMenu1: TMainMenu;
    N1: TMenuItem;
    procedure Button1Click(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure Button2Click(Sender: TObject);
    procedure N1Click(Sender: TObject);

    private { Private declarations }
  public{ Public declarations }
  end;

var
  Form1: TForm1;

implementation

{$R *.DFM}


procedure TForm1.FormCreate(Sender: TObject);
var n,m,i,j: integer;
begin
  n:=StrToInt(Edit1.Text);
  m:=StrToInt(Edit2.Text);
  //Задание числа строк и столбцов в таблице
  A.RowCount:=n+1;
  A.ColCount:=m+1;
  B.ColCount:=A.ColCount;
  B.RowCount:=A.RowCount;
  //Заполнение левой верхней ячейки матрицы названием
  A.Cells[0,0]:='A';
  B.Cells[0,0]:='B';
  //Заполнение первых столбца и строки поясняющими надписями i(j)=1,2,...n(m)
  for j:=1 to n do for i:=1 to m do
  begin
    A.Cells[0,j]:='i= '+IntToStr(j);
    A.Cells[i,0]:='j= '+IntToStr(i);
    B.Cells[0,j]:=A.Cells[0,j];
    B.Cells[i,0]:=A.Cells[i,0];
  end;
end;

//Обработка нажатия "Задать размерность матрицы"
procedure TForm1.Button1Click(Sender: TObject);
var n, m, i, j: integer;
begin
    try //начало защищенного блока
    n:=StrToInt(Edit1.Text);
    m:=StrToInt(Edit2.Text);
    if n <> 0 then
    if m <> 0 then
//Задание числа строк и столбцов в таблице
    A.RowCount:=n+1;
    A.ColCount:=m+1;
    B.ColCount:=A.ColCount;
    B.RowCount:=A.RowCount;
//Заполнение первых строки и столбца матрицы поясняющими записями i(j)=1,2..n(m)
    for j:=1 to n do
    for i:=1 to m do
    begin
      A.Cells[0,j]:='i= '+IntToStr(j);
      A.Cells[i,0]:='j= '+IntToStr(i);
      B.Cells[0,j]:=A.Cells[0,j];
      B.Cells[i,0]:=A.Cells[i,0];
    end;
   except//защищенный блок выводит ошибку при вводе всего кроме положит.вещ.числа
    ShowMessage('Ошибка при вводе размерности столбца или строки');
    Edit1.Text:=' ';
   end;
end;

procedure TForm1.Button2Click(Sender: TObject);
var
  n, m, i, j: integer;
  pos:Array of Array of Integer;//вспомог. матрица с результир. позициями строк
  buff_str:Array of Integer;//вспомог. массив для сохранения строки при сорт.
  buff, min: Integer;//вспомог. переменная для сохранения значения при сорт.
  fz:textfile;

begin

  n := StrToInt(Edit1.Text); //инициализируем количество строк
  m := StrToInt(Edit2.Text); //инициализируем количество столбцов

  SetLength(pos, 2, n); //задание размеров динамического массива
  SetLength(buff_str, m); //задание размеров динамического массива

  if RB1.Checked = TRUE then
  begin
    for i:=0 to n-1 do //получаем неотсортированную матрицу
    begin
      pos[0,i] := StrToInt(A.Cells[1,i+1]);
      pos[1,i] := i+1;
    end;

  end;
  if RB2.Checked = TRUE then
  begin
    for i:=0 to n-1 do
    begin
      pos[0,i] := 0;
      for j:=0 to m-1 do
      begin
        pos[0,i] := pos[0,i] + StrToInt(A.Cells[j+1,i+1]);
      end;
      pos[1,i] := i+1;
    end;
  end;
  if RB3.Checked = TRUE then
  begin
    for i:=0 to n-1 do//получаем неотсортированную матрицу
    begin
      pos[0,i] := StrToInt(A.Cells[1,i+1]);
      for j:=0 to m-1 do
      begin
        if pos[0,i] < StrToInt(A.Cells[j+1,i+1]) then
        begin
          pos[0,i] := StrToInt(A.Cells[j+1,i+1]);
        end;
      end;
      pos[1,i] := i+1;
    end;
  end;
  if RB4.Checked = TRUE then
  begin
    for i:=0 to n-1 do//получаем неотсортированную матрицу
    begin
      pos[0,i] := StrToInt(A.Cells[1,i+1]);
      for j:=0 to m-1 do
      begin
        if pos[0,i] > StrToInt(A.Cells[j+1,i+1]) then
        begin
          pos[0,i] := StrToInt(A.Cells[j+1,i+1]);
        end;
      end;

      pos[1,i] := i+1;
    end;
  end;
   for i:=0 to n-2 do//сортируем вспомогательную матрицу результир.позиций строк
    begin//для сортировки используется метод поиска минимума
      min:=i;
      for j:=i+1 to n-1 do
      begin
          if pos[0,j]<pos[0,min] then min:=j;
      end;
      buff:=pos[0,min];
      pos[0,min]:=pos[0,i];
      pos[0,i]:=buff;

      buff:=pos[1,min];
      pos[1,min]:=pos[1,i];
      pos[1,i]:=buff;
    end;
  for i:=1 to n do
  begin
    for j:=1 to m do
    begin
      B.Cells[j,i] := A.Cells[j, pos[1,i-1]];
    end;
  end;

AssignFile(fz,'output.txt');      //запись в файл
rewrite(fz);
for i:=1 to n do
begin
  for j:=1 to m do Write(fz,B.Cells[j,i],' ');
  WriteLn(fz);
end;
closeFile(fz);

end;

procedure TForm1.N1Click(Sender: TObject);
var fz:textfile; i,j,n,m:integer; c:real;
begin
 if OpenDialog1.Execute then
 begin
   AssignFile(fz,OpenDialog1.Filename);
   Reset(fz);
      Read(fz,n,m);    //считать размерность матрицы
      Edit1.Text:=IntToStr(n);
      Edit2.Text:=IntToStr(m);
      A.RowCount:=n+1;
      A.ColCount:=m+1;
      B.ColCount:=A.ColCount;
      B.RowCount:=A.RowCount;

      for i:=1 to n do for j:=1 to m do //заполнить матрицу
      begin
        Read(fz,c);
        A.Cells[j,i]:=FloatToStr(c);
      end;

    closefile(fz);
 end;

end;

end.













