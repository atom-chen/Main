#include "Scene.h"
#include "Utils.h"
#include "Shader.h"
#include "Ground.h"
#include "GameObject.h"
#include "SkyBox.h"
#include "Camera.h"
#include "Camera_2D.h"
#include "ParticleSystem.h"
#include "Texture.h"
#include "Time.h"
/*
�����ʼλ�ô�0��ʼ
��ͬ��־���Ĳ�۲�����
*/


bool Scene::Awake()
{
	glewInit();
	m_Ground.Init();
	m_Skybox.Init("res/front_1.bmp", "res/back_1.bmp", "res/top_1.bmp", "res/bottom_1.bmp", "res/left_1.bmp", "res/right_1.bmp");
	SetViewPortSize(WINDOW_WIDTH, WINDOW_HEIGHT);
	return 1;
}
void Scene::Start()
{


}


void Scene::Update()
{
	m_MainCamera.Update();
	m_Skybox.Update(m_MainCamera.GetPosition());
}
void Scene::OnDrawBegin()
{
	glClearColor(0, 0, 0, 1);
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	m_Skybox.Draw(m_MainCamera.GetViewMatrix(), m_MainCamera.GetProjectionMatrix());
	m_Ground.Draw(m_MainCamera.GetViewMatrix(), m_MainCamera.GetProjectionMatrix());
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
void Scene::OnDrawOver()
{
	m_FrameBuf->End();
	m_2DCamera.Draw();

	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
	glBindBuffer(GL_ARRAY_BUFFER, 0);
	glUseProgram(0);     //��ϰ�ߣ�����ǰ��������Ϊ0�ų���״̬���ĸ��
}


void Scene::SetViewPortSize(float width, float height)
{
	m_MainCamera.SetViewPortSize(width, height);
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
	m_Ground.Destory();
}

