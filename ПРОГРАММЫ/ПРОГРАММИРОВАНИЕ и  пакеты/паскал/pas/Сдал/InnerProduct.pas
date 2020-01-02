USES
  crt;

TYPE
  //Vector = array [1..1000] of real;
  Preal = ^real;


PROCEDURE Inp(VAR q:Preal; n:integer);
VAR
  i:word;
  e:Preal;
BEGIN
  GetMem(q,(n)*sizeOf(real));
  e:=q;
  WriteLn('Input ', n, ' coordinates of vector');
  FOR i:=1 to n DO
  BEGIN
    inc(e,i-1);
    Read(e^);
    e:=q;
  END;
END;

FUNCTION Power(q,w:Preal; n:integer):REAL;
VAR
  s:real;
  i:word;
BEGIN
  s:=0;
  for i:=1 to n do
  Begin
    s:= s + q^*w^;
    inc(q);inc(w);
  End;
  Power:=s;
END;

VAR
  //a,b:Vector;
  n:word;
  q,w:Preal;
BEGIN
  clrscr;

  Writeln('Input n');
  readln(n);

  Inp(q,n);
  Inp(w,n);

  WriteLn();
  Writeln('(a,b)= ',Power(q,w,n):3:4);
  ReadKey;
END.
