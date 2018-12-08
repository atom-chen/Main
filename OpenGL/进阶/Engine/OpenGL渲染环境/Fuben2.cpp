#include "Fuben2.h"
#include "Time.h"


bool Fuben2::Awake()
{
	m_Skybox.Init("res/front_3.bmp", "res/back_3.bmp", "res/top_3.bmp", "res/bottom_3.bmp", "res/left_3.bmp", "res/right_3.bmp");

	if (m_MainCamera == nullptr)
	{
		m_MainCamera = new Camera;
	}
	m_Sphere.Init("res/Sphere.obj", SHADER_ROOT"reflection.vert", SHADER_ROOT"refract.frag");
	m_Sphere2.Init("res/Sphere.obj", SHADER_ROOT"reflection.vert", SHADER_ROOT"reflection.frag");
	m_Sphere3.Init("res/Sphere.obj", SHADER_ROOT"FragObj.vert", SHADER_ROOT"FragObj.frag");
	return 1;
}

void Fuben2::Start()
{
	SceneManager::SetClearColor(vec4(0, 0, 0, 1));
	m_Sphere.GetShader().SetCubeMap("res/front_3.bmp", "res/back_3.bmp", "res/top_3.bmp", "res/bottom_3.bmp", "res/left_3.bmp", "res/right_3.bmp");
	m_Sphere2.GetShader().SetCubeMap("res/front_3.bmp", "res/back_3.bmp", "res/top_3.bmp", "res/bottom_3.bmp", "res/left_3.bmp", "res/right_3.bmp");

	m_Direction.SetAmbientColor(0.1f, 0.1f, 0.1f, 1.0f);
	m_Direction.SetDiffuseColor(0.1f, 0.2f, 1.0f, 1.0f);
	m_Direction.SetSpecularColor(0.1f, 0.9f, 0.9f, 1.0f);
	m_Direction.SetPosition(0, 1, 0);
	m_Direction.SetRotate(0, 0, 90);

	m_Sphere3.SetPosition(0, 0, 1);
	m_Sphere3.SetAmbientMaterial(0.1f, 0.1f, 0.1f, 1.0f);
	m_Sphere3.SetDiffuseMaterial(0.3f, 0.3f, 0.3f, 1.0f);
	m_Sphere3.SetSpecularMaterial(1, 1, 1, 1.0f);
	m_Sphere3.SetLight_1(m_Direction);
}

void Fuben2::Update()
{
	m_Skybox.Update(m_MainCamera->GetPosition());
	m_Sphere.Update(m_MainCamera->GetPosition());
	m_Sphere2.Update(m_MainCamera->GetPosition());
	m_Sphere3.Update(m_MainCamera->GetPosition());
}
void Fuben2::OnDrawBegin()
{
	m_Skybox.Draw();
}
void Fuben2::Draw3D()
{
	m_Sphere.Draw();
	m_Sphere2.Draw();
	m_Sphere3.Draw();
}

void Fuben2::Draw2D()
{

}
void Fuben2::OnDesrory()
{
	m_Sphere.Destroy();
	m_Skybox.Destroy();
	m_Sphere2.Destroy();
	m_Sphere3.Destroy();
}

void Fuben2::OnKeyDown(char KeyCode)//按下键盘时调用
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

void Fuben2::OnKeyUp(char KeyCode)//松开键盘时被调用
{
	switch (KeyCode)
	{
		//左移
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
		break;
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

void Fuben2::OnMouseMove(float deltaX, float deltaY)//鼠标移动导致旋转时被调
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