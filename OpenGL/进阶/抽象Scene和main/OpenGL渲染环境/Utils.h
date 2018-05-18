#pragma once
#include "ggl.h"


//解码BMP文件，返回其文件起始地址，图片宽、高
unsigned char* DecodeBMP(unsigned char* bmpFileData, int& width, int& height);

//获取一帧所消耗的时间
float GetFrameTime();

