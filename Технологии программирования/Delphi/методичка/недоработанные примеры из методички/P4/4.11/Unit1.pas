unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls, Grids, Xpman, Menus;

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
    RadioButton4: TRadioButton;
    RadioButton5: TRadioButton;
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
 try n:=StrToInt(Edit1.Text);
  except
   showMessage('Неправильный ввод N');
   exit;
 end;
 stringgrid1.Height:=(n+1)*24+11;
 stringgrid2.Height:=(n+1)*24+11;
 stringgrid3.Height:=(n+1)*24+11;
 StringGrid1.RowCount:=n+1;
 StringGrid2.RowCount:=n+1;
 StringGrid3.RowCount:=n+1;
end;

procedure TForm1.Button2Click(Sender: TObject);
var i,j:integer;
begin
 stringgrid3.Height:=(n+1)*24+10;
 StringGrid3.RowCount:=n+1;
 for i:=1 to n do
  begin
   try
    a[i]:=StrToInt(StringGrid1.Cells[0,i]);
    b[i]:=StrToInt(StringGrid2.Cells[0,i]);
   except
    showMessage('Неправильно введены массивы');
    exit;
   end;
  end;
 if RadioButton1.Checked then
   for i:=1 to n do
     StringGrid3.Cells[0,i]:=IntToStr(a[i]*b[i]);

 if RadioButton4.Checked then
  begin
   stringgrid3.Height:=(2*n+2)*24;
   StringGrid3.RowCount:=n*2+1;
   for i:=1 to n do
    StringGrid3.Cells[0,i]:=IntToStr(a[i]);
   for i:=n+1 to 2*n do
    StringGrid3.Cells[0,i]:=IntToStr(b[i-n]);
   end;

 if RadioButton5.Checked then
  begin
   stringgrid3.Height:=(2*n+2)*24;
   StringGrid3.RowCount:=n*2+1;
   for i:=1 to 2*n do
    begin
     StringGrid3.Cells[0,2*i]:=IntToStr(b[i]);
     StringGrid3.Cells[0,2*i-1]:=IntToStr(a[i]);
    end;
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
    StringGrid1.RowCount:=n+1;
    StringGrid2.RowCount:=n+1;
    StringGrid3.RowCount:=n+1;
    for i:=1 to n do
     begin
        read(txt,scan);
        StringGrid1.Cells[j,i]:=FLoatToStr(scan);
        read(txt,scan);
        StringGrid2.Cells[j,i]:=FLoatToStr(scan);
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
    for i:=1 to n do write(txt,StringGrid1.Cells[0,i]:6);
        writeln(txt);
    for i:=1 to n do write(txt,StringGrid2.Cells[0,i]:6);
        writeln(txt);
    for i:=1 to n do write(txt,StringGrid3.Cells[0,i]:6);
        writeln(txt);
    CloseFile(txt);
  end;
end;

end.
