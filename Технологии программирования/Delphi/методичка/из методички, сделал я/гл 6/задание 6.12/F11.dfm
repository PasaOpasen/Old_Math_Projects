object Form1: TForm1
  Left = 428
  Top = 169
  Width = 823
  Height = 517
  Caption = #1057#1087#1080#1089#1086#1082' '#1088#1072#1089#1090#1077#1085#1080#1081' ('#1055#1040#1057#1068#1050#1054', '#1079#1072#1076#1072#1085#1080#1077' 6.12)'
  Color = clMoneyGreen
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -19
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  Menu = MainMenu1
  OldCreateOrder = False
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 24
  object Label1: TLabel
    Left = 128
    Top = 16
    Width = 261
    Height = 24
    Caption = #1057#1087#1080#1089#1086#1082' '#1088#1072#1089#1090#1077#1085#1080#1081' '#1074' '#1084#1072#1075#1072#1079#1080#1085#1077
  end
  object OpenModelessForm: TButton
    Left = 344
    Top = 416
    Width = 161
    Height = 33
    Caption = #1053#1072#1081#1090#1080' '#1090#1086#1074#1072#1088
    TabOrder = 0
    OnClick = OpenModelessFormClick
  end
  object BitBtn1: TBitBtn
    Left = 688
    Top = 408
    Width = 89
    Height = 33
    TabOrder = 1
    OnClick = BitBtn1Click
    Kind = bkClose
  end
  object Button1: TButton
    Left = 8
    Top = 416
    Width = 209
    Height = 33
    Caption = #1044#1086#1073#1072#1074#1080#1090#1100' '#1088#1072#1089#1090#1077#1085#1080#1077
    TabOrder = 2
    OnClick = Button1Click
  end
  object StringGrid1: TStringGrid
    Left = 24
    Top = 56
    Width = 761
    Height = 329
    DefaultColWidth = 150
    DefaultRowHeight = 40
    FixedCols = 0
    TabOrder = 3
  end
  object MainMenu1: TMainMenu
    Left = 8
    Top = 8
    object N1: TMenuItem
      Caption = #1048#1079' '#1092#1072#1081#1083#1072
      object N2: TMenuItem
        Caption = #1044#1054#1041#1040#1042#1048#1058#1068
        OnClick = N2Click
      end
      object N3: TMenuItem
        Caption = #1055#1056#1054#1063#1045#1057#1058#1068' '#1047#1040#1053#1054#1042#1054
        OnClick = N3Click
      end
    end
  end
  object OpenDialog1: TOpenDialog
    Left = 640
    Top = 16
  end
  object SaveDialog1: TSaveDialog
    Left = 688
    Top = 16
  end
end
