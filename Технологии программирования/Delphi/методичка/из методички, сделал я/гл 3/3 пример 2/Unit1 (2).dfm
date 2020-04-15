object Form1: TForm1
  Left = 192
  Top = 114
  Width = 518
  Height = 429
  Caption = #1055#1040#1057#1068#1050#1054' '#1087#1072#1088#1072#1075#1088#1072#1092' 3 '#1087#1088#1080#1084#1077#1088' 2'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -24
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 29
  object Label1: TLabel
    Left = 8
    Top = 24
    Width = 174
    Height = 29
    Caption = #1042#1074#1077#1076#1080#1090#1077' '#1095#1080#1089#1083#1086
  end
  object Label2: TLabel
    Left = 16
    Top = 80
    Width = 31
    Height = 29
    Caption = 'X='
  end
  object x: TEdit
    Left = 48
    Top = 80
    Width = 121
    Height = 37
    TabOrder = 0
    Text = '42'
  end
  object GroupBox1: TGroupBox
    Left = 240
    Top = 32
    Width = 241
    Height = 177
    Caption = #1042#1099#1073#1077#1088#1080#1090#1077' '#1101#1083#1077#1084#1077#1085#1090
    TabOrder = 1
    object CkBx3: TCheckBox
      Left = 56
      Top = 32
      Width = 137
      Height = 33
      Caption = #1082#1088#1072#1090#1085#1086' 3'
      TabOrder = 0
    end
    object CkBx5: TCheckBox
      Left = 56
      Top = 64
      Width = 137
      Height = 33
      Caption = #1082#1088#1072#1090#1085#1086' 5'
      TabOrder = 1
    end
    object CkBx7: TCheckBox
      Left = 56
      Top = 96
      Width = 137
      Height = 33
      Caption = #1082#1088#1072#1090#1085#1086' 7'
      TabOrder = 2
    end
  end
  object Button1: TButton
    Left = 72
    Top = 224
    Width = 369
    Height = 33
    Caption = #1055#1088#1086#1074#1077#1088#1080#1090#1100' '#1082#1088#1072#1090#1085#1086#1089#1090#1100' '#1095#1080#1089#1083#1072' '#1061
    TabOrder = 2
    OnClick = Button1Click
  end
  object Memo1: TMemo
    Left = 72
    Top = 272
    Width = 369
    Height = 97
    ScrollBars = ssBoth
    TabOrder = 3
  end
end
