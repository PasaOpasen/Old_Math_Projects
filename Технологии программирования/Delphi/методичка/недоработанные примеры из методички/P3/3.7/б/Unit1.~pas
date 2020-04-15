unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls, TeEngine, Series, TeeProcs, Chart, TeeFunci;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    Edit1: TEdit;
    Label2: TLabel;
    Edit2: TEdit;
    Label3: TLabel;
    Edit3: TEdit;
    RadioGroup1: TRadioGroup;
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
  index: integer;

implementation

{$R *.dfm}

procedure TForm1.FormCreate(Sender: TObject);
begin

 edit1.Text:='-10';
 edit2.Text:='10';
 edit3.Text:='100';
end;

function f(x:real):real;
 begin
  case index of
   0: f:=x*x+3;
   1: f:=(x*x*x)-(1/x);
   2: f:=x/sin(x);
  end;
 end;

procedure TForm1.Button1Click(Sender: TObject);
var a,b,y,h,x,eps,mx,my:real;
    n,i:integer;
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
 h:=(b-a)/n;
 y:=0;
 index:=radioGroup1.ItemIndex;

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
 for i:=1 to n do
  begin
   x:=x+h;
   y:=y+f(x+h)*h;
  end;
 Memo1.Lines.Add(FloatToStr(y));
end;

end.
