#include "Zhucheng.h"
#include "Time.h"


bool Zhucheng::Awake()
{
	m_Skybox.Init("res/front.bmp", "res/back.bmp", "res/top.bmp", "res/bottom.bmp", "res/left.bmp", "res/right.bmp");
	m_MainCamera = new Camera_1st;
	m_Cube.Init("res/Cube.obj", "res/rgbCube.vert", "res/rgbCube.frag");
	return 1;
}

void Zhucheng::Start()
{
	m_Cube.SetPosition(0, 0, 0);
	m_MainCamera->SetTarget(&m_Cube.GetPosition());

}

void Zhucheng::Update()
{
	m_Cube.Update(m_MainCamera->GetPosition());

}
void Zhucheng::OnDrawBegin()
{

}
void Zhucheng::Draw3D()
{
	m_Cube.Draw();
}

void Zhucheng::Draw2D()
{

}

void Zhucheng::OnKeyDown(char KeyCode)//按下键盘时调用
{
	switch (KeyCode)
	{
	case 'A':
		m_MainCamera->MoveToLeft(true);
		break;
	case 'S':
		m_MainCamera->MoveToBottom(1);
		break;
	case 'W':
		m_MainCamera->MoveToTop(1);
		break;
	case 'D':
		m_MainCamera->MoveToRight(1);
		break;
	case VK_UP:
		m_Cube.MoveToTop(1);
		break;
	case VK_DOWN:
		m_Cube.MoveToBottom(1);
		break;
	case VK_LEFT:
		m_Cube.MoveToLeft(1);
		break;
	case VK_RIGHT:
		m_Cube.MoveToRight(1);
		break;
	default:
		break;
	}
}

void Zhucheng::OnKeyUp(char KeyCode)//松开键盘时被调用
{
	switch (KeyCode)
	{
		//左移
	case 'A':
		m_MainCamera->MoveToLeft(0);
		break;
	case 'S':
		m_MainCamera->MoveToBottom(0);
		break;
	case 'W':
		m_MainCamera->MoveToTop(0);
		break;
	case 'D':
		m_MainCamera->MoveToRight(0);
	case VK_UP:
		m_Cube.MoveToTop(0);
		break;
	case VK_DOWN:
		m_Cube.MoveToBottom(0);
		break;
	case VK_LEFT:
		m_Cube.MoveToLeft(0);
		break;
	case VK_RIGHT:
		m_Cube.MoveToRight(0);
		break;
	}
}

void Zhucheng::OnMouseMove(float deltaX, float deltaY)//鼠标移动导致旋转时被调
{
	Scene::OnMouseMove(deltaX, deltaY);
	float angleRotateByUp = deltaX / 1000.0f;
	float angleRotateByRight = deltaY / 1000.0f;
	m_MainCamera->Yaw(-angleRotateByUp);
	m_MainCamera->Pitch(-angleRotateByRight);
}

void Zhucheng::OnMouseWheel(int32_t direction)
{
	switch (direction)
	{
	case 120:
		m_MainCamera->MoveToFront();
		break;
	case 65416:
		m_MainCamera->MoveToBack();
		break;
	}
}