object Form1: TForm1
  Left = 192
  Top = 125
  Width = 972
  Height = 561
  Caption = #8470'13'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  OldCreateOrder = False
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 13
  object pbPen: TPaintBox
    Left = 0
    Top = 261
    Width = 956
    Height = 81
    Align = alBottom
  end
  object pbOut: TPaintBox
    Left = 0
    Top = 0
    Width = 956
    Height = 261
    Align = alClient
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object pnl1: TPanel
    Left = 0
    Top = 342
    Width = 956
    Height = 180
    Align = alBottom
    Color = clSkyBlue
    TabOrder = 0
    object lbl1: TLabel
      Left = 8
      Top = 8
      Width = 168
      Height = 19
      Caption = #1065#1077#1083#1082#1085#1080#1090#1077' '#1087#1086' '#1082#1085#1086#1087#1082#1077' '#1054#1050
      Font.Charset = RUSSIAN_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = []
      ParentFont = False
    end
    object lbl2: TLabel
      Left = 408
      Top = 96
      Width = 87
      Height = 19
      Caption = #1056#1077#1078#1080#1084' Mode'
      Font.Charset = RUSSIAN_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = []
      ParentFont = False
    end
    object lbl3: TLabel
      Left = 408
      Top = 32
      Width = 81
      Height = 19
      Caption = #1062#1074#1077#1090' '#1083#1080#1085#1080#1080
      Font.Charset = RUSSIAN_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = []
      ParentFont = False
    end
    object lbl4: TLabel
      Left = 672
      Top = 32
      Width = 70
      Height = 19
      Caption = #1062#1074#1077#1090' '#1092#1086#1085#1072
      Font.Charset = RUSSIAN_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = []
      ParentFont = False
    end
    object cbPen: TColorBox
      Left = 408
      Top = 56
      Width = 201
      Height = 22
      Font.Charset = RUSSIAN_CHARSET
      Font.Color = clWindowText
      Font.Height = -19
      Font.Name = 'Times New Roman'
      Font.Style = []
      ItemHeight = 16
      ParentFont = False
      TabOrder = 0
    end
    object cbBack: TColorBox
      Left = 672
      Top = 56
      Width = 185
      Height = 22
      Font.Charset = RUSSIAN_CHARSET
      Font.Color = clWindowText
      Font.Height = -19
      Font.Name = 'Times New Roman'
      Font.Style = []
      ItemHeight = 16
      ParentFont = False
      TabOrder = 1
    end
    object cbMode: TComboBox
      Left = 408
      Top = 120
      Width = 201
      Height = 29
      Font.Charset = RUSSIAN_CHARSET
      Font.Color = clWindowText
      Font.Height = -19
      Font.Name = 'Times New Roman'
      Font.Style = []
      ItemHeight = 21
      ParentFont = False
      TabOrder = 2
      Text = 'cbMode'
    end
    object bbPen: TButton
      Left = 672
      Top = 120
      Width = 113
      Height = 33
      Caption = #1051#1080#1085#1080#1103
      Font.Charset = RUSSIAN_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = []
      ParentFont = False
      TabOrder = 3
      OnClick = bbPenClick
    end
    object btn1: TBitBtn
      Left = 160
      Top = 80
      Width = 89
      Height = 33
      TabOrder = 4
      Kind = bkClose
    end
    object btn2: TBitBtn
      Left = 40
      Top = 80
      Width = 89
      Height = 33
      TabOrder = 5
      OnClick = btn2Click
      Kind = bkOK
    end
  end
end
