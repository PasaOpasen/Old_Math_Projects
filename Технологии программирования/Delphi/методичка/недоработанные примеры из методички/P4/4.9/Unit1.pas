unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, Grids, StdCtrls, Menus;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    Edit1: TEdit;
    Label2: TLabel;
    Edit2: TEdit;
    Button1: TButton;
    StringGrid1: TStringGrid;
    StringGrid2: TStringGrid;
    Button2: TButton;
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
  A:matr;
  n,m:integer;
implementation

{$R *.dfm}

procedure TForm1.FormCreate(Sender: TObject);
 var i:integer;
begin
 Edit1.Text:='4';
 Edit2.Text:='4';
 n:=4;
 m:=4;
 StringGrid1.ColCount:=m+1;
 StringGrid1.RowCount:=n+1;
 StringGrid2.ColCount:=m+1;
 StringGrid2.RowCount:=n+1;
 for i:=1 to n do
  begin
   StringGrid1.Cells[0,i]:=IntToStr(i);
   StringGrid1.Cells[i,0]:=IntToStr(i);
   StringGrid2.Cells[i,0]:=IntToStr(i);
   StringGrid2.Cells[0,i]:=IntToStr(i);
  end;
end;

procedure TForm1.Button1Click(Sender: TObject);
var i,j:integer;
begin
  try
    n:=StrToInt(Edit1.Text);
    m:=StrToInt(Edit2.Text);
  except
    ShowMessage('Не правильный ввод!!!');
    exit;
  end;

  StringGrid1.ColCount:=m+1;
  StringGrid1.RowCount:=n+1;
  StringGrid2.ColCount:=m+1;
  StringGrid2.RowCount:=n+1;
  for i:=1 to n do
  begin
   StringGrid1.Cells[i,0]:=IntToStr(i);
   StringGrid2.Cells[i,0]:=IntToStr(i);
  end;
  for i:=1 to m do
  begin
   StringGrid1.Cells[0,i]:=IntToStr(i);
   StringGrid2.Cells[0,i]:=IntToStr(i);
  end;
end;

procedure TForm1.Button2Click(Sender: TObject);
var i,j,iMin,iMax:integer;
    min,max,temp:real;
    B:array [1..10] of real;
begin
  try
    for i:=1 to n do
      for j:=1 to m do
        a[i,j]:=StrToFloat(StringGrid1.Cells[j,i]);
  except
    ShowMessage('Не правильный ввод!!!');
    exit;
  end;

  min:=a[1,1];
  max:=min;
  for i:=1 to n do
    for j:=1 to m do
      begin
        if min>a[i,j] then
          begin
            min:=a[i,j];
            iMin:=i;
          end;
        if max<a[i,j] then
          begin
            max:=a[i,j];
            iMax:=i;
          end;
      end;


  for j:=1 to n do
    begin
      temp:=a[iMax,j];
      a[iMax,j]:=a[iMin,j];
      a[iMin,j]:=temp;
    end;

  for i:=1 to n do
    for j:=1 to m do
      StringGrid2.Cells[j,i]:=FloatToStrF(A[i,j],fffixed,6,0);
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
    StringGrid1.ColCount:=m+1;
    StringGrid1.RowCount:=n+1;
    StringGrid2.ColCount:=m+1;
    StringGrid2.RowCount:=n+1;
    for i:=1 to n do
     begin
      for j:=1 to m do
        begin
          read(txt,scan);
          StringGrid1.Cells[j,i]:=FLoatToStr(scan);
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
    CloseFile(txt);
  end;
end;
end.
