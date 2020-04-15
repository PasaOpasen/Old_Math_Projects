object Form1: TForm1
  Left = 566
  Top = 218
  Width = 578
  Height = 459
  Caption = #1055#1040#1057#1068#1050#1054', '#1074#1086#1087#1088#1086#1089' 12'
  Color = clMoneyGreen
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  OldCreateOrder = False
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 13
  object lbl1: TLabel
    Left = 88
    Top = 16
    Width = 392
    Height = 23
    Caption = #1047#1072#1076#1072#1081#1090#1077' '#1090#1088#1080' '#1074#1077#1097#1077#1089#1090#1074#1077#1085#1085#1099#1093' '#1095#1080#1089#1083#1072' a,b,c:'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = [fsBold]
    ParentFont = False
  end
  object lbl2: TLabel
    Left = 16
    Top = 48
    Width = 29
    Height = 23
    Caption = 'A='
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = [fsBold]
    ParentFont = False
  end
  object lbl3: TLabel
    Left = 200
    Top = 48
    Width = 29
    Height = 23
    Caption = 'B='
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = [fsBold]
    ParentFont = False
  end
  object lbl4: TLabel
    Left = 376
    Top = 48
    Width = 29
    Height = 23
    Caption = 'C='
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = [fsBold]
    ParentFont = False
  end
  object edt1: TEdit
    Left = 48
    Top = 48
    Width = 121
    Height = 23
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 0
  end
  object edt2: TEdit
    Left = 232
    Top = 48
    Width = 121
    Height = 23
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 1
  end
  object edt3: TEdit
    Left = 416
    Top = 48
    Width = 121
    Height = 23
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 2
  end
  object rg1: TRadioGroup
    Left = 232
    Top = 80
    Width = 313
    Height = 113
    Caption = #1042#1099#1073#1088#1072#1090#1100' '#1087#1072#1088#1091
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 3
  end
  object grp1: TGroupBox
    Left = 16
    Top = 80
    Width = 201
    Height = 113
    Caption = #1042#1099#1073#1088#1072#1090#1100' '#1082#1088#1080#1090#1077#1088#1080#1081
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 4
    object chk1: TCheckBox
      Left = 16
      Top = 32
      Width = 161
      Height = 17
      Caption = #1052#1080#1085#1080#1084#1072#1083#1100#1085#1086#1077' '#1080#1079' '#1076#1074#1091#1093
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -11
      Font.Name = 'Tahoma'
      Font.Style = [fsBold]
      ParentFont = False
      TabOrder = 0
    end
    object chk2: TCheckBox
      Left = 16
      Top = 64
      Width = 161
      Height = 17
      Caption = #1052#1072#1082#1089#1080#1084#1072#1083#1100#1085#1086#1077' '#1080#1079' '#1076#1074#1091#1093
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -11
      Font.Name = 'Tahoma'
      Font.Style = [fsBold]
      ParentFont = False
      TabOrder = 1
    end
  end
  object rb1: TRadioButton
    Left = 240
    Top = 112
    Width = 225
    Height = 17
    Caption = 'a+b+c;  abc'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 5
    OnClick = rb1Click
  end
  object rb2: TRadioButton
    Left = 240
    Top = 136
    Width = 201
    Height = 17
    Caption = 'ab+c/2;  ab+c'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 6
    OnClick = rb2Click
  end
  object rb3: TRadioButton
    Left = 240
    Top = 160
    Width = 113
    Height = 17
    Caption = 'a/2;  b^2+c'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 7
    OnClick = rb3Click
  end
  object mmo1: TMemo
    Left = 24
    Top = 200
    Width = 513
    Height = 201
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 8
  end
end
