#include "Utils.h"

unsigned char* LoadFileContent(const char* path, int& filesize)
{
	unsigned char* fileContent = nullptr;
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
			fileContent = new unsigned char[nLen + 1];
			fread(fileContent, sizeof(unsigned char), nLen, pFile);
			fileContent[nLen] = '\0';
			filesize = nLen;
		}
		fclose(pFile);
	}
	return fileContent;
}

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
		for (int32_t i = 0; i < width*height * 3; i += 3)
		{
			unsigned char temp = pixelData[i];//b
			pixelData[i] = pixelData[i + 2];//b=r
			pixelData[i + 2] = temp;//r=b
		}
		return pixelData;
	}
	return nullptr;
}
//GL_CLAMP������1.0���ϵ��������꣬��ȡ1.0λ�õ�������ɫ
GLuint CreateTexture2D(unsigned char* pixelData, int width, int height, GLenum type)
{
	GLuint texture;//��������OpenGL�е�Ψһ��ʶ��
	glGenTextures(1, &texture);//����һ������
	glBindTexture(GL_TEXTURE_2D, texture);//��һ��2D����texture��
	
	//���ò���
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);//��������128*128������ӳ�䵽256*256��������ʱ���������󣩣�ʹ�����Թ���
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);//��������128*128������ӳ�䵽64*64��������ʱ��������С����ʹ�����Թ���
	
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP);//����������uvΪ0.5��ͼ��������1.0ʱ��ȥ����ı߽���ȡ(0.5�ĵط�ȡ)
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP);//����������uvΪ0.5��ͼ��������1.0ʱ��ȥ����ı߽���ȡ(0.5�ĵط�ȡ)

	//�������ݵ��Կ���ɶ���ݣ�MipMapLevel�����ò�ͬ�������������Ϊ�������ɫ���������������Կ��ϵ����ظ�ʽ�����ߣ�boder����д0�������������ڴ��еĸ�ʽ��ÿ���������ݵķ�����ʲô���ͣ������������ڴ����
	glTexImage2D(GL_TEXTURE_2D, 0, type, width, height, 0, type, GL_UNSIGNED_BYTE, pixelData);
	
	glBindTexture(GL_TEXTURE_2D, 0);//�ѵ�ǰ��������Ϊ0������

	return texture;	
}