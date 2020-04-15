unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls, Buttons;

type
  TForm1 = class(TForm)
    mmo1: TMemo;
    lblHintLabel: TLabel;
    bbOK: TBitBtn;
    bbAll: TBitBtn;
    bbIgnore: TBitBtn;
    bbClose: TBitBtn;
    cbWindow: TColorBox;
    bbWindow: TButton;
    lblSecondHint: TLabel;
    procedure mmo1DragDrop(Sender, Source: TObject; X, Y: Integer);
    procedure mmo1DragOver(Sender, Source: TObject; X, Y: Integer;
      State: TDragState; var Accept: Boolean);
    procedure bbWindowClick(Sender: TObject);
    procedure bbCloseClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

{$R *.dfm}


procedure TForm1.mmo1DragDrop(Sender, Source: TObject; X, Y: Integer);//отпустил компонент на окне
begin
  mmo1.Clear;
  mmo1.Lines.Add((Source as TComponent).Name);
end;

procedure TForm1.mmo1DragOver(Sender, Source: TObject; X, Y: Integer;   //компонент оказался в окне
  State: TDragState; var Accept: Boolean);
begin
   Accept:= True;       //готов примять
end;

procedure TForm1.bbWindowClick(Sender: TObject);
begin
  Form1.Color:= cbWindow.Colors[cbWindow.ItemIndex];
end;

procedure TForm1.bbCloseClick(Sender: TObject);
begin
  Close;
end;

end.
