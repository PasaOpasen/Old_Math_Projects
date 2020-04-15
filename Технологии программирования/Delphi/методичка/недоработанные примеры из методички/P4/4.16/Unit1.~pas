unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, Grids, StdCtrls, ExtCtrls, Menus;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    Label2: TLabel;
    Edit1: TEdit;
    Edit2: TEdit;
    Button1: TButton;
    Label3: TLabel;
    Edit3: TEdit;
    RadioButton1: TRadioButton;
    RadioButton2: TRadioButton;
    Button2: TButton;
    StringGrid1: TStringGrid;
    StringGrid2: TStringGrid;
    StringGrid3: TStringGrid;
    Label4: TLabel;
    MainMenu1: TMainMenu;
    N1: TMenuItem;
    N2: TMenuItem;
    OpenDialog1: TOpenDialog;
    SaveDialog1: TSaveDialog;
    procedure FormCreate(Sender: TObject);
    procedure Button1Click(Sender: TObject);
    procedure Button2Click(Sender: TObject);
    procedure N1Click(Sender: TObject);
    procedure N2Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;
type matr=array [1..10,1..10] of real;
var
  Form1: TForm1;
  a,b,c:matr;
  n,m,p:integer;
implementation

{$R *.dfm}

procedure TForm1.FormCreate(Sender: TObject);
 var i,j:integer;
begin
 Edit1.Text:='4';
 Edit2.Text:='4';
 Edit3.Text:='4';
 n:=StrToInt(Edit1.Text);
 m:=StrToInt(Edit2.Text);
 p:=StrToInt(Edit3.Text);
 StringGrid1.Cells[0,0]:='A';
 StringGrid2.Cells[0,0]:='B';
 StringGrid3.Cells[0,0]:='C';
 for i:=1 to n do
  begin
   StringGrid1.Cells[i,0]:=IntToStr(i);
   StringGrid1.Cells[0,i]:=IntToStr(i);
   StringGrid2.Cells[i,0]:=IntToStr(i);
   StringGrid2.Cells[0,i]:=IntToStr(i);
   StringGrid3.Cells[i,0]:=IntToStr(i);
   StringGrid3.Cells[0,i]:=IntToStr(i);
  end;
 radioButton1.Checked:=true;
end;

procedure TForm1.Button1Click(Sender: TObject);
var i,j:integer;
begin
 if RadioButton1.Checked then
  begin
    n:=StrToInt(Edit1.Text);
    m:=StrToInt(Edit1.Text);
    p:=StrToInt(Edit1.Text);
    StringGrid1.ColCount:=n+1;
    StringGrid1.RowCount:=n+1;
    StringGrid2.ColCount:=n+1;
    StringGrid2.RowCount:=n+1;
    StringGrid3.ColCount:=n+1;
    StringGrid3.RowCount:=n+1;
  end;
 if RadioButton2.Checked then
  begin
    n:=StrToInt(Edit1.Text);
    m:=StrToInt(Edit2.Text);
    p:=StrToInt(Edit3.Text);
    StringGrid1.ColCount:=m+1;
    StringGrid1.RowCount:=n+1;
    StringGrid2.ColCount:=p+1;
    StringGrid2.RowCount:=m+1;
    StringGrid3.ColCount:=n+1;
    StringGrid3.RowCount:=p+1;
  end;
end;

procedure TForm1.Button2Click(Sender: TObject);
 var i,j,k:integer;
     sum:real;
begin
  for i:=1 to n do
    for j:=1 to m do
      c[i,j]:=0;
  for i:=1 to n do
   for j:=1 to m do
    A[i,j]:=StrToFloat(StringGrid1.Cells[j,i]);
  for i:=1 to m do
   for j:=1 to p do
    B[i,j]:=StrToFloat(StringGrid2.Cells[j,i]);
  for i:=1 to n do
   for j:=1 to p do
    begin
     for k:=1 to m do C[i,j]:=C[i,j]+A[i,k]*B[k,j];
    end;
  for i:=1 to n do
   for j:=1 to p do
    StringGrid3.Cells[j,i]:=FloatToStrF(C[i,j],fffixed,6,1);
end;

procedure TForm1.N1Click(Sender: TObject);
var txt:TextFile;
    i,j:integer;
    scan:real;
begin
 if OpenDialog1.Execute then
  begin
    assignFile(txt,OpenDialog1.FileName);
    reset(txt);
    read(txt,n);
    read(txt,m);
    read(txt,p);
    StringGrid1.ColCount:=m+1;
    StringGrid1.RowCount:=n+1;
    StringGrid2.ColCount:=p+1;
    StringGrid2.RowCount:=m+1;
    StringGrid3.ColCount:=n+1;
    StringGrid3.RowCount:=p+1;
    for i:=1 to n do
     begin
      for j:=1 to m do
        begin
          read(txt,scan);
          StringGrid1.Cells[j,i]:=FLoatToStr(scan);
        end;
     end;
    for i:=1 to n do
     begin
      for j:=1 to m do
        begin
          read(txt,scan);
          StringGrid2.Cells[j,i]:=FLoatToStr(scan);
        end;
     end;
  end;
end;

procedure TForm1.N2Click(Sender: TObject);
var txt:textFile;
    i,j:integer;
begin
 if saveDialog1.Execute then
  begin
    assignFile(txt,SaveDialog1.FileName);
    rewrite(txt);
    for i:=1 to n do
      begin
        for j:=1 to m do write(txt,StringGrid1.Cells[j,i]:6);
        writeln(txt);
      end;
    writeln(txt);
    for i:=1 to n do
      begin
        for j:=1 to m do write(txt,StringGrid2.Cells[j,i]:6);
        writeln(txt);
      end;
    for i:=1 to n do
      begin
        for j:=1 to m do write(txt,StringGrid3.Cells[j,i]:6);
        writeln(txt);
      end;
    writeln(txt);
  end;
    CloseFile(txt);
  end;

end.
