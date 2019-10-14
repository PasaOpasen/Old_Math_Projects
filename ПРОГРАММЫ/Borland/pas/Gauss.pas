uses
  crt;

TYPE
  Matrix = ARRAY [1..20] OF REAL;

PROCEDURE Inp(A:MAtrix;n:WORD);
VAR
  i:WORD;
BEGIN
  FOR i:=1 TO n DO Read(A[i]);
END;

PROCEDURE Gauss(A,b:Matrix;n:WORD{;e:REAL});
VAR
  A1: array[1..400] of real;
  b1,x: array[1..20] of real;
  i, j, k: integer;
  z, r, g: real;
BEGIN
  for i := 1 to n*n do
    A1[i]:=A[i];

  for k := 1 to n do {прямой ход}
  begin
    for j := k + 1 to n do
    begin
      r := a[(j-1)*n+k] / a[(k-1)*n+k];
      for i := k to n do
      begin
        a[(j-1)*n+i] := a[(j-1)*n+i] - r * a[(k-1)*n+i];
      end;
      b[j] := b[j] - r * b[k];
    end;
  end;
  for k := n downto 1 do {обратный ход}
  begin
    r := 0;
    for j := k + 1 to n do
    begin
      g := a[(k-1)*n+j] * x[j];
      r := r + g;
    end;
    x[k] := (b[k] - r) / a[(k-1)*n+k];
  end;
  writeln('корни');
  for i := 1 to n do
    write('x[', i, ']=', x[i]:0:2, '   ');
  writeln;
  writeln('проверка соответствия');

  for i:=1 to n do
   begin
    for j:=1 to n do
      b1[i]:=b1[i]+a1[(i-1)*n+j]*x[j];
     writeln(b1[i]:3:2,' ');
    end;
  ReadKey;
END;

VAR
  A,b:Matrix;
  n:WORD;
  //eps:REAL;
BEGIN
  ClrScr;
  WriteLn('Input n');
  ReadLn(n);

  Inp(A,n*n);Inp(b,n);

 { WriteLn('Input eps');
  ReadLn(eps); }
  Gauss(A,b,n{,eps});

END.
