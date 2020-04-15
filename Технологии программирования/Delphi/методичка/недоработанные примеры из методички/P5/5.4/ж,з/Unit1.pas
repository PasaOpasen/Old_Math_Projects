unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls;

type
  TForm1 = class(TForm)
    Edit1: TEdit;
    ComboBox1: TComboBox;
    RadioGroup1: TRadioGroup;
    Label1: TLabel;
    Label2: TLabel;
    Edit2: TEdit;
    Edit3: TEdit;
    Label3: TLabel;
    Label4: TLabel;
    procedure Edit1KeyPress(Sender: TObject; var Key: Char);
    procedure ComboBox1Click(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure RadioGroup1Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

{$R *.dfm}

procedure TForm1.Edit1KeyPress(Sender: TObject; var Key: Char);
var st:string;
    i:integer;
begin
  if key=#13 then
    begin
      st:=Edit1.Text+' ';

      while (st[1]=' ') do delete(st,1,1);

      i:=1;
      while (i<=length(st)) do
        begin
          if (st[i-1]=' ') and (st[i]=' ') then delete(st,i,1)
          else inc(i);
        end;

      ComboBox1.Items.Add(st);
      Edit1.Text:='';
    end;
end;


procedure TForm1.ComboBox1Click(Sender: TObject);
var st,temp,s,slovo1,slovo2:string;
    n,i,first,last,nst,ind,sum,k,i1,j1:integer;

begin
  n:=0;
  ind:=0;
  nst:=ComboBox1.ItemIndex ;
  st:=comboBox1.Items[nst];


  if (RadioGroup1.ItemIndex=0) then
    begin
      sum:=0;
      try k:=StrToInt(Edit2.Text);
      except ShowMessage('К Введенно не верно');
      end;
      for i:=2 to length(st) do
        begin
          if (st[i]=' ') then inc(sum);
          if (k=sum) then
            begin
              delete(st,1,i);
              break;
            end;
        end;
      label2.Caption:=st;
    end;

  if (radioGroup1.ItemIndex=1) then
    begin
      try i1:=StrToInt(Edit2.Text);
          j1:=StrToInt(Edit3.Text);
      except ShowMessage('Поля заполненны не верно');
      end;
      sum:=0;
      for i:=1 to length(st) do
        begin
          if (st[i]<>' ') then temp:=temp+st[i]
          else
            begin
              inc(sum);
              if (sum=i1) then slovo1:=temp;
              if (sum=j1) then slovo2:=temp;
              temp:='';
            end;
        end;
        showMessage(slovo1+' '+slovo2);
        s:=StringReplace(st, slovo2, slovo1,[]);
        st:=StringReplace(s, slovo1, slovo2,[]);
      label2.Caption:=st;
    end;


end;




procedure TForm1.FormCreate(Sender: TObject);
begin
  label3.Hide;
  label4.Hide;
  edit2.Hide;
  edit3.Hide;
end;

procedure TForm1.RadioGroup1Click(Sender: TObject);
begin
  label3.Hide;
  label4.Hide;
  edit2.Hide;
  edit3.Hide;
  edit2.Text:='';
  edit3.Text:='';
  if (RadioGroup1.ItemIndex=0) then
    begin
      Label3.Caption:='K';
      Label3.Show;
      edit2.show;
    end;
  if (RadioGroup1.ItemIndex=1) then
    begin
      Label3.Caption:='I';
      Label3.Show;
      Label4.Caption:='J';
      Label4.Show;
      edit2.show;
      edit3.show;
    end;
end;

end.
