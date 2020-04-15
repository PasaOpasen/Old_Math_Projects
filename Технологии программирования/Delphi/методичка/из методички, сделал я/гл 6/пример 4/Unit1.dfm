object Form1: TForm1
  Left = 124
  Top = 172
  Width = 692
  Height = 396
  Caption = #1042#1077#1076#1086#1084#1086#1089#1090#1100' '#1072#1073#1080#1090#1091#1088#1080#1077#1085#1090#1086#1074' '#1055#1040#1057#1068#1050#1054
  Color = clYellow
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
    Left = 16
    Top = 48
    Width = 64
    Height = 24
    Caption = #1060'. '#1048'. '#1054
  end
  object Edit1: TEdit
    Left = 16
    Top = 72
    Width = 233
    Height = 33
    TabOrder = 0
  end
  object GroupBox1: TGroupBox
    Left = 16
    Top = 112
    Width = 233
    Height = 161
    Caption = #1054#1094#1077#1085#1082#1080
    TabOrder = 1
    object Label2: TLabel
      Left = 8
      Top = 32
      Width = 110
      Height = 24
      Caption = #1052#1072#1090#1077#1084#1072#1090#1080#1082#1072
    end
    object Label3: TLabel
      Left = 8
      Top = 72
      Width = 67
      Height = 24
      Caption = #1060#1080#1079#1080#1082#1072
    end
    object Label4: TLabel
      Left = 8
      Top = 112
      Width = 100
      Height = 24
      Caption = #1057#1086#1095#1080#1085#1077#1085#1080#1077
    end
    object Edit2: TEdit
      Left = 128
      Top = 32
      Width = 97
      Height = 25
      TabOrder = 0
    end
    object Edit3: TEdit
      Left = 128
      Top = 72
      Width = 97
      Height = 25
      TabOrder = 1
    end
    object Edit4: TEdit
      Left = 128
      Top = 112
      Width = 97
      Height = 25
      TabOrder = 2
    end
  end
  object Button1: TButton
    Left = 16
    Top = 296
    Width = 233
    Height = 33
    Caption = #1042#1074#1077#1089#1090#1080' '#1085#1086#1074#1091#1102' '#1079#1072#1087#1080#1089#1100
    TabOrder = 2
    OnClick = Button1Click
  end
  object StringGrid1: TStringGrid
    Left = 264
    Top = 72
    Width = 409
    Height = 201
    DefaultColWidth = 80
    FixedCols = 0
    RowCount = 7
    TabOrder = 3
  end
  object BitBtn1: TBitBtn
    Left = 424
    Top = 296
    Width = 145
    Height = 33
    TabOrder = 4
    OnClick = BitBtn1Click
    Kind = bkClose
  end
  object OpenDialog1: TOpenDialog
    DefaultExt = '.dat'
    Filter = #1060#1072#1081#1083' '#1076#1072#1085#1085#1099#1093'|*.dat|'#1042#1089#1077' '#1092#1072#1081#1083#1099'|*.*'
    Left = 592
    Top = 296
  end
  object SaveDialog1: TSaveDialog
    DefaultExt = '.txt'
    Filter = #1060#1072#1081#1083' '#1076#1072#1085#1085#1099#1093'|*.txt|'#1042#1089#1077' '#1092#1072#1081#1083#1099'|*.*'
    Left = 640
    Top = 296
  end
  object MainMenu1: TMainMenu
    Left = 496
    Top = 16
    object Fil: TMenuItem
      Caption = #1060#1072#1081#1083
      object new: TMenuItem
        Caption = #1089#1086#1079#1076#1072#1090#1100
        OnClick = newClick
      end
      object opn: TMenuItem
        Caption = #1086#1090#1082#1088#1099#1090#1100
        OnClick = opnClick
      end
      object sv: TMenuItem
        Caption = #1089#1086#1093#1088#1072#1085#1080#1090#1100
        OnClick = svClick
      end
    end
    object s: TMenuItem
      Caption = #1057#1086#1088#1090#1080#1088#1086#1074#1082#1072
      object srt: TMenuItem
        Caption = #1089#1086#1088#1090#1080#1088#1086#1074#1072#1090#1100
        OnClick = srtClick
      end
      object svsrt: TMenuItem
        Caption = #1089#1086#1093#1088#1072#1085#1080#1090#1100
        OnClick = svsrtClick
      end
    end
  end
end
