USES
  crt;

TYPE
   Preal = ^real;
  Polynom = RECORD
    deg:WORD;
    coef:Preal;
  END;


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

  //WriteLn(GetCoef(p,k));
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

  {Write('(',GetCoef(f,0):3:4,') + ');
  FOR i:=1 TO (f.deg-1) DO Write('(',GetCoef(f,i):3:4,')x^',i,' + ');
  Write('(',GetCoef(f,f.deg):3:4,')x^',i+1);}
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


VAR
 a,b:Polynom;
 n:WORD;
BEGIN
 ClrScr;

  //WriteLn();
  WriteLn('input n (degree of polynom A)');
  ReadLn(n); a:=CreatePolynom(n);
  InPutP(a);OutPutP(a);

  WriteLn();
  WriteLn('input n (degree of polynom B)');
  ReadLn(n); b:=CreatePolynom(n);
  InPutP(b);OutPutP(b);

 Write('sum: ');OutPutP(SumPolynom(a,b));
 Write('difference: ');OutPutP(DifPolynom(a,b));
 Write('power: ');OutPutP(PowerPolynom(a,b));

 ReadKey;
END.



