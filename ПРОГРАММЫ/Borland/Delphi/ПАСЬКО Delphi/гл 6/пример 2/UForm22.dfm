object Form3: TForm3
  Left = 295
  Top = 432
  Width = 398
  Height = 158
  Caption = 'Modeless Dialog Form'
  Color = clYellow
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -32
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  PixelsPerInch = 96
  TextHeight = 37
  object ByeLbl: TLabel
    Left = 32
    Top = 40
    Width = 150
    Height = 37
    Caption = 'Good Bye!'
  end
  object OKBtn: TButton
    Left = 232
    Top = 32
    Width = 113
    Height = 49
    Caption = 'OK'
    TabOrder = 0
    OnClick = OKBtnClick
  end
end
