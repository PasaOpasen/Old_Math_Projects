unit Unit2;       //содержание модуля 

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, Buttons;

type
  TForm2 = class(TForm)
    BlyeLbl: TLabel;
    OkBtn: TBitBtn;
    procedure OkBtnClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form2: TForm2;

implementation

{$R *.dfm}

procedure TForm2.OkBtnClick(Sender: TObject);
begin
  close;
end;

end.
