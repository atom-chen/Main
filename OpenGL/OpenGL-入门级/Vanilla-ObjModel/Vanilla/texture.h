#pragma once
#include <windows.h>
#include <gl/GL.h>
#include <gl/GLU.h>

unsigned char*  DecodeBMP(unsigned char * bmpFileData, int &width, int &height);
class Texture{
public:
	void Init(const char *imagePath);
	GLuint mTextureID;     //��GPU���� �����������GLuint���͵ģ��൱������ID��

};