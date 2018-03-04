#include "Scene.h"
#include "Utils.h"
GLuint texture;

bool Init()
{
	glMatrixMode(GL_PROJECTION);
	//gluPerspective(50.0f, 800.0f / 600.0f, 0.1f, 1000.0f);
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
	
	int nFileSize = 0;
	unsigned char *bmpFileContent = LoadFileContent("res/test.bmp", nFileSize);//把文件读到内存
	if (bmpFileContent == nullptr)
	{
		return false;
	}
	int bmpWidth = 0, bmpHeight = 0;
	unsigned char* pixelData = DecodeBMP(bmpFileContent, bmpWidth, bmpHeight);//解析BMP文件
	if (pixelData == nullptr)
	{
		return false;
	}
	texture = CreateTexture2D(pixelData, bmpWidth, bmpHeight, GL_RGB);//把这个像素数据读到内存
	return true;
}
void Draw()
{
	glClearColor(0, 0, 1, 1.0f);     //设置用什么颜色擦缓冲区
	glClear(GL_COLOR_BUFFER_BIT);
	glEnable(GL_TEXTURE_2D);
	glBindTexture(GL_TEXTURE_2D, texture);//设置为当前纹理
	
	
	DrawModel();
}
void DrawModel()
{

	glBegin(GL_QUADS);
	glColor4ub(255, 255, 255, 255);
	//逆时针方向设置点的位置
	//左下
	glTexCoord2f(0.0f, 0.0f);//纹理坐标(uv)
	glVertex3f(-0.1f, -0.1f, -0.4f);//第一个点的坐标

	//右下
	//glTexCoord2f(1.0f, 0.0f);
	glTexCoord2f(2.0f, 0.0f);
	glVertex3f(0.1f, -0.1f, -0.4f);

	//右上
	//glTexCoord2f(1.0f, 1.0f);
	glTexCoord2f(2.0f, 2.0f);
	glVertex3f(0.1f, 0.1f, -0.4f);

	//左上
	//glTexCoord2f(0, 1.0f);
	glTexCoord2f(0, 2.0f);
	glVertex3f(-0.1f, 0.1f, -0.4f);
	glEnd();
}
