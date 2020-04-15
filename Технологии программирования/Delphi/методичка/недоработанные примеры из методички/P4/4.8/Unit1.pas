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
    Memo1: TMemo;
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
type matr=array [1..100,1..100] of real;
var
  Form1: TForm1;
  a:matr;
  n,i,j:integer;
implementation

{$R *.dfm}

procedure TForm1.FormCreate(Sender: TObject);

begin
 n:=3;
 Edit1.Text:='3';
 StringGrid1.ColCount:=n+1;
 StringGrid1.RowCount:=n+1;
 for i:=1 to n do
  begin
    StringGrid1.Cells[i,0]:=IntToStr(i);
    StringGrid1.Cells[0,i]:=IntToStr(i);
  end;
end;

procedure TForm1.Button1Click(Sender: TObject);
begin
 n:=StrToInt(Edit1.Text);
 StringGrid1.ColCount:=n+1;
 StringGrid1.RowCount:=n+1;
 for i:=1 to n do
  begin
    StringGrid1.Cells[i,0]:=IntToStr(i);
    StringGrid1.Cells[0,i]:=IntToStr(i);
  end;
end;

procedure TForm1.Button2Click(Sender: TObject);
var min,max:real;
begin
 for i:=1 to n do
  for j:=1 to n do
   a[i,j]:=StrToFloat(StringGrid1.Cells[j,i]);
 max:=a[2,n];
 min:=a[1,2];
 for i:=1 to n do
  for j:=1 to n do
   begin
    if i-j<0 then
     if a[i,j]<min then min:=a[i,j];
    if i+j>1 then
     if a[i,j]>max then max:=a[i,j];
   end;
 memo1.Lines.Add(FloatToStrF(min,fffixed,6,2)+' '+FloatToStrF(max,fffixed,6,2));
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
    StringGrid1.ColCount:=n+1;
    StringGrid1.RowCount:=n+1;
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
    CloseFile(txt);
  end;
end;

end.
