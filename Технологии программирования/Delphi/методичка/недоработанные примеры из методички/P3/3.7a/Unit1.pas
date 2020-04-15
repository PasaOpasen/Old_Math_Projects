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
    Label3: TLabel;
    Edit3: TEdit;
    Button1: TButton;
    Memo1: TMemo;
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
Memo1.Clear;
Edit1.Text:='0';
Edit2.Text:='1';
Edit3.Text:='10';
RadioButton1.Checked:=true;
end;

procedure TForm1.Button1Click(Sender: TObject);
var a,b,y,x,h:real;
    i,j,n:integer;
    temp:boolean;
begin
a:=StrToFloat(Edit1.Text);
b:=StrToFloat(Edit2.Text);
n:=StrToInt(Edit3.Text);
h:=(b-a)/n;
x:=a;
temp:=false;
for i:=0 to n-1 do
 begin
  if(RadioButton1.Checked) then
    y:=x*x+3;

  if(RadioButton2.Checked) then
   begin
    if (x=0) or (x+h=0) then temp:=true
    else y:=x/sin(x);
   end;

  if(RadioButton3.Checked) then
   begin
    if (x=0) or (x+h=0) then temp:=true
    else y:=x*x*x-1/x;
   end;

  if temp then break;
  x:=x+h;
 end;

 if temp then
  begin
   y:=0;
   Memo1.Lines.Add('На ноль делить нельзя y(x)='+FloatToStr(y));
  end
 else
  y:=y*h;
  Memo1.Lines.Add('Значение y`(x)='+FloatToStrF(y,fffixed,8,3));
end;

end.
