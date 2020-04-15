object Form1: TForm1
  Left = 276
  Top = 244
  Width = 928
  Height = 480
  Caption = 'Form1'
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
    Left = 8
    Top = 8
    Width = 73
    Height = 13
    Caption = #1042#1074#1077#1076#1080#1090#1077' '#1090#1077#1082#1089#1090
  end
  object Label2: TLabel
    Left = 8
    Top = 152
    Width = 36
    Height = 13
    Caption = #1054#1090#1074#1077#1090'='
  end
  object Label3: TLabel
    Left = 48
    Top = 152
    Width = 47
    Height = 13
    Caption = #1055#1086#1083#1091#1095#1080#1090#1100
    OnClick = ComboBox1Click
  end
  object Edit1: TEdit
    Left = 8
    Top = 32
    Width = 265
    Height = 21
    TabOrder = 0
    Text = #1042#1074#1077#1076#1080#1090#1077' '#1089#1090#1088#1086#1082#1091' '#1080' '#1085#1072#1078#1084#1080#1090#1077' '#1080#1085#1090#1077#1088
    OnKeyPress = ComboBox1KeyPress
  end
  object BitBtn1: TBitBtn
    Left = 104
    Top = 192
    Width = 75
    Height = 25
    Caption = 'BitBtn1'
    TabOrder = 1
  end
  object ComboBox1: TComboBox
    Left = 8
    Top = 64
    Width = 265
    Height = 21
    ItemHeight = 13
    TabOrder = 2
  end
end
