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
	unsigned char *bmpFileContent = LoadFileContent("res/test.bmp", nFileSize);//���ļ������ڴ�
	if (bmpFileContent == nullptr)
	{
		return false;
	}
	int bmpWidth = 0, bmpHeight = 0;
	unsigned char* pixelData = DecodeBMP(bmpFileContent, bmpWidth, bmpHeight);//����BMP�ļ�
	if (pixelData == nullptr)
	{
		return false;
	}
	texture = CreateTexture2D(pixelData, bmpWidth, bmpHeight, GL_RGB);//������������ݶ����ڴ�
	return true;
}
void Draw()
{
	glClearColor(0, 0, 1, 1.0f);     //������ʲô��ɫ��������
	glClear(GL_COLOR_BUFFER_BIT);
	glEnable(GL_TEXTURE_2D);
	glBindTexture(GL_TEXTURE_2D, texture);//����Ϊ��ǰ����
	
	
	DrawModel();
}
void DrawModel()
{

	glBegin(GL_QUADS);
	glColor4ub(255, 255, 255, 255);
	//��ʱ�뷽�����õ��λ��
	//����
	glTexCoord2f(0.0f, 0.0f);//��������(uv)
	glVertex3f(-0.1f, -0.1f, -0.4f);//��һ���������

	//����
	glTexCoord2f(1.0f, 0.0f);
	//glTexCoord2f(2.0f, 0.0f);
	glVertex3f(0.1f, -0.1f, -0.4f);

	//����
	glTexCoord2f(1.0f, 1.0f);
	//glTexCoord2f(2.0f, 2.0f);
	glVertex3f(0.1f, 0.1f, -0.4f);

	//����
	glTexCoord2f(0, 1.0f);
	glVertex3f(-0.1f, 0.1f, -0.4f);
	glEnd();
}
