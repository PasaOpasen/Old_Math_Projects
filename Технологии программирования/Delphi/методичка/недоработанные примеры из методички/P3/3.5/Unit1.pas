unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls;

type
  TForm1 = class(TForm)
    GroupBox1: TGroupBox;
    CheckBox1: TCheckBox;
    CheckBox2: TCheckBox;
    CheckBox3: TCheckBox;
    Label1: TLabel;
    Label2: TLabel;
    Edit1: TEdit;
    Edit2: TEdit;
    Memo1: TMemo;
    Button1: TButton;
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
var x,y:real;
begin
 try x:=StrToFloat(Edit1.Text);
  except
   ShowMessage('Íåïğàâèëüíî ââåäåí X');
   exit;
 end;

  try y:=StrToFloat(Edit2.Text);
  except
   ShowMessage('Íåïğàâèëüíî ââåäåí Y');
   exit;
 end;
 if CheckBox1.Checked then
  begin
   if (x>0) and (y>0) and ((x+y)<5) then
    Memo1.Lines.Add('Òî÷êà ('+Edit1.Text+';'+Edit2.Text+') ïğèíàäëåæèò òğåóãîëüíèêó')
   else Memo1.Lines.Add('Òî÷êà ('+Edit1.Text+';'+Edit2.Text+') íå ïğèíàäëåæèò òğåóãîëüíèêó');
  end;

  if CheckBox2.Checked then
  begin
   if (0.5<x) and (x<3) and (-2<y) and (y<4)then
    Memo1.Lines.Add('Òî÷êà ('+Edit1.Text+';'+Edit2.Text+') ïğèíàäëåæèò Ïğÿìîóãîëüíèêó')
   else Memo1.Lines.Add('Òî÷êà ('+Edit1.Text+';'+Edit2.Text+') íå ïğèíàäëåæèò Ïğÿìîóãîëüíèêó');
  end;

  if CheckBox3.Checked then
   begin
    if (x*x+y*y)<=7 then
     Memo1.Lines.Add('Òî÷êà ('+Edit1.Text+';'+Edit2.Text+') ïğèíàäëåæèò Êğóãó')
    else Memo1.Lines.Add('Òî÷êà ('+Edit1.Text+';'+Edit2.Text+') íå ïğèíàäëåæèò Êğóãó');
   end;
end;

procedure TForm1.FormCreate(Sender: TObject);
begin
 Edit1.Text:='1';
 Edit2.Text:='2';
end;

end.
