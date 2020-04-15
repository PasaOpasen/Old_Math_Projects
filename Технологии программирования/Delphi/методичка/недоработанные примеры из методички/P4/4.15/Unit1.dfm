object Form1: TForm1
  Left = 237
  Top = 152
  Width = 819
  Height = 506
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
  object StringGrid1: TStringGrid
    Left = 8
    Top = 0
    Width = 393
    Height = 273
    Options = [goFixedVertLine, goFixedHorzLine, goVertLine, goHorzLine, goRangeSelect, goEditing]
    TabOrder = 0
  end
  object Button1: TButton
    Left = 288
    Top = 280
    Width = 193
    Height = 25
    Caption = #1042#1099#1087#1086#1083#1085#1080#1090#1100
    TabOrder = 1
    OnClick = Button1Click
  end
  object StringGrid2: TStringGrid
    Left = 408
    Top = 0
    Width = 385
    Height = 273
    TabOrder = 2
  end
  object Edit1: TEdit
    Left = 8
    Top = 280
    Width = 73
    Height = 29
    TabOrder = 3
    Text = 'Edit1'
  end
  object Button2: TButton
    Left = 88
    Top = 280
    Width = 193
    Height = 25
    Caption = #1053#1086#1074#1072#1103' '#1088#1072#1079#1084#1077#1088#1085#1086#1089#1090#1100
    TabOrder = 4
    OnClick = Button2Click
  end
  object RadioGroup1: TRadioGroup
    Left = 8
    Top = 312
    Width = 273
    Height = 105
    Caption = 'RadioGroup1'
    Items.Strings = (
      #1040
      #1041
      #1042
      #1043)
    TabOrder = 5
  end
  object MainMenu1: TMainMenu
    Left = 536
    Top = 376
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
    Left = 536
    Top = 312
  end
  object SaveDialog1: TSaveDialog
    Filter = 'save|*.txt'
    Left = 536
    Top = 344
  end
end
