unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    Label2: TLabel;
    x: TEdit;
    GroupBox1: TGroupBox;
    CkBx3: TCheckBox;
    CkBx5: TCheckBox;
    CkBx7: TCheckBox;
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
var s1,s2,s3:string[5]; Flag:Boolean;
begin
  Flag:=false;//начальное значение кратности, пусть нет кратных
  S1:='';S2:='';S3:='';//начал.знач.строк, хранящих инф. о рез.проверки на крт. 3,5,7
//если элм. в контейнере отмечен и заданное число кратно ему, то запомнить элм. в пере. S1
//и отметить, что кратный существует
if CkBx3.Checked and (StrToInt(x.Text) mod 3=0)
  then Begin S1:='    3'; Flag:=True End;
if CkBx5.Checked and (StrToInt(x.Text) mod 5=0)
  then Begin S2:='    5'; Flag:=True End;
if CkBx7.Checked and (StrToInt(x.Text) mod 7=0)
  then Begin S3:='    7'; Flag:=True End;
//если кратные обнаружены, то вывод их в окно Memo1
if flag then Memo1.Lines.Add(x.Text+' Кратно '+S1+S2+S3)
  else Memo1.Lines.Add(x.Text+'не кратно выделенным элементам');
end;

procedure TForm1.FormCreate(Sender: TObject);
begin
  Memo1.Clear;//очистка окна редактора
  CkBx3.Checked:=True;//отметить в контейнере элм. CkBx3
  CkBx7.Checked:=True;//отметить в контецнере элм. CkBx7
end;

end.
