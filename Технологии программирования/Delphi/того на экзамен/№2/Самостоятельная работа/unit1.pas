unit Unit1;

{$mode objfpc}{$H+}

interface

uses
  Classes, SysUtils, process, FileUtil, Forms, Controls, Graphics, Dialogs,
  ExtCtrls, StdCtrls, Buttons, LCLIntf, Windows;

type

  { TForm1 }

  TForm1 = class(TForm)
    BitBtn1: TBitBtn;
    AddString: TButton;
    Label3: TLabel;
    Label4: TLabel;
    ResultMemo: TButton;
    ResultButton: TButton;
    ClearString: TButton;
    ClearResult: TButton;
    ClearMemo: TButton;
    SelectString: TComboBox;
    TextString: TEdit;
    NumberSubstring: TEdit;
    ReadString: TGroupBox;
    Select: TGroupBox;
    SelectSubstring: TGroupBox;
    Label1: TLabel;
    Label2: TLabel;
    ResultText1: TLabel;
    ResultText2: TLabel;
    MemoWrite: TMemo;
    SelectDo: TRadioGroup;
    procedure AddStringClick(Sender: TObject);
    procedure ClearMemoClick(Sender: TObject);
    procedure ClearResultClick(Sender: TObject);
    procedure ClearStringClick(Sender: TObject);
    procedure FilesOpen(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure ResultButtonClick(Sender: TObject);
    procedure ResultMemoClick(Sender: TObject);
    procedure SelectDoClick(Sender: TObject);
    procedure SelectSringKeyPress(Sender: TObject; var Key: char);

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

procedure TForm1.ClearResultClick(Sender: TObject);
begin
  ResultText2.Caption := 'не выбрана';
  NumberSubstring.clear;
end;

procedure TForm1.AddStringClick(Sender: TObject);
var
  temp: String;
begin

  ClearResultClick(Sender);
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

procedure TForm1.ClearMemoClick(Sender: TObject);
begin
  MemoWrite.clear;
end;

procedure TForm1.FormCreate(Sender: TObject);
begin
  MemoWrite.Text := '';
  SelectDoClick(Sender);
end;


procedure TForm1.ResultButtonClick(Sender: TObject);
var
  s, word, st, temp: String;
  i, nst, p, n, d: Integer;
  Fr1, Fr2, t1, t2: int64;
  Dt: Real;
begin
 // ClearResultClick(Sender);
  try temp := SelectString.Text
  except
    ShowMessage('Ошибка ввода текста!');
    exit;
  end;
  if temp = '' then
  begin
    ShowMessage('Не выбрана ни одна строка');
    exit;
  end;

  QueryPerformanceFrequency(Fr1);
  QueryPerformanceCounter(t1);

  n := StrToInt(NumberSubstring.Text);
  nst := SelectString.ItemIndex;
  st := SelectString.Items[nst];
  s := Trim(st);
  p := 0;
  while Length(s)>0 do
  begin
    i := Pos(' ', s) - 1;
    if i = -1 then
    begin
      p := 1;
      word := s;
      break;
    end;
    word := Copy(s,1,i);
    Delete(s, 1, i);
    s := Trim(s);
    Inc(p);
    if n = p then
      break;
  end;

  QueryPerformanceCounter(t2);
  Dt := (t2 - t1)/ Fr1;
  Label3.Caption := FloatToStrF(Dt, ffGeneral, 12, 12) + ' c.';

  ResultText2.Caption := word;
end;

procedure TForm1.ResultMemoClick(Sender: TObject);
var
  s: String;
  c, d: Integer;
  q: Extended;
  Fr1, Fr2, t1, t2: int64;
  Dt: Real;
begin
  QueryPerformanceFrequency(Fr1);
  QueryPerformanceCounter(t1);
  s := ResultText2.Caption;
  val(s, q, c);
  if c = 0 then
  begin
    if frac(q) = 0 then
      begin
        if q < 0 then
          MemoWrite.Text := 'Подстрока является целым, отрицательным числом!'
        else
          MemoWrite.Text := 'Подстрока является целым, положительным числом!';
      end;

    if frac(q) <> 0 then
      begin
        if q < 0 then
          MemoWrite.Text := 'Подстрока является вещественным, отрицательным числом!'
        else
          MemoWrite.Text := 'Подстрока является вещественным, положительным числом!';
      end;
  end
  else
    MemoWrite.Text := ('Подстрока не является числом!');
  QueryPerformanceCounter(t2);
  Dt := (t2 - t1)/ Fr1;

  Label4.Caption := FloatToStrF(Dt, ffGeneral, 12, 12) + ' c.';
end;

procedure TForm1.SelectDoClick(Sender: TObject);
var
  s: Integer;
begin
  s := SelectDo.ItemIndex;
  ClearStringClick(Sender);
  case s of
    0: begin
         ReadString.Visible := true;
       end;
    1: begin
         ReadString.Visible := false;
         FilesOpen(Sender);
       end;
  end;
  ClearResultClick(Sender);
  TextString.Text := '';
end;

procedure TForm1.SelectSringKeyPress(Sender: TObject; var Key: char);
const
  Digit: set of Char=['1'..'9', '0', '.', 'E', '+', '-', ' '];
begin
  with (Sender as TEdit) do
  begin
    if (not(Key in Digit)) then
      Key := #0;
  end;

end;
end.

