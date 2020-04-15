object Form1: TForm1
  Left = 195
  Top = 131
  Width = 884
  Height = 490
  Caption = #1055#1040#1057#1068#1050#1054', '#1074#1086#1087#1088#1086#1089' 13'
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
    Left = 33
    Top = 24
    Width = 192
    Height = 23
    Caption = #1059#1082#1072#1078#1080#1090#1077' '#1088#1072#1079#1084#1077#1088#1085#1086#1089#1090#1100
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
  end
  object Label3: TLabel
    Left = 33
    Top = 88
    Width = 158
    Height = 23
    Caption = #1063#1090#1077#1085#1080#1077' '#1080#1079' '#1092#1072#1081#1083#1072':'
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
  end
  object Edit1: TEdit
    Left = 241
    Top = 16
    Width = 57
    Height = 31
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
    TabOrder = 0
  end
  object Button1: TButton
    Left = 329
    Top = 8
    Width = 449
    Height = 49
    Caption = #1047#1072#1076#1072#1090#1100' '#1088#1072#1079#1084#1077#1088#1085#1086#1089#1090#1100' '#1084#1072#1090#1088#1080#1094#1099' '#1080' '#1074#1077#1082#1090#1086#1088#1072
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
    TabOrder = 1
    OnClick = Button1Click
  end
  object StringGrid1: TStringGrid
    Left = 234
    Top = 72
    Width = 340
    Height = 201
    ColCount = 2
    DefaultColWidth = 80
    DefaultRowHeight = 30
    RowCount = 2
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    Options = [goFixedVertLine, goFixedHorzLine, goVertLine, goHorzLine, goRangeSelect, goEditing]
    ParentFont = False
    TabOrder = 2
  end
  object StringGrid2: TStringGrid
    Left = 592
    Top = 72
    Width = 75
    Height = 201
    ColCount = 1
    DefaultColWidth = 80
    DefaultRowHeight = 30
    FixedCols = 0
    RowCount = 3
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    Options = [goFixedVertLine, goFixedHorzLine, goVertLine, goHorzLine, goRangeSelect, goEditing]
    ParentFont = False
    ScrollBars = ssVertical
    TabOrder = 3
  end
  object StringGrid3: TStringGrid
    Left = 777
    Top = 72
    Width = 80
    Height = 201
    ColCount = 1
    DefaultColWidth = 80
    DefaultRowHeight = 30
    FixedCols = 0
    RowCount = 3
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
    ScrollBars = ssVertical
    TabOrder = 4
  end
  object Button2: TButton
    Left = 689
    Top = 160
    Width = 73
    Height = 41
    Caption = 'A*b'
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
    TabOrder = 5
    OnClick = Button2Click
  end
  object RadioGroup1: TRadioGroup
    Left = 33
    Top = 120
    Width = 178
    Height = 113
    Caption = #1042#1099#1073#1080#1088#1080#1090#1077' '#1092#1072#1081#1083
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
    TabOrder = 6
  end
  object RadioButton1: TRadioButton
    Left = 41
    Top = 152
    Width = 138
    Height = 25
    Caption = #1052#1072#1090#1088#1080#1094#1072' 3'#1093'3'
    Color = clSkyBlue
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentColor = False
    ParentFont = False
    TabOrder = 7
    OnClick = RadioButton1Click
  end
  object RadioButton2: TRadioButton
    Left = 41
    Top = 176
    Width = 136
    Height = 25
    Caption = #1052#1072#1090#1088#1080#1094#1072' 4'#1093'4'
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
    TabOrder = 8
    OnClick = RadioButton2Click
  end
  object RadioButton3: TRadioButton
    Left = 41
    Top = 200
    Width = 144
    Height = 25
    Caption = #1052#1072#1090#1088#1080#1094#1072' 5'#1093'5'
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
    TabOrder = 9
    OnClick = RadioButton3Click
  end
  object Button3: TButton
    Left = 583
    Top = 376
    Width = 254
    Height = 57
    Caption = #1042#1099#1093#1086#1076' '#1080#1079' '#1087#1088#1086#1075#1088#1072#1084#1084#1099
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
    TabOrder = 10
    OnClick = Button3Click
  end
  object RadioGroup2: TRadioGroup
    Left = 9
    Top = 288
    Width = 424
    Height = 153
    Caption = #1042#1099#1073#1077#1088#1077#1090#1077' '#1094#1074#1077#1090' '#1092#1086#1085#1072' '
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
    TabOrder = 11
  end
  object RadioButton4: TRadioButton
    Left = 17
    Top = 320
    Width = 184
    Height = 25
    Caption = #1072#1082#1074#1072#1084#1072#1088#1080#1085
    Color = clAqua
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentColor = False
    ParentFont = False
    TabOrder = 12
    OnClick = RadioButton4Click
  end
  object RadioButton5: TRadioButton
    Left = 225
    Top = 320
    Width = 192
    Height = 25
    Caption = #1090#1077#1084#1085#1086'-'#1079#1077#1083#1077#1085#1099#1081
    Color = clTeal
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentColor = False
    ParentFont = False
    TabOrder = 13
    OnClick = RadioButton5Click
  end
  object RadioButton6: TRadioButton
    Left = 17
    Top = 400
    Width = 184
    Height = 25
    Caption = #1078#1077#1083#1090#1099#1081
    Color = clYellow
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentColor = False
    ParentFont = False
    TabOrder = 14
    OnClick = RadioButton6Click
  end
  object RadioButton7: TRadioButton
    Left = 17
    Top = 360
    Width = 184
    Height = 25
    Caption = #1083#1072#1081#1084
    Color = clLime
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentColor = False
    ParentFont = False
    TabOrder = 15
    OnClick = RadioButton7Click
  end
  object RadioButton8: TRadioButton
    Left = 225
    Top = 360
    Width = 192
    Height = 25
    Caption = #1092#1080#1086#1083#1077#1090#1086#1074#1099#1081
    Color = clFuchsia
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentColor = False
    ParentFont = False
    TabOrder = 16
    OnClick = RadioButton8Click
  end
  object RadioButton9: TRadioButton
    Left = 225
    Top = 400
    Width = 192
    Height = 25
    Caption = #1073#1077#1083#1099#1081
    Color = clWhite
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentColor = False
    ParentFont = False
    TabOrder = 17
    OnClick = RadioButton9Click
  end
end
