object Form1: TForm1
  Left = 306
  Top = 204
  Width = 953
  Height = 470
  Caption = #1057#1077#1084#1077#1089#1090#1088#1086#1074#1086#1077' '#8470'4, '#1055#1040#1057#1068#1050#1054
  Color = clMoneyGreen
  Font.Charset = RUSSIAN_CHARSET
  Font.Color = clBlack
  Font.Height = -19
  Font.Name = 'Trebuchet MS'
  Font.Style = []
  OldCreateOrder = False
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 24
  object Label1: TLabel
    Left = 48
    Top = 24
    Width = 49
    Height = 24
    Caption = 'Xmin:'
  end
  object Label2: TLabel
    Left = 48
    Top = 64
    Width = 54
    Height = 24
    Caption = 'Xmax:'
  end
  object Label3: TLabel
    Left = 48
    Top = 104
    Width = 49
    Height = 24
    Caption = 'Ymin:'
  end
  object Label4: TLabel
    Left = 48
    Top = 144
    Width = 54
    Height = 24
    Caption = 'Ymax:'
  end
  object Label5: TLabel
    Left = 16
    Top = 192
    Width = 85
    Height = 24
    Caption = #1064#1072#1075' '#1087#1086' X:'
  end
  object Label6: TLabel
    Left = 16
    Top = 240
    Width = 85
    Height = 24
    Caption = #1064#1072#1075' '#1087#1086' Y:'
  end
  object Label7: TLabel
    Left = 16
    Top = 384
    Width = 197
    Height = 24
    Caption = #1064#1072#1075' '#1088#1072#1089#1095#1077#1090#1072' '#1090#1072#1073#1083#1080#1094#1099':'
  end
  object Edit1: TEdit
    Left = 112
    Top = 16
    Width = 105
    Height = 32
    TabOrder = 0
    Text = 'Edit1'
  end
  object Edit2: TEdit
    Left = 112
    Top = 56
    Width = 105
    Height = 32
    TabOrder = 1
    Text = 'Edit2'
  end
  object Edit3: TEdit
    Left = 112
    Top = 96
    Width = 105
    Height = 32
    TabOrder = 2
    Text = 'Edit3'
  end
  object Edit4: TEdit
    Left = 112
    Top = 136
    Width = 105
    Height = 32
    TabOrder = 3
    Text = 'Edit4'
  end
  object Edit5: TEdit
    Left = 112
    Top = 184
    Width = 105
    Height = 32
    TabOrder = 4
    Text = 'Edit5'
  end
  object Edit6: TEdit
    Left = 112
    Top = 232
    Width = 105
    Height = 32
    TabOrder = 5
    Text = 'Edit6'
  end
  object Button1: TButton
    Left = 16
    Top = 288
    Width = 233
    Height = 49
    Caption = #1056#1072#1079#1084#1077#1090#1080#1090#1100' '#1086#1089#1080
    TabOrder = 6
    OnClick = Button1Click
  end
  object Edit7: TEdit
    Left = 216
    Top = 384
    Width = 89
    Height = 32
    TabOrder = 7
    Text = 'Edit7'
  end
  object Button2: TButton
    Left = 352
    Top = 376
    Width = 209
    Height = 41
    Caption = #1055#1086#1089#1090#1088#1086#1080#1090#1100' '#1075#1088#1072#1092#1080#1082#1080
    TabOrder = 8
    OnClick = Button2Click
  end
  object Button3: TButton
    Left = 760
    Top = 376
    Width = 161
    Height = 41
    Caption = #1042#1099#1093#1086#1076
    TabOrder = 9
    OnClick = Button3Click
  end
  object Chart1: TChart
    Left = 264
    Top = 16
    Width = 657
    Height = 329
    BackWall.Brush.Color = clWhite
    BackWall.Brush.Style = bsClear
    Title.Font.Charset = DEFAULT_CHARSET
    Title.Font.Color = clBlack
    Title.Font.Height = -16
    Title.Font.Name = 'Arial'
    Title.Font.Style = [fsBold]
    Title.Text.Strings = (
      #1043#1088#1072#1092#1080#1082' '#1092#1091#1085#1082#1094#1080#1081)
    View3D = False
    TabOrder = 10
    object Series1: TLineSeries
      Marks.ArrowLength = 8
      Marks.Visible = False
      SeriesColor = clRed
      Title = 'sin(x)'
      Pointer.InflateMargins = True
      Pointer.Style = psRectangle
      Pointer.Visible = False
      XValues.DateTime = False
      XValues.Name = 'X'
      XValues.Multiplier = 1.000000000000000000
      XValues.Order = loAscending
      YValues.DateTime = False
      YValues.Name = 'Y'
      YValues.Multiplier = 1.000000000000000000
      YValues.Order = loNone
    end
    object Series2: TLineSeries
      Marks.ArrowLength = 8
      Marks.Visible = False
      SeriesColor = clGreen
      Title = 'cos(x)'
      Pointer.InflateMargins = True
      Pointer.Style = psRectangle
      Pointer.Visible = False
      XValues.DateTime = False
      XValues.Name = 'X'
      XValues.Multiplier = 1.000000000000000000
      XValues.Order = loAscending
      YValues.DateTime = False
      YValues.Name = 'Y'
      YValues.Multiplier = 1.000000000000000000
      YValues.Order = loNone
    end
    object Series3: TLineSeries
      Marks.ArrowLength = 8
      Marks.Visible = False
      SeriesColor = clBlue
      Title = 'sin(x)*cos(x)'
      Pointer.InflateMargins = True
      Pointer.Style = psRectangle
      Pointer.Visible = False
      XValues.DateTime = False
      XValues.Name = 'X'
      XValues.Multiplier = 1.000000000000000000
      XValues.Order = loAscending
      YValues.DateTime = False
      YValues.Name = 'Y'
      YValues.Multiplier = 1.000000000000000000
      YValues.Order = loNone
    end
  end
end
