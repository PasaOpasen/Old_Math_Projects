object Form1: TForm1
  Left = 198
  Top = 137
  Width = 928
  Height = 480
  Caption = 'Form1'
  Color = clBtnFace
  Font.Charset = RUSSIAN_CHARSET
  Font.Color = clWindowText
  Font.Height = -19
  Font.Name = 'Courier New'
  Font.Style = []
  OldCreateOrder = False
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 21
  object Label1: TLabel
    Left = 8
    Top = 24
    Width = 33
    Height = 21
    Caption = 'MIN'
  end
  object Label2: TLabel
    Left = 8
    Top = 56
    Width = 33
    Height = 21
    Caption = 'MAX'
  end
  object Label3: TLabel
    Left = 80
    Top = 0
    Width = 11
    Height = 21
    Caption = 'X'
  end
  object Label4: TLabel
    Left = 152
    Top = 0
    Width = 11
    Height = 21
    Caption = 'Y'
  end
  object Label5: TLabel
    Left = 8
    Top = 88
    Width = 33
    Height = 21
    Caption = #1064#1072#1075
  end
  object Chart1: TChart
    Left = 192
    Top = 0
    Width = 713
    Height = 441
    BackWall.Brush.Color = clWhite
    BackWall.Brush.Style = bsClear
    BackWall.Pen.Width = 3
    Title.Text.Strings = (
      #1043#1088#1072#1092#1080#1082)
    Chart3DPercent = 10
    Frame.Width = 3
    View3D = False
    TabOrder = 0
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
      SeriesColor = clBlue
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
      SeriesColor = 33023
      Title = 'cos(x)*sin(x)'
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
  object Edit1: TEdit
    Left = 48
    Top = 24
    Width = 65
    Height = 29
    TabOrder = 1
    Text = 'Edit1'
  end
  object Edit2: TEdit
    Left = 120
    Top = 24
    Width = 65
    Height = 29
    TabOrder = 2
    Text = 'Edit2'
  end
  object Edit3: TEdit
    Left = 48
    Top = 56
    Width = 65
    Height = 29
    TabOrder = 3
    Text = 'Edit3'
  end
  object Edit4: TEdit
    Left = 120
    Top = 56
    Width = 65
    Height = 29
    TabOrder = 4
    Text = 'Edit4'
  end
  object Edit5: TEdit
    Left = 48
    Top = 88
    Width = 65
    Height = 29
    TabOrder = 5
    Text = 'Edit3'
  end
  object Edit6: TEdit
    Left = 120
    Top = 88
    Width = 65
    Height = 29
    TabOrder = 6
    Text = 'Edit4'
  end
  object Button1: TButton
    Left = 8
    Top = 128
    Width = 177
    Height = 33
    Caption = #1059#1089#1090#1072#1085#1086#1074#1080#1090#1100' '#1086#1089#1080
    TabOrder = 7
    OnClick = Button1Click
  end
  object Button2: TButton
    Left = 8
    Top = 168
    Width = 177
    Height = 33
    Caption = #1043#1088#1072#1092#1080#1082
    TabOrder = 8
    OnClick = Button2Click
  end
end
