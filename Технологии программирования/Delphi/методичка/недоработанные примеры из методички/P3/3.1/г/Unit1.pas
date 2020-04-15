unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, TeEngine, Series, ExtCtrls, TeeProcs, Chart, StdCtrls, Math;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    Label2: TLabel;
    Edit1: TEdit;
    Edit2: TEdit;
    Button1: TButton;
    Memo1: TMemo;
    Chart1: TChart;
    Series1: TLineSeries;
    procedure Button1Click(Sender: TObject);
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

procedure TForm1.Button1Click(Sender: TObject);
var x,y,u:real;
begin
  try
    x:=StrToFloat(Edit1.text);
    y:=StrToFloat(Edit2.Text);
  except
    ShowMessage('Не правильный ввод');
    exit;
  end;

  if arctan(x)=0 then
    begin
      ShowMessage('Делелние на ноль, ArcTg(x) равен 0');
      exit;
    end;

  u:=sqr(cos(x))+abs(y)/arctan(x);
  Memo1.Lines.Add(FloatToStrF(u,fffixed,8,3));
end;

function f(x:real):real;
  begin
    f:=sqr(cos(x))+1/arctan(x);
  end;

procedure TForm1.FormCreate(Sender: TObject);
var x,h:real;
begin
  h:=0.01;
  x:=-10;
  Chart1.LeftAxis.PositionPercent:=50;
  Chart1.LeftAxis.SetMinMax(-4,4);
  Chart1.BottomAxis.PositionPercent:=50;
  while x<10 do
    begin
      if (abs(f(x)-f(x+h)))>1 then
        series1.AddNullXY(x+h,f(x+h),'')
      else
      Series1.AddXY(x,f(x),'',clRed);
      x:=x+h;
    end;
end;

end.
