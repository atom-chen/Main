#include "Scene.h"
#include "Utils.h"
#include "Shader.h"
#include "Ground.h"
#include "Model.h"

/*
插槽起始位置从0开始
不同标志符的插槽不共享
*/
GLuint texture;
glm::mat4 viewMatrix, projectionMatrix;
Ground m_Ground;
Model m_T;
bool Awake()
{
	glewInit();

	m_Ground.Init();
	//m_T.Init("res/niutou.obj","res/test.vert", "res/text.frag");
	m_T.Init("res/Sphere.obj");
	m_T.SetTexture2D("res/earth.bmp");
	m_T.SetPosition(0, 0, -5);

	return 1;
}

void SetViewPortSize(float width, float height)
{
	projectionMatrix = glm::perspective(60.0f, width / height, 0.1f, 1000.0f);//设置投影矩阵
}
void Update()
{
	float deltime = GetFrameTime();

}
void OnDrawBegin()
{
	glClearColor(0, 0, 0, 1);
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
}
void Draw()
{
	m_Ground.Draw(viewMatrix, projectionMatrix);
	m_T.Draw(viewMatrix, projectionMatrix);
}
void OnDrawOver()
{
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
	glBindBuffer(GL_ARRAY_BUFFER, 0);
	glUseProgram(0);     //好习惯：将当前程序设置为0号程序（状态机的概念）
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

