unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls;

type
  TForm1 = class(TForm)
    Shape1: TShape;
    ComboBox1: TComboBox;
    Button1: TButton;
    ColorBox1: TColorBox;
    ColorBox2: TColorBox;
    ComboBox2: TComboBox;
    ComboBox3: TComboBox;
    procedure Button1Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

{$R *.dfm}

procedure TForm1.Button1Click(Sender: TObject);
var s:string;
begin
  Shape1.Shape:=TShapeType(ComboBox1.ItemIndex);
  Shape1.Brush.Color:=ColorBox1.Selected;
  Shape1.Brush.Style:=TBrushStyle(ComboBox2.ItemIndex);
  Shape1.Pen.Color:=ColorBox2.Selected;
  Shape1.Pen.Style:=TPenStyle(ComboBox3.ItemIndex);


end;

end.
