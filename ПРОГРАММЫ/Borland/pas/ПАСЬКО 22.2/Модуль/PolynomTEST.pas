USES
 Polynomend,Crt;


VAR
 a,b:Polynom;
 n:WORD;
BEGIN
 ClrScr;

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