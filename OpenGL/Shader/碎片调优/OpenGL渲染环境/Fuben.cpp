#include "Fuben.h"
#include "Time.h"


bool Fuben::Awake()
{
	m_Skybox.Init("res/front_3.bmp", "res/back_3.bmp", "res/top_3.bmp", "res/bottom_3.bmp", "res/left_3.bmp", "res/right_3.bmp");

	m_MainCamera = new Camera_1st;
	//m_Sphere.Init("res/Sphere.obj","res/VertexLightObj.vert","res/VertexLightObj.frag");
	m_Sphere.Init("res/Sphere.obj", SHADER_ROOT"FragObj.vert", SHADER_ROOT"FragObj.frag");
	m_Ground.Init();
	return 1;
}

void Fuben::Start()
{
	SceneManager::SetClearColor(vec4(0, 0, 0, 1));
	m_Direction.SetAmbientColor(0.1f, 0.1f, 0.1f, 1.0f);
	m_Direction.SetDiffuseColor(0.1f, 0.2f, 1.0f, 1.0f);
	m_Direction.SetSpecularColor(0.1f, 0.9f, 0.9f, 1.0f);
	m_Direction.SetPosition(0, 1, 0);
	m_Direction.SetRotate(0, 0, 90);

	m_Sphere.SetPosition(0, 0, 1);
	m_Sphere.SetAmbientMaterial(0.1f, 0.1f, 0.1f, 1.0f);
	m_Sphere.SetDiffuseMaterial(0.3f, 0.3f, 0.3f, 1.0f);
	m_Sphere.SetSpecularMaterial(1, 1, 1, 1.0f);
	m_Sphere.SetLight_1(m_Direction);

	m_MainCamera->SetTarget(&m_Sphere.GetPosition());
}

void Fuben::Update()
{
	m_Skybox.Update(m_MainCamera->GetPosition());
	m_Sphere.Update(m_MainCamera->GetPosition());
	m_Direction.SetRotate(m_LightRotate++, 0, 0);
}
void Fuben::OnDrawBegin()
{
	m_Skybox.Draw();
}
void Fuben::Draw3D()
{
	m_Sphere.Draw();
}

void Fuben::Draw2D()
{

}
void Fuben::OnDesrory()
{
	m_Sphere.Destory();
	m_Ground.Destory();
	m_Skybox.Destory();
}

void Fuben::OnKeyDown(char KeyCode)//���¼���ʱ����
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

void Fuben::OnKeyUp(char KeyCode)//�ɿ�����ʱ������
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

void Fuben::OnMouseMove(float deltaX, float deltaY)//����ƶ�������תʱ����
{
	Scene::OnMouseMove(deltaX, deltaY);
	float angleRotateByUp = deltaX / 1000.0f;
	float angleRotateByRight = deltaY / 1000.0f;
	m_MainCamera->Yaw(-angleRotateByUp);
	m_MainCamera->Pitch(-angleRotateByRight);
}

void Fuben::OnMouseWheel(int direction)
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