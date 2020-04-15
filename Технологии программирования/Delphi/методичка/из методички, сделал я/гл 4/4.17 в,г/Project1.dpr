program Project1;

uses
  Forms,
  Unit1 in 'Unit1.pas' {Form1};

{$R *.res}

begin

//SetThreadLocale(1049);
  Application.Initialize;
  Application.CreateForm(TForm1, Form1);
  Application.Run;
end.
