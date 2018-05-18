#include "Scene.h"
#include "Utils.h"
#include "Shader.h"
#include "Ground.h"
#include "Model.h"
#include "SkyBox.h"
#include "Camera.h"
/*
插槽起始位置从0开始
不同标志符的插槽不共享
*/
GLuint texture;
glm::mat4 viewMatrix, projectionMatrix;
Ground m_Ground;
Model m_box;
Model m_Niu;
SkyBox m_Skybox;
Camera m_MainCamera;
bool Awake()
{
	glewInit();

	m_Ground.Init();
	m_box.Init("res/Sphere.obj");
	m_Skybox.Init("res/front.bmp", "res/back_1.bmp", "res/top_1.bmp", "res/bottom_1.bmp", "res/left_1.bmp", "res/right_1.bmp");
	m_Niu.Init("res/niutou.obj");
	return 1;
}
void Start()
{
	m_box.SetTexture2D("res/earth.bmp");
	m_box.SetPosition(0, 0, -5);
	m_Niu.SetTexture2D("res/niutou.bmp");
	m_Niu.SetPosition(-2, 0, -4);
	m_Niu.SetScale(0.01f, 0.01f, 0.01f);
}

void SetViewPortSize(float width, float height)
{
	projectionMatrix = glm::perspective(60.0f, width / height, 0.1f, 1000.0f);//设置投影矩阵
}
void Update()
{
	float deltime = GetFrameTime();
	m_Skybox.Update(m_MainCamera.GetPosition());
	m_box.Update(deltime, m_MainCamera.GetPosition());
	m_Niu.Update(deltime, m_MainCamera.GetPosition());
}
void OnDrawBegin()
{
	glClearColor(0, 0, 0, 1);
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	m_Skybox.Draw(viewMatrix, projectionMatrix);
}
void Draw()
{
	m_Ground.Draw(viewMatrix, projectionMatrix);
	m_box.Draw(viewMatrix, projectionMatrix);
	m_Niu.Draw(viewMatrix, projectionMatrix);
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

