unit Unit1;

{$mode objfpc}{$H+}

interface

uses
  Classes, SysUtils, FileUtil, Forms, Controls, Graphics, Dialogs, StdCtrls,
  ExtCtrls, Buttons, ActnList;

type

  { TForm1 }

  TForm1 = class(TForm)
    Cls: TAction;
    Go: TAction;
    ActionList1: TActionList;
    BitBtn1: TBitBtn;
    Button1: TButton;
    LabeledEdit1: TLabeledEdit;
    Memo1: TMemo;
    procedure ClsExecute(Sender: TObject);
    procedure GoExecute(Sender: TObject);
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


procedure TForm1.GoExecute(Sender: TObject);
var
  Mask: String;
  SR: TSearchRec;
begin
  Mask:= LabeledEdit1.Text;
  If Mask = '' then
  begin
    Mask:= '*.*';
    LabeledEdit1.Text:= 'D:\Setup\Mathcad\*.*';
  end;
  Memo1.Clear;
  If FindFirst(Mask,faAnyFile,SR)=0 then
    repeat
      Memo1.Lines.Add(SR.Name);
    until FindNext(SR)<>0
  Else
  begin
    ShowMessage('Такой папки или не существует, или она пуста');
    exit;
  end;
  FindClose(SR);
end;

procedure TForm1.ClsExecute(Sender: TObject);
begin
  Close;
end;

end.

