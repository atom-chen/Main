#include "Fuben.h"
#include "Time.h"


bool Fuben::Awake()
{
	m_Skybox.Init("res/front_3.bmp", "res/back_3.bmp", "res/top_3.bmp", "res/bottom_3.bmp", "res/left_3.bmp", "res/right_3.bmp");

	m_MainCamera = new Camera;

	return 1;
}

void Fuben::Start()
{
	SceneManager::SetClearColor(vec4(0, 0, 0, 1));


}

void Fuben::Update()
{
	m_Skybox.Update(m_MainCamera->GetPosition());
}
void Fuben::OnDrawBegin()
{
	m_Skybox.Draw();
}
void Fuben::Draw3D()
{

}

void Fuben::Draw2D()
{

}
void Fuben::OnDesrory()
{
	m_Skybox.Destory();
}

void Fuben::OnKeyDown(char KeyCode)//按下键盘时调用
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
	default:
		break;
	}
}

void Fuben::OnKeyUp(char KeyCode)//松开键盘时被调用
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
		break;
	}
}

void Fuben::OnMouseMove(float deltaX, float deltaY)//鼠标移动导致旋转时被调
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