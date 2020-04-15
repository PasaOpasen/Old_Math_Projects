object Form1: TForm1
  Left = 274
  Top = 252
  Width = 1045
  Height = 402
  Caption = #1057#1077#1084#1077#1089#1090#1088#1086#1074#1086#1077' '#8470'3, '#1055#1040#1057#1068#1050#1054
  Color = clMoneyGreen
  Font.Charset = RUSSIAN_CHARSET
  Font.Color = clWindowText
  Font.Height = -19
  Font.Name = 'Trebuchet MS'
  Font.Style = []
  Menu = MainMenu1
  OldCreateOrder = False
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 24
  object Label1: TLabel
    Left = 8
    Top = 8
    Width = 73
    Height = 24
    Caption = #1060'. '#1048'. '#1054'.'
  end
  object Edit1: TEdit
    Left = 8
    Top = 40
    Width = 193
    Height = 32
    TabOrder = 0
  end
  object GroupBox1: TGroupBox
    Left = 8
    Top = 80
    Width = 193
    Height = 193
    Caption = #1054#1094#1077#1085#1082#1080
    TabOrder = 1
    object Label2: TLabel
      Left = 8
      Top = 32
      Width = 106
      Height = 24
      Caption = #1052#1072#1090#1077#1084#1072#1090#1080#1082#1072
    end
    object Label3: TLabel
      Left = 8
      Top = 72
      Width = 65
      Height = 24
      Caption = #1060#1080#1079#1080#1082#1072
    end
    object Label4: TLabel
      Left = 8
      Top = 112
      Width = 95
      Height = 24
      Caption = #1057#1086#1095#1080#1085#1077#1085#1080#1077
    end
    object Edit2: TEdit
      Left = 120
      Top = 32
      Width = 65
      Height = 32
      TabOrder = 0
    end
    object Edit3: TEdit
      Left = 120
      Top = 72
      Width = 65
      Height = 32
      TabOrder = 1
    end
    object Edit4: TEdit
      Left = 120
      Top = 112
      Width = 65
      Height = 32
      TabOrder = 2
    end
  end
  object Button1: TButton
    Left = 8
    Top = 288
    Width = 217
    Height = 41
    Caption = #1044#1086#1073#1072#1074#1080#1090#1100' '#1085#1086#1074#1091#1102' '#1079#1072#1087#1080#1089#1100
    TabOrder = 2
    OnClick = Button1Click
  end
  object StringGrid1: TStringGrid
    Left = 216
    Top = 8
    Width = 681
    Height = 265
    DefaultColWidth = 80
    FixedCols = 0
    RowCount = 100
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Trebuchet MS'
    Font.Style = []
    Options = [goFixedVertLine, goFixedHorzLine, goVertLine, goHorzLine, goRangeSelect, goRowMoving]
    ParentFont = False
    ParentShowHint = False
    ShowHint = False
    TabOrder = 3
    ColWidths = (
      186
      122
      97
      107
      129)
  end
  object BitBtn1: TBitBtn
    Left = 752
    Top = 288
    Width = 97
    Height = 41
    TabOrder = 4
    OnClick = BitBtn1Click
    Kind = bkClose
  end
  object OpenDialog1: TOpenDialog
    DefaultExt = '.dat'
    Filter = #1060#1072#1080#1083' '#1076#1072#1085#1085#1099#1093'|*.dat|'#1042#1089#1077' '#1092#1072#1081#1083#1099'|*.*'
    Left = 680
    Top = 288
  end
  object SaveDialog1: TSaveDialog
    DefaultExt = '.txt'
    Filter = #1060#1072#1081#1083' '#1076#1072#1085#1085#1099#1093'|*.txt|'#1042#1089#1077' '#1092#1072#1081#1083#1099'|*.*'
    Left = 552
    Top = 288
  end
  object MainMenu1: TMainMenu
    Left = 512
    Top = 296
    object Fil: TMenuItem
      Caption = #1060#1072#1081#1083
      ShortCut = 16496
      object new: TMenuItem
        Action = Action1
        Caption = #1057#1086#1079#1076#1072#1090#1100
      end
      object opn: TMenuItem
        Action = Action2
        Caption = #1054#1090#1082#1088#1099#1090#1100
      end
      object sv: TMenuItem
        Action = Action3
        Caption = #1057#1086#1093#1088#1072#1085#1080#1090#1100
      end
    end
    object s: TMenuItem
      Caption = #1057#1086#1088#1090#1080#1088#1086#1074#1082#1072
      object srt: TMenuItem
        Action = Action4
        Caption = #1057#1086#1088#1090#1080#1088#1086#1074#1072#1090#1100
      end
      object svsrt: TMenuItem
        Action = Action5
        Caption = #1057#1086#1093#1088#1072#1085#1080#1090#1100
      end
    end
  end
  object ActionList1: TActionList
    Left = 608
    Top = 288
    object Action1: TAction
      Caption = 'Action1'
      ShortCut = 16496
      OnExecute = Action1Execute
    end
    object Action2: TAction
      Caption = 'Action2'
      ShortCut = 16497
      OnExecute = Action2Execute
    end
    object Action3: TAction
      Caption = 'Action3'
      ShortCut = 16498
      OnExecute = Action3Execute
    end
    object Action4: TAction
      Caption = 'Action4'
      ShortCut = 16499
      OnExecute = Action4Execute
    end
    object Action5: TAction
      Caption = 'Action5'
      ShortCut = 16500
      OnExecute = Action5Execute
    end
  end
end
