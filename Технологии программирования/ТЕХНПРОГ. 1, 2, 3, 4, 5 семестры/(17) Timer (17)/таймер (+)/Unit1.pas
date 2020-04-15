unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls;

type
  TForm1 = class(TForm)
    Timer1: TTimer;
    Label1: TLabel;
    Button1: TButton;
    Button2: TButton;
    procedure FormCreate(Sender: TObject);
    procedure Button1Click(Sender: TObject);
    procedure Button2Click(Sender: TObject);
    procedure Timer1Timer(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;
  sec, min: 0..60;

implementation

{$R *.dfm}
//событие, возникающее при создании формы

procedure TForm1.FormCreate(Sender: TObject);
begin
  Label1.Caption:='00:00'; //текст метки
  Timer1.Enabled:=False;  //таймер не запущен
end;

//обработка нажатия на кнопку "Пуск"/"Стоп"



procedure TForm1.Button1Click(Sender: TObject);
begin
  if Timer1.Enabled then   //Если секундомер запущен
  begin
    Timer1.Enabled:=False;         //останавливаем таймер
    Button1.Caption:= 'Пуск';     //меняем название кнопки
    Button2.Enabled:= True;        //кнопка "сброс" не дступна
  end
  else    //если секундомер не работает
  begin
    Timer1.Enabled:=True;        //запускаем таймер
    Button1.Caption:= 'Стоп';     //меняем название кнопки
    Button2.Enabled:= False;      //кнопка "сброс" не доступна
  end;
end;

//обрабатываем нажатие кнопки "сброс"


procedure TForm1.Button2Click(Sender: TObject);
begin
  sec:=0;
  min:=0;
  Label1.Caption:='00:00';
  Button2.Enabled:=False;   //кнопка "сброс" недоступна
  Timer1.Enabled:=False;     //останвливает таймер
end;

//событие, возникающее всякий раз по истечении времени , определяемого свойством Interval пока Timer1 реагирует на событие onTimer
procedure TForm1.Timer1Timer(Sender: TObject);
var str: string;
begin
  if sec=59 then
  begin
  inc(min);
  sec:=0;
  end
  else
  inc(sec);
  if min<10 then str:='0' + IntToStr(min)
  else str:=IntToStr(min);
  if sec<10 then str:=str + ':0' + IntToStr(sec)
  else str:=str + ':' + IntToStr(sec);
  Label1.Caption:=str;

end;

end.
