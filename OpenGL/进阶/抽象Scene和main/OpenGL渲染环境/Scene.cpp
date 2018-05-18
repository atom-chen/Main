#include "Scene.h"
#include "Utils.h"
#include "Shader.h"
#include "Ground.h"
#include "GameObject.h"
#include "SkyBox.h"
#include "Camera.h"
#include "Camera_2D.h"
#include "ParticleSystem.h"
#include "Texture.h"
/*
插槽起始位置从0开始
不同标志符的插槽不共享
*/


bool Scene::Awake()
{
	glewInit();

	m_Ground.Init();
	m_box.Init("res/Sphere.obj");
	m_Skybox.Init("res/front_1.bmp", "res/back_1.bmp", "res/top_1.bmp", "res/bottom_1.bmp", "res/left_1.bmp", "res/right_1.bmp");
	m_Niu.Init("res/niutou.obj");
	m_Texture.Init(-1, -1, 1, 1);
	m_ParticleSystem.Init(vec3(0, 0, 0));
	SetViewPortSize(WINDOW_WIDTH, WINDOW_HEIGHT);
	return 1;
}
void Scene::Start()
{
	m_box.SetTexture2D("res/earth.bmp");
	m_box.SetPosition(0, 0, -5);
	m_Niu.SetTexture2D("res/niutou.bmp");
	m_Niu.SetPosition(-2, 0, -4);
	m_Niu.SetScale(0.01f, 0.01f, 0.01f);
	m_Texture.SetImage("res/test.bmp");

	m_MainCamera.SwitchTo3D();
}


void Scene::Update()
{
	float deltime = GetFrameTime();
	m_MainCamera.Update(deltime);
	m_Skybox.Update(m_MainCamera.GetPosition());
	m_box.Update(deltime, m_MainCamera.GetPosition());
	m_Niu.Update(deltime, m_MainCamera.GetPosition());
	m_ParticleSystem.Update(deltime);
}
void Scene::OnDrawBegin()
{
	glClearColor(0, 0, 0, 1);
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

	m_Skybox.Draw(m_MainCamera.GetViewMatrix(), m_MainCamera.GetProjectionMatrix());
	m_ParticleSystem.Draw(m_MainCamera.GetViewMatrix(), m_MainCamera.GetProjectionMatrix());
}
void Scene::Draw3D()
{
	m_box.Draw(m_MainCamera.GetViewMatrix(), m_MainCamera.GetProjectionMatrix());
	m_Niu.Draw(m_MainCamera.GetViewMatrix(), m_MainCamera.GetProjectionMatrix());
}
void Scene::Draw2D()
{
	if (m_FrameBuf == nullptr)
	{
		return;
	}
	m_FrameBuf->Begin();
	m_Texture.Draw(m_2DCamera.GetViewMatrix(), m_MainCamera.GetProjectionMatrix());
	m_FrameBuf->End();
	m_2DCamera.Draw();
}
void Scene::OnDrawOver()
{
	m_Ground.Draw(m_MainCamera.GetViewMatrix(), m_MainCamera.GetProjectionMatrix());
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
	glBindBuffer(GL_ARRAY_BUFFER, 0);
	glUseProgram(0);     //好习惯：将当前程序设置为0号程序（状态机的概念）
}


void Scene::SetViewPortSize(float width, float height)
{
	m_MainCamera.SetViewPortSize(width, height);
	m_FrameBuf = new FrameBuffer;
	m_FrameBuf->AttachColorBuffer("2DUI", GL_COLOR_ATTACHMENT0, width, height);
	m_FrameBuf->AttachDepthBuffer("depth", width, height);
	m_FrameBuf->Finish();

	m_2DCamera.Init(*m_FrameBuf);
}
void Scene::OnKeyDown(char KeyCode)
{
	switch (KeyCode)
	{
		//左移
	case 'A':
		m_MainCamera.MoveToLeft(true);
		break;
	case 'S':
		m_MainCamera.MoveToBottom(1);
		break;
	case 'W':
		m_MainCamera.MoveToTop(1);
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

void Scene::OnKeyUp(char KeyCode)
{
	switch (KeyCode)
	{
		//左移
	case 'A':
		m_MainCamera.MoveToLeft(0);
		break;
	case 'S':
		m_MainCamera.MoveToBottom(0);
		break;
	case 'W':
		m_MainCamera.MoveToTop(0);
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

void Scene::OnMouseMove(float deltaX, float deltaY)
{
	float angleRotateByUp = deltaX / 1000.0f;
	float angleRotateByRight = deltaY / 1000.0f;
	m_MainCamera.Yaw(-angleRotateByUp);
	m_MainCamera.Pitch(-angleRotateByRight);

}
void Scene::OnMouseWheel(int32_t direction)
{
	switch (direction)
	{
	case 120:
		m_MainCamera.MoveToFront();
		break;
	case 65416:
		m_MainCamera.MoveToBack();
		break;
	}
}

