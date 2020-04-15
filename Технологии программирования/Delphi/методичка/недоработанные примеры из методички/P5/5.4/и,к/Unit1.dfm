object Form1: TForm1
  Left = 356
  Top = 197
  Width = 686
  Height = 190
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
    Left = 64
    Top = 120
    Width = 36
    Height = 13
    Caption = #1054#1090#1074#1077#1090': '
    OnClick = ComboBox1Click
  end
  object Label2: TLabel
    Left = 104
    Top = 120
    Width = 3
    Height = 13
  end
  object Edit1: TEdit
    Left = 64
    Top = 48
    Width = 249
    Height = 21
    TabOrder = 0
    Text = #1042#1074#1077#1076#1080#1090#1077' '#1080' '#1085#1072#1078#1084#1080#1090#1077' Enter'
    OnKeyPress = Edit1KeyPress
  end
  object ComboBox1: TComboBox
    Left = 64
    Top = 80
    Width = 249
    Height = 21
    ItemHeight = 13
    TabOrder = 1
    OnClick = ComboBox1Click
  end
  object RadioGroup1: TRadioGroup
    Left = 320
    Top = 48
    Width = 225
    Height = 57
    Caption = 'RadioGroup1'
    Items.Strings = (
      #1080
      #1082)
    TabOrder = 2
  end
  object Button1: TButton
    Left = 552
    Top = 48
    Width = 89
    Height = 57
    Caption = #1042#1074#1086#1076' '#1080#1079' '#1092#1072#1081#1083#1072
    TabOrder = 3
    OnClick = Button1Click
  end
end
