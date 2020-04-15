unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, Buttons, ExtCtrls;

type
  TForm1 = class(TForm)
    Label2: TLabel;
    Label3: TLabel;
    ComboBox1: TComboBox;
    Edit1: TEdit;
    BitBtn1: TBitBtn;
    Label5: TLabel;
    Label6: TLabel;
    Label7: TLabel;
    Label4: TLabel;
    Label8: TLabel;
    Label9: TLabel;
    Label10: TLabel;
    RadioGroup1: TRadioGroup;
    RadioButton1: TRadioButton;
    RadioButton2: TRadioButton;
    OpenDialog1: TOpenDialog;
    procedure ComdoBox1Click(Sender: TObject);
    procedure ComboBox1KeyPress(Sender: TObject; var Key: Char);
    procedure FormCreate(Sender: TObject);
    procedure RadioButton2Click(Sender: TObject);
    procedure RadioButton1Click(Sender: TObject);
    //procedure Label4Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

{$R *.dfm}

procedure TForm1.ComdoBox1Click(Sender: TObject);
var
st,s:string;
n,i,nst,ind,p:integer;
BegTime: TDateTime;
begin
  Label2.Show;
  Label7.Show;
  Label4.Show;
  Label8.Show;

  n:=0;
  ind:=0;
  nst:=ComboBox1.ItemIndex;
  st:=ComboBox1.Items[nst];

  BegTime:= Time;
  s:=ComboBox1.Items[nst];
  s:=s+' ';
  while s<>'' do
  begin
    p:=pos(' ',s);
    if p>1 then inc(n);
    delete(s,1,p);
  end;
  Label5.Caption:=IntToStr(n);
  Label9.Caption:= FormatDateTime('hh:mm:ss.zzz',Time - BegTime);

  BegTime:= Time;
  n:=0;
  for i:=1 to Length(st) do
  begin
    case ind of
    0: if st[i]<>' ' then
       begin
         ind:=1;
         inc(n);
       end;
    1: if st[i]=' ' then ind:=0;
    end;
  end;
  Label6.Caption:=IntToStr(n);
  Label10.Caption:= FormatDateTime('hh:mm:ss.zzz',Time - BegTime);
end;

procedure TForm1.ComboBox1KeyPress(Sender: TObject; var Key: Char);
begin
  if key=#13 then
  begin
    ComboBox1.Items.Add(Edit1.Text);
    Edit1.Text:='';
  end;
end;

{procedure TForm1.Label4Click(Sender: TObject);
var s:string;kol,p:integer;
begin
  p:=ComboBox1.ItemIndex;
  s:=ComboBox1.Items[p];
  s:=s+' '; kol:=0;
  while s<>'' do
  begin
    p:=pos(' ',s);
    if p>1 then inc(kol);
    delete(s,1,p);
    end;
  end;
  Label4.Caption:=IntToStr(kol);
end; }

procedure TForm1.FormCreate(Sender: TObject);
begin
   Label2.Hide;
   Label7.Hide;
   Label4.Hide;
   Label8.Hide;
   RadioButton1.Checked:=TRUE;
end;

procedure TForm1.RadioButton2Click(Sender: TObject);
var fz:textfile;
n:string ;
begin
 if RadioButton2.Checked=True then
 begin
   Edit1.Hide;
   if OpenDialog1.Execute then
   begin
    AssignFile(fz,OpenDialog1.Filename);
    Reset(fz);
    Read(fz,n);
    ComboBox1.Items.Add(n);
    closefile(fz);
   end;

 end;
end;

procedure TForm1.RadioButton1Click(Sender: TObject);
begin
  Edit1.Show;
end;

end.
