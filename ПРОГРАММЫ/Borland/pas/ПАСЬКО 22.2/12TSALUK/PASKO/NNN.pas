USES
  Crt;

TYPE
Line = RECORD
  A, B, C: REAL;
END;
Point = ARRAY [1..2] OF REAL;


VAR eps: REAL;

FUNCTION PointInLine(P: Point; a: Line):BOOLEAN;
VAR
q: REAL;
BEGIN
  q := abs(a.A*P[1]+a.B*P[2]+a.C)/sqrt(sqr(a.A)+sqr(a.B));
  IF q < eps THEN
     PointInLine := TRUE
  ELSE
     PointInLine := FALSE;
END;


VAR
  a:Line;
  P:Point;
BEGIN
  eps := 0.0001;
  ClrScr;

  WriteLn();
  a.A:=0;a.B:=1;a.C:=0;
  P[1]:=0;P[2]:=0;

  WriteLn(PointInLine(P,a));

ReadKey;
END.









