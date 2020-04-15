object Form2: TForm2
  Left = 192
  Top = 136
  Width = 928
  Height = 480
  Caption = 'ModelessDialogForm'
  Color = clBlue
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  PixelsPerInch = 96
  TextHeight = 16
  object BlyeLbl: TLabel
    Left = 200
    Top = 88
    Width = 209
    Height = 41
    Caption = 'Good Bye!!'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -31
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ParentFont = False
  end
  object OkBtn: TBitBtn
    Left = 200
    Top = 168
    Width = 153
    Height = 65
    Caption = 'Ok'
    TabOrder = 0
    OnClick = OkBtnClick
  end
end
