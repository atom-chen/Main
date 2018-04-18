#include "Yewai2.h"
#include "Time.h"


bool Yewai2::Awake()
{
	m_Skybox.Init("res/front_2.bmp", "res/back_2.bmp", "res/top_2.bmp", "res/bottom_2.bmp", "res/left_2.bmp", "res/right_2.bmp");
	m_MainCamera = new Camera_1st;
	m_Ground.Init("res/woodfloor.tga");
	m_box.Init("res/Sphere.obj", "res/VertexObj_Blin.vert", "res/VertexObj.frag");
	m_Niu.Init("res/niutou.obj", "res/FragObj.vert", "res/FragObj_Blin.frag");
	m_box.SetTexture2D("res/earth.bmp");
	m_Niu.SetTexture2D("res/niutou.bmp");
	m_Particle.Init(vec3(0, 0, 0));
	return 1;
}

void Yewai2::Start()
{
	m_SpotLight.SetType(LIGHT_SPOT);
	m_SpotLight.SetRotate(0, -1.5f, 0);
	m_SpotLight.SetAmbientColor(1, 0, 0, 1.0f);
	m_SpotLight.SetDiffuseColor(1, 0, 0, 1.0f);
	m_SpotLight.SetSpecularColor(1, 0, 0, 1.0f);
	m_SpotLight.SetPosition(0, 20, 0);
	m_SpotLight.SetCutoff(5);
	m_SpotLight.SetExponent(2);

	m_PointLight.SetType(LIGHT_POINT);
	m_PointLight.SetRotate(90, 0, 0);

	m_MainCamera->SetTarget(&m_Niu.GetPosition());
	m_box.SetPosition(0, 0, -5);
	m_Niu.SetPosition(-2, 0, -4);
	m_Niu.SetScale(0.01f, 0.01f, 0.01f);
	m_box.SetLight_1(m_DirectionLight);
	m_Niu.SetLight_1(m_DirectionLight);
	m_Ground.SetLight_1(m_SpotLight);
	m_Niu.SetRotate(0, 90, 0);

}

void Yewai2::Update()
{
	m_Skybox.Update(m_MainCamera->GetPosition());

	m_Particle.Update();
	m_box.Update(m_MainCamera->GetPosition());
	m_Niu.Update(m_MainCamera->GetPosition());
	m_box.SetRotate(0, m_BoxRotateY++, 0);
}
void Yewai2::OnDrawBegin()
{
	m_Skybox.Draw(m_MainCamera->GetViewMatrix(), m_MainCamera->GetProjectionMatrix());
	m_Ground.Draw();
}
void Yewai2::Draw3D()
{
	m_Particle.Draw();
	m_box.Draw();
	m_Niu.Draw();
}

void Yewai2::Draw2D()
{
}
void Yewai2::OnDesrory()
{
	m_Niu.Destory();
	m_box.Destory();
	m_Ground.Destory();
	m_Particle.Destory();
}

void Yewai2::OnKeyDown(char KeyCode)//���¼���ʱ����
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
		m_Niu.SetRotate(0, 90, 0);
		m_Niu.MoveToTop(1);
		break;
	case VK_DOWN:
		m_Niu.SetRotate(0, 270, 0);
		m_Niu.MoveToBottom(1);
		break;
	case VK_LEFT:
		m_Niu.SetRotate(0, 180, 0);
		m_Niu.MoveToLeft(1);
		break;
	case VK_RIGHT:
		m_Niu.SetRotate(0, 0, 0);
		m_Niu.MoveToRight(1);
		break;
	default:
		break;
	}
}

void Yewai2::OnKeyUp(char KeyCode)//�ɿ�����ʱ������
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

void Yewai2::OnMouseMove(float deltaX, float deltaY)//����ƶ�������תʱ����
{
	Scene::OnMouseMove(deltaX, deltaY);
	float angleRotateByUp = deltaX / 1000.0f;
	float angleRotateByRight = deltaY / 1000.0f;
	m_MainCamera->Yaw(-angleRotateByUp);
	m_MainCamera->Pitch(-angleRotateByRight);
}

void Yewai2::OnMouseWheel(int direction)
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