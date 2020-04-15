unit F22;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, Buttons, ExtCtrls;

type
  TForm3 = class(TForm)
    BitBtn1: TBitBtn;
    Label1: TLabel;
    Button1: TButton;
    Memo1: TMemo;
    Label2: TLabel;
    Label3: TLabel;
    Label4: TLabel;
    Edit1: TEdit;
    Edit2: TEdit;
    Label5: TLabel;
    Edit3: TEdit;
    Label6: TLabel;
    Label7: TLabel;
    Label8: TLabel;
    Edit4: TEdit;
    Edit5: TEdit;
    RadioGroup1: TRadioGroup;
    RadioButton1: TRadioButton;
    RadioButton2: TRadioButton;
    SaveDialog1: TSaveDialog;
    procedure BitBtn1Click(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure Button1Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form3: TForm3;

implementation

uses F11;

{$R *.dfm}

procedure TForm3.BitBtn1Click(Sender: TObject);
begin
  close;
end;

procedure TForm3.FormCreate(Sender: TObject);
begin
  RadioButton1.Checked:=True;
end;

procedure TForm3.Button1Click(Sender: TObject);
var i,k:word;
t:textfile;
begin
  Memo1.clear;
  AssignFile(t,'результат.txt');
  Rewrite(t);
  k:=0;
  for i:=1 to Form1.StringGrid1.RowCount-1 {kol} do
  if (Edit3.Text=Form1.StringGrid1.Cells[1,i])
     and (StrToFloat(Form1.StringGrid1.Cells[2,i])>=StrToFloat(Edit4.Text))
     and (StrToFloat(Form1.StringGrid1.Cells[2,i])<=StrToFloat(Edit5.Text))
        and (((RadioButton1.Checked=True) and (Form1.StringGrid1.Cells[3,i]='Да')) or (((RadioButton2.Checked=True) and (Form1.StringGrid1.Cells[3,i]='Нет'))))
         and (StrToFloat(Form1.StringGrid1.Cells[4,i])>=StrToFloat(Edit1.Text))
         and (StrToFloat(Form1.StringGrid1.Cells[4,i])<=StrToFloat(Edit2.Text))
  then
  begin
    Memo1.Lines.Add(Form1.StringGrid1.Cells[0,i]);
    WriteLn(t,Form1.StringGrid1.Cells[0,i]);
    k:=k+1;
  end;
  if k=0 then
  begin
    WriteLn(t,'Товар не найден');
    Memo1.Lines.Add('Товар не найден');
  end;
  {Edit4.Text:='';
  Edit5.Text:='';
  Edit1.Text:='';
  Edit2.Text:='';
  Edit3.Text:='';}
  closefile(t);
end;

end.
