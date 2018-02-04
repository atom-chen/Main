#include "utils.h"


unsigned char* LoadFileContent(const char *filePath)
{
	unsigned char* fileContent = nullptr;
	FILE* pFile = fopen(filePath, "rb");
	if (pFile)
	{
		//read
		fseek(pFile, 0, SEEK_END);        //�Ƶ��ļ�ĩβ
		unsigned nLen = ftell(pFile);     //�õ��ļ�����  ftell()��������stream(��)��ǰ���ļ�λ��,����������󷵻�-1. 


		//�����ǰ�ļ���Ϊ�գ����ȴ���0�� ��ȥ����
		if (nLen > 0)
		{
			rewind(pFile);                 //��pf�Ƶ�����ʼ��
			fileContent = new unsigned char[nLen + 1];
			fileContent[nLen] = '\0';
			fread(fileContent, sizeof(unsigned char), nLen, pFile);             //��ȡ�ļ�
		}
		fclose(pFile);
	}
	return fileContent;
}