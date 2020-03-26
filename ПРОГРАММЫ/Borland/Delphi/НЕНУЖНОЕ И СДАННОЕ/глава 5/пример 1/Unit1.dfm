object Form1: TForm1
  Left = 225
  Top = 167
  Width = 582
  Height = 266
  Caption = #1055#1088#1080#1084#1077#1088' 1, '#1055#1040#1057#1068#1050#1054' ('#1075#1083'. 5)'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -19
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  PixelsPerInch = 96
  TextHeight = 24
  object Label1: TLabel
    Left = 32
    Top = 24
    Width = 145
    Height = 24
    Caption = #1042#1074#1077#1076#1080#1090#1077' '#1089#1090#1088#1086#1082#1080
  end
  object Label2: TLabel
    Left = 128
    Top = 152
    Width = 170
    Height = 24
    Caption = #1050#1086#1083#1080#1095#1077#1089#1090#1074#1086' '#1089#1083#1086#1074' ='
  end
  object Label3: TLabel
    Left = 304
    Top = 152
    Width = 94
    Height = 24
    Caption = #1087#1086#1089#1095#1080#1090#1072#1090#1100
    OnClick = ComdoBox1Click
  end
  object Label4: TLabel
    Left = 408
    Top = 152
    Width = 136
    Height = 24
    Caption = '/'#1080#1089#1095#1077#1088#1087'. '#1084#1077#1090#1086#1076
    OnClick = ComdoBox1Click
  end
  object ComboBox1: TComboBox
    Left = 40
    Top = 112
    Width = 513
    Height = 32
    ItemHeight = 24
    TabOrder = 0
    Text = #1042#1099#1073#1080#1088#1072#1090#1100' '#1089#1090#1088#1086#1082#1091' '#1080#1079' '#1090#1077#1082#1089#1090#1072' '#1080' '#1085#1072#1078#1072#1090#1100' '#1085#1072' "'#1087#1086#1089#1095#1080#1090#1072#1090#1100'"'
    Items.Strings = (
      #1057#1090#1088#1086#1082#1072' 1'
      #1042#1090#1086#1088#1072#1103' '#1089#1090#1088#1086#1082#1072
      '"'#1063#1091#1078#1080#1093' '#1076#1077#1090#1077#1081' '#1085#1077' '#1073#1099#1074#1072#1077#1090'"')
  end
  object Edit1: TEdit
    Left = 40
    Top = 72
    Width = 473
    Height = 32
    TabOrder = 1
    Text = #1042#1074#1077#1076#1080#1090#1077' '#1089#1090#1088#1086#1082#1091' '#1080' '#1085#1072#1078#1084#1080#1090#1077' Enter'
    OnKeyPress = ComboBox1KeyPress
  end
  object BitBtn1: TBitBtn
    Left = 216
    Top = 192
    Width = 75
    Height = 25
    TabOrder = 2
    Kind = bkClose
  end
end
