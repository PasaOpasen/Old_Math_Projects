USES
  Crt;

TYPE
Line = RECORD
  A, B, C: REAL;
END;

Point = ARRAY [1..2] OF REAL;

END;

VAR eps: REAL;

FUNCTION PosPoints(P, Q: Point; a: Line):INTEGER;
VAR
d: REAL;
BEGIN
  d := (a.A*P[1]+a.B*P[2]+a.C)*(a.A*Q[1]+a.B*Q[2]+a.C);
  IF (abs((a.A*P[1]+a.B*P[2]+a.C))/sqrt(sqr(a.A)+sqr(a.B)) < eps) OR (abs((a.A*Q[1]+a.B*Q[2]+a.C))/sqrt(sqr(a.A)+sqr(a.B)) < eps) THEN   // хотя бы одна из точек принадлежит прямой
    PosPoints := 0
  ELSE
    IF d > 0 THEN
      {точки лежат по одну сторону от прямой} PosPoints := -1
    ELSE
      {точки лежат по разные стороны от прямой} PosPoints := 1
END;

VAR
  c:Line;
  F,Q:Point;
BEGIN
  eps := 0.0001;
  ClrScr;


  WriteLn();
  Q[1]:=2.2; Q[2]:=5.6;
  F[1]:=5.1; F[2]:=-6.7;
  c.A:=0.3; c.B:=1; c.C:=0;
  WriteLn(PosPoints(Q,F,c));

ReadKey;
END.







