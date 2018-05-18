#include "Scene.h"
/*
gluPerspective这个函数指定了观察的视景体在世界坐标系中的具体大小
void gluPerspective(
GLdouble fovy, //角度
GLdouble aspect,//视景体的宽高比
GLdouble zNear,//沿z轴方向的两裁面之间的距离的近处
GLdouble zFar //沿z轴方向的两裁面之间的距离的远处
)
fovy指定视景体的视野的角度，以度数为单位，y轴的上下方向
aspect指定你的视景体的宽高比（x 平面上）
zNear指定观察者到视景体的最近的裁剪面的距离（必须为正数）
zFar与上面的参数相反，这个指定观察者到视景体的最远的裁剪面的距离（必须为正数）
*/
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
	//在开始绘制前设置摄像机
	//脑袋的位置，眼睛看的视点（看向哪个方向），从头顶发射出去的方向向量
	gluLookAt(0,0,0,0,0,-1,0,1,0);
	glBegin(GL_TRIANGLES);
	glColor4ub(255, 255, 255, 255);
	glVertex3f(-0.2f, -0.2f, -1.5f);

	glColor4ub(255, 0, 0, 255);
	glVertex3f(0.2f, -0.2f, -1.5f);

	glColor4ub(0, 255, 0, 255);
	glVertex3f(0.0f, 0.2f, -1.5f);
	glEnd();
}
