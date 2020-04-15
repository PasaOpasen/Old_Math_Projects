unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, Menus, Buttons, Grids, StdCtrls, ActnList;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    Edit1: TEdit;
    GroupBox1: TGroupBox;
    Label2: TLabel;
    Label3: TLabel;
    Label4: TLabel;
    Edit2: TEdit;
    Edit3: TEdit;
    Edit4: TEdit;
    Button1: TButton;
    StringGrid1: TStringGrid;
    BitBtn1: TBitBtn;
    OpenDialog1: TOpenDialog;
    SaveDialog1: TSaveDialog;
    MainMenu1: TMainMenu;
    Fil: TMenuItem;
    new: TMenuItem;
    opn: TMenuItem;
    sv: TMenuItem;
    s: TMenuItem;
    srt: TMenuItem;
    svsrt: TMenuItem;
    ActionList1: TActionList;
    Action1: TAction;
    Action2: TAction;
    Action3: TAction;
    Action4: TAction;
    Action5: TAction;
    procedure FormCreate(Sender: TObject);
    procedure newClick(Sender: TObject);
    procedure Button1Click(Sender: TObject);
    procedure opnClick(Sender: TObject);
    procedure svClick(Sender: TObject);
    procedure srtClick(Sender: TObject);
    procedure svsrtClick(Sender: TObject);
    procedure BitBtn1Click(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure Action1Execute(Sender: TObject);
    procedure Action2Execute(Sender: TObject);
    procedure Action3Execute(Sender: TObject);
    procedure Action4Execute(Sender: TObject);
    procedure Action5Execute(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;
type TStudent = packed record
       FIO: string[40];  //Поле Ф.И.О.
       otc: array[1..3] of Real; //Поле массива оценок
       sball: Real; //Поле среднего балла
     end;
var
  Form1: TForm1;
  Fz: file of TStudent;  //Файл типа запись
  Ft: TextFile; //Текстовый файл
  Stud: array[1..100] of TStudent; //Массив записей
  flg: Boolean;
  nzap: Integer; //номер записи
implementation

{$R *.dfm}

procedure TForm1.FormCreate(Sender: TObject);
begin
  Button1.Hide;//Сделать невидимой кнопку Ввести запись
  //Заполнение заголовком полей записи в таблице
  with StringGrid1 do
  begin
    Cells[0, 0] := 'Ф.И.О.'; Cells[1, 0] := 'Математика';
    Cells[2, 0] := 'Физика'; Cells[3, 0] := 'Сочинение';
    Cells[4, 0] := 'Средний балл';
  end;
  flg := False; //Типизированный файл Fz закрыт
end;
//Выполнение пункта меню Файл->Создать
procedure TForm1.newClick(Sender: TObject);
var
  i: Word;
begin
  OpenDialog1.Title := 'Создать новый файл';
  if OpenDialog1.Execute then
  begin
    //Связь файловой переменной с физическим файлом, выбранным в результате диалога
    AssignFile(Fz, OpenDialog1.FileName);
    Rewrite(Fz); //Открыть файл для записи
    Button1.Show; //Сделать видимой кнопку Ввести запись
    nzap := 0; //Начальное значение счетчика номера записи
    //Очистка ячеек таблицы от старых данных
    for i := 1 to nzap do
      with StringGrid1 do
      begin
        Cells[0, i] := ''; Cells[1, i] := '';
        Cells[2, i] := ''; Cells[3, i] := ''; Cells[4, i] := '';
      end;
    flg := True; //Типизированный файл Fz открыт
  end;
end;
//Кнопка ввести запись
procedure TForm1.Button1Click(Sender: TObject);
begin
  inc(nzap); //Накопление счетчика
  with Stud[nzap] do
  begin
    try FIO := Edit1.Text
      except ShowMessage('Ф.И.О. введены не правильно');
      Dec(nzap);
      exit;
    end;
    try otc[1] := StrToFloat(Edit2.Text)
      except ShowMessage('Оценка за математику введена не правильно');
      Dec(nzap);
      exit;
    end;
    try otc[2] := StrToFloat(Edit3.Text)
      except ShowMessage('Оценка за физику введена не правильно');
      Dec(nzap);
      exit;
    end;
    try otc[3] := StrToFloat(Edit4.Text)
      except ShowMessage('Оценка за сочинение введена не правильно');
      Dec(nzap);
      exit;
    end;
    FIO := Edit1.Text;
    otc[1] := StrToInt(Edit2.Text);
    otc[2] := StrToInt(Edit3.Text);
    otc[3] := StrToInt(Edit4.Text);
    sball := (otc[1] + otc[2] + otc[3])/3;
  end;
  with StringGrid1 do
  begin //заполнение таблицы
    Cells[0, nzap] := Edit1.Text;
    Cells[1, nzap] := FloatToStrF(Stud[nzap].otc[1], fffixed, 2, 1);
    Cells[2, nzap] := FloatToStrF(Stud[nzap].otc[2], fffixed, 2, 1);
    Cells[3, nzap] := FloatToStrF(Stud[nzap].otc[3], fffixed, 2, 1);
    Cells[4, nzap] := FloatToStrF(Stud[nzap].sball, fffixed, 2, 1);
  end;
  write(fz, Stud[nzap]); //запись в файл соответствующей записи
  //Очистка окон ввода для новых данных
  Edit1.Text := ''; Edit2.Text := ''; Edit3.Text := ''; Edit4.Text := '';
end;
//Выполнение пункта меню Файл -> Открыть
procedure TForm1.opnClick(Sender: TObject);
begin
  if OpenDialog1.Execute then //Если результат диалога был успешным
  begin
    //Связь файловой переменной с физическим файломб выбранным в результате диалога
    AssignFile(Fz, OpenDialog1.FileName);
    Reset(Fz); //Открыть файл для чтения
    nzap := 0; //Начальное значение счетчика номера записи
    while not Eof(Fz) do
    begin //Чтение записи из файла в массив и в таблицу
      inc(nzap);
      read(fz, stud[nzap]);
      with StringGrid1, Stud[nzap] do
      begin
        Cells[0, nzap] := FIO;
        Cells[1, nzap] := FloatToStrF(otc[1], fffixed, 2, 1);
        Cells[2, nzap] := FloatToStrF(otc[2], fffixed, 2, 1);
        Cells[3, nzap] := FloatToStrF(otc[3], fffixed, 2, 1);
        Cells[4, nzap] := FloatToStrF(sball, fffixed, 2, 1);
      end;
    end;
    Button1.Show; //Сделать видимой кнопку ввести запись
  end;
end;
//Выполнение пункта меню файл -> Сохранить
procedure TForm1.svClick(Sender: TObject);
begin
  if flg then
  begin
    CloseFile(Fz);
    flg := False;
  end;
  Button1.Hide; //Сделать невидимой кнопку Ввести запись
end;
//Выполнение пункта меню Сортировка-> Сортировать
procedure TForm1.srtClick(Sender: TObject);
var
  i, j: Word;
  st: TStudent;
begin
  if nzap > 0 then //Если заполнена хотя бы одна запись
  begin //Сортировать массив записей
    for i := 1 to nzap - 1 do
      for j := i + 1 to nzap do
        if Stud[i].sball < Stud[j].sball then
        begin
          st := Stud[i];
          Stud[i] := Stud[j];
          Stud[j] := st;
        end;
    for i := 1 to nzap do //Заполнение таблицы элементами массива
      with StringGrid1, stud[i] do
      begin
        Cells[0, i] := FIO;
        Cells[1, i] := FloatToStrF(otc[1], fffixed, 2, 1);
        Cells[2, i] := FloatToStrF(otc[2], fffixed, 2, 1);
        Cells[3, i] := FloatToStrF(otc[3], fffixed, 2, 1);
        Cells[4, i] := FloatToStrF(sball, fffixed, 2, 1);
      end;
  end;
end;
//Выполнение пункта Сортировка -> Сохранить
procedure TForm1.svsrtClick(Sender: TObject);
var
  i: Word;
begin
  if SaveDialog1.Execute then // Если результат диалога был успешен
  begin
    //Связь файловой переменной с физическим файлом, выбранным в результате диалога
    AssignFile(Ft, SaveDialog1.FileName);
    Rewrite(Ft); //Открыть текстовый файл для записи
    for i := 2 to nzap do
      with Stud[i] do
        write(ft, i:4, '.', fio, sball:8:2);
    CloseFile(Ft);
  end;
end;

procedure TForm1.BitBtn1Click(Sender: TObject);
begin
  if flg then
    CloseFile(Fz);
end;
//Кнопка Х находящаяся в правом верхнем углу формы
procedure TForm1.FormClose(Sender: TObject; var Action: TCloseAction);
begin
  if flg then
    CloseFile(Fz);
end;
procedure TForm1.Action1Execute(Sender: TObject);
begin
  newClick(Sender);
end;

procedure TForm1.Action2Execute(Sender: TObject);
begin
  opnClick(Sender);
end;

procedure TForm1.Action3Execute(Sender: TObject);
begin
  svClick(Sender);
end;

procedure TForm1.Action4Execute(Sender: TObject);
begin
  srtClick(Sender);
end;

procedure TForm1.Action5Execute(Sender: TObject);
begin
  svsrtClick(Sender);
end;

end.
