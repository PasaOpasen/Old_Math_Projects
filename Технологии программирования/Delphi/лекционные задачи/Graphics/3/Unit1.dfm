object Form1: TForm1
  Left = 192
  Top = 124
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
  object Shape1: TShape
    Left = 32
    Top = 24
    Width = 361
    Height = 225
    Shape = stSquare
  end
  object ComboBox1: TComboBox
    Left = 416
    Top = 24
    Width = 145
    Height = 21
    ItemHeight = 13
    ItemIndex = 0
    TabOrder = 0
    Text = 'stRectangle'
    Items.Strings = (
      'stRectangle'
      'stSquare'
      'stRoundRect'
      'stRoundSquare'
      'stEllipse'
      'stCircle')
  end
  object Button1: TButton
    Left = 416
    Top = 168
    Width = 297
    Height = 57
    Caption = #1042#1099#1087#1086#1083#1085#1080#1090#1100
    TabOrder = 1
    OnClick = Button1Click
  end
  object ColorBox1: TColorBox
    Left = 568
    Top = 72
    Width = 145
    Height = 22
    ItemHeight = 16
    TabOrder = 2
  end
  object ColorBox2: TColorBox
    Left = 568
    Top = 128
    Width = 145
    Height = 22
    ItemHeight = 16
    TabOrder = 3
  end
  object ComboBox2: TComboBox
    Left = 416
    Top = 72
    Width = 145
    Height = 21
    ItemHeight = 13
    TabOrder = 4
    Text = 'bsSolid'
    Items.Strings = (
      'bsSolid'
      'bsClear'
      'bsHorizontal'
      'bsVertical'
      'bsFDiagonal'
      'bsBDiagonal'
      'bsCross'
      'bsDiagCross')
  end
  object ComboBox3: TComboBox
    Left = 416
    Top = 128
    Width = 145
    Height = 21
    ItemHeight = 13
    TabOrder = 5
    Text = 'bsSolid'
    Items.Strings = (
      'psSolid'
      'psDash'
      'psDot'
      'psDashDot'
      'psDashDotDot'
      'psClear'
      'psInsideframe'
      '')
  end
end
