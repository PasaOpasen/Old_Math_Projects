object Form1: TForm1
  Left = 246
  Top = 202
  Width = 928
  Height = 493
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
  object ComboBox1: TComboBox
    Left = 24
    Top = 64
    Width = 321
    Height = 21
    ItemHeight = 13
    TabOrder = 0
    Text = ' '
  end
  object Edit1: TEdit
    Left = 24
    Top = 40
    Width = 321
    Height = 21
    TabOrder = 1
    Text = #1042#1074#1077#1076#1080#1090#1077' '#1087#1088#1077#1076#1083#1086#1078#1077#1085#1080#1077' '#1080' '#1085#1072#1078#1084#1080#1090#1077' Enter'
    OnKeyPress = ComboBox1KeyPress
  end
  object Memo1: TMemo
    Left = 535
    Top = 40
    Width = 170
    Height = 81
    Lines.Strings = (
      'Memo1')
    ScrollBars = ssVertical
    TabOrder = 2
  end
  object Button1: TButton
    Left = 24
    Top = 88
    Width = 321
    Height = 33
    Caption = #1042#1099#1087#1086#1083#1085#1080#1090#1100
    TabOrder = 3
    OnClick = Button1Click
  end
  object RadioGroup1: TRadioGroup
    Left = 352
    Top = 40
    Width = 177
    Height = 81
    Caption = 'RadioGroup1'
    TabOrder = 4
  end
  object RadioButton1: TRadioButton
    Left = 360
    Top = 64
    Width = 113
    Height = 17
    Caption = #1040
    TabOrder = 5
  end
  object RadioButton2: TRadioButton
    Left = 360
    Top = 88
    Width = 113
    Height = 17
    Caption = #1041
    TabOrder = 6
  end
end
