unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls;

type
  TForm1 = class(TForm)
    Edit1: TEdit;
    Edit2: TEdit;
    Button1: TButton;
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

procedure TForm1.Button1Click(Sender: TObject);
var stream1,stream2:TStream;
begin
  Stream1:=TFileStream.Create(Edit1.Text,fmOpenRead or fmShareDenyWrite);
  try
    Stream2:=TFileStream.Create(Edit2.Text,fmOpenRead or fmShareDenyRead);
    try
      Stream2.CopyFrom(Stream1,Stream1.Size);
    finally
      Stream2.Free;
    end;
  finally
    Stream1.Free;
  end;
end;

end.
