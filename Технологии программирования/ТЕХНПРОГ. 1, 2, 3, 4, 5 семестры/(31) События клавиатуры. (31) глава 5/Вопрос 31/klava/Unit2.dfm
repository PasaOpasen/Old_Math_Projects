object Form2: TForm2
  Left = 0
  Top = 0
  Caption = #1056#1072#1073#1086#1090#1072' '#1089' '#1082#1083#1072#1074#1080#1072#1090#1091#1088#1086#1081
  ClientHeight = 387
  ClientWidth = 696
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  KeyPreview = True
  OldCreateOrder = False
  OnKeyPress = FormKeyPress
  OnKeyUp = FormKeyUp
  PixelsPerInch = 96
  TextHeight = 13
  object TouchKeyboard1: TTouchKeyboard
    Left = 0
    Top = 201
    Width = 696
    Height = 186
    Align = alClient
    GradientEnd = clSilver
    GradientStart = clGray
    Layout = 'Standard'
  end
  object GroupBox1: TGroupBox
    Left = 0
    Top = 0
    Width = 696
    Height = 201
    Align = alTop
    Caption = #1059#1087#1088#1072#1074#1083#1077#1085#1080#1077' '#1080' '#1080#1085#1092#1086#1088#1084#1072#1094#1080#1103
    TabOrder = 1
    object GroupBox2: TGroupBox
      Left = 2
      Top = 15
      Width = 247
      Height = 184
      Align = alLeft
      TabOrder = 0
      object Label1: TLabel
        Left = 8
        Top = 10
        Width = 80
        Height = 13
        Caption = #1056#1077#1078#1080#1084' '#1088#1072#1073#1086#1090#1099': '
      end
      object Label2: TLabel
        Left = 94
        Top = 10
        Width = 35
        Height = 13
        Caption = '-------'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clWindowText
        Font.Height = -11
        Font.Name = 'Tahoma'
        Font.Style = [fsBold]
        ParentFont = False
      end
      object Label3: TLabel
        Left = 8
        Top = 60
        Width = 147
        Height = 13
        Caption = #1055#1086#1089#1083#1077#1076#1085#1103#1103' '#1085#1072#1078#1072#1072#1103' '#1082#1083#1072#1074#1080#1096#1072':'
      end
      object Label4: TLabel
        Left = 18
        Top = 79
        Width = 22
        Height = 13
        Caption = 'Key:'
      end
      object Label5: TLabel
        Left = 94
        Top = 79
        Width = 35
        Height = 13
        Caption = '-------'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clWindowText
        Font.Height = -11
        Font.Name = 'Tahoma'
        Font.Style = [fsBold]
        ParentFont = False
      end
      object Label6: TLabel
        Left = 18
        Top = 98
        Width = 41
        Height = 13
        Caption = #1057#1080#1084#1074#1086#1083':'
      end
      object Label7: TLabel
        Left = 94
        Top = 100
        Width = 35
        Height = 13
        Caption = '-------'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clWindowText
        Font.Height = -11
        Font.Name = 'Tahoma'
        Font.Style = [fsBold]
        ParentFont = False
      end
      object Label8: TLabel
        Left = 12
        Top = 143
        Width = 154
        Height = 13
        Caption = #1048#1076#1077#1085#1090#1080#1092#1080#1082#1072#1090#1086#1088' '#1103#1079#1099#1082#1072' '#1074#1074#1086#1076#1072':'
      end
      object Label9: TLabel
        Left = 172
        Top = 143
        Width = 28
        Height = 13
        Caption = '-------'
      end
      object Label10: TLabel
        Left = 12
        Top = 162
        Width = 58
        Height = 13
        Caption = 'HEX '#1092#1086#1088#1084#1072':'
      end
      object Label11: TLabel
        Left = 172
        Top = 162
        Width = 28
        Height = 13
        Caption = '-------'
      end
      object Label12: TLabel
        Left = 12
        Top = 120
        Width = 105
        Height = 13
        Caption = #1057#1084#1077#1085#1080#1090#1100' '#1103#1079#1099#1082' '#1074#1074#1086#1076#1072
      end
      object Button1: TButton
        Left = 8
        Top = 29
        Width = 231
        Height = 25
        Caption = #1054#1089#1090#1072#1085#1086#1074#1080#1090#1100' '#1095#1090#1077#1085#1080#1077' '#1089' '#1082#1083#1072#1074#1080#1072#1090#1091#1088#1099
        TabOrder = 0
        OnClick = Button1Click
      end
      object Button2: TButton
        Left = 168
        Top = 116
        Width = 55
        Height = 21
        TabOrder = 1
        OnClick = Button2Click
      end
    end
    object GroupBox3: TGroupBox
      Left = 249
      Top = 15
      Width = 445
      Height = 184
      Align = alClient
      TabOrder = 1
      object Memo1: TMemo
        Left = 2
        Top = 15
        Width = 441
        Height = 167
        Align = alClient
        TabOrder = 0
      end
    end
  end
end
