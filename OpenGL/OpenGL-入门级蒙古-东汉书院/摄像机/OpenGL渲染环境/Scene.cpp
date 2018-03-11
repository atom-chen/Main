#include "Scene.h"
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
	return true;
}
void Draw()
{
	glClearColor(0, 0, 0, 1.0f);     //������ʲô��ɫ��������
	glClear(GL_COLOR_BUFFER_BIT);
	//�ڿ�ʼ����ǰ���������
	//�Դ���λ�ã��۾������ӵ㣨�����ĸ����򣩣���ͷ�������ȥ�ķ�������
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
