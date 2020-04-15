object Form1: TForm1
  Left = 288
  Top = 174
  Width = 655
  Height = 424
  Caption = 'Form1'
  Color = clBtnFace
  Font.Charset = RUSSIAN_CHARSET
  Font.Color = clWindowText
  Font.Height = -19
  Font.Name = 'Courier New'
  Font.Style = []
  Menu = MainMenu1
  OldCreateOrder = False
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 21
  object Label1: TLabel
    Left = 8
    Top = 8
    Width = 22
    Height = 21
    Caption = 'N='
  end
  object StringGrid1: TStringGrid
    Left = 8
    Top = 40
    Width = 68
    Height = 113
    ColCount = 1
    FixedCols = 0
    RowCount = 4
    Options = [goFixedVertLine, goFixedHorzLine, goVertLine, goHorzLine, goRangeSelect, goEditing]
    ScrollBars = ssVertical
    TabOrder = 0
  end
  object StringGrid2: TStringGrid
    Left = 88
    Top = 40
    Width = 68
    Height = 113
    ColCount = 1
    FixedCols = 0
    RowCount = 4
    Options = [goFixedVertLine, goFixedHorzLine, goVertLine, goHorzLine, goRangeSelect, goEditing]
    ScrollBars = ssVertical
    TabOrder = 1
  end
  object StringGrid3: TStringGrid
    Left = 168
    Top = 40
    Width = 68
    Height = 113
    ColCount = 1
    FixedCols = 0
    RowCount = 4
    ScrollBars = ssVertical
    TabOrder = 2
  end
  object Edit1: TEdit
    Left = 24
    Top = 8
    Width = 49
    Height = 29
    TabOrder = 3
    Text = 'Edit1'
  end
  object Button1: TButton
    Left = 80
    Top = 8
    Width = 209
    Height = 25
    Caption = #1047#1072#1076#1072#1090#1100' '#1056#1072#1079#1084#1077#1088#1085#1086#1089#1090#1100
    TabOrder = 4
    OnClick = Button1Click
  end
  object RadioGroup1: TRadioGroup
    Left = 240
    Top = 40
    Width = 281
    Height = 113
    Caption = #1042#1099#1073#1086#1088
    TabOrder = 5
  end
  object RadioButton1: TRadioButton
    Left = 248
    Top = 64
    Width = 265
    Height = 17
    Caption = #1057#1082#1072#1083#1103#1088#1085#1086#1077' '#1087#1088#1086#1080#1079#1074#1077#1076#1077#1085#1080#1077
    TabOrder = 6
  end
  object RadioButton4: TRadioButton
    Left = 248
    Top = 96
    Width = 241
    Height = 17
    Caption = #1057#1085#1072#1095#1072#1083#1072' 1 '#1087#1086#1090#1086#1084' 2'
    TabOrder = 7
  end
  object RadioButton5: TRadioButton
    Left = 248
    Top = 128
    Width = 265
    Height = 17
    Caption = #1063#1077#1090#1085#1099#1077' '#1085#1077#1095#1077#1090#1085#1099#1077
    TabOrder = 8
  end
  object Button2: TButton
    Left = 296
    Top = 8
    Width = 193
    Height = 25
    Caption = #1042#1099#1087#1086#1083#1085#1080#1090#1100
    TabOrder = 9
    OnClick = Button2Click
  end
  object MainMenu1: TMainMenu
    Left = 544
    Top = 128
    object N1: TMenuItem
      Caption = #1054#1090#1082#1088#1099#1090#1100
      OnClick = N1Click
    end
    object N2: TMenuItem
      Caption = #1057#1086#1093#1088#1072#1085#1080#1090#1100
      OnClick = N2Click
    end
  end
  object OpenDialog1: TOpenDialog
    Filter = 'in|*.txt'
    Left = 544
    Top = 48
  end
  object SaveDialog1: TSaveDialog
    Filter = 'save|*.txt'
    Left = 544
    Top = 88
  end
end
