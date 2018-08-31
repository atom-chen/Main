#include "Ground.h"

void Ground::Draw()
{
	//在绘制的时候设置材质
	glEnable(GL_LIGHTING);
	glMaterialfv(GL_FRONT, GL_AMBIENT, m_AmbientMaterial);
	glMaterialfv(GL_FRONT, GL_DIFFUSE, m_DiffuseMaterial);
	glMaterialfv(GL_FRONT, GL_SPECULAR, m_SpecularMaterial);
	glEnable(GL_COLOR_MATERIAL);

	glEnable(GL_DEPTH_TEST);
	glDisable(GL_TEXTURE_2D);
	glBegin(GL_QUADS);
	//地面：法线向上
	glNormal3f(0, 1, 0);
	for (int z = 0; z < 20;z++)
	{
		//起始坐标
		float zStart = 100 - z * 10;
		for (int x = 0; x < 20; x++)
		{
			float xStart = 100 - x * 10;
			if ((x % 2) == 0 && (z % 2) == 0)
			{
				glColor4ub(41, 41, 41, 255);
			}
			else
			{
				glColor4ub(200, 200, 200, 255);
			}
			glVertex3f(xStart, -1.0f, zStart);
			glVertex3f(xStart+10, -1.0f, zStart);
			glVertex3f(xStart + 10, -1.0f, zStart-10);
			glVertex3f(xStart, -1.0f, zStart - 10);
		}
	}
	glEnd();
}

void Ground::SetAmbientMaterialColor(float r, float g, float b, float a)
{
	m_AmbientMaterial[0] = r;
	m_AmbientMaterial[1] = g;
	m_AmbientMaterial[2] = b;
	m_AmbientMaterial[3] = a;
}
void Ground::SetDiffuseMaterialColor(float r, float g, float b, float a)
{
	m_DiffuseMaterial[0] = r;
	m_DiffuseMaterial[1] = g;
	m_DiffuseMaterial[2] = b;
	m_DiffuseMaterial[3] = a;
}
void Ground::SetSpecularMaterialColor(float r, float g, float b, float a)
{
	m_SpecularMaterial[0] = r;
	m_SpecularMaterial[1] = g;
	m_SpecularMaterial[2] = b;
	m_SpecularMaterial[3] = a;
}