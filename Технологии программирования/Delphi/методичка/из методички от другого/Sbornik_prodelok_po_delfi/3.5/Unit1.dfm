object Form1: TForm1
  Left = 231
  Top = 125
  Width = 558
  Height = 336
  Caption = '3.5'
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
    Left = 32
    Top = 24
    Width = 140
    Height = 13
    Caption = #1042#1074#1077#1076#1080#1090#1077' '#1082#1086#1086#1088#1076#1080#1085#1072#1090#1099' '#1090#1086#1095#1082#1080' '
  end
  object Label2: TLabel
    Left = 32
    Top = 56
    Width = 19
    Height = 13
    Caption = 'X = '
  end
  object Label3: TLabel
    Left = 32
    Top = 88
    Width = 19
    Height = 13
    Caption = 'Y = '
  end
  object EditX: TEdit
    Left = 56
    Top = 52
    Width = 81
    Height = 21
    TabOrder = 0
    Text = '1'
  end
  object EditY: TEdit
    Left = 56
    Top = 84
    Width = 81
    Height = 21
    TabOrder = 1
    Text = '1'
  end
  object GroupBox1: TGroupBox
    Left = 264
    Top = 24
    Width = 209
    Height = 97
    Caption = #1055#1088#1080#1085#1072#1076#1083#1077#1078#1080#1090' '#1083#1080' '#1090#1086#1095#1082#1072
    TabOrder = 2
    object treug: TCheckBox
      Left = 8
      Top = 24
      Width = 185
      Height = 17
      Caption = #1058#1088#1077#1091#1075#1086#1083#1100#1085#1080#1082#1091': x>0,y>0, x+y<5'
      Checked = True
      State = cbChecked
      TabOrder = 0
    end
    object praymoug: TCheckBox
      Left = 8
      Top = 48
      Width = 185
      Height = 17
      Caption = #1055#1088#1103#1084#1086#1091#1075#1086#1083#1100#1085#1080#1082#1091': 0,5<x<3, -2<y<4 '
      TabOrder = 1
    end
    object Krug: TCheckBox
      Left = 8
      Top = 72
      Width = 185
      Height = 17
      Caption = #1050#1088#1091#1075#1091': x^2+y^2<=7'
      TabOrder = 2
    end
  end
  object Button1: TButton
    Left = 32
    Top = 168
    Width = 153
    Height = 89
    Caption = #1055#1088#1086#1074#1077#1088#1080#1090#1100
    TabOrder = 3
    OnClick = Button1Click
  end
  object Memo1: TMemo
    Left = 224
    Top = 140
    Width = 289
    Height = 145
    Lines.Strings = (
      'Memo1')
    ScrollBars = ssBoth
    TabOrder = 4
  end
end
