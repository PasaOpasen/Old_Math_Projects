unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, Buttons;

type
  TForm1 = class(TForm)
    Button1: TButton;
    BitBtn1: TBitBtn;
    procedure Button1Click(Sender: TObject);
   
    procedure BitBtn1Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

{$R *.dfm}

procedure TForm1.Button1Click(Sender: TObject);
Var f:TextFile;     //объ€вление файловой переменной
begin
  AssignFile(f, '2.txt'); //прив€зка нфайла к переменной
  {$I-};         //отключение контрол€ ошибок ввода-вывода
  Append(f);       // открыть файл дл€  добавлени€
  if IOResult<>0 then   //если ошибка открыти€
  begin
    {$I-};       //отключение контрол€ ошибок ввода-вывода
    Rewrite(f);    //создать новый файл
    {$I+};         //включение контрол€ ошибок ввода-вывода
    if IOResult<>0 then   //ошибка создани€ файла
    begin
      ShowMessage('ошибка создани€ файла');    //вывести строку пользователю
      Exit;         //выход
    end;
  end;
  Writeln(f, 'привет');  //запись в файл строки с в конец файла
  CloseFile(f);     //закрытие файла

end;



procedure TForm1.BitBtn1Click(Sender: TObject);
begin
  Close;
end;

end.
