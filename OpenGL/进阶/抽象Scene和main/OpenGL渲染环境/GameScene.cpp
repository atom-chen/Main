#include "GameScene.h"

bool Zhucheng::Awake()
{
	return Scene::Awake();
}
void Zhucheng::Start()
{
	Scene::Start();
}

void Zhucheng::Update()
{
	Scene::Update();
}
void Zhucheng::OnDrawBegin()
{
	Scene::OnDrawBegin();
}
void Zhucheng::Draw3D()
{
	Scene::Draw3D();
}
void Zhucheng::Draw2D()
{
	Scene::Draw2D();
}
void Zhucheng::OnDrawOver()
{
	Scene::OnDrawOver();
}

void Zhucheng::SetViewPortSize(float width, float height)
{
	Scene::SetViewPortSize(width, height);
}

void Zhucheng::OnKeyDown(char KeyCode)//���¼���ʱ����
{
	Scene::OnKeyDown(KeyCode);
}
void Zhucheng::OnKeyUp(char KeyCode)//�ɿ�����ʱ������
{
	Scene::OnKeyUp(KeyCode);
}
void Zhucheng::OnMouseMove(float deltaX, float deltaY)//����ƶ�������תʱ����
{
	Scene::OnMouseMove(deltaX, deltaY);
}
void Zhucheng::OnMouseWheel(int32_t direction)
{
	Scene::OnMouseWheel(direction);
}