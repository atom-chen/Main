#include "GameScene.h"
#include "Time.h"


bool Zhucheng::Awake()
{
	if (!Scene::Awake())
	{
		return 0;
	}
	m_box.Init("res/Sphere.obj");
	m_Niu.Init("res/niutou.obj");
	m_Texture.Init(-1, -1, 1, 1);
	m_ParticleSystem.Init(vec3(0, 0, 0));
	return 1;
}

void Zhucheng::Start()
{
	Scene::Start();
	m_box.SetTexture2D("res/earth.bmp");
	m_box.SetPosition(0, 0, -5);
	m_Niu.SetTexture2D("res/niutou.bmp");
	m_Niu.SetPosition(-2, 0, -4);
	m_Niu.SetScale(0.01f, 0.01f, 0.01f);
	m_Texture.SetImage("res/test.bmp");

	m_box.SetLight_1(m_Sum);
	m_Niu.SetLight_1(m_Sum);
}

void Zhucheng::Update()
{
	Scene::Update();
	m_box.Update( m_MainCamera.GetPosition());
	m_Niu.Update(m_MainCamera.GetPosition());
	m_ParticleSystem.Update();
}

void Zhucheng::Draw3D()
{
	Scene::Draw3D();
	m_ParticleSystem.Draw(m_MainCamera.GetViewMatrix(), m_MainCamera.GetProjectionMatrix());
	m_box.Draw(m_MainCamera);
	m_Niu.Draw(m_MainCamera);
}

void Zhucheng::Draw2D()
{
	Scene::Draw2D();
	m_Texture.Draw(m_2DCamera.GetViewMatrix(), m_MainCamera.GetProjectionMatrix());
}

void Zhucheng::OnKeyDown(char KeyCode)//按下键盘时调用
{
	Scene::OnKeyDown(KeyCode);
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

void Zhucheng::OnKeyUp(char KeyCode)//松开键盘时被调用
{
	Scene::OnKeyUp(KeyCode);
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

void Zhucheng::OnMouseMove(float deltaX, float deltaY)//鼠标移动导致旋转时被调
{
	Scene::OnMouseMove(deltaX, deltaY);
	float angleRotateByUp = deltaX / 1000.0f;
	float angleRotateByRight = deltaY / 1000.0f;
	m_MainCamera.Yaw(-angleRotateByUp);
	m_MainCamera.Pitch(-angleRotateByRight);
}

void Zhucheng::OnMouseWheel(int32_t direction)
{
	Scene::OnMouseWheel(direction);
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