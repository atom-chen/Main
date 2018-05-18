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

void Zhucheng::OnKeyDown(char KeyCode)//按下键盘时调用
{
	Scene::OnKeyDown(KeyCode);
}
void Zhucheng::OnKeyUp(char KeyCode)//松开键盘时被调用
{
	Scene::OnKeyUp(KeyCode);
}
void Zhucheng::OnMouseMove(float deltaX, float deltaY)//鼠标移动导致旋转时被调
{
	Scene::OnMouseMove(deltaX, deltaY);
}
void Zhucheng::OnMouseWheel(int32_t direction)
{
	Scene::OnMouseWheel(direction);
}