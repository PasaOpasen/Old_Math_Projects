unit Unit1;

{$mode objfpc}{$H+}

interface

uses
  Classes, SysUtils, FileUtil, Forms, Controls, Graphics, Dialogs, StdCtrls,
  ExtCtrls, Buttons;

type

  { TForm1 }

  TForm1 = class(TForm)
    BitBtn1: TBitBtn;
    BitBtn2: TBitBtn;
    Label1: TLabel;
    Memo1: TMemo;
    procedure BitBtn1Click(Sender: TObject);
  private
    { private declarations }
  public
    { public declarations }
  end;

var
  Form1: TForm1;

implementation

{$R *.lfm}

{ TForm1 }

procedure TForm1.BitBtn1Click(Sender: TObject);
var
  MemSourceStream, MemDestStream: TMemoryStream;
begin
  MemSourceStream:= TMemoryStream.Create;
  try
    MemDestStream:= TMemoryStream.Create;
    try
      MemSourceStream.WriteComponent(BitBtn1);
      MemSourceStream.Seek(0, soFromBeginning);
      ObjectBinaryToText(MemSourceStream, MemDestStream);
      MemDestStream.Seek(0, soFromBeginning);
      Memo1.Lines.LoadFromStream(MemDestStream)
    finally
      MemDestStream.Free
    end;
  finally
    MemSourceStream.Free
  end;
end;

end.

