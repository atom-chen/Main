#include "Scene.h"
#include "Utils.h"
#include "Shader.h"
#include "Ground.h"
#include "Model.h"
#include "SkyBox.h"
#include "Camera.h"
#include "ParticleSystem.h"
/*
插槽起始位置从0开始
不同标志符的插槽不共享
*/
GLuint texture;
Ground m_Ground;
Model m_box;
Model m_Niu;
SkyBox m_Skybox;
ParticleSystem m_ParticleSystem;
Camera_1st m_MainCamera;

bool Awake()
{
	glewInit();

	m_Ground.Init();
	m_box.Init("res/Sphere.obj");
	m_Skybox.Init("res/front_1.bmp", "res/back_1.bmp", "res/top_1.bmp", "res/bottom_1.bmp", "res/left_1.bmp", "res/right_1.bmp");
	m_Niu.Init("res/niutou.obj");
	m_ParticleSystem.Init(vec3(0, 0, 0));
	return 1;
}
void Start()
{
	m_box.SetTexture2D("res/earth.bmp");
	m_box.SetPosition(0, 0, -5);
	m_Niu.SetTexture2D("res/niutou.bmp");
	m_Niu.SetPosition(-2, 0, -4);
	m_Niu.SetScale(0.01f, 0.01f, 0.01f);

	//m_MainCamera.SetTarget(&(m_ParticleSystem.GetPosition()));
}


void Update()
{
	float deltime = GetFrameTime();
	m_MainCamera.Update(deltime);
	m_Skybox.Update(m_MainCamera.GetPosition());
	m_box.Update(deltime, m_MainCamera.GetPosition());
	m_Niu.Update(deltime, m_MainCamera.GetPosition());
	m_ParticleSystem.Update(deltime);
}
void OnDrawBegin()
{
	glClearColor(0, 0, 0, 1);
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	m_Skybox.Draw(m_MainCamera.GetViewMatrix(), m_MainCamera.GetProjectionMatrix());
}
void Draw()
{
	m_box.Draw(m_MainCamera.GetViewMatrix(), m_MainCamera.GetProjectionMatrix());
	m_Niu.Draw(m_MainCamera.GetViewMatrix(), m_MainCamera.GetProjectionMatrix());
	m_ParticleSystem.Draw(m_MainCamera.GetViewMatrix(), m_MainCamera.GetProjectionMatrix());
}
void OnDrawOver()
{
	m_Ground.Draw(m_MainCamera.GetViewMatrix(), m_MainCamera.GetProjectionMatrix());
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
	glBindBuffer(GL_ARRAY_BUFFER, 0);
	glUseProgram(0);     //好习惯：将当前程序设置为0号程序（状态机的概念）
}
void OnKeyDown(char KeyCode)
{
	switch (KeyCode)
	{
	//左移
	case 'A':
		m_MainCamera.MoveToLeft(true);
		break;
	case 'S':
		m_MainCamera.MoveToBack(1);
		break;
	case 'W':
		m_MainCamera.MoveToFront(1);
		break;
	case 'D':
		m_MainCamera.MoveToRight(1);
		break;
	case VK_UP:
		m_Niu.MoveToTop(1);
		break;
	case VK_DOWN:
		m_Niu.MoveToBottom(1);
		break;
	case VK_LEFT:
		m_Niu.MoveToLeft(1);
		break;
		case VK_RIGHT:
		m_Niu.MoveToRight(1);
		break;
	default:
		break;
	}
}

void OnKeyUp(char KeyCode)
{
	switch (KeyCode)
	{
		//左移
	case 'A':
		m_MainCamera.MoveToLeft(0);
		break;
	case 'S':
		m_MainCamera.MoveToBack(0);
		break;
	case 'W':
		m_MainCamera.MoveToFront(0);
		break;
	case 'D':
		m_MainCamera.MoveToRight(0);
	default:
	case VK_UP:
		m_Niu.MoveToTop(0);
		break;
	case VK_DOWN:
		m_Niu.MoveToBottom(0);
		break;
	case VK_LEFT:
		m_Niu.MoveToLeft(0);
		break;
	case VK_RIGHT:
		m_Niu.MoveToRight(0);
		break;
	}
}

void OnMouseMove(float deltaX, float deltaY)
{
	float angleRotateByUp = deltaX / 1000.0f;
	float angleRotateByRight = deltaY / 1000.0f;
	m_MainCamera.Yaw(-angleRotateByUp);
	m_MainCamera.Pitch(-angleRotateByRight);

}

