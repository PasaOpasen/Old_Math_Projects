program Puzzle;

uses
  Forms,
  uMain in 'uMain.pas' {FrmMain},
  uJeu in 'uJeu.pas' {FrmJeu};

{$R *.RES}

begin
  Application.Initialize;
  Application.CreateForm(TFrmMain, FrmMain);
  Application.CreateForm(TFrmJeu, FrmJeu);
  Application.Run;
end.
