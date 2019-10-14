object Form1: TForm1
  Left = 995
  Top = 175
  Width = 522
  Height = 544
  Caption = #1055#1088#1080#1084#1077#1088' 1'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -24
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 29
  object Label1: TLabel
    Left = 128
    Top = 64
    Width = 217
    Height = 29
    Caption = #1042#1074#1077#1076#1080#1090#1077' '#1079#1085#1072#1095#1077#1085#1080#1103
  end
  object Label2: TLabel
    Left = 80
    Top = 128
    Width = 31
    Height = 29
    Caption = 'X='
  end
  object Label3: TLabel
    Left = 336
    Top = 128
    Width = 28
    Height = 29
    Caption = 'Z='
  end
  object Label4: TLabel
    Left = 208
    Top = 128
    Width = 30
    Height = 29
    Caption = 'Y='
  end
  object Edit1: TEdit
    Left = 112
    Top = 128
    Width = 81
    Height = 37
    TabOrder = 0
  end
  object Edit2: TEdit
    Left = 240
    Top = 128
    Width = 81
    Height = 37
    TabOrder = 1
  end
  object Edit3: TEdit
    Left = 368
    Top = 128
    Width = 81
    Height = 37
    TabOrder = 2
  end
  object Memo1: TMemo
    Left = 72
    Top = 240
    Width = 377
    Height = 177
    Lines.Strings = (
      'Memo1')
    ScrollBars = ssBoth
    TabOrder = 3
  end
  object Button1: TButton
    Left = 64
    Top = 192
    Width = 409
    Height = 41
    Caption = #1042#1099#1095#1080#1089#1083#1080#1090#1100' u = sec(x + y) + Exp(y - z)'
    TabOrder = 4
    OnClick = Button1Click
  end
end
