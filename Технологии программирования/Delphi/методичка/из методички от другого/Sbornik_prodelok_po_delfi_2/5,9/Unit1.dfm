object Form1: TForm1
  Left = 212
  Top = 200
  Width = 745
  Height = 484
  Caption = #1059#1087#1088#1072#1078#1085#1077#1085#1080#1077' 5.9 ()'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  PixelsPerInch = 96
  TextHeight = 13
  object Label1: TLabel
    Left = 16
    Top = 16
    Width = 244
    Height = 16
    Caption = #1042#1074#1077#1076#1080#1090#1077' '#1089#1090#1088#1086#1082#1091' '#1080' '#1085#1072#1078#1084#1080#1090#1077' Enter:'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -13
    Font.Name = 'MS Sans Serif'
    Font.Style = [fsBold]
    ParentFont = False
  end
  object Edit1: TEdit
    Left = 16
    Top = 48
    Width = 345
    Height = 24
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -13
    Font.Name = 'MS Sans Serif'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 0
    Text = #1042#1074#1077#1076#1080#1090#1077' '#1089#1090#1088#1086#1082#1091' '#1080' '#1085#1072#1078#1084#1080#1090#1077' Enter'
    OnKeyPress = Edit1KeyPress
  end
  object ListBox1: TListBox
    Left = 16
    Top = 88
    Width = 345
    Height = 201
    ItemHeight = 13
    TabOrder = 1
  end
  object ButtonTestData: TButton
    Left = 16
    Top = 312
    Width = 345
    Height = 25
    Caption = #1042#1074#1077#1089#1090#1080' '#1090#1077#1089#1090#1086#1074#1099#1081' '#1087#1088#1080#1084#1077#1088' '#1080#1079' '#1092#1072#1081#1083#1072
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -13
    Font.Name = 'MS Sans Serif'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 2
    OnClick = ButtonTestDataClick
  end
  object ButtonClearListBox: TButton
    Left = 16
    Top = 360
    Width = 345
    Height = 25
    Caption = #1054#1095#1080#1089#1090#1080#1090#1100' '#1089#1087#1080#1089#1086#1082' '#1089#1090#1088#1086#1082
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -13
    Font.Name = 'MS Sans Serif'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 3
    OnClick = ButtonClearListBoxClick
  end
  object ButtonShowLower: TButton
    Left = 392
    Top = 16
    Width = 329
    Height = 33
    Caption = #1042#1099#1074#1077#1089#1090#1080' '#1089#1090#1088#1086#1095#1085#1099#1077' '#1088#1091#1089#1089#1082#1080#1077' '#1073#1091#1082#1074#1099
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -13
    Font.Name = 'MS Sans Serif'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 4
    OnClick = ButtonShowLowerClick
  end
  object ButtonShowUpper: TButton
    Left = 392
    Top = 144
    Width = 329
    Height = 33
    Caption = #1042#1099#1074#1077#1089#1090#1080' '#1087#1088#1086#1087#1080#1089#1085#1099#1077' '#1088#1091#1089#1089#1082#1080#1077' '#1073#1091#1082#1074#1099
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -13
    Font.Name = 'MS Sans Serif'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 5
    OnClick = ButtonShowUpperClick
  end
  object BitBtn1: TBitBtn
    Left = 16
    Top = 408
    Width = 705
    Height = 25
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -13
    Font.Name = 'MS Sans Serif'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 6
    Kind = bkClose
  end
  object PanelShowLower: TPanel
    Left = 392
    Top = 72
    Width = 329
    Height = 25
    Caption = '     '
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -13
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ParentFont = False
    TabOrder = 7
  end
  object PanelShowUpper: TPanel
    Left = 392
    Top = 200
    Width = 329
    Height = 25
    Caption = '   '
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -13
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ParentFont = False
    TabOrder = 8
  end
end
