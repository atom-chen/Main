#include "Scene.h"
#include "Utils.h"
/*
gluPerspective�������ָ���˹۲���Ӿ�������������ϵ�еľ����С
void gluPerspective(
GLdouble fovy, //�Ƕ�
GLdouble aspect,//�Ӿ���Ŀ�߱�
GLdouble zNear,//��z�᷽���������֮��ľ���Ľ���
GLdouble zFar //��z�᷽���������֮��ľ����Զ��
)
fovyָ���Ӿ������Ұ�ĽǶȣ��Զ���Ϊ��λ��y������·���
aspectָ������Ӿ���Ŀ�߱ȣ�x ƽ���ϣ�
zNearָ���۲��ߵ��Ӿ��������Ĳü���ľ��루����Ϊ������
zFar������Ĳ����෴�����ָ���۲��ߵ��Ӿ������Զ�Ĳü���ľ��루����Ϊ������
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
}
