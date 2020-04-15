unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, Menus, Buttons, Grids, StdCtrls;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    EditFIO: TEdit;
    GroupBox1: TGroupBox;
    Label2: TLabel;
    Label3: TLabel;
    Label4: TLabel;
    EditMath: TEdit;
    EditPhys: TEdit;
    EditOpus: TEdit;
    ButtonAddNewRec: TButton;
    StringGrid1: TStringGrid;
    BitBtn1: TBitBtn;
    MainMenu1: TMainMenu;
    OpenDialog1: TOpenDialog;
    SaveDialog1: TSaveDialog;
    N1: TMenuItem;
    NNew: TMenuItem;
    NOpen: TMenuItem;
    NSave: TMenuItem;
    NS: TMenuItem;
    NSort: TMenuItem;
    NSortSave: TMenuItem;
    procedure FormCreate(Sender: TObject);
    procedure NNewClick(Sender: TObject);
    procedure ButtonAddNewRecClick(Sender: TObject);
    procedure NOpenClick(Sender: TObject);
    procedure NSaveClick(Sender: TObject);
    procedure NSortClick(Sender: TObject);
    procedure NSortSaveClick(Sender: TObject);
    procedure BitBtn1Click(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
  private
    { Private declarations }
  public
    { Public declarations }
  end;
type TStudent = record
       FIO: String[40];
       otc: array [1..3] of Word;
       sball: Real;
     end;
var
  fz: File of TStudent;
  Stud: array [1..100] of TStudent;
  nzap: Integer;
  flg: Boolean;
  Form1: TForm1;

implementation

{$R *.dfm}



procedure TForm1.FormCreate(Sender: TObject);
begin
  ButtonAddNewRec.Hide;
  with StringGrid1 do
  begin
    Cells[0,0]:='Ф.И.О.';
    Cells[1,0]:='Матем.';
    Cells[2,0]:='Физика';
    Cells[3,0]:='Сочин.';
    Cells[4,0]:='Средн. балл';
  end;
  flg:=false;
end;

procedure TForm1.NNewClick(Sender: TObject);
var i: Word;
begin
  OpenDialog1.Title:='Создать новый файл';
  if OpenDialog1.Execute then
  begin
    if flg then
      CloseFile(fz);
    AssignFile(Fz,OpenDialog1.FileName);
    try
      Rewrite(Fz);
    except
      ShowMessage('Ошибка открытия файла');
      flg:=false;
      Exit;
    end;
    ButtonAddNewRec.Show;
    for i:=1 to nzap do
      with StringGrid1 do
      begin
        Cells[0,i]:='';
        Cells[1,i]:='';
        Cells[2,i]:='';
        Cells[3,i]:='';
        Cells[4,i]:='';
      end;
    StringGrid1.RowCount:=5;
    nzap:=0;
    flg:=true;
  end;
end;

procedure TForm1.ButtonAddNewRecClick(Sender: TObject);
begin
  inc(nzap);
  with Stud[nzap] do
  begin
    FIO:=EditFIO.Text;
    try
      otc[1]:=StrToInt(EditMath.Text);
      otc[2]:=StrToInt(EditPhys.Text);
      otc[3]:=StrToInt(EditOpus.Text);
    except
      ShowMessage('Некорректно заданная оценка');
      dec(nzap);
      Exit;
    end;
    sball:=(otc[1]+otc[2]+otc[3])/3;
  end;
  StringGrid1.RowCount:=nzap+1;
  with StringGrid1 do
  begin
    Cells[0,nzap]:=EditFIO.Text;
    Cells[1,nzap]:=EditMath.Text;
    Cells[2,nzap]:=EditPhys.Text;
    Cells[3,nzap]:=EditOpus.Text;
    Cells[4,nzap]:=FloatToStrF(Stud[nzap].sball,fffixed,2,1);
  end;
  EditFIO.Text:='';
  EditMath.Text:='';
  EditPhys.Text:='';
  EditOpus.Text:='';

end;

procedure TForm1.NOpenClick(Sender: TObject);
var i: Integer;
begin
  OpenDialog1.Title:='Открыть файл';
  if OpenDialog1.Execute then
  begin
    if flg then
      CloseFile(fz);
    AssignFile(fz,OpenDialog1.FileName);
    try
      Reset(fz);
    except
      ShowMessage('Ошибка открытия файла');
      flg:=false;
      Exit;
    end;
    for i:=1 to nzap do
      with StringGrid1 do
      begin
        Cells[0,i]:='';
        Cells[1,i]:='';
        Cells[2,i]:='';
        Cells[3,i]:='';
        Cells[4,i]:='';
      end;
    nzap:=0;
    while not EOF(fz) do
    begin
      inc(nzap);
      read(Fz,stud[nzap]);
      with StringGrid1,stud[nzap] do
      begin
        Cells[0,nzap]:=FIO;
        Cells[1,nzap]:=IntToStr(otc[1]);
        Cells[2,nzap]:=IntToStr(otc[2]);
        Cells[3,nzap]:=IntToStr(otc[3]);
        Cells[4,nzap]:=FloatToStrF(sball,fffixed,2,1);
      end;
    end;
    if nzap = 0 then
      StringGrid1.RowCount:=5
    else
      StringGrid1.RowCount:=nzap+1;
    ButtonAddNewRec.Show;
    flg:=true;
  end;
end;

procedure TForm1.NSaveClick(Sender: TObject);
var i: Word;
begin
  if flg then
  begin
    CloseFile(fz);
    ReWrite(fz);
    for i:=1 to nzap do
      write(fz,stud[i]);
    CloseFile(fz);
    flg:=false;
  end;
  ButtonAddNewRec.Hide;
end;

procedure TForm1.NSortClick(Sender: TObject);
var i,j: Word;
    st: TStudent;
begin
  if nzap > 0 then
  begin
    for i:=1 to nzap-1 do
      for j:=i+1 to nzap do
        if Stud[i].sball < Stud[j].sball then
        begin
          st:=stud[i];
          Stud[i]:=Stud[j];
          Stud[j]:=st;
        end;
    for i:=1 to nzap do
      with StringGrid1, stud[i] do
      begin
        Cells[0,i]:=FIO;
        Cells[1,i]:=IntToStr(otc[1]);
        Cells[2,i]:=IntToStr(otc[2]);
        Cells[3,i]:=IntToStr(otc[3]);
        Cells[4,i]:=FloatToStrF(sball,fffixed,2,1);
      end;
  end;
end;

procedure TForm1.NSortSaveClick(Sender: TObject);
var ft: TextFile;
    i: Word;
begin
  if SaveDialog1.Execute then
  begin
    AssignFile(Ft,SaveDialog1.FileName);
    try
      Rewrite(Ft);
    except
      ShowMessage('Ошибка сохранения');
      Exit;
    end;
    for i:=1 to nzap do
      with stud[i] do
        WriteLn(ft,i:4,'.',FIO,sball:8:2);
    CloseFile(ft);
  end;
end;

procedure TForm1.BitBtn1Click(Sender: TObject);
begin
  if flg then
    CloseFile(Fz);
end;

procedure TForm1.FormClose(Sender: TObject; var Action: TCloseAction);
begin
  if flg then
    CloseFile(Fz);
end;

end.
