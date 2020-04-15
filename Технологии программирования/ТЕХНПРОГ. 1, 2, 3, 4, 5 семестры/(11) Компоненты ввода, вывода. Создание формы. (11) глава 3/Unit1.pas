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
    Edit3: TEdit;
    Memo1: TMemo;
    Button1: TButton;
    Label4: TLabel;
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
  Edit1.Text:='0,5';
  Edit2.Text:='2,5';
  Edit3.Text:='1,5';
  Memo1.Clear;
  //Memo1.Lines.Add('');
end;

procedure TForm1.Button1Click(Sender: TObject);
var x,y,z,u: real;
begin
  x:=StrToFloat(Edit1.Text);
  Memo1.Lines.Add('X='+Edit1.Text);
  y:=StrToFloat(Edit2.Text);
  Memo1.Lines.Add('Y='+Edit2.Text);
  z:=StrToFloat(Edit3.Text);
  Memo1.Lines.Add('Z='+Edit3.Text);
  u:=1/Cos(x+y)+Exp(y-z);
  Memo1.Lines.Add('Result: u= '+FloatToStrF(u,fffixed,8,3));
end;

end.
