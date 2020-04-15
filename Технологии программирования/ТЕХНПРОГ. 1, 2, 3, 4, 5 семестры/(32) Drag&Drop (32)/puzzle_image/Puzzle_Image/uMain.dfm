object FrmMain: TFrmMain
  Left = 238
  Top = 125
  BorderIcons = [biSystemMenu, biMinimize]
  BorderStyle = bsSingle
  Caption = 'Puzzle Image'
  ClientHeight = 166
  ClientWidth = 201
  Color = clTeal
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -14
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  Menu = Menu
  OldCreateOrder = False
  Position = poDesktopCenter
  OnClose = FormClose
  OnCreate = FormCreate
  PixelsPerInch = 120
  TextHeight = 16
  object Button1: TButton
    Left = 8
    Top = 10
    Width = 185
    Height = 23
    Caption = 'Game'
    TabOrder = 0
    OnClick = Button1Click
  end
  object Panel1: TPanel
    Left = 8
    Top = 41
    Width = 185
    Height = 40
    TabOrder = 1
    object Label1: TLabel
      Left = 10
      Top = 13
      Width = 66
      Height = 16
      Caption = 'Movement:'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -13
      Font.Name = 'MS Sans Serif'
      Font.Style = []
      ParentFont = False
    end
    object Edit1: TEdit
      Left = 120
      Top = 8
      Width = 57
      Height = 21
      BiDiMode = bdRightToLeft
      ParentBiDiMode = False
      TabOrder = 0
      Text = '0'
    end
  end
  object Panel2: TPanel
    Left = 6
    Top = 88
    Width = 187
    Height = 73
    TabOrder = 2
    object Label2: TLabel
      Left = 10
      Top = 44
      Width = 29
      Height = 16
      Caption = 'Size:'
    end
    object Button2: TButton
      Left = 80
      Top = 8
      Width = 97
      Height = 25
      Caption = 'Change Image'
      TabOrder = 0
      OnClick = Button2Click
    end
    object chkSon: TCheckBox
      Left = 7
      Top = 10
      Width = 66
      Height = 21
      Caption = 'Sound'
      Checked = True
      State = cbChecked
      TabOrder = 1
    end
    object cbTaille: TComboBox
      Left = 80
      Top = 40
      Width = 97
      Height = 24
      ItemHeight = 16
      TabOrder = 2
      Text = '4 X 4'
      Items.Strings = (
        '4 X 4'
        '8 X 8'
        '12 X 12'
        '16 X 16'
        '32 X 32')
    end
  end
  object Menu: TMainMenu
    Left = 104
    object Fichier: TMenuItem
      Caption = '&File'
      object Ouvrir1: TMenuItem
        Caption = '&Open'
        OnClick = Ouvrir1Click
      end
      object convertir: TMenuItem
        Caption = '&Convert JPG->BMP'
        OnClick = convertirClick
      end
      object Quitter1: TMenuItem
        Caption = '&Exit'
        OnClick = Quitter1Click
      end
    end
  end
  object OpenDialog: TOpenDialog
    Filter = '*.bmp|bitmap|*.jpg|jpeg'
    Left = 16
  end
end
