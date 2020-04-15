object Form1: TForm1
  Left = 459
  Top = 252
  Width = 794
  Height = 291
  Caption = #1079#1072#1076#1072#1085#1080#1077' 6.6  '#1055#1040#1057#1068#1050#1054
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -19
  Font.Name = 'Tahoma'
  Font.Style = []
  OldCreateOrder = False
  Position = poScreenCenter
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 23
  object Label1: TLabel
    Left = 8
    Top = 8
    Width = 109
    Height = 23
    Caption = #1056#1072#1089#1087#1080#1089#1072#1085#1080#1077':'
  end
  object StringGrid1: TStringGrid
    Left = 8
    Top = 32
    Width = 529
    Height = 137
    ColCount = 4
    FixedCols = 0
    Options = [goFixedVertLine, goFixedHorzLine, goVertLine, goHorzLine, goRangeSelect, goEditing]
    TabOrder = 0
    ColWidths = (
      86
      129
      172
      130)
  end
  object btnLoad: TButton
    Left = 136
    Top = 184
    Width = 369
    Height = 25
    Caption = #1055#1086#1083#1091#1095#1080#1090#1100' '#1076#1072#1085#1085#1099#1077' '#1080#1079' '#1092#1072#1081#1083#1072
    TabOrder = 1
    OnClick = btnLoadClick
  end
  object btnSave: TButton
    Left = 136
    Top = 216
    Width = 369
    Height = 25
    Caption = #1047#1072#1087#1080#1089#1072#1090#1100' '#1076#1072#1085#1085#1099#1077' '#1074' '#1092#1072#1081#1083
    TabOrder = 2
    OnClick = btnSaveClick
  end
  object Button3: TButton
    Left = 552
    Top = 32
    Width = 209
    Height = 65
    Caption = #1054#1090#1089#1086#1088#1090#1080#1088#1086#1074#1072#1090#1100' '#1087#1086' '#1091#1073#1099#1074#1072#1085#1080#1102
    TabOrder = 3
    WordWrap = True
    OnClick = Button3Click
  end
  object Button4: TButton
    Left = 552
    Top = 104
    Width = 209
    Height = 73
    Caption = #1054#1090#1089#1086#1088#1090#1080#1088#1086#1074#1072#1090#1100' '#1087#1086' '#1074#1086#1079#1088#1072#1089#1090#1072#1085#1080#1102
    TabOrder = 4
    WordWrap = True
    OnClick = Button4Click
  end
end
