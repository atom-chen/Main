#include "Zhucheng.h"
#include "Time.h"


bool Zhucheng::Awake()
{
	m_Skybox.Init("res/front_1.bmp", "res/back_1.bmp", "res/top_1.bmp", "res/bottom_1.bmp", "res/left_1.bmp", "res/right_1.bmp");
	m_MainCamera = new Camera_1st;
	m_RGBCube.Init("res/Cube.obj", "res/rgbCube.vert", "res/rgbCube.frag");
	m_Sphere.Init("res/Sphere.obj");
	return 1;
}

void Zhucheng::Start()
{
	SceneManager::SetClearColor(vec4(0.8f, 0.8f, 0.8f, 1));
	m_RGBCube.SetPosition(0, 0, 0);

	m_Sphere.SetTexture2D("res/earth.bmp");
	m_Sphere.SetPosition(2, 2, 2);
	m_Sphere.SetScale(0.2f, 0.2f, 0.2f);
	m_Sphere.SetExp(0.5f);
	m_Sphere.SetMoveSpeed(2);

	m_MainCamera->SetTarget(&m_Sphere.GetPosition());
}

void Zhucheng::Update()
{
	m_Skybox.Update(m_MainCamera->GetPosition());
	m_RGBCube.Update(m_MainCamera->GetPosition());
	m_Sphere.Update(m_MainCamera->GetPosition());
}
void Zhucheng::OnDrawBegin()
{
	m_Skybox.Draw();
}
void Zhucheng::Draw3D()
{
	m_RGBCube.Draw();
	m_Sphere.Draw();
}

void Zhucheng::Draw2D()
{

}
void Zhucheng::OnDesrory()
{
	m_RGBCube.Destory();
	m_Sphere.Destory();
	m_Skybox.Destory();
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
		m_Sphere.MoveToTop(1);
		break;
	case VK_DOWN:
		m_Sphere.MoveToBottom(1);
		break;
	case VK_LEFT:
		m_Sphere.MoveToLeft(1);
		break;
	case VK_RIGHT:
		m_Sphere.MoveToRight(1);
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
		m_Sphere.MoveToTop(0);
		break;
	case VK_DOWN:
		m_Sphere.MoveToBottom(0);
		break;
	case VK_LEFT:
		m_Sphere.MoveToLeft(0);
		break;
	case VK_RIGHT:
		m_Sphere.MoveToRight(0);
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

void Zhucheng::OnMouseWheel(int direction)
{
	switch (direction)
	{
	case MOUSE_UP:
		m_MainCamera->MoveToFront();
		break;
	case MOUSE_DOWN:
		m_MainCamera->MoveToBack();
		break;
	}
}