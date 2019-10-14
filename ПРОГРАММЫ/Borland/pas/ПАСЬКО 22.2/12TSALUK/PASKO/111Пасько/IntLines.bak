USES
  Crt;

TYPE
Line = RECORD
  A, B, C: REAL;
END;
Point = ARRAY [1..2] OF REAL;

VAR eps: REAL;

FUNCTION SectionLines(a, b: Line; VAR P: Point):BOOLEAN;
VAR
s1, s2, S, q: REAL;
BEGIN
  q := (sqrt(sqr(a.A)+sqr(a.B))*sqrt(sqr(b.A)+sqr(b.B))); // произведение модулей векторов
  S := a.A*b.B - a.B*b.A; // основной определитель

  IF abs(S/q) < eps THEN // равенство нулю определителя от нормированных векторов
    BEGIN
    {прямые не имеют точки пересечения'}
      SectionLines := FALSE;
    END
  ELSE
    BEGIN
    {вычисление координат точки пересечения}
      s1 := (-a.C*b.B + a.B*b.C)/s;
      s2 := (-a.A*b.C + b.A*a.C)/s;
      P[1] := s1;
      P[2] := s2;
      SectionLines := TRUE;
    END;
END;


VAR
  a,b:Line;
  P:Point;
BEGIN
  eps := 0.0001;
  ClrScr;

  WriteLn();

  a.A:=1; a.B:=2; a.C:=3;
  b.A:=1.001; b.B:=2; b.C:=3;

  IF SectionLines(a, b, P) THEN
    WriteLn('координаты точки пересечения  ','(',P[1]:5:2,',',P[2]:5:2,')')
  ELSE
    WriteLn('FALSE');

ReadKey;
END.









