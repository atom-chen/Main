#pragma once
#include <windows.h>
#include <gl/GL.h>
#include <gl/GLU.h>

unsigned char*  DecodeBMP(unsigned char * bmpFileData, int &width, int &height);
class Texture{
public:
	void Init(const char *imagePath);
	GLuint mTextureID;     //在GPU看来 纹理的名字是GLuint类型的（相当于纹理ID）

};