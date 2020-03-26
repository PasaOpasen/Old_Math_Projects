object Form1: TForm1
  Left = 122
  Top = 202
  Width = 1117
  Height = 230
  Caption = '5.5 '#1055#1040#1057#1068#1050#1054
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
    Left = 40
    Top = 24
    Width = 145
    Height = 24
    Caption = #1042#1074#1077#1076#1080#1090#1077' '#1089#1090#1088#1086#1082#1080
  end
  object Label2: TLabel
    Left = 32
    Top = 104
    Width = 146
    Height = 24
    Caption = #1054#1090#1074#1077#1090' '#1086' '#1095#1080#1089#1083#1077' ='
  end
  object Label3: TLabel
    Left = 192
    Top = 104
    Width = 109
    Height = 24
    Caption = #1086#1073#1085#1072#1088#1091#1078#1080#1090#1100
    OnClick = ComdoBox1Click
  end
  object Label4: TLabel
    Left = 197
    Top = 128
    Width = 5
    Height = 24
    OnClick = ComdoBox1Click
  end
  object ComboBox1: TComboBox
    Left = 560
    Top = 16
    Width = 513
    Height = 32
    ItemHeight = 24
    TabOrder = 0
    Text = #1042#1099#1073#1080#1088#1072#1090#1100' '#1089#1090#1088#1086#1082#1091' '#1080#1079' '#1090#1077#1082#1089#1090#1072' '#1080' '#1085#1072#1078#1072#1090#1100' '#1085#1072' "'#1087#1086#1089#1095#1080#1090#1072#1090#1100'"'
    Items.Strings = (
      '10'#1077'11+3-4'#1077'2 '
      '3   4'#1077'32-1 90'
      '1 2 3 '#1077'3+11'
      #1077'-11 +3-22 e10'
      '8   1e34 333'
      ' 123       '
      ' -233e-1 ')
  end
  object Edit1: TEdit
    Left = 40
    Top = 56
    Width = 473
    Height = 32
    TabOrder = 1
    Text = #1042#1074#1077#1076#1080#1090#1077' '#1089#1090#1088#1086#1082#1091' '#1080' '#1085#1072#1078#1084#1080#1090#1077' Enter'
    OnKeyPress = ComboBox1KeyPress
  end
  object BitBtn1: TBitBtn
    Left = 784
    Top = 160
    Width = 75
    Height = 25
    TabOrder = 2
    Kind = bkClose
  end
  object Memo1: TMemo
    Left = 560
    Top = 56
    Width = 513
    Height = 49
    TabOrder = 3
    OnClick = ComdoBox1Click
  end
end
