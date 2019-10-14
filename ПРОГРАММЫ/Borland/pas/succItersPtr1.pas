TYPE
  RHS = PROCEDURE(px: PDouble; py: PDouble);


PROCEDURE CopyVector(px: PDouble; n: WORD; py: PDouble);
{x -> y}
VAR
  i: WORD;
BEGIN
  FOR i := 1 TO n DO
    BEGIN
      py^ := px^;
      Inc(px);  Inc(py);
    END;
END;


FUNCTION Dist2(px, py: PDouble; n: WORD): REAL;
VAR
  i: WORD;
  s: REAL;
BEGIN
  s := 0;
  FOR i := 1 TO n DO
    BEGIN
      s := s + Sqr(py^ - px^);
      Inc(px);  Inc(py);
    END;
  Dist2 := s;
END;

PROCEDURE MarkPoint(py: PDouble; n: WORD; counter: LONGINT); {OK}
VAR
  d: REAL;
  y1, y2: REAL;
BEGIN
  y1 := py^;
  Inc(py);
  y2 := py^;

  d := Sqrt(Sqr(y1-Pi) + y2*y2);
  WriteLn(counter:6, y1:16:9, y2:16:9
, '  ', d);
  ReadLn;
END;

FUNCTION SuccIters(F: RHS; px: PDouble; n: WORD; eps: REAL;
                    maxcount: LONGINT): BOOLEAN;  {OK}
VAR
  py, p, tmp: PDouble;
  d: REAL;
  counter: LONGINT;
BEGIN
  succIters := FALSE;
  counter := 0;
  eps := eps*eps;
  GetMem(py, n * SizeOf(REAL));
  WHILE counter <= maxcount DO
    BEGIN
      Inc(counter);

      F(px, py);   {짨᫥ ᫥䯩 ⦠樨}

      MarkPoint(py, n, counter);  { ⫠!!!}

      d := Dist2(px, py, n);

      CopyVector(py, n, px);

      IF d < eps THEN
        BEGIN
          succIters := TRUE;
          Break;
        END;

    END;

  FreeMem(py, n * SizeOf(REAL));
END;

PROCEDURE MySystem(px: PDouble; py: PDouble);
{௦䠠 짮⥫ ⯠៰x^ 짨ᬯˊ⯠ ᫥䯩 ⦠樨  ࠧ頥ࠥ  ড៰y.
୮⣬ ᩡ⥬頨⢭ 짮⥫���
VAR
  x1, x2: REAL;

BEGIN
  x1 := px^;
  Inc(px);
  x2 := px^;

  py^ := sin(x1)/2 + cos(x2)/2 + Pi - 0.5;
  Inc(py);
  py^ := cos(x1 - Pi/2)/3 + sin(x1 + x2)/2;
END;

{MAIN PRO}
VAR
  x, exact: ARRAY[1..2] OF REAL;
  eps: REAL;
  success: BOOLEAN;

BEGIN
  exact[1] := Pi;  exact[2] := 0;

  x[1] := 0;  x[2] := Pi;
  eps := 1.0E-8; //0.8e-3;

  success := SuccIters(@MySystem, @x, 2, eps, 10);
  MarkPoint(@x, 2, 0);
END.
