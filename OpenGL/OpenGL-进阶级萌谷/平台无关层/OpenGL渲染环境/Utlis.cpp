#include "Utils.h"
#include "SOIL.h"



unsigned char* DecodeBMP(unsigned char* bmpFileData, int& width, int& height)
{
	//如果文件头是0X4D42 说明是BMP文件
	if (0x4D42 == *((unsigned short*)bmpFileData))
	{
		int pixelDataOffset = (*(int*)(bmpFileData + 10));//像素数据的偏移值
		width = (*(int*)(bmpFileData + 18));//图片宽
		height = (*(int*)(bmpFileData + 22));//图片高
		unsigned char* pixelData = bmpFileData + pixelDataOffset;//像素数据起始地址
		//由BGR->RGB
		for (int32_t i = 0; i < width*height * 3; i += 3)
		{
			unsigned char temp = pixelData[i];//b
			pixelData[i] = pixelData[i + 2];//b=r
			pixelData[i + 2] = temp;//r=b
		}
		return pixelData;
	}
	return nullptr;
}


float GetFrameTime()
{
	static unsigned long lastTime = 0, timeSinceComputerStar = 0;//上次渲染时间，
	timeSinceComputerStar = timeGetTime();//当前时间
	unsigned long frameTime = lastTime == 0 ? 0 : timeSinceComputerStar - lastTime;
	//如果是第一次启动
	if (lastTime != 0)
	{
		frameTime = timeSinceComputerStar - lastTime;
	}
	lastTime = timeSinceComputerStar;
	return float(frameTime) / 1000.0f;
}