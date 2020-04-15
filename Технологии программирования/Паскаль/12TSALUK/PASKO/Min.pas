USES Crt;

TYPE
Dependence = FUNCTION (x:REAL):REAL;

FUNCTION f(x:REAL):REAL;
BEGIN
f := 2*x*x;
END;


FUNCTION zel(x:REAL):WORD;
VAR
i:WORD;
BEGIN
i := 0;
WHILE(i<x) DO i := i+1;
zel := i-1;
END;

FUNCTION Min(f:Dependence):REAL;

VAR
i,k: WORD;
d,a,b,loc,e: REAL;
LABEL z1,z2;


BEGIN
WriteLn('введите точность и шаг');
ReadLn(e,d);
WriteLn('введите границы поиска минимума функции, левую и правую');
ReadLn(a,b);

IF a >= b THEN
  BEGIN
  WriteLn ('ошибка');
  END
ELSE

  BEGIN
  z1:
  k := zel((b-a)/d);
  loc := f(a);

  FOR i := 1 TO k DO
    BEGIN
    a := a + d;
    IF f(a) > loc THEN
      BEGIN
      IF (b-a)/2 < e THEN
        BEGIN
        Min := (b-a)/2;
        GOTO z2;
        END
      ELSE
        BEGIN
        b := a;
        a := a - d;
        d := d/2;
        GOTO z1;
        END;
      END
    ELSE
    loc := f(a);
    END;
  END;
z2:
END;


BEGIN
WriteLn('минимум  ', Min(s));
ReadKey;
END.