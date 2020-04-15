unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, TeEngine, Series, ExtCtrls, TeeProcs, Chart;

type
  TForm1 = class(TForm)
    Chart1: TChart;
    Series1: TLineSeries;
    Series2: TLineSeries;
    Series3: TLineSeries;
    Edit1: TEdit;
    Edit2: TEdit;
    Edit3: TEdit;
    Edit4: TEdit;
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    Label4: TLabel;
    Edit5: TEdit;
    Edit6: TEdit;
    Label5: TLabel;
    Button1: TButton;
    Button2: TButton;
    procedure FormCreate(Sender: TObject);
    procedure Button1Click(Sender: TObject);
    procedure Button2Click(Sender: TObject);
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
 Edit1.Text:='0';
 Edit2.Text:='-1';
 Edit3.Text:='5';
 Edit4.Text:='1';
 Edit5.Text:='1';
 Edit6.Text:='1';
end;

procedure TForm1.Button1Click(Sender: TObject);
var YMin,YMax,XMin,XMax,Hx,Hy:real;
begin
  XMin:=StrToFloat(Edit1.Text);
  YMin:=StrToFloat(Edit2.Text);
  XMax:=StrToFloat(Edit3.Text);
  YMax:=StrToFloat(Edit4.Text);
  Hx:=StrToFloat(Edit5.Text);
  Hy:=StrToFloat(Edit6.Text);

  Chart1.LeftAxis.SetMinMax(YMin,YMax);
  Chart1.LeftAxis.Increment:=Hy;

  chart1.BottomAxis.SetMinMax(XMin,XMax);
  Chart1.BottomAxis.Increment:=Hx;

end;

procedure TForm1.Button2Click(Sender: TObject);
var x,XMin,XMax,h:real;
begin
 Series1.Clear;
 Series2.Clear;
 Series3.Clear;
 Series1.LinePen.Width:=2;
 Series2.LinePen.Width:=2;
 Series3.LinePen.Width:=2;
 XMin:=StrToFloat(Edit1.Text);
 XMax:=StrToFloat(Edit3.Text);
 x:=XMin;
 h:=0.01;
 while x<XMax do
  begin
    Series1.AddXY(x,sin(x),'',clTeeColor);
    Series2.AddXY(x,cos(x),'',clTeeColor);
    Series3.AddXY(x,sin(x)*cos(x),'',clTeeColor);
    x:=x+h;
  end;
end;

end.
