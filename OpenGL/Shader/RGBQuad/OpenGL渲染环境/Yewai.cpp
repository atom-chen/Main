#include "Yewai.h"
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
	m_MainCamera->SetTarget(&vec3(0, 0, 0));

}

void Zhucheng::Update()
{


}
void Zhucheng::OnDrawBegin()
{

}
void Zhucheng::Draw3D()
{

}

void Zhucheng::Draw2D()
{
	m_Cube.Draw();
}

void Zhucheng::OnKeyDown(char KeyCode)//按下键盘时调用
{
	switch (KeyCode)
	{
	case 'A':
		m_MainCamera->MoveToLeft(1);
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