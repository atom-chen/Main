#include "2DScene.h"
#include "Time.h"
#include "Utils.h"

const char* const stonePic = RES_ROOT"stone.bmp";
const char* const woodPic = RES_ROOT"wood.bmp";
const char* const ssxPic = RES_ROOT"ny.png";




bool Scene2D::Awake()
{
	if (m_MainCamera == nullptr)
	{
		m_MainCamera = new Camera;
	}

	state = Scene2DType::MAX - 1;
	m_FSQS[Scene2DType::BLEND].Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"alpha_blend.frag");   //混合
	m_FSQS[Scene2DType::LIGHTER].Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"lighter.frag");         //变亮
	m_FSQS[Scene2DType::DARKER].Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"darker.frag");           //变暗
	m_FSQS[Scene2DType::ZPDD].Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"zpdd.frag");               //正片叠底
	m_FSQS[Scene2DType::MOTE_DARKER].Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"moto_darker.frag");        //颜色加深
	m_FSQS[Scene2DType::MOTE_LIGHTER].Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"moto_lighter.frag");        //颜色减淡
	m_FSQS[Scene2DType::RG].Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"rg.frag");         //柔光
	m_FSQS[Scene2DType::ADD].Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"add.frag");       //相加
	m_FSQS[Scene2DType::DEL].Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"minus.frag");       //相减
	m_FSQS[Scene2DType::DJ].Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"dj.frag");       //叠加
	m_FSQS[Scene2DType::QG].Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"qg.frag");       //强光
	m_FSQS[Scene2DType::CZ].Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"cz.frag");       //插值
	m_FSQS[Scene2DType::FCZ].Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"fcz.frag");       //反插值
	m_FSQS[Scene2DType::PC].Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"pc.frag");       //排除
	m_FSQS[Scene2DType::SMOOTH].Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"smooth.frag");       //平滑
	m_FSQS[Scene2DType::SHARPEN].Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"sharpen.frag");       //锐化
	m_FSQS[Scene2DType::EDGE].Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"edgeDetect.frag");       //边缘检测
	return 1;
}

void Scene2D::Start()
{
	SceneManager::SetClearColor(vec4(0, 0, 0, 1));
	for (int i = 0; i < ARRLEN(m_FSQS); i++)
	{
		if (i == Scene2DType::SMOOTH || i == Scene2DType::SHARPEN || i == Scene2DType::EDGE)
		{
			m_FSQS[i].SetTexture2D(ssxPic);
			continue;
		}
		m_FSQS[i].SetTexture2D(DEFAULT_TEXTURE2D);
		m_FSQS[i].SetTexture2D(ssxPic, "U_Texture_2");
	}

}

void Scene2D::Update()
{
	
}
void Scene2D::OnDrawBegin()
{

}
void Scene2D::Draw3D()
{
	if (state >= 0 && state < ARRLEN(m_FSQS))
	{
		m_FSQS[state].Draw();
	}
}

void Scene2D::Draw2D()
{

}
void Scene2D::OnDesrory()
{
	for (int i = 0; i < ARRLEN(m_FSQS); i++)
	{
		m_FSQS[i].Destroy();
	}
}

void Scene2D::OnKeyDown(char KeyCode)//按下键盘时调用
{
	switch (KeyCode)
	{
	case VK_LEFT:if (state > Scene2DType::TINVALID + 1){ state--; } break;
	case VK_RIGHT:if (state < Scene2DType::MAX - 1){ state++; } break;
	case VK_UP:state = Scene2DType::TINVALID + 1; break;
	case VK_DOWN:state = Scene2DType::MAX -1; break;
	default:break;
	}
	cout << state << endl;
}

void Scene2D::OnKeyUp(char KeyCode)//松开键盘时被调用
{

}

void Scene2D::OnMouseMove(float deltaX, float deltaY)//鼠标移动导致旋转时被调
{
	Scene::OnMouseMove(deltaX, deltaY);
	float angleRotateByUp = deltaX / 1000.0f;
	float angleRotateByRight = deltaY / 1000.0f;
	m_MainCamera->Yaw(-angleRotateByUp);
	m_MainCamera->Pitch(-angleRotateByRight);
}

void Scene2D::OnMouseWheel(int direction)
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