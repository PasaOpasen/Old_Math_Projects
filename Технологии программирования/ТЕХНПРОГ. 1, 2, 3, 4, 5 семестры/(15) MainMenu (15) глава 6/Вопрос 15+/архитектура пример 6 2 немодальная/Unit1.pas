unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, Buttons;

type
  TForm1 = class(TForm)
    OpenModelesForm: TButton;
    CloseBtn: TBitBtn;
    procedure OpenModelesFormClick(Sender: TObject);
    procedure CloseBtnClick(Sender: TObject);
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

procedure TForm1.OpenModelesFormClick(Sender: TObject);
begin
  Form2.Show;
end;

procedure TForm1.CloseBtnClick(Sender: TObject);
begin
  close;
end;

end.
