FUNCTION zel(x:REAL):WORD;
VAR
i:WORD;
BEGIN
i := 0;
WHILE(i<x) DO i := i+1;
zel := i-1;
END;

BEGIN
write(zel(10));
REadln;
END.