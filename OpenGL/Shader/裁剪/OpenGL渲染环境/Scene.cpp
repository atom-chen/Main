#include "Scene.h"
#include "Utils.h"
#include "Shader.h"
#include "Time.h"
#include "SceneManager.h"
/*
�����ʼλ�ô�0��ʼ
��ͬ��־���Ĳ�۲�����
*/
void Scene::SystemAwake()
{

}
void Scene::SystemStart()
{
	SetViewPortSize(WINDOW_WIDTH, WINDOW_HEIGHT);
	m_3DRendList.SetCamera(m_MainCamera);
	SceneManager::SetClearColor(vec4(0, 0, 0, 1));
}
void Scene::SystemUpdate()
{
	m_MainCamera->Update();

}
void Scene::SystemOnDrawBegin()
{
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
}
void Scene::SystemDraw3D()
{
	m_3DRendList.Clip();
	m_3DRendList.Draw();
}
void Scene::SystemDraw2D()
{

}
void Scene::SystemOnDraw2DOver()
{

}
void Scene::SystemOnDrawOver()
{
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
	glBindBuffer(GL_ARRAY_BUFFER, 0);
	glUseProgram(0);     //��ϰ�ߣ�����ǰ��������Ϊ0�ų���״̬���ĸ��
}
void Scene::SystemDesrory()
{
	if (m_MainCamera != nullptr)
	{
		delete m_MainCamera;
		m_MainCamera = nullptr;
	}
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
void Scene::OnMouseWheel(int direction)
{

}
void Scene::OnDesrory()
{

}

