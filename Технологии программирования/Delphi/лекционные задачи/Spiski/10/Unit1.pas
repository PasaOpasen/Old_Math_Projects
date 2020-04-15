unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, Grids;

type
  TForm1 = class(TForm)
    StringGrid1: TStringGrid;
    StringGrid2: TStringGrid;
    Button1: TButton;
    Label1: TLabel;
    Label2: TLabel;
    procedure FormCreate(Sender: TObject);
    procedure Button1Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

type Arr=array [1..25] of integer;
var
  Form1: TForm1;

implementation

{$R *.dfm}

procedure TForm1.FormCreate(Sender: TObject);
var i:integer;
begin
  for i:=1 to 5 do
    begin
      StringGrid1.Cells[0,i]:=IntToStr(i);
      StringGrid1.Cells[i,0]:=IntToStr(i);
      StringGrid2.Cells[0,i]:=IntToStr(i);
      StringGrid2.Cells[i,0]:=IntToStr(i);
    end;
end;

procedure sort(var ar: Arr; m, l: Integer);
var 
    i,j:Integer;
    x,tmp:Integer;
begin 
    i:=m;
    j:=l;
    x:=ar[(m+l) div 2];
    repeat
        while ar[i] < x Do Inc(i);
        while ar[j] > x Do Dec(j); 
        if i <= j then begin 
            tmp:=ar[i];
            ar[i]:=ar[j];
            ar[j]:=tmp;
            Inc(i);
            Dec(j) 
        end 
    until i > j;
    if m < j then sort(ar, m, j);
    if i < l then sort(ar, i, l)
end;

procedure TForm1.Button1Click(Sender: TObject);
var SortMas,Mas:Arr;
    i,j,k:integer;
begin
  randomize;
  for i:=1 to 25 do
    begin
      Mas[i]:=random(100);
      SortMas[i]:=Mas[i];
    end;

  sort(SortMas,1,25);

  j:=1;
  i:=1;
  for k:=1 to 25 do
    begin
      StringGrid1.Cells[j,i]:=IntToStr(Mas[k]);
      StringGrid2.Cells[j,i]:=IntToStr(SortMas[k]);
      if k mod 5 =0 then
        begin
          i:=i+1;
          j:=0;
        end;
      inc(j);

    end;

end;

end.
