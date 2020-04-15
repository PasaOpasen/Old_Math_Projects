object Form1: TForm1
  Left = 336
  Top = 293
  Width = 826
  Height = 506
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
    Top = 72
    Width = 241
    Height = 185
    Caption = 'GroupBox1'
    TabOrder = 0
    object Label3: TLabel
      Left = 16
      Top = 80
      Width = 31
      Height = 13
      Caption = #1040#1076#1088#1077#1089
    end
    object Label4: TLabel
      Left = 16
      Top = 112
      Width = 24
      Height = 13
      Caption = #1056#1086#1089#1090
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
      Width = 27
      Height = 13
      Caption = #1060#1048#1054
    end
    object Label2: TLabel
      Left = 16
      Top = 48
      Width = 18
      Height = 13
      Caption = #1043#1086#1076
    end
    object Label6: TLabel
      Left = 16
      Top = 144
      Width = 19
      Height = 13
      Caption = #1042#1077#1089
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
  end
  object StringGrid1: TStringGrid
    Left = 288
    Top = 72
    Width = 369
    Height = 153
    FixedCols = 0
    TabOrder = 1
  end
  object Button1: TButton
    Left = 32
    Top = 296
    Width = 161
    Height = 25
    Caption = 'Button1'
    TabOrder = 2
    OnClick = Button1Click
  end
  object BitBtn1: TBitBtn
    Left = 200
    Top = 296
    Width = 75
    Height = 25
    TabOrder = 3
    OnClick = BitBtn1Click
    Kind = bkClose
  end
  object StringGrid2: TStringGrid
    Left = 288
    Top = 232
    Width = 369
    Height = 153
    FixedCols = 0
    TabOrder = 4
  end
  object OpenDialog1: TOpenDialog
    DefaultExt = '.dat'
    Filter = #1060#1072#1081#1083' '#1044#1072#1085#1085#1099#1093'|*.dat||*.*'
    Left = 72
    Top = 32
  end
  object SaveDialog1: TSaveDialog
    DefaultExt = '.dat'
    Filter = #1060#1072#1081#1083'|*.txt||*.*'
    Left = 112
    Top = 32
  end
  object MainMenu1: TMainMenu
    Left = 32
    Top = 32
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
