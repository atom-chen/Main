#include "Utils.h"
#include "SOIL.h"
#include "Pixel.h"

unsigned char* LoadFileContent(const char* path, int& filesize)
{
	unsigned char* fileContent = nullptr;
	filesize = 0;
	FILE* pFile = fopen(path, "rb");
	//按二进制打开文件，只读
	if (pFile)
	{
		fseek(pFile, 0, SEEK_END);//移动到文件尾
		int nLen = ftell(pFile);
		if (nLen > 0)
		{
			rewind(pFile);//移到文件头部
			fileContent = new unsigned char[nLen + 1];
			fread(fileContent, sizeof(unsigned char), nLen, pFile);
			fileContent[nLen] = '\0';
			filesize = nLen;
		}
		fclose(pFile);
	}
	return fileContent;
}

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
		for (int i = 0; i < width*height * 3; i += 3)
		{
			unsigned char temp = pixelData[i];//b
			pixelData[i] = pixelData[i + 2];//b=r
			pixelData[i + 2] = temp;//r=b
		}
		return pixelData;
	}
	return nullptr;
}
//GL_CLAMP：大于1.0以上的纹理坐标，会取1.0位置的纹理颜色
//GL_REPEAT:大于1.0以上的纹理坐标，会取坐标-1.0位置的纹理颜色，循环往复
//GL_CLAMP_TO_EDGE：天空盒无缝相接

//GL_LINEAR：线性过滤
//另一个：取最近点
GLuint CreateTexture2D(unsigned char* pixelData, int width, int height, GLenum type)
{
	GLuint texture;//该纹理在OpenGL中的唯一标识符
	glGenTextures(1, &texture);//申请一个纹理
	glBindTexture(GL_TEXTURE_2D, texture);//绑定一个2D纹理到texture上
	
	//设置参数
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);//举例：当128*128的纹理映射到256*256的物体上时（纹理扩大），使用线性过滤
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);//举例：当128*128的纹理映射到64*64的物体上时（纹理缩小），使用线性过滤
	
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);//举例：当在uv为0.5的图形上输入1.0时，去纹理的边界上取(0.5的地方取)
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);//举例：当在uv为0.5的图形上输入1.0时，去纹理的边界上取(0.5的地方取)

	//拷贝数据到显卡（啥数据，MipMapLevel级别（用不同级别的像素数据为多边形着色），纹理数据在显卡上的像素格式，宽，高，boder必须写0，纹理数据在内存中的格式，每个像素数据的分量是什么类型，像素数据在内存哪里）
	glTexImage2D(GL_TEXTURE_2D, 0, type, width, height, 0, type, GL_UNSIGNED_BYTE, pixelData);
	
	glBindTexture(GL_TEXTURE_2D, 0);//把当前纹理设置为0号纹理

	return texture;	
}

GLuint CreateTexture2DFromBMP(const char* bmpPath)
{
	//读文件
	int fileLen = 0;
	unsigned char* pileContent = LoadFileContent(bmpPath, fileLen);
	if (pileContent == nullptr)
	{
		return 0;
	}
	//解码BMP
	int width = 0, height = 0;
	unsigned char* pixelData = DecodeBMP(pileContent, width, height);
	if (width == 0 || height == 0 || pixelData == nullptr)
	{
		delete pileContent;
		return 0;
	}
	GLuint texture = CreateTexture2D(pixelData, width, height, GL_RGB);
	if (pileContent != nullptr)
	{
		delete pileContent;
	}
	return texture;
}

GLuint CreateTexture2DFromPNG(const char* bmpPath, bool invertY)
{
	int nFileSize = 0;
	unsigned char* fileContent = LoadFileContent(bmpPath,nFileSize);
	if (fileContent == nullptr)
	{
		return 0;
	}
	unsigned int flags = SOIL_FLAG_POWER_OF_TWO;
	if (invertY)
	{
		flags |= SOIL_FLAG_INVERT_Y;
	}
	GLuint texture = SOIL_load_OGL_texture_from_memory(fileContent, nFileSize, 0, 0, flags);
	delete fileContent;
	return texture;
}

GLuint CreateProcedureTexture(const int& lenth,ALPHA_TYPE type)
{
	unsigned char  *imageData = new unsigned char[lenth*lenth*4];//rgba

	//计算中心点坐标
	float halfSize = (float)lenth / 2.0f;
	float maxDistance = sqrtf(halfSize*halfSize*2);             //最远点到中心点的距离
	float centerX = halfSize;
	float centerY = halfSize;

	for (int y = 0; y < lenth; y++)
	{
		for (int x = 0; x < lenth; x++)
		{
			//加入线性渐变：根据到中心点的距离
			float deltaX = (float)x - centerX;
			float deltaY = (float)y - centerY;
			float distance = sqrtf(deltaX*deltaX + deltaY*deltaY);                               //内积公式
			float alpha = INVALID;
			switch (type)
			{
			case ALPHA_LINNER:
				alpha = (1.0f - distance / maxDistance);
				break;
			case ALPHA_GAUSSIAN:
				alpha = powf(1.0f - (distance / maxDistance),10);
				break;
			default:
				alpha = 1;
				break;
			}
			alpha = alpha>1 ? 1 : alpha;


			//设置RGBA
			int index = (lenth*y + x) * 4;
			imageData[index + 0] = 150;
			imageData[index + 1] = 200;
			imageData[index + 2] = 128;
			imageData[index + 3] = static_cast<unsigned char>(alpha * 255);
		}
	}
	GLuint texture = CreateTexture2D(imageData, lenth, lenth, GL_RGBA);
	delete imageData;
	return texture;
}

GLuint CreateDisplayList(std::function<void()> foo)
{
	GLuint displayList = glGenLists(1);
	glNewList(displayList, GL_COMPILE);
	foo();
	glEndList();
	return displayList;
}


float GetFrameTime()
{
	static unsigned long lastTime = 0, timeSinceComputerStar=0;//上次渲染时间，
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
