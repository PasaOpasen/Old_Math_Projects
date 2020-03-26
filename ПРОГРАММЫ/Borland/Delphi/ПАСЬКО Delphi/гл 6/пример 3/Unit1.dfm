object Form1: TForm1
  Left = 197
  Top = 164
  Width = 277
  Height = 224
  Caption = #1055#1072#1089#1100#1082#1086' '#1044'. '#1040'.'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -24
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  Menu = MainMenu1
  OldCreateOrder = False
  PixelsPerInch = 96
  TextHeight = 29
  object BitBtn1: TBitBtn
    Left = 72
    Top = 96
    Width = 113
    Height = 49
    TabOrder = 0
    Kind = bkClose
  end
  object MainMenu1: TMainMenu
    Left = 16
    Top = 32
    object N1: TMenuItem
      Caption = #1054#1082#1085#1086
    end
    object OpenModalForm: TMenuItem
      Caption = #1084#1086#1076#1072#1083#1100#1085#1086#1077
      OnClick = OpenModalFormClick
    end
    object OpenModelessForm: TMenuItem
      Caption = #1085#1077#1084#1086#1076#1072#1083#1100#1085#1086#1077
      OnClick = OpenModelessFormClick
    end
  end
end
