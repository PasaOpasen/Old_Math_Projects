object Form1: TForm1
  Left = 262
  Top = 311
  Width = 785
  Height = 320
  Caption = #1057#1077#1084#1077#1089#1090#1088#1086#1074#1086#1077' '#8470'2, '#1055#1040#1057#1068#1050#1054
  Color = clMoneyGreen
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -19
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 24
  object Label2: TLabel
    Left = 16
    Top = 200
    Width = 437
    Height = 24
    Caption = #1050#1086#1083#1080#1095#1077#1089#1090#1074#1086' '#1089#1083#1086#1074' ('#1087#1088#1086#1094#1077#1076#1091#1088#1072#1084#1080' '#1082#1083#1072#1089#1089#1072' TString) ='
  end
  object Label3: TLabel
    Left = 576
    Top = 112
    Width = 96
    Height = 24
    Caption = #1055#1086#1089#1095#1080#1090#1072#1090#1100
    OnClick = ComdoBox1Click
  end
  object Label5: TLabel
    Left = 464
    Top = 176
    Width = 5
    Height = 24
  end
  object Label6: TLabel
    Left = 456
    Top = 200
    Width = 5
    Height = 24
  end
  object Label7: TLabel
    Left = 16
    Top = 176
    Width = 440
    Height = 24
    Caption = #1050#1086#1083#1080#1095#1077#1089#1090#1074#1086' '#1089#1083#1086#1074' ('#1073#1077#1079' '#1087#1088#1086#1094#1077#1076#1091#1088' '#1082#1083#1072#1089#1089#1072' TString) ='
  end
  object Label4: TLabel
    Left = 528
    Top = 176
    Width = 73
    Height = 24
    Caption = #1042#1088#1077#1084#1103' ='
  end
  object Label8: TLabel
    Left = 528
    Top = 200
    Width = 73
    Height = 24
    Caption = #1042#1088#1077#1084#1103' ='
  end
  object Label9: TLabel
    Left = 616
    Top = 200
    Width = 5
    Height = 24
  end
  object Label10: TLabel
    Left = 616
    Top = 176
    Width = 5
    Height = 24
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
      '5 66 75 4 3 2 4 6 4 00000')
  end
  object Edit1: TEdit
    Left = 208
    Top = 32
    Width = 473
    Height = 32
    TabOrder = 1
    Text = #1042#1074#1077#1076#1080#1090#1077' '#1089#1090#1088#1086#1082#1091' '#1080' '#1085#1072#1078#1084#1080#1090#1077' Enter'
    OnKeyPress = ComboBox1KeyPress
  end
  object BitBtn1: TBitBtn
    Left = 616
    Top = 240
    Width = 89
    Height = 33
    TabOrder = 2
    Kind = bkClose
  end
  object RadioGroup1: TRadioGroup
    Left = 16
    Top = 8
    Width = 177
    Height = 89
    Caption = #1042#1074#1077#1076#1080#1090#1077' '#1089#1090#1088#1086#1082#1080
    TabOrder = 3
  end
  object RadioButton1: TRadioButton
    Left = 24
    Top = 40
    Width = 137
    Height = 25
    Caption = #1042#1088#1091#1095#1085#1091#1102
    TabOrder = 4
    OnClick = RadioButton1Click
  end
  object RadioButton2: TRadioButton
    Left = 24
    Top = 72
    Width = 137
    Height = 17
    Caption = #1048#1079' '#1092#1072#1081#1083#1072
    TabOrder = 5
    OnClick = RadioButton2Click
  end
  object OpenDialog1: TOpenDialog
    Left = 344
    Top = 64
  end
end
