unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, Buttons;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    Edit1: TEdit;
    ComboBox1: TComboBox;
    Label2: TLabel;
    Label3: TLabel;
    BitBtn1: TBitBtn;
    GroupBox1: TGroupBox;
    RadioButton1: TRadioButton;
    RadioButton2: TRadioButton;
    RadioButton3: TRadioButton;
    Button1: TButton;
    procedure Edit1KeyPress(Sender: TObject; var Key: Char);
    procedure Label3Click(Sender: TObject);
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
begin
  if key = #13 then
  begin    //если нажата клавиша Enter
           //строка из окна редактирования заносится в список выбора
     ComboBox1.Items.Add(Edit1.Text);  // добавить в список текст из первой строки
     Edit1.Text:=''; // очистка окна редактирования
  end;
end;

procedure TForm1.Label3Click(Sender: TObject);
var sum,position: Integer;
    str: String;
begin
  str:=ComboBox1.Text;
  str:=str+' ';  //алгоритм исчерпывания
  sum:=0;
  while str <> '' do
  begin
    position:=Pos(' ',str);
    if position > 1 then
      sum:=sum+1;
    Delete(str,1,position);
  end;
  Label3.Caption:=IntToStr(sum);  //переводит целый в строковый формат
end;



procedure TForm1.Button1Click(Sender: TObject);
var n,i: Integer;
    f: TextFile;
    str: String;
begin
  if RadioButton1.Checked
    then n:=0         //количество переходов на строку
    else  if RadioButton2.Checked then n:=1
                                  else n:=2;
  AssignFile(f,'In.txt'); //связь логического и физического файлов
  try                                                     // защищенный блок
    Reset(f);  //открытие для чтения, текущая первая позиция
    for i:=1 to n do ReadLn(f);  //переводит указатель на след.строку.
    Read(f,str);   //чтение элемента из файла
    Edit1.Text:=str;
    CloseFile(f);  //закрытие файла
  except
    ShowMessage('Ошибка чтения из файла');
  end;
end;

end.
