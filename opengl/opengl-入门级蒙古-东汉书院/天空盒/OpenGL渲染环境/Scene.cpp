#include "Scene.h"
#include "Utils.h"
#include "SkyBox.h"

SkyBox m_SkyBox;
bool Init()
{
	glMatrixMode(GL_PROJECTION);
	gluPerspective(50.0f, 800.0f / 600.0f, 0.1f, 1000.0f);//�����Ӿ���
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
	m_SkyBox.Init("res/");
	return 1;
}
void Draw()
{
	glClearColor(0, 0, 0, 1.0f);     //������ʲô��ɫ��������
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);//����ǰ������Ȼ���������ɫ������
	//����Ȳ��ԣ��������֡������㷨��
	glEnable(GL_DEPTH_TEST);
	
	
	DrawModel();
}
void DrawModel()
{

	glBegin(GL_QUADS);

	//��ʱ�뷽�����õ��λ��

	//�����ı���
	glColor4ub(255, 255, 255, 255);//��ɫ
	//����
	glVertex3f(-0.1f, -0.1f, -0.4f);//��һ���������
	//����
	glVertex3f(0.1f, -0.1f, -0.4f);
	//����
	glVertex3f(0.1f, 0.1f, -0.4f);
	//����
	glVertex3f(-0.1f, 0.1f, -0.4f);

	//Զ���ı���
	glColor4ub(0, 50, 200, 255);//��ɫ
	//����
	glVertex3f(-0.1f, -0.1f, -0.6f);
	//����
	glVertex3f(0.1f, -0.1f, -0.6f);
	//����
	glVertex3f(0.1f, 0.1f, -0.6f);
	//����
	glVertex3f(-0.1f, 0.1f, -0.6f);
	glEnd();
	m_SkyBox.Draw();
}
