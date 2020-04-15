unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, Buttons;

type
  TForm1 = class(TForm)
    Memo1: TMemo;
    Button1: TButton;
    Memo2: TMemo;
    BitBtn1: TBitBtn;
    procedure Button1Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

type
  PDouble = ^Double;

var
  Form1: TForm1;

implementation

{$R *.dfm}

function Srt(Item1, Item2: Pointer): Integer;
begin
  If PDouble(Item1)^ < PDouble(Item2)^ then
    Result:= -1
  Else If PDouble(Item1)^ > PDouble(Item2)^ then
    Result:= 1
  Else
    Result:= 0
end;

procedure TForm1.Button1Click(Sender: TObject);
var
  k: Integer;
  List: TList;
  pD: Pdouble;
begin
  List := TList.Create;
  List.Capacity:= 20; // задаем ёмкость списка
  for k:= 1 to 20 do
  begin
    New(pD);//создали указатель
    pD^:= Random;
    List.Add(pD);//записали указатель
  end;
  List.Sort(Srt);
  Memo1.Clear;
  for k:= 0 to List.Count-1 do
  begin
    pd:= List[k];
    Memo1.Lines.Add(FloatToStr(pd^));
    Dispose(pd);
  end;
  List.Free;
end;

end.
