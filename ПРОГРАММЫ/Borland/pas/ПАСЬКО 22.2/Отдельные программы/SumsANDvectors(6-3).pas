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

PROCEDURE Powers(q:Pinteger;n,m:word);
VAR
  i,j,pow:word;
  e:Pinteger;
BEGIN
  e:=q;
  pow:=0;

  WriteLn();
  WriteLn('Sum of colums ');
  FOR j:= 1 TO m DO
  BEGIN
    FOR i:=1  TO n DO
    BEGIN
      inc(e,(i-1)*m+j-1);
      pow:=pow+e^;
      e:=q;
      //inc(e);
    END;
     Write(pow,' ');
     pow:=0;
  END;

  WriteLn();
  WriteLn('Sum of strings ');
  FOR i:=1  TO n DO
  BEGIN
    FOR j:= 1 TO m DO
    BEGIN
      inc(e,(i-1)*m+j-1);
      pow:=pow+e^;
      e:=q;
      //inc(e);
    END;
     WriteLn(pow,' ');
     pow:=0;
   END;
  WriteLn();
END;

VAR
 // A:Matrix;
  f,m,n:word;
  q: Pinteger;
BEGIN
  ClrScr;

  Writeln('Input n,m');
  Read(n,m);
  //Inp(A,n,m);
  //q:=@A[1];

  Inp(q,n,m);

  Powers(q,n,m);

  Writeln('Input number of string (1<=number<=',n,',)');
  Read(f);

  St(q,f,n,m);

  Writeln('Input number of column (1<=number<=',m,')');
  Read(f);
  Sc(q,f,n,m);

  ReadKey;
END.
