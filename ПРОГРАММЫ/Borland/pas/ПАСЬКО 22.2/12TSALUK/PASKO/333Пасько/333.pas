USES
  Crt;

TYPE
Line = RECORD
  A, B, C: REAL;
END;

Point = ARRAY [1..2] OF REAL;
Vector = ARRAY [1..2] OF REAL;
Circle = RECORD
  center: Point;
  radius: REAL;
END;

VAR eps: REAL;

FUNCTION Tangentv(ASD:Circle; v: Vector; VAR a: Line):BOOLEAN;
VAR
q, w, e: REAL;
f: Point;
b: Vector;
BEGIN
IF (ASD.radius < eps) OR (sqrt(sqr(v[1])+sqr(v[2])) < eps) THEN
  Tangentv := FALSE
ELSE
  BEGIN
  Tangentv := TRUE;

  q := sqrt(sqr(v[1])+sqr(v[2]));
  b[1] := v[1]; b[2] := v[2];
  b[1] := b[1] / q; b[2] := b[2] / q; // нормирование вектора
  b[1] := b[1] * ASD.radius; b[2] := b[2] * ASD.radius;// умножение на радиус

  f[1] := ASD.center[1] + b[1]; f[2] := ASD.center[2] + b[2]; //откладывание от центра

  a.A := b[1]; a.B := b[2]; a.C := -(a.A*f[1] + a.B*f[2]);// нахождение прямой, для которой получившийся вектор является нормальным
  END;
END;

VAR
  d:Line;
  e:Vector;
  ASD:Circle;

BEGIN
  ClrScr;
  eps := 0.0001;

  WriteLn();
  ASD.center[1]:=0;
  ASD.center[2]:=0;
  ASD.radius:=1.414;
  e[1]:=0; e[2]:= 0.00001;

  IF Tangentv(ASD,e,d) THEN
    WriteLn('прямая имеет уравнение: ',d.A:5:2,'x + ',d.B:5:2,'y + ',d.C:5:2,' = 0')
  ELSE
    WriteLn(Tangentv(ASD,e,d));

ReadKey;
END.









