unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls;

type
  TForm1 = class(TForm)
    Button1: TButton;
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

procedure TForm1.Button1Click(Sender: TObject);
var f:TextFile;      //объявление файловой переменной
    st: String;      //строковая переменная
begin
  AssignFile(f, '1.txt');    //привязка названия файла к файловой переменной
  {$I-}                     //отключение контроля ошибок ввод-вывода
  Reset(f);        // открытие файла для чтения
  {$I-}     //включение контроля ошибок ввода-вывода
  if IOResult<>0 then        //если  есть ошибка открытия
  begin
    ShowMessage('Ошибка открытия файла ');
    Exit;  //выход из процедуры при ошибке открытия файла
  end;
  While not EOF(f) do  //пока не конец файла делаем
  begin
    Readln(f,st);    //читать строку из файла
    ShowMessage(st);     //показывать строку пользователю
  end;
  CloseFile(f);     //закрыть фвйл
end;

end.
