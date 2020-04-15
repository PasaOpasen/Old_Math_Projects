unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    Edit1: TEdit;
    Edit2: TEdit;
    Edit3: TEdit;
    RadioGroup1: TRadioGroup;
    Button1: TButton;
    RadioButton1: TRadioButton;
    RadioButton2: TRadioButton;
    RadioButton3: TRadioButton;
    RadioButton4: TRadioButton;
    Memo1: TMemo;
    Image1: TImage;
    Edit4: TEdit;
    Edit5: TEdit;
    Edit6: TEdit;
    Label4: TLabel;
    Label5: TLabel;
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
Edit1.Text:='1';
Edit2.Text:='0';
Edit3.Text:='1';
Edit4.Text:='1';
Edit5.Text:='1';
Edit6.Text:='0';
image1.Hide;
Memo1.Clear;
RadioButton1.Checked:=true;
end;

procedure TForm1.Button1Click(Sender: TObject);
var a,b,c,p,h,m,l,mashtab,max:real;
    x1,x2,x3,y1,y2,y3:integer;
begin
  image1.show;
  image1.Canvas.Pen.Color:=clwhite;
  image1.Canvas.Rectangle(0,0,image1.Width,image1.Height);
  image1.Canvas.Pen.Color:=clblack;
  x1:=StrToInt(Edit1.Text);
  x2:=StrToInt(Edit2.Text);
  x3:=StrToInt(Edit3.Text);
  y1:=StrToInt(Edit4.Text);
  y2:=StrToInt(Edit5.Text);
  y3:=StrToInt(Edit6.Text);

  a:=sqrt(sqr(x2-x1)+sqr(y2-y1));
  b:=sqrt(sqr(x3-x2)+sqr(y3-y2));
  c:=sqrt(sqr(x1-x3)+sqr(y1-y3));

  max:=abs(x1);
  if max<abs(x2) then max:=x2;
  if max<abs(x3) then max:=x3;
  if max<abs(y1) then max:=y1;
  if max<abs(y2) then max:=y2;
  if max<abs(y3) then max:=y3;

  mashtab:=(image1.Height-3)/max;

  image1.Canvas.Pen.Width:=3;
  x1:=round(x1*mashtab);
  x2:=round(x2*mashtab);
  x3:=round(x3*mashtab);
  y1:=round(y1*mashtab);
  y2:=round(y2*mashtab);
  y3:=round(y3*mashtab);

  image1.canvas.MoveTo(x1,y1);
  image1.Canvas.LineTo(x2,y2);
  image1.Canvas.LineTo(x3,y3);
  image1.Canvas.LineTo(x1,y1);
  if RadioButton1.Checked then
    begin
     p:=a+b+c;
     Memo1.Lines.Add('Пириметр равен '+FloatToStrF(p,fffixed,8,3));

    end;
  if RadioButton2.Checked then
   begin
    p:=(a+b+c)/2;
    h:=(2/a)*sqrt(p*(p-a)*(p-b)*(p-c));
    image1.Canvas.MoveTo(x1,y1);
    image1.Canvas.LineTo(round((x3+x2)/2),round((y3+y2)/2));
    image1.Canvas.MoveTo(x2,y2);
    image1.Canvas.LineTo(round((x1+x3)/2),round((y1+y3)/2));
    image1.Canvas.MoveTo(x3,y3);
    image1.Canvas.LineTo(round((x1+x2)/2),round((y1+y2)/2));

    Memo1.Lines.Add('Длина высоты '+FloatToStrF(h,fffixed,8,3));
   end;
  if RadioButton3.Checked then
   begin
    m:=0.5*sqrt(2*a*a+2*b*b-c*c);
    Memo1.Lines.Add('Длина медианы '+FloatToStrF(m,fffixed,8,3));
    image1.Canvas.MoveTo(x1,y1);
    image1.Canvas.LineTo(round((x3+x2)/2),round((y3+y2)/2));
    image1.Canvas.MoveTo(x2,y2);
    image1.Canvas.LineTo(round((x1+x3)/2),round((y1+y3)/2));
    image1.Canvas.MoveTo(x3,y3);
    image1.Canvas.LineTo(round((x1+x2)/2),round((y1+y2)/2));

   end;
  if RadioButton4.Checked then
   begin
    l:=sqrt(a*b*(a+b+c)*(a+b-c))/(a+b);
    Memo1.Lines.Add('Длина биссектрис '+FloatToStrF(l,fffixed,8,3));
    image1.Canvas.MoveTo(x1,y1);
    image1.Canvas.LineTo(round((x3+x2)/2),round((y3+y2)/2));
    image1.Canvas.MoveTo(x2,y2);
    image1.Canvas.LineTo(round((x1+x3)/2),round((y1+y3)/2));
    image1.Canvas.MoveTo(x3,y3);
    image1.Canvas.LineTo(round((x1+x2)/2),round((y1+y2)/2));
   end;
end;

end.
