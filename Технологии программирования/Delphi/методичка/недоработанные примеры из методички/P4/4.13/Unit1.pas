unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, Menus, StdCtrls, Grids, ExtCtrls;

type
  TForm1 = class(TForm)
    StringGrid1: TStringGrid;
    StringGrid2: TStringGrid;
    Edit1: TEdit;
    Edit2: TEdit;
    Label1: TLabel;
    Label2: TLabel;
    Button1: TButton;
    Button2: TButton;
    MainMenu1: TMainMenu;
    N1: TMenuItem;
    N2: TMenuItem;
    OpenDialog1: TOpenDialog;
    SaveDialog1: TSaveDialog;
    Edit3: TEdit;
    Edit4: TEdit;
    RadioGroup1: TRadioGroup;
    Label3: TLabel;
    Label4: TLabel;
    procedure N1Click(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure Button2Click(Sender: TObject);
    procedure N2Click(Sender: TObject);
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
      read(txt,m);
      StringGrid1.RowCount:=n+1;
      StringGrid1.ColCount:=m+1;
      StringGrid2.RowCount:=n+1;
      StringGrid2.ColCount:=m+1;
      for i:=1 to n do
        for j:=1 to m do
          begin
            read(txt,scan);
            StringGrid1.Cells[j,i]:=FloatToSTr(scan);
          end;




    end;

end;

procedure TForm1.FormCreate(Sender: TObject);
begin
  n:=3;
  m:=3;
  EDit1.Text:='3';
  Edit2.Text:='3';
  EDit3.Text:='1';
  Edit4.Text:='3';
  StringGrid1.RowCount:=n+1;
  StringGrid1.ColCount:=m+1;
  StringGrid2.RowCount:=n+1;
  StringGrid2.ColCount:=m+1;
  for i:=1 to 10 do
    begin
      StringGrid1.Cells[0,i]:=intToStr(i);
      StringGrid1.Cells[i,0]:=intToStr(i);
      StringGrid2.Cells[0,i]:=intToStr(i);
      StringGrid2.Cells[i,0]:=intToStr(i);
    end;
end;

procedure TForm1.Button2Click(Sender: TObject);
var temp,max,min:real;
    k,l:integer;
begin
  for i:=1 to n do
    for j:=1 to m do
      a[i,j]:=StrToFloat(StringGrid1.Cells[j,i]);
  if RadioGroup1.ItemIndex=0 then
    begin
      l:=StrToInt(Edit3.Text);
      k:=StrToInt(Edit4.Text);
      for i:=1 to m do
        begin
          temp:=a[k,i];
          a[k,i]:=a[l,i];
          a[l,i]:=temp;
        end;
    end;

  if RadioGroup1.ItemIndex=1 then
    begin
      l:=StrToInt(Edit3.Text);
      k:=StrToInt(Edit4.Text);
      for i:=1 to m do
        begin
          temp:=a[i,k];
          a[i,k]:=a[i,l];
          a[i,l]:=temp;
        end;
    end;

  if RadioGroup1.ItemIndex=2 then
    begin
      l:=1;
      k:=1;
      max:=a[1,1];
      min:=max;
      for i:=1 to n do
        for j:=1 to m do
          begin
            if max<a[i,j]then
              begin
                MAX:=A[I,J];
                l:=i;
              end;
            if min>a[i,j]then
              begin
                min:=A[I,J];
                k:=j;
              end;
          end;


      for i:=1 to m do
        begin
          temp:=a[k,i];
          a[k,i]:=a[l,i];
          a[l,i]:=temp;
        end;
    end;

  if RadioGroup1.ItemIndex=3 then
    begin
      l:=1;
      k:=1;
      max:=a[1,1];
      min:=max;
      for i:=1 to n do
        for j:=1 to m do
          begin
            if max<a[i,j]then
              begin
                MAX:=A[I,J];
                l:=i;
              end;
            if min>a[i,j]then
              begin
                min:=A[I,J];
                k:=j;
              end;
          end;
      for i:=1 to m do
        begin
          temp:=a[i,k];
          a[i,k]:=a[i,l];
          a[i,l]:=temp;
        end;
    end;


  if RadioGroup1.ItemIndex=4 then
    begin
      for i:=1 to n do
        for j:=1 to m do
          stringGrid2.Cells[j,i]:=stringGrid1.Cells[i,j];
    end
    else
    for i:=1 to n do
      for j:=1 to m do
        StringGrid2.Cells[j,i]:=floatToStr(a[i,j]);
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
