unit Unit1;

{$mode objfpc}{$H+}

interface

uses
  Classes, SysUtils, FileUtil, Forms, Controls, Graphics, Dialogs, StdCtrls,
  Buttons, Menus, ActnList;

type

  { TForm1 }

  TForm1 = class(TForm)
    Action1: TAction;
    ActionList1: TActionList;
    BitBtn1: TBitBtn;
    MainMenu1: TMainMenu;
    MenuItem1: TMenuItem;
    MenuItem2: TMenuItem;
    MenuItem3: TMenuItem;
    procedure Action1Execute(Sender: TObject);
    procedure MenuItem2Click(Sender: TObject);
    procedure MenuItem3Click(Sender: TObject);
  private
    { private declarations }
  public
    { public declarations }
  end;

var
  Form1: TForm1;

implementation
uses Unit2, Unit3;
{$R *.lfm}

{ TForm1 }

procedure TForm1.Action1Execute(Sender: TObject);
begin
  Close;
end;

procedure TForm1.MenuItem2Click(Sender: TObject);
begin
  Form2.ShowModal;
end;

procedure TForm1.MenuItem3Click(Sender: TObject);
begin
  Form3.Show;
end;

end.

