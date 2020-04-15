object Form1: TForm1
  Left = 168
  Top = 304
  Width = 968
  Height = 335
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
    Left = 16
    Top = 8
    Width = 11
    Height = 21
    Caption = 'X'
  end
  object Label2: TLabel
    Left = 16
    Top = 40
    Width = 11
    Height = 21
    Caption = 'Y'
  end
  object Edit1: TEdit
    Left = 40
    Top = 8
    Width = 121
    Height = 29
    TabOrder = 0
    Text = '0'
  end
  object Edit2: TEdit
    Left = 40
    Top = 40
    Width = 121
    Height = 29
    TabOrder = 1
    Text = '0'
  end
  object Button1: TButton
    Left = 168
    Top = 8
    Width = 265
    Height = 65
    Caption = 'u=|cos(x)|+arctg(1/y)'
    TabOrder = 2
    OnClick = Button1Click
  end
  object Memo1: TMemo
    Left = 16
    Top = 80
    Width = 417
    Height = 209
    ScrollBars = ssVertical
    TabOrder = 3
  end
  object Chart1: TChart
    Left = 440
    Top = 8
    Width = 505
    Height = 281
    BackWall.Brush.Color = clWhite
    BackWall.Brush.Style = bsClear
    Title.Text.Strings = (
      '|cos(x)|+arctg(1/x)')
    Legend.Visible = False
    View3D = False
    TabOrder = 4
    object Series1: TLineSeries
      Marks.ArrowLength = 8
      Marks.Visible = False
      SeriesColor = clRed
      Dark3D = False
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
