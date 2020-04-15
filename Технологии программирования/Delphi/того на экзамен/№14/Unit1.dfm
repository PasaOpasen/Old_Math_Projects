object Form1: TForm1
  Left = 137
  Top = 127
  Width = 1099
  Height = 692
  Caption = #8470'14'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  OldCreateOrder = False
  PixelsPerInch = 96
  TextHeight = 13
  object pbOut: TPaintBox
    Left = 0
    Top = 0
    Width = 1083
    Height = 404
    Align = alClient
  end
  object pbBit: TPaintBox
    Left = 0
    Top = 407
    Width = 1083
    Height = 105
    Align = alBottom
  end
  object spl1: TSplitter
    Left = 0
    Top = 404
    Width = 1083
    Height = 3
    Cursor = crVSplit
    Align = alBottom
    Color = clBackground
    ParentColor = False
  end
  object pnl1: TPanel
    Left = 0
    Top = 512
    Width = 1083
    Height = 141
    Align = alBottom
    Color = clSkyBlue
    TabOrder = 0
    object lbl1: TLabel
      Left = 464
      Top = 32
      Width = 79
      Height = 19
      Caption = #1062#1074#1077#1090' '#1082#1080#1089#1090#1080':'
      Font.Charset = RUSSIAN_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = []
      ParentFont = False
    end
    object btn2: TBitBtn
      Left = 8
      Top = 80
      Width = 105
      Height = 33
      Font.Charset = RUSSIAN_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = []
      ParentFont = False
      TabOrder = 0
      Kind = bkClose
    end
    object cbBrushColor: TColorBox
      Left = 552
      Top = 32
      Width = 177
      Height = 22
      Font.Charset = RUSSIAN_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = []
      ItemHeight = 16
      ParentFont = False
      TabOrder = 1
    end
    object btn3: TBitBtn
      Left = 552
      Top = 80
      Width = 185
      Height = 33
      Caption = #1042#1099#1074#1077#1089#1090#1080' '#1089#1090#1080#1083#1080
      Font.Charset = RUSSIAN_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = []
      ParentFont = False
      TabOrder = 2
      OnClick = btn3Click
      Kind = bkOK
    end
    object bb1: TButton
      Left = 288
      Top = 80
      Width = 169
      Height = 33
      Caption = #1047#1072#1075#1088#1091#1079#1080#1090#1100' '#1086#1073#1088#1072#1079#1077#1094
      Font.Charset = RUSSIAN_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = []
      ParentFont = False
      TabOrder = 3
      OnClick = bb1Click
    end
  end
  object dlgOpenPic1: TOpenPictureDialog
    Left = 720
    Top = 184
  end
end
