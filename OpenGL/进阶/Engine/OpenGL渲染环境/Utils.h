#pragma once
#include "ggl.h"


//解码BMP文件，返回其文件起始地址，图片宽、高
unsigned char* DecodeBMP(unsigned char* bmpFileData, int& width, int& height);


//读一个文件 返回其在内存中的指针、文件长度
bool LoadFileContent(const char* path, int& filesize,char* content);


void Debug(const char* fmt, ...);