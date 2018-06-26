#include "Tools.h"
#include <vector>


float GetSlope(int x0, int y0, int x1, int y1)
{
	//计算斜率
	float k = ((y1 - (float)y0) / ((float)x1 - x0));
	return k;
}
struct vertx
{
	int posX;
	int posY;
	unsigned char r;
	unsigned char g;
	unsigned char b;
	unsigned char a;
};
std::vector<vertx> m_Vertexs;
//OpenGL固定管线使用右手坐标系
void DrawPoint(int x, int y, unsigned char r, unsigned char g, unsigned b, unsigned a)
{
	vertx v;
	v.posX = x;
	v.posY = y;
	v.r = r;
	v.g = g;
	v.b = b;
	v.a = a;
	m_Vertexs.push_back(v);
	glEnd();
}

void DrawPoint(int x, int y, COLOR color)
{
	vertx v;
	v.posX = x;
	v.posY = y;
	switch (color)
	{
	case White:
		v.r = 255;
		v.g = 255;
		v.b = 255;
		v.a = 255;
		break;
	case Black:
		v.r = 0;
		v.g = 0;
		v.b = 0;
		v.a = 255;
		break;
	case Red:
		v.r = 255;
		v.g = 0;
		v.b = 0;
		v.a = 255;
		break;
	case Blue:
		v.r = 0;
		v.g = 0;
		v.b = 255;
		v.a = 255;
		break;
	case Green:
		v.r = 0;
		v.g = 255;
		v.b = 0;
		v.a = 255;
		break;
	default:
		v.r = 255;
		v.g = 255;
		v.b = 255;
		v.a = 255;
		break;
	}
	m_Vertexs.push_back(v);
}

void CommitPoints()
{
	glBegin(GL_POINTS);
	for (auto it = m_Vertexs.begin(); it != m_Vertexs.end(); it++)
	{
		vertx v= *it;
		glColor4ub(v.r, v.g, v.b, v.a);
		glVertex2i(v.posX, v.posY);
	}
	glEnd();
	m_Vertexs.clear();
}