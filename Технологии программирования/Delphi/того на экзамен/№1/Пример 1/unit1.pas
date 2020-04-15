unit Unit1;

{$mode objfpc}{$H+}

interface

uses
  Classes, SysUtils, FileUtil, Forms, Controls, Graphics, Dialogs, StdCtrls,
  ExtCtrls, Grids;

type

  { TForm1 }

  TForm1 = class(TForm)
    Button1: TButton;
    SaveRazm: TButton;
    Clear: TButton;
    Edit1: TEdit;
    Edit2: TEdit;
    FilesSelect: TGroupBox;
    Label1: TLabel;
    Label2: TLabel;
    SelectDo: TRadioGroup;
    ReadSelect: TGroupBox;
    StringGrid2: TStringGrid;
    StringGrid3: TStringGrid;
    Vector: TRadioGroup;
    Matrix: TRadioGroup;
    StringGrid1: TStringGrid;
   { procedure Button1Click(Sender: TObject);
    procedure ClearClick(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure MatrixClick(Sender: TObject);
    procedure SaveRazmClick(Sender: TObject);
    procedure SelectDoClick(Sender: TObject);
    procedure VectorClick(Sender: TObject);  }
  private
    { private declarations }
  public
    { public declarations }
  end;

var
  Form1: TForm1;

implementation

{$R *.lfm}

{ TForm1 }
{procedure TForm1.FormCreate(Sender: TObject);
var
  i, j, n, m, r, s: Integer;
  temp: Real;
  Files: TextFile;
begin
  s := SelectDo.ItemIndex;
  case s of
    0: begin
      FilesSelect.Visible := True;
      ReadSelect.Visible := False;
      StringGrid1.Options:=[goFixedVertLine,goFixedHorzLine,goVertLine,goHorzLine,goRangeSelect,goSmoothScroll];
      StringGrid2.Options:=[goFixedVertLine,goFixedHorzLine,goVertLine,goHorzLine,goRangeSelect,goSmoothScroll];
      n := Matrix.ItemIndex;
      case n of
        0: r := 3;
        1: r := 5;
        2: r := 8;
      end;
      StringGrid1.ColCount := r + 1;
      StringGrid1.RowCount := r + 1;
      StringGrid2.RowCount := r + 1;
      StringGrid3.RowCount := r + 1;
      StringGrid1.Cells[0,0] := 'Матрица';
      StringGrid2.Cells[0,0] := 'Вектор';
      StringGrid3.Cells[0,0] := 'Результат';
      for i := 1 to r do
      begin
        StringGrid1.Cells[0,i] := 'i = ' + IntToStr(i);
        StringGrid1.Cells[i,0] := 'j = ' + IntToStr(i);
        StringGrid3.Cells[0,i] := '';
      end;
      AssignFile(Files, 'M' + IntToStr(r) + '.txt');
      Reset(Files);
      for i := 1 to r do
      begin
        for j := 1 to r do
        begin
          Read(Files, temp);
          StringGrid1.Cells[j,i] := FloatToStrF(temp,fffixed,2,1);
        end;
        ReadLn(Files);
      end;
      CloseFile(Files);
      AssignFile(Files, 'V' + IntToStr(r) + '.txt');
      Reset(Files);
      for i := 1 to r do
      begin
        Read(Files, temp);
        StringGrid2.Cells[0,i] := FloatToStrF(temp,fffixed,2,1);
      end;
      CloseFile(Files);
    end;
    1: begin
      FilesSelect.Visible := False;
      ReadSelect.Visible := True;
      StringGrid1.Options:=[goEditing,goFixedVertLine,goFixedHorzLine,goVertLine,goHorzLine,goRangeSelect,goSmoothScroll];
      StringGrid2.Options:=[goEditing,goFixedVertLine,goFixedHorzLine,goVertLine,goHorzLine,goRangeSelect,goSmoothScroll];
      try n := StrToInt(Edit1.Text)
        except ShowMessage('Некорректно введено число строк!');
        exit;
      end;
      try m := StrToInt(Edit2.Text)
        except ShowMessage('Некорректно введено число столбцов!');
        exit;
      end;
      StringGrid1.ColCount := m + 1;
      StringGrid1.RowCount := n + 1;
      StringGrid2.RowCount := m + 1;
      StringGrid3.RowCount := n + 1;
      StringGrid1.Cells[0,0] := 'Матрица';
      StringGrid2.Cells[0,0] := 'Вектор';
      StringGrid3.Cells[0,0] := 'Результат';
      for i := 1 to n do
        for j := 1 to m do
        begin
          StringGrid1.Cells[0,i] := 'i = ' + IntToStr(i);
          StringGrid1.Cells[j,0] := 'j = ' + IntToStr(j);
          StringGrid1.Cells[j,i] := '';
          StringGrid2.Cells[0,j] := '';
          StringGrid3.Cells[0,i] := '';;
        end;
    end;
  end;
end;

procedure TForm1.Button1Click(Sender: TObject);
var
  i, j, m, n: Integer;
  temp: Real;
begin
  n := StringGrid1.RowCount;
  m := StringGrid1.ColCount;
  for i := 1 to n - 1 do
    for j := 1 to m - 1 do
      try StrToFloat(StringGrid1.Cells[j,i])
        except ShowMessage('Некорректно введены данные в матрице!');
        exit;
      end;
  for j := 1 to m - 1 do
    try StrToFloat(StringGrid2.Cells[0,j])
      except ShowMessage('Некорректно введены данные в векторе!');
      exit;
    end;
  for i := 1 to n - 1 do
  begin
    temp := 0;
    for j := 1 to m - 1 do
    begin
      temp := temp + StrToFloat(StringGrid1.Cells[j,i]) * StrToFloat(StringGrid2.Cells[0,j]);
    end;
    StringGrid3.Cells[0,i] := FloatToStrF(temp,fffixed,2,1);
  end;
end;

procedure TForm1.ClearClick(Sender: TObject);
var
  i, j, n, m: Integer;
begin
  n := StringGrid1.RowCount;
  m := StringGrid1.ColCount;
  StringGrid1.ColCount := m;
  StringGrid1.RowCount := n;
  StringGrid2.RowCount := m;
  StringGrid3.RowCount := n;
  StringGrid1.Cells[0,0] := 'Матрица';
  StringGrid2.Cells[0,0] := 'Вектор';
  StringGrid3.Cells[0,0] := 'Результат';
  for i := 1 to n - 1 do
    for j := 1 to m - 1 do
    begin
      StringGrid1.Cells[0,i] := 'i = ' + IntToStr(i);
      StringGrid1.Cells[j,0] := 'j = ' + IntToStr(j);
      StringGrid1.Cells[j,i] := '';
      StringGrid2.Cells[0,j] := '';
      StringGrid3.Cells[0,i] := '';
    end;
end;

procedure TForm1.MatrixClick(Sender: TObject);
var
  i, j, n, r: Integer;
  temp: Real;
  Files: TextFile;
begin
  n := Matrix.ItemIndex;
  Vector.ItemIndex := n;
  case n of
    0: r := 3;
    1: r := 5;
    2: r := 8;
  end;
  StringGrid1.ColCount := r+1;
  StringGrid1.RowCount := r+1;
  StringGrid2.RowCount := r+1;
  StringGrid3.RowCount := r+1;
  StringGrid1.Cells[0,0] := 'Матрица';
  StringGrid2.Cells[0,0] := 'Вектор';
  StringGrid3.Cells[0,0] := 'Результат';
  for i := 1 to r do
  begin
    StringGrid1.Cells[0,i] := 'i = ' + IntToStr(i);
    StringGrid1.Cells[i,0] := 'j = ' + IntToStr(i);
    StringGrid3.Cells[0,i] := '';
  end;
  AssignFile(Files, 'M' + IntToStr(r) + '.txt');
  Reset(Files);
  for i := 1 to r do
  begin
    for j := 1 to r do
    begin
      Read(Files, temp);
      StringGrid1.Cells[j,i] := FloatToStrF(temp,fffixed,2,1);
    end;
    ReadLn(Files);
  end;
  CloseFile(Files);
  AssignFile(Files, 'V' + IntToStr(r) + '.txt');
  Reset(Files);
  for i := 1 to r do
  begin
    Read(Files, temp);
    StringGrid2.Cells[0,i] := FloatToStrF(temp,fffixed,2,1);
  end;
  CloseFile(Files);
end;

procedure TForm1.SaveRazmClick(Sender: TObject);
var
  n,m,i,j:Integer;
begin
  try n := StrToInt(Edit1.Text)
    except ShowMessage('Некорректно введено число строк!');
    exit;
  end;
  try m := StrToInt(Edit2.Text)
    except ShowMessage('Некорректно введено число столбцов!');
    exit;
  end;
  StringGrid1.ColCount := m + 1;
  StringGrid1.RowCount := n + 1;
  StringGrid2.RowCount := m + 1;
  StringGrid3.RowCount := n + 1;
  StringGrid1.Cells[0,0] := 'Матрица';
  StringGrid2.Cells[0,0] := 'Вектор';
  StringGrid3.Cells[0,0] := 'Результат';
  for i := 1 to n do
    for j := 1 to m do
    begin
      StringGrid1.Cells[0,i] := 'i = ' + IntToStr(i);
      StringGrid1.Cells[j,0] := 'j = ' + IntToStr(j);
      StringGrid1.Cells[j,i] := '';
      StringGrid2.Cells[0,j] := '';
      StringGrid3.Cells[0,i] := '';;
    end;
end;

procedure TForm1.SelectDoClick(Sender: TObject);
var
  s,n,m,i,j,r: Integer;
  Files: TextFile;
  temp: Real;
begin
  s := SelectDo.ItemIndex;
  case s of
    0: begin
      n := Matrix.ItemIndex;
      case n of
        0: r := 3;
        1: r := 5;
        2: r := 8;
      end;
      FilesSelect.Visible := True;
      ReadSelect.Visible := False;
      StringGrid1.Options:=[goFixedVertLine,goFixedHorzLine,goVertLine,goHorzLine,goRangeSelect,goSmoothScroll];
      StringGrid2.Options:=[goFixedVertLine,goFixedHorzLine,goVertLine,goHorzLine,goRangeSelect,goSmoothScroll];
      StringGrid1.ColCount := r+1;
      StringGrid1.RowCount := r+1;
      StringGrid2.RowCount := r+1;
      StringGrid3.RowCount := r+1;
      StringGrid1.Cells[0,0] := 'Матрица';
      StringGrid2.Cells[0,0] := 'Вектор';
      StringGrid3.Cells[0,0] := 'Результат';
      for i := 1 to r do
      begin
        StringGrid1.Cells[0,i] := 'i = ' + IntToStr(i);
        StringGrid1.Cells[i,0] := 'j = ' + IntToStr(i);
        StringGrid3.Cells[0,i] := '';
      end;
      AssignFile(Files, 'M' + IntToStr(r) + '.txt');
      Reset(Files);
      for i := 1 to r do
      begin
        for j := 1 to r do
        begin
          Read(Files, temp);
          StringGrid1.Cells[j,i] := FloatToStrF(temp,fffixed,2,1);
        end;
        ReadLn(Files);
      end;
      CloseFile(Files);
      AssignFile(Files, 'V' + IntToStr(r) + '.txt');
      Reset(Files);
      for i := 1 to r do
      begin
        Read(Files, temp);
        StringGrid2.Cells[0,i] := FloatToStrF(temp,fffixed,2,1);
      end;
      CloseFile(Files);
    end;
    1: begin
      FilesSelect.Visible := False;
      ReadSelect.Visible := True;
      StringGrid1.Options:=[goEditing,goFixedVertLine,goFixedHorzLine,goVertLine,goHorzLine,goRangeSelect,goSmoothScroll];
      StringGrid2.Options:=[goEditing,goFixedVertLine,goFixedHorzLine,goVertLine,goHorzLine,goRangeSelect,goSmoothScroll];
      try n := StrToInt(Edit1.Text)
        except ShowMessage('Некорректно введено число строк!');
        exit;
      end;
      try m := StrToInt(Edit2.Text)
        except ShowMessage('Некорректно введено число столбцов!');
        exit;
      end;
      StringGrid1.ColCount := m + 1;
      StringGrid1.RowCount := n + 1;
      StringGrid2.RowCount := m + 1;
      StringGrid3.RowCount := n + 1;
      StringGrid1.Cells[0,0] := 'Матрица';
      StringGrid2.Cells[0,0] := 'Вектор';
      StringGrid3.Cells[0,0] := 'Результат';
      for i := 1 to n do
        for j := 1 to m do
        begin
          StringGrid1.Cells[0,i] := 'i = ' + IntToStr(i);
          StringGrid1.Cells[j,0] := 'j = ' + IntToStr(j);
          StringGrid1.Cells[j,i] := '';
          StringGrid2.Cells[0,j] := '';
          StringGrid3.Cells[0,i] := '';;
        end;
    end;
  end;
end;

procedure TForm1.VectorClick(Sender: TObject);
var
  i, j, n, r: Integer;
  temp: Real;
  Files: TextFile;
begin
  n := Vector.ItemIndex;
  Matrix.ItemIndex := n;
  case n of
    0: r := 3;
    1: r := 5;
    2: r := 8;
  end;
  StringGrid1.ColCount := r+1;
  StringGrid1.RowCount := r+1;
  StringGrid2.RowCount := r+1;
  StringGrid3.RowCount := r+1;
  StringGrid1.Cells[0,0] := 'Матрица';
  StringGrid2.Cells[0,0] := 'Вектор';
  StringGrid3.Cells[0,0] := 'Результат';
  for i := 1 to r do
  begin
    StringGrid1.Cells[0,i] := 'i = ' + IntToStr(i);
    StringGrid1.Cells[i,0] := 'j = ' + IntToStr(i);
    StringGrid3.Cells[0,i] := '';
  end;
  AssignFile(Files, 'M' + IntToStr(r) + '.txt');
  Reset(Files);
  for i := 1 to r do
  begin
    for j := 1 to r do
    begin
      Read(Files, temp);
      StringGrid1.Cells[j,i] := FloatToStrF(temp,fffixed,2,1);
    end;
    ReadLn(Files);
  end;
  CloseFile(Files);
  AssignFile(Files, 'V' + IntToStr(r) + '.txt');
  Reset(Files);
  for i := 1 to r do
  begin
    Read(Files, temp);
    StringGrid2.Cells[0,i] := FloatToStrF(temp,fffixed,2,1);
  end;
  CloseFile(Files);
end;
  }
end.

