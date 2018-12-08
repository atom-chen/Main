#include "Utils.h"
#include "SOIL.h"



unsigned char* DecodeBMP(unsigned char* bmpFileData, int& width, int& height)
{
	//����ļ�ͷ��0X4D42 ˵����BMP�ļ�
	if (0x4D42 == *((unsigned short*)bmpFileData))
	{
		int pixelDataOffset = (*(int*)(bmpFileData + 10));//�������ݵ�ƫ��ֵ
		width = (*(int*)(bmpFileData + 18));//ͼƬ��
		height = (*(int*)(bmpFileData + 22));//ͼƬ��
		unsigned char* pixelData = bmpFileData + pixelDataOffset;//����������ʼ��ַ
		//��BGR->RGB
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


//��һ���ļ� ���������ڴ��е�ָ�롢�ļ�����
bool LoadFileContent(const char* path, int& filesize, char* content)
{
	if (content == nullptr)
	{
		return 0;
	}
	filesize = 0;
	FILE* pFile = fopen(path, "rb");
	//�������ƴ��ļ���ֻ��
	if (pFile)
	{
		fseek(pFile, 0, SEEK_END);//�ƶ����ļ�β
		int nLen = ftell(pFile);
		if (nLen > 0)
		{
			rewind(pFile);//�Ƶ��ļ�ͷ��
			fread(content, sizeof(unsigned char), nLen, pFile);
			content[nLen] = '\0';
			filesize = nLen;
		}
		fclose(pFile);
		return 1;
	}
	return 0;
}

void Debug(const char* fmt, ...)
{
	va_list ap;
	const char *p, *sval;
	int ival;
	double dval;

	va_start(ap, fmt);// ap point head

	for (p = fmt; (*p); p++)
	{
		if (*p != '%')
		{
			putchar(*p);
			continue;
		}
		switch (*++p)
		{
		case 'd':
			ival = va_arg(ap, int);
			printf("%d", ival);
			break;
		case 's':
			for (sval = va_arg(ap, char*); *sval; sval++)
			{
				putchar(*sval);
			}
			break;
		case 'f':
			dval = va_arg(ap, double);
			printf("%f", dval);
			break;
		default:
			break;
		}
	}
	va_end(ap);
}