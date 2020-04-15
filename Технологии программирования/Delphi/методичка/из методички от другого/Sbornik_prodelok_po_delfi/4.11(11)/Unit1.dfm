object Form1: TForm1
  Left = 99
  Top = 241
  Width = 928
  Height = 478
  Caption = 'Form1'
  Color = clBtnFace
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
    Left = 40
    Top = 32
    Width = 14
    Height = 13
    Caption = 'N='
  end
  object StringGrid1: TStringGrid
    Left = 32
    Top = 64
    Width = 73
    Height = 361
    ColCount = 1
    FixedCols = 0
    RowCount = 4
    Options = [goFixedVertLine, goFixedHorzLine, goVertLine, goHorzLine, goRangeSelect, goEditing]
    TabOrder = 0
  end
  object StringGrid2: TStringGrid
    Left = 112
    Top = 64
    Width = 73
    Height = 369
    ColCount = 1
    FixedCols = 0
    RowCount = 4
    Options = [goFixedVertLine, goFixedHorzLine, goVertLine, goHorzLine, goRangeSelect, goEditing]
    TabOrder = 1
  end
  object StringGrid3: TStringGrid
    Left = 192
    Top = 64
    Width = 249
    Height = 249
    ColCount = 1
    FixedCols = 0
    RowCount = 4
    TabOrder = 2
  end
  object Edit1: TEdit
    Left = 56
    Top = 32
    Width = 33
    Height = 21
    TabOrder = 3
    Text = 'Edit1'
  end
  object Button1: TButton
    Left = 96
    Top = 32
    Width = 169
    Height = 25
    Caption = #1047#1072#1076#1072#1090#1100' '#1056#1072#1079#1084#1077#1088#1085#1086#1089#1090#1100
    TabOrder = 4
    OnClick = Button1Click
  end
  object RadioGroup1: TRadioGroup
    Left = 448
    Top = 64
    Width = 185
    Height = 105
    Caption = #1042#1099#1073#1086#1088
    TabOrder = 5
  end
  object RadioButton1: TRadioButton
    Left = 456
    Top = 80
    Width = 169
    Height = 17
    Caption = #1057#1082#1072#1083#1103#1088#1085#1086#1077' '#1087#1088#1086#1080#1079#1074#1077#1076#1077#1085#1080#1077
    TabOrder = 6
  end
  object RadioButton2: TRadioButton
    Left = 456
    Top = 96
    Width = 113
    Height = 17
    Caption = #1057#1091#1084#1084#1072
    TabOrder = 7
  end
  object RadioButton3: TRadioButton
    Left = 456
    Top = 112
    Width = 113
    Height = 17
    Caption = #1055#1088#1086#1080#1079#1074#1077#1076#1077#1085#1080#1077
    TabOrder = 8
  end
  object RadioButton4: TRadioButton
    Left = 456
    Top = 128
    Width = 113
    Height = 17
    Caption = #1057#1085#1072#1095#1072#1083#1072' 1 '#1087#1086#1090#1086#1084' 2'
    TabOrder = 9
  end
  object RadioButton5: TRadioButton
    Left = 456
    Top = 144
    Width = 113
    Height = 17
    Caption = #1063#1077#1090#1085#1099#1077' '#1085#1077#1095#1077#1090#1085#1099#1077
    TabOrder = 10
  end
  object Button2: TButton
    Left = 272
    Top = 32
    Width = 361
    Height = 25
    Caption = #1047#1072#1076#1072#1095#1072
    TabOrder = 11
    OnClick = Button2Click
  end
end
