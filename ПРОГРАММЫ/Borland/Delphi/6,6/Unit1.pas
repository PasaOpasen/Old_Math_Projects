unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, Grids, StdCtrls;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    StringGrid1: TStringGrid;
    btnLoad: TButton;
    btnSave: TButton;
    Button3: TButton;
    Button4: TButton;
    procedure FormCreate(Sender: TObject);
    procedure Button5Click(Sender: TObject);
    procedure btnLoadClick(Sender: TObject);
    procedure Button3Click(Sender: TObject);
    procedure Button4Click(Sender: TObject);
    procedure btnSaveClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1   : TForm1;

type
  TmyRecord = record
    Numbers : String[25];
    Types   : String[25];
    Citys   : String[25];
    Times   : TDateTime;
  end;

var
  myRecord : array of TmyRecord;
  f        : File of TmyRecord;

  numbers : array[1..4] of String;
  types   : array[1..4] of String;
  citys   : array[1..4] of String;
  times   : array[1..4] of TDateTime;
  buf1,
  buf2,
  buf3    : String;
  buf4    : TDateTime;

implementation

{$R *.dfm}

procedure TForm1.FormCreate(Sender: TObject);
begin
  StringGrid1.Cells[0, 0] := '№ рейса';
  StringGrid1.Cells[1, 0] := 'Тип самолёта';
  StringGrid1.Cells[2, 0] := 'Пункт назначения';
  StringGrid1.Cells[3, 0] := 'Время вылета';
end;

procedure TForm1.Button5Click(Sender: TObject);
begin
  Form1.Close;
end;

procedure TForm1.btnLoadClick(Sender: TObject);
var
  fName : String;
//  buf   : String;
  I     : Integer;
begin
  fName := 'In.dat';
  AssignFile(f, fName);
  {$I-}
  Reset(f);
  {$I+}
  {if IOResult <> 0 then
  begin
    MessageDlg('Ошибка доступа к файлу ' + fName, mtError, [mbOk], 0);
    Label2.Caption := '1';
    Exit;
  end; }

  I := 0;
  while not Eof(f) do
  begin
    SetLength(myRecord, succ(I));
    Read(f, myRecord[I]);
    inc(I);
  end;

  for I := 0 to Length(myRecord) - 1 do
  begin
    StringGrid1.Cells[0, succ(I)] := myRecord[I].Numbers;
    StringGrid1.Cells[1, succ(I)] := myRecord[I].Types;
    StringGrid1.Cells[2, succ(I)] := myRecord[I].Citys;
    StringGrid1.Cells[3, succ(I)] := TimeToStr(myRecord[I].Times);
  end;

{  for I := 1 to 4 do
  begin
    ReadLn(f, buf);
    StringGrid1.Cells[0, I] := buf;
    readln(f, buf);
    StringGrid1.Cells[1, I] := buf;
    readln(f,buf);
    StringGrid1.Cells[2, I] := buf;
    readln(f,buf);
    StringGrid1.Cells[3, I] := buf;
  end;}

  CloseFile(f);
end;

procedure TForm1.btnSaveClick(Sender: TObject);
var
// f     : datFile;
  fName : String;
// buf   : String;
 I     : Integer;
begin
  fName := 'Out.dat';
  AssignFile(f, fName);
  Rewrite(f);

  for I := 0 to Length(MyRecord) - 1 do
  begin
    myRecord[I].Numbers := StringGrid1.Cells[0, succ(I)];
    myRecord[I].Types   := StringGrid1.Cells[1, succ(I)];
    myRecord[I].Citys   := StringGrid1.Cells[2, succ(I)];
    myRecord[I].Times   := StrToTime(StringGrid1.Cells[3, succ(I)]);
    Write(f, myRecord[I]);
  end;

{  for I := 1 to 4 do
  begin
    buf := StringGrid0.Cells[0, I];
    Write(f, buf);
    buf := StringGrid1.Cells[1, I];
    Write(f, buf);
    buf := StringGrid2.Cells[2, I];
    Write(f, buf);
    buf := StringGrid3.Cells[3, I];
    Write(f, buf);
  end;}
  Closefile(f);
  MessageDlg('Данные записаны в файл ' + fName, mtInformation, [mbOk], 4);
end;

procedure TForm1.Button3Click(Sender: TObject);
var
  max     : Integer;
  I, J, K : Integer;
begin
  btnLoad.Click;

  {if Label2.Caption = '1' then
    Exit;    }

  for I := 1 to 4 do
  begin
    numbers[I] := StringGrid1.Cells[0, I];
    types[I]   := StringGrid1.Cells[1, I];
    citys[I]   := StringGrid1.Cells[2, I];
    times[I]   := StrToTime(StringGrid1.Cells[3, I]);
  end;

  for I := 1 to 4 do
  begin
    max := I;
    for j := I + 1 to 4 do
      if times[J] > times[max] then
        max := J;

    buf1 := numbers[I];
    buf2 := types[I];
    buf3 := citys[I];
    buf4 := times[I];
    times[I]   := times[max];
    numbers[I] := numbers[max];
    types[I]   := types[max];
    citys[I]   := citys[max];
    numbers[max] := buf1;
    times[max]   := buf4;
    types[max]   := buf2;
    citys[max]   := buf3;

    for K := 1 to 4 do
    begin
      StringGrid1.Cells[0, K] := numbers[K];
      StringGrid1.Cells[1, K] := types[K];
      StringGrid1.Cells[2, K] := citys[K];
      StringGrid1.Cells[3, K] := TimeToStr(times[K]);
    end;
  end;
end;

procedure TForm1.Button4Click(Sender: TObject);
var
  min     : Integer;
  I, J, K : Integer;
begin
  btnLoad.Click;

  {if Label2.Caption = '1' then
    Exit; }

  for I := 1 to 4 do
  begin
    numbers[I] := StringGrid1.Cells[0, I];
    types[I]   := StringGrid1.Cells[1, I];
    citys[I]   := StringGrid1.Cells[2, I];
    times[I]   := StrToTime(StringGrid1.Cells[3, I]);
  end;

  for I := 1 to 4 do
  begin
    min := I;
    for J := I + 1 to 4 do
      if times[J] < times[min] then
        min := J;

    buf1 := numbers[I];
    buf2 := types[I];
    buf3 := citys[I];
    buf4 := times[I];
    times[I]   := times[min];
    numbers[I] := numbers[min];
    types[I]   := types[min];
    citys[I]   := citys[min];
    numbers[min] := buf1;
    times[min]   := buf4;
    types[min]   := buf2;
    citys[min]   := buf3;

    for K := 1 to 4 do
    begin
      StringGrid1.Cells[0, K] := numbers[k];
      StringGrid1.Cells[1, K] := types[k];
      StringGrid1.Cells[2, K] := citys[k];
      StringGrid1.Cells[3, K] := TimeToStr(times[k]);
    end;
  end;
end;

end.
