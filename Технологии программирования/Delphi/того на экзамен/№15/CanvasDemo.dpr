program CanvasDemo;

uses
  Forms,
  CanvasU in 'CanvasU.pas' {fmExample};

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TfmExample, fmExample);
  Application.Run;
end.
