object Form1: TForm1
  Left = 303
  Top = 233
  Width = 711
  Height = 391
  Caption = #1055#1088#1080#1084#1077#1088' 4.15'
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
    Left = 8
    Top = 16
    Width = 163
    Height = 13
    Caption = #1042#1074#1077#1076#1080#1090#1077' '#1088#1072#1079#1084#1077#1088#1085#1086#1089#1090#1100' '#1084#1072#1090#1088#1080#1094#1099':'
  end
  object Label2: TLabel
    Left = 8
    Top = 44
    Width = 39
    Height = 13
    Caption = #1057#1090#1088#1086#1082#1080':'
  end
  object Label3: TLabel
    Left = 128
    Top = 44
    Width = 47
    Height = 13
    Caption = #1057#1090#1086#1083#1073#1094#1099':'
  end
  object EditNN: TEdit
    Left = 56
    Top = 40
    Width = 41
    Height = 21
    TabOrder = 0
    Text = '4'
  end
  object SetDim: TButton
    Left = 8
    Top = 80
    Width = 217
    Height = 33
    Caption = #1047#1072#1076#1072#1090#1100' '#1088#1072#1079#1084#1077#1088#1085#1086#1089#1090#1100
    TabOrder = 1
    OnClick = SetDimClick
  end
  object GroupBox1: TGroupBox
    Left = 8
    Top = 152
    Width = 217
    Height = 129
    Caption = #1042#1099#1073#1077#1088#1080#1090#1077' '#1088#1072#1079#1084#1077#1088#1085#1086#1089#1090#1100' '#1084#1072#1090#1088#1080#1094#1099
    TabOrder = 2
    object RadioButton3: TRadioButton
      Left = 8
      Top = 32
      Width = 113
      Height = 17
      Caption = '3 x 3'
      TabOrder = 0
      OnClick = RadioButton3Click
    end
    object RadioButton4: TRadioButton
      Left = 8
      Top = 64
      Width = 113
      Height = 17
      Caption = '4 x 4'
      Checked = True
      TabOrder = 1
      TabStop = True
      OnClick = RadioButton3Click
    end
    object RadioButton5: TRadioButton
      Left = 8
      Top = 96
      Width = 113
      Height = 17
      Caption = '5 x 5'
      TabOrder = 2
      OnClick = RadioButton3Click
    end
  end
  object TestData: TButton
    Left = 8
    Top = 312
    Width = 217
    Height = 33
    Caption = #1042#1074#1077#1089#1090#1080' '#1090#1077#1089#1090#1086#1074#1099#1077' '#1076#1072#1085#1085#1099#1077
    TabOrder = 3
    OnClick = TestDataClick
  end
  object StringGrid1: TStringGrid
    Left = 272
    Top = 104
    Width = 417
    Height = 241
    Options = [goFixedVertLine, goFixedHorzLine, goVertLine, goHorzLine, goRangeSelect, goEditing]
    TabOrder = 4
  end
  object SortRow: TButton
    Left = 272
    Top = 16
    Width = 417
    Height = 25
    Caption = 
      #1054#1090#1089#1086#1088#1090#1080#1088#1086#1074#1072#1090#1100' '#1089#1090#1088#1086#1082#1080' '#1074' '#1087#1086#1088#1103#1076#1082#1077' '#1074#1086#1079#1088#1072#1089#1090#1072#1085#1080#1103'  '#1101#1083#1077#1084#1077#1085#1090#1086#1074' '#1087#1077#1088#1074#1086#1075#1086' '#1089#1090 +
      #1086#1083#1073#1094#1072
    TabOrder = 5
    OnClick = SortRowClick
  end
  object SortCol: TButton
    Left = 272
    Top = 56
    Width = 417
    Height = 25
    Caption = 
      #1054#1090#1089#1086#1088#1090#1080#1088#1086#1074#1072#1090#1100' '#1089#1090#1086#1083#1073#1094#1099' '#1074' '#1087#1086#1088#1103#1076#1082#1077' '#1074#1086#1079#1088#1072#1089#1090#1072#1085#1080#1103' '#1101#1083#1077#1084#1077#1085#1090#1086#1074' '#1087#1077#1088#1074#1086#1081' '#1089#1090#1088 +
      #1086#1082#1080
    TabOrder = 6
    OnClick = SortColClick
  end
  object EditMM: TEdit
    Left = 184
    Top = 40
    Width = 41
    Height = 21
    TabOrder = 7
    Text = '4'
  end
end
