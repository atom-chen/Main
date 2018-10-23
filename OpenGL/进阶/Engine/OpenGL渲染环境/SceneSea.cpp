#include "SceneSea.h"
#include "Time.h"


bool SceneSea::Awake()
{
	m_Skybox.Init("res/front.bmp", "res/back.bmp", "res/top.bmp", "res/bottom.bmp", "res/left.bmp", "res/right.bmp");
	m_MainCamera = new Camera;
	return 1;
}

void SceneSea::Start()
{
	SceneManager::SetClearColor(vec4(0, 0, 0, 1));




}

void SceneSea::Update()
{
	m_Skybox.Update(m_MainCamera->GetPosition());
}
void SceneSea::OnDrawBegin()
{
	m_Skybox.Draw();
}
void SceneSea::Draw3D()
{

}

void SceneSea::Draw2D()
{
}
void SceneSea::OnDesrory()
{
	m_Skybox.Destory();



}

void SceneSea::OnKeyDown(char KeyCode)//���¼���ʱ����
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

void SceneSea::OnKeyUp(char KeyCode)//�ɿ�����ʱ������
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
	}
}

void SceneSea::OnMouseMove(float deltaX, float deltaY)//����ƶ�������תʱ����
{
	Scene::OnMouseMove(deltaX, deltaY);
	float angleRotateByUp = deltaX / 1000.0f;
	float angleRotateByRight = deltaY / 1000.0f;
	m_MainCamera->Yaw(-angleRotateByUp);
	m_MainCamera->Pitch(-angleRotateByRight);
}

void SceneSea::OnMouseWheel(int direction)
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