unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    Label2: TLabel;
    Edit1: TEdit;
    Edit2: TEdit;
    RadioGroup1: TRadioGroup;
    RadioButton1: TRadioButton;
    RadioButton2: TRadioButton;
    RadioButton3: TRadioButton;
    Memo1: TMemo;
    Button1: TButton;
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
 Memo1.clear;
 Edit1.Text:='0';
 Edit2.Text:='2';
 RadioButton3.Checked:=true;
end;

procedure TForm1.Button1Click(Sender: TObject);
var y,x:real;
    a,b,eps,h:real;
    n,i,j:integer;
begin
  a:=StrToFloat(Edit1.Text);
  b:=StrToFloat(Edit2.text);
  n:=10;
  h:=(b-a)/n;
  x:=a;
  while (x<b-h) do
   begin
    if (RadioButton1.Checked) then
     begin
      y:=x+x*x*x;
      x:=x+h;
     end;
    if (RadioButton2.Checked) then
     begin
      y:=exp(x)/sin(x);
      x:=x+h;
     end;
    if (RadioButton3.Checked) then
     begin
      y:=x*x-cos(x);
      x:=x+h;
     end;
   end;
   //y:=y*h;
   Memo1.Lines.Add(FloatToStrF(y,fffixed,8,3));
end;

end.
