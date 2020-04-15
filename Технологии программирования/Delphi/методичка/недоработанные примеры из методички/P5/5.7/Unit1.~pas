unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls;

type
  TForm1 = class(TForm)
    ComboBox1: TComboBox;
    Edit1: TEdit;
    Memo1: TMemo;
    Button1: TButton;
    RadioGroup1: TRadioGroup;
    RadioButton1: TRadioButton;
    RadioButton2: TRadioButton;
    procedure ComboBox1KeyPress(Sender: TObject; var Key: Char);
    procedure FormCreate(Sender: TObject);
    procedure Button1Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;
function Transliterate(s: string): string;
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

procedure TForm1.FormCreate(Sender: TObject);
begin
 Memo1.Clear;
 RadioButton1.Checked:=true;
end;

procedure TForm1.Button1Click(Sender: TObject);
var arr:array [1..100] of string;
    st,NewSt:string;
    nst,n,i,max,k:integer;
begin
  nst:=ComboBox1.ItemIndex;
  st:=ComboBox1.Items[nst]+' ';
  n:=length(st);

  if RadioButton1.Checked then
   begin
    k:=1;
    for i:=1 to n do
     begin
      if st[i]<>' ' then
       begin
        NewSt:=NewSt+st[i];
       end
      else
       begin
        arr[k]:=NewSt;
        inc(k);
        NewSt:='';
       end;
     end;
     dec(k);
     max:=length(arr[1]);
     for i:=2 to k do
      if max<length(arr[i]) then max:=length(arr[i]);
     for i:=1 to k do
      begin
       //Memo1.Lines.add(inttostr(length(arr[i])));
       while (length(arr[i])<max) do arr[i]:=' '+arr[i];
       Memo1.Lines.Add(arr[i]);
       //Memo1.Lines.add(inttostr(length(arr[i])));
      end;
   end;
  if RadioButton2.checked then
   begin
    Memo1.Lines.Add(Transliterate(st));
   end;
end;

function Transliterate(s: string): string;
var
 i: integer;
 t: string;
begin
 for i:=1 to Length(s) do
  begin
   case s[i] of
        'à': t:=t+'a';
        'á': t:=t+'b';
        'â': t:=t+'v';
        'ã': t:=t+'g';
        'ä': t:=t+'d';
        'å': t:=t+'e';
        '¸': t:=t+'ye';
        'æ': t:=t+'zh';
        'ç': t:=t+'z';
        'è': t:=t+'i';
        'é': t:=t+'y';
        'ê': t:=t+'k';
        'ë': t:=t+'l';
        'ì': t:=t+'m';
        'í': t:=t+'n';
        'î': t:=t+'o';
        'ï': t:=t+'p';
        'ð': t:=t+'r';
        'ñ': t:=t+'s';
        'ò': t:=t+'t';
        'ó': t:=t+'u';
        'ô': t:=t+'f';
        'õ': t:=t+'ch';
        'ö': t:=t+'z';
        '÷': t:=t+'ch';
        'ø': t:=t+'sh';
        'ù': t:=t+'ch';
        'ú': t:=t+'''';
        'û': t:=t+'y';
        'ü': t:=t+'''';
        'ý': t:=t+'e';
        'þ': t:=t+'yu';
        'ÿ': t:=t+'ya';
        'À': T:=T+'A';
        'Á': T:=T+'B';
        'Â': T:=T+'V';
        'Ã': T:=T+'G';
        'Ä': T:=T+'D';
        'Å': T:=T+'E';
        '¨': T:=T+'Ye';
        'Æ': T:=T+'Zh';
        'Ç': T:=T+'Z';
        'È': T:=T+'I';
        'É': T:=T+'Y';
        'Ê': T:=T+'K';
        'Ë': T:=T+'L';
        'Ì': T:=T+'M';
        'Í': T:=T+'N';
        'Î': T:=T+'O';
        'Ï': T:=T+'P';
        'Ð': T:=T+'R';
        'Ñ': T:=T+'S';
        'Ò': T:=T+'T';
        'Ó': T:=T+'U';
        'Ô': T:=T+'F';
        'Õ': T:=T+'Ch';
        'Ö': T:=T+'Z';
        '×': T:=T+'Ch';
        'Ø': T:=T+'Sh';
        'Ù': T:=T+'Ch';
        'Ú': T:=T+'''';
        'Û': T:=T+'Y';
        'Ü': T:=T+'''';
        'Ý': T:=T+'E';
        'Þ': T:=T+'Yu';
        'ß': T:=T+'Ya';
      else t:=t+s[i];
   end;
  end;
 Result:=t;
end;

end.
