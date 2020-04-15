object Form1: TForm1
  Left = 408
  Top = 53
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
    Left = 56
    Top = 0
    Width = 22
    Height = 21
    Caption = 'N='
  end
  object Label2: TLabel
    Left = 56
    Top = 32
    Width = 22
    Height = 21
    Caption = 'M='
  end
  object Label3: TLabel
    Left = 56
    Top = 64
    Width = 22
    Height = 21
    Caption = 'P='
  end
  object Label4: TLabel
    Left = 440
    Top = 256
    Width = 11
    Height = 24
    Caption = '='
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ParentFont = False
  end
  object Edit1: TEdit
    Left = 80
    Top = 0
    Width = 57
    Height = 29
    TabOrder = 0
    Text = 'Edit1'
  end
  object Edit2: TEdit
    Left = 80
    Top = 32
    Width = 57
    Height = 29
    TabOrder = 1
    Text = 'Edit2'
  end
  object Button1: TButton
    Left = 144
    Top = 0
    Width = 161
    Height = 89
    Caption = #1056#1072#1079#1084#1077#1088#1085#1086#1089#1090#1100
    TabOrder = 2
    OnClick = Button1Click
  end
  object Edit3: TEdit
    Left = 80
    Top = 64
    Width = 57
    Height = 29
    TabOrder = 3
    Text = 'Edit3'
  end
  object RadioButton1: TRadioButton
    Left = 312
    Top = 8
    Width = 113
    Height = 17
    Caption = 'NxN'
    TabOrder = 4
  end
  object RadioButton2: TRadioButton
    Left = 312
    Top = 32
    Width = 113
    Height = 17
    Caption = 'NxM MxP'
    TabOrder = 5
  end
  object Button2: TButton
    Left = 88
    Top = 96
    Width = 337
    Height = 25
    Caption = #1055#1086#1089#1095#1080#1090#1072#1090#1100
    TabOrder = 6
    OnClick = Button2Click
  end
  object StringGrid1: TStringGrid
    Left = 88
    Top = 128
    Width = 345
    Height = 145
    ColCount = 4
    RowCount = 4
    Options = [goFixedVertLine, goFixedHorzLine, goVertLine, goHorzLine, goRangeSelect, goEditing]
    TabOrder = 7
  end
  object StringGrid2: TStringGrid
    Left = 88
    Top = 280
    Width = 345
    Height = 161
    ColCount = 4
    RowCount = 4
    Options = [goFixedVertLine, goFixedHorzLine, goVertLine, goHorzLine, goRangeSelect, goEditing]
    TabOrder = 8
  end
  object StringGrid3: TStringGrid
    Left = 464
    Top = 200
    Width = 353
    Height = 145
    ColCount = 4
    RowCount = 4
    TabOrder = 9
  end
  object MainMenu1: TMainMenu
    Left = 680
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
    Left = 680
    Top = 8
  end
  object SaveDialog1: TSaveDialog
    Filter = 'save|*.txt'
    Left = 680
    Top = 48
  end
end
