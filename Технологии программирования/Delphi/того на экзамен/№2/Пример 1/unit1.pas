unit Unit1;

{$mode objfpc}{$H+}

interface

uses
  Classes, SysUtils, process, FileUtil, Forms, Controls, Graphics, Dialogs,
  ExtCtrls, StdCtrls, Buttons, LCLIntf, Windows;

type

  { TForm1 }

  TForm1 = class(TForm)
    AddString: TButton;
    ClearResult1: TButton;
    CloseProgram: TBitBtn;
    ClearString: TButton;
    ClearResult: TButton;
    Result1: TGroupBox;
    ResultButton: TButton;
    ResultButton1: TButton;
    ResultText2: TLabel;
    ResultText1: TLabel;
    Result: TGroupBox;
    ResultText3: TLabel;
    ResultText4: TLabel;
    ResultText5: TLabel;
    ResultText6: TLabel;
    TextString: TEdit;
    ReadString: TGroupBox;
    SelectString: TComboBox;
    Select: TGroupBox;
    SelectDo: TRadioGroup;
    procedure AddStringClick(Sender: TObject);
    procedure ClearResultClick(Sender: TObject);
    procedure ClearResultClick1(Sender: TObject);
    procedure ClearStringClick(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure FilesOpen(Sender: TObject);
    procedure ResultButtonClick(Sender: TObject);
    procedure ResultButtonClick1(Sender: TObject);
    procedure SelectDoClick(Sender: TObject);
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
procedure TForm1.FilesOpen(Sender: TObject);
var
  Files: TextFile;
  temp: String;
begin
  AssignFile(Files, 'String.txt');
  try Reset(Files)
  except
    ShowMessage('Файл "String.txt" не найден!');
    exit;
  end;
  while not eof(Files) do
  begin
    try ReadLn(Files, temp)
    except
      ShowMessage('Битый файл!');
      exit;
    end;
    SelectString.Items.Add(AnsiToUtf8(temp));
  end;
  CloseFile(Files);
end;

procedure TForm1.ResultButtonClick(Sender: TObject);
var
  i, d, s, n, t, c, r: Integer;
  temp, ereg: String;
  Dt: Real;
  Fr1, Fr2, t1, t2: int64;
begin
  QueryPerformanceFrequency(Fr1);
  QueryPerformanceCounter(t1);

  s := SelectString.ItemIndex;
  n := SelectString.Items.Count - 1;
  if (s < 0) or (s > n) then
  begin
    ShowMessage('Выберите строку!');
    exit;
  end;
  r := 0;
  while r < d do
  begin
    t := 0;
    c := 0;
    ereg := ' ';
    temp := SelectString.Items[s];
    for i := 1 to Length(temp) do
    begin
      case t of
        0: if not ( temp[i] = ereg ) then
           begin
             t := 1;
             Inc(c);
           end;
        1: if ( temp[i] = ereg ) then t := 0;
      end;
    end;
    Inc(r);
  end;

  QueryPerformanceCounter(t2);
  Dt := (t2 - t1)/ Fr1;

  ResultText6.Caption := FloatToStrF(Dt, ffGeneral, 12, 12) + ' c.';
  ResultText2.Caption := IntToStr(c);
end;

procedure TForm1.ResultButtonClick1(Sender: TObject);
var
  i, d, s, n, c, r: Integer;
  temp, ereg: String;
  Dt: Real;
  Fr1, Fr2, t1, t2: int64;
begin

  QueryPerformanceFrequency(Fr1);
  QueryPerformanceCounter(t1);

  s := SelectString.ItemIndex;
  n := SelectString.Items.Count - 1;
  if (s < 0) or (s > n) then
  begin
    ShowMessage('Выберите строку!');
    exit;
  end;
  r := 0;
  while r < d do
  begin
    c := 0;
    ereg := ' ';
    temp := Trim(SelectString.Items[s]);
    if Length(temp)>0 then
      c := 1;
    while Length(temp)>0 do
    begin
      i := Pos(ereg, temp) - 1;
      if i = -1 then
        break;
      Delete(temp, 1, i);
      temp := Trim(temp);
      Inc(c);
    end;
    Inc(r);
  end;

  QueryPerformanceCounter(t2);
  Dt := (t2 - t1)/ Fr1;

  ResultText5.Caption := FloatToStrF(Dt, ffGeneral, 12, 12) + ' c.';
  ResultText4.Caption := IntToStr(c);
end;

procedure TForm1.SelectDoClick(Sender: TObject);
var
  s: Integer;
begin
  s := SelectDo.ItemIndex;
  ClearStringClick(Sender);
  case s of
    0:begin
      ReadString.Visible := false;
      FilesOpen(Sender);
    end;
    1:begin
      ReadString.Visible := true;
    end;
  end;
  ClearResultClick(Sender);
  ClearResultClick1(Sender);
  TextString.Text := '';
end;

procedure TForm1.FormCreate(Sender: TObject);
begin
  SelectDoClick(Sender);
end;

procedure TForm1.ClearStringClick(Sender: TObject);
var
  i, n: Integer;
begin
  ClearResultClick(Sender);
  n := SelectString.Items.Count - 1;
  for i := n downto 0 do
    SelectString.Items.Delete(i);
  SelectString.ItemIndex := -1;
  SelectString.Text := '';
end;

procedure TForm1.AddStringClick(Sender: TObject);
var
  temp: String;
begin
  ClearResultClick(Sender);
  ClearResultClick1(Sender);
  try temp := TextString.Text
  except
    ShowMessage('Ошибка ввода текста!');
    exit;
  end;
  if temp = '' then
  begin
    ShowMessage('Строка не может быть пустой!');
    exit;
  end;
  SelectString.Items.Add(temp);
  TextString.Text := '';
end;

procedure TForm1.ClearResultClick(Sender: TObject);
begin
  ResultText2.Caption := 'не посчитано';
  ResultText6.Caption := '';
end;

procedure TForm1.ClearResultClick1(Sender: TObject);
begin
  ResultText4.Caption := 'не посчитано';
  ResultText5.Caption := '';
end;

end.


