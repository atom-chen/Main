#include "BloomScene.h"
#include "Time.h"


bool BloomScene::Awake()
{
	m_Skybox.Init("res/front.bmp", "res/back.bmp", "res/top.bmp", "res/bottom.bmp", "res/left.bmp", "res/right.bmp");
	if (m_MainCamera == nullptr)
	{
		m_MainCamera = new Camera;
	}

	m_Light.Init(RES_ROOT"Sphere.obj",SHADER_ROOT"light_render.vert",SHADER_ROOT"light_render.frag");
	m_FSQ.Init();
	m_Fbo.AttachColorBuffer("color", GL_COLOR_ATTACHMENT0, m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	m_Fbo.AttachColorBuffer("hdr", GL_COLOR_ATTACHMENT1, m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	m_Fbo.AttachDepthBuffer("depth", m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	m_Fbo.Finish();

	blurFSQH.Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"GaussHorizontal.frag");
	blurFSQV.Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"GaussVertical.frag");

	blurFBOH.AttachColorBuffer("color", GL_COLOR_ATTACHMENT0, m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	blurFBOH.Finish();
	blurFSQH.SetTexture2D(blurFBOH.GetBuffer("color"));

	blurFBOV.AttachColorBuffer("color", GL_COLOR_ATTACHMENT0, m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	blurFBOV.Finish();
	blurFSQV.SetTexture2D(blurFBOV.GetBuffer("color"));

	//将低光图和模糊高光图合并
	combineFSQ.Init(SHADER_ROOT"fullScreenQuad.vert", "ShaderSource/up/texture.frag");
	combineFSQ.SetTexture2D(m_Fbo.GetBuffer("color"));
	combineFSQ.SetTexture2D(blurFBOH.GetBuffer("color"), "U_Texture_2");
	return 1;
}

void BloomScene::Start()
{
	SceneManager::SetClearColor(vec4(0, 0, 0, 1));
	m_Light.SetPosition(0, 1.2f, 0);
	m_Light.SetScale(0.1f, 0.1f, 0.1f);
}

void BloomScene::Update()
{
	m_Light.Update(m_MainCamera->GetPosition());
	m_Skybox.Update(m_MainCamera->GetPosition());
}
void BloomScene::OnDrawBegin()
{
	m_Fbo.Begin();
	m_Skybox.Draw();
	m_Light.Draw();
	m_Fbo.End();

	m_FSQ.SetTexture2D(m_Fbo.GetBuffer("color"));          //低光图
	m_FSQ.DrawToLeftTop();
	m_MainCamera->Draw();

	m_FSQ.SetTexture2D(m_Fbo.GetBuffer("hdr"));            //高光图
	m_FSQ.DrawToRightTop();
	m_MainCamera->Draw();
}
void BloomScene::Draw3D()
{
	//模糊处理
	blurFBOV.Begin();
	blurFSQV.SetTexture2D(m_Fbo.GetBuffer("hdr"));                //拿到高光图，做模糊处理
	blurFSQV.Draw();
	blurFBOV.End();

	blurFBOH.Begin();
	blurFSQH.SetTexture2D(blurFBOV.GetBuffer("color"));                //拿到模糊后的图，做二次模糊
	blurFSQH.Draw();
	blurFBOH.End();

	m_FSQ.SetTexture2D(blurFBOH.GetBuffer("color"));
	m_FSQ.DrawToLeftBottom();
	m_MainCamera->Draw();

	combineFSQ.DrawToRightBottom();
}

void BloomScene::Draw2D()
{

}
void BloomScene::OnDesrory()
{

}

void BloomScene::OnKeyDown(char KeyCode)//按下键盘时调用
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

void BloomScene::OnKeyUp(char KeyCode)//松开键盘时被调用
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

void BloomScene::OnMouseMove(float deltaX, float deltaY)//鼠标移动导致旋转时被调
{
	Scene::OnMouseMove(deltaX, deltaY);
	float angleRotateByUp = deltaX / 1000.0f;
	float angleRotateByRight = deltaY / 1000.0f;
	m_MainCamera->Yaw(-angleRotateByUp);
	m_MainCamera->Pitch(-angleRotateByRight);
}

void BloomScene::OnMouseWheel(int direction)
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