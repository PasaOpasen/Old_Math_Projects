object Form1: TForm1
  Left = 706
  Top = 185
  Width = 1022
  Height = 535
  Caption = #1055#1072#1089#1100#1082#1086' 4.4'
  Color = clWhite
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -21
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  Menu = MainMenu1
  OldCreateOrder = False
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 25
  object Label1: TLabel
    Left = 8
    Top = 42
    Width = 332
    Height = 29
    Caption = #1042#1074#1077#1076#1080#1090#1077' '#1088#1072#1079#1084#1077#1088#1085#1086#1089#1090#1100' '#1084#1072#1090#1088#1080#1094#1099':'
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -25
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object Label2: TLabel
    Left = 8
    Top = 83
    Width = 90
    Height = 26
    Caption = #1057#1090#1088#1086#1082#1080':'
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -23
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold, fsItalic]
    ParentFont = False
  end
  object Label3: TLabel
    Left = 8
    Top = 125
    Width = 104
    Height = 26
    Caption = #1057#1090#1086#1083#1073#1094#1099':'
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -23
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold, fsItalic]
    ParentFont = False
  end
  object Label4: TLabel
    Left = 692
    Top = 58
    Width = 229
    Height = 30
    Caption = ' '#1048#1089#1093#1086#1076#1085#1072#1103' '#1084#1072#1090#1088#1080#1094#1072' '
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -25
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold, fsItalic]
    ParentFont = False
  end
  object Label5: TLabel
    Left = 675
    Top = 267
    Width = 250
    Height = 30
    Caption = ' '#1055#1086#1083#1091#1095#1072#1077#1084#1072#1103' '#1084#1072#1090#1088#1080#1094#1072
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -25
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold, fsItalic]
    ParentFont = False
  end
  object Edit1: TEdit
    Left = 100
    Top = 75
    Width = 51
    Height = 32
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -23
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold, fsItalic]
    ParentFont = False
    TabOrder = 0
  end
  object Edit2: TEdit
    Left = 117
    Top = 117
    Width = 51
    Height = 32
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -23
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold, fsItalic]
    ParentFont = False
    TabOrder = 1
  end
  object Button1: TButton
    Left = 217
    Top = 100
    Width = 209
    Height = 34
    Caption = #1047#1072#1076#1072#1090#1100
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -23
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
    TabOrder = 2
    OnClick = Button1Click
  end
  object A: TStringGrid
    Left = 625
    Top = 92
    Width = 343
    Height = 159
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -23
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold, fsItalic]
    Options = [goFixedVertLine, goFixedHorzLine, goVertLine, goHorzLine, goRangeSelect, goEditing]
    ParentFont = False
    TabOrder = 3
  end
  object RadioGroup1: TRadioGroup
    Left = 8
    Top = 192
    Width = 476
    Height = 134
    Caption = #1059#1087#1086#1088#1103#1076#1086#1095#1080#1090#1100' '#1089#1090#1088#1086#1082#1080' '#1084#1072#1090#1088#1080#1094#1099' '#1087#1086' '#1085#1077#1091#1073#1099#1074#1072#1085#1080#1102
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -23
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold, fsItalic]
    ParentFont = False
    TabOrder = 4
  end
  object RB1: TRadioButton
    Left = 25
    Top = 225
    Width = 334
    Height = 18
    Caption = #1072') '#1101#1083#1077#1084#1077#1085#1090#1086#1074' '#1087#1077#1088#1074#1086#1075#1086' '#1089#1090#1086#1083#1073#1094#1072
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -23
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold, fsItalic]
    ParentFont = False
    TabOrder = 5
  end
  object RB2: TRadioButton
    Left = 25
    Top = 250
    Width = 334
    Height = 18
    Caption = #1073') '#1089#1091#1084#1084#1099' '#1080#1093' '#1101#1083#1077#1084#1077#1085#1090#1086#1074
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -23
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold, fsItalic]
    ParentFont = False
    TabOrder = 6
  end
  object RB3: TRadioButton
    Left = 25
    Top = 275
    Width = 318
    Height = 18
    Caption = #1074') '#1080#1093' '#1085#1072#1080#1073#1086#1083#1100#1096#1080#1093' '#1101#1083#1077#1084#1077#1085#1090#1086#1074
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -23
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold, fsItalic]
    ParentFont = False
    TabOrder = 7
  end
  object RB4: TRadioButton
    Left = 25
    Top = 300
    Width = 351
    Height = 18
    Caption = #1075') '#1080#1093' '#1085#1072#1080#1084#1077#1085#1100#1096#1080#1093' '#1101#1083#1077#1084#1077#1085#1090#1086#1074
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -23
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold, fsItalic]
    ParentFont = False
    TabOrder = 8
  end
  object Button2: TButton
    Left = 179
    Top = 358
    Width = 164
    Height = 43
    Caption = #1042#1099#1087#1086#1083#1085#1080#1090#1100
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -23
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
    TabOrder = 9
    OnClick = Button2Click
  end
  object B: TStringGrid
    Left = 625
    Top = 300
    Width = 343
    Height = 159
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -23
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold, fsItalic]
    ParentFont = False
    TabOrder = 10
    RowHeights = (
      24
      24
      24
      24
      24)
  end
  object OpenDialog1: TOpenDialog
    Left = 256
    Top = 8
  end
  object SaveDialog1: TSaveDialog
    Left = 288
    Top = 8
  end
  object MainMenu1: TMainMenu
    Left = 8
    Top = 8
    object N1: TMenuItem
      Caption = #1055#1088#1086#1095#1077#1089#1090#1100' '#1088#1072#1079#1084#1077#1088#1085#1086#1089#1090#1100' '#1084#1072#1090#1088#1080#1094#1099' '#1080' '#1084#1072#1090#1088#1080#1094#1091' '#1080#1079' '#1092#1072#1081#1083#1072
      OnClick = N1Click
    end
  end
end
