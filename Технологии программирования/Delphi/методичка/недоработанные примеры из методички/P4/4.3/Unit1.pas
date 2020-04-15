unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, Grids, Menus;

type
  TForm1 = class(TForm)
    Edit1: TEdit;
    Edit2: TEdit;
    StringGrid1: TStringGrid;
    Label1: TLabel;
    Label2: TLabel;
    Button1: TButton;
    Button2: TButton;
    Memo1: TMemo;
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
  n,m,i,j:integer;

implementation

{$R *.dfm}

procedure TForm1.FormCreate(Sender: TObject);
begin
 n:=3;
 m:=3;
 Edit1.Text:='3';
 Edit2.Text:='3';
 StringGrid1.ColCount:=m+1;
 StringGrid1.RowCount:=n+1;
 for i:=1 to n do
  begin
   StringGrid1.Cells[0,i]:=IntToStr(i);
   StringGrid1.Cells[i,0]:=IntToStr(i);
  end;
 Memo1.Clear;
end;

procedure TForm1.Button1Click(Sender: TObject);
begin
 n:=StrToInt(Edit1.Text);
 m:=StrToInt(Edit2.Text);
 StringGrid1.ColCount:=m+1;
 StringGrid1.RowCount:=n+1;
 for i:=1 to n do StringGrid1.Cells[0,i]:=IntToStr(i);
 for i:=1 to m do StringGrid1.Cells[i,0]:=IntToStr(i);
end;

procedure TForm1.Button2Click(Sender: TObject);
var temp:boolean;
    k,g:integer;
    b:array [1..100] of integer;
begin
 for i:=1 to n do
  for j:=1 to m do
   a[i,j]:=StrToInt(StringGrid1.Cells[j,i]);

 k:=1;
 b[1]:=a[1,1];
 for i:=1 to n do
  for j:=1 to m do
   begin
    temp:=true;
    for g:=1 to k do
     if a[i,j]=b[g] then
      begin
       temp:=false;
       break;
      end;
    if temp then
     begin
      inc(k);
      b[k]:=a[i,j];
     end;
   end;
  Memo1.Lines.Add(IntToStr(k));
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
    CloseFile(txt);
  end;
end;

end.
