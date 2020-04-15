unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls;

type
  TForm1 = class(TForm)
    lbl1: TLabel;
    lbl2: TLabel;
    edt1: TEdit;
    lbl3: TLabel;
    edt2: TEdit;
    lbl4: TLabel;
    edt3: TEdit;
    rg1: TRadioGroup;
    grp1: TGroupBox;
    chk1: TCheckBox;
    chk2: TCheckBox;
    rb1: TRadioButton;
    rb2: TRadioButton;
    rb3: TRadioButton;
    mmo1: TMemo;
    procedure rb1Click(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure rb2Click(Sender: TObject);
    procedure rb3Click(Sender: TObject);
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
  Mmo1.Clear;
  edt1.Text:='3';
  edt2.Text:='4';
  edt3.Text:='5';
  chk1.Checked:=True;
  chk2.Checked:=True;
  rb1.Checked:=True;
end;

procedure TForm1.rb1Click(Sender: TObject);
var flag: Boolean; s1,s2: string; a,b,c: Real;
begin
  flag:=False;
  s1:='';
  s2:='';
  a:=StrToFloat(edt1.Text);
  b:=StrToFloat(edt2.Text);
  c:=StrToFloat(edt3.Text);
  if chk1.Checked then
    if ((a+b+c)<(a*b*c)) then
    begin
      flag:=True;
      s1:='Минимальное: a+b+c; ';
    end
    else
    begin
      flag:=True;
      s1:=s1+'Минимальное: abc;';
    end;
  if chk2.Checked then
    if ((a+b+c)<(a*b*c)) then
    begin
      flag:=True;
      s2:='Максимальное: abc;';
    end
    else
    begin
      flag:=True;
      s2:='Максимальное: a+b+c;';
    end;
  if flag then
    Mmo1.Lines.Add(s1+s2)
  else
    mmo1.Lines.Add('Нам нечего Вам предложить:)');
end;



procedure TForm1.rb2Click(Sender: TObject);
var flag: Boolean; s1,s2: string; a,b,c: Real;
begin
  flag:=False;
  s1:='';
  s2:='';
  a:=StrToFloat(edt1.Text);
  b:=StrToFloat(edt2.Text);
  c:=StrToFloat(edt3.Text);
  if chk1.Checked then
    if ((a*b+c/2)<(a*b+c)) then
    begin
      flag:=True;
      s1:='Минимальное: ab+c/2; ';
    end
    else
    begin
      flag:=True;
      s1:=s1+'Минимальное: ab+c;';
    end;
  if chk2.Checked then
    if ((a*b+c/2)<(a*b+c)) then
    begin
      flag:=True;
      s2:='Максимальное: ab+c;';
    end
    else
    begin
      flag:=True;
      s2:='Максимальное: ab+c/2;';
    end;
  if flag then
    Mmo1.Lines.Add(s1+s2)
  else
    mmo1.Lines.Add('Нам нечего Вам предложить:)');
end;


procedure TForm1.rb3Click(Sender: TObject);
var flag: Boolean; s1,s2: string; a,b,c: Real;
begin
  flag:=False;
  s1:='';
  s2:='';
  a:=StrToFloat(edt1.Text);
  b:=StrToFloat(edt2.Text);
  c:=StrToFloat(edt3.Text);
  if chk1.Checked then
    if ((a/2)<(Sqr(b)+c)) then
    begin
      flag:=True;
      s1:='Минимальное: a/2; ';
    end
    else
    begin
      flag:=True;
      s1:=s1+'Минимальное: b^2+c;';
    end;
  if chk2.Checked then
    if ((a/2)<(Sqr(b)+c)) then
    begin
      flag:=True;
      s2:='Максимальное: b^2+c;';
    end
    else
    begin
      flag:=True;
      s2:='Максимальное: a/2;';
    end;
  if flag then
    Mmo1.Lines.Add(s1+s2)
  else
    mmo1.Lines.Add('Нам нечего Вам предложить:)');
end;

end.
