object Form1: TForm1
  Left = 640
  Top = 202
  Width = 928
  Height = 466
  Caption = 'Form1'
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
    Top = 40
    Width = 13
    Height = 13
    Caption = 'A='
  end
  object Label2: TLabel
    Left = 8
    Top = 64
    Width = 13
    Height = 13
    Caption = 'B='
  end
  object Edit1: TEdit
    Left = 24
    Top = 40
    Width = 49
    Height = 21
    TabOrder = 0
    Text = 'Edit1'
  end
  object Edit2: TEdit
    Left = 24
    Top = 64
    Width = 49
    Height = 21
    TabOrder = 1
    Text = 'Edit2'
  end
  object RadioGroup1: TRadioGroup
    Left = 104
    Top = 64
    Width = 153
    Height = 89
    Caption = 'RadioGroup1'
    TabOrder = 2
  end
  object RadioButton1: TRadioButton
    Left = 112
    Top = 80
    Width = 113
    Height = 17
    Caption = 'y(x)=x+x^3'
    TabOrder = 3
  end
  object RadioButton2: TRadioButton
    Left = 112
    Top = 104
    Width = 113
    Height = 17
    Caption = 'y(x)=e^x/sin(x)'
    TabOrder = 4
  end
  object RadioButton3: TRadioButton
    Left = 112
    Top = 128
    Width = 113
    Height = 17
    Caption = 'y(x)=x*x-cos(x)'
    TabOrder = 5
  end
  object Memo1: TMemo
    Left = 264
    Top = 64
    Width = 241
    Height = 89
    Lines.Strings = (
      'Memo1')
    ScrollBars = ssBoth
    TabOrder = 6
  end
  object Button1: TButton
    Left = 104
    Top = 32
    Width = 401
    Height = 25
    Caption = #1042#1099#1089#1095#1080#1090#1072#1090#1100' '#1080#1085#1090#1077#1075#1088#1072#1083
    TabOrder = 7
    OnClick = Button1Click
  end
end
