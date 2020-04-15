unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls, TeEngine, Series, TeeProcs, Chart;

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
    Chart1: TChart;
    Series1: TLineSeries;
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

function f(x:real; Index:integer):real;
 begin
  case index of
   0: f:=x*x+3;
   1: f:=(x*x*x)-(1/x);
   2: f:=x/sin(x);
  end;
 end;

procedure TForm1.Button1Click(Sender: TObject);
var a,b,y,h,x:real;
    n,i,index:integer;

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
 index:=RadioGroup1.ItemIndex;
 series1.Clear;
 chart1.BottomAxis.SetMinMax(a,b);
 Chart1.LeftAxis.SetMinMax(-100,100);

 x:=a;
 while (x<b) do
  begin
   if (index=1)and (x>-0.005) and (x<0.005) then series1.AddNullXY(x,f(x,index),'')
   else series1.AddXY(x,f(x,index),'',clRed);
   if (index=2) and (sin(x)>-0.005) and (sin(x)<0.005) then series1.AddNullXY(x,f(x,index),'')
   else series1.AddXY(x,f(x,index),'',clRed);
   x:=x+0.001;
  end;

 h:=(b-a)/n;
 y:=0;
 y:=h*(f(a,index)+f(b,index))/2;
 for i:=1 to n-1 do y:=y+f(a+i*h,index)*h;
 Memo1.Lines.Add(FloatToStr(y));
end;

end.
