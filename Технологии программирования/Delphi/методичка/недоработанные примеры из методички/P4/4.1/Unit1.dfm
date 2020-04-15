object Form1: TForm1
  Left = 209
  Top = 265
  Width = 925
  Height = 477
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
    Left = 16
    Top = 24
    Width = 22
    Height = 21
    Caption = 'N='
  end
  object Label2: TLabel
    Left = 96
    Top = 24
    Width = 22
    Height = 21
    Caption = 'M='
  end
  object Edit1: TEdit
    Left = 32
    Top = 24
    Width = 49
    Height = 29
    TabOrder = 0
    Text = 'Edit1'
  end
  object Edit2: TEdit
    Left = 112
    Top = 24
    Width = 49
    Height = 29
    TabOrder = 1
    Text = 'Edit2'
  end
  object Button1: TButton
    Left = 176
    Top = 24
    Width = 225
    Height = 25
    Caption = #1047#1072#1076#1072#1090#1100' '#1088#1072#1079#1084#1077#1088#1085#1086#1089#1090#1100
    TabOrder = 2
    OnClick = Button1Click
  end
  object StringGrid1: TStringGrid
    Left = 16
    Top = 120
    Width = 385
    Height = 201
    ColCount = 4
    RowCount = 4
    Options = [goFixedVertLine, goFixedHorzLine, goVertLine, goHorzLine, goRangeSelect, goEditing]
    TabOrder = 3
  end
  object StringGrid2: TStringGrid
    Left = 408
    Top = 120
    Width = 73
    Height = 201
    ColCount = 1
    FixedCols = 0
    RowCount = 4
    TabOrder = 4
  end
  object Button2: TButton
    Left = 16
    Top = 64
    Width = 385
    Height = 49
    Caption = #1055#1086#1083#1091#1095#1080#1090#1100' '#1074#1077#1082#1090#1086#1088
    TabOrder = 5
    OnClick = Button2Click
  end
  object RadioGroup1: TRadioGroup
    Left = 408
    Top = 16
    Width = 353
    Height = 97
    Caption = #1047#1072#1076#1072#1085#1080#1103
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Courier New'
    Font.Style = []
    ParentFont = False
    TabOrder = 6
  end
  object RadioButton1: TRadioButton
    Left = 416
    Top = 40
    Width = 313
    Height = 17
    Caption = #1072')0 k-'#1099#1081' '#1089#1090#1086#1083#1073#1077#1094' '#1085#1091#1083#1077#1074#1086#1081
    TabOrder = 7
  end
  object RadioButton2: TRadioButton
    Left = 416
    Top = 64
    Width = 329
    Height = 17
    Caption = #1073')1 k-'#1099#1081' '#1089#1090#1086#1083#1073#1077#1094' '#1091#1087#1086#1088#1103#1076#1086#1095#1077#1085
    TabOrder = 8
  end
  object RadioButton3: TRadioButton
    Left = 416
    Top = 88
    Width = 337
    Height = 17
    Caption = #1074')1 k-'#1072#1103' '#1089#1090#1088#1086#1082#1072' '#1089#1080#1084#1084#1077#1090#1088#1080#1095#1085#1072
    TabOrder = 9
  end
  object MainMenu1: TMainMenu
    Left = 800
    Top = 104
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
    Left = 800
    Top = 24
  end
  object SaveDialog1: TSaveDialog
    Filter = 'save|*.txt'
    Left = 800
    Top = 64
  end
end
