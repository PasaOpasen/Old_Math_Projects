unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ComCtrls, Spin, Buttons, XPMan;

type
  TForm1 = class(TForm)
    SpinEdit1: TSpinEdit;
    ProgressBar1: TProgressBar;
    Button1: TButton;
    Button2: TButton;
    Label2: TLabel;
    Label3: TLabel;
    Label4: TLabel;
    Label5: TLabel;
    XPManifest1: TXPManifest;
    BitBtn1: TBitBtn;
    Label1: TLabel;
    procedure Button2Click(Sender: TObject);
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

procedure TForm1.Button2Click(Sender: TObject);
var
  f: File of real;
  k: integer;
  BegTime: TDateTime;
  r: real;
begin
  BegTime:= Time;
  ProgressBar1.Max:= SpinEdit1.Value;
  ProgressBar1.Position:= 0;
  ProgressBar1.Show;
  AssignFile(f,'Test.dat');
  Rewrite(f);
  for k:= 1 to SpinEdit1.Value do
  begin
    r:= random;
    write(f,r);
    Label4.Caption:= IntToStr(k);
    ProgressBar1.Position:= k;
    Application.ProcessMessages;
  end;
  CloseFile(f);
  ProgressBar1.Hide;
  Label4.Caption:= FormatDateTime('hh:mm:ss.zzz',Time - BegTime);
end;

procedure TForm1.Button1Click(Sender: TObject);
type
  PReal = ^Real;
var
  HFile, HMap: THandle;
  AdrBase, AdrReal: PReal;
  k: integer;
  FSize: Cardinal;
  BegTime: TDateTime;
begin
  BegTime:= Time;
  ProgressBar1.Max:= SpinEdit1.Value;
  ProgressBar1.Position:= 0;
  ProgressBar1.Show;
  FSize:= SizeOf(Real)*SpinEdit1.Value; // задали размер файла
  HFile:=FileCreate('Test.dat');  // создали файл
  If HFile=0 then    // ? КАК ПРИЙТИ К ЭТОМУ ИСКЛЮЧЕНИЮ
    raise Exception.Create('Ошибка создания файла');
  try // ? ЗАЧЕМ ЗДЕСЬ TRY, ЕСЛИ RAISE - ЭТО ИСКЛЮЧЕНИЕ, КОТОРОЕ ОСТАНАВЛИВАЕТ ДЕЙСТВИЕ. ЕСЛИ ЭТО НЕ ТАК, ТО ПОЧЕМУ ДО ЭТОГО НЕТ TRY
    HMap:= CreateFileMapping(HFile, NIL, PAGE_READWRITE, 0, FSize, NIL);  // ? АНАЛОГ ASSIGN
    If HMap = 0 then
      raise Exception.Create('Ошибка отображения файла');
    try
      AdrBase:= MapViewOfFile(HMap, FILE_MAP_WRITE, 0, 0, FSize); // ? АНАЛОГ RESET
      If AdrBase = NIL then
        raise Exception.Create('Невозможно просмотреть файл');
      AdrReal:= AdrBase;    // ? ЗАЧЕМ ДЕЛАТЬ ПЕРЕПРИСВОЕНИЕ
      for k:= 1 to SpinEdit1.Value do
      begin
        AdrReal^:= Random;
        AdrReal:= Pointer(Integer(AdrReal) + SizeOf(Real)); // ? ЧТО ЭТО
        Label3.Caption:= IntToStr(k);
        ProgressBar1.Position:= k;
        Application.ProcessMessages;
      end;
      UnmapViewOfFile(AdrBase)
    finally
      CloseHandle(HMap)
    end                // ? ЗАЧЕМ БЫЛО СОЗДАВАТЬ ДВА
  finally
    CloseHandle(HFile)
  end;
  ProgressBar1.Hide;
  Label3.Caption:= FormatDateTime('hh:mm:ss.zzz',Time - BegTime);
end;

end.
