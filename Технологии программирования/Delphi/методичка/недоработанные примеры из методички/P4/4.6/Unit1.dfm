object Form1: TForm1
  Left = 192
  Top = 124
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
    Left = 24
    Top = 8
    Width = 22
    Height = 21
    Caption = 'N='
  end
  object Label2: TLabel
    Left = 24
    Top = 40
    Width = 22
    Height = 21
    Caption = 'M='
  end
  object Edit1: TEdit
    Left = 48
    Top = 8
    Width = 57
    Height = 29
    TabOrder = 0
    Text = 'Edit1'
  end
  object Edit2: TEdit
    Left = 48
    Top = 40
    Width = 57
    Height = 29
    TabOrder = 1
    Text = 'Edit2'
  end
  object Button1: TButton
    Left = 112
    Top = 8
    Width = 193
    Height = 49
    Caption = #1053#1086#1074#1072#1103' '#1088#1072#1079#1084#1077#1088#1085#1086#1089#1090#1100
    TabOrder = 2
    OnClick = Button1Click
  end
  object Button2: TButton
    Left = 312
    Top = 8
    Width = 217
    Height = 49
    Caption = #1056#1077#1079#1091#1083#1100#1090#1072#1090
    TabOrder = 3
    OnClick = Button2Click
  end
  object StringGrid1: TStringGrid
    Left = 24
    Top = 72
    Width = 329
    Height = 177
    Options = [goFixedVertLine, goFixedHorzLine, goVertLine, goHorzLine, goRangeSelect, goEditing]
    TabOrder = 4
  end
  object Memo1: TMemo
    Left = 360
    Top = 72
    Width = 169
    Height = 177
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Courier New'
    Font.Style = []
    Lines.Strings = (
      'Memo1')
    ParentFont = False
    ScrollBars = ssVertical
    TabOrder = 5
  end
  object MainMenu1: TMainMenu
    Left = 544
    Top = 88
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
    Top = 8
  end
  object SaveDialog1: TSaveDialog
    Filter = 'save|*.txt'
    Left = 544
    Top = 48
  end
end
