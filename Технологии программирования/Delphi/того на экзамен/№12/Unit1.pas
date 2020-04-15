unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, Buttons, ExtCtrls;

type
  TForm1 = class(TForm)
    pb1: TPaintBox;
    pnl1: TPanel;
    btn1: TBitBtn;
    btn2: TBitBtn;
    lbl1: TLabel;
    btn3: TButton;
    btn4: TButton;
    edt1: TEdit;
    dlgFont1: TFontDialog;
    procedure btn3Click(Sender: TObject);
    procedure btn1Click(Sender: TObject);
    procedure btn4Click(Sender: TObject);
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
begin
  if dlgFont1.Execute then
    pb1.Canvas.Font:= dlgFont1.Font;
end;

procedure TForm1.btn1Click(Sender: TObject);
begin
  with pb1.Canvas, Font do
  begin
    TextOut(10,10,'MS Sans Serif, Size=10'); //выводит в pb1 строку
    Size:= 20;
    TextOut(10,30,'MS Sans Serif, Size=20');
    Name:= 'Courier';
    Style:= [fsBold];
    TextOut(10,70, 'Courier, Size=20');
    Name:= 'Times New Roman';
    Style:= [fsItalic, fsUnderline, fsBold];
    TextOut(10, 110, 'Times New Roman, курсив, жирный, ' + 'подеркн.');
  end;
  edt1.Show;
  btn3.Show;
  btn4.Show;
  btn1.Hide;
  lbl1.Caption:= 'Выберте шрифт, введите текст и щелкните по кнопке "Вывод"!';
end;

procedure TForm1.btn4Click(Sender: TObject);
begin
  if edt1.Text <> '' then
    pb1.Canvas.TextOut(10, 150, edt1.Text);
  edt1.Text:= '';
  edt1.SetFocus;
end;

end.
