unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, Buttons;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    Edit1: TEdit;
    BitBtn1: TBitBtn;
    ComboBox1: TComboBox;
    procedure ComboBox1KeyPress(Sender: TObject; var Key: Char);
    procedure ComboBox1Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;
const D=['à'..'ÿ'];
var
  Form1: TForm1;

implementation

{$R *.dfm}

procedure TForm1.ComboBox1KeyPress(Sender: TObject; var Key: Char);
begin
 if key=#13 then
  begin
   ComboBox1.Items.Add(Edit1.Text);
   Edit1.Text:='';
  end;
end;

procedure TForm1.ComboBox1Click(Sender: TObject);
var st,uniqe,res:string;
    i,j,n,nst:integer;
    temp:boolean;
begin
 n:=0;
 nst:=ComboBox1.ItemIndex;
 st:=ComboBox1.Items[nst];

 res:='a) ';
 for i:=1 to length(st) do
   if st[i] in D then res:=res+st[i]+' ';

 uniqe:=st[1];
 for i:=2 to length(st) do
  begin
   temp:=true;
   for j:=1 to length(uniqe) do
    if st[i]=uniqe[j] then
     begin
      temp:=false;
     end;
   if temp then uniqe:=uniqe+st[i];
  end;

 res:=res+'á) ';
 res:=res+uniqe;
 Label3.Caption:=res;
end;

end.
