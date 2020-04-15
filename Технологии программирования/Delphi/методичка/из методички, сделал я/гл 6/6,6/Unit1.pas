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
    Edit1: TEdit;
    Button1: TButton;
    procedure FormCreate(Sender: TObject);
    procedure Button5Click(Sender: TObject);
    procedure btnLoadClick(Sender: TObject);
    procedure Button3Click(Sender: TObject);
    procedure Button4Click(Sender: TObject);
    procedure btnSaveClick(Sender: TObject);
    procedure Button1Click(Sender: TObject);
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
  ff:textfile;

  numbers : array[1..4] of String;
  types   : array[1..4] of String;
  citys   : array[1..4] of String;
  times   : array[1..4] of TDateTime;
  buf1,
  buf2,
  buf3    : String;
  buf4    : TDateTime;
  constant:integer;

implementation

{$R *.dfm}

procedure TForm1.FormCreate(Sender: TObject);
begin
  StringGrid1.Cells[0, 0] := '№ рейса';
  StringGrid1.Cells[1, 0] := 'Тип самолёта';
  StringGrid1.Cells[2, 0] := 'Пункт назначения';
  StringGrid1.Cells[3, 0] := 'Время вылета';
  constant:=4;
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
   constant:=4;
  CloseFile(f);
end;

procedure TForm1.btnSaveClick(Sender: TObject);
var
// f     : datFile;
  fName : String;
// buf   : String;
 I     : Integer;
begin
  fName := 'Out.txt';
  AssignFile(ff, fName);
  Rewrite(ff);

  for I := 0 to constant{Length(MyRecord) - 1} do
  begin
    {myRecord[I].Numbers := StringGrid1.Cells[0, succ(I)];
    myRecord[I].Types   := StringGrid1.Cells[1, succ(I)];
    myRecord[I].Citys   := StringGrid1.Cells[2, succ(I)];
    myRecord[I].Times   := StrToTime(StringGrid1.Cells[3, succ(I)]);
    Write(f, myRecord[I]); }
      Write(ff,StringGrid1.Cells[0, i]); Write(ff,'  ');
      Write(ff,StringGrid1.Cells[1, i]); Write(ff,'  ');
      Write(ff,StringGrid1.Cells[2, i]); Write(ff,'  ');
      Write(ff,StringGrid1.Cells[3, i]); WriteLn(ff,'  ');
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
  Closefile(ff);
  MessageDlg('Данные записаны в файл ' + fName, mtInformation, [mbOk], constant);
end;

procedure TForm1.Button3Click(Sender: TObject);
var
  max     : Integer;
  I, J, K : Integer;
begin
  //btnLoad.Click;
  {if Label2.Caption = '1' then
    Exit;    }

  {for I := 1 to constant do
  begin
    numbers[I] := StringGrid1.Cells[0, I];
    types[I]   := StringGrid1.Cells[1, I];
    citys[I]   := StringGrid1.Cells[2, I];
    times[I]   := StrToTime(StringGrid1.Cells[3, I]);
  end;

  for I := 1 to constant do
  begin
    max := I;
    for j := I + 1 to constant do
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

    for K := 1 to constant do
    begin
      StringGrid1.Cells[0, K] := numbers[K];
      StringGrid1.Cells[1, K] := types[K];
      StringGrid1.Cells[2, K] := citys[K];
      StringGrid1.Cells[3, K] := TimeToStr(times[K]);
    end;
  end; }
     for I := 1 to constant do
  begin
    numbers[I] := StringGrid1.Cells[0, I];
    types[I]   := StringGrid1.Cells[1, I];
    citys[I]   := StringGrid1.Cells[2, I];
    times[I]   := StrToTime(StringGrid1.Cells[3, I]);
  end;

  for I := 1 to constant do
  begin
    max := I;
    for J := I + 1 to constant do
      if StrToTime(StringGrid1.Cells[3, j]) > StrToTime(StringGrid1.Cells[3, max]) then
        max := J;

    buf1 := StringGrid1.Cells[0, I];
    buf2 := StringGrid1.Cells[1, I];
    buf3 := StringGrid1.Cells[2, I];
    buf4 := StrToTime(StringGrid1.Cells[3, I]);
    StringGrid1.Cells[1, I]:= StringGrid1.Cells[1, max];
    StringGrid1.Cells[0, I]:= StringGrid1.Cells[0, max];
    StringGrid1.Cells[3, I]:=StringGrid1.Cells[3, max];
    StringGrid1.Cells[2, I]:= StringGrid1.Cells[2, max];
    StringGrid1.Cells[0, max]:= buf1;
    StringGrid1.Cells[3, max]:= TimeToStr(buf4);
    StringGrid1.Cells[1, max]:= buf2;
    StringGrid1.Cells[2, max]:= buf3;

    {for K := 1 to constant do
    begin
      StringGrid1.Cells[0, K] := numbers[k];
      StringGrid1.Cells[1, K] := types[k];
      StringGrid1.Cells[2, K] := citys[k];
      StringGrid1.Cells[3, K] := TimeToStr(times[k]);
    end;   }
  end;
end;

procedure TForm1.Button4Click(Sender: TObject);
var
  min     : Integer;
  I, J, K : Integer;
begin
  //btnLoad.Click;

  {if Label2.Caption = '1' then
    Exit; }


  for I := 1 to constant do
  begin
    numbers[I] := StringGrid1.Cells[0, I];
    types[I]   := StringGrid1.Cells[1, I];
    citys[I]   := StringGrid1.Cells[2, I];
    times[I]   := StrToTime(StringGrid1.Cells[3, I]);
  end;

  for I := 1 to constant do
  begin
    min := I;
    for J := I + 1 to constant do
      if StrToTime(StringGrid1.Cells[3, j]) < StrToTime(StringGrid1.Cells[3, min]) then
        min := J;

    buf1 := StringGrid1.Cells[0, I];
    buf2 := StringGrid1.Cells[1, I];
    buf3 := StringGrid1.Cells[2, I];
    buf4 := StrToTime(StringGrid1.Cells[3, I]);
    StringGrid1.Cells[1, I]:= StringGrid1.Cells[1, min];
    StringGrid1.Cells[0, I]:= StringGrid1.Cells[0, min];
    StringGrid1.Cells[3, I]:=StringGrid1.Cells[3, min];
    StringGrid1.Cells[2, I]:= StringGrid1.Cells[2, min];
    StringGrid1.Cells[0, min]:= buf1;
    StringGrid1.Cells[3, min]:= TimeToStr(buf4);
    StringGrid1.Cells[1, min]:= buf2;
    StringGrid1.Cells[2, min]:= buf3;

    {for K := 1 to constant do
    begin
      StringGrid1.Cells[0, K] := numbers[k];
      StringGrid1.Cells[1, K] := types[k];
      StringGrid1.Cells[2, K] := citys[k];
      StringGrid1.Cells[3, K] := TimeToStr(times[k]);
    end;   }
  end;
end;

procedure TForm1.Button1Click(Sender: TObject);
var s:string; mas:array [1..4] of integer; k,i:integer;
begin
  s:=Edit1.Text;
  i:=0;
  for k:=1 to constant do
  begin
   if s=StringGrid1.Cells[2, K] then
   begin
     i:=i+1;
     mas[i]:=k;
   end;
  end;

  for K := 1 to i do
    begin
      StringGrid1.Cells[0, K] := StringGrid1.Cells[0, mas[k]];
      StringGrid1.Cells[1, K] := StringGrid1.Cells[1, mas[k]];
      StringGrid1.Cells[2, K] := StringGrid1.Cells[2, mas[k]];
      StringGrid1.Cells[3, K] := StringGrid1.Cells[3, mas[k]];
    end;

if i<constant then
begin
  for k:=i+1 to constant do
  begin
      StringGrid1.Cells[0, K] := '';
      StringGrid1.Cells[1, K] := '';
      StringGrid1.Cells[2, K] := '';
      StringGrid1.Cells[3, K] := '';
  end;
  constant:=i;
end;

end;

end.
