USES
Crt;

CONST
  n = 3;
  m = 5;

TYPE
  Matrix = ARRAY [1..n,1..m] OF INTEGER;
  Strng = ARRAY [1..m] OF INTEGER;
  Sing = ARRAY [1..n] OF INTEGER;

PROCEDURE Powerm (x: Matrix; y:Strng; var c:Sing);
VAR
  i,k,sum:integer;
BEGIN
  FOR i := 1 TO n DO
    BEGIN
      sum := 0;
      FOR k := 1 TO m DO
        BEGIN
          sum := sum + x[i,k]*y[k];
        END;
      c[i] := sum;
    END;
END;

VAR
  c:Sing;
  x:Matrix;
  y:Strng;
  i:INTEGER;

BEGIN
x[1,1] := 10; x[1,2] := 1; x[1,3] := 6; x[1,4] := 111; x[1,5] := 1;
x[2,1] := 60; x[2,2] := 1; x[2,3] := 100; x[2,4] := 1; x[2,5] := 65;
x[3,1] := 245; x[3,2] := 1; x[3,3] := 6; x[3,4] := 1; x[3,5] := 1;

y[1] := 1; y[2] := 1; y[3] := 1; y[4] := 0; y[5] := 1;


Powerm(x,y,c);

FOR i := 1 TO n DO
WriteLn(c[i]);


ReadKey;
END.