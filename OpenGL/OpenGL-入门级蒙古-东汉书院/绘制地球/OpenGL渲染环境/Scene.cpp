#include "Scene.h"
#include "Utils.h"
#include "SkyBox.h"
#include "Model.h"

static SkyBox m_SkyBox;
static Model m_Model;
bool Init()
{
	glMatrixMode(GL_PROJECTION);
	gluPerspective(50.0f, 800.0f / 600.0f, 0.1f, 1000.0f);//定义视景体
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
	m_SkyBox.Init("res/");
	//去读模型
	m_Model.Init("res/Sphere.obj");
	m_Model.SetTexture("res/earth.bmp");

	return 1;
}
void Draw()
{
	glClearColor(0, 0, 0, 1.0f);     //设置用什么颜色擦缓冲区
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);//作画前擦除深度缓冲区和颜色缓冲区
	//要先画天空盒（画家算法）
	m_SkyBox.Draw();
	m_Model.Draw();

}

