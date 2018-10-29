#include "Fuben.h"
#include "Time.h"


bool Fuben::Awake()
{
	m_Skybox.Init("res/front.bmp", "res/back.bmp", "res/top.bmp", "res/bottom.bmp", "res/left.bmp", "res/right.bmp");

	m_MainCamera = new Camera;

	m_Sphere.Init("res/Sphere.obj", SHADER_ROOT"FragObj.vert", SHADER_ROOT"DirectionHDR.frag");
	m_Sphere.SetAmbientMaterial(0.1f, 0.1f, 0.1f, 1.0f);
	m_Sphere.SetDiffuseMaterial(0.4f, 0.4f, 0.4f, 1.0f);
	m_Sphere.SetSpecularMaterial(1, 1, 1, 1);

	m_DR.SetAmbientColor(0.1f,0.1f,0.1f,1.0f);
	m_DR.SetDiffuseColor(0.8f, 0.8f, 0.8f, 1.0f);
	m_DR.SetSpecularColor(1, 1, 1, 1);

	m_Fbo.AttachColorBuffer("color", GL_COLOR_ATTACHMENT0, m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	m_Fbo.AttachColorBuffer("hdr", GL_COLOR_ATTACHMENT1, m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	m_Fbo.AttachDepthBuffer("depth", m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	m_Fbo.Finish();
	return 1;
}

void Fuben::Start()
{
	SceneManager::SetClearColor(vec4(0, 0, 0, 1));
	m_Sphere.SetPosition(0, 0, 0);

	m_Sphere.SetLight_1(m_DR);
}

void Fuben::Update()
{
	m_Skybox.Update(m_MainCamera->GetPosition());
	m_Sphere.Update(m_MainCamera->GetPosition());
}
void Fuben::OnDrawBegin()
{
	m_Skybox.Draw();
}
void Fuben::Draw3D()
{
	m_Fbo.Begin();
	m_Sphere.Draw();
	m_Fbo.End();

	m_FSQ.SetTexture2D(m_Fbo.GetBuffer("color"));
	m_FSQ.DrawToLeftTop();
	m_MainCamera->Draw();
	m_FSQ.SetTexture2D(m_Fbo.GetBuffer("hdr"));
	m_FSQ.DrawToRightBottom();
}

void Fuben::Draw2D()
{

}
void Fuben::OnDesrory()
{
	m_Skybox.Destory();
	m_Sphere.Destory();
	m_FSQ.Destory();
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