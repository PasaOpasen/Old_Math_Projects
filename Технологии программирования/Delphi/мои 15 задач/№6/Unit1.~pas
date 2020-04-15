unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, Buttons;

type
  TForm1 = class(TForm)
    Memo1: TMemo;
    BitBtn1: TBitBtn;
    Edit1: TEdit;
    procedure BitBtn1Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

{$R *.dfm}

procedure TForm1.BitBtn1Click(Sender: TObject);
var
  Mask: String;
  SR: TSearchRec;
begin
  Mask:= Edit1.Text;
  If Mask = '' then
  begin
    Mask:= '*.*';
    Edit1.Text:= 'C:\*.*';
  end;
  Memo1.Clear;
  If FindFirst(Mask,faAnyFile,SR)=0 then
    repeat
      Memo1.Lines.Add(SR.Name);
    until FindNext(SR)<>0
  Else
  begin
    //ShowMessage('¦â¦-¦¦¦-¦¦ ¦¬¦-¦¬¦¦¦¬ ¦¬¦¬¦¬ ¦-¦¦ TÁTÃTÉ¦¦TÁTÂ¦-TÃ¦¦TÂ, ¦¬¦¬¦¬ ¦-¦-¦- ¦¬TÃTÁTÂ¦-');
    exit;
  end;
  FindClose(SR);
end;

end.
