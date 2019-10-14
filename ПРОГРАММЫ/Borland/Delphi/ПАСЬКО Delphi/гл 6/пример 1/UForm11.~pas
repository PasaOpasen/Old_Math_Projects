unit UForm11;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls;

type
  TForm1 = class(TForm)
    OpenModalForm: TButton;
    CloseBtn: TButton;
    procedure OpenModalFormClick(Sender: TObject);
    procedure CloseBtnClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

uses UForm12;

{$R *.dfm}

procedure TForm1.OpenModalFormClick(Sender: TObject);
begin
  Form2.ShowModal;
end;

procedure TForm1.CloseBtnClick(Sender: TObject);
begin
  Close;
end;

end.
