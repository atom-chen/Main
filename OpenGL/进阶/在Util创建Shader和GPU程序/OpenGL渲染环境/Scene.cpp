#include "Scene.h"
#include "Utils.h"

GLuint vbo;                  //vbo�൱��ָ���Դ��ָ��
GLint program;
bool Init()
{
	glewInit();
	float data[] = { -0.2f, -0.2f, -0.6f, 1.0f, 0.2f, -0.2f, -0.6f, 1.0f, 0, 0.2f, -0.6f, 1 };
	
	glGenBuffers(1,&vbo);//����һ��vbo,Ȼ��vbo��ʶ�����ĵ�ַ��OpenGL��ȥ�޸�����ڴ棬ʹָ֮��ĳ���Դ�
	glBindBuffer(GL_ARRAY_BUFFER, vbo);//����Ϊ��ǰvbo
	glBufferData(GL_ARRAY_BUFFER, sizeof(float) * 12, data, GL_STATIC_DRAW);//�ڵ�ǰvbo����12��float����ڴ棬�����ֵΪdataָ��ָ�������ڴ棬�����Դ��Ͳ����޸�����
	glBindBuffer(GL_ARRAY_BUFFER, 0);

	return 1;
}
void SetViewPortSize(float width, float height)
{

}
void Update()
{

}
void Draw()
{

}
void OnKeyDown(char KeyCode)
{

}

void OnKeyUp(char KeyCode)
{
}

void OnMouseMove(float deltaX, float deltaY)
{


}

