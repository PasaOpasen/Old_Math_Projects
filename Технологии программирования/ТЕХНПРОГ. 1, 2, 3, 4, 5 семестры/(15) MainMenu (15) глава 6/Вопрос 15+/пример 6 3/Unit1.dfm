object Form1: TForm1
  Left = 212
  Top = 257
  Width = 323
  Height = 202
  Caption = #1055#1040#1057#1068#1050#1054', '#1074#1086#1087#1088#1086#1089' 15'
  Color = clMoneyGreen
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  Menu = MainMenu1
  OldCreateOrder = False
  Visible = True
  PixelsPerInch = 96
  TextHeight = 13
  object BitBtn1: TBitBtn
    Left = 169
    Top = 59
    Width = 104
    Height = 38
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ParentFont = False
    TabOrder = 0
    Kind = bkClose
  end
  object MainMenu1: TMainMenu
    Left = 72
    Top = 64
    object OpenModalessForm: TMenuItem
      Caption = #1054#1082#1085#1086
      object OpenModelForm: TMenuItem
        Caption = #1084#1086#1076#1072#1083#1100#1085#1086#1077
        OnClick = OpenModelFormClick
      end
      object OpenModelessForm: TMenuItem
        Caption = #1085#1077#1084#1086#1076#1072#1083#1100#1085#1086#1077
        OnClick = OpenModelessFormClick
      end
    end
  end
end
