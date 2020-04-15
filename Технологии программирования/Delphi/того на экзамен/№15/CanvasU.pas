unit CanvasU;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, Buttons, ExtCtrls;

type
  TfmExample = class(TForm)
    Panel1: TPanel;
    bbClose: TBitBtn;
    pbUfo: TPaintBox;
    Timer1: TTimer;
    procedure Timer1Timer(Sender: TObject);
  private
    { Private declarations }
    SRect: TRect;
    procedure CreateUfo(X, Y: Integer);
  public
    { Public declarations }
  end;

var
  fmExample: TfmExample;

implementation

{$R *.dfm}

procedure TfmExample.CreateUfo(X, Y: Integer);
const
  r = 30;   //Характерный размер НЛО
begin
  with pbUfo.Canvas do
  begin
    //Создаем основной эллипс:
    Brush.Style := bsSolid;
    Brush.Color := clWhite;
    Ellipse(X, Y+(r div 6), X+3*r, Y+r);
    //Отсекаем его нижнюю часть:
    Arc(X, Y+(r div 6), X+3*r, Y+r-r div 6,
        X, (Y+r-r div 6) div 2, X+3*r, (Y+r-r div 6) div 2);
    //Закрашиваем нижнюю часть:
    Brush.Color := clBlack;
    FloodFill((X+3*r) div 2, Y+r-4, clWhite, fsSurface);
    //Создаем "антенны":
    Ellipse(X, Y, X+r div 4, Y+r div 4);
    Ellipse(X+3*r-r div 4, Y, X+3*r, Y+r div 4);
    MoveTo(X+r div 8, Y+r div 8);
    LineTo(X+r div 2, Y+r div 2);
    MoveTo(X+3*r-r div 7, Y+r div 8);
    LineTo(X+3*r-r div 2, Y+r div 2);
    //Запоминаем координаты изображения:
    SRect.Left := X;
    SRect.Top := Y;
    SRect.Right := X+3*r;
    SRect.Bottom := Y+r;
  end;
end;

procedure TfmExample.Timer1Timer(Sender: TObject);
begin
  if SRect.Right<>0 then with pbUfo.Canvas do
  begin
    Brush.Style := bsSolid;
    Brush.Color := clBtnFace;
    FillRect(SRect);
  end;
    CreateUfo(Random(pbUfo.Canvas.ClipRect.Right),
      Random(pbUfo.Canvas.ClipRect.Bottom))
end;

end.
