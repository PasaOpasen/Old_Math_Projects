unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls, Grids, Menus;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    Edit1: TEdit;
    Label2: TLabel;
    Edit2: TEdit;
    Button1: TButton;
    StringGrid1: TStringGrid;
    StringGrid2: TStringGrid;
    Button2: TButton;
    RadioGroup1: TRadioGroup;
    RadioButton1: TRadioButton;
    RadioButton2: TRadioButton;
    RadioButton3: TRadioButton;
    MainMenu1: TMainMenu;
    N1: TMenuItem;
    OpenDialog1: TOpenDialog;
    N2: TMenuItem;
    SaveDialog1: TSaveDialog;
    procedure FormCreate(Sender: TObject);
    procedure Button1Click(Sender: TObject);
    procedure Button2Click(Sender: TObject);
    procedure N1Click(Sender: TObject);
    procedure N2Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

type Matr=array [1..100,1..100] of real;
     Vect=array [1..100] of real;
var
  Form1: TForm1;
  n,m:integer;
  A:Matr;
  B:Vect;

implementation

{$R *.dfm}

procedure TForm1.FormCreate(Sender: TObject);
var i,j:integer;
begin
Edit1.Text:='3';
Edit2.Text:='3';
m:=3;
n:=3;
RadioButton1.Checked:=true;
StringGrid1.Cells[0,0]:='A';
StringGrid2.Cells[0,0]:='B';
for i:=1 to StrToInt(edit1.Text) do
 begin
   StringGrid1.Cells[0,i]:=IntToStr(i);
   StringGrid1.Cells[i,0]:=IntToStr(i);
 end;

end;

procedure TForm1.Button1Click(Sender: TObject);
var i,j:integer;
begin
 n:=StrToInt(Edit1.Text);
 m:=StrToInt(Edit2.Text);
 StringGrid1.RowCount:=n+1;
 StringGrid2.RowCount:=n+1;
 StringGrid1.ColCount:=m+1;
 for i:=1 to n do StringGrid1.Cells[0,i]:=IntToStr(i);
 for i:=1 to m do StringGrid1.Cells[i,0]:=IntToStr(i);


end;

procedure TForm1.Button2Click(Sender: TObject);
var i,j,k:integer;
     temp:boolean;
     max:real;
begin
 for i:=1 to n do
  for j:=1 to m do
    A[i,j]:=StrToFloat(StringGrid1.Cells[j,i]);

  if RadioButton1.Checked then
   begin

    for i:=1 to n do
     begin
      temp:=true;
      for j:=1 to m do
        if A[j,i]<>0 then
         begin
           temp:=false;
           //break;
         end;
      if temp then StringGrid2.Cells[0,i]:='0'
      else StringGrid2.Cells[0,i]:='1';
     end;

   end;

  if RadioButton2.Checked then
   begin
     for i:=1 to n do
      begin
        temp:=true;
        max:=a[1,i];
        for j:=1 to m do
          if a[j,i]<=max then max:=a[j,i]  //равенство
          else temp:=false;
        if temp then StringGrid2.Cells[0,i]:='1'
        else StringGrid2.Cells[0,i]:='0';
      end;
   end;

   if RadioButton3.Checked then
   begin
    for i:=1 to n do
     begin
      temp:=true;
      for j:=1 to m div 2 do
       begin
        if A[i,j]<>A[i,m+1-j] then
         begin
          temp:=false;
          break;
         end;

       end;
      if temp then StringGrid2.Cells[0,i]:='1'
      else StringGrid2.Cells[0,i]:='0';
     end;

   end;



end;

procedure TForm1.N1Click(Sender: TObject);
var txt:TextFile;
    i,j:integer;
    scan:real;
begin
 if OpenDialog1.Execute then
  begin
    assignFile(txt,OpenDialog1.FileName);
    reset(txt);
    read(txt,n);
    read(txt,m);
    StringGrid1.RowCount:=n+1;
    StringGrid2.RowCount:=n+1;
    StringGrid1.ColCount:=m+1;
    for i:=1 to n do
     begin
      for j:=1 to m do
        begin
          read(txt,scan);
          StringGrid1.Cells[j,i]:=FLoatToStr(scan);
        end;
     end;
  end;
end;

procedure TForm1.N2Click(Sender: TObject);
var txt:textFile;
    i,j:integer;
begin
 if saveDialog1.Execute then
  begin
    assignFile(txt,SaveDialog1.FileName);
    rewrite(txt);
    for i:=1 to n do
      begin
        for j:=1 to m do write(txt,StringGrid1.Cells[j,i]:6);
        write(txt,'|':6,StringGrid2.Cells[0,i]:2);
        writeln(txt);
      end;
    CloseFile(txt);
  end;
end;

end.
