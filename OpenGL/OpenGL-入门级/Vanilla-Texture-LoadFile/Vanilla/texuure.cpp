#include "texture.h"
#include "utils.h"

void Texture::Init(const char *imagePath)
{
	//��Ӳ�̰��ļ������ڴ�
	unsigned char* imageFileContent = LoadFileContent(imagePath);

	//����ͼƬ
	//generate an opengl texture
}