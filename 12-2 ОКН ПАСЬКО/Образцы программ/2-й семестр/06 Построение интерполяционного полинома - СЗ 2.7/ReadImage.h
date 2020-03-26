#include<stdio.h>
#include<windows.h>

static BITMAPFILEHEADER bmpfh;
static BITMAPINFOHEADER bmpih;

//--------------------------------------------------------------------------------
void GetSize(char* FileName,int* h, int* w)
{
	FILE *file = fopen(FileName,"rb");
	fread(&bmpfh,sizeof(BITMAPFILEHEADER),1,file); 
	fread(&bmpih,sizeof(BITMAPINFOHEADER),1,file);
	*h = bmpih.biHeight;
	*w = bmpih.biWidth;
	fclose(file);
}
//----------------------------------------------------------------------------------
void ReadImageFromFile(char* FileName, char* B, char* G, char* R)
{
	FILE *file = fopen(FileName,"rb");

	fread(&bmpfh,sizeof(BITMAPFILEHEADER),1,file); 
	/*printf("bmpfh.bfOffBits = %d\n",bmpfh.bfOffBits);
	printf("bmpfh.bfReserved1 =  %d\n",bmpfh.bfReserved1);
	printf("bmpfh.bfReserved2 =  %d\n",bmpfh.bfReserved2);
	printf("bmpfh.bfSize =  %d\n",bmpfh.bfSize);
	printf("bmpfh.bfType = %d\n",bmpfh.bfType);*/
	fread(&bmpih,sizeof(BITMAPINFOHEADER),1,file);
	/*printf("bmpih.biBitCount = %d\n",bmpih.biBitCount);	
	printf("bmpih.biHeight = %d\n",bmpih.biHeight);	
	printf("bmpih.biWidth = %d\n",bmpih.biWidth);	*/


	//printf("%d\n",ftell(file));
	fseek(file,bmpfh.bfOffBits,SEEK_SET);
	//printf("%d\n",ftell(file));

	for(int i = 0; i<bmpih.biHeight; i++)
	{
		for(int j = 0;j<bmpih.biWidth; j++)
		{	 
			fread(B+i*bmpih.biWidth+j,1,1,file);
			fread(G+i*bmpih.biWidth+j,1,1,file);
			fread(R+i*bmpih.biWidth+j,1,1,file);
		}
		for(int k = 3*bmpih.biWidth%4; k!=0; k = (k+1)%4)
		{
			char ctemp = 0; 
			fread(&ctemp,1,1,file);
		}
	}
	fclose(file);
}
//----------------------------------------------------------------------------
void WriteImage(char* FileName,  char* B, char* G, char* R)
{
	FILE *file = fopen(FileName,"wb");

	fwrite(&bmpfh,sizeof(BITMAPFILEHEADER),1,file);
	fwrite(&bmpih,sizeof(BITMAPINFOHEADER),1,file);

	for(int i = 0; i<bmpih.biHeight; i++)
	{
		for(int j = 0;j<bmpih.biWidth; j++)
		{	 
			fwrite(B+i*bmpih.biWidth+j,1,1,file);
			fwrite(G+i*bmpih.biWidth+j,1,1,file);
			fwrite(R+i*bmpih.biWidth+j,1,1,file);
		}
		for(int k = 3*bmpih.biWidth%4; k!=0; k = (k+1)%4)
		{
			char ctemp = 0; 
			fwrite(&ctemp,1,1,file);
		}
	}
	if(ftell(file)<bmpfh.bfSize)
	{
		printf("%d\n",bmpfh.bfSize - ftell(file));
		char ctemp = 0;
		for(int counter = bmpfh.bfSize - ftell(file); counter>0; counter--) fwrite(&ctemp,1,1,file);
	}

	fclose(file);
}
//----------------------------------------------------------------------

