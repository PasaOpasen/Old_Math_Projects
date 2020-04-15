unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, Grids, StdCtrls;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    EditNN: TEdit;
    SetDim: TButton;
    GroupBox1: TGroupBox;
    TestData: TButton;
    StringGrid1: TStringGrid;
    SortRow: TButton;
    SortCol: TButton;
    Label2: TLabel;
    Label3: TLabel;
    EditMM: TEdit;
    RadioButton3: TRadioButton;
    RadioButton4: TRadioButton;
    RadioButton5: TRadioButton;
    procedure FormCreate(Sender: TObject);
    procedure SetDimClick(Sender: TObject);
    procedure TestDataClick(Sender: TObject);
    procedure SortRowClick(Sender: TObject);
    procedure SortColClick(Sender: TObject);
    procedure RadioButton3Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

{$R *.dfm}


procedure TForm1.FormCreate(Sender: TObject);
var i: Integer;
begin
  StringGrid1.Cells[0,0]:='A';
  for i:=1 to 100 do
    begin
     StringGrid1.Cells[0,i]:='i='+IntToStr(i);
     StringGrid1.Cells[i,0]:='j='+IntToStr(i);
     end;
end;


procedure TForm1.SetDimClick(Sender: TObject);
var n,m,i,j: Integer;
begin
  try
    n:=StrToInt(EditNN.Text);
    m:=StrToInt(EditMM.Text);
  except
    ShowMessage('Некорректная размерность');
    Exit;
  end;
  TestData.Enabled:=true;
  if n = m then
    case n of
      3: RadioButton3.Checked:=true;
      4: RadioButton4.Checked:=true;
      5: RadioButton5.Checked:=true;
    else
      TestData.Enabled:=false;
      RadioButton3.Checked:=false;
      RadioButton4.Checked:=false;
      RadioButton5.Checked:=false;
    end
  else
    begin
      TestData.Enabled:=false;
      RadioButton3.Checked:=false;
      RadioButton4.Checked:=false;
      RadioButton5.Checked:=false;
    end;
  StringGrid1.ColCount:=m+1;
  StringGrid1.RowCount:=n+1;
  for i:=1 to n do
    for j:=1 to m do
      StringGrid1.Cells[j,i]:='';
end;




procedure TForm1.TestDataClick(Sender: TObject);
var f: TextFile;
    n,i,j: Integer;
    symb: Real;
begin
  AssignFile(f,'In.txt');
  try
  Reset(f);
  n:=StringGrid1.RowCount-1;
  for i:=1 to n do
    begin
     for j:=1 to n do
       begin
        Read(f,symb);
        StringGrid1.Cells[j,i]:=FloatToStr(symb);
       end;
     ReadLn(f);
    end;
  CloseFile(f);
  except
    ShowMessage('Ошибка чтения из файла');
  end;
end;

procedure TForm1.SortRowClick(Sender: TObject);
var a: array [1..100,1..100] of Real;
    n,m,i,j,num: Integer;
    c: Real;
begin
  n:=StringGrid1.RowCount-1;
  m:=StringGrid1.ColCount-1;
  try
    for i:=1 to n do
      for j:=1 to m do
        a[i,j]:=StrToFloat(StringGrid1.Cells[j,i]);
  except
    ShowMessage('Некорректно заданная матрица');
    Exit;
  end;
  for i:=1 to n-1 do
    begin
     c:=a[i,1];
     num:=i;
     for j:=i+1 to n do
       if a[j,1] < c then
         begin
          c:=a[j,1];
          num:=j;
         end;
     if num <> i then
       for j:=1 to m do
         begin
          c:=a[num,j];
          a[num,j]:=a[i,j];
          a[i,j]:=c;
         end;
    end;
  for i:=1 to n do
    for j:=1 to m do
      StringGrid1.Cells[j,i]:=FloatToStr(a[i,j]);
end;

procedure TForm1.SortColClick(Sender: TObject);
var a: array [1..100,1..100] of Real;
    n,m,i,j,num: Integer;
    c: Real;
begin
  n:=StringGrid1.RowCount-1;
  m:=StringGrid1.ColCount-1;
  try
    for i:=1 to n do
      for j:=1 to m do
        a[i,j]:=StrToFloat(StringGrid1.Cells[j,i]);
  except
    ShowMessage('Некорректно заданная матрица');
    Exit;
  end;
  for i:=1 to m-1 do
    begin
     c:=a[1,i];
     num:=i;
     for j:=i+1 to m do
       if a[1,j] < c then
         begin
          c:=a[1,j];
          num:=j;
         end;
     if num <> i then
       for j:=1 to n do
         begin
          c:=a[j,num];
          a[j,num]:=a[j,i];
          a[j,i]:=c;
         end;
    end;
  for i:=1 to n do
    for j:=1 to m do
      StringGrid1.Cells[j,i]:=FloatToStr(a[i,j]);
end;

procedure TForm1.RadioButton3Click(Sender: TObject);
var i,j,n: Integer;
begin
  if RadioButton3.Checked then
    n:=3
  else
    if RadioButton4.Checked then
      n:=4
    else
      n:=5;
  TestData.Enabled:=true;
  EditNN.Text:=IntToStr(n);
  EditMM.Text:=IntToStr(n);
  StringGrid1.ColCount:=n+1;
  StringGrid1.RowCount:=n+1;
  for i:=1 to n do
    for j:=1 to n do
      StringGrid1.Cells[j,i]:='';
end;

end.
