object Form1: TForm1
  Left = 298
  Top = 246
  Width = 783
  Height = 452
  Caption = #1055#1088#1080#1084#1077#1088' 1'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 13
  object Label1: TLabel
    Left = 8
    Top = 16
    Width = 13
    Height = 13
    Caption = 'X='
  end
  object Label2: TLabel
    Left = 88
    Top = 16
    Width = 13
    Height = 13
    Caption = 'Y='
  end
  object Label3: TLabel
    Left = 168
    Top = 16
    Width = 13
    Height = 13
    Caption = 'Z='
  end
  object Edit1: TEdit
    Left = 24
    Top = 16
    Width = 57
    Height = 21
    TabOrder = 0
    Text = 'Edit1'
  end
  object Edit2: TEdit
    Left = 104
    Top = 16
    Width = 57
    Height = 21
    TabOrder = 1
    Text = 'Edit1'
  end
  object Edit3: TEdit
    Left = 184
    Top = 16
    Width = 57
    Height = 21
    TabOrder = 2
    Text = 'Edit1'
  end
  object Button1: TButton
    Left = 16
    Top = 48
    Width = 225
    Height = 25
    Caption = 'u=sec(x+y)+Exp(y-z)'
    TabOrder = 3
    OnClick = Button1Click
  end
  object Memo1: TMemo
    Left = 16
    Top = 80
    Width = 225
    Height = 113
    Lines.Strings = (
      'Memo1')
    ScrollBars = ssBoth
    TabOrder = 4
  end
end
