unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls, Grids, Xpman;

type
  TForm1 = class(TForm)
    StringGrid1: TStringGrid;
    StringGrid2: TStringGrid;
    StringGrid3: TStringGrid;
    Label1: TLabel;
    Edit1: TEdit;
    Button1: TButton;
    RadioGroup1: TRadioGroup;
    RadioButton1: TRadioButton;
    RadioButton2: TRadioButton;
    RadioButton3: TRadioButton;
    RadioButton4: TRadioButton;
    RadioButton5: TRadioButton;
    Button2: TButton;
    procedure FormCreate(Sender: TObject);
    procedure Button1Click(Sender: TObject);
    procedure Button2Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;
type vect=array [1..100] of integer;
var
  Form1: TForm1;
  a,b,c:vect;
  n:integer;
implementation

{$R *.dfm}

procedure TForm1.FormCreate(Sender: TObject);
begin
 n:=3;
 Edit1.Text:='3';
 StringGrid1.RowCount:=n+1;
 StringGrid2.RowCount:=n+1;
 StringGrid3.RowCount:=n+1;
 StringGrid1.Cells[0,0]:='A';
 StringGrid2.Cells[0,0]:='B';
 StringGrid3.Cells[0,0]:='C';
 RadioButton1.Checked:=true;
end;

procedure TForm1.Button1Click(Sender: TObject);
begin
 n:=StrToInt(Edit1.Text);
 StringGrid1.RowCount:=n+1;
 StringGrid2.RowCount:=n+1;
 StringGrid3.RowCount:=n+1;
end;

procedure TForm1.Button2Click(Sender: TObject);
var i,j:integer;
begin
 for i:=1 to n do
  begin
   a[i]:=StrToInt(StringGrid1.Cells[0,i]);
   b[i]:=StrToInt(StringGrid1.Cells[0,i]);
  end;
 if RadioButton1.Checked then
   for i:=1 to n do
     StringGrid3.Cells[0,i]:=IntToStr(a[i]*b[i]);

 if RadioButton2.Checked then
  for i:=1 to n do StringGrid3.Cells[0,i]:=IntToStr(a[i]+b[i]);

 if RadioButton4.Checked then
  begin
   StringGrid3.RowCount:=n*2+1;
   for i:=1 to n do
    StringGrid3.Cells[0,i]:=IntToStr(a[i]);
   for i:=n+1 to 2*n do
    StringGrid3.Cells[0,i]:=IntToStr(b[i-n]);
   end;

 if RadioButton5.Checked then
  begin
   StringGrid3.RowCount:=n*2+1;
   for i:=1 to 2*n do
    begin
     StringGrid3.Cells[0,2*i]:=IntToStr(b[i]);
     StringGrid3.Cells[0,2*i-1]:=IntToStr(a[i]);
    end;
   end;
end;

end.
