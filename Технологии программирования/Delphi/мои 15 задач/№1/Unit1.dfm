object Form1: TForm1
  Left = 305
  Top = 184
  Width = 612
  Height = 476
  Caption = #1057#1077#1084#1077#1089#1090#1088#1086#1074#1086#1077' '#8470'1, '#1055#1040#1057#1068#1050#1054
  Color = clMoneyGreen
  Font.Charset = RUSSIAN_CHARSET
  Font.Color = clWindowText
  Font.Height = -19
  Font.Name = 'Arial'
  Font.Style = []
  OldCreateOrder = False
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 22
  object Label1: TLabel
    Left = 216
    Top = 152
    Width = 115
    Height = 22
    Caption = #1088#1072#1079#1084#1077#1088#1085#1086#1089#1090#1100
    Color = clMoneyGreen
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Arial'
    Font.Style = []
    ParentColor = False
    ParentFont = False
  end
  object Label2: TLabel
    Left = 16
    Top = 152
    Width = 187
    Height = 22
    Caption = #1042#1074#1077#1089#1090#1080' '#1089' '#1082#1083#1072#1074#1080#1072#1090#1091#1088#1099':'
  end
  object edt1: TEdit
    Left = 344
    Top = 144
    Width = 41
    Height = 30
    TabOrder = 0
  end
  object btn1: TButton
    Left = 408
    Top = 148
    Width = 97
    Height = 25
    Caption = #1047#1072#1076#1072#1090#1100
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clNavy
    Font.Height = -19
    Font.Name = 'Arial'
    Font.Style = []
    ParentFont = False
    TabOrder = 1
    OnClick = btn1Click
  end
  object strngrd1: TStringGrid
    Left = 25
    Top = 192
    Width = 300
    Height = 225
    ColCount = 4
    DefaultColWidth = 45
    DefaultRowHeight = 35
    RowCount = 4
    Options = [goFixedVertLine, goFixedHorzLine, goVertLine, goHorzLine, goRangeSelect, goEditing]
    ScrollBars = ssNone
    TabOrder = 2
  end
  object strngrd2: TStringGrid
    Left = 349
    Top = 192
    Width = 69
    Height = 225
    ColCount = 1
    DefaultRowHeight = 35
    FixedCols = 0
    RowCount = 2
    Options = [goFixedVertLine, goFixedHorzLine, goVertLine, goHorzLine, goRangeSelect, goEditing]
    ScrollBars = ssNone
    TabOrder = 3
  end
  object strngrd3: TStringGrid
    Left = 509
    Top = 192
    Width = 69
    Height = 225
    ColCount = 1
    DefaultRowHeight = 35
    FixedCols = 0
    RowCount = 2
    ScrollBars = ssNone
    TabOrder = 4
  end
  object btn2: TButton
    Left = 429
    Top = 272
    Width = 75
    Height = 49
    Caption = 'A*b = '
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlue
    Font.Height = -19
    Font.Name = 'Arial'
    Font.Style = []
    ParentFont = False
    TabOrder = 5
    OnClick = btn2Click
  end
  object RadioGroup1: TRadioGroup
    Left = 12
    Top = 16
    Width = 437
    Height = 121
    Caption = ' '#1042#1099#1073#1077#1088#1080#1090#1077' '#1084#1072#1090#1088#1080#1094#1091' '#1080' '#1074#1077#1082#1090#1086#1088' ('#1087#1088#1080#1084#1077#1088' '#1080#1079' '#1092#1072#1081#1083#1072'):'
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Arial'
    Font.Style = []
    Items.Strings = (
      '3 '#1093' 3 '#1080' 3 '#1093' 1'
      '4 '#1093' 4 '#1080' 4 '#1093' 1'
      '5 '#1093' 5 '#1080' 5 '#1093' 1')
    ParentFont = False
    TabOrder = 6
    OnClick = RadioGroup1Click
  end
end
