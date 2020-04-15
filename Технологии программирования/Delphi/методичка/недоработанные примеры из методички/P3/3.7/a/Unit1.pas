unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    Edit1: TEdit;
    Label2: TLabel;
    Edit2: TEdit;
    Label3: TLabel;
    Edit3: TEdit;
    RadioGroup1: TRadioGroup;
    RadioButton1: TRadioButton;
    RadioButton2: TRadioButton;
    RadioButton3: TRadioButton;
    Memo1: TMemo;
    Button1: TButton;
    PaintBox1: TPaintBox;
    procedure FormCreate(Sender: TObject);
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

procedure TForm1.FormCreate(Sender: TObject);
begin
 RadioButton1.Checked:=true;
end;


procedure TForm1.Button1Click(Sender: TObject);
var a,b,y,h,x:real;
    n,i:integer;

function f(x:real):real;
 begin
  if RadioButton1.Checked then f:=x*x+3;

  if RadioButton2.Checked then
   if x=0 then
    begin
     ShowMessage('Невозможно деление на ноль');
     f:=0;
     exit;
    end
   else f:=x*x*x-1/x;

  if RadioButton3.Checked then
   if sin(x)=0 then
    begin
     ShowMessage('Невозможно деление на ноль');
     f:=0;
     exit;
    end
   else f:=x/sin(x);
 end;

begin
 try a:=StrToFloat(Edit1.text)
  except ShowMessage('Ошибка ввода числа A');
        exit;
 end;
 try b:=StrToFloat(Edit2.text)
  except ShowMessage('Ошибка ввода числа B');
        exit;
 end;
 try n:=StrToInt(Edit3.text)
  except ShowMessage('Ошибка ввода числа N');
        exit;
 end;
 x:=a;
 PaintBox1.Canvas.Brush.Color:=clWhite;
 PaintBox1.Canvas.FillRect(PaintBox1.ClientRect);
 PaintBox1.Canvas.Brush.Color:=clBlack;
 PaintBox1.Canvas.MoveTo((PaintBox1.Width div 2)+round(x*20),(PaintBox1.Height div 2)-round(f(x)*10));
 while (x<b) do
  begin
   if (sin(x)<-0.01)or (sin(x)>0.01) then PaintBox1.Canvas.LineTo((PaintBox1.Width div 2)+round(x*20),(PaintBox1.Height div 2)-round(f(x)*10))
   else  PaintBox1.Canvas.MoveTo((PaintBox1.Width div 2)+round(x*20),(PaintBox1.Height div 2)-round(f(x)*10));
   if (x<-0.01)or (x>0.01) then PaintBox1.Canvas.LineTo((PaintBox1.Width div 2)+round(x*20),(PaintBox1.Height div 2)-round(f(x)*10))
   else  PaintBox1.Canvas.MoveTo((PaintBox1.Width div 2)+round(x*20),(PaintBox1.Height div 2)-round(f(x)*10));
   x:=x+0.001;
  end;
 x:=a;
 h:=(b-a)/n;
 y:=0;
 for i:=1 to n-1 do y:=y+f(a+i*h)*h;
 Memo1.Lines.Add(FloatToStr(y));
end;

end.
