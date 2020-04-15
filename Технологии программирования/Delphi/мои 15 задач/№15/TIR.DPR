program tir;

uses
  Forms,
  tir_ in 'tir_.pas' {Form1};

{$R *.res}

begin
  Application.Initialize;
  Application.Title := 'вш№';
  Application.CreateForm(TForm1, Form1);
  Application.Run;
end.
