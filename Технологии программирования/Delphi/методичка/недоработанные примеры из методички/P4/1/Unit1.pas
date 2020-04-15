unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, Grids, ExtCtrls, Menus;

type
  TForm1 = class(TForm)
    StringGrid1: TStringGrid;
    StringGrid2: TStringGrid;
    StringGrid3: TStringGrid;
    Button2: TButton;
    RadioGroup1: TRadioGroup;
    MainMenu1: TMainMenu;
    N1: TMenuItem;
    OpenDialog1: TOpenDialog;
    procedure FormCreate(Sender: TObject);
    procedure Button2Click(Sender: TObject);
    procedure RadioGroup1Click(Sender: TObject);
    procedure N1Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;
type Matr=array [1..10,1..10] of real;
     vect=array [1..10] of real;


var
  Form1: TForm1;
  A:Matr;
  y,B:Vect;
  n:integer;

implementation

{$R *.dfm}

procedure TForm1.FormCreate(Sender: TObject);
var i,j:integer;
begin
 n:=0;
 {StringGrid1.ColCount:=n+1;
 StringGrid1.RowCount:=n+1;
 StringGrid2.RowCount:=n+1;
 StringGrid3.RowCount:=n+1;
 StringGrid1.Cells[0,0]:='Матрица A';
 StringGrid2.Cells[0,0]:='Вектор B';
 StringGrid3.Cells[0,0]:='Вектор Y';
 for i:=1 to n do
  begin
   StringGrid1.Cells[0,i]:=IntToStr(i);
   StringGrid1.Cells[i,0]:=IntToStr(i);
  end;}
end;



procedure TForm1.Button2Click(Sender: TObject);
var i,j:integer;
begin
 for i:=1 to n do
  for j:=1 to n do
   A[i,j]:=StrToFloat(StringGrid1.Cells[j,i]);
 for i:=1 to n do
  B[i]:=StrToFloat(StringGrid2.Cells[0,i]);
 for i:=1 to n do
  begin
   y[i]:=0;
   for j:=1 to n do y[i]:=y[i]+A[i,j]*B[j];
   StringGrid3.Cells[0,i]:=FloatToStrF(y[i],fffixed,6,2);
  end;
end;


procedure TForm1.RadioGroup1Click(Sender: TObject);
var i:integer;
begin
 case RadioGroup1.ItemIndex of
  0: n:=3;
  1: n:=5;
  2: n:=7;
 end;
 StringGrid1.ColCount:=n+1;
 StringGrid1.RowCount:=n+1;
 StringGrid2.RowCount:=n+1;
 StringGrid3.RowCount:=n+1;
 StringGrid1.Cells[0,0]:='Матрица A';
 StringGrid2.Cells[0,0]:='Вектор B';
 StringGrid3.Cells[0,0]:='Вектор Y';
 for i:=1 to n do
 begin
  StringGrid1.Cells[0,i]:=IntToStr(i);
  StringGrid1.Cells[i,0]:=IntToStr(i);
 end;
end;

procedure TForm1.N1Click(Sender: TObject);
var t:textfile;
    i,j,x:integer;
begin
 if n<=0 then
  begin
   ShowMessage('Выберите размерность чтобы продолжить');
   exit;
  end;
 if OpenDialog1.Execute then
  begin
   assignFile(t,OpenDialog1.fileName);
   reset(t);
   for i:=1 to n do
    begin
     for j:=1 to n do
      begin
       try
         read(t,x)
       except
         showMessage('Не правильный ввод!!!');
         exit;
       end;
       StringGrid1.Cells[j,i]:=IntToStr(x);
      end;
      try
        read(t,x)
      except
        showMessage('Не правильный ввод!!!');
        exit;
      end;
     StringGrid2.Cells[0,i]:=IntToStr(x);
    end;
  end;
end;

end.
