unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, Buttons, ExtCtrls;

type
  TForm1 = class(TForm)
    PaintBox1: TPaintBox;
    BitBtn1: TBitBtn;
    Edit1: TEdit;
    Button1: TButton;
    FontDialog1: TFontDialog;
    procedure Button1Click(Sender: TObject);
    procedure BitBtn1Click(Sender: TObject);
    procedure FormCreate(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;


var
  Form1: TForm1;
  x,y,h:integer;

implementation

{$R *.dfm}

procedure TForm1.FormCreate(Sender: TObject);
begin
 x:=30;
 y:=30;
 h:=40;
end;

procedure TForm1.Button1Click(Sender: TObject);
begin
  if FontDialog1.Execute then
    PaintBox1.Canvas.Font:=FontDialog1.Font;
end;



procedure TForm1.BitBtn1Click(Sender: TObject);
begin;
 PaintBox1.Canvas.Font:=FontDialog1.Font;
 PaintBox1.Canvas.TextOut(x,y,Edit1.Text);
 y:=y+h;

end;


end.
