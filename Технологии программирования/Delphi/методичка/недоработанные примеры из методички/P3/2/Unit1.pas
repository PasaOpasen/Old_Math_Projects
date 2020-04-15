unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    Label2: TLabel;
    Edit1: TEdit;
    GroupBox1: TGroupBox;
    CheckBox1: TCheckBox;
    CheckBox2: TCheckBox;
    CheckBox3: TCheckBox;
    Button1: TButton;
    Memo1: TMemo;
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
Edit1.Text:='21';
Memo1.Clear;
CheckBox1.Checked:=true;
CheckBox3.Checked:=true;
end;

procedure TForm1.Button1Click(Sender: TObject);
var s1,s2,s3:String[5];
    flag:boolean;
begin
s1:='';
s2:='';
s3:='';
flag:=false;
if CheckBox1.Checked and(StrToInt(Edit1.Text) mod 3=0) then
 begin
  s1:='   3';
  flag:=true;
 end;
 if CheckBox2.Checked and(StrToInt(Edit1.Text) mod 5=0) then
 begin
  s2:='   5';
  flag:=true;
 end;
 if CheckBox3.Checked and(StrToInt(Edit1.Text) mod 7=0) then
 begin
  s3:='   7';
  flag:=true;
 end;
 if flag then Memo1.Lines.Add(Edit1.Text+' Кратно '+s1+s2+s3)
 else  Memo1.Lines.Add(Edit1.Text+' Некратно выделенным элементам');
end;

end.
