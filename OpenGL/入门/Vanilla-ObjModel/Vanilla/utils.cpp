#include "utils.h"


unsigned char* LoadFileContent(const char *filePath)
{
	unsigned char* fileContent = nullptr;
	FILE* pFile = fopen(filePath, "rb");
	if (pFile)
	{
		//read
		fseek(pFile, 0, SEEK_END);        //移到文件末尾
		unsigned nLen = ftell(pFile);     //拿到文件长度  ftell()函数返回stream(流)当前的文件位置,如果发生错误返回-1. 


		//如果当前文件不为空（长度大于0） 则去读它
		if (nLen > 0)
		{
			rewind(pFile);                 //把pf移到流开始处
			fileContent = new unsigned char[nLen + 1];
			fileContent[nLen] = '\0';
			fread(fileContent, sizeof(unsigned char), nLen, pFile);             //读取文件
		}
		fclose(pFile);
	}
	return fileContent;
}