import std.stdio;
import std.math;

void main()
{
    //writeln("Размер в байтах: ", real.sizeof);
    
    real ar = sqrt(2.0);
    writefln("  real = \t%.63f", ar);
    writefln("square: \t%.63f", ar * ar);

    ar+=sin(ar);
   writefln("%.20f",ar);
}
