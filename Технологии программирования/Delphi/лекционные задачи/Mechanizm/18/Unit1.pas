unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, Buttons, ExtCtrls;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    BitBtn1: TBitBtn;
    ShapeRectangle: TShape;
    ShapeCircle: TShape;
    RadioGroup1: TRadioGroup;
    procedure Label1DragOver(Sender, Source: TObject; X, Y: Integer;
      State: TDragState; var Accept: Boolean);


  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

{$R *.dfm}







procedure TForm1.Label1DragOver(Sender, Source: TObject; X, Y: Integer;
  State: TDragState; var Accept: Boolean);
begin
 Accept:=True;
 Label1.Caption:=(Source as Tcomponent).Name;
end;




end.
