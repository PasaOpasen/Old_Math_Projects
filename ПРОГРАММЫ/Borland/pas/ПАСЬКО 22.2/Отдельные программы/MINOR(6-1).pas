USES
  crt;

TYPE
  //Matrix = array [1..1000] of INTEGER;
  Pinteger = ^integer;


PROCEDURE Inp(VAR q:Pinteger; n,m:integer);
VAR
  i:word;
  e:Pinteger;
BEGIN
  GetMem(q,(n*m)*sizeOf(integer));
  e:=q;
  WriteLn('Input Matrix ',n,'x',m);
  FOR i:=1 to n*m DO
  BEGIN
    inc(e,i-1);
    Read(e^);
    e:=q;
  END;
END;



PROCEDURE St(q:Pinteger;f,n,m:word);
VAR
  i:word;
  e:Pinteger;
BEGIN
  e:=q;
  inc(e,m*(f-1));
  IF (f<1) OR (f>n) THEN WriteLn('Error!')
  ELSE
  FOR i:=1 TO m DO
  BEGIN
    Write(e^, ' ');
    inc(e);
  END;
  WriteLn();
END;

PROCEDURE Sc(q:Pinteger;f,n,m:word);
VAR
  i:word;
  e:Pinteger;
BEGIN
  e:=q;
  inc(e,f-1);
  IF (f<1) OR (f>m) THEN WriteLn('Error!')
  ELSE
  FOR i:=1 TO n DO
  BEGIN
    WriteLn('  ',e^);
    inc(e,m);
  END;
  WriteLn();
END;

PROCEDURE Minor(q:Pinteger;n1,m1,n,m:word);
VAR
  i,j,k:word;
  e:Pinteger;
BEGIN
  e:=q;
  k:=0;
  IF ((n1<1) OR (n1>n)) OR ((m1<1) OR (m1>m)) THEN WriteLn('Error!')
  ELSE
    FOR i:=1 TO n DO
    BEGIN
      IF i {div m} <> n1{-1} THEN
        FOR j:=1 TO m DO
          IF j {mod n} <> m1 THEN
          BEGIN
            inc(e,-1+(i-1)*m +j);
            inc(k);
            Write(e^, ' ');
            e:=q;
            IF k mod (m-1) = 0 THEN WriteLn();
          END;
    END;
END;

VAR
  //A:Matrix;
  f,n1,m1,m,n:word;
  q: Pinteger;
BEGIN
  ClrScr;

  Writeln('Input n,m');
  Read(n,m);
  Inp(q,n,m);

  Writeln('Input number of string (1<=number<=',n,',)');
  Read(f);

  St(q,f,n,m);

  Writeln('Input number of column (1<=number<=',m,')');
  Read(f);
  Sc(q,f,n,m);

  Writeln('Input numbers of string and column (1<=string<=',n,';1<=column<=',m,')');
  Read(n1,m1);
  Minor(q,n1,m1,n,m);

  ReadKey;
END.
