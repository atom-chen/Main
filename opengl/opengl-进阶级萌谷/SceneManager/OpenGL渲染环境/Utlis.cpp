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
