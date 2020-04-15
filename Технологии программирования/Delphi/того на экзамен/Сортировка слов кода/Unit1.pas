unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, Grids, StdCtrls, Buttons, ExtCtrls;

type
  TfmStGrid = class(TForm)
    Panel1: TPanel;
    BitBtn1: TBitBtn;
    BitBtn2: TBitBtn;
    OpenDialog1: TOpenDialog;
    StringGrid1: TStringGrid;
    procedure BitBtn1Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  fmStGrid: TfmStGrid;

implementation

{$R *.dfm}

procedure TfmStGrid.BitBtn1Click(Sender: TObject);
function GetWord(var S: string): string;
{Вспомогательная функция для выделения очередного слова из строки}
const  //мно-во символов слова
  Letters: set of Char = ['a'..'z', 'A'..'Z', 'А'..'я'];
begin
  Result := '';
  //удаляем в начале строки все символы, не относящиеся к слову
  while (S <> '') and not (S[1] in Letters) do
    Delete(S, 1, 1);
  //формируем очередное слово
  while (S <> '') and (S[1] in Letters) do
  begin
    Result := Result + S[1];
    Delete(S, 1, 1);
  end;
end; //GetWord
var
  F: TextFile; //файл с текстом
  S, Word: string; //Вспомогательные строки
  NCol, NRow: Integer; //Номер текущей колонки и ттекущего ряда
  Words: TStringList; //Список отсортированных слов из файла
begin
  //С помощью стандартного диалогового окнаполучаем имя файла
  if not OpenDialog1.Execute then
    Exit;  //пользователь отказался выбрать файл
  //пытаемся открыть файл
  AssignFile(F, OpenDialog1.FileName);
  try
    Reset(F);
  except
    //файл нельзя открыть
    ShowMessage('Невозможно открыть файл ' + OpenDialog1.FileName);
    Exit;
  end;
  //Готовим список Words
  Words := TStringList.Create;
  Words.Sorted := True; //Сортируем строки
  Words.Duplicates := dupIgnore; //Отвергаем дубликаты
  //Изменяем указатель мыши перед длительной работой
  Screen.Cursor := crHourGlass;
  //Читаем файл по строкам
  while not Eof(F) do
  begin
    Readln(F, S); //Читаем очередную строку => выделяем из строки слова и заносим в Words
    while S <> '' do
    begin
      Word := GetWord(S);
      if Word <> '' then
        Words.Add(Word); //Не вставляем пустые строки
    end;
  end; //while not eof(f) do
  Screen.Cursor := crDefault; //Не вставляем пустые строки
  CloseFile(F); //Закрываем файл
  if Words.Count = 0 then
    Exit; //Пустой файл - выводим
  with StringGrid1 do
  begin
    NCol := 1; // номер первой колонки слов
    //Цикл формирования таблицы
    while Words.Count >0 do
    begin
      //Формируем// заголовок колонки и нач. значение номера ряда
      Cells[NCol, 0] := AnsiUpperCase(Words[0][1]);
      NRow := 0;
      //цикл заполнения очередной колонки
      while (Words.Count > 0) and (Cells[NCol, 0] = AnsiUpperCase(Words[0][1])) do
      begin
        inc(NRow); //Номер текущего ряда
        if NRow = RowCount then
        begin
           //расширяем таблицу вниз
           RowCount := RowCount + 1; {для св-ва нельзя исп inc!}
           Cells[0, NRow] := IntToStr(NRow);
        end;
        Cells[NCol, NRow] := Words[0];
        Words.Delete(0);
      end;
      //переходим к следующей колонке
      if Words.Count = 0 then
        Break; //кончаем работу, если слов больше нет
        inc(NCol); //переходим к следующей колонке
        ColCount := ColCount + 1; //расштряем таблицу вправо на 1 колонку
    end;
  end;
end;
end.
