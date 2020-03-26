program FormDlg2;

uses
  Forms,
  UForm21 in 'UForm21.pas' {Form1},
  UForm12 in '..\пример 2\UForm12.pas' {Form2},
  UForm22 in 'UForm22.pas' {Form3};

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TForm1, Form1);
  Application.CreateForm(TForm2, Form2);
  Application.CreateForm(TForm3, Form3);
  Application.Run;
end.
