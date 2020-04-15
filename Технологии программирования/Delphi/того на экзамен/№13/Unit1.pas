unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, Buttons, StdCtrls, ExtCtrls;

type
  TForm1 = class(TForm)
    pbPen: TPaintBox;
    pbOut: TPaintBox;
    pnl1: TPanel;
    cbPen: TColorBox;
    cbBack: TColorBox;
    cbMode: TComboBox;
    bbPen: TButton;
    lbl1: TLabel;
    btn1: TBitBtn;
    btn2: TBitBtn;
    lbl2: TLabel;
    lbl3: TLabel;
    lbl4: TLabel;
    procedure btn2Click(Sender: TObject);
    procedure bbPenClick(Sender: TObject);
    procedure FormCreate(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

{$R *.dfm}

procedure TForm1.btn2Click(Sender: TObject);
var
  k: Integer;
const
  Names: array [0..6] of string[30] = ('psSolid','psDash','psDot','psDashDot',
    'psDashDotDot','psClear','psInsideFrame');
begin
  with pbOut.Canvas, Pen do
    for k:= 0 to 6 do
    begin
      Style:= TPenStyle(k);
      MoveTo(10, k*25+10);
      LineTo(350, k*25+10);
      TextOut(360, k*25+5, Names[k]);
    end;
  lbl1.Caption:= 'Выберите цвет фона и линии, режим Mode и щелкните по кнопке "Линия"';
end;

const
  PenMode: array [0..15] of string[13] = ('pmBlack','pmWhite','pmNop','pmNot',
    'pmCopy','pmNotCopy','pmMergePenNot','pmMaskPenNot','pmMergeNotPen','pmMaskNotPen',
    'pmMerge','pmNotMerge','pmMask','pmNotMask','pmXOR','pmNotWor');

procedure TForm1.bbPenClick(Sender: TObject);
var
  X, Y, L: Integer;
begin
  with pbPen.Canvas do
  begin
    Pen.Width:= 5;
    //Закрашиваем фон
    Brush.Color:= cbBack.Colors[cbBack.ItemIndex];
    FillRect(ClipRect);
    //Устанавливаем цвет линии и режим
    Pen.Color:= cbPen.Colors[cbPen.ItemIndex];
    Pen.Mode:= TPenMode(cbMode.ItemIndex);
    //Центрируем линию в поле pbPen и выводим ее
    X:= 10;
    Y:= ClipRect.Bottom div 2;
    L:= ClipRect.Right - 10;
    MoveTo(X,Y);
    LineTo(L,Y);
  end;
end;

procedure TForm1.FormCreate(Sender: TObject);
var
  k: Integer;
begin
  cbMode.Items.Clear;
  for k:= 0 to 15 do
    cbMode.Items.Add(PenMode[k])
end;

end.
