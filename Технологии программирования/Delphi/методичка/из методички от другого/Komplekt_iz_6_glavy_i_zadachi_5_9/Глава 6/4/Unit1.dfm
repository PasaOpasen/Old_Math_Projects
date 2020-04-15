object Form1: TForm1
  Left = 320
  Top = 302
  Width = 687
  Height = 353
  Caption = #1042#1077#1076#1086#1084#1086#1089#1090#1100' '#1072#1073#1080#1090#1091#1088#1080#1077#1085#1090#1086#1074
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  Menu = MainMenu1
  OldCreateOrder = False
  OnClose = FormClose
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 13
  object Label1: TLabel
    Left = 16
    Top = 16
    Width = 40
    Height = 16
    Caption = #1060'.'#1048'.'#1054'.'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -13
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ParentFont = False
  end
  object EditFIO: TEdit
    Left = 16
    Top = 40
    Width = 185
    Height = 24
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -13
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ParentFont = False
    TabOrder = 0
  end
  object GroupBox1: TGroupBox
    Left = 16
    Top = 80
    Width = 185
    Height = 137
    Caption = #1054#1094#1077#1085#1082#1080
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -13
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ParentFont = False
    TabOrder = 1
    object Label2: TLabel
      Left = 8
      Top = 28
      Width = 81
      Height = 16
      Caption = #1052#1072#1090#1077#1084#1072#1090#1080#1082#1072
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -13
      Font.Name = 'MS Sans Serif'
      Font.Style = []
      ParentFont = False
    end
    object Label3: TLabel
      Left = 8
      Top = 58
      Width = 50
      Height = 16
      Caption = #1060#1080#1079#1080#1082#1072
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -13
      Font.Name = 'MS Sans Serif'
      Font.Style = []
      ParentFont = False
    end
    object Label4: TLabel
      Left = 8
      Top = 90
      Width = 73
      Height = 16
      Caption = #1057#1086#1095#1080#1085#1077#1085#1080#1077
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -13
      Font.Name = 'MS Sans Serif'
      Font.Style = []
      ParentFont = False
    end
    object EditMath: TEdit
      Left = 104
      Top = 26
      Width = 41
      Height = 24
      TabOrder = 0
    end
    object EditPhys: TEdit
      Left = 104
      Top = 56
      Width = 41
      Height = 24
      TabOrder = 1
    end
    object EditOpus: TEdit
      Left = 104
      Top = 88
      Width = 41
      Height = 24
      TabOrder = 2
    end
  end
  object ButtonAddNewRec: TButton
    Left = 16
    Top = 240
    Width = 185
    Height = 33
    Caption = #1042#1074#1077#1089#1090#1080' '#1085#1086#1074#1091#1102' '#1079#1072#1087#1080#1089#1100
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -13
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ParentFont = False
    TabOrder = 2
    OnClick = ButtonAddNewRecClick
  end
  object StringGrid1: TStringGrid
    Left = 232
    Top = 40
    Width = 417
    Height = 177
    FixedCols = 0
    TabOrder = 3
  end
  object BitBtn1: TBitBtn
    Left = 324
    Top = 240
    Width = 89
    Height = 33
    TabOrder = 4
    OnClick = BitBtn1Click
    Kind = bkClose
  end
  object MainMenu1: TMainMenu
    Left = 512
    Top = 240
    object N1: TMenuItem
      Caption = #1060#1072#1081#1083
      object NNew: TMenuItem
        Caption = #1057#1086#1079#1076#1072#1090#1100
        OnClick = NNewClick
      end
      object NOpen: TMenuItem
        Caption = #1054#1090#1082#1088#1099#1090#1100
        OnClick = NOpenClick
      end
      object NSave: TMenuItem
        Caption = #1057#1086#1093#1088#1072#1085#1080#1090#1100
        OnClick = NSaveClick
      end
    end
    object NS: TMenuItem
      Caption = #1057#1086#1088#1090#1080#1088#1086#1074#1082#1072
      object NSort: TMenuItem
        Caption = #1057#1086#1088#1090#1080#1088#1086#1074#1072#1090#1100
        OnClick = NSortClick
      end
      object NSortSave: TMenuItem
        Caption = #1057#1086#1093#1088#1072#1085#1080#1090#1100
        OnClick = NSortSaveClick
      end
    end
  end
  object OpenDialog1: TOpenDialog
    DefaultExt = '.dat'
    Filter = #1060#1072#1081#1083' '#1076#1072#1085#1085#1099#1093'|*.dat|'#1042#1089#1077' '#1092#1072#1081#1083#1099'|*.*'
    Left = 560
    Top = 240
  end
  object SaveDialog1: TSaveDialog
    DefaultExt = '.txt'
    Filter = #1058#1077#1082#1089#1090#1086#1074#1099#1081' '#1092#1072#1081#1083'|*.txt|'#1042#1089#1077' '#1092#1072#1081#1083#1099'|*.*'
    Left = 608
    Top = 240
  end
end
