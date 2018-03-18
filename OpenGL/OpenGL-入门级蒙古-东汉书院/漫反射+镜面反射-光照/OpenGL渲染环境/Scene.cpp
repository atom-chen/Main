#include "Scene.h"

bool Init()
{
	glMatrixMode(GL_PROJECTION);
	gluPerspective(50.0f, 800.0f / 600.0f, 0.1f, 1000.0f);
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
	return true;
}
void Draw()
{
	glClearColor(0, 0, 0, 1.0f);     //设置用什么颜色擦缓冲区
	glClear(GL_COLOR_BUFFER_BIT);
	glEnable(GL_LIGHTING);
	glEnable(GL_LIGHT0);
	//齐次坐标=(x/w,y/w,z/w)，1/0=无穷大，光的位置在无穷远处，认为是太阳光（方向光）
	float lightPos[] = { 0.0f, 0.1f, 0.0f, 0.0f };
	glLightfv(GL_LIGHT0, GL_POSITION, lightPos);//方向光
	
	float whiteColor[] = { 1.0f, 1.0f, 1.0f, 1.0f };//白颜色
	float blackColor[] = { 0.0f, 0.0f, 0.0f, 1.0f };//黑色
	float ambientMat[] = { 0.07f, 0.07f, 0.07f, 1.0f };//环境光反射材质
	float diffuseMat[] = { 0.4f, 0.9f, 0.9f, 1.0f };//漫反射材质
	float specularMat[] = { 0.9f, 0.9f, 0.9f, 1.0f };//镜面反射材质

	glLightfv(GL_LIGHT0, GL_AMBIENT, whiteColor);//设置环境光
	glMaterialfv(GL_FRONT, GL_AMBIENT, ambientMat);//设置环境光反射系数
	glLightfv(GL_LIGHT0, GL_DIFFUSE, whiteColor);
	glMaterialfv(GL_FRONT, GL_DIFFUSE, diffuseMat);
	glLightfv(GL_LIGHT0, GL_SPECULAR, whiteColor);
	glMaterialfv(GL_FRONT, GL_SPECULAR, specularMat);
	DrawModel();
}
void DrawModel()
{
	glBegin(GL_TRIANGLES);
	glColor4ub(255, 255, 255, 255);
	glVertex3f(-0.2f, -0.2f, -1.5f);

	glColor4ub(255, 0, 0, 255);
	glVertex3f(0.2f, -0.2f, -1.5f);

	glColor4ub(0, 255, 0, 255);
	glVertex3f(0.0f, 0.2f, -1.5f);
	glEnd();
}
