unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls, Grids, Menus;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    Edit1: TEdit;
    Label2: TLabel;
    Edit2: TEdit;
    Button1: TButton;
    Button2: TButton;
    StringGrid1: TStringGrid;
    Memo1: TMemo;
    RadioGroup1: TRadioGroup;
    RadioButton1: TRadioButton;
    RadioButton2: TRadioButton;
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
type
 matr=array [1..10,1..10] of real;
var
  Form1: TForm1;
  A:matr;
  n,m:integer;
implementation

{$R *.dfm}

procedure TForm1.FormCreate(Sender: TObject);
 var i:integer;
begin
 Edit1.Text:='3';
 Edit2.Text:='3';
 StringGrid1.Cells[0,0]:='A';
 for i:=1 to 10 do
  begin
   StringGrid1.Cells[i,0]:=IntToStr(i);
   StringGrid1.Cells[0,i]:=IntToStr(i);
  end;
 RadioButton1.Checked:=true;
 Memo1.Clear;
end;

procedure TForm1.Button1Click(Sender: TObject);
 var i:integer;
begin

 n:=StrToInt(Edit1.Text);
 m:=StrToInt(Edit2.Text);
 StringGrid1.RowCount:=n+1;
 StringGrid1.ColCount:=m+1;
 for i:=1 to n do StringGrid1.Cells[0,i]:=IntToStr(i);
 for i:=1 to m do StringGrid1.Cells[i,0]:=IntToStr(i);
end;

procedure TForm1.Button2Click(Sender: TObject);
var i,j:integer;
    sum,k:real;
begin
 for i:=1 to n do
  for j:=1 to m do
   A[i,j]:=StrToFloat(StringGrid1.Cells[j,i]);
 Memo1.Clear;
 if RadioButton1.Checked then
  begin
  for i:=1 to n do
   begin
    sum:=0;
    for j:=1 to m do sum:=a[j,i]+sum;
    for j:=1 to m do
     if a[j,i]>(sum-a[j,i]) then
      begin
       k:=a[j,i];
       Memo1.Lines.Add(FloatToStrF(k,fffixed,6,2));
      end;
   end;
  end;
 if RadioButton2.Checked then
  begin
   for i:=1 to n do
    for j:=2 to m-1 do
      if (A[i,j]>A[i,j-1]) and (A[i,j]<A[i,j+1]) then
        Memo1.Lines.Add(FloatToStrF(A[i,j],fffixed,6,2));

  end;
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
    StringGrid1.RowCount:=n+1;
    StringGrid1.ColCount:=m+1;
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
