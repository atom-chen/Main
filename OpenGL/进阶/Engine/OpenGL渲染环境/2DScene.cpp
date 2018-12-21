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
	m_FSQS[Scene2DType::BLEND].Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"alpha_blend.frag");   //���
	m_FSQS[Scene2DType::LIGHTER].Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"lighter.frag");         //����
	m_FSQS[Scene2DType::DARKER].Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"darker.frag");           //�䰵
	m_FSQS[Scene2DType::ZPDD].Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"zpdd.frag");               //��Ƭ����
	m_FSQS[Scene2DType::MOTE_DARKER].Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"moto_darker.frag");        //��ɫ����
	m_FSQS[Scene2DType::MOTE_LIGHTER].Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"moto_lighter.frag");        //��ɫ����
	m_FSQS[Scene2DType::RG].Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"rg.frag");         //���
	m_FSQS[Scene2DType::ADD].Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"add.frag");       //���
	m_FSQS[Scene2DType::DEL].Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"minus.frag");       //���
	m_FSQS[Scene2DType::DJ].Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"dj.frag");       //����
	m_FSQS[Scene2DType::QG].Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"qg.frag");       //ǿ��
	m_FSQS[Scene2DType::CZ].Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"cz.frag");       //��ֵ
	m_FSQS[Scene2DType::FCZ].Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"fcz.frag");       //����ֵ
	m_FSQS[Scene2DType::PC].Init(SHADER_ROOT"fullScreenQuad.vert", SHADER_ROOT"pc.frag");       //����ֵ
	return 1;
}

void Scene2D::Start()
{
	SceneManager::SetClearColor(vec4(0, 0, 0, 1));
	for (int i = 0; i < ARRLEN(m_FSQS); i++)
	{
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

void Scene2D::OnKeyDown(char KeyCode)//���¼���ʱ����
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

void Scene2D::OnKeyUp(char KeyCode)//�ɿ�����ʱ������
{

}

void Scene2D::OnMouseMove(float deltaX, float deltaY)//����ƶ�������תʱ����
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