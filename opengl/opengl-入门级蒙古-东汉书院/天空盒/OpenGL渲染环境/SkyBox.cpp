#include "SkyBox.h"
#include "Utils.h"

bool SkyBox::Init(const char* bmpPath)
{
	char temp[256];
	memset(temp, 0, 256);
	strcpy(temp, bmpPath);//temp="res/"
	strcat(temp, "front.bmp");//temp="res/front.bmp"
	//����
	m_Texture[0] = CreateTexture2DFromBMP(temp);

	memset(temp, 0, 256);
	strcpy(temp, bmpPath);//temp="res/"
	strcat(temp, "back.bmp");//temp="res/front.bmp"
	//����
	m_Texture[1] = CreateTexture2DFromBMP(temp);

	memset(temp, 0, 256);
	strcpy(temp, bmpPath);//temp="res/"
	strcat(temp, "left.bmp");//temp="res/front.bmp"
	//����
	m_Texture[2] = CreateTexture2DFromBMP(temp);

	memset(temp, 0, 256);
	strcpy(temp, bmpPath);//temp="res/"
	strcat(temp, "right.bmp");//temp="res/front.bmp"
	//����
	m_Texture[3] = CreateTexture2DFromBMP(temp);

	memset(temp, 0, 256);
	strcpy(temp, bmpPath);//temp="res/"
	strcat(temp, "top.bmp");//temp="res/front.bmp"
	//����
	m_Texture[4] = CreateTexture2DFromBMP(temp);

	memset(temp, 0, 256);
	strcpy(temp, bmpPath);//temp="res/"
	strcat(temp, "bottom.bmp");//temp="res/front.bmp"
	//����
	m_Texture[5] = CreateTexture2DFromBMP(temp);

	return 1;
}
void SkyBox::Draw()
{
	//����պл����������޸���պе����ֵ����ôOpenGL�ͻ���Ϊ������Զ����Ĭ�ϣ�
	glDisable(GL_DEPTH_TEST);//�ص���Ȳ���
	glEnable(GL_TEXTURE_2D);
	glColor4ub(255, 255, 255, 255);

	//��ʱ�뷽�����õ��λ�ã������Լ�վ�ں����ڲ� ȥ��
	//ǰ
	glBindTexture(GL_TEXTURE_2D, m_Texture[0]);
	glBegin(GL_QUADS);
	glTexCoord2f(0, 0);
	glVertex3f(-0.5f, -0.5f, -0.5f);//1
	glTexCoord2f(1, 0);
	glVertex3f(0.5f, -0.5f, -0.5f);//2
	glTexCoord2f(1, 1);
	glVertex3f(0.5f, 0.5f, -0.5f);//3
	glTexCoord2f(0, 1);
	glVertex3f(-0.5f, 0.5f, -0.5f);//4
	glEnd();

	//��
	glBindTexture(GL_TEXTURE_2D, m_Texture[1]);
	glBegin(GL_QUADS);
	glTexCoord2f(0, 0);
	glVertex3f(-0.5f, -0.5f, -0.2f);//1
	glTexCoord2f(1, 0);
	glVertex3f(0.5f, -0.5f, -0.2f);//2
	glTexCoord2f(1, 1);
	glVertex3f(0.5f, 0.5f, -0.2f);//3
	glTexCoord2f(0, 1);
	glVertex3f(-0.5f, 0.5f, -0.2f);//4
	glEnd();

	//��
	glBindTexture(GL_TEXTURE_2D, m_Texture[2]);
	glBegin(GL_QUADS);
	glTexCoord2f(0, 0);
	glVertex3f(-0.5f, -0.5f, -0.2f);//1==b��1
	glTexCoord2f(1, 0);
	glVertex3f(-0.5f, -0.5f, -0.5f);//2==f��1
	glTexCoord2f(1, 1);
	glVertex3f(-0.5f, 0.5f, -0.5f);//3==f��4
	glTexCoord2f(0, 1);
	glVertex3f(-0.5f, 0.5f, -0.2f);//4==b��4
	glEnd();

	//��
	glBindTexture(GL_TEXTURE_2D, m_Texture[3]);
	glBegin(GL_QUADS);
	glTexCoord2f(0, 0);
	glVertex3f(0.5f, -0.5f, -0.2f);//1==b��2
	glTexCoord2f(1, 0);
	glVertex3f(0.5f, -0.5f, -0.5f);;//2==f��2
	glTexCoord2f(1, 1);
	glVertex3f(0.5f, 0.5f, -0.5f);//3==f��3
	glTexCoord2f(0, 1);
	glVertex3f(0.5f, 0.5f, -0.2f);//4==b��3
	glEnd();

	//��
	glBindTexture(GL_TEXTURE_2D, m_Texture[4]);
	glBegin(GL_QUADS);
	glTexCoord2f(0, 0);
	glVertex3f(-0.5f, 0.5f, -0.2f);//1==l��4
	glTexCoord2f(1, 0);
	glVertex3f(0.5f, 0.5f, -0.2f);//2==r��4
	glTexCoord2f(1, 1);
	glVertex3f(0.5f, 0.5f, -0.5f);//3==r��3
	glTexCoord2f(0, 1);
	glVertex3f(-0.5f, 0.5f, -0.5f);//4==l��3
	glEnd();

	//��
	glBindTexture(GL_TEXTURE_2D, m_Texture[5]);
	glBegin(GL_QUADS);
	glTexCoord2f(0, 0);
	glVertex3f(-0.5f, -0.5f, -0.2f);//1==l��1
	glTexCoord2f(1, 0);
	glVertex3f(0.5f, -0.5f, -0.2f);//2==r��1
	glTexCoord2f(1, 1);
	glVertex3f(0.5f, -0.5f, -0.5f);//3==r��2
	glTexCoord2f(0, 1);
	glVertex3f(-0.5f, -0.5f, -0.5f);//4==l��2
	glEnd();

}