#include "GameScene.h"
#include "Time.h"


bool Yewai::Awake()
{
	m_Skybox.Init("res/front_1.bmp", "res/back_1.bmp", "res/top_1.bmp", "res/bottom_1.bmp", "res/left_1.bmp", "res/right_1.bmp");
	m_MainCamera = new Camera_3rd;
	m_Ground.Init("res/1.jpg");
	m_box.Init("res/Sphere.obj");
	m_Niu.Init("res/niutou.obj");
	m_Texture.Init(-1, -1, 0.25f, 0.25f);
	m_ParticleSystem.Init(vec3(0, 0, 0));
	return 1;
}

void Yewai::Start()
{
	m_MainCamera->SetTarget(&m_Niu.GetPosition());
	m_box.SetTexture2D("res/earth.bmp");
	m_box.SetPosition(0, 0, -5);
	m_Niu.SetTexture2D("res/niutou.bmp");
	m_Niu.SetPosition(-2, 0, -4);
	m_Niu.SetScale(0.01f, 0.01f, 0.01f);
	m_Texture.SetImage("res/test.bmp");

	m_box.SetLight_1(m_DirectionLight);
	m_Niu.SetLight_1(m_DirectionLight);
	m_Ground.SetLight_1(m_DirectionLight);


}

void Yewai::Update()
{
	m_Skybox.Update(m_MainCamera->GetPosition());

	m_ParticleSystem.Update();
	m_box.Update(m_MainCamera->GetPosition());
	m_Niu.Update(m_MainCamera->GetPosition());

}
void Yewai::OnDrawBegin()
{
	m_Skybox.Draw(m_MainCamera->GetViewMatrix(), m_MainCamera->GetProjectionMatrix());
	m_Ground.Draw();
}
void Yewai::Draw3D()
{
	m_ParticleSystem.Draw();
	m_box.Draw();
	m_Niu.Draw();
}

void Yewai::Draw2D()
{
	m_Texture.Draw(m_2DCamera.GetViewMatrix(), m_MainCamera->GetProjectionMatrix());
}

void Yewai::OnKeyDown(char KeyCode)//���¼���ʱ����
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

void Yewai::OnKeyUp(char KeyCode)//�ɿ�����ʱ������
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

void Yewai::OnMouseMove(float deltaX, float deltaY)//����ƶ�������תʱ����
{
	Scene::OnMouseMove(deltaX, deltaY);
	float angleRotateByUp = deltaX / 1000.0f;
	float angleRotateByRight = deltaY / 1000.0f;
	m_MainCamera->Yaw(-angleRotateByUp);
	m_MainCamera->Pitch(-angleRotateByRight);
}

void Yewai::OnMouseWheel(int32_t direction)
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