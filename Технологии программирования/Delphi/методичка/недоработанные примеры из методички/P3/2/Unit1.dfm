object Form1: TForm1
  Left = 215
  Top = 209
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
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 13
  object Label1: TLabel
    Left = 48
    Top = 72
    Width = 74
    Height = 13
    Caption = #1042#1074#1077#1076#1080#1090#1077' '#1095#1080#1089#1083#1086
  end
  object Label2: TLabel
    Left = 48
    Top = 96
    Width = 13
    Height = 13
    Caption = #1061'='
  end
  object Edit1: TEdit
    Left = 64
    Top = 96
    Width = 41
    Height = 21
    TabOrder = 0
    Text = 'Edit1'
  end
  object GroupBox1: TGroupBox
    Left = 152
    Top = 72
    Width = 145
    Height = 105
    Caption = 'GroupBox1'
    TabOrder = 1
    object CheckBox1: TCheckBox
      Left = 8
      Top = 24
      Width = 97
      Height = 17
      Caption = #1050#1088#1072#1090#1085#1086' 3'
      TabOrder = 0
    end
    object CheckBox2: TCheckBox
      Left = 8
      Top = 48
      Width = 97
      Height = 17
      Caption = #1050#1088#1072#1090#1085#1086' 5'
      TabOrder = 1
    end
    object CheckBox3: TCheckBox
      Left = 8
      Top = 72
      Width = 97
      Height = 17
      Caption = #1050#1088#1072#1090#1085#1086' 7'
      TabOrder = 2
    end
  end
  object Button1: TButton
    Left = 304
    Top = 72
    Width = 225
    Height = 17
    Caption = #1055#1088#1086#1074#1077#1088#1080#1090#1100' '#1082#1088#1072#1090#1085#1086#1089#1090#1100' '#1095#1080#1089#1083#1072
    TabOrder = 2
    OnClick = Button1Click
  end
  object Memo1: TMemo
    Left = 304
    Top = 96
    Width = 225
    Height = 81
    Lines.Strings = (
      'Memo1')
    ScrollBars = ssBoth
    TabOrder = 3
  end
end
