#include "Fuben2.h"
#include "Time.h"


bool Fuben2::Awake()
{
	m_Skybox.Init("res/front_3.bmp", "res/back_3.bmp", "res/top_3.bmp", "res/bottom_3.bmp", "res/left_3.bmp", "res/right_3.bmp");

	m_MainCamera = new Camera;
	m_Sphere.Init("res/Sphere.obj", SHADER_ROOT"reflection.vert", SHADER_ROOT"refract.frag");
	m_Sphere2.Init("res/Sphere.obj", SHADER_ROOT"reflection.vert", SHADER_ROOT"reflection.frag");
	return 1;
}

void Fuben2::Start()
{
	SceneManager::SetClearColor(vec4(0, 0, 0, 1));
	m_Sphere.GetShader().SetCueMap("res/front_3.bmp", "res/back_3.bmp", "res/top_3.bmp", "res/bottom_3.bmp", "res/left_3.bmp", "res/right_3.bmp");
	m_Sphere2.GetShader().SetCueMap("res/front_3.bmp", "res/back_3.bmp", "res/top_3.bmp", "res/bottom_3.bmp", "res/left_3.bmp", "res/right_3.bmp");
}

void Fuben2::Update()
{
	m_Skybox.Update(m_MainCamera->GetPosition());
	m_Sphere.Update(m_MainCamera->GetPosition());
	m_Sphere2.Update(m_MainCamera->GetPosition());
}
void Fuben2::OnDrawBegin()
{
	m_Skybox.Draw();
}
void Fuben2::Draw3D()
{
	m_Sphere.Draw();
	m_Sphere2.Draw();
}

void Fuben2::Draw2D()
{

}
void Fuben2::OnDesrory()
{
	m_Sphere.Destory();
	m_Skybox.Destory();
	m_Sphere2.Destory();
}

void Fuben2::OnKeyDown(char KeyCode)//���¼���ʱ����
{
	switch (KeyCode)
	{
	case 'A':
		m_Sphere.MoveToLeft(1);
		break;
	case 'S':
		m_Sphere.MoveToBottom(1);
		break;
	case 'W':
		m_Sphere.MoveToTop(1);
		break;
	case 'D':
		m_Sphere.MoveToRight(1);
		break;
	case VK_UP:
		m_Sphere2.MoveToTop(1);
		break;
	case VK_DOWN:
		m_Sphere2.MoveToBottom(1);
		break;
	case VK_LEFT:
		m_Sphere2.MoveToLeft(1);
		break;
	case VK_RIGHT:
		m_Sphere2.MoveToRight(1);
		break;
	default:
		break;
	}
}

void Fuben2::OnKeyUp(char KeyCode)//�ɿ�����ʱ������
{
	switch (KeyCode)
	{
		//����
	case 'A':
		m_Sphere.MoveToLeft(0);
		break;
	case 'S':
		m_Sphere.MoveToBottom(0);
		break;
	case 'W':
		m_Sphere.MoveToTop(0);
		break;
	case 'D':
		m_Sphere.MoveToRight(0);
	case VK_UP:
		m_Sphere2.MoveToTop(0);
		break;
	case VK_DOWN:
		m_Sphere2.MoveToBottom(0);
		break;
	case VK_LEFT:
		m_Sphere2.MoveToLeft(0);
		break;
	case VK_RIGHT:
		m_Sphere2.MoveToRight(0);
		break;
	}
}

void Fuben2::OnMouseMove(float deltaX, float deltaY)//����ƶ�������תʱ����
{
	Scene::OnMouseMove(deltaX, deltaY);
	float angleRotateByUp = deltaX / 1000.0f;
	float angleRotateByRight = deltaY / 1000.0f;
	m_MainCamera->Yaw(-angleRotateByUp);
	m_MainCamera->Pitch(-angleRotateByRight);
}

void Fuben2::OnMouseWheel(int direction)
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