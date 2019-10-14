object Form1: TForm1
  Left = 265
  Top = 250
  Width = 504
  Height = 150
  Caption = 'Main Form'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -19
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  PixelsPerInch = 96
  TextHeight = 24
  object OpenModalForm: TButton
    Left = 16
    Top = 32
    Width = 281
    Height = 41
    Caption = #1042#1099#1079#1086#1074' '#1084#1086#1076#1072#1083#1100#1085#1086#1081' '#1092#1086#1088#1084#1099
    TabOrder = 0
    OnClick = OpenModalFormClick
  end
  object CloseBtn: TButton
    Left = 360
    Top = 32
    Width = 105
    Height = 41
    Caption = 'Close'
    TabOrder = 1
    OnClick = CloseBtnClick
  end
end
