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
    Label3: TLabel;
    Edit2: TEdit;
    Label4: TLabel;
    Label5: TLabel;
    Edit3: TEdit;
    Label6: TLabel;
    Edit4: TEdit;
    Label7: TLabel;
    Label8: TLabel;
    Edit5: TEdit;
    Label9: TLabel;
    Edit6: TEdit;
    Label10: TLabel;
    Label11: TLabel;
    Label12: TLabel;
    Label13: TLabel;
    RadioGroup1: TRadioGroup;
    Memo1: TMemo;
    Button1: TButton;
    RadioButton1: TRadioButton;
    RadioButton2: TRadioButton;
    RadioButton3: TRadioButton;
    RadioButton4: TRadioButton;
    RadioButton5: TRadioButton;
    RadioButton6: TRadioButton;
    RadioButton7: TRadioButton;
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
 Edit1.Text:='0';
 Edit2.Text:='0';

 Edit3.Text:='0';
 Edit4.Text:='1';

 Edit5.Text:='1';
 Edit6.Text:='0';
 Memo1.Clear;
 RadioButton1.Checked:=true;
end;

procedure TForm1.Button1Click(Sender: TObject);
var ab,bc,ca,p,s,m,h,l:real;
begin
 ab:=sqrt(sqr(strtofloat(Edit1.text)-strtofloat(Edit3.text))+sqr(strtofloat(Edit2.text)-strtofloat(Edit4.text)));
 bc:=sqrt(sqr(strtofloat(Edit3.text)-strtofloat(Edit5.text))+sqr(strtofloat(Edit4.text)-strtofloat(Edit6.text)));
 ca:=sqrt(sqr(strtofloat(Edit1.text)-strtofloat(Edit5.text))+sqr(strtofloat(Edit2.text)-strtofloat(Edit6.text)));
 p:=(ab+bc+ca)/2;
 s:=sqrt(p*(p-ab)*(p-bc)*(p-ca));
 if radiobutton1.Checked then
  begin
   h:=(2/ab)*s;
   Memo1.Lines.Add(FloatToStrF(h,fffixed,6,2));
  end;
 if radiobutton2.Checked then
  begin
   h:=(2/bc)*s;
   Memo1.Lines.Add(FloatToStrF(h,fffixed,6,2));
  end;
 if radiobutton3.Checked then
  begin
   h:=(2/ca)*s;
   Memo1.Lines.Add(FloatToStrF(h,fffixed,6,2));
  end;
 if radiobutton4.Checked then Memo1.Lines.Add(FloatToStrF(p*2,fffixed,6,2));
 if radiobutton5.Checked then
  begin
   l:=(sqrt(ab*bc*(ab+bc+ca)*(ab+bc-ca)))/(ab+bc);
   Memo1.Lines.Add(FloatToStrF(l,fffixed,6,2));
  end;
 if radiobutton6.Checked then
  begin
   l:=(sqrt(bc*ca*(ab+bc+ca)*(ab+bc-ca)))/(bc+ca);
   Memo1.Lines.Add(FloatToStrF(l,fffixed,6,2));
  end;
 if radiobutton7.Checked then
  begin
   l:=(sqrt(ca*ab*(ab+bc+ca)*(ab+bc-ca)))/(ab+ca);
   Memo1.Lines.Add(FloatToStrF(l,fffixed,6,2));
  end;
end;

end.
