unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls;

type
  TForm1 = class(TForm)
    ButtonOpenModelessForm: TButton;
    ButtonClose: TButton;
    procedure ButtonCloseClick(Sender: TObject);
    procedure ButtonOpenModelessFormClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

uses Unit2;

{$R *.dfm}

procedure TForm1.ButtonCloseClick(Sender: TObject);
begin
  Close;
end;

procedure TForm1.ButtonOpenModelessFormClick(Sender: TObject);
begin
  Form2.Show;
end;

end.
