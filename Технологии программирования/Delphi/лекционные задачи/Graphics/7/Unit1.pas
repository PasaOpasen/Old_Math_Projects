unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls;

type
  TForm1 = class(TForm)
    PaintBox1: TPaintBox;
    ColorBox1: TColorBox;
    Button1: TButton;
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
const Style:array[0..7] of string[20]=('bsSolid','bsClear','bsHorizontal','bsVertical',
      'bsFDiagonal','bsBDiagonal','bsCross','bsDiagCross');
var x,y,i:integer;
begin
  PaintBox1.Refresh;
  x:=10;
  y:=10;
  PaintBox1.Refresh;
  for i:=0 to 7 do
    begin
      PaintBox1.Canvas.Brush.Color:=clBtnFace;
      PaintBox1.Canvas.TextOut(x+110,y+110,Style[i]);
      PaintBox1.Canvas.Brush.Color:=ColorBox1.Selected;
      PaintBox1.Canvas.Brush.Style:=TBrushStyle(i);
      PaintBox1.Canvas.Rectangle(x,y,x+100,y+100);
      y:=y+50;
      if i<>3 then y:=y+60
      else
        begin
          y:=10;
          x:=300;
        end;
    end;
end;

end.
