UNIT Polynomend;


INTERFACE
TYPE
   Preal = ^real;
  Polynom = RECORD
    deg:WORD;
    coef:Preal;
  END;
FUNCTION CreatePolynom (n:WORD): Polynom;
FUNCTION GetCoef(p:Polynom; k:WORD):REAL;
PROCEDURE SetCoef(p:Polynom; k:WORD; a:REAL);
FUNCTION SumPolynom (p,q: Polynom): Polynom;
FUNCTION DifPolynom (p,q: Polynom): Polynom;
FUNCTION PowerPolynom (p,q: Polynom): Polynom;
PROCEDURE InPutP(a:Polynom);
PROCEDURE OutPutP(a:Polynom);

IMPLEMENTATION

{создание полинома степени n}
FUNCTION CreatePolynom (n:WORD): Polynom;
VAR
  p:Polynom;
BEGIN
  new(p.coef);
  p.deg := n;
  GetMem(p.coef,(n+1)*sizeOf(real));
  CreatePolynom:=p;
END;


{обнуление полинома}
{PROCEDURE KillPolynom (VAR p:Polynom);
BEGIN
  FreeMem(p.coef,(p.deg+1)*sizeOf(real));
  dispose(p);
  p:= NULL;
END;  }


{взятие значения k-го коэффициента}
FUNCTION GetCoef(p:Polynom; k:WORD):REAL;
VAR
  ptr:Preal;
BEGIN
  ptr:=p.coef;
  inc(ptr,k);
  GetCoef:=ptr^;
END;

{присвоение k-му коэффициенту значение a}
PROCEDURE SetCoef(p:Polynom; k:WORD; a:REAL);
VAR
  ptr:Preal;
BEGIN
  ptr:=p.coef;
  inc(ptr,k);
  ptr^:=a;

END;

{сумма полиномов}
FUNCTION SumPolynom (p,q: Polynom): Polynom;
VAR
  f:Polynom;
  i: WORD;
BEGIN
  WriteLn();
  IF p.deg>q.deg THEN f:=CreatePolynom(p.deg)
  ELSE f:=CreatePolynom(q.deg);

  IF p.deg=q.deg THEN
    FOR i:=0 TO p.deg DO SetCoef(f,i,GetCoef(p,i)+GetCoef(q,i))
  ELSE IF p.deg<q.deg THEN
    BEGIN
      FOR i:=q.deg DOWNTO p.deg+1 DO SetCoef(f,i,GetCoef(q,i));
      FOR i:=p.deg DOWNTO 0 DO  SetCoef(f,i,GetCoef(p,i)+GetCoef(q,i));
    END
  ELSE IF q.deg<p.deg THEN
    BEGIN
      FOR i:=p.deg DOWNTO q.deg+1 DO SetCoef(f,i,GetCoef(p,i));
      FOR i:=q.deg DOWNTO 0 DO SetCoef(f,i,GetCoef(p,i)+GetCoef(q,i));
    END;

  SumPolynom:=f;

END;

{разность полиномов}
FUNCTION DifPolynom (p,q: Polynom): Polynom;
VAR
  f:Polynom;
  i: WORD;
BEGIN
  WriteLn();
  IF p.deg>q.deg THEN f:=CreatePolynom(p.deg)
  ELSE f:=CreatePolynom(q.deg);

  IF p.deg=q.deg THEN
    FOR i:=0 TO p.deg DO SetCoef(f,i,GetCoef(p,i)-GetCoef(q,i))
  ELSE IF p.deg<q.deg THEN
    BEGIN
      FOR i:=q.deg DOWNTO p.deg+1 DO  SetCoef(f,i,-GetCoef(q,i));
      FOR i:=p.deg DOWNTO 0 DO SetCoef(f,i,GetCoef(p,i)-GetCoef(q,i));
    END
  ELSE IF q.deg<p.deg THEN
    BEGIN
      FOR i:=p.deg DOWNTO q.deg+1 DO  SetCoef(f,i,GetCoef(p,i));
      FOR i:=q.deg DOWNTO 0 DO SetCoef(f,i,GetCoef(p,i)-GetCoef(q,i));
    END;

  DifPolynom:=f;
END;

{произведение полиномов}
FUNCTION PowerPolynom (p,q: Polynom): Polynom;
VAR
  f:Polynom;
  s:REAL;
  i,j: WORD;
BEGIN
  WriteLn();
  f:=CreatePolynom(q.deg+p.deg);

  FOR i:=0 TO p.deg+q.deg DO
  BEGIN
    s:=0;
    FOR j:=0 TO i DO s:= s+GetCoef(p,j)*GetCoef(q,i-j);
    SetCoef(f,i,s)
  END;

  PowerPolynom:=f;
END;

{ввод полинома (вспомогательная)}
PROCEDURE InPutP(a:Polynom);
VAR
  i,n:WORD;
  b:REAL;
BEGIN
  //ReadLn(n); a:=CreatePolynom(n);
  WriteLn('input coef (',a.deg+1,' koeffitsents on increase of degree)');
  FOR i:=0 TO a.deg DO
  BEGIN
    Read(b);
    SetCoef(a,i,b);
  END;

END;

{вывод полинома (вспомогательная)}
PROCEDURE OutPutP(a:Polynom);
VAR
  i: WORD;
BEGIN
  //WriteLn();
  IF a.deg >= 2 THEN
  BEGIN
    Write('(',GetCoef(a,0):3:4,') + ');
    FOR i:=1 TO (a.deg-1) DO Write('(',GetCoef(a,i):3:4,')x^',i,' + ');
    Write('(',GetCoef(a,a.deg):3:4,')x^',i+1);WriteLn();
  END
  ELSE
  BEGIN
    Write('(',GetCoef(a,0):3:4,') + (',GetCoef(a,1):3:4,')x^1');
    WriteLn();
  END;
END;
BEGIN

END.



