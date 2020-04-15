unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, Buttons;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    ComboBox1: TComboBox;
    Label2: TLabel;
    ComboBox2: TComboBox;
    Memo1: TMemo;
    BitBtn1: TBitBtn;
    ComboBox3: TComboBox;
    ComboBox4: TComboBox;
    ComboBox5: TComboBox;
    Label3: TLabel;
    Label4: TLabel;
    Label5: TLabel;
    procedure ComboBox1Change(Sender: TObject);
    procedure ComboBox2Change(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure ComboBox3Change(Sender: TObject);
    procedure ComboBox4Change(Sender: TObject);
    procedure ComboBox5Change(Sender: TObject);

  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

{$R *.dfm}

procedure TForm1.ComboBox1Change(Sender: TObject);
var f:TextFile;
    nom: Integer;
    str: String;

begin
  Memo1.Clear;
  nom:=ComboBox1.ItemIndex;
  if nom=0 then
  Begin
  AssignFile(f,'FOR.txt');
  Reset(f);
  While not eof (f) do
  Begin
    Readln(f,str);
    Memo1.Lines.Add(str);
  End;
  CloseFile(f);
  end;
  if nom=1 then
  Begin
  AssignFile(f,'WHILE.txt');
  Reset(f);
  While not eof (f) do
  Begin
    Readln(f,str);
    Memo1.Lines.Add(str);
  End;
  CloseFile(f);
  end;
   if nom=2 then
  Begin
  AssignFile(f,'REPEAT.txt');
  Reset(f);
  while not eof(f) do
  Begin
    Readln(f,str);
    Memo1.Lines.Add(str);
  end;
  CloseFile(f);
  end;
end;

procedure TForm1.ComboBox2Change(Sender: TObject);
var f:TextFile;
    nom: Integer;
    str: String;
begin
  Memo1.Clear;
  nom:=ComboBox2.ItemIndex;
  if nom=0 then
  Begin
    AssignFile(f,'COPY.txt');
    Reset(f);
    while not eof(f) do
    Begin
      Readln(f,str);
      Memo1.Lines.Add(str);
    end;
    CloseFile(f);
  end;
  if nom=1 then
  Begin
    AssignFile(f,'DELETE.txt');
    Reset(f);
    while not eof(f) do
    Begin
      Readln(f,str);
      Memo1.Lines.Add(str);
    end;
    CloseFile(f);
  end;
  if nom=2 then
  Begin
    AssignFile(f,'CONCAT.txt');
    Reset(f);
    while not eof(f) do
    Begin
      Readln(f,str);
      Memo1.Lines.Add(str);
    end;
    CloseFile(f);
  end;
end;


procedure TForm1.FormCreate(Sender: TObject);
begin
  Memo1.Clear;
end;


procedure TForm1.ComboBox3Change(Sender: TObject);
var f:TextFile;
    nom: Integer;
    str: String;
begin
  Memo1.Clear;
  nom:=ComboBox3.ItemIndex;
  if nom=0 then
  Begin
    AssignFile(f,'integer.txt');
    Reset(f);
    while not eof(f) do
    Begin
      Readln(f,str);
      Memo1.Lines.Add(str);
    end;
    CloseFile(f);
  end;
  if nom=1 then
  Begin
    AssignFile(f,'real.txt');
    Reset(f);
    while not eof(f) do
    Begin
      Readln(f,str);
      Memo1.Lines.Add(str);
    end;
    CloseFile(f);
  end;
  if nom=2 then
  Begin
    AssignFile(f,'boolean.txt');
    Reset(f);
    while not eof(f) do
    Begin
      Readln(f,str);
      Memo1.Lines.Add(str);
    end;
    CloseFile(f);
  end;
  if nom=3 then
  Begin
    AssignFile(f,'array.txt');
    Reset(f);
    while not eof(f) do
    Begin
      Readln(f,str);
      Memo1.Lines.Add(str);
    end;
    CloseFile(f);
  end;
end;




procedure TForm1.ComboBox4Change(Sender: TObject);
var f:TextFile;
    nom: Integer;
    str: String;
begin
  Memo1.Clear;
  nom:=ComboBox4.ItemIndex;
  if nom=0 then
  Begin
    AssignFile(f,'if.txt');
    Reset(f);
    while not eof(f) do
    Begin
      Readln(f,str);
      Memo1.Lines.Add(str);
    end;
    CloseFile(f);
  end;
  if nom=1 then
  Begin
    AssignFile(f,'case.txt');
    Reset(f);
    while not eof(f) do
    Begin
      Readln(f,str);
      Memo1.Lines.Add(str);
    end;
    CloseFile(f);
  end;
end;





procedure TForm1.ComboBox5Change(Sender: TObject);
var f:TextFile;
    nom: Integer;
    str: String;
begin
  Memo1.Clear;
  nom:=ComboBox5.ItemIndex;
  if nom=0 then
  Begin
    AssignFile(f,'break.txt');
    Reset(f);
    while not eof(f) do
    Begin
      Readln(f,str);
      Memo1.Lines.Add(str);
    end;
    CloseFile(f);
  end;
  if nom=1 then
  Begin
    AssignFile(f,'exit.txt');
    Reset(f);
    while not eof(f) do
    Begin
      Readln(f,str);
      Memo1.Lines.Add(str);
    end;
    CloseFile(f);
  end;
  if nom=2 then
  Begin
    AssignFile(f,'halt.txt');
    Reset(f);
    while not eof(f) do
    Begin
      Readln(f,str);
      Memo1.Lines.Add(str);
    end;
    CloseFile(f);
  end;
end;

end.



