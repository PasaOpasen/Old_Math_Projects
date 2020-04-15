object Form1: TForm1
  Left = 218
  Top = 229
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
  PixelsPerInch = 96
  TextHeight = 21
  object Label1: TLabel
    Left = 8
    Top = 32
    Width = 154
    Height = 21
    Caption = #1042#1074#1077#1076#1080#1090#1077' '#1089#1090#1088#1086#1082#1091
  end
  object Label2: TLabel
    Left = 8
    Top = 128
    Width = 176
    Height = 21
    Caption = #1050#1086#1083#1080#1095#1077#1089#1090#1074#1086' '#1089#1083#1086#1074':'
  end
  object Label3: TLabel
    Left = 8
    Top = 152
    Width = 242
    Height = 21
    Caption = #1053#1072#1078#1084#1080#1090#1077' '#1095#1090#1086#1073' '#1087#1086#1089#1095#1080#1090#1072#1090#1100
    OnClick = ComboBox1Click
  end
  object Edit1: TEdit
    Left = 8
    Top = 64
    Width = 321
    Height = 29
    TabOrder = 0
    Text = #1042#1074#1077#1076#1080#1090#1077' '#1076#1077#1089#1103#1090#1080#1095#1085#1099#1077' '#1094#1080#1092#1088#1099
    OnKeyPress = ComboBox1KeyPress
  end
  object ComboBox1: TComboBox
    Left = 8
    Top = 96
    Width = 321
    Height = 29
    ItemHeight = 21
    TabOrder = 1
  end
  object BitBtn1: TBitBtn
    Left = 184
    Top = 192
    Width = 75
    Height = 25
    TabOrder = 2
    Kind = bkClose
  end
  object OpenDialog1: TOpenDialog
    Filter = 'in|*.txt'
    Left = 376
    Top = 8
  end
  object SaveDialog1: TSaveDialog
    Filter = 'save|*.txt'
    Left = 376
    Top = 40
  end
  object MainMenu1: TMainMenu
    Left = 376
    Top = 72
    object N1: TMenuItem
      Caption = #1054#1090#1082#1088#1099#1090#1100
      OnClick = N1Click
    end
    object N2: TMenuItem
      Caption = #1057#1086#1093#1088#1072#1085#1080#1090#1100
      OnClick = N2Click
    end
  end
end
