unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, TeEngine, Series, ExtCtrls, TeeProcs, Chart;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    Label4: TLabel;
    Edit1: TEdit;
    Edit2: TEdit;
    Edit3: TEdit;
    Edit4: TEdit;
    Button1: TButton;
    Memo1: TMemo;
    Chart1: TChart;
    Series1: TLineSeries;
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
 Edit1.Text:='0';
 Edit2.Text:='0';
 Edit3.Text:='0';
 Edit4.Text:='0';
 Memo1.Clear;
end;

procedure TForm1.Button1Click(Sender: TObject);
var x1,y1,x2,y2,rav:real;
begin
 try
  x1:=StrToFloat(Edit1.Text);
  y1:=StrToFloat(Edit2.Text);
  x2:=StrToFloat(Edit3.Text);
  y2:=StrToFloat(Edit4.Text);
 except
  ShowMessage('Неправильный ввод');
  exit;
 end;
 series1.Clear;
 series1.AddXY(x1,y1,'',clRed);
 series1.AddXY(x2,y2,'',clRed);

 rav:=sqrt((x2-x1)*(x2-x1)+(y2-y1)*(y2-y1));
 Memo1.Lines.Add(FloatToStrF(rav,fffixed,6,2));
end;

end.
