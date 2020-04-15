unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, Buttons, StdCtrls, ActnList;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    Edit1: TEdit;
    Content: TMemo;
    ActionList1: TActionList;
    BitBtn1: TBitBtn;
    Button1: TButton;
    procedure Button1Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

{$R *.dfm}


procedure TForm1.Button1Click(Sender: TObject);
var
  Mask: String;
  SR: TSearchRec;
begin
  Mask:= Edit1.Text;
  If Mask ='.' then
  begin
    Mask:='*.*';
    Edit1.Text:= '*.*';
  end;
  Content.Clear;
  If FindFirst(Mask,faAnyFile,SR)=0 then
    repeat
      Content.Lines.Add(SR.Name);
    until FindNext(SR)<>0
  Else
  begin
    ShowMessage('Такой папки не существует,или она пустая');
    exit;
  end;
  FindClose(SR);
end;
end.
