unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, TeEngine, Series, ExtCtrls, TeeProcs, Chart, StdCtrls;

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
  x:=StrToFloat(Edit1.text);
  y:=StrToFloat(Edit1.Text);
  u:=sin(x*x+y*y)+exp(y-x);
  Memo1.Lines.Add(FloatToStrF(u,fffixed,8,3));
end;

procedure TForm1.FormCreate(Sender: TObject);
var x:real;
begin
  x:=-10;
  Chart1.LeftAxis.PositionPercent:=50;
  Chart1.LeftAxis.SetMinMax(-10,10);
  Chart1.BottomAxis.PositionPercent:=50;
  while x<10 do
    begin
      Series1.AddXY(x,sin(x*x)+exp(-x),'',clRed);
      x:=x+0.01;
    end;
end;

end.
