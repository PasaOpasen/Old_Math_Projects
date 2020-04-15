object Form1: TForm1
  Left = 551
  Top = 151
  Width = 928
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
    Top = 48
    Width = 22
    Height = 21
    Caption = 'M='
  end
  object Edit1: TEdit
    Left = 32
    Top = 8
    Width = 49
    Height = 29
    TabOrder = 0
    Text = 'Edit1'
  end
  object Edit2: TEdit
    Left = 32
    Top = 48
    Width = 49
    Height = 29
    TabOrder = 1
    Text = 'Edit2'
  end
  object Button1: TButton
    Left = 88
    Top = 8
    Width = 273
    Height = 33
    Caption = #1053#1086#1074#1099#1081' '#1088#1072#1079#1084#1077#1088' '#1084#1072#1090#1088#1080#1094#1099
    TabOrder = 2
    OnClick = Button1Click
  end
  object StringGrid1: TStringGrid
    Left = 8
    Top = 88
    Width = 353
    Height = 161
    ColCount = 4
    RowCount = 4
    Options = [goFixedVertLine, goFixedHorzLine, goVertLine, goHorzLine, goRangeSelect, goEditing]
    TabOrder = 3
  end
  object StringGrid2: TStringGrid
    Left = 368
    Top = 88
    Width = 345
    Height = 161
    ColCount = 4
    RowCount = 4
    TabOrder = 4
  end
  object Button2: TButton
    Left = 88
    Top = 48
    Width = 273
    Height = 33
    Caption = #1042#1099#1095#1077#1089#1083#1080#1090#1100
    TabOrder = 5
    OnClick = Button2Click
  end
  object MainMenu1: TMainMenu
    Left = 464
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
    Left = 384
    Top = 8
  end
  object SaveDialog1: TSaveDialog
    Filter = 'save|*.txt'
    Left = 424
    Top = 8
  end
end
