unit Unit1;

{$mode objfpc}{$H+}

interface

uses
  Classes, SysUtils, FileUtil, Forms, Controls, Graphics, Dialogs, Grids,
  ExtCtrls, StdCtrls;

type

  { TForm1 }

  TForm1 = class(TForm)
    Button1: TButton;
    Button2: TButton;
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    Label4: TLabel;
    Memo1: TMemo;
    RadioGroup1: TRadioGroup;
    StringGrid1: TStringGrid;
    procedure Button1Click(Sender: TObject);
    procedure Button2Click(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure RadioGroup1Click(Sender: TObject);
  private
    { private declarations }
  public
    { public declarations }
  end;

var
  Form1: TForm1;
  Rect: TRect;
  Files: TextFile;
  i, j, n: Integer;

implementation

{$R *.lfm}

{ TForm1 }

procedure TForm1.FormCreate(Sender: TObject);
begin
  Memo1.clear;
  Memo1.Lines.Add('Окно для вывода особых элементов матрицы!');
  case RadioGroup1.ItemIndex of
    0: n := 3;
    1: n := 5;
    2: n := 6;
  end;

  StringGrid1.ColCount := n + 1;
  StringGrid1.RowCount := n + 1;
  StringGrid1.Cells[0,0] := 'Матрица';
  for i := 1 to n do
  begin
    StringGrid1.Cells[0,i] := 'i=' + intToStr(i);
    StringGrid1.Cells[i,0] := 'j=' + intToStr(i);
  end;
end;

procedure TForm1.Button2Click(Sender: TObject);
var
  lf, rh: boolean;
  q, k: integer;
begin
  Memo1.clear;
  case RadioGroup1.ItemIndex of
    0: n := 3;
    1: n := 5;
    2: n := 6;
  end;
  k := 0;
  begin
    for i := 1 to n do
      for j := 2 to n - 1 Do
        begin
          lf := true;
          rh := true;
          for q := 1 to j - 1 do
            if (StrToFloat(StringGrid1.Cells[q, i]) >= StrToFloat(StringGrid1.Cells[j, i])) then
              lf := false;
          for q := j + 1 to n do
            if (StrToFloat(StringGrid1.Cells[q, i]) <= StrToFloat(StringGrid1.Cells[j, i]))  then
              rh := false;
            if (lf = true) and (rh = true) then
              begin
                k := k + 1;
                Memo1.Lines.Add('Число: ' + StringGrid1.Cells[j, i] + '; ' + 'Позиция: ' + 'j=' + floattostr(j) + '; ' + 'i=' + floattostr(i));

              end;
        end;

    Label4.Caption := IntToStr(k);
  end;
end;

procedure TForm1.Button1Click(Sender: TObject);
var
  q, k: integer;
  summ: real;
begin
  Memo1.clear;
  case RadioGroup1.ItemIndex of
    0: n := 3;
    1: n := 5;
    2: n := 6;
  end;
  k := 0;
  for j := 1 to n do
    for i := 1 to n do
    begin
      summ := 0;
      for q := 1 to n do
        begin
          if (q <> i) then
            summ := summ + StrToFloat(StringGrid1.Cells[j, q]);
        end;
      if (StrToFloat(StringGrid1.Cells[j, i]) > summ) then
        begin
          k := k + 1;
          Memo1.Lines.Add('Число: ' + StringGrid1.Cells[j, i] + '; ' + 'Позиция: ' + 'j=' + floattostr(j) + '; ' + 'i=' + floattostr(i));
        end;
    end;
  Label3.Caption := IntToStr(k);
end;

procedure TForm1.RadioGroup1Click(Sender: TObject);
var
  temp: real;
begin
  case RadioGroup1.ItemIndex of
    0: n := 3;
    1: n := 5;
    2: n := 6;
  end;

  StringGrid1.ColCount := n + 1;
  StringGrid1.RowCount := n + 1;
  StringGrid1.Cells[0,0] := 'Матрица';
  for i := 1 to n do
  begin
    StringGrid1.Cells[0,i] := 'i=' + intToStr(i);
    StringGrid1.Cells[i,0] := 'j=' + intToStr(i);
  end;

  AssignFile(Files, 'M' + IntToStr(n) + '.txt');
  Reset(Files);
  for i := 1 to n do
  begin
    for j := 1 to n do
    begin
      Read(Files, temp);
      StringGrid1.Cells[j, i] := FloatToStrF(temp, fffixed, 2, 1);
    end;
    ReadLn(Files);
  end;
  CloseFile(Files);
end;

end.

