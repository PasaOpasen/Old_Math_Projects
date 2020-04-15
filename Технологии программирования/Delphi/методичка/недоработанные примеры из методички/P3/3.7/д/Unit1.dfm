object Form1: TForm1
  Left = 405
  Top = 377
  Width = 787
  Height = 289
  Caption = 'Form1'
  Color = clBtnFace
  Font.Charset = RUSSIAN_CHARSET
  Font.Color = clWindowText
  Font.Height = -19
  Font.Name = 'Courier New'
  Font.Style = []
  OldCreateOrder = False
  PixelsPerInch = 96
  TextHeight = 21
  object Label1: TLabel
    Left = 8
    Top = 32
    Width = 22
    Height = 21
    Caption = 'A='
  end
  object Label2: TLabel
    Left = 8
    Top = 64
    Width = 22
    Height = 21
    Caption = 'B='
  end
  object Label3: TLabel
    Left = 8
    Top = 96
    Width = 22
    Height = 21
    Caption = 'N='
  end
  object PaintBox1: TPaintBox
    Left = 368
    Top = 0
    Width = 401
    Height = 249
  end
  object Edit1: TEdit
    Left = 32
    Top = 32
    Width = 65
    Height = 29
    TabOrder = 0
  end
  object Edit2: TEdit
    Left = 32
    Top = 64
    Width = 65
    Height = 29
    TabOrder = 1
  end
  object Edit3: TEdit
    Left = 32
    Top = 96
    Width = 65
    Height = 29
    TabOrder = 2
  end
  object RadioGroup1: TRadioGroup
    Left = 104
    Top = 32
    Width = 257
    Height = 97
    Caption = #1059#1088#1072#1085#1077#1085#1080#1103
    ItemIndex = 0
    Items.Strings = (
      'x*x+3'
      'x*x*x-1/x'
      'x/sin(x)')
    TabOrder = 3
  end
  object Memo1: TMemo
    Left = 8
    Top = 136
    Width = 353
    Height = 113
    ScrollBars = ssVertical
    TabOrder = 4
  end
  object Button1: TButton
    Left = 8
    Top = 0
    Width = 353
    Height = 25
    Caption = #1042#1099#1095#1077#1089#1083#1080#1090#1100
    TabOrder = 5
    OnClick = Button1Click
  end
end
