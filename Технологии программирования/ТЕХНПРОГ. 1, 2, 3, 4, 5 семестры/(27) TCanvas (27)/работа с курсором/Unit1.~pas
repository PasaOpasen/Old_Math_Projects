unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, Buttons;

type
  TForm1 = class(TForm)
    BitBtn1: TBitBtn;
    BitBtn2: TBitBtn;
    BitBtn3: TBitBtn;
    BitBtn4: TBitBtn;
    BitBtn5: TBitBtn;
    procedure BitBtn1Click(Sender: TObject);
    procedure BitBtn2Click(Sender: TObject);
    procedure BitBtn3Click(Sender: TObject);
    procedure BitBtn4Click(Sender: TObject);
    procedure BitBtn5Click(Sender: TObject);
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
begin
 SetCursorPos(600,400);
end;

procedure TForm1.BitBtn2Click(Sender: TObject);
begin
  ShowCursor(False);
end;

procedure TForm1.BitBtn3Click(Sender: TObject);
begin
  ShowCursor(True);
end;

procedure TForm1.BitBtn4Click(Sender: TObject);
begin
  SwapMouseButton(true);
end;

procedure TForm1.BitBtn5Click(Sender: TObject);
begin
  SwapMouseButton(false);
end;

end.
