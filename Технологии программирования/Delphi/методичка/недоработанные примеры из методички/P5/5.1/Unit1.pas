unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, Buttons, Menus;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    Label3: TLabel;
    Edit1: TEdit;
    ComboBox1: TComboBox;
    BitBtn1: TBitBtn;
    Label2: TLabel;
    Label4: TLabel;
    OpenDialog1: TOpenDialog;
    SaveDialog1: TSaveDialog;
    MainMenu1: TMainMenu;
    N1: TMenuItem;
    N2: TMenuItem;
    Label5: TLabel;
    Label6: TLabel;
    Label7: TLabel;
    procedure ComboBox1Click(Sender: TObject);
    procedure ComboBox1KeyPress(Sender: TObject; var Key: Char);
    procedure N1Click(Sender: TObject);
    procedure N2Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

{$R *.dfm}

procedure TForm1.ComboBox1Click(Sender: TObject);
var i,k,n,nst,max5,min,max:integer;
    st,newst,s,otvet1,otvet2:string;
begin
 nst:=ComboBox1.ItemIndex;
 st:=ComboBox1.Items[nst]+' ';
 n:=length(st);

 for i:=1 to n do
  if (st[i]<>'1') and (st[i]<>'0') and (st[i]<>' ') then
   begin
    showMessage('Должно быть только 0 или 1');
    exit;
   end;

 k:=0;
 max5:=0;
 for i:=1 to n do
  begin
   if st[i]<>' ' then
    begin
     inc(k);
     if k=5 then inc(max5);
    end
   else k:=0;
  end;

  min:=9999;
  k:=0;
  s:='';
  for i:=1 to n do
   begin
    if st[i]<>' ' then
     begin
      inc(k);
      s:=s+st[i];
     end
    else
     begin
      if min>k then
       begin
        min:=k;
        newst:=s;
        s:='';
        k:=0;
       end;
     end;
   end;



  max:=0;
  k:=0;
  for i:=1 to length(st) do
    begin
      if st[i]<>' ' then inc(k)
      else
        begin
          if k>max then max:=k;
          k:=0;
        end;
    end;


  k:=0;
  s:='';
  otvet1:='';
  for i:=1 to length(st) do
    begin
      if st[i]<>' ' then
        begin
          inc(k);
          s:=s+st[i];
        end
      else
        begin
          if k mod 2=0 then otvet1:=otvet1+s+' ';
          k:=0;
          s:='';
        end;
    end;


  k:=0;
  s:='';
  otvet2:='';
  for i:=1 to length(st) do
    begin
      if st[i]<>' ' then
        begin
          inc(k);
          s:=s+st[i];
        end
      else
        begin
          if k mod 2=1 then otvet2:=otvet2+s+' ';
          k:=0;
          s:='';
        end;
    end;
  label2.Caption:='Количество символов в самой длинной группе: '+inttostr(max);
  label4.Caption:='Группа с четным количеством символов '+ otvet1;
  label5.Caption:='Кол-во групп с 5 символами равно '+inttostr(max5);
  label6.Caption:='Минимальная группа равна '+newst;
  label7.Caption:='Группа с нечетным количеством символов '+ otvet2;

end;

procedure TForm1.ComboBox1KeyPress(Sender: TObject; var Key: Char);
begin
if key=#13 then
 begin
  ComboBox1.Items.Add(Edit1.Text);
  Edit1.Text:='';
 end;
end;

procedure TForm1.N1Click(Sender: TObject);
var txt:textFile;
    s:string;

begin
  if openDialog1.Execute then
    begin
      assignFile(txt,OpenDialog1.FileName);
      reset(txt);
      while not eof(txt) do
        begin
          readln(txt,s);
          comboBox1.Items.Add(s);
        end;
      closeFile(txt);
    end;
end;

procedure TForm1.N2Click(Sender: TObject);
var txt:textFile;
    nst:integer;
begin
  if saveDialog1.Execute then
    begin
      assignFile(txt,SaveDialog1.FileName);
      rewrite(txt);
      nst:=ComboBox1.ItemIndex;
      writeln(txt,ComboBox1.Items[nst]);
      writeln(txt,Label2.Caption);
      writeln(txt,Label4.Caption);
      writeln(txt,Label5.Caption);
      writeln(txt,Label6.Caption);
      writeln(txt,Label7.Caption);
      closeFile(txt);

    end;
end;

end.
