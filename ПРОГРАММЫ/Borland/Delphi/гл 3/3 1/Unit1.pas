unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    Edit1: TEdit;
    Edit2: TEdit;
    Button1: TButton;
    Button2: TButton;
    Button3: TButton;
    Button4: TButton;
    Memo1: TMemo;
    procedure FormCreate(Sender: TObject);

    procedure Button1Click(Sender: TObject);
    procedure Button2Click(Sender: TObject);
    procedure Button4Click(Sender: TObject);
  
    procedure Button3Click(Sender: TObject);private
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
  Memo1.Lines.Add('������ 3.1 �������� ������ �. �.');
  Edit1.Text:='0,5';
  Edit2.Text:='2,5';
  Memo1.Clear;
end;


procedure TForm1.Button1Click(Sender: TObject);
var x,y,z,u:real;
begin
  x:=StrToFloat(Edit1.Text);
  Memo1.Lines.Add('X='+Edit1.Text);
  y:=StrToFloat(Edit1.Text);
  Memo1.Lines.Add('Y='+Edit2.Text);
  u:=sin(sqr(x)+sqr(y))+Exp(y-x);
  Memo1.Lines.Add('��������� u =' +FloatToStrF(u,fffixed,8,3));
end;

procedure TForm1.Button2Click(Sender: TObject);
var x,y,z,u:real;
begin
  x:=StrToFloat(Edit1.Text);
  Memo1.Lines.Add('X='+Edit1.Text);
  y:=StrToFloat(Edit1.Text);
  Memo1.Lines.Add('Y='+Edit2.Text);
  if y=0 then ShowMessage('y=0!!!')
  else
    begin
      u:=Abs(Cos(x))+arctan(1/y);
      Memo1.Lines.Add('��������� u =' +FloatToStrF(u,fffixed,8,3));
    end;
end;

procedure TForm1.Button4Click(Sender: TObject);
var x,y,z,u:real;
begin
  x:=StrToFloat(Edit1.Text);
  Memo1.Lines.Add('X='+Edit1.Text);
  y:=StrToFloat(Edit1.Text);
  Memo1.Lines.Add('Y='+Edit2.Text);
  if arctan(y)=0 then ShowMessage('y=0!!!')
  else
    begin
      u:=sqr(Cos(x))+abs(y)/arctan(y);
      Memo1.Lines.Add('��������� u =' +FloatToStrF(u,fffixed,8,3));
    end;
end;


procedure TForm1.Button3Click(Sender: TObject);
var x,y,z,u:real;
begin
  x:=StrToFloat(Edit1.Text);
  Memo1.Lines.Add('X='+Edit1.Text);
  y:=StrToFloat(Edit1.Text);
  Memo1.Lines.Add('Y='+Edit2.Text);
  u:=sin(x+sqr(y))/cos(x+sqr(y))+y*ln(x);
  Memo1.Lines.Add('��������� u =' +FloatToStrF(u,fffixed,8,3));
end;

//u = tg(x+y^2)+y*ln(x)
 
end.



