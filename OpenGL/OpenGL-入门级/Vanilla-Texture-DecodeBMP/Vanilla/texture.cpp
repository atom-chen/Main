#include "texture.h"
#include "utils.h"

unsigned char*  DecodeBMP(unsigned char * bmpFileData,_Out_ int &width,_Out_ int &height)
{
	//判断类型，如果开头是0x4D42 那么我们认为该文件是bitmap文件
	if (0x4D42 == *((unsigned short*)bmpFileData))
	{
		int pixelDataOffset = *((int*)(bmpFileData + 10));        //拿到像素数据的偏移地址
		width = *((int*)(bmpFileData + 18));                      //拿到图片宽度
		height = *((int*)(bmpFileData + 22));                     //拿到图片高度
		unsigned char* pixelData = bmpFileData + pixelDataOffset;   //源文件+偏移地址=rgb开始的地址
		//bgr bgr bgr->rgb rgb rgb
		for (int i = 0; i < width*height * 3; i += 3)
		{
			//交换 b和r 的位置
			unsigned char temp = pixelData[i];
			pixelData[i] = pixelData[i + 2];
			pixelData[i + 2] = temp;
		}
		return pixelData;                     //返回rgb rgb rgb rgb...
	}
	else{
		return nullptr;
	}
}
void Texture::Init(const char *imagePath)
{
	//从硬盘把文件读到内存
	unsigned char* imageFileContent = LoadFileContent(imagePath);

	//解码图片
	int width = 0, height = 0;
	unsigned char* pixelData = DecodeBMP(imageFileContent, width, height);
	//生成一张OpenGL纹理
	glGenTextures(1, &mTextureID);
	glBindTexture(GL_TEXTURE_2D, mTextureID);
	//对当前纹理进行纹理过滤
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP);
	
	glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, width, height, 0, GL_RGB, GL_UNSIGNED_BYTE, pixelData);             //生成纹理 内存->显存
	//此时1可以指向显存的一个地点 那里摆着一张纹理
	glBindTexture(GL_TEXTURE_2D, 0);               //用完了要将id状态置为0
	delete imageFileContent;
}