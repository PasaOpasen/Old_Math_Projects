object Form1: TForm1
  Left = 307
  Top = 247
  Width = 928
  Height = 493
  Caption = 'Form1'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  PixelsPerInch = 96
  TextHeight = 13
  object Label1: TLabel
    Left = 24
    Top = 144
    Width = 3
    Height = 13
  end
  object ComboBox1: TComboBox
    Left = 24
    Top = 64
    Width = 321
    Height = 21
    ItemHeight = 13
    TabOrder = 0
    Items.Strings = (
      #1072' '#1088#1086#1079#1072' '#1091#1087#1072#1083#1072' '#1085#1072' '#1083#1072#1087#1091' '#1072#1079#1086#1088#1072)
  end
  object Edit1: TEdit
    Left = 24
    Top = 40
    Width = 321
    Height = 21
    TabOrder = 1
    Text = #1042#1074#1077#1076#1080#1090#1077' '#1087#1088#1077#1076#1083#1086#1078#1077#1085#1080#1077' '#1080' '#1085#1072#1078#1084#1080#1090#1077' Enter'
    OnKeyPress = ComboBox1KeyPress
  end
  object Button1: TButton
    Left = 24
    Top = 88
    Width = 321
    Height = 33
    Caption = #1042#1099#1087#1086#1083#1085#1080#1090#1100
    TabOrder = 2
    OnClick = Button1Click
  end
end
