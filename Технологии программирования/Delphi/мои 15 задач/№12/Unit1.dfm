object Form1: TForm1
  Left = 192
  Top = 127
  Width = 928
  Height = 443
  Caption = #1057#1077#1084#1077#1089#1090#1088#1086#1074#1086#1077' '#8470'12, '#1055#1040#1057#1068#1050#1054
  Color = clMoneyGreen
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -19
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 24
  object PaintBox1: TPaintBox
    Left = 16
    Top = 8
    Width = 881
    Height = 297
    Color = clBtnFace
    ParentColor = False
  end
  object Label1: TLabel
    Left = 344
    Top = 320
    Width = 205
    Height = 24
    Caption = #1055#1088#1077#1086#1073#1088#1072#1079#1091#1077#1084#1099#1081' '#1090#1077#1082#1089#1090':'
  end
  object BitBtn1: TBitBtn
    Left = 200
    Top = 320
    Width = 129
    Height = 49
    Caption = #1055#1086#1082#1072#1079#1072#1090#1100
    TabOrder = 0
    OnClick = BitBtn1Click
    Kind = bkOK
  end
  object Edit1: TEdit
    Left = 560
    Top = 312
    Width = 289
    Height = 32
    TabOrder = 1
    Text = #1058#1077#1087#1077#1088#1100'_'#1090#1077#1082#1089#1090'-'#1074#1099#1075#1083#1103#1076#1080#1090'+'#1090#1072#1082
  end
  object Button1: TButton
    Left = 16
    Top = 320
    Width = 169
    Height = 49
    Caption = #1042#1099#1073#1088#1072#1090#1100' '#1096#1088#1080#1092#1090
    TabOrder = 2
    OnClick = Button1Click
  end
  object BitBtn2: TBitBtn
    Left = 408
    Top = 360
    Width = 89
    Height = 33
    TabOrder = 3
    OnClick = BitBtn2Click
    Kind = bkClose
  end
  object FontDialog1: TFontDialog
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -11
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    Left = 808
    Top = 128
  end
end
