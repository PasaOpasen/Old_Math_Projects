unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, XPMan, Buttons, StdCtrls, Grids, ComCtrls;

type
  TForm1 = class(TForm)
    StringGrid1: TStringGrid;
    lbl1: TLabel;
    StringGrid2: TStringGrid;
    lbl2: TLabel;
    btn1: TButton;
    btn2: TButton;
    lbl3: TLabel;
    lbl4: TLabel;
    lbl5: TLabel;
    lbl6: TLabel;
    btn3: TBitBtn;
    XPManifest1: TXPManifest;
    btn4: TButton;
    procedure btn1Click(Sender: TObject);
    procedure btn4Click(Sender: TObject);
    procedure btn2Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;
type
  FOI = file of Integer;
var
  Form1: TForm1;

implementation

{$R *.dfm}

procedure TForm1.btn1Click(Sender: TObject);
type
  MasOfInt = array [1..100] of Integer;
var
  M: MasOfInt;
  i, j, min, ind: Integer;
  F: FOI;
  Tm: TDateTime;
begin
  if StringGrid1.Cells[0,0]='' then
  begin
    ShowMessage('Вы не сгенерировали файл, который собираетесь сортировать');
    Exit;
  end;
  Tm:= Time;
  AssignFile(F,'FileOfInteger.dat');
  Reset(F);
  for i:= 1 to 100 do
    read(F,M[i]);
  for i:= 1 to 99 do
  begin
    min:= M[i];
    ind:= i;
    for j:= i+1 to 100 do
      if min>M[j] then
      begin
        min:= M[j];
        ind:= j;
      end;
    if min<M[i] then
    begin
      M[ind]:= M[i];
      M[i]:= min;
    end;
    StringGrid2.Cells[(i-1) mod 10, (i-1) div 10]:= IntToStr(min);
  end;
  StringGrid2.Cells[9,9]:= IntToStr(M[100]);
  lbl5.Caption:= TimeToStr((Time - Tm)*1000);
  CloseFile(F);
end;

procedure TForm1.btn4Click(Sender: TObject);
var
  F: FOI;
  i, j, k: Integer;
begin
  AssignFile(F,'FileOfInteger.dat');
  Rewrite(F);
  for i:= 0 to 9 do
    for j:= 0 to 9 do
    begin
      k:= 1 + Random(100);
      StringGrid1.Cells[j,i]:= IntToStr(k);
      write(F,k);
    end;
  CloseFile(F);
end;

procedure TForm1.btn2Click(Sender: TObject);
var
  F: FOI;
  i, j, ind, min, el, qst: Integer;
  Tm: TDateTime;
begin
  if StringGrid1.Cells[0,0]='' then
  begin
    ShowMessage('Вы не сгенерировали файл, который собираетесь сортировать');
    Exit;
  end;
  Tm:= Time;
  AssignFile(F,'FileOfInteger.dat');
  Reset(F);
  for i:= 0 to 98 do
  begin
    ind:= i;
    read(F,min);
    qst:= min;
    for j:= i+1 to 99 do
    begin
      read(F,el);
      if min>el then
      begin
        ind:= j;
        min:= el;
      end;
    end;
    if min<qst then
    begin
      Seek(F,ind);
      write(F,qst);
      Seek(F,i);
      write(F,min);
    end
    else Seek(F,i+1);
    StringGrid2.Cells[ i mod 10, i div 10]:= IntToStr(min);
  end;
  StringGrid2.Cells[9,9]:= IntToStr(el);
  lbl6.Caption:= TimeToStr((Time - Tm)*1000);
  CloseFile(F);
end;

end.
