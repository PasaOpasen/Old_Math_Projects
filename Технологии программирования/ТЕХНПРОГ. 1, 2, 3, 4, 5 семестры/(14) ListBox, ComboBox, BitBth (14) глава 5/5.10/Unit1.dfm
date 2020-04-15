object Form1: TForm1
  Left = 451
  Top = 247
  Width = 1162
  Height = 423
  Caption = #1055#1040#1057#1068#1050#1054', '#1074#1086#1087#1088#1086#1089' 14'
  Color = clMoneyGreen
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
    Left = 700
    Top = 172
    Width = 156
    Height = 23
    Caption = #1054#1087#1077#1088#1072#1090#1086#1088#1099' '#1094#1080#1082#1083#1072
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
  end
  object Label2: TLabel
    Left = 703
    Top = 302
    Width = 146
    Height = 23
    Caption = #1054#1087#1080#1089#1072#1085#1080#1077' '#1089#1090#1088#1086#1082
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
  end
  object Label3: TLabel
    Left = 700
    Top = 20
    Width = 133
    Height = 23
    Caption = #1058#1080#1087#1099' '#1076#1072#1085#1085#1099#1093
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
  end
  object Label4: TLabel
    Left = 696
    Top = 100
    Width = 201
    Height = 23
    Caption = #1059#1089#1083#1086#1074#1085#1099#1077' '#1086#1087#1077#1088#1072#1090#1086#1088#1099
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
  end
  object Label5: TLabel
    Left = 701
    Top = 242
    Width = 282
    Height = 23
    Caption = #1055#1088#1086#1094#1077#1076#1091#1088#1099' '#1087#1088#1077#1088#1099#1074#1072#1085#1080#1103' '#1088#1072#1073#1086#1090#1099
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
  end
  object ComboBox1: TComboBox
    Left = 698
    Top = 204
    Width = 223
    Height = 31
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ItemHeight = 23
    ParentFont = False
    TabOrder = 0
    Text = #1042#1099#1073#1077#1088#1080#1090#1077' '#1086#1087#1077#1088#1072#1090#1086#1088
    OnChange = ComboBox1Change
    Items.Strings = (
      'FOR'
      'WHILE'
      'REPEAT')
  end
  object ComboBox2: TComboBox
    Left = 696
    Top = 329
    Width = 225
    Height = 31
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ItemHeight = 23
    ParentFont = False
    TabOrder = 1
    Text = #1042#1099#1073#1077#1088#1080#1090#1077' '#1089#1090#1088#1086#1082#1091' '
    OnChange = ComboBox2Change
    Items.Strings = (
      'FUNCTION COPY'
      'PROCEDURE DELETE'
      'FUNCTION CONCAT')
  end
  object Memo1: TMemo
    Left = 32
    Top = 26
    Width = 643
    Height = 335
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    Lines.Strings = (
      '')
    ParentFont = False
    ReadOnly = True
    TabOrder = 2
  end
  object BitBtn1: TBitBtn
    Left = 1008
    Top = 184
    Width = 113
    Height = 49
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
    TabOrder = 3
    Kind = bkClose
  end
  object ComboBox3: TComboBox
    Left = 700
    Top = 52
    Width = 261
    Height = 31
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ItemHeight = 23
    ParentFont = False
    TabOrder = 4
    Text = #1042#1099#1073#1077#1088#1080#1090#1077' '#1090#1080#1087' '#1076#1072#1085#1085#1099#1093
    OnChange = ComboBox3Change
    Items.Strings = (
      #1062#1045#1051#1067#1045
      #1042#1045#1065#1045#1057#1058#1042#1045#1053#1053#1067#1045
      #1051#1054#1043#1048#1063#1045#1057#1050#1048#1045
      #1052#1040#1057#1057#1048#1042#1067)
  end
  object ComboBox4: TComboBox
    Left = 698
    Top = 132
    Width = 263
    Height = 31
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ItemHeight = 23
    ParentFont = False
    TabOrder = 5
    Text = #1042#1099#1073#1077#1088#1080#1090#1077' '#1091#1089#1083'. '#1086#1087#1077#1088#1072#1090#1086#1088
    OnChange = ComboBox4Change
    Items.Strings = (
      'IF'
      'CASE')
  end
  object ComboBox5: TComboBox
    Left = 698
    Top = 268
    Width = 223
    Height = 31
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ItemHeight = 23
    ParentFont = False
    TabOrder = 6
    Text = #1042#1099#1073#1077#1088#1080#1090#1077' '#1087#1088#1086#1094#1077#1076#1091#1088#1091
    OnChange = ComboBox5Change
    Items.Strings = (
      'BREAK'
      'EXIT'
      'HALT')
  end
end
