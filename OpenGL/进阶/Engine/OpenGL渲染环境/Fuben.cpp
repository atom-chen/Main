#include "Fuben.h"
#include "Time.h"


bool Fuben::Awake()
{
	m_Skybox.Init("res/front.bmp", "res/back.bmp", "res/top.bmp", "res/bottom.bmp", "res/left.bmp", "res/right.bmp");

	m_MainCamera = new Camera;

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

void Fuben::Start()
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

void Fuben::Update()
{
	m_Skybox.Update(m_MainCamera->GetPosition());
	m_Sphere.Update(m_MainCamera->GetPosition());
	m_Sphere2.Update(m_MainCamera->GetPosition());
	
}
void Fuben::OnDrawBegin()
{
	m_Skybox.Draw();

	m_Fbo.Begin();
	m_Sphere.Draw();           //��ΪFBO��������ɫ������������DirectionHDR.frag�Ĵ��� �Ὣ��������ɫд��hdr������
	m_Fbo.End();

	//��Ŀ����Ⱦ
	m_FSQ.SetTexture2D(m_Fbo.GetBuffer("color"));
	m_FSQ.DrawToLeftTop();
	m_MainCamera->Draw();
	m_FSQ.SetTexture2D(m_Fbo.GetBuffer("hdr"));
	m_FSQ.DrawToRightTop();      //�ұ߸߹�
	m_MainCamera->Draw();

	mCombinefsq.DrawToLeftTop();
	m_MainCamera->Draw();
}
void Fuben::Draw3D()
{
	//��Ŀ����Ⱦ��Ҫ��Ⱦ����
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

void Fuben::Draw2D()
{

}
void Fuben::OnDesrory()
{
	m_Skybox.Destroy();
	m_Sphere.Destroy();
	m_FSQ.Destroy();
	m_Sphere2.Destroy();
}

void Fuben::OnKeyDown(char KeyCode)//���¼���ʱ����
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

void Fuben::OnKeyUp(char KeyCode)//�ɿ�����ʱ������
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
		break;
	}
}

void Fuben::OnMouseMove(float deltaX, float deltaY)//����ƶ�������תʱ����
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