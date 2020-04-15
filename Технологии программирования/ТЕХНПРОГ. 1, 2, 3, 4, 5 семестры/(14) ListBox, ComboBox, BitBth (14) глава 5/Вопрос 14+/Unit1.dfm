object Form1: TForm1
  Left = 426
  Top = 320
  Width = 901
  Height = 262
  Caption = #1055#1040#1057#1068#1050#1054', '#1074#1086#1087#1088#1086#1089' 14'
  Color = clMoneyGreen
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -19
  Font.Name = 'MS Sans Serif'
  Font.Style = [fsBold]
  OldCreateOrder = False
  PixelsPerInch = 96
  TextHeight = 24
  object Label1: TLabel
    Left = 16
    Top = 120
    Width = 137
    Height = 23
    Caption = #1042#1074#1077#1076#1080#1090#1077' '#1089#1090#1088#1086#1082#1080
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
  end
  object Label2: TLabel
    Left = 594
    Top = 184
    Width = 173
    Height = 23
    Caption = #1050#1086#1083#1080#1095#1077#1089#1090#1074#1086' '#1089#1083#1086#1074' = '
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
  end
  object Label3: TLabel
    Left = 780
    Top = 184
    Width = 89
    Height = 23
    Caption = #1087#1086#1089#1095#1080#1090#1072#1090#1100
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
    OnClick = Label3Click
  end
  object Edit1: TEdit
    Left = 168
    Top = 120
    Width = 641
    Height = 31
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
    TabOrder = 0
    Text = #1042#1074#1077#1076#1080#1090#1077' '#1089#1090#1088#1086#1082#1091' '#1080' '#1085#1072#1078#1084#1080#1090#1077' Enter'
    OnKeyPress = Edit1KeyPress
  end
  object ComboBox1: TComboBox
    Left = 16
    Top = 176
    Width = 545
    Height = 31
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ItemHeight = 23
    ParentFont = False
    TabOrder = 1
    Text = #1042#1099#1073#1088#1072#1090#1100' '#1089#1090#1088#1086#1082#1091' '#1080#1079' '#1090#1077#1082#1089#1090#1072' '#1080' '#1085#1072#1078#1072#1090#1100' '#1085#1072' '#1089#1083#1086#1074#1086' '#1087#1086#1089#1095#1080#1090#1072#1090#1100
  end
  object BitBtn1: TBitBtn
    Left = 752
    Top = 40
    Width = 89
    Height = 41
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
    TabOrder = 2
    Kind = bkClose
  end
  object GroupBox1: TGroupBox
    Left = 16
    Top = 16
    Width = 441
    Height = 73
    Caption = #1042#1099#1073#1080#1088#1080#1090#1077' '#1082#1086#1083'-'#1074#1086' '#1089#1083#1086#1074' '#1074' '#1090#1077#1089#1090#1086#1074#1086#1084' '#1087#1088#1077#1076#1083#1086#1078#1077#1085#1080#1080
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
    TabOrder = 3
    object RadioButton1: TRadioButton
      Left = 8
      Top = 40
      Width = 113
      Height = 17
      Caption = '6 '#1089#1083#1086#1074
      Checked = True
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clBlack
      Font.Height = -19
      Font.Name = 'MS Sans Serif'
      Font.Style = [fsBold]
      ParentFont = False
      TabOrder = 0
      TabStop = True
    end
    object RadioButton2: TRadioButton
      Left = 160
      Top = 40
      Width = 113
      Height = 17
      Caption = '1 '#1089#1083#1086#1074#1086
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clBlack
      Font.Height = -19
      Font.Name = 'MS Sans Serif'
      Font.Style = [fsBold]
      ParentFont = False
      TabOrder = 1
    end
    object RadioButton3: TRadioButton
      Left = 312
      Top = 40
      Width = 113
      Height = 17
      Caption = '5 '#1089#1083#1086#1074
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clBlack
      Font.Height = -19
      Font.Name = 'MS Sans Serif'
      Font.Style = [fsBold, fsUnderline]
      ParentFont = False
      TabOrder = 2
    end
  end
  object Button1: TButton
    Left = 488
    Top = 32
    Width = 241
    Height = 57
    Caption = #1057#1095#1080#1090#1072#1090#1100' '#1089#1090#1088#1086#1082#1091' '#1080#1079' '#1092#1072#1081#1083#1072
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clBlack
    Font.Height = -19
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
    TabOrder = 4
    OnClick = Button1Click
  end
end
