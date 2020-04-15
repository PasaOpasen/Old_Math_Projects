unit F33;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, Buttons, ExtCtrls;

type
  TForm4 = class(TForm)
    BitBtn1: TBitBtn;
    Label1: TLabel;
    Button1: TButton;
    Label2: TLabel;
    Edit1: TEdit;
    Label5: TLabel;
    Edit3: TEdit;
    Label6: TLabel;
    Edit5: TEdit;
    RadioGroup1: TRadioGroup;
    RadioButton1: TRadioButton;
    RadioButton2: TRadioButton;
    Label3: TLabel;
    Edit2: TEdit;
    procedure BitBtn1Click(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure Button1Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form4: TForm4;

implementation

uses F11;

{$R *.dfm}

procedure TForm4.BitBtn1Click(Sender: TObject);
begin
 close;
end;

procedure TForm4.FormCreate(Sender: TObject);
begin
  RadioButton1.Checked:=True;
end;

procedure TForm4.Button1Click(Sender: TObject);
var x:real;
begin
  Form1.StringGrid1.RowCount:=Form1.StringGrid1.RowCount+1;

  try x:=StrToFloat(Edit5.Text)
    except ShowMessage('ошибка ввода потребления'); Exit;
  end;
  try x:=StrToFloat(Edit1.Text)
    except ShowMessage('ошибка ввода цены'); Exit;
  end;

  Form1.StringGrid1.Cells[0,Form1.StringGrid1.RowCount-1]:= Edit2.Text;
  Form1.StringGrid1.Cells[1,Form1.StringGrid1.RowCount-1]:= Edit3.Text;
  Form1.StringGrid1.Cells[2,Form1.StringGrid1.RowCount-1]:= Edit5.Text;
  if (RadioButton1.Checked=True) then (Form1.StringGrid1.Cells[3,Form1.StringGrid1.RowCount-1]:='Да')
   else (Form1.StringGrid1.Cells[3,Form1.StringGrid1.RowCount-1]:='Нет');
  Form1.StringGrid1.Cells[4,Form1.StringGrid1.RowCount-1]:= Edit1.Text;

  Edit5.Text:='';
  Edit1.Text:='';
  Edit3.Text:='';
  Edit2.Text:='';

end;
end.
