object Form1: TForm1
  Left = 349
  Top = 117
  Width = 929
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
    Top = 16
    Width = 22
    Height = 21
    Caption = 'N='
  end
  object Label2: TLabel
    Left = 88
    Top = 16
    Width = 22
    Height = 21
    Caption = 'M='
  end
  object Edit1: TEdit
    Left = 40
    Top = 16
    Width = 41
    Height = 29
    TabOrder = 0
    Text = 'Edit1'
  end
  object Edit2: TEdit
    Left = 104
    Top = 16
    Width = 41
    Height = 29
    TabOrder = 1
    Text = 'Edit2'
  end
  object Button1: TButton
    Left = 152
    Top = 16
    Width = 209
    Height = 25
    Caption = #1047#1072#1076#1072#1090#1100' '#1088#1072#1079#1084#1077#1088#1085#1086#1089#1090#1100
    TabOrder = 2
    OnClick = Button1Click
  end
  object StringGrid1: TStringGrid
    Left = 24
    Top = 56
    Width = 281
    Height = 121
    ColCount = 4
    RowCount = 4
    Options = [goFixedVertLine, goFixedHorzLine, goVertLine, goHorzLine, goRangeSelect, goEditing]
    TabOrder = 3
  end
  object StringGrid2: TStringGrid
    Left = 312
    Top = 56
    Width = 289
    Height = 121
    ColCount = 4
    RowCount = 4
    Options = [goFixedVertLine, goFixedHorzLine, goVertLine, goHorzLine, goRangeSelect, goEditing]
    TabOrder = 4
    RowHeights = (
      24
      24
      24
      25)
  end
  object StringGrid3: TStringGrid
    Left = 24
    Top = 192
    Width = 577
    Height = 209
    TabOrder = 5
  end
  object Button2: TButton
    Left = 368
    Top = 16
    Width = 233
    Height = 25
    Caption = #1056#1077#1079#1091#1083#1100#1090#1072#1090
    TabOrder = 6
    OnClick = Button2Click
  end
  object RadioGroup1: TRadioGroup
    Left = 616
    Top = 16
    Width = 185
    Height = 153
    Caption = 'RadioGroup1'
    TabOrder = 7
  end
  object RadioButton1: TRadioButton
    Left = 648
    Top = 56
    Width = 113
    Height = 17
    Caption = '2n*m'
    TabOrder = 8
  end
  object RadioButton2: TRadioButton
    Left = 648
    Top = 80
    Width = 113
    Height = 17
    Caption = 'n*2m'
    TabOrder = 9
  end
  object RadioButton3: TRadioButton
    Left = 648
    Top = 104
    Width = 113
    Height = 17
    Caption = 'B'
    TabOrder = 10
  end
  object RadioButton4: TRadioButton
    Left = 648
    Top = 128
    Width = 113
    Height = 17
    Caption = #1043
    TabOrder = 11
  end
  object MainMenu1: TMainMenu
    Left = 704
    Top = 272
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
    Left = 704
    Top = 192
  end
  object SaveDialog1: TSaveDialog
    Filter = 'save|*.txt'
    Left = 704
    Top = 232
  end
end
