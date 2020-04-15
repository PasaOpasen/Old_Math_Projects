object Form1: TForm1
  Left = 534
  Top = 290
  Width = 557
  Height = 368
  Caption = #1055#1040#1057#1068#1050#1054', '#1074#1086#1087#1088#1086#1089' 12'
  Color = clMoneyGreen
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
    Left = 16
    Top = 16
    Width = 129
    Height = 23
    Caption = #1042#1074#1077#1076#1080#1090#1077' '#1095#1080#1089#1083#1086
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
  end
  object Label2: TLabel
    Left = 176
    Top = 24
    Width = 25
    Height = 23
    Caption = 'X='
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
  end
  object x: TEdit
    Left = 208
    Top = 24
    Width = 65
    Height = 25
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
    TabOrder = 0
    Text = '21'
  end
  object GroupBox1: TGroupBox
    Left = 15
    Top = 72
    Width = 186
    Height = 217
    Caption = #1042#1099#1073#1077#1088#1080#1090#1077' '#1101#1083#1077#1084#1077#1085#1090
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
    TabOrder = 1
    object CkBx3: TCheckBox
      Left = 16
      Top = 56
      Width = 177
      Height = 25
      Caption = #1082#1088#1072#1090#1085#1086' 3'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clBlue
      Font.Height = -24
      Font.Name = 'MS Sans Serif'
      Font.Style = [fsBold]
      ParentFont = False
      TabOrder = 0
    end
    object CkBx5: TCheckBox
      Left = 16
      Top = 112
      Width = 169
      Height = 25
      Caption = #1082#1088#1072#1090#1085#1086' 5'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clBlue
      Font.Height = -24
      Font.Name = 'MS Sans Serif'
      Font.Style = [fsBold]
      ParentFont = False
      TabOrder = 1
    end
    object CkBx7: TCheckBox
      Left = 16
      Top = 168
      Width = 169
      Height = 25
      Caption = #1082#1088#1072#1090#1085#1086' 7'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clBlue
      Font.Height = -24
      Font.Name = 'MS Sans Serif'
      Font.Style = [fsBold]
      ParentFont = False
      TabOrder = 2
    end
  end
  object Button1: TButton
    Left = 232
    Top = 72
    Width = 289
    Height = 33
    Caption = #1055#1088#1086#1074#1077#1088#1080#1090#1100' '#1082#1088#1072#1090#1085#1086#1089#1090#1100' '#1095#1080#1089#1083#1072' x'
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
    TabOrder = 2
    OnClick = Button1Click
  end
  object Memo1: TMemo
    Left = 232
    Top = 120
    Width = 289
    Height = 185
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
    ScrollBars = ssBoth
    TabOrder = 3
  end
end
