unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, Grids, StdCtrls;

type
  TForm1 = class(TForm)
    StringGrid1: TStringGrid;
    Label1: TLabel;
    Edit1: TEdit;
    Button2: TButton;
    Label2: TLabel;
    Button1: TButton;
    StringGrid2: TStringGrid;
    Edit2: TEdit;
    Memo1: TMemo;
    procedure StringGrid1Click(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure Button2Click(Sender: TObject);
    procedure Button1Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;
  fz:textfile;
  n,n0,m,m0: integer;

implementation

{$R *.dfm}

procedure TForm1.StringGrid1Click(Sender: TObject);
begin

end;
 //задание начальных значений
procedure TForm1.FormCreate(Sender: TObject);
var i,j: integer;
begin
  n0:=3;
  m0:=2;
  Edit1.Text:=FloatToStr(n0);
  Edit2.Text:=FloatToStr(m0);
  StringGrid1.ColCount:=m0+1;
  StringGrid1.RowCount:=n0+1;
  StringGrid2.ColCount:=m0+1;
  StringGrid2.RowCount:=n0+1;
  StringGrid1.Cells[0,0]:='Matrix A:';
  StringGrid2.Cells[0,0]:='Matrix B:';

  for j:=1 to n0 do
  begin
    StringGrid1.Cells[0,j]:='i='+intToStr(j);
    StringGrid2.Cells[0,j]:='i='+intToStr(j);
  end;
  for i:=1 to m0 do
  begin
    StringGrid1.Cells[i,0]:='j='+intToStr(i);
    StringGrid2.Cells[i,0]:='j='+intToStr(i);
  end;
end;


//обработка нажатия клавиши "Зафиксировать"
procedure TForm1.Button2Click(Sender: TObject);
var i,j: integer;
begin
   try n:=StrToInt(Edit1.Text)
    except ShowMessage('Error!');
    Exit;
  end;
    try m:=StrToInt(Edit2.Text)
    except ShowMessage('Error!');
    Exit;
  end;

  if (n<>n0) or (m<>m0) then
  for i:=1 to m0 do
      for j:=1 to n0 do
      begin
        StringGrid1.Cells[i,j]:=' ';
        StringGrid2.Cells[i,j]:=' ';
      end;
  n0:=n;m0:=m;

  StringGrid1.ColCount:=m0+1;
  StringGrid1.RowCount:=n0+1;
  StringGrid2.ColCount:=m0+1;
  StringGrid2.RowCount:=n0+1;
  StringGrid1.Cells[0,0]:='Matrix A:';
  StringGrid2.Cells[0,0]:='Matrix B:';

  for i:=1 to n0 do
  begin
    StringGrid1.Cells[0,i]:='i='+intToStr(i);
    StringGrid2.Cells[0,i]:='i='+intToStr(i);
  end;
  for i:=1 to m0 do
  begin
    StringGrid1.Cells[i,0]:='j='+intToStr(i);
    StringGrid2.Cells[i,0]:='j='+intToStr(i);
  end;
end;


//обработка нажатия клавиши "Запуск"
procedure TForm1.Button1Click(Sender: TObject);
type
  Matr = array [1..10,1..10] of real;
var
  i,j,k,t: integer;
  A,B: Matr;
begin
  	

  for i:=1 to n do for j:=1 to m do
  try A[j,i]:=StrToFloat(StringGrid1.Cells[j,i])
    except ShowMessage('Error!');
    Exit;
  end;
  for i:=1 to n do for j:=1 to m do
  try B[j,i]:=StrToFloat(StringGrid2.Cells[j,i])
    except ShowMessage('Error!');
    Exit;
  end;

 //в
  for i:=1 to m do
   for j:=1 to m do
   if A[i,1]=B[j,1] then //если равны первые элементы столбца, то происходит проверка по остальным
   begin
    k:=1;
    for t:=2 to n do
     if A[i,t]=B[j,t] then k:=k+1;
    if k=n then Memo1.Lines.Add(IntToStr(i)+'-th column of the matrix A coincides with the '+IntToStr(j)+'-th column of the matrix B');
   end;

 //г
   k:=0;
   for i:=2 to n-1 do
    for j:=2 to m-1 do
    if A[j,i]=B[j,i] then k:=k+1;//проверяется совпадение в центральной части
   if k=(m-2)*(n-2) then //если центры матриц совпадают, то выясняется более подробно
   begin
      if A[1,1]=B[1,1] then
      begin
         k:=0;
         for i:=2 to n-1 do if A[1,i]=B[1,i] then k:=k+1;
         for j:=2 to m-1 do if A[j,1]=B[j,1] then k:=k+1;
         if k=(n-2)+(m-2) then Memo1.Lines.Add('the minors of the elements in the bottom right corner');
      end;
      if A[m,n]=B[m,n] then
      begin
         k:=0;
         for i:=2 to n-1 do if A[m,i]=B[m,i] then k:=k+1;
         for j:=2 to m-1 do if A[j,n]=B[j,n] then k:=k+1;
         if k=(n-2)+(m-2) then Memo1.Lines.Add('the minors of the elements in the upper left corner');
      end;
      if A[m,1]=B[m,1] then
      begin
         k:=0;
         for i:=2 to n-1 do if A[m,i]=B[m,i] then k:=k+1;
         for j:=2 to m-1 do if A[j,1]=B[j,1] then k:=k+1;
         if k=(n-2)+(m-2) then Memo1.Lines.Add('the minors of the elements in the bottom left corner');
      end;
      if A[1,n]=B[1,n] then
      begin
         k:=0;
         for i:=2 to n-1 do if A[1,i]=B[1,i] then k:=k+1;
         for j:=2 to m-1 do if A[j,n]=B[j,n] then k:=k+1;
         if k=(n-2)+(m-2) then Memo1.Lines.Add('the minors of the elements in the upper right corner');
      end;
    end;
end;

end.

{
//îáðàáîòêà íàæàòèÿ êëàâèøè "A*b"
procedure TForm1.Button1Click(Sender: TObject);
var f:textfile;
begin
 assignFile(f,'output.txt');
 rewrite(f);
 //âû÷èñëåíèÿ
 for i:=1 to n do for j:=1 to n do A[i,j]:=StrToFloat(StringGrid1.Cells[j,i]);
 for i:=1 to n do b[i]:=StrToFloat(StringGrid2.Cells[0,i]);
 //âûâîä
 for i:=1 to n do
  begin
    y[i]:=0;
    for j:=1 to n do y[i]:=y[i]+A[i,j]*b[j];
    StringGrid3.Cells[0,i]:=FloatToStrf(y[i],fffixed,6,2);
    Writeln(f,StringGrid3.Cells[0,i]);
  end;
  closefile(f);
end;

   //ïðî÷åñòü èç ôàéëà âñ¸
procedure TForm1.D1Click(Sender: TObject);
var a:real; n:integer;
begin
  if OpenDialog1.Execute then
  begin
    AssignFile(fz,OpenDialog1.Filename);
    Reset(fz);

    while not seekeof(fz) do
    begin
      Read(fz,n);
      Edit1.Text:=IntToStr(n);

      for i:=1 to n do for j:=1 to n do
      begin
        Read(fz,a);
        StringGrid1.Cells[j,i]:=FloatToStr(a);
      end;

      for i:=1 to n do
      begin
        Read(fz,a);
        StringGrid2.Cells[0,i]:=FloatToStr(a);
      end;
    end;

    closefile(fz);
  end;
end;
}
