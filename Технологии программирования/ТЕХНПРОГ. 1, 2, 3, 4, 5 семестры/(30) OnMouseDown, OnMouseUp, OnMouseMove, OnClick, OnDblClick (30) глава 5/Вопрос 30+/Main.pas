unit Main;

interface

uses
  Windows, Messages, SysUtils, Classes, Graphics, Controls, Forms, Dialogs,
  ExtCtrls, ComCtrls;

type
  TMainForm = class(TForm)
    ColorDlg: TColorDialog;
    StatusBar: TStatusBar;
    Timer: TTimer;
    procedure FormMouseDown(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure FormMouseUp(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure FormMouseMove(Sender: TObject; Shift: TShiftState; X,
      Y: Integer);
    procedure TimerTimer(Sender: TObject);
  private
    MouseRect: TRect;
    IsDown: Boolean;
    RectColor: TColor;
  public
    { Public declarations }
  end;

var
  MainForm: TMainForm;

implementation

{$R *.DFM}

procedure TMainForm.FormMouseDown(Sender: TObject; Button: TMouseButton;
  Shift: TShiftState; X, Y: Integer);
begin
 if Button = mbLeft then
 with MouseRect do
 begin
  IsDown := True;
  Left := X;
  Top := Y;
  Right := X;
  Bottom := Y;
  Canvas.Pen.Color := RectColor;
 end;
 if (Button = mbRight) and ColorDlg.Execute
 then RectColor := ColorDlg.Color;
end;

procedure TMainForm.FormMouseUp(Sender: TObject; Button: TMouseButton;
  Shift: TShiftState; X, Y: Integer);
begin
 IsDown := False;
 Canvas.Pen.Color := Color;
 with MouseRect do
 Canvas.Polyline([Point(Left, Top), Point(Right, Top), Point(Right, Bottom), Point(Left, Bottom), Point(Left, Top)]);
 with StatusBar do
 begin
  Panels[4].Text := '';
  Panels[5].Text := '';
 end;
end;

procedure TMainForm.FormMouseMove(Sender: TObject; Shift: TShiftState; X,
  Y: Integer);
begin
 with StatusBar do
 begin
  Panels[2].Text := 'X: ' + IntToStr(X);
  Panels[3].Text := 'Y: ' + IntToStr(Y);
 end;
 if Not IsDown then Exit;
 Canvas.Pen.Color := Color;
 with mouserect do
 begin
  Canvas.Polyline([Point(Left, Top), Point(Right, Top), Point(Right, Bottom), Point(Left, Bottom), Point(Left, Top)]);
  Right := X;
  Bottom := Y;
  Canvas.Pen.Color := RectColor;
  Canvas.Polyline([Point(Left, Top), Point(Right, Top), Point(Right, Bottom), Point(Left, Bottom), Point(Left, Top)]);
 end;
 with StatusBar do
 begin
  Panels[4].Text := 'Ширина: ' + IntToStr(Abs(MouseRect.Right - MouseRect.Left));
  Panels[5].Text := 'Высота: ' + IntToStr(Abs(MouseRect.Bottom - MouseRect.Top));
 end;
end;

procedure TMainForm.TimerTimer(Sender: TObject);
begin
 with StatusBar do
 begin
  Panels[0].Text := 'Дата: ' + DateToStr(Now);
  Panels[1].Text := 'Время: ' + TimeToStr(Now);
 end;
end;

end.
