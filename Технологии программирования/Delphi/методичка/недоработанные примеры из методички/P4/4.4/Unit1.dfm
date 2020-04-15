object Form1: TForm1
  Left = 192
  Top = 124
  Width = 819
  Height = 480
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
  object Label2: TLabel
    Left = 8
    Top = 40
    Width = 22
    Height = 21
    Caption = 'M='
  end
  object Edit1: TEdit
    Left = 32
    Top = 8
    Width = 57
    Height = 29
    TabOrder = 0
    Text = 'Edit1'
  end
  object Edit2: TEdit
    Left = 32
    Top = 40
    Width = 57
    Height = 29
    TabOrder = 1
    Text = 'Edit2'
  end
  object StringGrid1: TStringGrid
    Left = 8
    Top = 88
    Width = 361
    Height = 193
    ColCount = 4
    RowCount = 4
    Options = [goFixedVertLine, goFixedHorzLine, goVertLine, goHorzLine, goRangeSelect, goEditing]
    TabOrder = 2
  end
  object Button1: TButton
    Left = 96
    Top = 8
    Width = 217
    Height = 25
    Caption = #1053#1086#1074#1072#1103' '#1056#1072#1079#1084#1077#1088#1085#1086#1089#1090#1100
    TabOrder = 3
    OnClick = Button1Click
  end
  object Button2: TButton
    Left = 96
    Top = 40
    Width = 217
    Height = 25
    Caption = #1042#1099#1087#1086#1083#1085#1080#1090#1100
    TabOrder = 4
    OnClick = Button2Click
  end
  object StringGrid2: TStringGrid
    Left = 376
    Top = 88
    Width = 361
    Height = 193
    ColCount = 4
    RowCount = 4
    Options = [goFixedVertLine, goFixedHorzLine, goVertLine, goHorzLine, goRangeSelect, goEditing]
    TabOrder = 5
  end
  object RadioGroup1: TRadioGroup
    Left = 320
    Top = 0
    Width = 185
    Height = 81
    Caption = 'RadioGroup1'
    ItemIndex = 0
    Items.Strings = (
      #1040
      #1041
      #1042
      #1043)
    TabOrder = 6
  end
  object MainMenu1: TMainMenu
    Left = 592
    Top = 8
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
    Left = 512
    Top = 8
  end
  object SaveDialog1: TSaveDialog
    Filter = 'save|*.txt'
    Left = 552
    Top = 8
  end
end
