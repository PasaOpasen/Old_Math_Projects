unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, Menus, StdCtrls, Buttons;

type
  TForm1 = class(TForm)
    MainMenu1: TMainMenu;
    OpenModalessForm: TMenuItem;
    OpenModelForm: TMenuItem;
    OpenModelessForm: TMenuItem;
    BitBtn1: TBitBtn;
    procedure OpenModelessFormClick(Sender: TObject);
    procedure OpenModelFormClick(Sender: TObject);
   
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

uses Unit2, Unit3;

{$R *.dfm}



procedure TForm1.OpenModelFormClick(Sender: TObject);
begin
  Form2.ShowModal; //גûחמג למהאכüםמי פמנלû
end;

procedure TForm1.OpenModelessFormClick(Sender: TObject);
begin
  Form3.Show;  //גûחמג םולמהאכüםמי פמנלû
end;

end.
