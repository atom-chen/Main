#include "Gauss.h"
#include "Time.h"

//��˹ģ��
bool GaussScene::Awake()
{
	if (m_MainCamera == nullptr)
	{
		m_MainCamera = new Camera;
	}

	m_Cube.Init("res/Cube.obj", SHADER_ROOT"rgbCube.vert", SHADER_ROOT"rgbCube.frag");
	mCube2.Init("res/Cube.obj", SHADER_ROOT"rgbCube.vert", SHADER_ROOT"rgbCube.frag");
	mFSQ.Init();
	blurFSQ.Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"Gauss.frag");

	mFBO.AttachColorBuffer("Color", GL_COLOR_ATTACHMENT0, m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	mFBO.AttachDepthBuffer("Depth", m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	blurFSQ.SetTexture2D(mFBO.GetBuffer("Color"));        //1��FBO����ɫ����������blurFSQ�ϣ���ģ������
	mFBO.Finish();

	blurFBO.AttachColorBuffer("Color", GL_COLOR_ATTACHMENT0, m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	blurFBO.AttachDepthBuffer("Depth", m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	mFSQ.SetTexture2D(blurFBO.GetBuffer("Color"));         //blurFSQ�����ݻ�������FBO�ϣ�ʵ���Ͼ��ǻ���mFSQ��
	blurFBO.Finish();
	return 1;
}

//�ֲ�ģ��
void GaussScene::Start()
{
	SceneManager::SetClearColor(vec4(0, 0, 0, 1));
	mFSQ1.Init();
	blurFSQH.Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"GaussHorizontal.frag");
	blurFSQV.Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"GaussVertical.frag");

	mFBO1.AttachColorBuffer("Color", GL_COLOR_ATTACHMENT0, m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	mFBO1.AttachDepthBuffer("Depth", m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	mFBO1.Finish();
	blurFSQH.SetTexture2D(mFBO1.GetBuffer("Color"));             //ȡ������Ⱦ��rt�ϵ�ͼ�� ��FullSceneQuad��һ�δ���

	blurFBOH.AttachColorBuffer("Color", GL_COLOR_ATTACHMENT0, m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	blurFBOH.AttachDepthBuffer("Depth", m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	blurFBOH.Finish();
	blurFSQV.SetTexture2D(blurFBOH.GetBuffer("Color"));             //FSQV�õ�������ֱģ����ͼ�񣬽��ж��δ���

	blurFBOV.AttachColorBuffer("Color", GL_COLOR_ATTACHMENT0, m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	blurFBOV.AttachDepthBuffer("Depth", m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	blurFBOV.Finish();
	mFSQ1.SetTexture2D(blurFBOV.GetBuffer("Color"));             //fsqȡ������ ˮƽ�����ͼ����������Ⱦ
}

void GaussScene::Update()
{
	m_Cube.Update(m_MainCamera->GetPosition());
	mCube2.Update(m_MainCamera->GetPosition());
}
//��˹ģ��
void GaussScene::OnDrawBegin()
{
	//�Ȼ���FSQ����ɫ��������
	mFBO.Begin();
	m_Cube.Draw();
	mFBO.End();


	blurFBO.Begin();
	blurFSQ.Draw();       //��ģ������
	blurFBO.End();

	mFSQ.DrawToLeftTop();        //��colorbuff�ϵ�ͼ����ģ������gaussShader��
	m_MainCamera->Draw();        //�ֶ��ύһ����Ⱦ
}


//�ֲ�ģ���Ͷ��ģ��
bool bOpenDoubleFuzzy = true;
void GaussScene::Draw3D()
{
	blurFSQH.SetTexture2D(mFBO1.GetBuffer("Color"));             //ȡ������Ⱦ��rt�ϵ�ͼ�� ��FullSceneQuad��һ�δ���
	mFBO1.Begin();
	mCube2.Draw();             //����fbo1��
	mFBO1.End();

	blurFBOH.Begin();
	blurFSQH.Draw();           //������ֱģ����Ȼ�󻭵�fboh��
	blurFBOH.End();

	blurFBOV.Begin();
	blurFSQV.Draw();         //������ֱģ����Ȼ�󻭵�FBOV��
	blurFBOV.End();

	mFSQ1.DrawToLeftBottom();
	m_MainCamera->Draw();        //�ֶ��ύһ����Ⱦ
	if (bOpenDoubleFuzzy)
	{
		blurFSQH.SetTexture2D(blurFBOV.GetBuffer("Color"));     //�õ���һ��ģ���Ľ��
		blurFBOH.Begin();
		blurFSQH.Draw();
		blurFBOH.End();

		blurFBOV.Begin();
		blurFSQV.Draw();
		blurFBOV.End();
		mFSQ1.DrawToRightBottom();
	}

}

void GaussScene::Draw2D()
{
}
void GaussScene::OnDesrory()
{
	m_Cube.Destroy();
	mCube2.Destroy();
	mFSQ.Destroy();
	mFSQ1.Destroy();
	blurFSQ.Destroy();
	blurFSQH.Destroy();
	blurFSQV.Destroy();
}

void GaussScene::OnKeyDown(char KeyCode)//���¼���ʱ����
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

void GaussScene::OnKeyUp(char KeyCode)//�ɿ�����ʱ������
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

void GaussScene::OnMouseMove(float deltaX, float deltaY)//����ƶ�������תʱ����
{
	Scene::OnMouseMove(deltaX, deltaY);
	float angleRotateByUp = deltaX / 1000.0f;
	float angleRotateByRight = deltaY / 1000.0f;
	m_MainCamera->Yaw(-angleRotateByUp);
	m_MainCamera->Pitch(-angleRotateByRight);
}

void GaussScene::OnMouseWheel(int direction)
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