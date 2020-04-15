unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls;

type
  TForm1 = class(TForm)
    Edit1: TEdit;
    ComboBox1: TComboBox;
    RadioGroup1: TRadioGroup;
    Label1: TLabel;
    Label2: TLabel;
    Button1: TButton;
    procedure Edit1KeyPress(Sender: TObject; var Key: Char);
    procedure ComboBox1Click(Sender: TObject);
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


procedure TForm1.Edit1KeyPress(Sender: TObject; var Key: Char);
var st:string;
    i:integer;
begin
  if key=#13 then
    begin
      st:=Edit1.Text+' ';

      while (st[1]=' ') do delete(st,1,1);

      i:=1;
      while (i<=length(st)) do
        begin
          if (st[i-1]=' ') and (st[i]=' ') then delete(st,i,1)
          else inc(i);
        end;

      ComboBox1.Items.Add(st);
      Edit1.Text:='';
    end;
end;

procedure swap(var a,b:char);
var temp:char;
begin
 temp:=a;
 a:=b;
 b:=temp;
end;

Procedure zamena(var str:string);
var s:string;
    i:integer;
begin
  s:='';
  for i:=1 to length(str) do
    case str[i] of
      ' ':s:=s+' ';
      'à':s:=s+'a';
      'á':s:=s+'b';
      'â':s:=s+'v';
      'ã':s:=s+'g';
      'ä':s:=s+'d';
      'å':s:=s+'e';
      '¸':s:=s+'e';
      'æ':s:=s+'zh';
      'ç':s:=s+'z';
      'è':s:=s+'i';
      'é':s:=s+'i';
      'ê':s:=s+'k';
      'ë':s:=s+'l';
      'ì':s:=s+'m';
      'í':s:=s+'n';
      'î':s:=s+'o';
      'ï':s:=s+'p';
      'ð':s:=s+'r';
      'ñ':s:=s+'s';
      'ò':s:=s+'t';
      'ó':s:=s+'u';
      'ô':s:=s+'f';
      'õ':s:=s+'x';
      'ö':s:=s+'c';
      '÷':s:=s+'ch';
      'ø':s:=s+'sh';
      'ù':s:=s+'sha';
      'ú':s:=s+'';
      'û':s:=s+'';
      'ü':s:=s+'';
      'ý':s:=s+'e';
      'þ':s:=s+'yu';
      'ÿ':s:=s+'ya';
      'À':s:=s+'A';
      'Á':s:=s+'B';
      'Â':s:=s+'V';
      'Ã':s:=s+'G';
      'Ä':s:=s+'D';
      'Å':s:=s+'R';
      '¨':s:=s+'R';
      'Æ':s:=s+'Zh';
      'Ç':s:=s+'Z';
      'È':s:=s+'I';
      'É':s:=s+'I';
      'Ê':s:=s+'K';
      'Ë':s:=s+'L';
      'Ì':s:=s+'M';
      'Í':s:=s+'N';
      'Î':s:=s+'O';
      'Ï':s:=s+'P';
      'Ð':s:=s+'R';
      'Ñ':s:=s+'S';
      'Ò':s:=s+'T';
      'Ó':s:=s+'U';
      'Ô':s:=s+'F';
      'Õ':s:=s+'X';
      'Ö':s:=s+'C';
      '×':s:=s+'Ch';
      'Ø':s:=s+'Sh';
      'Ù':s:=s+'Sha';
      'Ý':s:=s+'E';
      'Þ':s:=s+'Yu';
      'ß':s:=s+'Ya';
    end;
    str:=s;
end;


procedure TForm1.ComboBox1Click(Sender: TObject);
var st:string;
    n,i,nst,ind,numChar1,numChar2:integer;
    c:char;
begin
  n:=0;
  ind:=0;
  nst:=ComboBox1.ItemIndex ;
  st:=comboBox1.Items[nst];

  if (RadioGroup1.ItemIndex=0) then
    begin
      numChar1:=1;
      for i:=1 to length(st) do
       begin
        if(st[i-1]=' ') then numChar1:=i;
        if(st[i+1]=' ') then
         begin
          numChar2:=i;
          swap(st[numChar1],st[numChar2]);
         end;
       end;
      label2.Caption:=st;
    end;

  if (radioGroup1.ItemIndex=1) then
    begin
      zamena(st);
      label2.Caption:=st;
    end;


end;

procedure TForm1.Button1Click(Sender: TObject);
var txt:textFile;
    st:string;
begin
assignFile(txt,'in.txt');
reset(txt);
while not eof(txt) do
  begin
    readln(txt,st);
    comboBox1.Items.Add(st);
  end;
closeFile(txt);
end;


end.
