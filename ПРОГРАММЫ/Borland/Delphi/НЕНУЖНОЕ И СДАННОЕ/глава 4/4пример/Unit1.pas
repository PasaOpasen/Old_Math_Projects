unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, Grids, StdCtrls, Menus;

type
  TForm1 = class(TForm)
    StringGrid1: TStringGrid;
    StringGrid2: TStringGrid;
    StringGrid3: TStringGrid;
    Button1: TButton;
    Label1: TLabel;
    Edit1: TEdit;
    Button2: TButton;
    Label2: TLabel;
    OpenDialog1: TOpenDialog;
    SaveDialog1: TSaveDialog;
    MainMenu1: TMainMenu;
    N1: TMenuItem;
    N2: TMenuItem;
    N3: TMenuItem;
    N4: TMenuItem;
    D1: TMenuItem;
    procedure StringGrid1Click(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure Button2Click(Sender: TObject);
    procedure Button1Click(Sender: TObject);
    procedure D1Click(Sender: TObject);
    
  private
    { Private declarations }
  public
    { Public declarations }
  end;
  // объ€вление типов
type
  Matr = array [1..10,1..10] of real;
  Vct = array[1..10] of real;

var
  Form1: TForm1;
  //объ€вление массивов
  A: Matr;
  b,y:Vct;
  n,i,j: integer;

implementation

{$R *.dfm}

procedure TForm1.StringGrid1Click(Sender: TObject);
begin

end;
 //задание начальных значений
procedure TForm1.FormCreate(Sender: TObject);
begin
  n:=3;

  Edit1.Text:=FloatToStr(n);
  StringGrid1.ColCount:=n+1;
  StringGrid1.RowCount:=n+1;
  StringGrid2.RowCount:=n+1;
  StringGrid3.RowCount:=n+1;
  StringGrid1.Cells[0,0]:='Matrix A:';
  StringGrid2.Cells[0,0]:='Vector B:';
  StringGrid3.Cells[0,0]:='Vector Y:';

  for i:=1 to n do
  begin
    StringGrid1.Cells[0,i]:='i='+intToStr(i);
    StringGrid1.Cells[i,0]:='j='+intToStr(i);
  end;
end;
//обработка нажати€ клавиши "«афиксировать"
procedure TForm1.Button2Click(Sender: TObject);
begin
  // Edit1:=из файла

  n:=StrToInt(Edit1.Text);
  StringGrid1.ColCount:=n+1;
  StringGrid1.RowCount:=n+1;
  StringGrid2.RowCount:=n+1;
  StringGrid3.RowCount:=n+1;
  StringGrid1.Cells[0,0]:='Matrix A:';
  StringGrid2.Cells[0,0]:='Vector B:';
  StringGrid3.Cells[0,0]:='Vector Y:';

  for i:=1 to n do
  begin
    StringGrid1.Cells[0,i]:='i='+intToStr(i);
    StringGrid1.Cells[i,0]:='j='+intToStr(i);
  end;
end;
//обработка нажати€ клавиши "A*b"
procedure TForm1.Button1Click(Sender: TObject);
var f:textfile;
begin
 assignFile(f,'output.txt');
 rewrite(f);
 //вычислени€
 for i:=1 to n do for j:=1 to n do A[i,j]:=StrToFloat(StringGrid1.Cells[j,i]);
 for i:=1 to n do b[i]:=StrToFloat(StringGrid2.Cells[0,i]);
 //вывод
 for i:=1 to n do
  begin
    y[i]:=0;
    for j:=1 to n do y[i]:=y[i]+A[i,j]*b[j];
    StringGrid3.Cells[0,i]:=FloatToStrf(y[i],fffixed,6,2);
    Writeln(f,StringGrid3.Cells[0,i]);
  end;
  closefile(f);
end;


procedure TForm1.D1Click(Sender: TObject);
begin
  if OpenDialog1/Execute then
  begin
    AssignFile(fz,OpenDialog1.Filename);
    Reset(Fz);
    
  end;
end;

end.


