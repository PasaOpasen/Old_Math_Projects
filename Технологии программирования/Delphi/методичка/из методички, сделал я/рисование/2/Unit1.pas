unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    Button1: TButton;
    PaintBox1: TPaintBox;
    Label2: TLabel;
    Label3: TLabel;
    Label4: TLabel;
    Edit1: TEdit;
    Edit2: TEdit;
    Edit3: TEdit;
    Label5: TLabel;
    Edit4: TEdit;
    Edit5: TEdit;
    RadioGroup1: TRadioGroup;
    Button2: TButton;
    procedure Button1Click(Sender: TObject);
    procedure Button2Click(Sender: TObject);
    procedure RadioGroup1Click(Sender: TObject);
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
var
  a,b,c,ab,ac,bc,aa,bb,cc,x,y,q,w,t: real;
  n,i: integer;
  p:array [0..2] of Tpoint;
begin
  n:= 2;
  PaintBox1.Canvas.Pen.Width:= 2;
  case RadioGroup1.itemindex of

    0: begin
         try
         ab:= StrToInt(Edit1.Text);
         ac:= StrToInt(Edit2.Text);
         bc:= StrToInt(Edit3.Text);
         a:= ab; b:= ac; c:= bc;

         if ((a>0) and (b>0) and (c>0)) and ((a+b>c) and (b+c>a) and (a+c>b)) then
         begin

           x:= ((a*a+b*b-c*c)/(2*a))*50;
           y:= sqrt(b*b*50*50-x*x);

           p[0].x:= 0;            p[0].y:= 0;
           p[1].x:= trunc(a*50);  p[1].y:= 0;
           p[2].x:= trunc(x);     p[2].y:= trunc(y);

           PaintBox1.Canvas.MoveTo(p[2].x+100,p[2].y+50);
           for i:= 0 to n do
             begin
               PaintBox1.Canvas.LineTo(p[i].x+100,p[i].y+50);
             end;

           PaintBox1.Canvas.TextOut(p[0].x+80,p[0].y+40,'A');
           PaintBox1.Canvas.TextOut(p[1].x+110,p[1].y+40,'B');
           PaintBox1.Canvas.TextOut(p[2].x+95,p[2].y+55,'C');
         end
         else
         begin
           ShowMessage('Треугольник с заданными сторонами не может существовать!');
           exit;
         end;
         except
           ShowMessage('Проверьте правильность заданных сторон!');
         end;
       end;

    1: begin
         try
         ab:= StrToInt(Edit1.Text);
         ac:= StrToInt(Edit2.Text);
         bb:= StrToInt(Edit4.Text);
         a:= ab; b:= ac;

         if (a>0) and (b>0) then
         begin
           if (bb<180) and (bb>0) then
           begin
             //найдем 3 сторону по 2 сторонам и углу между ними по теореме косинусов
             q:= a*b*2*cos((bb*pi)/180);
             c:= sqrt(a*a+b*b-q);

             x:= (a*a+b*b-c*c)/(2*a)*50;
             y:= sqrt(b*b*50*50-x*x);

             p[0].x:= 0;             p[0].y:= 0;
             p[1].x:= trunc(a*50);   p[1].y:= 0;
             p[2].x:= trunc(x);      p[2].y:= trunc(y);

             PaintBox1.Canvas.MoveTo(p[2].x+100,p[2].y+50);
             for i:= 0 to n do
               begin
                 PaintBox1.Canvas.LineTo(p[i].x+100,p[i].y+50);
               end;

             PaintBox1.Canvas.TextOut(p[0].x+80,p[0].y+40,'A');
             PaintBox1.Canvas.TextOut(p[1].x+110,p[1].y+40,'B');
             PaintBox1.Canvas.TextOut(p[2].x+95,p[2].y+55,'C');
           end
           else
             ShowMessage('Треугольник с заданными углом не может существовать!');
             exit;
         end
         else
         begin
           ShowMessage('Треугольник с заданными сторонами не может существовать!');
           exit;
         end;
         except
           ShowMessage('Проверьте правильность заданных сторон и угла между ними!');
         end;
       end;

    2: begin
         try
         ab:= StrToInt(Edit1.Text);
         bb:= StrToInt(Edit4.Text);
         aa:= StrToInt(Edit5.Text);
         a:= ab;

         cc:= 180-aa-bb;
         if (a>0) then
         begin
           if (aa<180) and (aa>0) and (bb<180) and (bb>0) and (cc<180) and (cc>0) then
           begin

             q:= sin((aa*pi)/180);
             w:= sin((bb*pi)/180);
             t:= sin((cc*pi)/180);

             b:= q/t*a;
             c:= w/t*a;

             x:= (a*a+b*b-c*c)/(2*a)*50;
             y:= sqrt(b*b*50*50-x*x);

             p[0].x:= 0;             p[0].y:= 0;
             p[1].x:= trunc(a*50);   p[1].y:= 0;
             p[2].x:= trunc(x);      p[2].y:= trunc(y);

             PaintBox1.Canvas.MoveTo(p[2].x+100,p[2].y+50);
             for i:= 0 to n do
               begin
                 PaintBox1.Canvas.LineTo(p[i].x+100,p[i].y+50);
               end;

             PaintBox1.Canvas.TextOut(p[0].x+80,p[0].y+40,'A');
             PaintBox1.Canvas.TextOut(p[1].x+110,p[1].y+40,'B');
             PaintBox1.Canvas.TextOut(p[2].x+95,p[2].y+55,'C');
           end
           else
             ShowMessage('Треугольник с заданными углами не может существовать!');
             exit;
         end
         else
         begin
           ShowMessage('Треугольник с заданной стороной не может существовать!');
           exit;
         end;
         except
           ShowMessage('Проверьте правильность заданной стороны и 2 прилежащим углам!');
         end;
       end;
  end;
end;

procedure TForm1.Button2Click(Sender: TObject);
var
  i,j:integer;
begin
  for i:= 0 to PaintBox1.Height do
    //for j:= 0 to PaintBox1.Width do
    for j:=i to PaintBox1.Height do
    begin
      PaintBox1.Canvas.Pixels[PaintBox1.Height-i,PaintBox1.Height-j]:= RGB(255,255,255);
      PaintBox1.Canvas.Pixels[i,j]:= RGB(255,255,255);
    end;
end;

procedure TForm1.RadioGroup1Click(Sender: TObject);
begin
  Edit1.Text:= '';
  Edit2.Text:= '';
  Edit3.Text:= '';
  Edit4.Text:= '';
  Edit5.Text:= '';

  case RadioGroup1.itemindex of

  0: begin
     Form1.Button1.Show;
     Form1.Label1.Show;
     Form1.Edit1.Show;
     Form1.Label2.Show;
     Form1.Edit2.Show;
     Form1.Label3.Show;
     Form1.Edit3.Show;
     Form1.Label4.Hide;
     Form1.Edit4.Hide;
     Form1.Label5.Hide;
     Form1.Edit5.Hide;
     end;

  1: begin
     Form1.Button1.Show;
     Form1.Label1.Show;
     Form1.Edit1.Show;
     Form1.Label2.Show;
     Form1.Edit2.Show;
     Form1.Label3.Hide;
     Form1.Edit3.Hide;
     Form1.Label4.Show;
     Form1.Edit4.Show;
     Form1.Label5.Hide;
     Form1.Edit5.Hide;
     end;

  2: begin
     Form1.Button1.Show;
     Form1.Label1.Show;
     Form1.Edit1.Show;
     Form1.Label2.Hide;
     Form1.Edit2.Hide;
     Form1.Label3.Hide;
     Form1.Edit3.Hide;
     Form1.Label4.Show;
     Form1.Edit4.Show;
     Form1.Label5.Show;
     Form1.Edit5.Show;
     end;

  end;
end;

procedure TForm1.FormCreate(Sender: TObject);
begin
  Form1.Label1.Hide;
  Form1.Label2.Hide;
  Form1.Label3.Hide;
  Form1.Label4.Hide;
  Form1.Label5.Hide;
  Form1.Edit1.Hide;
  Form1.Edit2.Hide;
  Form1.Edit3.Hide;
  Form1.Edit4.Hide;
  Form1.Edit5.Hide;
  Form1.Button1.Hide;
end;

end.
