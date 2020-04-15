unit F11;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, Grids, Buttons, Menus;

type
  TForm1 = class(TForm)
    OpenModelessForm: TButton;
    BitBtn1: TBitBtn;
    Label1: TLabel;
    Button1: TButton;
    StringGrid1: TStringGrid;
    MainMenu1: TMainMenu;
    OpenDialog1: TOpenDialog;
    SaveDialog1: TSaveDialog;
    N1: TMenuItem;
    N2: TMenuItem;
    N3: TMenuItem;
    procedure OpenModelessFormClick(Sender: TObject);
    procedure BitBtn1Click(Sender: TObject);
    procedure Button1Click(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure N2Click(Sender: TObject);
    procedure N3Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;
  kol:integer;
  fz:textfile;

implementation

uses UForm12, F22, F33;

{$R *.dfm}

procedure TForm1.OpenModelessFormClick(Sender: TObject);
begin
  Form3.Show;
end;

procedure TForm1.BitBtn1Click(Sender: TObject);
begin
  close;
end;

procedure TForm1.Button1Click(Sender: TObject);
begin
  Form4.Show;
end;

procedure TForm1.FormCreate(Sender: TObject);
begin
  with StringGrid1 do
  begin
    //TForm1.StringGrid1.
    RowCount:=8;
    Cells[0,0]:='имя'; Cells[1,0]:='период цвет.';Cells[2,0]:='кол. влаги'; Cells[3,0]:='светолюбив.'; Cells[4,0]:='цена';
    Cells[0,1]:='Трава'; Cells[1,1]:='август-сентябрь';Cells[2,1]:='120'; Cells[3,1]:='Да'; Cells[4,1]:='10';
    Cells[0,2]:='Одуванчик'; Cells[1,2]:='август-декабрь';Cells[2,2]:='120'; Cells[3,2]:='Да'; Cells[4,2]:='100';
    Cells[0,3]:='Пшеница'; Cells[1,3]:='август-сентябрь';Cells[2,3]:='300'; Cells[3,3]:='Да'; Cells[4,3]:='150';
    Cells[0,4]:='Капуста'; Cells[1,4]:='август-сентябрь';Cells[2,4]:='150'; Cells[3,4]:='Да'; Cells[4,4]:='200';
    Cells[0,5]:='Укроп'; Cells[1,5]:='август-сентябрь';Cells[2,5]:='100'; Cells[3,5]:='Да'; Cells[4,5]:='200';
    Cells[0,6]:='Перец'; Cells[1,6]:='август-сентябрь';Cells[2,6]:='90'; Cells[3,6]:='Да'; Cells[4,6]:='300';
    Cells[0,7]:='Репа'; Cells[1,7]:='июль-октябрь';Cells[2,7]:='110'; Cells[3,7]:='Нет'; Cells[4,7]:='350';
  end;
end;

//добавить из файла
procedure TForm1.N2Click(Sender: TObject);
var a:string; n,i,j,k:integer;
begin
 if OpenDialog1.Execute then
  begin
    AssignFile(fz,OpenDialog1.Filename);
    Reset(fz);
    ReadLn(fz,n);
    k:= StringGrid1.RowCount;
    StringGrid1.RowCount{kol}:=StringGrid1.RowCount+n;

    with StringGrid1 do
    begin
      for j:=k to StringGrid1.RowCount-1 do
         for i:=0 to 4 do
         begin
           ReadLn(fz,a); Cells[i,j]:=a;
         end;
     end;

  closefile(fz);
  end;
end;


 //прочесть заново
procedure TForm1.N3Click(Sender: TObject);
var a:string; n,i,j:integer;
begin
  if OpenDialog1.Execute then
  begin
    AssignFile(fz,OpenDialog1.Filename);
    Reset(fz);
    Readln(fz,n);

    StringGrid1.RowCount{kol}:= n + 1;
    with StringGrid1 do
    begin
      for j:=1 to RowCount-1 do
         for i:=0 to 4 do
         begin
           Readln(fz,a); Cells[i,j]:=a;
         end;
    end;

  closefile(fz);
  end;
end;

end.
