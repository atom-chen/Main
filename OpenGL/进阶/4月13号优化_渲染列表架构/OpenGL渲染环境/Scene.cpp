#include "Scene.h"
#include "Utils.h"
#include "Shader.h"
#include "Time.h"
/*
�����ʼλ�ô�0��ʼ
��ͬ��־���Ĳ�۲�����
*/
void Scene::SystemAwake()
{
	glewInit();
}
void Scene::SystemStart()
{
	SetViewPortSize(WINDOW_WIDTH, WINDOW_HEIGHT);
	m_3DRendList.SetCamera(m_MainCamera);
}
void Scene::SystemUpdate()
{
	m_MainCamera->Update();

}
void Scene::SystemOnDrawBegin()
{
	glClearColor(0, 0, 0, 1);
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
}
void Scene::SystemDraw3D()
{
	m_3DRendList.Clip();
	m_3DRendList.Draw();
}
void Scene::SystemDraw2D()
{
	if (m_FrameBuf == nullptr)
	{
		return;
	}
	m_FrameBuf->Begin();
}
void Scene::SystemOnDraw2DOver()
{
	m_FrameBuf->End();
	m_2DCamera.Draw();
}
void Scene::SystemOnDrawOver()
{
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
	glBindBuffer(GL_ARRAY_BUFFER, 0);
	glUseProgram(0);     //��ϰ�ߣ�����ǰ��������Ϊ0�ų���״̬���ĸ��
}

bool Scene::Awake()
{

	return 1;
}
void Scene::Start()
{

}


void Scene::Update()
{


}
void Scene::OnDrawBegin()
{

}
void Scene::Draw3D()
{

}
void Scene::Draw2D()
{


}

void Scene::OnDrawOver()
{


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

}

void Scene::OnKeyUp(char KeyCode)
{

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

