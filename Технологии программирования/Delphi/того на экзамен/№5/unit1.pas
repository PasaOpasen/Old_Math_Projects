unit Unit1;

{$mode objfpc}{$H+}

interface

uses
  Classes, SysUtils, FileUtil, Forms, Controls, Graphics, Dialogs, StdCtrls,
  Buttons, ActnList, ExtCtrls;

type

  { TForm1 }

  TForm1 = class(TForm)
    Button3: TButton;
    Cls: TAction;
    BitBtn1: TBitBtn;
    LabeledEdit1: TLabeledEdit;
    Memo1: TMemo;
    Sv: TAction;
    Rd: TAction;
    ActionList1: TActionList;
    Button1: TButton;
    Button2: TButton;
    procedure Button3Click(Sender: TObject);
    procedure ClsExecute(Sender: TObject);
    procedure RdExecute(Sender: TObject);
    procedure SvExecute(Sender: TObject);
  private
    { private declarations }
  public
    { public declarations }
  end;

var
  Form1: TForm1;
  z: TextFile;

implementation

{$R *.lfm}

{ TForm1 }

procedure TForm1.RdExecute(Sender: TObject);
var
  f: TextFile;
  Nm, Nm_bak: string;
begin
  Memo1.Clear;
  If Button2.Visible then  CloseFile(z);
  Nm:= LabeledEdit1.Text;
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
begin
  LabeledEdit1.Text:= '1.txt';
end;

procedure TForm1.SvExecute(Sender: TObject);
var
  i: integer;
begin
  With Memo1 do
   for i:= 0 to Lines.Count-1 do
    writeln(z,Lines[i]);
  CloseFile(z);
  Button2.Visible:= false;
  LabeledEdit1.Text:='';
  Memo1.Clear;
  LabeledEdit1.SetFocus;
end;


end.

