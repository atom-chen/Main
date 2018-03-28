#include "Utils.h"
#include "SOIL.h"
#include "Pixel.h"

unsigned char* LoadFileContent(const char* path, int& filesize)
{
	unsigned char* fileContent = nullptr;
	filesize = 0;
	FILE* pFile = fopen(path, "rb");
	//�������ƴ��ļ���ֻ��
	if (pFile)
	{
		fseek(pFile, 0, SEEK_END);//�ƶ����ļ�β
		int nLen = ftell(pFile);
		if (nLen > 0)
		{
			rewind(pFile);//�Ƶ��ļ�ͷ��
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
	//����ļ�ͷ��0X4D42 ˵����BMP�ļ�
	if (0x4D42 == *((unsigned short*)bmpFileData))
	{
		int pixelDataOffset = (*(int*)(bmpFileData + 10));//�������ݵ�ƫ��ֵ
		width = (*(int*)(bmpFileData + 18));//ͼƬ��
		height = (*(int*)(bmpFileData + 22));//ͼƬ��
		unsigned char* pixelData = bmpFileData + pixelDataOffset;//����������ʼ��ַ
		//��BGR->RGB
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
//GL_CLAMP������1.0���ϵ��������꣬��ȡ1.0λ�õ�������ɫ
//GL_REPEAT:����1.0���ϵ��������꣬��ȡ����-1.0λ�õ�������ɫ��ѭ������
//GL_CLAMP_TO_EDGE����պ��޷����

//GL_LINEAR�����Թ���
//��һ����ȡ�����
GLuint CreateTexture2D(unsigned char* pixelData, int width, int height, GLenum type)
{
	GLuint texture;//��������OpenGL�е�Ψһ��ʶ��
	glGenTextures(1, &texture);//����һ������
	glBindTexture(GL_TEXTURE_2D, texture);//��һ��2D����texture��
	
	//���ò���
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);//��������128*128������ӳ�䵽256*256��������ʱ���������󣩣�ʹ�����Թ���
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);//��������128*128������ӳ�䵽64*64��������ʱ��������С����ʹ�����Թ���
	
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);//����������uvΪ0.5��ͼ��������1.0ʱ��ȥ����ı߽���ȡ(0.5�ĵط�ȡ)
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);//����������uvΪ0.5��ͼ��������1.0ʱ��ȥ����ı߽���ȡ(0.5�ĵط�ȡ)

	//�������ݵ��Կ���ɶ���ݣ�MipMapLevel�����ò�ͬ�������������Ϊ�������ɫ���������������Կ��ϵ����ظ�ʽ�����ߣ�boder����д0�������������ڴ��еĸ�ʽ��ÿ���������ݵķ�����ʲô���ͣ������������ڴ����
	glTexImage2D(GL_TEXTURE_2D, 0, type, width, height, 0, type, GL_UNSIGNED_BYTE, pixelData);
	
	glBindTexture(GL_TEXTURE_2D, 0);//�ѵ�ǰ��������Ϊ0������

	return texture;	
}

GLuint CreateTexture2DFromBMP(const char* bmpPath)
{
	//���ļ�
	int32_t fileLen = 0;
	unsigned char* pileContent = LoadFileContent(bmpPath, fileLen);
	if (pileContent == nullptr)
	{
		return 0;
	}
	//����BMP
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
	int32_t nFileSize = 0;
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

GLuint CreateProcedureTexture(const int32_t& lenth,ALPHA_TYPE type)
{
	unsigned char  *imageData = new unsigned char[lenth*lenth*4];//rgba

	//�������ĵ�����
	float halfSize = (float)lenth / 2.0f;
	float maxDistance = sqrtf(halfSize*halfSize*2);             //��Զ�㵽���ĵ�ľ���
	float centerX = halfSize;
	float centerY = halfSize;

	for (int32_t y = 0; y < lenth; y++)
	{
		for (int32_t x = 0; x < lenth; x++)
		{
			//�������Խ��䣺���ݵ����ĵ�ľ���
			float deltaX = (float)x - centerX;
			float deltaY = (float)y - centerY;
			float distance = sqrtf(deltaX*deltaX + deltaY*deltaY);                               //�ڻ���ʽ
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


			//����RGBA
			int32_t index = (lenth*y + x) * 4;
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
	static unsigned long lastTime = 0, timeSinceComputerStar=0;//�ϴ���Ⱦʱ�䣬
	timeSinceComputerStar = timeGetTime();//��ǰʱ��
	unsigned long frameTime = lastTime == 0 ? 0 : timeSinceComputerStar - lastTime;
	//����ǵ�һ������
	if (lastTime != 0)
	{
		frameTime = timeSinceComputerStar - lastTime;
	}
	lastTime = timeSinceComputerStar;
	return float(frameTime) / 1000.0f;
}
