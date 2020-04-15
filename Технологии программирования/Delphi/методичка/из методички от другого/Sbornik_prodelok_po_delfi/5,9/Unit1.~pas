unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls, Buttons;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    Edit1: TEdit;
    ListBox1: TListBox;
    ButtonTestData: TButton;
    ButtonClearListBox: TButton;
    ButtonShowLower: TButton;
    ButtonShowUpper: TButton;
    BitBtn1: TBitBtn;
    PanelShowLower: TPanel;
    PanelShowUpper: TPanel;
    procedure Edit1KeyPress(Sender: TObject; var Key: Char);
    procedure ButtonTestDataClick(Sender: TObject);
    procedure ButtonClearListBoxClick(Sender: TObject);
    procedure ButtonShowLowerClick(Sender: TObject);
    procedure ButtonShowUpperClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

{$R *.dfm}


procedure TForm1.Edit1KeyPress(Sender: TObject; var Key: Char);  //Процедура, выполняемая при нажатии какой-либо клавиши в Edit1
begin
  if Key = #13 then  //Если нажат Enter
  begin
    ListBox1.Items.Add(Edit1.Text); //Добавляем в ListBox1 содержимое Edit1
    Edit1.Text:='Введите строку и нажмите Enter';
    ListBox1.Selected[ListBox1.Tag]:=true;  //Выделяем добавленную в ListBox1 строку
    ListBox1.Tag:=ListBox1.Tag+1; //Увеличиваем счетчик кол-ва строк на 1
  end;
end;

procedure TForm1.ButtonTestDataClick(Sender: TObject); //Процедура, выполняемая при нажатии на кнопку "Ввести тестовый пример из файла"
var f: TextFile;
    str: String;
begin
  AssignFile(f,'In.txt'); //Устанавливаем связь между файловой переменной и файлом
  try
    Reset(f);  //Открываем файл для чтения
    while not SeekEOF(f) do  //Пока не конец файла делать
    begin
      ReadLn(f,str);  //Считываем строку из файла в переменную str
      ListBox1.Items.Add(str);  //Добавляем в ListBox1 строку str
      ListBox1.Tag:=ListBox1.Tag+1; //Увеличиваем счетчик кол-ва строк на 1
    end;
    if ListBox1.Tag <> 0 then //Если кол-во строк в ListBox1 не 0
      ListBox1.Selected[ListBox1.Tag-1]:=true; //Выделяем последнюю строку в ListBox1
    CloseFile(f); //Закрываем файл
  except
    ShowMessage('Ошибка чтения из файла');
  end;
end;

procedure TForm1.ButtonClearListBoxClick(Sender: TObject); //Процедура, выполняемая при нажатии на кнопку "Очистить список строк"
begin
  ListBox1.Clear;  //Очищаем ListBox1
  ListBox1.Tag:=0; //Обнуляем счетчик кол-ва строк
end;

procedure TForm1.ButtonShowLowerClick(Sender: TObject); //Процедура, выполняемая при нажатии на кнопку "Вывести строчные русские буквы"
const lower: set of Char = ['а'..'я','ё']; //Константа-множество из всех русских строчных букв
var str,st: String;
    i: Integer;
begin
  try
    str:=ListBox1.Items[ListBox1.ItemIndex]; //Считываем в переменную str выделенную в ListBox1 строку
  except
    ShowMessage('Выберите строку!');
    Exit; //Выход из процедуры
  end;
  st:=''; //В переменной st храним все строчные русские буквы из строки str
  for i:=1 to length(str) do //Функция length(str) возвращает длину строки str
    if str[i] in lower then //Если i-тый символ из строки str принадлежит множеству всех русских строчных букв
      st:=st+str[i]; //Добавляем в строку st i-тый символ
  if st <> '' then //Если st не пуста
    PanelShowLower.Caption:=st //Выводим st
  else
    PanelShowLower.Caption:='Нет строчных русских букв!';
end;

procedure TForm1.ButtonShowUpperClick(Sender: TObject); //Процедура, выполняемая при нажатии на нопку "Вывести прописные русские буквы"
const upper: array [1..33] of Char = ('А','Б','В','Г','Д','Е','Ё','Ж','З','И','Й','К','Л', //Константа-массив из всех прописных русских букв
                                      'М','Н','О','П','Р','С','Т','У','Ф','Х','Ц','Ч','Ш',
                                      'Щ','Ъ','Ы','Ь','Э','Ю','Я');
var CharsMass: array [1..33] of Boolean; //Если, например, CharsMass[2] равен true, то строка str содержит букву "Б", CharsMass[33] равен true, то str содержит букву "Я"
    str: String;
    i,j: Integer;
begin
  try
    str:=ListBox1.Items[ListBox1.ItemIndex]; //Считываем выделенную в ListBox1 строку в переменную str
  except
    ShowMessage('Выберите строку!');
    Exit; //Выход из процедуры
  end;
  for i:=1 to 33 do //Считаем, что str не содержит ни одной прописной русской буквы
    CharsMass[i]:=false;
  for i:=1 to length(str) do
    for j:=1 to 33 do //Сравниваем i-тый элемент строки str со всеми прописными буквами
      if str[i] = upper[j] then
        CharsMass[j]:=true;
  str:=''; //Теперь в str храним в алфавитном порядке прописные русские буквы из строки str
  for i:=1 to 33 do
    if CharsMass[i] then
      str:=str+upper[i];
  if str <> '' then
    PanelShowUpper.Caption:=str
  else
    PanelShowUpper.Caption:='Нет прописных русских букв!';

end;

end.
