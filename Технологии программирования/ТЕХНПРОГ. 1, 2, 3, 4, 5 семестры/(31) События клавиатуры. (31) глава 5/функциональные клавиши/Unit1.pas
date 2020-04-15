unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    procedure FormKeyDown(Sender: TObject; var Key: Word;
      Shift: TShiftState);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

{$R *.dfm}

procedure TForm1.FormKeyDown(Sender: TObject; var Key: Word;
  Shift: TShiftState);
begin
  if key=VK_F1  then
  ShowMessage('вы нажали клавишу F1')  else
  if key=VK_F2 then
  ShowMessage('вы нажали клавишу F2')else
  if key=VK_F3 then
  ShowMessage('вы нажали клавишу F3')else
  if key=VK_F4 then
  ShowMessage('вы нажали клавишу F4')else
  if key=VK_DELETE then
  ShowMessage('вы нажали клавишу delete')else
  if key=VK_F5 then
  ShowMessage('вы нажали клавишу F5') else
  if key=VK_F6 then
  ShowMessage('вы нажали клавишу F6') else
  if key=VK_F7 then
  ShowMessage('вы нажали клавишу F7') else
  if key=VK_F8 then
  ShowMessage('вы нажали клавишу F8')  else
  if key=VK_F9 then
  ShowMessage('вы нажали клавишу F9')  else
  if key=VK_F10 then
  ShowMessage('вы нажали клавишу F10')  else
  if key=VK_F11 then
  ShowMessage('вы нажали клавишу F11')  else
  if key=VK_F12 then
  ShowMessage('вы нажали клавишу F12')   else
  ShowMessage('вы нажали не функциональную клавишу');



end;

end.
