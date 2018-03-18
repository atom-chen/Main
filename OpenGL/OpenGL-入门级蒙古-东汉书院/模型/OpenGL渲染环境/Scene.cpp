#include "Scene.h"
#include "Utils.h"
#include "SkyBox.h"
#include "Model.h"

static SkyBox m_SkyBox;
static Model m_Model;
bool Init()
{
	glMatrixMode(GL_PROJECTION);
	gluPerspective(50.0f, 800.0f / 600.0f, 0.1f, 1000.0f);//�����Ӿ���
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
	m_SkyBox.Init("res/");
	//ȥ��ģ��
	m_Model.Init("res/Quad.obj");

	return 1;
}
void Draw()
{
	glClearColor(0, 0, 0, 1.0f);     //������ʲô��ɫ��������
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);//����ǰ������Ȼ���������ɫ������
	//Ҫ�Ȼ���պУ������㷨��
	m_SkyBox.Draw();
	m_Model.Draw();

	DrawModel();
}
void DrawModel()
{
	//����Ȳ��ԣ��������֡������㷨��
	glEnable(GL_DEPTH_TEST);

}
