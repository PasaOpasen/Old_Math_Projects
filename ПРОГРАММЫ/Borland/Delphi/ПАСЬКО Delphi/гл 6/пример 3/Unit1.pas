unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, Menus, StdCtrls, Buttons;

type
  TForm1 = class(TForm)
    MainMenu1: TMainMenu;
    N1: TMenuItem;
    OpenModalForm: TMenuItem;
    OpenModelessForm: TMenuItem;
    BitBtn1: TBitBtn;
    procedure OpenModalFormClick(Sender: TObject);
    procedure OpenModelessFormClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

uses Unit21, Unit22;

{$R *.dfm}

procedure TForm1.OpenModalFormClick(Sender: TObject);
begin
  Form2.ShowModal;
end;

procedure TForm1.OpenModelessFormClick(Sender: TObject);
begin
  Form3.Show;
end;

end.
