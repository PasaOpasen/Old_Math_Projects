object Form1: TForm1
  Left = 210
  Top = 141
  Width = 928
  Height = 480
  Caption = 'Form1'
  Color = clBtnFace
  DragMode = dmAutomatic
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  PixelsPerInch = 96
  TextHeight = 13
  object Label1: TLabel
    Left = 258
    Top = 24
    Width = 213
    Height = 13
    Caption = #1055#1077#1088#1077#1090#1072#1097#1080#1090#1077' '#1089#1102#1076#1072' '#1083#1102#1073#1086#1081' '#1086#1073#1100#1077#1082#1090' '#1089' '#1092#1086#1088#1084#1099
    OnDragOver = Label1DragOver
  end
  object ShapeRectangle: TShape
    Left = 32
    Top = 72
    Width = 73
    Height = 73
    DragMode = dmAutomatic
  end
  object ShapeCircle: TShape
    Left = 120
    Top = 72
    Width = 73
    Height = 73
    DragMode = dmAutomatic
    Shape = stCircle
  end
  object BitBtn1: TBitBtn
    Left = 25
    Top = 21
    Width = 169
    Height = 33
    DragMode = dmAutomatic
    TabOrder = 0
    Kind = bkOK
  end
  object RadioGroup1: TRadioGroup
    Left = 40
    Top = 160
    Width = 185
    Height = 105
    Caption = 'RadioGroup1'
    DragMode = dmAutomatic
    Items.Strings = (
      '1'
      '2')
    TabOrder = 1
  end
end
