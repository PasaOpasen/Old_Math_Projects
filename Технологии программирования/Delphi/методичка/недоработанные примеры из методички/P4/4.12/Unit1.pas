unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, Menus, StdCtrls, Grids, ExtCtrls;

type
  TForm1 = class(TForm)
    StringGrid1: TStringGrid;
    Edit1: TEdit;
    Label1: TLabel;
    Button1: TButton;
    Button2: TButton;
    MainMenu1: TMainMenu;
    N1: TMenuItem;
    N2: TMenuItem;
    OpenDialog1: TOpenDialog;
    SaveDialog1: TSaveDialog;
    RadioGroup1: TRadioGroup;
    Label3: TLabel;
    Label4: TLabel;
    procedure N1Click(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure Button2Click(Sender: TObject);
    procedure N2Click(Sender: TObject);
    procedure Button1Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;
type matr=array [1..100,1..100] of real;
var
  Form1: TForm1;
  a,b:matr;
  n,m,i,j:integer;

implementation

{$R *.dfm}

procedure TForm1.N1Click(Sender: TObject);
var txt:TextFile;
    scan:real;
begin
  if OPenDialog1.Execute then
    begin
      assignFile(txt,openDialog1.FileName);
      reset(txt);
      read(txt,n);
      StringGrid1.RowCount:=n+1;
      StringGrid1.ColCount:=n+1;
      for i:=1 to n do
        for j:=1 to n do
          begin
            read(txt,scan);
            StringGrid1.Cells[j,i]:=FloatToSTr(scan);
          end;




    end;

end;

procedure TForm1.FormCreate(Sender: TObject);
begin
  n:=3;

  EDit1.Text:='3';
  StringGrid1.RowCount:=n+1;
  StringGrid1.ColCount:=n+1;
  for i:=1 to 10 do
    begin
      StringGrid1.Cells[0,i]:=intToStr(i);
      StringGrid1.Cells[i,0]:=intToStr(i);

    end;
end;

procedure TForm1.Button2Click(Sender: TObject);
var flag:boolean;
    sum:real;
begin
  if RadioGroup1.ItemIndex=0 then
    begin
      flag:=false;
      for i:=1 to n do
        for j:=1 to n do
          if i>j then
            if StringGrid1.Cells[j,i]<>'0' then flag:=true;
      if flag then
        Label4.Caption:='Неявляеться правой треугольной'
      else Label4.Caption:='Являеться правой треугольной';
    end;

  if RadioGroup1.ItemIndex=1 then
    begin
      flag:=false;
      for i:=1 to n do
        for j:=1 to n do
          if i<j then
            if StringGrid1.Cells[j,i]<>'0' then flag:=true;
      if flag then
        Label4.Caption:='Неявляеться левой треугольной'
      else Label4.Caption:='Являеться левой треугольной';
    end;

  if RadioGroup1.ItemIndex=2 then
    begin
      flag:=false;
      for i:=1 to n do
        for j:=1 to n do
          if i<>j then
            if StringGrid1.Cells[j,i]<>StringGrid1.Cells[i,j] then flag:=true;
      if flag then
        Label4.Caption:='Неявляеться семетричной'
      else Label4.Caption:='Являеться семетричной';
    end;

  if RadioGroup1.ItemIndex=3 then
    begin
      flag:=false;
      for i:=1 to n do
        for j:=1 to n do
          if i+j<>n+1 then
            if StringGrid1.Cells[j,i]<>StringGrid1.Cells[i,j] then flag:=true;
      if flag then
        Label4.Caption:='Неявляеться семетричной'
      else Label4.Caption:='Являеться семетричной';
    end;

  if RadioGroup1.ItemIndex=4 then
    begin
      flag:=false;
      for i:=1 to n do
        for j:=1 to n do
          a[i,j]:=strToFloat(StringGrid1.Cells[j,i]);
      for i:=1 to n do
        begin
          sum:=0;
          for j:=1 to n do
            if a[i,j]<0 then flag:=true
            else sum:=sum+a[i,j];
          if sum<>1 then flag:=true;
        end;
      if flag then
        Label4.Caption:='Неявляеться стохастической'
      else Label4.Caption:='Являеться стохостической';
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
    writeln(txt,Label4.caption);
    writeln(txt);
    CloseFile(txt);
  end;
end;

procedure TForm1.Button1Click(Sender: TObject);
begin
  n:=strtoint(edit1.Text);
  StringGrid1.RowCount:=n+1;
  StringGrid1.ColCount:=n+1;

end;

end.
