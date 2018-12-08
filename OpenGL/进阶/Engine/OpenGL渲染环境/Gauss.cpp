#include "Gauss.h"
#include "Time.h"

//高斯模糊
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
	blurFSQ.SetTexture2D(mFBO.GetBuffer("Color"));        //1级FBO的颜色缓冲区画到blurFSQ上，做模糊处理
	mFBO.Finish();

	blurFBO.AttachColorBuffer("Color", GL_COLOR_ATTACHMENT0, m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	blurFBO.AttachDepthBuffer("Depth", m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	mFSQ.SetTexture2D(blurFBO.GetBuffer("Color"));         //blurFSQ的数据画到二级FBO上，实际上就是画到mFSQ上
	blurFBO.Finish();
	return 1;
}

//分步模糊
void GaussScene::Start()
{
	SceneManager::SetClearColor(vec4(0, 0, 0, 1));
	mFSQ1.Init();
	blurFSQH.Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"GaussHorizontal.frag");
	blurFSQV.Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"GaussVertical.frag");

	mFBO1.AttachColorBuffer("Color", GL_COLOR_ATTACHMENT0, m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	mFBO1.AttachDepthBuffer("Depth", m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	mFBO1.Finish();
	blurFSQH.SetTexture2D(mFBO1.GetBuffer("Color"));             //取出刚渲染到rt上的图像 给FullSceneQuad做一次处理

	blurFBOH.AttachColorBuffer("Color", GL_COLOR_ATTACHMENT0, m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	blurFBOH.AttachDepthBuffer("Depth", m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	blurFBOH.Finish();
	blurFSQV.SetTexture2D(blurFBOH.GetBuffer("Color"));             //FSQV拿到经过竖直模糊的图像，进行二次处理

	blurFBOV.AttachColorBuffer("Color", GL_COLOR_ATTACHMENT0, m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	blurFBOV.AttachDepthBuffer("Depth", m_MainCamera->GetWidth(), m_MainCamera->GetHeight());
	blurFBOV.Finish();
	mFSQ1.SetTexture2D(blurFBOV.GetBuffer("Color"));             //fsq取出经过 水平处理的图像，做最终渲染
}

void GaussScene::Update()
{
	m_Cube.Update(m_MainCamera->GetPosition());
	mCube2.Update(m_MainCamera->GetPosition());
}
//高斯模糊
void GaussScene::OnDrawBegin()
{
	//先画到FSQ的颜色缓冲区上
	mFBO.Begin();
	m_Cube.Draw();
	mFBO.End();


	blurFBO.Begin();
	blurFSQ.Draw();       //做模糊处理
	blurFBO.End();

	mFSQ.DrawToLeftTop();        //对colorbuff上的图像做模糊处理（gaussShader）
	m_MainCamera->Draw();        //手动提交一次渲染
}


//分步模糊和多次模糊
bool bOpenDoubleFuzzy = true;
void GaussScene::Draw3D()
{
	blurFSQH.SetTexture2D(mFBO1.GetBuffer("Color"));             //取出刚渲染到rt上的图像 给FullSceneQuad做一次处理
	mFBO1.Begin();
	mCube2.Draw();             //画到fbo1上
	mFBO1.End();

	blurFBOH.Begin();
	blurFSQH.Draw();           //进行竖直模糊，然后画到fboh上
	blurFBOH.End();

	blurFBOV.Begin();
	blurFSQV.Draw();         //进行竖直模糊，然后画倒FBOV上
	blurFBOV.End();

	mFSQ1.DrawToLeftBottom();
	m_MainCamera->Draw();        //手动提交一次渲染
	if (bOpenDoubleFuzzy)
	{
		blurFSQH.SetTexture2D(blurFBOV.GetBuffer("Color"));     //拿到第一次模糊的结果
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

void GaussScene::OnKeyDown(char KeyCode)//按下键盘时调用
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

void GaussScene::OnKeyUp(char KeyCode)//松开键盘时被调用
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

void GaussScene::OnMouseMove(float deltaX, float deltaY)//鼠标移动导致旋转时被调
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