unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls;

type
  TForm1 = class(TForm)
    Image1: TImage;
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    Edit1: TEdit;
    Edit2: TEdit;
    Edit3: TEdit;
    RadioGroup1: TRadioGroup;
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
  Edit1.Text:='3';
  Edit2.Text:='3';
  Edit3.Text:='3';
end;

procedure TForm1.Button1Click(Sender: TObject);
var p,a,b,c,s:real;
begin
  try
    a:=StrToFloat(Edit1.Text);
    b:=StrToFloat(Edit2.Text);
    c:=StrToFloat(Edit3.Text);
  except
    ShowMessage('Неправильнный ввод');
    Exit;
  end;

  p:=(a+b+c)/2;
  s:=sqrt(p*(p-a)*(p-b)*(p-c));
  case RadioGroup1.ItemIndex of
    0: Memo1.Lines.Add('Пириметр: '+FloatToStrF(p*2,fffixed,6,3));
    1: Memo1.Lines.Add('Площадь: '+FloatToStrF(s,fffixed,6,3));
  end;

end;

end.
