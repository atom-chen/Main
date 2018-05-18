#include "Sprite.h"
#include "Utils.h"

void Sprite::SetImage(const char* path)
{
	m_Texture = CreateTexture2DFromPNG(path);
}
void Sprite::SetRect(float x, float y, float width, float height)
{
	//���½�
	m_Vertexes[0].x = x;
	m_Vertexes[0].y = y;

	//���½�
	m_Vertexes[1].x = x + width;
	m_Vertexes[1].y = y;

	//���Ͻ�
	m_Vertexes[2].x = x + width;
	m_Vertexes[2].y = y + height;

	//���Ͻ�
	m_Vertexes[3].x = x;
	m_Vertexes[3].y = y + height;
}
void Sprite::Draw()
{
	//glEnable(GL_ALPHA_TEST);
	//glAlphaFunc(GL_GREATER, 0);
	glEnable(GL_BLEND);//alpha���
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

	glEnable(GL_TEXTURE_2D);
	glBindTexture(GL_TEXTURE_2D, m_Texture);

	//2D���鲻�ܹ��պ���ɫӰ��
	glDisable(GL_LIGHTING);
	glColor4ub(255, 255, 255, 255);

	glBegin(GL_QUADS);
	glTexCoord2f(0, 0);
	glVertex2f(m_Vertexes[0].x, m_Vertexes[0].y);

	glTexCoord2f(1, 0);
	glVertex2f(m_Vertexes[1].x, m_Vertexes[1].y);

	glTexCoord2f(1,1);
	glVertex2f(m_Vertexes[2].x, m_Vertexes[2].y);

	glTexCoord2f(0, 1);
	glVertex2f(m_Vertexes[3].x, m_Vertexes[3].y);


	glEnd();
	glDisable(GL_BLEND);
}