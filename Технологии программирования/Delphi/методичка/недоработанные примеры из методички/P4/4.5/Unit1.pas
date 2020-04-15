unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, Grids, Menus;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    Edit1: TEdit;
    StringGrid1: TStringGrid;
    Label2: TLabel;
    Button1: TButton;
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
type matr=array [1..100,1..100] of integer;
var
  Form1: TForm1;
  a:matr;
  n,i,j,m:integer;

implementation

{$R *.dfm}

procedure TForm1.FormCreate(Sender: TObject);
begin
 edit1.Text:='3';
 n:=3;
 for i:=1 to n do
  begin
   stringGrid1.Cells[0,i]:=inttostr(i);
   stringGrid1.Cells[i,0]:=inttostr(i);
  end;
end;

procedure TForm1.Button1Click(Sender: TObject);
begin
 n:=StrToInt(edit1.text);
 stringGrid1.ColCount:=n+1;
 stringGrid1.RowCount:=n+1;
 for i:=1 to n do
  begin
   stringGrid1.Cells[0,i]:=inttostr(i);
   stringGrid1.Cells[i,0]:=inttostr(i);
  end;
end;

procedure TForm1.Button2Click(Sender: TObject);
var temp:boolean;
begin
 for i:=1 to n do
  for j:=1 to n do
   a[i,j]:=strtoint(StringGrid1.Cells[j,i]);
 temp:=true;
 for i:=1 to n-1 do
  begin
   for j:=1 to n-1 do
     if (a[i,j]<>a[n-i+1,n-j+1]) then
      begin
      temp:=false;
      break;
     end;
   if temp=false then break;
  end;
  if temp then Label2.Caption:='Матрица симметрична'
  else Label2.Caption:='Матрица не симметрична';
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
    stringGrid1.ColCount:=n+1;
    stringGrid1.RowCount:=n+1;
    for i:=1 to n do
     begin
      for j:=1 to n do
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
        for j:=1 to n do write(txt,StringGrid1.Cells[j,i]:6);
        writeln(txt);
      end;
    writeln(txt,Label2.Caption);
    CloseFile(txt);
  end;
end;

end.
