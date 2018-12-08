#include "Yewai.h"
#include "Time.h"


bool Yewai::Awake()
{
	m_Skybox.Init("res/front.bmp", "res/back.bmp", "res/top.bmp", "res/bottom.bmp", "res/left.bmp", "res/right.bmp");
	if (m_MainCamera == nullptr)
	{
		m_MainCamera = new Camera_3rd;
	}
	//m_Ground.Init("res/1.jpg");
	m_box.Init("res/Sphere.obj", SHADER_ROOT"VertexObj.vert", SHADER_ROOT"VertexObj.frag");
	m_Niu.Init("res/niutou.obj", SHADER_ROOT"FragObj.vert", SHADER_ROOT"FragObj.frag");
	m_box.SetTexture2D("res/earth.bmp");
	m_Niu.SetTexture2D("res/niutou.bmp");
	m_ParticleSystem.Init(vec3(0, 0, 0));
	return 1;
}

void Yewai::Start()
{
	SceneManager::SetClearColor(vec4(0, 0, 0, 1));
	m_SpotLight.SetRotate(0,-1.5f, 0);
	m_SpotLight.SetAmbientColor(1, 0, 0, 1.0f);
	m_SpotLight.SetDiffuseColor(1, 0, 0, 1.0f);
	m_SpotLight.SetSpecularColor(1, 0, 0, 1.0f);
	m_SpotLight.SetPosition(0, 20, 0);
	m_SpotLight.SetCutoff(5);
	m_SpotLight.SetExponent(2);

	m_PointLight.SetRotate(90, 0, 0);

	m_MainCamera->SetTarget(&m_Niu.GetPosition());
	m_box.SetPosition(0, 0, 10);
	m_Niu.SetPosition(-2, 0, -4);
	m_Niu.SetScale(0.01f, 0.01f, 0.01f);
	m_box.SetLight_1(m_DirectionLight);
	m_Niu.SetLight_1(m_DirectionLight);
	//m_Ground.SetLight_1(m_SpotLight);
	m_Niu.SetRotate(0, 90, 0);

}

void Yewai::Update()
{
	m_Skybox.Update(m_MainCamera->GetPosition());
	m_ParticleSystem.Update();
	m_box.Update(m_MainCamera->GetPosition());
	m_Niu.Update(m_MainCamera->GetPosition());
	m_box.SetRotate(0, m_BoxRotateY++, 0);
}
void Yewai::OnDrawBegin()
{
	m_Skybox.Draw();
	//m_Ground.Draw();
}
void Yewai::Draw3D()
{
	m_ParticleSystem.Draw();
	m_box.Draw();
	m_Niu.Draw();
}

void Yewai::Draw2D()
{
	
}
void Yewai::OnDesrory()
{
	m_Niu.Destroy();
	m_box.Destroy();
	//m_Ground.Destory();
	m_ParticleSystem.Destroy();
	m_Skybox.Destroy();
}

void Yewai::OnKeyDown(char KeyCode)//按下键盘时调用
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

void Yewai::OnKeyUp(char KeyCode)//松开键盘时被调用
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

void Yewai::OnMouseMove(float deltaX, float deltaY)//鼠标移动导致旋转时被调
{
	Scene::OnMouseMove(deltaX, deltaY);
	float angleRotateByUp = deltaX / 1000.0f;
	float angleRotateByRight = deltaY / 1000.0f;
	m_MainCamera->Yaw(-angleRotateByUp);
	m_MainCamera->Pitch(-angleRotateByRight);
}

void Yewai::OnMouseWheel(int direction)
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