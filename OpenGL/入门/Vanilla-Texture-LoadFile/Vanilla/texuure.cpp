#include "texture.h"
#include "utils.h"

void Texture::Init(const char *imagePath)
{
	//从硬盘把文件读到内存
	unsigned char* imageFileContent = LoadFileContent(imagePath);

	//解码图片
	//generate an opengl texture
}