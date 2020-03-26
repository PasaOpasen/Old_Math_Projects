unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, Grids, StdCtrls;

type
  TForm1 = class(TForm)
    StringGrid1: TStringGrid;
    StringGrid2: TStringGrid;
    StringGrid3: TStringGrid;
    Label1: TLabel;
    Edit1: TEdit;
    Button2: TButton;
    Label2: TLabel;
    Button1: TButton;
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


implementation

{$R *.dfm}

procedure TForm1.StringGrid1Click(Sender: TObject);
begin

end;
 //������� ��������� ��������
procedure TForm1.FormCreate(Sender: TObject);
var i,j,n0: integer;
begin
  n0:=3;
  Edit1.Text:=FloatToStr(n0);
  StringGrid1.ColCount:=n0+1;
  StringGrid1.RowCount:=n0+1;
  StringGrid2.RowCount:=n0+1;
  StringGrid3.RowCount:=n0+1;
  StringGrid1.Cells[0,0]:='Matrix A:';
  StringGrid2.Cells[0,0]:='Vector B:';
  StringGrid3.Cells[0,0]:='Vector Y:';

  for i:=1 to n0 do
  begin
    StringGrid1.Cells[0,i]:='i='+intToStr(i);
    StringGrid1.Cells[i,0]:='j='+intToStr(i);
  end;
end;
//��������� ������� ������� "�������������"
procedure TForm1.Button2Click(Sender: TObject);
var i,j,n,n0: integer;
begin
   try n:=StrToInt(Edit1.Text)
    except ShowMessage('Error!');
    Exit;
  end;
  n0:=3;
  if n<>n0 then
  for i:=1 to n0 do
    begin
      StringGrid2.Cells[0,i]:=' ';
      StringGrid3.Cells[0,i]:=' ';
      for j:=1 to n0 do
      StringGrid1.Cells[i,j]:=' ';
    end;
  n0:=n;

  StringGrid1.ColCount:=n0+1;
  StringGrid1.RowCount:=n0+1;
  StringGrid2.RowCount:=n0+1;
  StringGrid3.RowCount:=n0+1;
  StringGrid1.Cells[0,0]:='Matrix A:';
  StringGrid2.Cells[0,0]:='Vector B:';
  StringGrid3.Cells[0,0]:='Vector Y:';

  for i:=1 to n0 do
  begin
    StringGrid1.Cells[0,i]:='i='+intToStr(i);
    StringGrid1.Cells[i,0]:='j='+intToStr(i);
  end;


end;
//��������� ������� ������� "A*b"
procedure TForm1.Button1Click(Sender: TObject);
type
  Matr = array [1..10,1..10] of real;
  Vct = array[1..10] of real;
var
  i,j,n: integer;
  A: Matr;
  b,y:Vct;
begin
  try n:=StrToInt(Edit1.Text)
    except ShowMessage('Error!');
  Exit;
  end;
  for i:=1 to n do for j:=1 to n do
  try A[i,j]:=StrToFloat(StringGrid1.Cells[j,i])
    except ShowMessage('Error!');
    Exit;
  end;
  
  for i:=1 to n do 
  try b[i]:=StrToFloat(StringGrid2.Cells[0,i])
    except ShowMessage('Error!');
    Exit;
  end;
 
  for i:=1 to n do
  begin
    y[i]:=0;
    for j:=1 to n do y[i]:=y[i]+A[i,j]*b[j];
    StringGrid3.Cells[0,i]:=FloatToStrf(y[i],fffixed,6,2);
  end;
end;
end.


