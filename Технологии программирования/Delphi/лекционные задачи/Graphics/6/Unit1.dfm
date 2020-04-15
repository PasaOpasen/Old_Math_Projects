object Form1: TForm1
  Left = 256
  Top = 66
  Width = 928
  Height = 537
  Caption = #1057#1077#1084#1077#1089#1090#1088#1086#1074#1086#1077' '#8470'13, '#1055#1040#1057#1068#1050#1054
  Color = clMoneyGreen
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -19
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  PixelsPerInch = 96
  TextHeight = 24
  object PaintBox1: TPaintBox
    Left = 8
    Top = 8
    Width = 889
    Height = 185
  end
  object PaintBox2: TPaintBox
    Left = 8
    Top = 232
    Width = 889
    Height = 145
    Color = clRed
    ParentColor = False
  end
  object Label1: TLabel
    Left = 32
    Top = 392
    Width = 108
    Height = 24
    Caption = #1062#1074#1077#1090' '#1083#1080#1085#1080#1081':'
  end
  object Label2: TLabel
    Left = 184
    Top = 392
    Width = 100
    Height = 24
    Caption = #1062#1074#1077#1090' '#1092#1086#1085#1072':'
  end
  object Label3: TLabel
    Left = 336
    Top = 392
    Width = 119
    Height = 24
    Caption = #1057#1090#1080#1083#1100' '#1083#1080#1085#1080#1080':'
  end
  object ColorBox1: TColorBox
    Left = 32
    Top = 424
    Width = 145
    Height = 22
    DefaultColorColor = clGreen
    NoneColorColor = clGreen
    Selected = clGreen
    ItemHeight = 16
    TabOrder = 0
  end
  object Button1: TButton
    Left = 552
    Top = 448
    Width = 241
    Height = 33
    Caption = #1055#1088#1080#1084#1077#1088#1099' '#1089#1090#1080#1083#1077#1081
    TabOrder = 1
    OnClick = Button1Click
  end
  object Button3: TButton
    Left = 584
    Top = 392
    Width = 185
    Height = 41
    Caption = #1053#1072#1088#1080#1089#1086#1074#1072#1090#1100' '#1083#1080#1085#1080#1102
    TabOrder = 2
    OnClick = Button3Click
  end
  object ComboBox1: TComboBox
    Left = 336
    Top = 424
    Width = 145
    Height = 32
    ItemHeight = 24
    ItemIndex = 0
    TabOrder = 3
    Text = 'psSolid'
    Items.Strings = (
      'psSolid'
      'psDash'
      'psDot'
      'psDashDot'
      'psDashDotDot'
      'psClear'
      'psInsideFrame')
  end
  object ColorBox2: TColorBox
    Left = 184
    Top = 424
    Width = 145
    Height = 22
    ItemHeight = 16
    TabOrder = 4
  end
end
