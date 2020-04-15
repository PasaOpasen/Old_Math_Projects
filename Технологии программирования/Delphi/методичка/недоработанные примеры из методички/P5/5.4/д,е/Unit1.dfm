object Form1: TForm1
  Left = 309
  Top = 183
  Width = 712
  Height = 143
  Caption = 'Form1'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  PixelsPerInch = 96
  TextHeight = 13
  object Label1: TLabel
    Left = 16
    Top = 80
    Width = 111
    Height = 13
    Caption = #1053#1072#1078#1084#1080#1090#1077' '#1076#1083#1103' '#1086#1090#1074#1077#1090#1072': '
    OnClick = ComboBox1Click
  end
  object Label2: TLabel
    Left = 128
    Top = 80
    Width = 3
    Height = 13
  end
  object Edit1: TEdit
    Left = 16
    Top = 16
    Width = 289
    Height = 21
    TabOrder = 0
    Text = #1042#1074#1077#1076#1080#1090#1077' '#1090#1077#1082#1089#1090' '#1080' '#1085#1072#1078#1084#1080#1090#1077' Enter'
    OnKeyPress = Edit1KeyPress
  end
  object ComboBox1: TComboBox
    Left = 16
    Top = 48
    Width = 289
    Height = 21
    ItemHeight = 13
    TabOrder = 1
    OnClick = ComboBox1Click
  end
  object RadioGroup1: TRadioGroup
    Left = 320
    Top = 8
    Width = 185
    Height = 65
    Caption = 'RadioGroup1'
    Items.Strings = (
      #1076
      #1077)
    TabOrder = 2
  end
  object Button1: TButton
    Left = 512
    Top = 8
    Width = 169
    Height = 25
    Caption = #1042#1074#1086#1076' '#1080#1079' '#1092#1072#1081#1083#1072
    TabOrder = 3
    OnClick = Button1Click
  end
end
