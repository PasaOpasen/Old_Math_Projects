object Form1: TForm1
  Left = 212
  Top = 247
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
    Top = 40
    Width = 154
    Height = 21
    Caption = #1042#1074#1077#1076#1080#1090#1077' '#1089#1090#1088#1086#1082#1091
  end
  object Label3: TLabel
    Left = 8
    Top = 128
    Width = 231
    Height = 21
    Caption = #1053#1072#1078#1084#1080#1090#1077' '#1095#1090#1086' '#1087#1086#1089#1095#1080#1090#1072#1090#1100
    OnClick = ComboBox1Click
  end
  object Label2: TLabel
    Left = 136
    Top = 152
    Width = 11
    Height = 22
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Courier New'
    Font.Style = [fsBold, fsItalic]
    ParentFont = False
  end
  object Label4: TLabel
    Left = 136
    Top = 176
    Width = 11
    Height = 22
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Courier New'
    Font.Style = [fsBold, fsItalic]
    ParentFont = False
  end
  object Label5: TLabel
    Left = 136
    Top = 200
    Width = 11
    Height = 22
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Courier New'
    Font.Style = [fsBold, fsItalic]
    ParentFont = False
  end
  object Label6: TLabel
    Left = 136
    Top = 224
    Width = 11
    Height = 22
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Courier New'
    Font.Style = [fsBold, fsItalic]
    ParentFont = False
  end
  object Label7: TLabel
    Left = 136
    Top = 248
    Width = 11
    Height = 22
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Courier New'
    Font.Style = [fsBold, fsItalic]
    ParentFont = False
  end
  object Edit1: TEdit
    Left = 8
    Top = 64
    Width = 481
    Height = 29
    TabOrder = 0
    Text = #1042#1074#1077#1076#1080#1090#1077' '#1089#1090#1088#1086#1082#1091' '#1080' '#1085#1072#1078#1084#1080#1090#1077' Enter'
    OnKeyPress = ComboBox1KeyPress
  end
  object ComboBox1: TComboBox
    Left = 8
    Top = 96
    Width = 481
    Height = 29
    ItemHeight = 21
    TabOrder = 1
  end
  object BitBtn1: TBitBtn
    Left = 128
    Top = 296
    Width = 97
    Height = 25
    TabOrder = 2
    Kind = bkClose
  end
  object OpenDialog1: TOpenDialog
    Filter = 'in|*.txt'
    Left = 712
    Top = 32
  end
  object SaveDialog1: TSaveDialog
    Filter = 'save|*.txt'
    Left = 712
    Top = 64
  end
  object MainMenu1: TMainMenu
    Left = 712
    Top = 96
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
