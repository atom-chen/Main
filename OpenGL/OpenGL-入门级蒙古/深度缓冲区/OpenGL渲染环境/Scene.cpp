#include "Scene.h"
#include "Utils.h"
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
	return 1;
}
void Draw()
{
	glClearColor(0, 0, 0, 1.0f);     //设置用什么颜色擦缓冲区
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);//作画前擦除深度缓冲区和颜色缓冲区
	//打开深度测试（发生遮罩、画家算法）
	glEnable(GL_DEPTH_TEST);
	
	
	DrawModel();
}
void DrawModel()
{

	glBegin(GL_QUADS);

	//逆时针方向设置点的位置

	//近的四边形
	glColor4ub(255, 255, 255, 255);//白色
	//左下
	glVertex3f(-0.1f, -0.1f, -0.4f);//第一个点的坐标
	//右下
	glVertex3f(0.1f, -0.1f, -0.4f);
	//右上
	glVertex3f(0.1f, 0.1f, -0.4f);
	//左上
	glVertex3f(-0.1f, 0.1f, -0.4f);

	//远的四边形
	glColor4ub(0, 50, 200, 255);//蓝色
	//左下
	glVertex3f(-0.1f, -0.1f, -0.6f);
	//右下
	glVertex3f(0.1f, -0.1f, -0.6f);
	//右上
	glVertex3f(0.1f, 0.1f, -0.6f);
	//左上
	glVertex3f(-0.1f, 0.1f, -0.6f);
	glEnd();
}
