#include "Scene.h"
#include "Utils.h"
#include "Shader.h"
#include "Time.h"
/*
插槽起始位置从0开始
不同标志符的插槽不共享
*/


bool Scene::Awake()
{
	glewInit();
	m_Skybox.Init("res/front_1.bmp", "res/back_1.bmp", "res/top_1.bmp", "res/bottom_1.bmp", "res/left_1.bmp", "res/right_1.bmp");
	return 1;
}
void Scene::Start()
{
	SetViewPortSize(WINDOW_WIDTH, WINDOW_HEIGHT);
}


void Scene::Update()
{
	m_MainCamera->Update();
	m_Skybox.Update(m_MainCamera->GetPosition());
}
void Scene::OnDrawBegin()
{
	glClearColor(0, 0, 0, 1);
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	m_Skybox.Draw(m_MainCamera->GetViewMatrix(), m_MainCamera->GetProjectionMatrix());
}
void Scene::Draw3D()
{

}
void Scene::Draw2D()
{
	if (m_FrameBuf == nullptr)
	{
		return;
	}
	m_FrameBuf->Begin();

}
void Scene::OnDraw2DOver()
{
	m_FrameBuf->End();
	m_2DCamera.Draw();
}
void Scene::OnDrawOver()
{

	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
	glBindBuffer(GL_ARRAY_BUFFER, 0);
	glUseProgram(0);     //好习惯：将当前程序设置为0号程序（状态机的概念）
}


void Scene::SetViewPortSize(float width, float height)
{
	m_MainCamera->SetViewPortSize(width, height);
	m_FrameBuf = new FrameBuffer;
	m_FrameBuf->AttachColorBuffer("2DUI", GL_COLOR_ATTACHMENT0, width, height);
	m_FrameBuf->AttachDepthBuffer("depth", width, height);
	m_FrameBuf->Finish();
	m_2DCamera.Init(*m_FrameBuf);
}
void Scene::OnKeyDown(char KeyCode)
{
	switch (KeyCode)
	{
		//左移
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
	}
}

void Scene::OnKeyUp(char KeyCode)
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

void Scene::OnMouseMove(float deltaX, float deltaY)
{


}
void Scene::OnMouseWheel(int32_t direction)
{

}
void Scene::OnDesrory()
{
	if (m_FrameBuf != nullptr)
	{
		delete m_FrameBuf;
	}
	if (m_MainCamera != nullptr)
	{
		delete m_MainCamera;
	}
}

