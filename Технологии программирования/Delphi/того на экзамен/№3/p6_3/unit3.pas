unit Unit3;

{$mode objfpc}{$H+}

interface

uses
  Classes, SysUtils, FileUtil, Forms, Controls, Graphics, Dialogs, StdCtrls,
  Buttons, ActnList;

type

  { TForm3 }

  TForm3 = class(TForm)
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
  Form3: TForm3;

implementation

{$R *.lfm}

{ TForm3 }

procedure TForm3.Action1Execute(Sender: TObject);
begin
  Close;
end;

end.

