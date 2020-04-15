unit Unit2;

{$mode objfpc}{$H+}

interface

uses
  Classes, SysUtils, FileUtil, Forms, Controls, Graphics, Dialogs, StdCtrls,
  Buttons, ActnList;

type

  { TForm2 }

  TForm2 = class(TForm)
    Action1: TAction;
    ActionList1: TActionList;
    BitBtn1: TBitBtn;
    Label1: TLabel;
    procedure Action1Execute(Sender: TObject);
  private
    { private declarations }
  public
    { public declarations }
  end;

var
  Form2: TForm2;

implementation

{$R *.lfm}

{ TForm2 }

procedure TForm2.Action1Execute(Sender: TObject);
begin
  Close;
end;

end.

