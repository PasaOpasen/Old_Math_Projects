object Form1: TForm1
  Left = 214
  Top = 120
  Width = 614
  Height = 616
  Caption = '3.1 '#1055#1040#1057#1068#1050#1054
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
    Left = 184
    Top = 40
    Width = 251
    Height = 29
    Caption = #1042#1042#1045#1044#1048#1058#1045' '#1047#1053#1040#1063#1045#1053#1048#1071
  end
  object Label2: TLabel
    Left = 144
    Top = 112
    Width = 31
    Height = 29
    Caption = 'X='
  end
  object Label3: TLabel
    Left = 376
    Top = 112
    Width = 30
    Height = 29
    Caption = 'Y='
  end
  object Edit1: TEdit
    Left = 176
    Top = 112
    Width = 121
    Height = 37
    TabOrder = 0
  end
  object Edit2: TEdit
    Left = 408
    Top = 112
    Width = 121
    Height = 37
    TabOrder = 1
  end
  object Button1: TButton
    Left = 120
    Top = 160
    Width = 353
    Height = 41
    Caption = 'u = sin(x^2+y^2)+exp(y-x)'
    TabOrder = 2
    OnClick = Button1Click
  end
  object Button2: TButton
    Left = 120
    Top = 208
    Width = 353
    Height = 41
    Caption = 'u = abs(cos(x))+arctg(1/y)'
    TabOrder = 3
    OnClick = Button2Click
  end
  object Button4: TButton
    Left = 120
    Top = 304
    Width = 353
    Height = 41
    Caption = 'u = cos(x)^2+abs(y)/arctg(x)'
    TabOrder = 4
    OnClick = Button4Click
  end
  object Memo1: TMemo
    Left = 88
    Top = 360
    Width = 433
    Height = 153
    Lines.Strings = (
      'Memo1')
    ScrollBars = ssBoth
    TabOrder = 5
  end
  object Button3: TButton
    Left = 120
    Top = 256
    Width = 353
    Height = 41
    Caption = 'u = tg(x+y^2)+y*ln(x)'
    TabOrder = 6
    OnClick = Button3Click
  end
end
