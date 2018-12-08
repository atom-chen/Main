#include "2DScene.h"
#include "Time.h"
#include "Utils.h"

const char* const stonePic = RES_ROOT"stone.bmp";
const char* const woodPic = RES_ROOT"wood.bmp";
const char* const ssxPic = RES_ROOT"ny.png";


enum Scene2DType
{
	TINVALID = -1,
	BLEND = 0,
	LIGHTER = 1,
	DARKER = 2,
	ZPDD = 3,
	MOTE_DARKER = 4,
	MOTE_LIGHTER = 5,
	RG = 6,
	ADD = 7,
	DEL = 8,
	MAX
};

bool Scene2D::Awake()
{
	if (m_MainCamera == nullptr)
	{
		m_MainCamera = new Camera;
	}

	state = 0;
	m_AlphaBlendFSQ.Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"alpha_blend.frag");   //混合
	m_LighterFSQ.Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"lighter.frag");         //变亮
	m_DarkerFSQ.Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"darker.frag");           //变暗
	m_zpddFSQ.Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"zpdd.frag");               //正片叠底
	m_moteDarkerFSQ.Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"moto_darker.frag");        //颜色加深
	m_moteLighterFSQ.Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"moto_lighter.frag");        //颜色减淡
	m_rgFSQ.Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"rg.frag");         //柔光
	m_AddFSQ.Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"add.frag");       //相加
	m_DelFSQ.Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"minus.frag");       //相减
	return 1;
}

void Scene2D::Start()
{
	SceneManager::SetClearColor(vec4(0, 0, 0, 1));
	m_AlphaBlendFSQ.SetTexture2D(DEFAULT_TEXTURE2D);
	m_AlphaBlendFSQ.SetTexture2D(ssxPic, "U_Texture_2");

	m_LighterFSQ.SetTexture2D(DEFAULT_TEXTURE2D);
	m_LighterFSQ.SetTexture2D(ssxPic, "U_Texture_2");

	m_DarkerFSQ.SetTexture2D(DEFAULT_TEXTURE2D);
	m_DarkerFSQ.SetTexture2D(ssxPic, "U_Texture_2");

	m_zpddFSQ.SetTexture2D(DEFAULT_TEXTURE2D);
	m_zpddFSQ.SetTexture2D(ssxPic, "U_Texture_2");

	m_moteDarkerFSQ.SetTexture2D(DEFAULT_TEXTURE2D);
	m_moteDarkerFSQ.SetTexture2D(ssxPic, "U_Texture_2");

	m_moteLighterFSQ.SetTexture2D(DEFAULT_TEXTURE2D);
	m_moteLighterFSQ.SetTexture2D(ssxPic, "U_Texture_2");

	m_rgFSQ.SetTexture2D(DEFAULT_TEXTURE2D);
	m_rgFSQ.SetTexture2D(ssxPic, "U_Texture_2");

	m_AddFSQ.SetTexture2D(DEFAULT_TEXTURE2D);
	m_AddFSQ.SetTexture2D(ssxPic, "U_Texture_2");

	m_DelFSQ.SetTexture2D(DEFAULT_TEXTURE2D);
	m_DelFSQ.SetTexture2D(ssxPic, "U_Texture_2");
}

void Scene2D::Update()
{
	
}
void Scene2D::OnDrawBegin()
{

}
void Scene2D::Draw3D()
{
	switch (state)
	{
	case Scene2DType::BLEND:	m_AlphaBlendFSQ.Draw(); break;
	case Scene2DType::LIGHTER:m_LighterFSQ.Draw(); break;
	case Scene2DType::DARKER:m_DarkerFSQ.Draw(); break;
	case Scene2DType::ZPDD:m_zpddFSQ.Draw(); break;
	case Scene2DType::MOTE_DARKER:m_moteDarkerFSQ.Draw(); break;
	case Scene2DType::MOTE_LIGHTER:m_moteLighterFSQ.Draw(); break;
	case Scene2DType::RG:m_rgFSQ.Draw(); break;
	case Scene2DType::ADD:m_AddFSQ.Draw(); break;
	case Scene2DType::DEL:m_DelFSQ.Draw(); break;
	default:break;
	}
}

void Scene2D::Draw2D()
{

}
void Scene2D::OnDesrory()
{
	m_AlphaBlendFSQ.Destroy();
	m_LighterFSQ.Destroy();
	m_DarkerFSQ.Destroy();
	m_zpddFSQ.Destroy();
	m_moteDarkerFSQ.Destroy();
	m_moteLighterFSQ.Destroy();
	m_rgFSQ.Destroy();
	m_DelFSQ.Destroy();
	m_AddFSQ.Destroy();
}

void Scene2D::OnKeyDown(char KeyCode)//按下键盘时调用
{
	switch (KeyCode)
	{
	case VK_UP:if (state > Scene2DType::TINVALID + 1){ state--; } break;
	case VK_DOWN:if (state < Scene2DType::MAX -1){ state++; } break;
	case VK_LEFT:state = Scene2DType::TINVALID + 1; break;
	case VK_RIGHT:state = Scene2DType::MAX -1; break;
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