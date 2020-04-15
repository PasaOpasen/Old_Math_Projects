object Form1: TForm1
  Left = 358
  Top = 231
  Width = 928
  Height = 480
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
    Left = 8
    Top = 8
    Width = 80
    Height = 13
    Caption = #1042#1074#1077#1076#1080#1090#1077' '#1089#1090#1088#1086#1082#1080
  end
  object Label2: TLabel
    Left = 8
    Top = 88
    Width = 92
    Height = 13
    Caption = #1050#1086#1083#1080#1095#1077#1089#1090#1074#1086' '#1089#1083#1086#1074'='
  end
  object Label3: TLabel
    Left = 104
    Top = 88
    Width = 51
    Height = 13
    Caption = #1087#1086#1089#1095#1080#1090#1072#1090#1100
  end
  object ComboBox1: TComboBox
    Left = 8
    Top = 56
    Width = 329
    Height = 21
    ItemHeight = 13
    TabOrder = 0
    Text = #1042#1099#1073#1088#1072#1090#1100' '#1089#1090#1088#1086#1082#1091' '#1080#1079' '#1090#1077#1082#1089#1090#1072' '#1080' '#1085#1072#1078#1072#1090#1100' '#1085#1072' '#1089#1083#1086#1074#1086' '#1087#1086#1089#1095#1080#1090#1072#1090#1100
    OnClick = ComboBox1Click
    Items.Strings = (
      #1073#1083#1103' '#1073#1083#1103' '#1073#1083#1103)
  end
  object Edit1: TEdit
    Left = 8
    Top = 32
    Width = 329
    Height = 21
    TabOrder = 1
    Text = #1042#1074#1077#1076#1080#1090#1077' '#1089#1090#1088#1088#1086#1082#1091' '#1080' '#1085#1072#1078#1084#1080#1090#1090#1077' '#1080#1085#1090#1077#1088
    OnKeyPress = ComboBox1KeyPress
  end
  object BitBtn1: TBitBtn
    Left = 152
    Top = 120
    Width = 73
    Height = 25
    TabOrder = 2
    Kind = bkClose
  end
end
