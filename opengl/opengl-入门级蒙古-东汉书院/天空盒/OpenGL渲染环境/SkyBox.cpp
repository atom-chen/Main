#include "SkyBox.h"
#include "Utils.h"

bool SkyBox::Init(const char* bmpPath)
{
	char temp[256];
	memset(temp, 0, strlen(temp));
	strncpy(temp, bmpPath,256);//temp="res/"
	strncat(temp, "front.bmp", 256);//temp="res/front.bmp"
	//加载
	if (m_Texture != nullptr)
	{
		this->m_Texture[0] = CreateTexture2DFromBMP(temp);
	}

	return 1;
}
void SkyBox::Draw()
{
	glEnable(GL_TEXTURE_2D);
	glBindTexture(GL_TEXTURE_2D, m_Texture[0]);
	glColor4ub(255, 255, 255, 255);
	//把天空盒画出来
	glBegin(GL_QUADS);
	glTexCoord2f(0, 0);
	glVertex3f(-0.5f, -0.5f, -0.5f);

	glTexCoord2f(1, 0);
	glVertex3f(0.5f, -0.5f, -0.5f);

	glTexCoord2f(1, 1);
	glVertex3f(0.5f, 0.5f, -0.5f);

	glTexCoord2f(0, 1);
	glVertex3f(-0.5f, 0.5f, -0.5f);


	glEnd();
}