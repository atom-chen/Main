#include "Zhucheng.h"
#include "Time.h"


bool Zhucheng::Awake()
{
	m_Skybox.Init("res/front_1.bmp", "res/back_1.bmp", "res/top_1.bmp", "res/bottom_1.bmp", "res/left_1.bmp", "res/right_1.bmp");
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

void Zhucheng::OnKeyDown(char KeyCode)//���¼���ʱ����
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

void Zhucheng::OnKeyUp(char KeyCode)//�ɿ�����ʱ������
{
	switch (KeyCode)
	{
		//����
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

void Zhucheng::OnMouseMove(float deltaX, float deltaY)//����ƶ�������תʱ����
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