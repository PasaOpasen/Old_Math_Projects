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
    Button2: TButton;
    Label1: TLabel;
    Label2: TLabel;
    procedure Button1Click(Sender: TObject);
    procedure Button2Click(Sender: TObject);
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
var File1:TextFile;
    File2:file;
    NameFile1,NameFile2:string;

    Symbol:char;
    NSymbol:Integer;
    codes:array[1..20] of Byte;

    Buffer:array[1..20] of Char;
    i:word;
begin
  NameFile1:=Edit1.Text;
  AssignFile(File1,NameFile1);
  Reset(File1);

  NameFile2:=Edit2.Text;
  AssignFile(File2,NameFile2);
  Reset(File2,1);

  NSymbol:=0;
  BlockWrite(File2,NSymbol,4);

  Randomize;
  for i:=1 to 20 do
    Codes[i]:=random(256);
  BlockWrite(File2,Codes,NSymbol);

  i:=0;
  while not Eof(File1) do
  begin
      inc(NSymbol);
      inc(i);
      if Eoln(File1) then
      begin
        Buffer[i]:=Chr((13+Codes[i]) mod 256);
        if i=20 then
        begin
          BlockWrite(File2,Buffer,20);
          i:=0;
        end;
        inc(i);
        Buffer[i]:=Chr((10+Codes[i]) mod 256);
        ReadLn(File1);
      end
      else
      begin
        Read(File1,Symbol);
        Buffer[i]:=Chr((Ord(Symbol)+Codes[i]) mod 256);
      end;
      if i=20 then
      begin
        BlockWrite(File2,Buffer,20);
        i:=0;
      end;
  end;
  CloseFile(File1);

  if i<>0 then BlockWrite(File2,Buffer,i);
  NSymbol:=FileSize(File2)-20-4;
  Seek(File2,0);
  BlockWrite(File2,NSymbol,4);
  CloseFile(File2);
  ShowMessage('Конец Работы программы');
end;

procedure TForm1.Button2Click(Sender: TObject);
var File1:TextFile;
    File2:File;
    s:string;
begin
  AssignFile(File1,Edit1.Text);
  Reset(File1);

  AssignFile(File2,Edit2.Text);
  Rewrite(File2);

  While not Eof(File1) do
  begin
    ReadLn(File1,s);
    BlockWrite(File2,s,5);
  end;
  CloseFile(File1);
  CloseFile(File2);

  AssignFile(File1,Edit1.Text);
  Append(File1);

  AssignFile(File2,Edit2.Text);
  Reset(File2);

  While not  Eof(File2) do
  begin
    BlockRead(File2,s,5);
    Writeln(File1,s);
  end;
  CloseFile(File1);
  CloseFile(File2);
end;

end.
