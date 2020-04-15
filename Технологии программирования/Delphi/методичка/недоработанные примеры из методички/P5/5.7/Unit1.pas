unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls;

type
  TForm1 = class(TForm)
    ComboBox1: TComboBox;
    Edit1: TEdit;
    Button1: TButton;
    Label1: TLabel;
    procedure ComboBox1KeyPress(Sender: TObject; var Key: Char);
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
procedure TForm1.ComboBox1KeyPress(Sender: TObject; var Key: Char);
begin
 if key=#13 then
  begin
   ComboBox1.Items.Add(Edit1.text);
   Edit1.Text:='';
  end;
end;

procedure TForm1.Button1Click(Sender: TObject);
var arr:array [1..100] of string;
    st:string;
    nst,n,i:integer;
begin
  nst:=ComboBox1.ItemIndex;
  st:=ComboBox1.Items[nst]+' ';
  n:=length(st);

  for i:=1 to n do
    if st[i]=' ' then delete(st,i,1);
  n:=length(st);
  for i:=1 to n do
   begin
    if st[i]=st[n+1-i] then
     label1.Caption:='Это полиндром'
    else
     label1.Caption:='Это не полиндром';
   end;
 end;
end.
