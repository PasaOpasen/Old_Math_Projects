unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    Label4: TLabel;
    Label5: TLabel;
    Label6: TLabel;
    Label7: TLabel;
    procedure Label2Click(Sender: TObject);
    procedure Label7Click(Sender: TObject);
    procedure Label4Click(Sender: TObject);
    procedure Label6Click(Sender: TObject);
    procedure Label5Click(Sender: TObject);
    procedure Label3Click(Sender: TObject);
    procedure Label1Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

{$R *.dfm}

procedure TForm1.Label2Click(Sender: TObject);
begin
Form1.Color:=clGreen;
end;

procedure TForm1.Label7Click(Sender: TObject);
begin
Form1.Color:=clBlue;
end;

procedure TForm1.Label4Click(Sender: TObject);
begin
Form1.Color:=clYellow;
end;

procedure TForm1.Label6Click(Sender: TObject);
begin
Form1.Color:=clPurple;
end;

procedure TForm1.Label5Click(Sender: TObject);
begin
Form1.Color:=clRed;
end;

procedure TForm1.Label3Click(Sender: TObject);
begin
Form1.Color:=clAqua;
end;

procedure TForm1.Label1Click(Sender: TObject);
begin
Form1.Color:=clWhite;
end;

end.
