#include "HDR.h"
#include "Time.h"


bool HDRScene::Awake()
{
	m_Skybox.Init("res/front.bmp", "res/back.bmp", "res/top.bmp", "res/bottom.bmp", "res/left.bmp", "res/right.bmp");

	if (m_MainCamera == nullptr)
	{
		m_MainCamera = new Camera;
	}

	mCombineFSQ.Init(SHADER_ROOT"fullScreenQuad.vert", "ShaderSource/up/texture.frag");
	mCombinefsq.Init(SHADER_ROOT"fullScreenQuad.vert", "ShaderSource/up/texture.frag");

	m_Sphere.Init("res/Sphere.obj", SHADER_ROOT"FragObj.vert", SHADER_ROOT"DirectionHDR.frag");
	m_Sphere.SetAmbientMaterial(0.1f, 0.1f, 0.1f, 1.0f);
	m_Sphere.SetDiffuseMaterial(0.4f, 0.4f, 0.4f, 1.0f);
	m_Sphere.SetSpecularMaterial(1, 1, 1, 1);

	m_DR.SetAmbientColor(0.1f,0.1f,0.1f,1.0f);
	m_DR.SetDiffuseColor(0.8f, 0.8f, 0.8f, 1.0f);
	m_DR.SetSpecularColor(1, 1, 1, 1);
	m_DR.SetPosition(0, 1, 0);

	m_Fbo.AttachColorBuffer("color", GL_COLOR_ATTACHMENT0, m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	m_Fbo.AttachColorBuffer("hdr", GL_COLOR_ATTACHMENT1, m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	m_Fbo.AttachDepthBuffer("depth", m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	m_Fbo.Finish();

	m_FSQ.Init();
	m_Sphere.SetPosition(0, 0, 0);
	m_Sphere.SetLight_1(m_DR);
	mCombinefsq.SetTexture2D(m_Fbo.GetBuffer("color"));
	mCombinefsq.SetTexture2D(m_Fbo.GetBuffer("hdr"), "U_Texture_2");
	return 1;
}

void HDRScene::Start()
{
	SceneManager::SetClearColor(vec4(0, 0, 0, 1));

	m_Sphere2.Init("res/Sphere.obj", SHADER_ROOT"FragObj.vert", SHADER_ROOT"hdr.frag");
	m_Sphere2.SetAmbientMaterial(0.1f, 0.1f, 0.1f, 1.0f);
	m_Sphere2.SetDiffuseMaterial(0.4f, 0.4f, 0.4f, 1.0f);
	m_Sphere2.SetSpecularMaterial(1, 1, 1, 1);
	m_Sphere2.SetLight_1(m_DR);

	ldrFbo.AttachColorBuffer("color", GL_COLOR_ATTACHMENT0, m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	hdrFbo.AttachColorBuffer("color", GL_COLOR_ATTACHMENT0, m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	ldrFbo.AttachDepthBuffer("depth", m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	hdrFbo.AttachDepthBuffer("depth", m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	ldrFbo.Finish();
	hdrFbo.Finish();

	ldrShader.Init(SHADER_ROOT"FragObj.vert", SHADER_ROOT"ldr.frag");
	ldrShader.SetVec4("U_AmbientMaterial", 0.1f, 0.1f, 0.1f, 1.0f);
	ldrShader.SetVec4("U_DiffuseMaterial", 0.4f, 0.4f, 0.4f, 1.0f);
	ldrShader.SetVec4("U_SpecularMaterial", 1, 1, 1, 1);
	ldrShader.SetVec4("U_Light1_Dir", vec4(m_DR.GetRotate(), 0));
	ldrShader.SetVec4("U_Light1_Ambient", m_DR.GetAmbientColor());
	ldrShader.SetVec4("U_Light1_Diffuse", m_DR.GetDiffuseColor());
	ldrShader.SetVec4("U_Light1_Specular", m_DR.GetSpecularColor());
	ldrShader.SetVec4("U_Light1_Opt", 1, 0, 0, 32);

	mCombineFSQ.SetTexture2D(ldrFbo.GetBuffer("color"));
	mCombineFSQ.SetTexture2D(hdrFbo.GetBuffer("color"),"U_Texture_2");

}

void HDRScene::Update()
{
	m_Skybox.Update(m_MainCamera->GetPosition());
	m_Sphere.Update(m_MainCamera->GetPosition());
	m_Sphere2.Update(m_MainCamera->GetPosition());
	
}
void HDRScene::OnDrawBegin()
{
	m_Skybox.Draw();

	m_Fbo.Begin();
	m_Sphere.Draw();           //因为FBO有两个颜色缓冲区，按照DirectionHDR.frag的代码 会将过亮的颜色写到hdr缓冲区
	m_Fbo.End();

	//多目标渲染
	m_FSQ.SetTexture2D(m_Fbo.GetBuffer("color"));
	m_FSQ.DrawToLeftTop();
	m_MainCamera->Draw();
	m_FSQ.SetTexture2D(m_Fbo.GetBuffer("hdr"));
	m_FSQ.DrawToRightTop();      //右边高光
	m_MainCamera->Draw();

	mCombinefsq.DrawToLeftTop();
	m_MainCamera->Draw();
}
void HDRScene::Draw3D()
{
	//单目标渲染需要渲染两次
	hdrFbo.Begin();
	m_Sphere2.Draw();
	hdrFbo.End();

	Shader temp = (m_Sphere2.GetShader());
	m_Sphere2.GetShader() = ldrShader;

	ldrFbo.Begin();
	m_Sphere2.Draw();
	ldrFbo.End();

	m_FSQ.SetTexture2D(ldrFbo.GetBuffer("color"));
	m_FSQ.DrawToLeftBottom();
	m_MainCamera->Draw();
	m_FSQ.SetTexture2D(hdrFbo.GetBuffer("color"));
	m_FSQ.DrawToRightBottom();
	m_MainCamera->Draw();

	m_Sphere2.GetShader() = temp;

	mCombineFSQ.DrawToRightTop();
}

void HDRScene::Draw2D()
{

}
void HDRScene::OnDesrory()
{
	m_Skybox.Destroy();
	m_Sphere.Destroy();
	m_FSQ.Destroy();
	m_Sphere2.Destroy();
}

void HDRScene::OnKeyDown(char KeyCode)//按下键盘时调用
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

void HDRScene::OnKeyUp(char KeyCode)//松开键盘时被调用
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

void HDRScene::OnMouseMove(float deltaX, float deltaY)//鼠标移动导致旋转时被调
{
	Scene::OnMouseMove(deltaX, deltaY);
	float angleRotateByUp = deltaX / 1000.0f;
	float angleRotateByRight = deltaY / 1000.0f;
	m_MainCamera->Yaw(-angleRotateByUp);
	m_MainCamera->Pitch(-angleRotateByRight);
}

void HDRScene::OnMouseWheel(int direction)
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