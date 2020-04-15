unit uMain;

interface

uses
  Windows, Messages, SysUtils, Classes, Graphics, Controls, Forms, Dialogs,
  StdCtrls, ExtCtrls, Menus,jpeg;

type
  TFrmMain = class(TForm)
    Button1: TButton;
    Menu: TMainMenu;
    Fichier: TMenuItem;
    Ouvrir1: TMenuItem;
    convertir: TMenuItem;
    Quitter1: TMenuItem;
    OpenDialog: TOpenDialog;
    Panel1: TPanel;
    Label1: TLabel;
    Edit1: TEdit;
    Panel2: TPanel;
    Button2: TButton;
    chkSon: TCheckBox;
    cbTaille: TComboBox;
    Label2: TLabel;
    procedure Button1Click(Sender: TObject);
    procedure Button2Click(Sender: TObject);
    procedure convertirClick(Sender: TObject);
    procedure Quitter1Click(Sender: TObject);
    procedure Ouvrir1Click(Sender: TObject);
    procedure Jpg2bmp(JpgFilePath : string; BmpSavePath : string);
    procedure FormCreate(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
  private
    { Déclarations privées }
  public
    { Déclarations publiques }
    convert:boolean;
  end;

var
  FrmMain: TFrmMain;

implementation

uses uJeu;

{$R *.DFM}

procedure TFrmMain.Button1Click(Sender: TObject);
begin
FrmJeu.TailleGrille:=(cbTaille.itemindex+1)*4;
if cbtaille.itemindex=4 then FrmJeu.TailleGrille:=32;
FrmJeu.show;
end;

procedure TFrmMain.jpg2bmp(JpgFilePath : string; BmpSavePath : string);
var bmp : TBitmap;
    Jpg : TJpegImage;
begin
  bmp := TBitmap.Create;
  jpg := TJpegImage.Create;
  try
    jpg.LoadFromFile (jpgfilepath);
    bmp.Assign(jpg);
    bmp.SaveToFile (BmpSavePath + '.bmp');

  finally
    jpg.Free;
    bmp.Free;
  end;

end;

procedure TFrmMain.convertirClick(Sender: TObject);
begin
if convert=true then
   deletefile('temp.bmp');
opendialog.Filter:='*.jpg';
if opendialog.execute then
   begin
     convert:=true;
   jpg2bmp(opendialog.filename,'.\temp');

   with frmJeu do
        begin
   nomfichier:='.\temp.bmp';
   image.Picture.LoadFromFile(NomFichier);
   TrueImage.Picture.LoadFromFile(NomFichier);
   init;
   start:=true;
   Dessinegrille;
        end;
   end;
end;

procedure TFrmMain.Button2Click(Sender: TObject);
begin
if convert=true then
   deletefile('temp.bmp');
opendialog.Filter:='*.bmp';
if OpenDialog.execute then
   begin
   convert:=false;
   with frmJeu do
        begin
   nomfichier:=opendialog.filename;
   image.Picture.LoadFromFile(NomFichier);
   TrueImage.Picture.LoadFromFile(NomFichier);
   init;
   start:=true;
   Dessinegrille;
        end;
   end;
end;

procedure TFrmMain.Quitter1Click(Sender: TObject);
begin
frmJEu.close;
FrmMain.close;
end;

procedure TFrmMain.Ouvrir1Click(Sender: TObject);
begin
button2Click(self);
end;

procedure TFrmMain.FormCreate(Sender: TObject);
begin
cbTaille.ItemIndex:=0;
convert:=false;
end;

procedure TFrmMain.FormClose(Sender: TObject; var Action: TCloseAction);
begin
if convert=true then
   deletefile('temp.bmp');
end;

end.
