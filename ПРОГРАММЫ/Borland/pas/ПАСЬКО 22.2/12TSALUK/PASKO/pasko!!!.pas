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


FUNCTION IntLines(VAR a, b: Line; P: Point):BOOLEAN;
VAR
s1, S2, S, q: REAL;
BEGIN
  q := (sqrt(sqr(a.A)+sqr(a.B))*sqrt(sqr(b.A)+sqr(b.B)));
  S := a.A*b.B - a.B*b.A;
  IF q < eps THEN
    IntLines := FALSE
  ELSE
    BEGIN
      IF abs(S/q) < eps THEN
       BEGIN
          {Writeln('���� �� ����� �窨 ����祭��');}
          IntLines := FALSE;
       END
      ELSE
      BEGIN
        s1 := (-a.C*b.B + a.B*b.C)/s;
        s2 := (-a.A*b.C + b.A*a.C)/s;
        P[1] := s1;
        P[2] := s2;
        IntLines := TRUE;
        {WriteLn('���न���� �窨 ����祭��  ','(',s1:5:2,',',s2:5:2,')');}
      END;
    END;
END;


PROCEDURE PositionOfPoints(P, F: Point; a: Line);
VAR
s1, s2, s,ab: REAL;
b:Line;
G:Point;
BEGIN
  {�஢������ ��אַ� �१ �窨 P � F}
  b.A := F[2] - P[2];
  b.B := P[1] - F[1];
  b.C := F[1]*P[2]-P[1]*F[2];


  S := a.A*b.B - a.B*b.A;

  IF abs(s) < eps THEN
    IF abs(a.C - b.C) < eps THEN
      Writeln('�窨 ����� �� ������ ��אַ�')
    ELSE
      WriteLn('�窨 ����� �� ���� ��஭� �� ��אַ�')
  ELSE
  BEGIN
    s1 := (-a.C*b.B + a.B*b.C)/s;
    s2 := (-a.A*b.C + b.A*a.C)/s;
    G[1] := s1;
    G[2] := s2;

    s := (sqrt(sqr(P[1]-s1)+sqr(P[2]-s2)));
    ab := (sqrt(sqr(F[1]-s1)+sqr(F[2]-s2)));

    IF  s < eps THEN
      WriteLn('���� �� �祪 ����� �� ��אַ�')
    ELSE IF ab < eps THEN
      WriteLn('���� �� �祪 ����� �� ��אַ�')
    ELSE
      BEGIN
        s := s + ab;
        ab :=  sqrt(sqr(P[1]-F[1])+sqr(P[2]-F[2]));
        IF abs(s-ab) < eps THEN
          WriteLn('�窨 ����� �� ࠧ�� ��஭� �� ��אַ�')
        ELSE
          WriteLn('�窨 ����� �� ���� ��஭� �� ��אַ�')
      END;
  END;
END;

FUNCTION PosPoints(P, F: Point; a: Line):BOOLEAN;
VAR
s1, s2, s, ab, q: REAL;
b:Line;
G:Point;
BEGIN
  {�஢������ ��אַ� �१ �窨 P � F}
  b.A := F[2] - P[2];
  b.B := P[1] - F[1];
  b.C := F[1]*P[2]-P[1]*F[2];

  q := (sqrt(sqr(a.A)+sqr(a.B))*sqrt(sqr(b.A)+sqr(b.B)));
  S := a.A*b.B - a.B*b.A;

  IF abs(s/q) < eps THEN
    IF abs(a.C - b.C) < eps THEN
      {Writeln('�窨 ����� �� ������ ��אַ�')} PosPoints := FALSE
    ELSE
      {WriteLn('�窨 ����� �� ���� ��஭� �� ��אַ�')} PosPoints := FALSE
  ELSE
  BEGIN
    s1 := (-a.C*b.B + a.B*b.C)/s;
    s2 := (-a.A*b.C + b.A*a.C)/s;
    G[1] := s1;
    G[2] := s2;

    s := (sqrt(sqr(P[1]-s1)+sqr(P[2]-s2)));
    ab := (sqrt(sqr(F[1]-s1)+sqr(F[2]-s2)));

    IF  s < eps THEN
      {WriteLn('���� �� �祪 ����� �� ��אַ�')} PosPoints := FALSE
    ELSE IF ab < eps THEN
      {WriteLn('���� �� �祪 ����� �� ��אַ�')} PosPoints := FALSE
    ELSE
      BEGIN
        s := s + ab;
        ab :=  sqrt(sqr(P[1]-F[1])+sqr(P[2]-F[2]));
        IF abs(s-ab) < eps THEN
          {WriteLn('�窨 ����� �� ࠧ�� ��஭� �� ��אַ�')} PosPoints := TRUE
        ELSE
          {WriteLn('�窨 ����� �� ���� ��஭� �� ��אַ�')} PosPoints := FALSE
      END;
  END;
{Writeln(PosPoints);}
END;

PROCEDURE Tangentv(ASD:Circle; a: Vector; VAR v: Line);
VAR
q, w, e: REAL;
f: Point;
b: Vector;
BEGIN
q := sqrt(sqr(a[1])+sqr(a[2]));
b[1] := a[1]; b[2] := a[2];
b[1] := b[1] / q; b[2] := b[2] / q;
b[1] := b[1] * ASD.radius; b[2] := b[2] * ASD.radius;

f[1] := ASD.center[1] + b[1]; f[2] := ASD.center[2] + b[2];

v.A := b[1]; v.B := b[2]; v.C := -(v.A*f[1] + v.B*f[2]);
{WriteLn(v.A,v.B,v.C);}
{WriteLn('��ﬠ� ����� �ࠢ�����: ',v.A:5:2,'x + ',v.B:5:2,'y + ',v.C:5:2,' = 0');}
END;


VAR
  a,b,c,d:Line;
  P,F,Q:Point;
  e:Vector;
  ASD:Circle;
BEGIN
  eps := 0.0001;
  ClrScr;

  WriteLn();
  a.A:=0;a.B:=1;a.C:=0;
  b.A:=1;b.B:=1;b.C:=-3;
  P[1]:=0;P[2]:=0;

  WriteLn(IntLines(a,b,P));
  WriteLn('���न���� �窨 ����祭��  ','(',P[1]:5:2,',',P[2]:5:2,')');

  WriteLn();
  Q[1]:=0;Q[2]:=0;
  F[1]:=1;F[2]:=0;
  c.A:=0;c.B:=1;c.C:=0;
  {PositionOfPoints(Q,F,c);}
  WriteLn(PosPoints(Q,F,c));

  WriteLn();
  d.A:=0;d.B:=1;d.C:=0;
  ASD.center[1]:=0;
  ASD.center[2]:=0;
  ASD.radius:=1;
  e[1]:=0; e[2]:= -5;
  Tangentv(ASD,e,d);
  WriteLn('��ﬠ� ����� �ࠢ�����: ',d.A:5:2,'x + ',d.B:5:2,'y + ',d.C:5:2,' = 0');


ReadKey;
END.















