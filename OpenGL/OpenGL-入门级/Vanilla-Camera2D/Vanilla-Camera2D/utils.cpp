#define  _CRT_SECURE_NO_WARNINGS
#include "utils.h"


unsigned char* LoadFileContent(const char*filePath)
{
	unsigned char* fileContent = nullptr;
	FILE*pFile = fopen(filePath, "rb");
	if (pFile)
	{
		//read
		fseek(pFile,0,SEEK_END);
		int nLen = ftell(pFile);
		if (nLen>0)
		{
			rewind(pFile);
			fileContent = new unsigned char[nLen+1];
			fread(fileContent,sizeof(unsigned char),nLen,pFile); 
			fileContent[nLen]='\0';
		}
		fclose(pFile);
	}
	return fileContent;
}