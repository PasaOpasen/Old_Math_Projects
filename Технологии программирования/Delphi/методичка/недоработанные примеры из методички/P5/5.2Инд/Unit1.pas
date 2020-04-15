unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, Buttons, StdCtrls;

type
  TForm1 = class(TForm)
    Edit1: TEdit;
    ListBox1: TListBox;
    ButtonTestData: TButton;
    ButtonClearListBox: TButton;
    BitBtn1: TBitBtn;
    ButtonInt: TButton;
    ButtonFloat1: TButton;
    LabelInt: TLabel;
    LabelFloat1: TLabel;
    procedure Edit1KeyPress(Sender: TObject; var Key: Char);
    procedure ButtonTestDataClick(Sender: TObject);
    procedure ButtonClearListBoxClick(Sender: TObject);
    procedure ButtonIntClick(Sender: TObject);
    procedure ButtonFloat1Click(Sender: TObject);
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

procedure TForm1.Edit1KeyPress(Sender: TObject; var Key: Char);
begin
  if Key = #13 then  //Если нажата клавиша Enter
  begin
    ListBox1.Items.Add(Edit1.Text); //Добавляем в ListBox1 строку
    Edit1.Text:='Введите строку и нажмите Enter';
//Свойство Tag объекта ListBox1 используем для хранения кол-ва строк в ListBox1
    ListBox1.Selected[ListBox1.Tag]:=true; //Выбираем в ListBox1 последнюю строку
    ListBox1.Tag:=ListBox1.Tag+1;
  end;
end;

procedure TForm1.ButtonTestDataClick(Sender: TObject);
var f: TextFile;
    str: String;
begin
  AssignFile(f,'In.txt'); //Устанавливаем связь между файловой переменной и файлом
  try
    Reset(f); //Открываем файл для чтения
    while not SeekEOF(f) do //Пока не конец файла
    begin
      ReadLn(f,str);  //Считываем строку из файла в переменную str
      ListBox1.Items.Add(str);  //Добавляем строку в ListBox1
      ListBox1.Tag:=ListBox1.Tag+1; //Увеличиваем счетчик числа строк
    end;
    if ListBox1.Tag <> 0 then //Если в listBox1 есть хотябы одна строка
      ListBox1.Selected[ListBox1.Tag-1]:=true; //Выбираем последнюю строку
    CloseFile(f); //Закрываем файл
  except
    ShowMessage('Ошибка чтения из файла!');
  end;
end;

procedure TForm1.ButtonClearListBoxClick(Sender: TObject);
begin
  ListBox1.Clear;  //Очищаем ListBox1
  ListBox1.Tag:=0; //Обнуляем счетчик кол-ва строк
end;

procedure TForm1.ButtonIntClick(Sender: TObject);
const symb: Set of Char = ['+','-','0'..'9']; //Константа-множество, состоящее из +,-,0,1,2,3,4,5,6,7,8,9
var str,st: String;
    i: Integer;
    flag: Boolean;
begin
  try
    str:=ListBox1.Items[ListBox1.ItemIndex];  //Запоминаем в str выделенную в ListBox строку
  except
    ShowMessage('Выберите строку!');
    Exit; //Выход из процедуры
  end;
  str:=str+' '; //Для считывания последнего числа из строки str необходимо наличие за этим числом какого-либо символа
  st:=''; //В st храним набор символов, который предположительно является числом
  flag:=false; //Если flag равен true, то st содержит набор символов, являющийся, предположительно, числом
  LabelInt.Caption:='';
  for i:=1 to length(str) do  //Функция Length(str) возвращает длину строки str
    if str[i] in symb then  //Если i-тый символ из строки str содержится в константе-множестве symb
    begin
      st:=st+str[i];
      flag:=true;
    end else
      if flag then //Если st содержит набор символов
      begin
//Пытаемся сконвертировать содержимое строки st в целое число
//Если успешно, то выводим это целое число
        try
          StrToInt(st);
          LabelInt.Caption:=LabelInt.Caption+' '+st;
        except
        end;
        flag:=false;
        st:='';
      end;
  if LabelInt.Caption = '' then
    LabelInt.Caption:='Строка не содержит целых чисел';
end;

procedure TForm1.ButtonFloat1Click(Sender: TObject);
const symb: Set of Char = ['.','+','-','0'..'9'];
var str,st: String;
    i: Integer;
    flag,flag2: Boolean;
begin
  try
    str:=ListBox1.Items[ListBox1.ItemIndex];
  except
    ShowMessage('Выберите строку!');
    Exit;
  end;
  str:=str+' ';
  st:='';
  flag:=false;
  flag2:=false;  //Если flag2 равен true, то строка st содержит точку
  LabelFloat1.Caption:='';
  for i:=1 to length(str) do
    if str[i] in symb then
    begin
      st:=st+str[i];
      flag:=true;
      if str[i] = '.' then
        flag2:=true;
    end else
      if flag then
      begin
        if flag2 then //Если строка st содержит точку
          try
            StrToFloat(st);
            LabelFloat1.Caption:=LabelFloat1.Caption+' '+st;
          except
          end;
        flag:=false;
        flag2:=false;
        st:='';
      end;
  if LabelFloat1.Caption = '' then
    LabelFloat1.Caption:='Строка не содержит веществ. чисел с фиксированной точкой';
end;

procedure TForm1.FormCreate(Sender: TObject);
begin
  DecimalSeparator:='.';  //Устанавливаем в качестве разделителя целой и дробной частей - точку
end;

end.
