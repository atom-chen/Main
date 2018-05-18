#include "Scene.h"
#include "Utils.h"
#include "Shader.h"
#include "Ground.h"
#include "GameObject.h"
/*
�����ʼλ�ô�0��ʼ
��ͬ��־���Ĳ�۲�����
*/
GLuint texture;
glm::mat4 modelMatrix, viewMatrix, projectionMatrix;
Ground m_Ground;
GameObject m_T;
bool Awake()
{
	glewInit();

	m_Ground.Init();
	m_T.Init();
	return 1;
}

void SetViewPortSize(float width, float height)
{
	projectionMatrix = glm::perspective(60.0f, width / height, 0.1f, 1000.0f);//����ͶӰ����
}
void Update()
{
	float deltime = GetFrameTime();

}
void OnDrawBegin()
{
	glClearColor(0.1f, 0.4f, 0.6f, 1);
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
}
void Draw()
{
	//glUseProgram(textShader.GetProgram());

	m_Ground.Draw(viewMatrix, projectionMatrix);
	m_T.Draw(viewMatrix, projectionMatrix);
}
void OnDrawOver()
{
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
	glBindBuffer(GL_ARRAY_BUFFER, 0);
	glUseProgram(0);     //��ϰ�ߣ�����ǰ��������Ϊ0�ų���״̬���ĸ��
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

