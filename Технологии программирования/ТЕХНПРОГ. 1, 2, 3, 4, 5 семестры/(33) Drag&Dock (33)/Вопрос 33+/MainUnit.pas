unit MainUnit;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, ComCtrls, ToolWin;

type
  TMainForm = class(TForm)
    CoolBar1: TCoolBar;
    ToolBar1: TToolBar;
    ToolButton1: TToolButton;
    ColorsBar: TToolBar;
    WhiteBtn: TToolButton;
    BlueBtn: TToolButton;
    GreenBtn: TToolButton;
    LimeBtn: TToolButton;
    PurpleBtn: TToolButton;
    RedBtn: TToolButton;
    SoundsBar: TToolBar;
    SoundBtn1: TToolButton;
    SoundBtn2: TToolButton;
    SoundBtn3: TToolButton;
    TealBtn: TToolButton;
    UndoBtn: TToolButton;
    procedure WhiteBtnClick(Sender: TObject);
    procedure BlueBtnClick(Sender: TObject);
    procedure GreenBtnClick(Sender: TObject);
    procedure LimeBtnClick(Sender: TObject);
    procedure PurpleBtnClick(Sender: TObject);
    procedure RedBtnClick(Sender: TObject);
    procedure TealBtnClick(Sender: TObject);
    procedure UndoBtnClick(Sender: TObject);
    procedure SoundBtn1Click(Sender: TObject);
    procedure SoundBtn2Click(Sender: TObject);
    procedure SoundBtn3Click(Sender: TObject);
    procedure ToolButton1Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  MainForm: TMainForm;

implementation

{$R *.dfm}

procedure TMainForm.WhiteBtnClick(Sender: TObject);
begin
 MainForm.Color :=clWhite;
end;

procedure TMainForm.BlueBtnClick(Sender: TObject);
begin
 MainForm.Color :=clBlue;
end;

procedure TMainForm.GreenBtnClick(Sender: TObject);
begin
 MainForm.Color :=clGreen;
end;

procedure TMainForm.LimeBtnClick(Sender: TObject);
begin
 MainForm.Color :=clLime;
end;

procedure TMainForm.PurpleBtnClick(Sender: TObject);
begin
 MainForm.Color :=clPurple;
end;

procedure TMainForm.RedBtnClick(Sender: TObject);
begin
 MainForm.Color :=clRed;
end;

procedure TMainForm.TealBtnClick(Sender: TObject);
begin
 MainForm.Color :=clTeal;
end;

procedure TMainForm.UndoBtnClick(Sender: TObject);
begin
 MainForm.Color :=clBtnFace;
end;

procedure TMainForm.SoundBtn1Click(Sender: TObject);
begin
 Beep;
end;

procedure TMainForm.SoundBtn2Click(Sender: TObject);
begin
 Beep;
 Beep;
end;

procedure TMainForm.SoundBtn3Click(Sender: TObject);
begin
 Beep;
 Beep;
 Beep;
end;

procedure TMainForm.ToolButton1Click(Sender: TObject);
begin
 Close;
end;

end.
