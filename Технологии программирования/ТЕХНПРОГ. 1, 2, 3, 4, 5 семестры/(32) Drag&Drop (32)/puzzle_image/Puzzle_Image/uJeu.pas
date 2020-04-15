unit uJeu;

interface

uses
  Windows, Messages, SysUtils, Classes, Graphics, Controls, Forms, Dialogs,
  ExtCtrls, StdCtrls, math, Menus, MPlayer;

type
  TFrmJeu = class(TForm)
    Panel1: TPanel;
    Image: TImage;
    Button2: TButton;
    Button1: TButton;
    Button3: TButton;
    Button4: TButton;
    TrueImage: TImage;
    Edit1: TEdit;
    lblAvert: TLabel;

    procedure ImageMouseMove(Sender: TObject; Shift: TShiftState; X,
      Y: Integer);
    procedure ImageClick(Sender: TObject);
    function MoveBlock(x,y:integer):boolean;
    procedure Init;
    procedure placeBlanc(x,y:integer);
    procedure InitMatrice(x,y:integer);
    procedure Button1Click(Sender: TObject);
    procedure DessineGrille;
    procedure Button3Click(Sender: TObject);
    procedure FormShow(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure Button2Click(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    Procedure Selection(x,y:integer);
    function Comparer:boolean;
  private
    { Déclarations privées }
  public
    { Déclarations publiques }
    position:record
                   x,y:integer;
                   end;
    matrice:array[0..32] of array[0..32] of boolean;
    start:boolean;
    nomfichier:string;
    clic:boolean;
    finpartie:boolean;
    TailleGrille:integer;
  end;

var
  FrmJeu: TFrmJeu;

implementation

uses uMain;

{$R *.DFM}



function TFrmJeu.MoveBlock(x,y:integer):boolean;
var
   Taille:record
           x,y:real;
          end;
   i,j:integer;
   REctCopySource:trect;
   REctCopyDest:trect;
   destx,desty:integer;
   moveable:boolean;
begin
moveable:=false;
if x>0 then if matrice[x-1][y]=false then
   begin
   destx:=x-1;
   desty:=y;
   moveable:=true;
   end;
if x<taillegrille then if matrice[x+1][y]=false then
   begin
   destx:=x+1;
   desty:=y;
   moveable:=true;
   end;
if y>0 then if matrice[x][y-1]=false then
   begin
   destx:=x;
   desty:=y-1;
   moveable:=true;
   end;
if y<taillegrille then if matrice[x][y+1]=false then
   begin
   destx:=x;
   desty:=y+1;
   moveable:=true;
   end;
if moveable=false then
   begin
   result:=false;
   exit;
   end;
Taille.x:=(image.picture.width/taillegrille);
taille.y:=(image.picture.height/taillegrille);
rectcopySource.Left:=floor(x*taille.x);
rectcopySource.Top:=floor(y*taille.y);
rectcopySource.Right:=floor((x+1)*(taille.x));
rectcopySource.bottom:=floor((y+1)*(taille.y));
rectcopyDest.Left:=floor(destx*taille.x);
rectcopyDest.Top:=floor(desty*taille.y);
rectcopyDest.Right:=floor((destx+1)*(taille.x));
rectcopyDest.bottom:=floor((desty+1)*(taille.y));
image.Canvas.CopyRect(rectcopydest,image.canvas,rectcopysource);
matrice[destx][desty]:=true;
result:=true;
end;

procedure TfrmJeu.placeBlanc(x,y:integer);
var
   posx,posy:integer;
   top,right,left,bottom:integer;
begin
matrice[x][y]:=false;
x:=floor(x*image.picture.width/taillegrille);
y:=floor(y*image.picture.height/taillegrille);
top:=y;
bottom:=top+floor(image.picture.height/taillegrille);
left:=x;
right:=left+floor(image.picture.width/taillegrille);
image.Canvas.Brush.color:=clpurple;
image.canvas.Brush.Style:=bsSolid;
image.Canvas.Rectangle(left,top,right,bottom);
image.Canvas.Brush.color:=clred;
image.canvas.Brush.Style:=bsDiagCross;
image.Canvas.Rectangle(left,top,right,bottom);
end;

procedure TFrmJeu.ImageMouseMove(Sender: TObject; Shift: TShiftState; X,
  Y: Integer);
begin
position.x:=X;
position.y:=Y;
//timer1.enabled:=false;
//timer1.enabled:=true;
end;

procedure TFrmJeu.ImageClick(Sender: TObject);
var
   x,y:integer;
begin
clic:=true;
x:=floor(taillegrille*position.x/image.Width);
y:=floor(taillegrille*position.y/image.height);
if start=true then
   begin
   start:=false;
   image.Picture.LoadFromFile(nomfichier);
   TrueImage.Picture.LoadFromFile(nomfichier);
   Selection(x,y);
   trueimage.picture:=image.picture;
   Button1.enabled:=true;
   lblAvert.caption:='<--- Cliquer ici !!';
   exit;
   end;
if finpartie=true then exit;

if matrice[x][y]=false then
   exit;
if moveblock(x,y)=true then
   begin
   placeBlanc(x,y);
   FrmMain.edit1.text:=inttostr(Strtoint(FrmMain.Edit1.text)+1);
   end;
if comparer=true then
   begin
   EDit1.text:='Ok';
   end
else
    Edit1.text:='Pas OK';
end;

procedure TfrmJeu.DessineGrille;
var
   taille:record
                x,y:real;
                end;
   i,temp:integer;
begin
Taille.x:=(image.picture.width/taillegrille);
taille.y:=(image.picture.height/taillegrille);
for i:=1 to taillegrille-1 do
    begin
    temp:=floor(taille.x*i);
    image.canvas.moveto(temp,0);
    image.canvas.lineto(temp,image.height);
    end;
for i:=1 to taillegrille-1 do
    begin
    temp:=floor(taille.y*i);
    image.canvas.moveto(0,temp);
    image.canvas.lineto(image.width,temp);
    end;
end;

procedure TFrmJeu.Init;
var
   taille:record
                x,y:integer;
                end;
   i:integer;
begin
FrmMain.edit1.text:='0';
end;

Procedure TfrmJeu.Selection(x,y:integer);
begin
InitMatrice(x,y);
Placeblanc(x,y);
end;

procedure TFrmJeu.InitMatrice(x,y:integer);
var
   i,j:integer;
begin
for i:=0 to taillegrille do
    for j:=0 to taillegrille do
        matrice[i][j]:=true;
matrice[x,y]:=false;
end;

procedure TFrmJeu.Button1Click(Sender: TObject);
var
   i,x,y,max:integer;
begin
init;
finpartie:=false;
lblavert.visible:=false;
max:=1000*taillegrille;
if taillegrille=32 then max:=50000;
for i:=1 to max do
    begin
    x:=random(taillegrille);
    y:=random(taillegrille);
    if matrice[x][y]=false then
       next;
    if moveblock(x,y)=true then
       placeBlanc(x,y);
    end;

end;

procedure TFrmJeu.Button3Click(Sender: TObject);
begin
image.Picture.LoadFromFile('vache.bmp');
end;




procedure TFrmJeu.FormShow(Sender: TObject);
begin
init;
Dessinegrille;
end;



procedure TFrmJeu.FormClose(Sender: TObject; var Action: TCloseAction);
begin
FrmMain.close;
end;

procedure TFrmJeu.Button2Click(Sender: TObject);
begin
Dessinegrille;
end;

procedure TFrmJeu.FormCreate(Sender: TObject);
begin
finpartie:=true;
nomfichier:='vache.bmp';
start:=true;
clic:=false;
TailleGrille:=4;
lblAvert.visible:=true;
lblavert.caption:='Cliquer sur la case de départ.';
end;

function TFrmJeu.comparer:boolean;
var
   i,j:integer;
   hauteur,largeur:integer;
   couleur1,couleur2:tcolor;
   match:longint;
begin
Match:=-1;
Largeur:=trueimage.Picture.Bitmap.Width;
Hauteur:=trueimage.Picture.Bitmap.height;
clic:=false;
for i:=1 to largeur do
    begin
    for j:=1 to hauteur do
        begin
        if j/50-floor(j/50)<>0 then
         next;
        application.processmessages;
        if clic=true then
           begin
           clic:=false;
           exit;
           end;
        couleur1:=trueimage.picture.bitmap.canvas.Pixels[i,j];
        couleur2:=image.Picture.Bitmap.canvas.pixels[i,j];
        if couleur1=couleur2 then
           begin
           inc(match);
           end;
        end;
    end;
if floor((match/(i*j))*100)<98 then
   result:=false
else
    begin
    result:=true;
    finpartie:=true;
    end;
end;

end.
