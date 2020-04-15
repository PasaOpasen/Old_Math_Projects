unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, Menus, StdCtrls, Grids, ExtCtrls;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    Label2: TLabel;
    Edit1: TEdit;
    Edit2: TEdit;
    StringGrid1: TStringGrid;
    Button1: TButton;
    Button2: TButton;
    MainMenu1: TMainMenu;
    N1: TMenuItem;
    N2: TMenuItem;
    OpenDialog1: TOpenDialog;
    SaveDialog1: TSaveDialog;
    StringGrid2: TStringGrid;
    RadioGroup1: TRadioGroup;
    procedure Button1Click(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure Button2Click(Sender: TObject);
    procedure N1Click(Sender: TObject);
    procedure N2Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;
type Matr=array [1..100,1..100] of real;

var
  Form1: TForm1;
  n,m:integer;
  A,B:Matr;


implementation

{$R *.dfm}

procedure TForm1.Button1Click(Sender: TObject);
var i,j:integer;
begin
 n:=StrToInt(Edit1.Text);
 m:=StrToInt(Edit2.Text);
 StringGrid1.RowCount:=n+1;
 StringGrid2.RowCount:=n+1;
 StringGrid1.ColCount:=m+1;
 StringGrid2.ColCount:=m+1;
 for i:=1 to n do
  begin
    StringGrid1.Cells[0,i]:=IntToStr(i);
    StringGrid2.Cells[0,i]:=IntToStr(i);
  end;
 for i:=1 to m do
  begin
    StringGrid1.Cells[i,0]:=IntToStr(i);
    StringGrid2.Cells[i,0]:=IntToStr(i);
  end;

end;


procedure TForm1.FormCreate(Sender: TObject);
var i,j:integer;
begin
Edit1.Text:='3';
Edit2.Text:='3';
m:=3;
n:=3;
StringGrid1.Cells[0,0]:='A';
StringGrid2.Cells[0,0]:='B';
for i:=1 to StrToInt(edit1.Text) do
 begin
   StringGrid1.Cells[0,i]:=IntToStr(i);
   StringGrid1.Cells[i,0]:=IntToStr(i);
   StringGrid2.Cells[0,i]:=IntToStr(i);
   StringGrid2.Cells[i,0]:=IntToStr(i);
 end;

end;

procedure TForm1.Button2Click(Sender: TObject);
var i,j,k,c:integer;
    sum1,sum2,max1,max2,min1,min2:real;
    arr:array [1..100] of real;
begin
  for i:=1 to n do
    for j:=1 to m do
      begin
        a[i,j]:=StrToFloat(StringGrid1.Cells[j,i]);
        b[i,j]:=a[i,j];
      end;
  if radioGroup1.ItemIndex=0 then
    begin
      for i:=1 to n-1 do
        for j:=i+1 to n do
            if a[i,1]>a[j,1] then
              for k:=1 to m do
                begin
                  arr[k]:=a[i,k];
                  a[i,k]:=a[j,k];
                  a[j,k]:=arr[k];
                end;
    for i:=1 to n do
      for j:=1 to m do
        STringGrid2.Cells[j,i]:=FloatToStr(a[i,j]);
    end;

  if radioGroup1.ItemIndex=1 then
    begin
      for i:=1 to n do
        begin
          sum1:=0;
          for j:=1 to m do sum1:=sum1+a[i,j];
          for j:=1 to n do
            begin
              sum2:=0;
              for c:=1 to m do sum2:=sum2+a[j,c];
              if sum1<sum2 then
                for k:=1 to m do
                  begin
                    arr[k]:=a[i,k];
                    a[i,k]:=a[j,k];
                    a[j,k]:=arr[k];
                  end;
            end;
        end;

      for i:=1 to n do
        for j:=1 to m do STringGrid2.Cells[j,i]:=FloatToStr(a[i,j]);
    end;

  if radioGroup1.ItemIndex=2 then
    begin

      for i:=1 to n do
        begin
          max1:=a[i,1];
          for j:=1 to m do
            if max1<a[i,j] then max1:=a[i,j];
          for j:=1 to n do
            begin
              max2:=a[j,1];
              for c:=1 to m do
                if max2<a[j,c] then max2:=a[j,c];
              if max1<max2 then
                for k:=1 to m do
                  begin
                    arr[k]:=a[i,k];
                    a[i,k]:=a[j,k];
                    a[j,k]:=arr[k];
                  end;
            end;
        end;
      for i:=1 to n do
        for j:=1 to m do
          STringGrid2.Cells[j,i]:=FloatToStr(a[i,j]);
    end;

  if radioGroup1.ItemIndex=3 then
    begin

      for i:=1 to n do
        begin
          min1:=a[i,1];
          for j:=1 to m do
            if min1>a[i,j] then min2:=a[i,j];
          for j:=1 to n do
            begin
              min2:=a[j,1];
              for c:=1 to m do
                if min2>a[j,c] then min2:=a[j,c];
              if min1>min2 then
                for k:=1 to m do
                  begin
                    arr[k]:=a[i,k];
                    a[i,k]:=a[j,k];
                    a[j,k]:=arr[k];
                  end;
            end;
        end;
      for i:=1 to n do
        for j:=1 to m do
          STringGrid2.Cells[j,i]:=FloatToStr(a[i,j]);
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
    StringGrid2.RowCount:=n+1;
    StringGrid1.ColCount:=m+1;
    StringGrid2.ColCount:=m+1;
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
