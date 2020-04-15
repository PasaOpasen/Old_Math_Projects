unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls, Grids, Menus;

type
  TForm1 = class(TForm)
    StringGrid1: TStringGrid;
    Edit1: TEdit;
    Button1: TButton;
    Label1: TLabel;
    StringGrid2: TStringGrid;
    RadioGroup1: TRadioGroup;
    Button2: TButton;
    Edit2: TEdit;
    Edit3: TEdit;
    Label2: TLabel;
    Label3: TLabel;
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

type Vect=array [1..100] of integer;
var
  Form1: TForm1;
  n,m:integer;
  A:Vect;
implementation

{$R *.dfm}

procedure TForm1.FormCreate(Sender: TObject);
var i,j:integer;
begin
Edit1.Text:='4';
Edit2.Text:='2';
Edit3.Text:='3';
n:=3;
StringGrid1.Cells[0,0]:='A';
StringGrid2.Cells[0,0]:='B';
end;

procedure TForm1.Button1Click(Sender: TObject);
begin
 n:=StrToInt(Edit1.Text);
 StringGrid1.RowCount:=n+1;
 StringGrid2.RowCount:=n+1;
end;

procedure TForm1.Button2Click(Sender: TObject);
var i,j,k,i1,j1:integer;

begin
  for i:=1 to n do a[i]:=StrToInt(StringGrid1.Cells[0,i]);
  i:=StrToInt(edit2.Text);
  j:=StrToInt(edit3.Text);
  if i>j then
    begin
      ShowMessage('i>j условие неверно');
      exit;
    end;
  if RadioGroup1.ItemIndex=0 then
    begin
      while (i<j) do
        begin
          k:=a[i];
          a[i]:=a[i+1];
          a[i+1]:=k;
          inc(i);
        end;
    end;
  if RadioGroup1.ItemIndex=1 then
    begin
      while (j>i) do
        begin
          k:=a[j];
          a[j]:=a[j-1];
          a[j-1]:=k;
          dec(j);
        end;
    end;
  for i:=1 to n do StringGrid2.Cells[0,i]:=IntToStr(a[i]);
end;

procedure TForm1.N1Click(Sender: TObject);
var txt:TextFile;
    i,scan:integer;
    s:string;
begin
 if OpenDialog1.Execute then
  begin
    assignFile(txt,OpenDialog1.FileName);
    reset(txt);
    read(txt,n);
    StringGrid1.RowCount:=n+1;
    StringGrid2.RowCount:=n+1;
    for i:=1 to n do
     begin
        read(txt,scan);
        StringGrid1.Cells[0,i]:=IntToStr(scan);
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
    
    CloseFile(txt);
  end;
end;

end.
