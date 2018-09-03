#include <stdio.h>
#include <stdlib.h>
#include <fcntl.h>

//读一个文件 返回其在内存中的指针、文件长度
bool LoadFileContent(const char* path, char* content, int* filesize = nullptr)
{
	if (content == nullptr)
	{
		return 0;
	}
	if(filesize!=NULL)
	{
	    *filesize = 0;		
	}
	FILE* pFile = NULL;
	if(fopen_s(&pFile,path, "rb") == 0)
	{
	    //按二进制打开文件，只读
		fseek(pFile, 0, SEEK_END);//移动到文件尾
		int nLen = ftell(pFile);
		if (nLen > 0)
		{
			rewind(pFile);//移到文件头部
			fread(content, sizeof(unsigned char), nLen, pFile);
			content[nLen] = '\0';
	        if(filesize!=NULL)
	        {
	            *filesize = nLen;		
	        }
		}
		fclose(pFile);
		return 1;
	}
	return 0;
}

unsigned char* DecodeBMP(unsigned char* bmpFileData, int& width, int& height)
{
	//如果文件头是0X4D42 说明是BMP文件
	if (0x4D42 == *((unsigned short*)bmpFileData))
	{
		int pixelDataOffset = (*(int*)(bmpFileData + 10));//像素数据的偏移值
		width = (*(int*)(bmpFileData + 18));//图片宽
		height = (*(int*)(bmpFileData + 22));//图片高
		unsigned char* pixelData = bmpFileData + pixelDataOffset;//像素数据起始地址
		//由BGR->RGB
		for (int i = 0; i < width*height * 3; i += 3)
		{
			unsigned char temp = pixelData[i];//b
			pixelData[i] = pixelData[i + 2];//b=r
			pixelData[i + 2] = temp;//r=b
		}
		return pixelData;
	}
	return nullptr;
}

