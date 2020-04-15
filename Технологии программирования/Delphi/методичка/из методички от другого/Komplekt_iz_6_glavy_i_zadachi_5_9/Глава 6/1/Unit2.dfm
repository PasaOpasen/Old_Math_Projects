object Form2: TForm2
  Left = 747
  Top = 135
  Width = 249
  Height = 126
  Caption = 'Modal Dialog Form'
  Color = clYellow
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  PixelsPerInch = 96
  TextHeight = 13
  object Label1Bye: TLabel
    Left = 8
    Top = 32
    Width = 71
    Height = 20
    Caption = 'Good Bye'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ParentFont = False
  end
  object ButtonOK: TButton
    Left = 96
    Top = 32
    Width = 75
    Height = 25
    Caption = 'OK'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -11
    Font.Name = 'MS Sans Serif'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 0
    OnClick = ButtonOKClick
  end
end
