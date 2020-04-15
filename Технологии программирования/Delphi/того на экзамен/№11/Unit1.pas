unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, Buttons;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    BitBtn1: TBitBtn;
    BitBtn2: TBitBtn;
    Memo1: TMemo;
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

procedure TForm1.BitBtn1Click(Sender: TObject);
var
  k: byte;
  List1, List2: TStringList;
  S: string;
begin
  List1:= TStringList.Create;
  List1.Sorted:= true;
  List2:= TStringList.Create;
  for k:= 32 to 255 do
  begin
    S:= IntToStr(k);
    If k<100 then
      S:= '0' + S;
    List1.Add(Char(k) + #9 + S);
    List2.Add(S + #9 + Char(k));
  end;
  List1.Sorted:= false;
  for k:= 0 to List1.Count-1 do
    List1[k]:= List1[k] + #9 + List2[k];
  Memo1.Lines.Assign(List1);
  List1.Free;
  List2.Free;
end;

end.
