USES
Crt;

CONST
  n = 5;
  m = 4;
VAR
  a:ARRAY [1..m,1..n] OF integer;
  i, k, b, j, sum:integer;

BEGIN
  sum := 0;
  k := 0;

a[1,1] := 7;  a[1,2] := -7;  a[1,3] := 52;  a[1,4] := -300;  a[1,5] := 1;
a[2,1] := 1;  a[2,2] := -8;  a[2,3] := 53;  a[2,4] := -387;  a[2,5] := 0;
a[3,1] := 10;  a[3,2] := -8;  a[3,3] := 15;  a[3,4] := -100;  a[3,5] := 8;
a[4,1] := 300;  a[4,2] := 85;  a[4,3] := 52;  a[4,4] := 101;  a[4,5] := 28;


{SOLUTION}
FOR i := 1 TO n DO
  BEGIN
  b := a[1,i];
  FOR j := 2 TO m DO
    BEGIN
      IF a[j,i] > b THEN b := a[j,i];
    END;
  IF b mod 2 = 0 THEN
    BEGIN
      sum := sum + b;
      k := k+1;
    END;
  END;

{OUTPUT}
IF k <> 0 THEN
WriteLn('сумма максимальных элементов столбцов, оказавшихся чётными, равна  ',sum)
ELSE
Writeln('максимальных - при этом чётных - элементов столбцов не найдено');

ReadKey;
END.