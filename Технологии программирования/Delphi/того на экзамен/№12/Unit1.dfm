object Form1: TForm1
  Left = 192
  Top = 125
  Width = 923
  Height = 490
  Caption = #8470'12'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  OldCreateOrder = False
  PixelsPerInch = 96
  TextHeight = 13
  object pb1: TPaintBox
    Left = 0
    Top = 0
    Width = 905
    Height = 313
    Color = clScrollBar
    ParentColor = False
  end
  object pnl1: TPanel
    Left = -8
    Top = 312
    Width = 912
    Height = 137
    Color = clSkyBlue
    TabOrder = 0
    object lbl1: TLabel
      Left = 16
      Top = 8
      Width = 198
      Height = 21
      Caption = #1065#1077#1083#1082#1085#1080#1090#1077' '#1087#1086' '#1082#1085#1086#1087#1082#1077' '#1054#1050
      Font.Charset = RUSSIAN_CHARSET
      Font.Color = clWindowText
      Font.Height = -19
      Font.Name = 'Times New Roman'
      Font.Style = []
      ParentFont = False
    end
    object btn1: TBitBtn
      Left = 56
      Top = 80
      Width = 89
      Height = 33
      TabOrder = 0
      OnClick = btn1Click
      Kind = bkOK
    end
    object btn2: TBitBtn
      Left = 192
      Top = 80
      Width = 89
      Height = 33
      TabOrder = 1
      Kind = bkClose
    end
    object btn3: TButton
      Left = 544
      Top = 80
      Width = 95
      Height = 35
      Caption = #1064#1088#1080#1092#1090
      Font.Charset = RUSSIAN_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = []
      ParentFont = False
      TabOrder = 2
      Visible = False
      OnClick = btn3Click
    end
    object btn4: TButton
      Left = 720
      Top = 80
      Width = 95
      Height = 35
      Caption = #1042#1099#1074#1086#1076
      Font.Charset = RUSSIAN_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = []
      ParentFont = False
      TabOrder = 3
      Visible = False
      OnClick = btn4Click
    end
    object edt1: TEdit
      Left = 280
      Top = 40
      Width = 329
      Height = 27
      Font.Charset = RUSSIAN_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = []
      ParentFont = False
      TabOrder = 4
      Visible = False
    end
  end
  object dlgFont1: TFontDialog
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -11
    Font.Name = 'Tahoma'
    Font.Style = []
    Left = 696
    Top = 144
  end
end
