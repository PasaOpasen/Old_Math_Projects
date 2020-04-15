object Form1: TForm1
  Left = 659
  Top = 112
  Width = 553
  Height = 242
  Caption = #1057#1087#1088#1072#1074#1086#1095#1085#1072#1103' '#1072#1101#1088#1086#1087#1086#1088#1090#1072
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  OldCreateOrder = False
  Position = poScreenCenter
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 13
  object Label1: TLabel
    Left = 8
    Top = 8
    Width = 62
    Height = 13
    Caption = #1056#1072#1089#1087#1080#1089#1072#1085#1080#1077':'
  end
  object Label2: TLabel
    Left = 384
    Top = 8
    Width = 31
    Height = 13
    Caption = 'Label2'
    Visible = False
  end
  object StringGrid1: TStringGrid
    Left = 8
    Top = 32
    Width = 369
    Height = 137
    ColCount = 4
    FixedCols = 0
    Options = [goFixedVertLine, goFixedHorzLine, goVertLine, goHorzLine, goRangeSelect, goEditing]
    TabOrder = 0
    ColWidths = (
      86
      86
      105
      82)
  end
  object btnLoad: TButton
    Left = 8
    Top = 176
    Width = 177
    Height = 25
    Caption = #1055#1086#1083#1091#1095#1080#1090#1100' '#1076#1072#1085#1085#1099#1077' '#1080#1079' '#1092#1072#1081#1083#1072
    TabOrder = 1
    OnClick = btnLoadClick
  end
  object btnSave: TButton
    Left = 192
    Top = 176
    Width = 177
    Height = 25
    Caption = #1047#1072#1087#1080#1089#1072#1090#1100' '#1076#1072#1085#1085#1099#1077' '#1074' '#1092#1072#1081#1083
    TabOrder = 2
    OnClick = btnSaveClick
  end
  object Button3: TButton
    Left = 384
    Top = 32
    Width = 113
    Height = 49
    Caption = #1054#1090#1089#1086#1088#1090#1080#1088#1086#1074#1072#1090#1100' '#1087#1086' '#1091#1073#1099#1074#1072#1085#1080#1102
    TabOrder = 3
    WordWrap = True
    OnClick = Button3Click
  end
  object Button4: TButton
    Left = 384
    Top = 88
    Width = 113
    Height = 49
    Caption = #1054#1090#1089#1086#1088#1090#1080#1088#1086#1074#1072#1090#1100' '#1087#1086' '#1074#1086#1079#1088#1072#1089#1090#1072#1085#1080#1102
    TabOrder = 4
    WordWrap = True
    OnClick = Button4Click
  end
  object Button5: TButton
    Left = 392
    Top = 176
    Width = 105
    Height = 25
    Caption = #1042#1099#1093#1086#1076
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -11
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
    TabOrder = 5
    OnClick = Button5Click
  end
end
