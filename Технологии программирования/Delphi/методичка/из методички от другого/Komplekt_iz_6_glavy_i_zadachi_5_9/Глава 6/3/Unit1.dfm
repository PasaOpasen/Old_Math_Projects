object Form1: TForm1
  Left = 210
  Top = 136
  Width = 284
  Height = 272
  Caption = #1055#1088#1080#1084#1077#1088' 3'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  Menu = MainMenu1
  OldCreateOrder = False
  PixelsPerInch = 96
  TextHeight = 13
  object BitBtn1: TBitBtn
    Left = 104
    Top = 136
    Width = 89
    Height = 33
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -13
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ParentFont = False
    TabOrder = 0
    Kind = bkClose
  end
  object MainMenu1: TMainMenu
    Left = 8
    Top = 56
    object N1: TMenuItem
      Caption = #1054#1082#1085#1086
      object NOpenModalForm: TMenuItem
        Caption = #1052#1086#1076#1072#1083#1100#1085#1086#1077
        OnClick = NOpenModalFormClick
      end
      object NOpenModelessForm: TMenuItem
        Caption = #1053#1077#1084#1086#1076#1072#1083#1100#1085#1086#1077
        OnClick = NOpenModelessFormClick
      end
    end
  end
end
