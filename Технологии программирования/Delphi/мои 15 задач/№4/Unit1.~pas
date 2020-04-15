unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, ExtCtrls, TeeProcs, TeEngine, Chart, StdCtrls, Series;

type
  TForm1 = class(TForm)
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    Label4: TLabel;
    Label5: TLabel;
    Label6: TLabel;
    Edit1: TEdit;
    Edit2: TEdit;
    Edit3: TEdit;
    Edit4: TEdit;
    Edit5: TEdit;
    Edit6: TEdit;
    Button1: TButton;
    Label7: TLabel;
    Edit7: TEdit;
    Button2: TButton;
    Button3: TButton;
    Series1: TLineSeries;
    Series2: TLineSeries;
    Series3: TLineSeries;
    Chart1: TChart;
    procedure FormCreate(Sender: TObject);
    procedure Button1Click(Sender: TObject);
    procedure Button2Click(Sender: TObject);
    procedure Button3Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;
  Xmin, Xmax, Ymin, Ymax, Hx, Hy,h: Extended;

implementation

{$R *.dfm}

procedure TForm1.FormCreate(Sender: TObject);
begin
  {установка начальных параметров координатных осей}
  Xmin :=  0;  Xmax := 2 * pi;
  Ymin := -1; Ymax := 1;
  Hx := pi/2; Hy := 0.5;
  h := 0.01; //установка шага расчета таблицы
  {Вывод данных в окна однострочных редакторов}
  Edit1.Text := FloatToStr(Xmin);
  Edit2.Text := FloatToStr(Xmax);
  Edit3.Text := FloatToStr(Ymin);
  Edit4.Text := FloatToStr(Ymax);
  Edit5.Text := FloatToStr(Hx);
  Edit6.Text := FloatToStr(Hy);
  Edit7.Text := FloatToStr(h);
  Chart1.BottomAxis.Automatic := False; //Отключение автоматического определения параметров нижней оси
  Chart1.BottomAxis.Minimum := Xmin; //Установка левой границы нижней оси
  Chart1.BottomAxis.Maximum := Xmax; //Установка правой границы нижней оси
  Chart1.LeftAxis.Automatic := False; //Отключение автоматического определения параметров левой оси
  Chart1.LeftAxis.Minimum := Ymin; //Установка нижней границы левой оси
  Chart1.LeftAxis.Maximum := Ymax; //Установка верхней границы левой оси
  Chart1.BottomAxis.Increment := Hx; //Установка шага разметки по нижней оси
  Chart1.LeftAxis.Increment := Hy; //Установка шага разметки по левой оси
end;

procedure TForm1.Button1Click(Sender: TObject);
begin
  {чтение данных из окон однострочных редакторов}
  Xmin := StrToFloat(Edit1.Text); Xmax := StrToFloat(Edit2.Text);
  Ymin := StrToFloat(Edit3.Text); Ymax := StrToFloat(Edit4.Text);
  Hx := StrToFloat(Edit5.Text); Hy := StrToFloat(Edit6.Text);
  Chart1.BottomAxis.Minimum := Xmin; //Установка левой границы нижней оси
  Chart1.BottomAxis.Maximum := Xmax + 0.01; //Установка правой границы нижней оси
  Chart1.LeftAxis.Minimum := Ymin; //Установка нижней границы левой оси
  Chart1.LeftAxis.Maximum := Ymax; //Установка верхней границы левой оси
  Chart1.BottomAxis.Increment := Hx; //Установка шага разметки по нижней оси
  Chart1.LeftAxis.Increment := Hy; //Установка шага разметки по левой оси
end;

procedure TForm1.Button2Click(Sender: TObject);
var
  x, y1, y2, y3: Extended;
begin
  Series1.Clear;
  Series2.Clear;
  Series3.Clear;
  Xmin := StrToFloat(Edit1.Text);
  Xmax := StrToFloat(Edit2.Text);
  h := StrToFloat(Edit7.Text); //Шаг расчета таблицы для графика
  x := Xmin; //Начальное значение по оси X
  repeat
    y1 := sin(x); //Расчет функции
    Series1.AddXY(x, y1, '', clTeeColor); //Вывод точки на график
    y2 := cos(x); //Расчет функции
    Series2.AddXY(x, y2, '', clTeeColor); //Вывод точки на график
    x := x + h; //Увеличение значения X на величину шага
    y3 := Sin(x) * Cos(x); //Расчет функции
    Series3.AddXY(x, y3, '', clTeeColor); //Вывод точки на график
    x := x + h; //Увеличение значения Х на величину шага
  Until (x > Xmax);
end;

procedure TForm1.Button3Click(Sender: TObject);
begin
  Close;
end;

end.
