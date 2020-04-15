unit GraphWin;

interface

uses
  SysUtils, Windows, Messages, Classes, Graphics, Controls, Forms, Dialogs,
  Buttons, ExtCtrls, StdCtrls, ComCtrls, Menus, ToolWin, ExtDlgs, ImgList;

type
  TDrawingTool = (dtLine, dtRectangle, dtEllipse, dtRoundRect, dtBezier);
  TForm1 = class(TForm)
    StatusBar1: TStatusBar;
    ScrollBox1: TScrollBox;
    Image: TImage;
    ColorDialog1: TColorDialog;
    MainMenu1: TMainMenu;
    File1: TMenuItem;
    New1: TMenuItem;
    Open1: TMenuItem;
    Save1: TMenuItem;
    Saveas1: TMenuItem;
    Print1: TMenuItem;
    N1: TMenuItem;
    Exit1: TMenuItem;
    Edit1: TMenuItem;
    Cut1: TMenuItem;
    Copy1: TMenuItem;
    Paste1: TMenuItem;
    InstrImageList: TImageList;
    PenImageList: TImageList;
    BrushImageList: TImageList;
    PenMenu: TPopupMenu;
    BrushMenu: TPopupMenu;
    SolidItem: TMenuItem;
    DashItem: TMenuItem;
    DotItem: TMenuItem;
    DashDotItem: TMenuItem;
    DashDotDotItem: TMenuItem;
    ClearItem: TMenuItem;
    SolidBrushItem: TMenuItem;
    ClearBrushItem: TMenuItem;
    HorizBrushItem: TMenuItem;
    VertBrushItem: TMenuItem;
    FDiagBrushItem: TMenuItem;
    BDiagBrushItem: TMenuItem;
    CrossBrushItem: TMenuItem;
    DiagCrossBrushItem: TMenuItem;
    OpenPictureDialog1: TOpenPictureDialog;
    SavePictureDialog1: TSavePictureDialog;
    ControlBar1: TControlBar;
    InstrToolBar: TToolBar;
    LineToolButton: TToolButton;
    RectangleButton: TToolButton;
    EllipseButton: TToolButton;
    RoundRectButton: TToolButton;
    BezierButton: TToolButton;
    ToolButton1: TToolButton;
    PenButton: TToolButton;
    BrushButton: TToolButton;
    PenToolBar: TToolBar;
    ClearPen: TToolButton;
    SolidPen: TToolButton;
    DashPen: TToolButton;
    PenColor: TToolButton;
    DotPen: TToolButton;
    DashDotPen: TToolButton;
    PenSize: TEdit;
    PenWidth: TUpDown;
    DashDotDotPen: TToolButton;
    BrushToolBar: TToolBar;
    SolidBrush: TToolButton;
    BrushColor: TToolButton;
    ToolButton6: TToolButton;
    ClearBrush: TToolButton;
    BDiagonalBrush: TToolButton;
    HorizontalBrush: TToolButton;
    CrossBrush: TToolButton;
    VerticalBrush: TToolButton;
    DiagCrossBrush: TToolButton;
    FDiagonalBrush: TToolButton;
    ToolButton2: TToolButton;
    procedure FormMouseDown(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure FormMouseUp(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure FormMouseMove(Sender: TObject; Shift: TShiftState; X,
      Y: Integer);
    procedure LineButtonClick(Sender: TObject);
    procedure RectangleButtonClick(Sender: TObject);
    procedure EllipseButtonClick(Sender: TObject);
    procedure RoundRectButtonClick(Sender: TObject);
    procedure SetPenStyle(Sender: TObject);
    procedure PenSizeChange(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure SetBrushStyle(Sender: TObject);
    procedure PenColorClick(Sender: TObject);
    procedure BrushColorClick(Sender: TObject);
    procedure Exit1Click(Sender: TObject);
    procedure Open1Click(Sender: TObject);
    procedure Save1Click(Sender: TObject);
    procedure Saveas1Click(Sender: TObject);
    procedure New1Click(Sender: TObject);
    procedure Copy1Click(Sender: TObject);
    procedure Cut1Click(Sender: TObject);
    procedure Paste1Click(Sender: TObject);
    procedure BezierButtonClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
    BrushStyle: TBrushStyle;
    PenStyle: TPenStyle;
    PenWide: Integer;
    Drawing: Boolean;
    Origin, MovePt: TPoint;
    DrawingTool: TDrawingTool;
    CurrentFile: string;
    procedure SaveStyles;
    procedure RestoreStyles;
    procedure DrawShape(TopLeft, BottomRight: TPoint; AMode: TPenMode);
  end;

var
  Form1: TForm1;

implementation

uses BMPDlg, Clipbrd, jpeg;

{$R *.DFM}

procedure TForm1.FormMouseDown(Sender: TObject; Button: TMouseButton;
  Shift: TShiftState; X, Y: Integer);
begin
  Drawing := True;
  Image.Canvas.MoveTo(X, Y);
  Origin := Point(X, Y);
  MovePt := Origin;
  StatusBar1.Panels[0].Text := Format('Origin: (%d, %d)', [X, Y]);
end;

procedure TForm1.FormMouseUp(Sender: TObject; Button: TMouseButton;
  Shift: TShiftState; X, Y: Integer);
begin
  if Drawing then
  begin
    DrawShape(Origin, Point(X, Y), pmCopy);
    Drawing := False;
  end;
end;

procedure TForm1.FormMouseMove(Sender: TObject; Shift: TShiftState; X,
  Y: Integer);
begin
  if Drawing then
  begin
    DrawShape(Origin, MovePt, pmNotXor);
    MovePt := Point(X, Y);
    DrawShape(Origin, MovePt, pmNotXor);
  end;
  StatusBar1.Panels[1].Text := Format('Current: (%d, %d)', [X, Y]);
end;

procedure TForm1.LineButtonClick(Sender: TObject);
begin
  DrawingTool := dtLine;
end;

procedure TForm1.RectangleButtonClick(Sender: TObject);
begin
  DrawingTool := dtRectangle;
end;

procedure TForm1.EllipseButtonClick(Sender: TObject);
begin
  DrawingTool := dtEllipse;
end;

procedure TForm1.RoundRectButtonClick(Sender: TObject);
begin
  DrawingTool := dtRoundRect;
end;

procedure TForm1.DrawShape(TopLeft, BottomRight: TPoint; AMode: TPenMode);
begin
  with Image.Canvas do
  begin
    Pen.Mode := AMode;
    case DrawingTool of
      dtLine:
        begin
          Image.Canvas.MoveTo(TopLeft.X, TopLeft.Y);
          Image.Canvas.LineTo(BottomRight.X, BottomRight.Y);
        end;
      dtRectangle: Image.Canvas.Rectangle(TopLeft.X, TopLeft.Y, BottomRight.X,
        BottomRight.Y);
      dtEllipse: Image.Canvas.Ellipse(Topleft.X, TopLeft.Y, BottomRight.X,
        BottomRight.Y);
      dtRoundRect: Image.Canvas.RoundRect(TopLeft.X, TopLeft.Y, BottomRight.X,
        BottomRight.Y, (TopLeft.X - BottomRight.X) div 2,
        (TopLeft.Y - BottomRight.Y) div 2);
      dtBezier: Image.Canvas.PolyBezier([TopLeft,Point(TopLeft.X,BottomRight.Y),
      Point(BottomRight.X,TopLeft.Y),BottomRight]);
    end;
  end;
end;

procedure TForm1.SetPenStyle(Sender: TObject);
var i:Integer;
begin
 with Image.Canvas.Pen do
   Style := TPenStyle((Sender as TComponent).Tag - 1);
 if Sender is TMenuItem then with PenToolBar do
   begin
  (Sender as TMenuItem).Checked := True;
   for i:=0 to ButtonCount-1 do
    with Buttons[i] do
    if Tag = (Sender as TComponent).Tag
      then Down := True
       else if (Style=tbsCheck) and Grouped then Down := False;
   end
 else
  PenMenu.Items[(Sender as TComponent).Tag-1].Checked:=True;
end;

procedure TForm1.PenSizeChange(Sender: TObject);
begin
  Image.Canvas.Pen.Width := PenWidth.Position;
end;

procedure TForm1.FormCreate(Sender: TObject);
var
  Bitmap: TBitmap;
begin
  Bitmap := TBitmap.Create;
  Bitmap.Width := 200;
  Bitmap.Height := 200;
  Image.Picture.Graphic := Bitmap;
end;

procedure TForm1.SetBrushStyle(Sender: TObject);
 var i:Integer;
begin
 with Image.Canvas.Brush do
   Style := TBrushStyle((Sender as TComponent).Tag - 1);
 if Sender is TMenuItem then
  begin
  (Sender as TMenuItem).Checked := True;
   with BrushToolBar do
   for i:=0 to ButtonCount-1 do
    with Buttons[i] do
    if Tag = (Sender as TComponent).Tag
      then Down := True
       else if (Style=tbsCheck) and Grouped then Down := False;
  end;
end;

procedure TForm1.PenColorClick(Sender: TObject);
begin
  ColorDialog1.Color := Image.Canvas.Pen.Color;
  if ColorDialog1.Execute then
    Image.Canvas.Pen.Color := ColorDialog1.Color;
end;

procedure TForm1.BrushColorClick(Sender: TObject);
begin
  ColorDialog1.Color := Image.Canvas.Brush.Color;
  if ColorDialog1.Execute then
    Image.Canvas.Brush.Color := ColorDialog1.Color;
end;

procedure TForm1.Exit1Click(Sender: TObject);
begin
  Close;
end;

procedure TForm1.Open1Click(Sender: TObject);
begin
  if OpenPictureDialog1.Execute then
  begin
    CurrentFile := OpenPictureDialog1.FileName;
    SaveStyles;
    Image.Picture.LoadFromFile(CurrentFile);
    RestoreStyles;
  end;
end;

procedure TForm1.Save1Click(Sender: TObject);
begin
  if CurrentFile <> EmptyStr then
    Image.Picture.SaveToFile(CurrentFile)
  else SaveAs1Click(Sender);
end;

procedure TForm1.Saveas1Click(Sender: TObject);
begin
  if SavePictureDialog1.Execute then
  begin
    CurrentFile := SavePictureDialog1.FileName;
    Save1Click(Sender);
  end;
end;

procedure TForm1.New1Click(Sender: TObject);
var
  Bitmap: TBitmap;
begin
  with NewBMPForm do
  begin
    ActiveControl := WidthEdit;
    WidthEdit.Text := IntToStr(Image.Picture.Graphic.Width);
    HeightEdit.Text := IntToStr(Image.Picture.Graphic.Height);
    if ShowModal <> idCancel then
    begin
      Bitmap := TBitmap.Create;
      Bitmap.Width := StrToInt(WidthEdit.Text);
      Bitmap.Height := StrToInt(HeightEdit.Text);
      SaveStyles;
      Image.Picture.Graphic := Bitmap;
      RestoreStyles;
      CurrentFile := EmptyStr;
    end;
  end;
end;

procedure TForm1.Copy1Click(Sender: TObject);
begin
  Clipboard.Assign(Image.Picture);
end;

procedure TForm1.Cut1Click(Sender: TObject);
var
  ARect: TRect;
begin
  Copy1Click(Sender);
  with Image.Canvas do
  begin
    CopyMode := cmWhiteness;
    ARect := Rect(0, 0, Image.Width, Image.Height);
    CopyRect(ARect, Image.Canvas, ARect);
    CopyMode := cmSrcCopy;
  end;
end;

procedure TForm1.Paste1Click(Sender: TObject);
var
  Bitmap: TBitmap;
begin
  if Clipboard.HasFormat(CF_BITMAP) then
  begin
    Bitmap := TBitmap.Create;
    try
      Bitmap.Assign(Clipboard);
      Image.Canvas.Draw(0, 0, Bitmap);
    finally
      Bitmap.Free;
    end;
  end;
end;

procedure TForm1.SaveStyles;
begin
  with Image.Canvas do
  begin
    BrushStyle := Brush.Style;
    PenStyle := Pen.Style;
    PenWide := Pen.Width;
  end;
end;

procedure TForm1.RestoreStyles;
begin
  with Image.Canvas do
  begin
    Brush.Style := BrushStyle;
    Pen.Style := PenStyle;
    Pen.Width := PenWide;
  end;
end;

procedure TForm1.BezierButtonClick(Sender: TObject);
begin
  DrawingTool := dtBezier;
end;

end.
