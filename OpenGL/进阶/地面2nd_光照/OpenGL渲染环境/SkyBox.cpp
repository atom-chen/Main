#include "SkyBox.h"
#include "Utils.h"


bool SkyBox::Init(const char* bmpPath)
{
	char temp[256];
	memset(temp, 0, 256);
	strcpy(temp, bmpPath);//temp="res/"
	strcat(temp, "front.bmp");//temp="res/front.bmp"
	//加载
	m_Texture[0] = CreateTexture2DFromBMP(temp);

	memset(temp, 0, 256);
	strcpy(temp, bmpPath);//temp="res/"
	strcat(temp, "back.bmp");//temp="res/front.bmp"
	//加载
	m_Texture[1] = CreateTexture2DFromBMP(temp);

	memset(temp, 0, 256);
	strcpy(temp, bmpPath);//temp="res/"
	strcat(temp, "left.bmp");//temp="res/front.bmp"
	//加载
	m_Texture[2] = CreateTexture2DFromBMP(temp);

	memset(temp, 0, 256);
	strcpy(temp, bmpPath);//temp="res/"
	strcat(temp, "right.bmp");//temp="res/front.bmp"
	//加载
	m_Texture[3] = CreateTexture2DFromBMP(temp);

	memset(temp, 0, 256);
	strcpy(temp, bmpPath);//temp="res/"
	strcat(temp, "top.bmp");//temp="res/front.bmp"
	//加载
	m_Texture[4] = CreateTexture2DFromBMP(temp);

	memset(temp, 0, 256);
	strcpy(temp, bmpPath);//temp="res/"
	strcat(temp, "bottom.bmp");//temp="res/front.bmp"
	//加载
	m_Texture[5] = CreateTexture2DFromBMP(temp);

	m_FastDrawCall = CreateDisplayList([this]()->void {DrawCommon(); });
	return 1;
}
void SkyBox::DrawCommon()
{
	//把天空盒画出来，不修改天空盒的深度值，那么OpenGL就会认为它在最远处（默认）
	glDisable(GL_DEPTH_TEST);//关掉深度测试
	glEnable(GL_TEXTURE_2D);
	glColor4ub(255, 255, 255, 255);

	//逆时针方向设置点的位置，假设自己站在盒子内部 去画
	//前
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

	//后
	glBindTexture(GL_TEXTURE_2D, m_Texture[1]);
	glBegin(GL_QUADS);
	glTexCoord2f(0, 0);
	glVertex3f(0.5f, -0.5f, 0.5f);//1
	glTexCoord2f(1, 0);
	glVertex3f(-0.5f, -0.5f, 0.5f);//2
	glTexCoord2f(1, 1);
	glVertex3f(-0.5f, 0.5f, 0.5f);//3
	glTexCoord2f(0, 1);
	glVertex3f(0.5f, 0.5f, 0.5f);//4
	glEnd();

	//左
	glBindTexture(GL_TEXTURE_2D, m_Texture[2]);
	glBegin(GL_QUADS);
	glTexCoord2f(0, 0);
	glVertex3f(-0.5f, -0.5f, 0.5f);//1==b的1
	glTexCoord2f(1, 0);
	glVertex3f(-0.5f, -0.5f, -0.5f);//2==f的1
	glTexCoord2f(1, 1);
	glVertex3f(-0.5f, 0.5f, -0.5f);//3==f的4
	glTexCoord2f(0, 1);
	glVertex3f(-0.5f, 0.5f, 0.5f);//4==b的4
	glEnd();

	//右
	glBindTexture(GL_TEXTURE_2D, m_Texture[3]);
	glBegin(GL_QUADS);
	glTexCoord2f(0, 0);
	glVertex3f(0.5f, -0.5f, -0.5f);//1==b的2
	glTexCoord2f(1, 0);
	glVertex3f(0.5f, -0.5f, 0.5f);;//2==f的2
	glTexCoord2f(1, 1);
	glVertex3f(0.5f, 0.5f, 0.5f);//3==f的3
	glTexCoord2f(0, 1);
	glVertex3f(0.5f, 0.5f, -0.5f);//4==b的3
	glEnd();

	//上
	glBindTexture(GL_TEXTURE_2D, m_Texture[4]);
	glBegin(GL_QUADS);
	glTexCoord2f(0, 0);
	glVertex3f(-0.5f, 0.5f, -0.5f);//1==l的4
	glTexCoord2f(1, 0);
	glVertex3f(0.5f, 0.5f, -0.5f);//2==r的4
	glTexCoord2f(1, 1);
	glVertex3f(0.5f, 0.5f, 0.5f);//3==r的3
	glTexCoord2f(0, 1);
	glVertex3f(-0.5f, 0.5f, 0.5f);//4==l的3
	glEnd();

	//下
	glBindTexture(GL_TEXTURE_2D, m_Texture[5]);
	glBegin(GL_QUADS);
	glTexCoord2f(0, 0);
	glVertex3f(-0.5f, -0.5f, 0.5f);//1==l的1
	glTexCoord2f(1, 0);
	glVertex3f(0.5f, -0.5f, 0.5f);//2==r的1
	glTexCoord2f(1, 1);
	glVertex3f(0.5f, -0.5f, -0.5f);//3==r的2
	glTexCoord2f(0, 1);
	glVertex3f(-0.5f, -0.5f, -0.5f);//4==l的2
	glEnd();

}

void SkyBox::Draw(const Vector3& cameraPos)
{
	if (m_FastDrawCall != NULL)
	{
		glDisable(GL_LIGHTING);
		glPushMatrix();
		glTranslatef(cameraPos.x, cameraPos.y, cameraPos.z);//跟着摄像机走
		glCallList(m_FastDrawCall);
		glPopMatrix();
	}
}