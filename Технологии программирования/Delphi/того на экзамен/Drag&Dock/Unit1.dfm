object Form1: TForm1
  Left = 325
  Top = 192
  Width = 589
  Height = 347
  Caption = 'Drag&Dock'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  OldCreateOrder = False
  PixelsPerInch = 96
  TextHeight = 13
  object lbl1: TLabel
    Left = 168
    Top = 32
    Width = 233
    Height = 49
    Caption = #1055#1077#1088#1077#1090#1072#1097#1080#1090#1077' '#1082#1085#1086#1087#1082#1091' '#1089#1085#1072#1095#1072#1083#1072#13#10#1085#1072' '#1087#1072#1085#1077#1083#1100', '#1072' '#1079#1072#1090#1077#1084' '#1086#1073#1088#1072#1090#1085#1086
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object Panel1: TPanel
    Left = 0
    Top = 176
    Width = 573
    Height = 132
    Cursor = crArrow
    Align = alBottom
    Color = clSkyBlue
    DockSite = True
    TabOrder = 0
  end
  object bb1: TButton
    Left = 224
    Top = 112
    Width = 123
    Height = 41
    Caption = 'Drag&Dock'
    DragKind = dkDock
    DragMode = dmAutomatic
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
    TabOrder = 1
  end
  object bb2: TBitBtn
    Left = 496
    Top = 8
    Width = 75
    Height = 25
    TabOrder = 2
    Kind = bkClose
  end
end
