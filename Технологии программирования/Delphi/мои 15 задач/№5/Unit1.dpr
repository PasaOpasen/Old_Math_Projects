unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, Buttons, StdCtrls, ActnList;

type
  TForm1 = class(TForm)
    Label1:Tlabel;
    Button3: TButton;
    BitBtn1: TBitBtn;
    Edit1: TEdit;
    Memo1: TMemo;
    Button1: TButton;
    Button2: TButton;
    ActionList1: TActionList;
    procedure Button3Click(Sender: TObject);
    procedure ClsExecute(Sender: TObject);
    procedure RdExecute(Sender: TObject);
    procedure SvExecute(Sender: TObject);
    procedure Button1Click(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure Button2Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;
  z: TextFile;
  constfile:string;

implementation

{$R *.dfm}
procedure TForm1.RdExecute(Sender: TObject);
var
  f: TextFile;
  //Fi,Fo:textfile;//исходный и отредактированный файл
  Nm, Nm_bak: string;//имя исходного и отредактированного файла
begin
  Memo1.Clear;

  If Button2.Visible then  CloseFile(z);
  Nm:= Edit1.Text;
  //проверка существования файла
  If FileExists(Nm) then
    AssignFile(f,Nm)
  Else
  begin
    ShowMessage('Такого файла не существует');
    exit;
  end;

  //удаление, если существует, bак файла
  Nm_bak:= copy(Nm, 1, pos('.',Nm)) + 'bak';
  If FileExists(Nm_bak) then
  begin
   AssignFile(z, Nm_bak);
   Erase(z);
  end;

  //чтение из файла в поле
  Memo1.Lines.LoadFromFile(Nm);
  //создание bak файла
  Rename(f,Nm_bak);
  Reset(f);
  AssignFile(z,Nm);
  Rewrite(z);
  CloseFile(f);
  Button2.Visible:= true;
end;

procedure TForm1.ClsExecute(Sender: TObject);
begin
  Close;
end;

procedure TForm1.Button3Click(Sender: TObject);
var
  i: integer;
begin
  //Edit1.Text:= constfile;
  With Memo1 do
  for i:= 0 to Lines.Count-1 do writeln(z,Lines[i]);
  CloseFile(z);
  //Button2.Visible:= false;
  Edit1.Text:='';
  Memo1.Clear;
  Edit1.SetFocus;
end;

procedure TForm1.SvExecute(Sender: TObject);  //сохранение
var
  i: integer;
begin
  With Memo1 do
   for i:= 0 to Lines.Count-1 do
    writeln(z,Lines[i]);
  CloseFile(z);
  Button2.Visible:= false;
  Edit1.Text:='';
  Memo1.Clear;
  Edit1.SetFocus;
end;


procedure TForm1.Button1Click(Sender: TObject);
begin
  Edit1.Text:= constfile;
end;

procedure TForm1.FormCreate(Sender: TObject);
begin
 constfile:='1.txt';
end;

procedure TForm1.Button2Click(Sender: TObject);
var
  f: TextFile;
  //Fi,Fo:textfile;//исходный и отредактированный файл
  Nm, Nm_bak: string;//имя исходного и отредактированного файла
begin
  Memo1.Clear;

  //If Button2.Visible then  CloseFile(z);
  Nm:= Edit1.Text;
  //проверка существования файла
  If FileExists(Nm) then
    AssignFile(f,Nm)
  Else
  begin
    ShowMessage('Такого файла не существует');
    exit;
  end;

  //удаление, если существует, bак файла
  Nm_bak:= copy(Nm, 1, pos('.',Nm)) + 'bak';
  If FileExists(Nm_bak) then
  begin
   AssignFile(z, Nm_bak);
   Erase(z);
  end;

  //чтение из файла в поле
  Memo1.Lines.LoadFromFile(Nm);
  //создание bak файла
  Rename(f,Nm_bak);
  Reset(f);
  AssignFile(z,Nm);
  Rewrite(z);
  CloseFile(f);
  //Button2.Visible:= true;
end;

end.


