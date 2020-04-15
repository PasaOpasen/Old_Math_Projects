object Form1: TForm1
  Left = 328
  Top = 111
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
    Left = 64
    Top = 32
    Width = 22
    Height = 21
    Caption = 'N='
  end
  object Label2: TLabel
    Left = 120
    Top = 32
    Width = 22
    Height = 21
    Caption = 'M='
  end
  object Edit1: TEdit
    Left = 80
    Top = 32
    Width = 41
    Height = 29
    TabOrder = 0
    Text = 'Edit1'
  end
  object Edit2: TEdit
    Left = 136
    Top = 32
    Width = 41
    Height = 29
    TabOrder = 1
    Text = 'Edit2'
  end
  object Button1: TButton
    Left = 184
    Top = 32
    Width = 209
    Height = 25
    Caption = #1047#1072#1076#1072#1090#1100' '#1088#1072#1079#1084#1077#1088#1085#1086#1089#1090#1100
    TabOrder = 2
    OnClick = Button1Click
  end
  object Button2: TButton
    Left = 64
    Top = 64
    Width = 329
    Height = 33
    Caption = #1042#1099#1087#1086#1083#1085#1080#1090#1100
    TabOrder = 3
    OnClick = Button2Click
  end
  object StringGrid1: TStringGrid
    Left = 400
    Top = 32
    Width = 409
    Height = 233
    ColCount = 4
    RowCount = 4
    Options = [goFixedVertLine, goFixedHorzLine, goVertLine, goHorzLine, goRangeSelect, goEditing]
    TabOrder = 4
  end
  object Memo1: TMemo
    Left = 64
    Top = 176
    Width = 329
    Height = 89
    Lines.Strings = (
      'Memo1')
    ScrollBars = ssBoth
    TabOrder = 5
  end
  object RadioGroup1: TRadioGroup
    Left = 64
    Top = 96
    Width = 329
    Height = 73
    Caption = #1047#1072#1076#1072#1085#1080#1077
    TabOrder = 6
  end
  object RadioButton1: TRadioButton
    Left = 72
    Top = 120
    Width = 113
    Height = 17
    Caption = #1040
    TabOrder = 7
  end
  object RadioButton2: TRadioButton
    Left = 72
    Top = 144
    Width = 113
    Height = 17
    Caption = #1041
    TabOrder = 8
  end
  object MainMenu1: TMainMenu
    Left = 832
    Top = 112
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
    Left = 832
    Top = 32
  end
  object SaveDialog1: TSaveDialog
    Filter = 'save|*.txt'
    Left = 832
    Top = 72
  end
end
