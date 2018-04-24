#pragma once
#include "ggl.h"

//读一个文件 返回其在内存中的指针、文件长度
unsigned char* LoadFileContent(const char* path, int& filesize);

//解码BMP文件，返回其文件起始地址，图片宽、高
unsigned char* DecodeBMP(unsigned char* bmpFileData, int& width, int& height);

//创建2D纹理对象
GLuint CreateTexture2D(unsigned char* pixelData, int width, int height, GLenum type);