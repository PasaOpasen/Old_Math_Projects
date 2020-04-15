unit Unit1;

{$mode objfpc}{$H+}

interface

uses
  Classes, SysUtils, FileUtil, Forms, Controls, Graphics, Dialogs, StdCtrls,
  Buttons, ActnList, Unit2;

type

  { TForm1 }

  TForm1 = class(TForm)
    Action1: TAction;
    Action2: TAction;
    ActionList1: TActionList;
    BitBtn1: TBitBtn;
    Button1: TButton;
    procedure Action1Execute(Sender: TObject);
    procedure Action2Execute(Sender: TObject);
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

procedure TForm1.Action1Execute(Sender: TObject);
begin
  Form2.ShowModal;
end;

procedure TForm1.Action2Execute(Sender: TObject);
begin
  Close;
end;

end.

