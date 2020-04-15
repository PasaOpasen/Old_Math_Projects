unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtDlgs, ExtCtrls, Buttons;

type
  TForm1 = class(TForm)
    pnl1: TPanel;
    btn2: TBitBtn;
    pbOut: TPaintBox;
    pbBit: TPaintBox;
    spl1: TSplitter;
    cbBrushColor: TColorBox;
    dlgOpenPic1: TOpenPictureDialog;
    btn3: TBitBtn;
    lbl1: TLabel;
    bb1: TButton;
    procedure btn3Click(Sender: TObject);
    procedure bb1Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

{$R *.dfm}

procedure TForm1.btn3Click(Sender: TObject);
const
  Names: array[0..7] of string[12] = ('bsSolid','bsClear','bsHorizontal','bsVertical',
    'bsFDiagonal','bsBDiagonal','bcCross','bsDiagCross');
var
  k, X, Y, H, W: Integer;
  R: TRect;
begin
  X:= 10;
  Y:= 10;
  with pbOut.Canvas do
  begin
    Brush.Color:= clWhite;
    Brush.Style:= bsSolid;
    FillRect(ClipRect);
    W:= ClipRect.Right div 2 - 90;
    H:= ClipRect.Bottom div 4 - 10;
    for k:= 0 to 7 do
    begin            
      if k=4 then
      begin
        X:= W+90;
        Y:= 5;
      end;
      Brush.Style:= TBrushStyle(k);
      if k<>1 then
        Brush.Color:= cbBrushColor.Colors[cbBrushColor.ItemIndex];
      R.Top:= Y;
      inc(Y, H);
      R.Bottom:= R.Top + H;
      R.Left:= X+70;
      R.Right:= R.Left + W;
      FillRect(R);
      Brush.Color:= clWhite;
      TextOut(X, R.Top+5, Names[k]);
      R.Top:= R.Top+H;
    end;
  end;
end;

procedure TForm1.bb1Click(Sender: TObject);
begin
  with pbBit.Canvas, Brush do
  begin
    Bitmap:= TBitmap.Create;   //создаем объект-изображение
    try
      if dlgOpenPic1.Execute then
        Bitmap.LoadFromFile(dlgOpenPic1.FileName);
      FillRect(ClipRect); // заполняем весь прямоугольник
    finally
      Bitmap.Free;
    end;
  end;
end;

end.
