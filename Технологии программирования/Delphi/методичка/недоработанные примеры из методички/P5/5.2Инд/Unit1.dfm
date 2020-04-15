object Form1: TForm1
  Left = 217
  Top = 149
  Width = 878
  Height = 466
  Caption = #1055#1088#1080#1084#1077#1088' 5.2'
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
  object LabelInt: TLabel
    Left = 224
    Top = 296
    Width = 15
    Height = 16
    Caption = '     '
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -13
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ParentFont = False
  end
  object LabelFloat1: TLabel
    Left = 16
    Top = 296
    Width = 15
    Height = 16
    Caption = '     '
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -13
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ParentFont = False
  end
  object Edit1: TEdit
    Left = 432
    Top = 32
    Width = 313
    Height = 24
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -13
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ParentFont = False
    TabOrder = 0
    Text = #1042#1074#1077#1076#1080#1090#1077' '#1089#1090#1088#1086#1082#1091' '#1080' '#1085#1072#1078#1084#1080#1090#1077' Enter'
    OnKeyPress = Edit1KeyPress
  end
  object ListBox1: TListBox
    Left = 432
    Top = 56
    Width = 313
    Height = 217
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -13
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ItemHeight = 16
    ParentFont = False
    TabOrder = 1
  end
  object ButtonTestData: TButton
    Left = 16
    Top = 128
    Width = 385
    Height = 41
    Caption = #1042#1074#1077#1089#1090#1080' '#1090#1077#1089#1090#1086#1074#1099#1081' '#1087#1088#1080#1084#1077#1088' '#1080#1079' '#1092#1072#1081#1083#1072
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -13
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ParentFont = False
    TabOrder = 2
    OnClick = ButtonTestDataClick
  end
  object ButtonClearListBox: TButton
    Left = 16
    Top = 184
    Width = 385
    Height = 33
    Caption = #1054#1095#1080#1089#1090#1080#1090#1100' '#1089#1087#1080#1089#1086#1082' '#1089#1090#1088#1086#1082
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -13
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ParentFont = False
    TabOrder = 3
    OnClick = ButtonClearListBoxClick
  end
  object BitBtn1: TBitBtn
    Left = 16
    Top = 232
    Width = 385
    Height = 33
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -13
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ParentFont = False
    TabOrder = 4
    Kind = bkClose
  end
  object ButtonInt: TButton
    Left = 16
    Top = 80
    Width = 385
    Height = 33
    Caption = #1042#1099#1074#1077#1089#1090#1080' '#1094#1077#1083#1099#1077' '#1095#1080#1089#1083#1072
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -13
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ParentFont = False
    TabOrder = 5
    OnClick = ButtonIntClick
  end
  object ButtonFloat1: TButton
    Left = 16
    Top = 32
    Width = 385
    Height = 33
    Caption = #1042#1099#1074#1077#1089#1090#1080' '#1074#1077#1097#1077#1089#1090#1074#1077#1085#1085#1099#1077' '#1095#1080#1089#1083#1072' '#1089' '#1092#1080#1082#1089#1080#1088#1086#1074#1072#1085#1085#1086#1081' '#1090#1086#1095#1082#1086#1081
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -13
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ParentFont = False
    TabOrder = 6
    OnClick = ButtonFloat1Click
  end
end
