object Form1: TForm1
  Left = 321
  Top = 223
  Width = 550
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
    Left = 72
    Top = 24
    Width = 11
    Height = 21
    Caption = 'X'
  end
  object Label2: TLabel
    Left = 120
    Top = 24
    Width = 11
    Height = 21
    Caption = 'Y'
  end
  object Label3: TLabel
    Left = 32
    Top = 40
    Width = 22
    Height = 21
    Caption = 'A='
  end
  object Label4: TLabel
    Left = 32
    Top = 64
    Width = 22
    Height = 21
    Caption = 'B='
  end
  object Edit1: TEdit
    Left = 56
    Top = 40
    Width = 41
    Height = 29
    TabOrder = 0
    Text = 'Edit1'
  end
  object Edit2: TEdit
    Left = 104
    Top = 40
    Width = 41
    Height = 29
    TabOrder = 1
    Text = 'Edit2'
  end
  object Edit3: TEdit
    Left = 56
    Top = 64
    Width = 41
    Height = 29
    TabOrder = 2
    Text = 'Edit3'
  end
  object Edit4: TEdit
    Left = 104
    Top = 64
    Width = 41
    Height = 29
    TabOrder = 3
    Text = 'Edit4'
  end
  object Button1: TButton
    Left = 152
    Top = 24
    Width = 265
    Height = 25
    Caption = #1056#1072#1089#1090#1086#1103#1085#1080#1077' '#1088#1072#1074#1085#1086
    TabOrder = 4
    OnClick = Button1Click
  end
  object Memo1: TMemo
    Left = 152
    Top = 56
    Width = 265
    Height = 105
    Lines.Strings = (
      'Memo1')
    ScrollBars = ssVertical
    TabOrder = 5
  end
  object Chart1: TChart
    Left = 32
    Top = 168
    Width = 385
    Height = 209
    BackWall.Brush.Color = clWhite
    BackWall.Brush.Style = bsClear
    Title.Text.Strings = (
      'TChart')
    Title.Visible = False
    Legend.Visible = False
    View3D = False
    TabOrder = 6
    object Series1: TLineSeries
      Marks.ArrowLength = 8
      Marks.Visible = False
      SeriesColor = clRed
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
