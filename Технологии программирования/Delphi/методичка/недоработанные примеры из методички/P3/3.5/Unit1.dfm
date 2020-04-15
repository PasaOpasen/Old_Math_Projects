object Form1: TForm1
  Left = 548
  Top = 324
  Width = 352
  Height = 341
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
    Top = 16
    Width = 22
    Height = 21
    Caption = 'X='
  end
  object Label2: TLabel
    Left = 8
    Top = 56
    Width = 22
    Height = 21
    Caption = 'Y='
  end
  object GroupBox1: TGroupBox
    Left = 120
    Top = 8
    Width = 209
    Height = 137
    Caption = #1042#1099#1073#1086#1088
    TabOrder = 0
    object CheckBox1: TCheckBox
      Left = 8
      Top = 32
      Width = 177
      Height = 17
      Caption = 'X>0 Y>0 X+Y<5'
      Checked = True
      State = cbChecked
      TabOrder = 0
    end
    object CheckBox2: TCheckBox
      Left = 8
      Top = 64
      Width = 193
      Height = 17
      Caption = '0.5<X<3, -2<Y<4'
      TabOrder = 1
    end
    object CheckBox3: TCheckBox
      Left = 8
      Top = 96
      Width = 137
      Height = 17
      Caption = 'X*X+Y*Y<=7'
      TabOrder = 2
    end
  end
  object Edit1: TEdit
    Left = 32
    Top = 16
    Width = 73
    Height = 29
    TabOrder = 1
  end
  object Edit2: TEdit
    Left = 32
    Top = 56
    Width = 73
    Height = 29
    TabOrder = 2
  end
  object Memo1: TMemo
    Left = 8
    Top = 152
    Width = 321
    Height = 145
    ScrollBars = ssVertical
    TabOrder = 3
  end
  object Button1: TButton
    Left = 8
    Top = 88
    Width = 105
    Height = 57
    Caption = #1042#1099#1095#1077#1089#1083#1080#1090#1100
    TabOrder = 4
    OnClick = Button1Click
  end
end
