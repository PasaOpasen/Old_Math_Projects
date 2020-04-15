program Project1;

uses
  Forms,
  Unit1 in 'Unit1.pas' {fmStGrid};

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TfmStGrid, fmStGrid);
  Application.Run;
end.
