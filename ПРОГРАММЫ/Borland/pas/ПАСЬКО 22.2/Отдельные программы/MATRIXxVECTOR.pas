USES
  crt;

TYPE
 //Vector = array [1..1000] of real;
 Preal = ^real;


PROCEDURE InpV(VAR q:Preal; n:integer);
VAR
  i:word;
  e:Preal;
BEGIN
  GetMem(q,(n)*sizeOf(real));
  e:=q;
  WriteLn('Input vector');
  FOR i:=1 to n DO
  BEGIN
    inc(e,i-1);
    Read(e^);
    e:=q;
  END;
END;

PROCEDURE InpM(VAR q:Preal; n:integer);
VAR
  i:word;
  e:Preal;
BEGIN
  GetMem(q,(n)*sizeOf(real));
  e:=q;
  WriteLn('Input matrix');
  FOR i:=1 to n DO
  BEGIN
    inc(e,i-1);
    Read(e^);
    e:=q;
  END;
END;


PROCEDURE PowerM(VAR q,w:Preal; m,n:integer);
VAR
i,j:word;
pv:Preal;
a:real;
BEGIN
  for i:=1 to m do
  Begin
    pv:=q;
    a:=0;
    for j:=1 to n do
    Begin
      a:=a +w^*pv^;
      inc(w);
      inc(pv);
    end;
    Writeln(a:3:4);
  End;
END;

VAR
  //a,b:Vector;
  n,m:word;
  qa,wb:Preal;
BEGIN
  clrscr;
  Writeln('Input m, n');
  readln(m,n);

  InpV(qa,n);
  InpM(wb,m*n);

  WriteLn();
  PowerM(qa,wb,m,n);

  ReadKey;
END.

