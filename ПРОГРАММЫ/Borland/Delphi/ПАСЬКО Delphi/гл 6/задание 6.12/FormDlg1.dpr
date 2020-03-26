program FormDlg1;

uses
  Forms,
  F11 in 'F11.pas' {Form1},
  UForm12 in 'UForm12.pas' {Form2},
  F22 in 'F22.pas' {Form3},
  F33 in 'F33.pas' {Form4};

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TForm1, Form1);
  Application.CreateForm(TForm2, Form2);
  Application.CreateForm(TForm3, Form3);
  Application.CreateForm(TForm4, Form4);
  Application.Run;
end.
