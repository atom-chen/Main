#include "texture.h"
#include "utils.h"

unsigned char*  DecodeBMP(unsigned char * bmpFileData,_Out_ int &width,_Out_ int &height)
{
	//�ж����ͣ������ͷ��0x4D42 ��ô������Ϊ���ļ���bitmap�ļ�
	if (0x4D42 == *((unsigned short*)bmpFileData))
	{
		int pixelDataOffset = *((int*)(bmpFileData + 10));        //�õ��������ݵ�ƫ�Ƶ�ַ
		width = *((int*)(bmpFileData + 18));                      //�õ�ͼƬ���
		height = *((int*)(bmpFileData + 22));                     //�õ�ͼƬ�߶�
		unsigned char* pixelData = bmpFileData + pixelDataOffset;   //Դ�ļ�+ƫ�Ƶ�ַ=rgb��ʼ�ĵ�ַ
		//bgr bgr bgr->rgb rgb rgb
		for (int i = 0; i < width*height * 3; i += 3)
		{
			//���� b��r ��λ��
			unsigned char temp = pixelData[i];
			pixelData[i] = pixelData[i + 2];
			pixelData[i + 2] = temp;
		}
		return pixelData;                     //����rgb rgb rgb rgb...
	}
	else{
		return nullptr;
	}
}
void Texture::Init(const char *imagePath)
{
	//��Ӳ�̰��ļ������ڴ�
	unsigned char* imageFileContent = LoadFileContent(imagePath);

	//����ͼƬ
	int width = 0, height = 0;
	unsigned char* pixelData = DecodeBMP(imageFileContent, width, height);
	//����һ��OpenGL����
	glGenTextures(1, &mTextureID);
	glBindTexture(GL_TEXTURE_2D, mTextureID);
	//�Ե�ǰ��������������
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP);
	
	glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, width, height, 0, GL_RGB, GL_UNSIGNED_BYTE, pixelData);             //�������� �ڴ�->�Դ�
	//��ʱ1����ָ���Դ��һ���ص� �������һ������
	glBindTexture(GL_TEXTURE_2D, 0);               //������Ҫ��id״̬��Ϊ0
	delete imageFileContent;
}