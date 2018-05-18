#include "texture.h"
#include "utils.h"

unsigned char* DecodeBMP(unsigned char*bmpFileData, int&width, int&height)
{
	if (0x4D42==*((unsigned short*)bmpFileData))
	{
		int pixelDataOffset = *((int*)(bmpFileData + 10));
		width = *((int*)(bmpFileData+18));
		height = *((int*)(bmpFileData + 22));
		unsigned char*pixelData = bmpFileData + pixelDataOffset;
		//bgr bgr bgr ....
		//rgb rgb rgb
		for (int i=0;i<width*height*3;i+=3)
		{
			unsigned char temp = pixelData[i];
			pixelData[i] = pixelData[i+2];
			pixelData[i+2]=temp;
		}
		return pixelData;
	}
	else
	{
		return nullptr;
	}
}

void Texture::Init(const char*imagePath)
{
	//load image file from disk to memory
	unsigned char*imageFileContent = LoadFileContent(imagePath);
	//decode image
	int width = 0, height = 0;
	unsigned char* pixelData = DecodeBMP(imageFileContent, width, height);
	//generate an opengl texture
	glGenTextures(1, &mTextureID);
	glBindTexture(GL_TEXTURE_2D, mTextureID);
	//operation on current texture
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
	glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, width, height, 0, GL_RGB, GL_UNSIGNED_BYTE, pixelData);
	glBindTexture(GL_TEXTURE_2D, 0);
	delete imageFileContent;
}