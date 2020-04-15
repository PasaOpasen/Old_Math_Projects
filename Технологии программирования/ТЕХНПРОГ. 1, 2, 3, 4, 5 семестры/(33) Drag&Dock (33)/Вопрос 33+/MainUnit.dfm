object MainForm: TMainForm
  Left = 192
  Top = 107
  Width = 696
  Height = 480
  Caption = 'DemoDocking'
  Color = clBtnFace
  DockSite = True
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  PixelsPerInch = 96
  TextHeight = 13
  object CoolBar1: TCoolBar
    Left = 0
    Top = 0
    Width = 688
    Height = 97
    Bands = <
      item
        Control = ToolBar1
        ImageIndex = -1
        MinHeight = 29
        Width = 684
      end
      item
        Control = ColorsBar
        ImageIndex = -1
        Width = 684
      end
      item
        Control = SoundsBar
        ImageIndex = -1
        Width = 684
      end>
    object ToolBar1: TToolBar
      Left = 9
      Top = 0
      Width = 671
      Height = 29
      ButtonHeight = 21
      ButtonWidth = 88
      Caption = 'ToolBar1'
      DragKind = dkDock
      DragMode = dmAutomatic
      ShowCaptions = True
      TabOrder = 0
      object ToolButton1: TToolButton
        Left = 0
        Top = 2
        Caption = 'Close Application'
        ImageIndex = 0
        OnClick = ToolButton1Click
      end
    end
    object ColorsBar: TToolBar
      Left = 9
      Top = 31
      Width = 671
      Height = 25
      ButtonHeight = 21
      ButtonWidth = 60
      Caption = 'ColorsBar'
      DragKind = dkDock
      DragMode = dmAutomatic
      ShowCaptions = True
      TabOrder = 1
      object WhiteBtn: TToolButton
        Left = 0
        Top = 2
        Caption = 'White'
        ImageIndex = 0
        OnClick = WhiteBtnClick
      end
      object BlueBtn: TToolButton
        Left = 60
        Top = 2
        Caption = 'Blue'
        ImageIndex = 1
        OnClick = BlueBtnClick
      end
      object GreenBtn: TToolButton
        Left = 120
        Top = 2
        Caption = 'Green'
        ImageIndex = 2
        OnClick = GreenBtnClick
      end
      object LimeBtn: TToolButton
        Left = 180
        Top = 2
        Caption = 'Lime'
        ImageIndex = 3
        OnClick = LimeBtnClick
      end
      object PurpleBtn: TToolButton
        Left = 240
        Top = 2
        Caption = 'Purple'
        ImageIndex = 4
        OnClick = PurpleBtnClick
      end
      object RedBtn: TToolButton
        Left = 300
        Top = 2
        Caption = 'Red'
        ImageIndex = 5
        OnClick = RedBtnClick
      end
      object TealBtn: TToolButton
        Left = 360
        Top = 2
        Caption = 'Teal'
        ImageIndex = 6
        OnClick = TealBtnClick
      end
      object UndoBtn: TToolButton
        Left = 420
        Top = 2
        Caption = 'Undo Color'
        ImageIndex = 7
        OnClick = UndoBtnClick
      end
    end
    object SoundsBar: TToolBar
      Left = 9
      Top = 58
      Width = 671
      Height = 25
      ButtonHeight = 21
      ButtonWidth = 63
      Caption = 'SoundsBar'
      DragKind = dkDock
      DragMode = dmAutomatic
      ShowCaptions = True
      TabOrder = 2
      object SoundBtn1: TToolButton
        Left = 0
        Top = 2
        Caption = 'One Beep'
        ImageIndex = 0
        OnClick = SoundBtn1Click
      end
      object SoundBtn2: TToolButton
        Left = 63
        Top = 2
        Caption = 'Two Beep'
        ImageIndex = 1
        OnClick = SoundBtn2Click
      end
      object SoundBtn3: TToolButton
        Left = 126
        Top = 2
        Caption = 'Three Beep'
        ImageIndex = 2
        OnClick = SoundBtn3Click
      end
    end
  end
end
