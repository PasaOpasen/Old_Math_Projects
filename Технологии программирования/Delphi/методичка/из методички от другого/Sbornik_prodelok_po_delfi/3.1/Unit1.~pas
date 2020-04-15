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
    EditX: TEdit;
    EditY: TEdit;
    GroupBox1: TGroupBox;
    CheckBoxA: TCheckBox;
    CheckBoxB: TCheckBox;
    CheckBoxV: TCheckBox;
    CheckBoxG: TCheckBox;
    Button1: TButton;
    Memo1: TMemo;
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
var x,y,u: Real;
begin
 try
   x:=StrToFloat(EditX.Text);
   y:=StrToFloat(EditY.Text);
 except
   ShowMessage('Неверный числовой формат');
   exit;
 end;
 if CheckBoxA.Checked then
  begin
   u:=Sin(x*x+y*y)+Exp(y-x);
   Memo1.Lines.Add(CheckBoxA.Caption + ' = ' + FloatToStrF(u,fffixed,8,3));
  end;
 if CheckBoxB.Checked then
  try
   u:=Sin(x+y*y)/Cos(x+y*y)+y*ln(x);
   Memo1.Lines.Add(CheckBoxB.Caption + ' = ' + FloatToStrF(u,fffixed,8,3));
  except
   Memo1.Lines.Add('Деление на ноль');
  end;
 if CheckBoxV.Checked then
  try
   u:=abs(cos(x))+arctan(1/y);
   Memo1.Lines.Add(CheckBoxV.Caption + ' = ' + FloatToStrF(u,fffixed,8,3));
  except
   Memo1.Lines.Add('Деление на ноль');
  end;
 if CheckBoxG.Checked then
  try
   u:=Cos(x)*Cos(x)+abs(y)/arctan(x);
   Memo1.Lines.Add(CheckBoxG.Caption + ' = ' + FloatToStrF(u,fffixed,8,3));
  except
   Memo1.Lines.Add('Деление на ноль');
  end;
end;

procedure TForm1.FormCreate(Sender: TObject);
begin
 Memo1.Clear;
end;

end.
