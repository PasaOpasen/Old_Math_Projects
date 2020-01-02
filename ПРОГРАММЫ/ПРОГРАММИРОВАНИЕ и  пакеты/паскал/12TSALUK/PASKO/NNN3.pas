USES
  Crt;

TYPE
Line = RECORD
  A, B, C: REAL;
END;
Point = ARRAY [1..2] OF REAL;


VAR eps: REAL;

FUNCTION PointInLine(A, B, P: Point):BOOLEAN;
VAR
q: REAL;
l: Line;
BEGIN
  l.A := B[2] - A[2];
  l.B := B[1] - A[1];
  l.C := B[1]*A[2] - A[1]*B[2];

  q := abs(l.A*P[1]+l.B*P[2]+l.C)/sqrt(sqr(l.A)+sqr(l.B));
  IF q < eps THEN
     PointInLine := TRUE
  ELSE
     PointInLine := FALSE;
END;


VAR
  A,B,P:Point;
BEGIN
  eps := 0.0001;
  ClrScr;

  WriteLn();
  P[1]:=0;P[2]:=0;
  A[1]:=1;A[2]:=1;
  B[1]:=1;B[2]:=2;

  WriteLn(PointInLine(A,B,P));

ReadKey;
END.





