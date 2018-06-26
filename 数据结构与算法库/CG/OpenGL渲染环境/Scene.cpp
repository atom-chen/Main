#include "Scene.h"
#include "DDA.h"
#include "MidDrawLine.h"
#include "Bresenham.h"
#include "MidCircle.h"
#include "SimpleCircle.h"
#include "BresenhamCircle.h"
#include "Rect.h"
#include "Oval.h"


void DrawDDA()
{
	DDA(0, 0, 200, 500);
	DDA(0, 0, -200, 500);
	DDA(0, 0, 200, -500);
	DDA(0, 0, -200, -500);
}
void DrawMid()
{
	Mid(0, 0, 200, 500);
	Mid(0, 0, -200, 500);
	Mid(0, 0, 200, -500);
	Mid(0, 0, -200, -500);
}
void DrawBresenham()
{
	Bresenham(0, 0, 200, 500);
	Bresenham(0, 0, -200, 500);
	Bresenham(0, 0, 200, -500);
	Bresenham(0, 0, -200, -500);
}
void SwitchTo2D()
{
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();
	gluOrtho2D(-400, 400, -300, 300);
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
}
void SwitchTo3D()
{
	glMatrixMode(GL_PROJECTION);
	gluPerspective(50.0f, 800.0f / 600.0f, 0.1f, 1000.0f);
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
}
bool Init()
{
	glClearColor(0, 0, 0, 1.0f);     //设置用什么颜色擦缓冲区
	SwitchTo2D();
	glViewport(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT);
	return true;
}
void Draw()
{
	glClear(GL_COLOR_BUFFER_BIT);
	//DrawDDA();
	//DrawMid();
	//BDrawCircle(0, 0, 50);
	//DrawBresenham();
	//DrawRect(0, 0, 200, 200);
	DrawOval(20, 30);
	CommitPoints();
}
