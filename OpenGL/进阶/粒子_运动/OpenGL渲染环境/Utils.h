#pragma once
#include "ggl.h"

enum  ALPHA_TYPE
{
	ALPHA_INVALID = -1,
	ALPHA_LINNER = 0,
	ALPHA_GAUSSIAN = 1,
};

//读一个文件 返回其在内存中的指针、文件长度
unsigned char* LoadFileContent(const char* path, int& filesize);

//解码BMP文件，返回其文件起始地址，图片宽、高
unsigned char* DecodeBMP(unsigned char* bmpFileData, int& width, int& height);

//创建2D纹理对象
GLuint CreateTexture2D(unsigned char* pixelData, int width, int height, GLenum type);

//从BMP创建2D纹理对象
GLuint CreateTexture2DFromBMP(const char* bmpPath);

//从PNG创建纹理
GLuint CreateTexture2DFromPNG(const char* bmpPath, bool invertY = 1);

//生成一个程序纹理
GLuint CreateProcedureTexture(const int32_t& size, ALPHA_TYPE type=ALPHA_TYPE::ALPHA_LINNER);

//创建一个显示列表
GLuint CreateDisplayList(std::function<void()> foo);

//获取一帧所消耗的时间
float GetFrameTime();

//创建GPU BufferObject
GLuint CreateBufferObject(GLenum bufferType, const GLsizeiptr& size, const GLenum& usage, void* data = nullptr);