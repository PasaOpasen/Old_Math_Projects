USES
 Crt;

TYPE
 //Vector = array [1..1000] of integer;
 Pinteger = ^integer;

PROCEDURE Inp(VAR q:Pinteger; n:integer);
VAR
  i:word;
  e:Pinteger;
BEGIN
  GetMem(q,(n)*sizeOf(integer));
  e:=q;
  WriteLn('Input ', n, ' coordinates of vector');
  FOR i:=1 to n DO
  BEGIN
    inc(e,i-1);
    Read(e^);
    e:=q;
  END;
END;

FUNCTION min1(q:Pinteger; n:integer):integer;
VAR
  min,kolm:integer;
  i:word;
  o:Pinteger;
BEGIN
  min:=q^;
  o:=q;
  inc(q);
  for i:=2 to n do
      BEGIN
      if q^<min THEN min:=q^;
      Inc(q);
      END;
  kolm:=0;
  FOR i:=1 to n do
     BEGIN
     If o^=min THEN kolm:=kolm+1;inc(o);
     END;
 min1:=kolm;
END;

FUNCTION minabs1(q:Pinteger; n:integer):integer;
VAR
  min,kolm:integer;
  i:word;
  o:Pinteger;
BEGIN
  min:=abs(q^);
  o:=q;
  Inc(q);
  for i:=2 to n do
    BEGIN
      if abs(q^)<min THEN min:=abs(q^);
      Inc(q);
    END;
  kolm:=0;
  FOR i:=1 to n do
    BEGIN
      If abs(o^)=min THEN kolm:=kolm+1;
      inc(o);
    END;
  minabs1:=kolm;
END;

FUNCTION min2(q:Pinteger; n:integer):integer;
VAR
  min,kolm:integer;
  i:word;
BEGIN
  min:=q^;kolm:=1;
  Inc(q);
  for i:=2 to n do
  BEGIN
    if q^=min THEN kolm:=kolm+1;
    if q^<min THEN
      BEGIN
      min:=q^;
      kolm:=1;
      END;
    Inc(q);
  END;
  min2:=kolm;
END;

FUNCTION minabs2(q:Pinteger; n:integer):integer;
VAR
  min,kolm:integer;
  i:word;
BEGIN
  min:=abs(q^); kolm:=1;
  Inc(q);
  for i:=2 to n do
  BEGIN
    if abs(q^)=min THEN kolm:=kolm+1;
    if abs(q^)<min THEN
      BEGIN
      min:=abs(q^);
      kolm:=1;
      END;
    Inc(q);
  END;
  minabs2:=kolm;
END;


VAR
  //a:Vector;
  n:word;
  q:Pinteger;
BEGIN
  clrscr;
  Writeln('Input (n>5)');
  ReadLn(n);

  Inp(q,n);

  WriteLn('число минимальных (2 прохода) ',min1(q,n));
  WriteLn('число минимальных (1 проход) ',min2(q,n));

  WriteLn('число минимальных по модулю (2 прохода) ',minabs1(q,n));
  WriteLn('число минимальных по модулю (1 проход) ',minabs2(q,n));

ReadKey;
END.
