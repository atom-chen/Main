#include "ground.h"

void Ground::Init()
{
	mGround = glGenLists(1);
	glNewList(mGround, GL_COMPILE);
	glBegin(GL_QUADS);
	for (int z =0; z<20;++z)
	{
		float zStart = 100.0f - z*10.0f;
		for (int x=0;x<20;++x)
		{
			float xStart = x*10.0f - 100.0f;
			if ((z%2)^(x%2))
			{
				glColor4ub(41, 41, 41, 255);
			}
			else
			{
				glColor4ub(200, 200, 200, 255);
			}
			glVertex3f(xStart, -1.0f, zStart);
			glVertex3f(xStart+10.0f, -1.0f, zStart);
			glVertex3f(xStart+10.0f, -1.0f, zStart-10.0f);
			glVertex3f(xStart, -1.0f, zStart-10.0f);
		}
	}
	glEnd();
	glEndList();
}

void Ground::Draw()
{
	glEnable(GL_DEPTH_TEST);
	glDisable(GL_TEXTURE_2D);
	glDisable(GL_BLEND);
	glDisable(GL_LIGHTING);
	glCallList(mGround);
}