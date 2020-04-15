object Form1: TForm1
  Left = 402
  Top = 192
  Width = 520
  Height = 379
  Caption = 'Drag&Drop'
  Color = clSkyBlue
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  OldCreateOrder = False
  PixelsPerInch = 96
  TextHeight = 13
  object lblHintLabel: TLabel
    Left = 80
    Top = 32
    Width = 328
    Height = 19
    Caption = #1055#1077#1088#1077#1090#1072#1097#1080#1090#1077' '#1086#1073#1098#1077#1082#1090' '#1074' '#1086#1082#1085#1086' '#1080' '#1074#1099' '#1091#1074#1080#1076#1080#1090#1077' '#1077#1075#1086' '#1080#1084#1103
    DragMode = dmAutomatic
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object lblSecondHint: TLabel
    Left = 96
    Top = 224
    Width = 306
    Height = 19
    Caption = #1054#1073#1098#1077#1082#1090#1099' '#1085#1080#1078#1077' '#1087#1077#1088#1077#1090#1072#1089#1082#1080#1074#1072#1085#1080#1102' '#1085#1077' '#1087#1086#1076#1083#1077#1078#1072#1090
    DragMode = dmAutomatic
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object mmo1: TMemo
    Left = 0
    Top = 64
    Width = 504
    Height = 65
    Align = alCustom
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold, fsItalic]
    ParentFont = False
    ReadOnly = True
    TabOrder = 0
    OnDragDrop = mmo1DragDrop
    OnDragOver = mmo1DragOver
  end
  object bbOK: TBitBtn
    Left = 40
    Top = 176
    Width = 91
    Height = 33
    DragMode = dmAutomatic
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
    TabOrder = 1
    Kind = bkOK
  end
  object bbAll: TBitBtn
    Left = 208
    Top = 176
    Width = 91
    Height = 33
    DragMode = dmAutomatic
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
    TabOrder = 2
    Kind = bkAll
  end
  object bbIgnore: TBitBtn
    Left = 368
    Top = 176
    Width = 91
    Height = 33
    DragMode = dmAutomatic
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
    TabOrder = 3
    Kind = bkIgnore
  end
  object bbClose: TBitBtn
    Left = 216
    Top = 304
    Width = 75
    Height = 25
    TabOrder = 4
    OnClick = bbCloseClick
    Kind = bkClose
  end
  object cbWindow: TColorBox
    Left = 40
    Top = 256
    Width = 177
    Height = 26
    Selected = clSkyBlue
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ItemHeight = 20
    ParentFont = False
    TabOrder = 5
  end
  object bbWindow: TButton
    Left = 280
    Top = 256
    Width = 177
    Height = 25
    Caption = #1048#1079#1084#1077#1085#1080#1090#1100' '#1094#1074#1077#1090' '#1092#1086#1088#1084#1099
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
    TabOrder = 6
    OnClick = bbWindowClick
  end
end
