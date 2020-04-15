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
    ComboBox1: TComboBox;
    Edit1: TEdit;
    BitBtn1: TBitBtn;
    Label4: TLabel;
    Memo1: TMemo;
    procedure ComdoBox1Click(Sender: TObject);
    procedure ComboBox1KeyPress(Sender: TObject; var Key: Char);
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
var st,stt,le,re:string;  n,i,nst,p,index,indexe:integer;
begin
  n:=0;
  nst:=ComboBox1.ItemIndex;//опредение номера выбранной строки
  st:=ComboBox1.Items[nst];//занесение выбранной строки в переменную
  Label4.Caption:=('');//очистка второй метки, ибо в неё может быть ничего не занесено
  Memo1.Lines.Add('');

  for i:=1 to Length(st) do
    if st[i]=' ' then inc(n);
  if n<=1 then
  begin
    Label3.Caption:=('подстроки не существует: мало пробелов');//если пробелов не больше одного, то подстроки не существует: ей неоткуда взяться
    exit;//окончание программы
  end;

  {подстрока есть:пробелов больше одного}

  for i:=2 to Length(st) do
  if (st[i-1]=' ') and (st[i]<>' ') then
  begin
    index:=i;//номер, с которого начинается подстрока
    break; //пока не встретится число после пробела
  end;

  p:=0;
  indexe:=0;
  while(st[i]<>' ') do //пока не наткнётся на следующий пробел - анализ подстроки
  begin
    if (st[i]<>'e') and (st[i]<>'+') and (st[i]<>'-') then inc(p);
    if (st[i]='e') then inc(indexe);
    inc(i);
  end;

  stt:=copy(st,index,i-index);//выделение нужной подстроки
  Memo1.Lines.Add(stt);

  if (p=0) or (indexe>1) then
  begin
     Label3.Caption:=('подстрока не является числом: нет цифр или много букв "e"');//если в подстроке не будет цифр или в подстроке больше одной буквы 'e', то подстрокиа не будет числом
     exit;
  end;

   i:=1;
   while (i<=Length(stt)) and (stt[i]<>'e') do inc(i); //узнаётся место числа 'e'

   le:=copy(stt,1,i-1);//часть числа левее 'e'
   re:=copy(stt,i+1,Length(stt)-i);//часть числа правее 'e'
   Memo1.Lines.Add(le);  Memo1.Lines.Add(re);

   i:=2;
   while (i<=Length(le)) do
   begin
    if (le[i]='+') or (le[i]='-') then
    begin
     Label3.Caption:=('подстрока не является числом: знак операции в первой половине');//если до буквы 'e' встретится знак операции (помимо первого элемента), то подстрока не будет числом
     exit;
    end;
    inc(i);
   end;
   i:=2;
   while (i<=Length(re)) do
   begin
    if (re[i]='+') or (re[i]='-') then
    begin
     Label3.Caption:=('подстрока не является числом: знак операции во второй половине');//если после буквы 'e' встретится знак операции (помимо первого элемента), то подстрока не будет числом
     exit;
    end;
    inc(i);
   end;

   if (Length(re)=1) and ((re[1]='+') or (re[1]='-')) then
   begin
     Label3.Caption:=('подстрока не является числом: после "е" есть лишь символ');//если после буквы 'e' есть только символ операции, то подстрока не будет числом
     exit;
   end;


   {подстрока является числом}

   

   if Length(le)=0 then Label3.Caption:=('число положительное')//если в подстроке до 'e' ничего не стоит, то подстрокиа будет положительным числом
   else if(le[1]='-') then
        begin
        Label3.Caption:=('число отрицательное');//если в подстроке первый знак '-', то подстрокиа будет отрицательным числом
        end
        else
        begin
          Label3.Caption:=('число положительное');//если в подстроке первый знак не '-', то подстрокиа будет положительным числом
        end;

   if Length(re)=0 then Label4.Caption:=('число целое')//если после 'e' ничего не стоит, то подстрока считается целым числом
   else if (re[1]='-') then
        begin
          Label4.Caption:=('число дробное');//если после 'e' стоит '-', то подстрока считается дробным числом
          exit;
        end
        else
        begin
          Label4.Caption:=('число целое');//если после 'e' стоят '+' или цифра, то подстрока считается целым числом
          exit;
        end;

end;

procedure TForm1.ComboBox1KeyPress(Sender: TObject; var Key: Char);
begin
  if key=#13 then
  begin
    ComboBox1.Items.Add(Edit1.Text);
    Edit1.Text:='';
  end;
end;

end.
