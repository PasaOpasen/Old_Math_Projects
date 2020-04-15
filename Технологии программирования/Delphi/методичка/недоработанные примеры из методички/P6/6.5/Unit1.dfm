object Form1: TForm1
  Left = 178
  Top = 150
  Width = 1177
  Height = 480
  Caption = 'Form1'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  Menu = MainMenu1
  OldCreateOrder = False
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 13
  object GroupBox1: TGroupBox
    Left = 32
    Top = 64
    Width = 241
    Height = 217
    Caption = 'GroupBox1'
    TabOrder = 0
    object Label3: TLabel
      Left = 16
      Top = 80
      Width = 61
      Height = 13
      Caption = #1085#1072#1076#1077#1078#1085#1086#1089#1090#1100
    end
    object Label4: TLabel
      Left = 16
      Top = 112
      Width = 75
      Height = 13
      Caption = #1050#1086#1084#1092#1086#1088#1090#1085#1086#1089#1090#1100
    end
    object Label5: TLabel
      Left = 16
      Top = 24
      Width = 3
      Height = 13
    end
    object Label1: TLabel
      Left = 16
      Top = 24
      Width = 33
      Height = 13
      Caption = #1052#1072#1088#1082#1072
    end
    object Label2: TLabel
      Left = 16
      Top = 48
      Width = 36
      Height = 13
      Caption = #1056#1072#1089#1093#1086#1076
    end
    object Edit2: TEdit
      Left = 104
      Top = 48
      Width = 65
      Height = 21
      TabOrder = 0
    end
    object Edit3: TEdit
      Left = 104
      Top = 80
      Width = 65
      Height = 21
      TabOrder = 1
    end
    object Edit4: TEdit
      Left = 104
      Top = 112
      Width = 65
      Height = 21
      TabOrder = 2
    end
    object Edit1: TEdit
      Left = 104
      Top = 16
      Width = 65
      Height = 21
      TabOrder = 3
    end
    object Edit5: TEdit
      Left = 104
      Top = 144
      Width = 65
      Height = 21
      TabOrder = 4
    end
    object Edit6: TEdit
      Left = 104
      Top = 176
      Width = 65
      Height = 21
      TabOrder = 5
    end
  end
  object StringGrid1: TStringGrid
    Left = 288
    Top = 72
    Width = 401
    Height = 153
    ColCount = 6
    FixedCols = 0
    TabOrder = 1
  end
  object Button1: TButton
    Left = 24
    Top = 288
    Width = 161
    Height = 25
    Caption = 'Button1'
    TabOrder = 2
    OnClick = Button1Click
  end
  object BitBtn1: TBitBtn
    Left = 312
    Top = 288
    Width = 75
    Height = 25
    TabOrder = 3
    OnClick = BitBtn1Click
    Kind = bkClose
  end
  object Edit7: TEdit
    Left = 808
    Top = 40
    Width = 73
    Height = 25
    TabOrder = 4
    Text = #1043#1086#1076
  end
  object Button2: TButton
    Left = 696
    Top = 40
    Width = 113
    Height = 25
    Caption = #1042#1074#1099#1074#1077#1089#1090#1080' '#1087#1086' '#1075#1086#1076#1091
    TabOrder = 5
    OnClick = Button2Click
  end
  object StringGrid2: TStringGrid
    Left = 696
    Top = 72
    Width = 465
    Height = 153
    ColCount = 6
    FixedCols = 0
    TabOrder = 6
    RowHeights = (
      24
      24
      24
      22
      24)
  end
  object OpenDialog1: TOpenDialog
    DefaultExt = '.dat'
    Filter = #1060#1072#1081#1083' '#1044#1072#1085#1085#1099#1093'|*.dat||*.*'
    Left = 448
  end
  object SaveDialog1: TSaveDialog
    DefaultExt = '.dat'
    Filter = #1060#1072#1081#1083'|*.txt||*.*'
    Left = 488
  end
  object MainMenu1: TMainMenu
    Left = 408
    object Fil: TMenuItem
      Caption = ' '#1060#1072#1080#1083
      object new: TMenuItem
        Caption = #1057#1086#1079#1076#1072#1090#1100
        OnClick = newClick
      end
      object opn: TMenuItem
        Caption = #1054#1090#1082#1088#1099#1090#1100
        OnClick = opnClick
      end
      object sv: TMenuItem
        Caption = #1057#1086#1093#1088#1072#1085#1080#1090#1100
        OnClick = svClick
      end
    end
    object s: TMenuItem
      Caption = #1057#1086#1088#1090#1080#1088#1086#1074#1082#1072
      object srt: TMenuItem
        Caption = #1057#1086#1088#1090#1080#1088#1086#1074#1072#1090#1100
        OnClick = srtClick
      end
      object svsrt: TMenuItem
        Caption = #1057#1086#1093#1088#1072#1085#1080#1090#1100
        OnClick = svsrtClick
      end
    end
  end
end
