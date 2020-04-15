#include <iostream>
#include <stdio.h>
#include <stdlib.h>

void main() 
{
	int x, y; 


	FILE *e = fopen("ini.txt", "r"); 
	fscanf(e, "%d", &x); 
	fclose(e); 

	y = x % 12; 


	FILE *rer = fopen("vtoroy.txt", "w"); 
	if (y == 0) fprintf(rer, "ƒмитрий ѕасько");
	else  fprintf(rer, "89615193646");

	fclose(rer); 

}