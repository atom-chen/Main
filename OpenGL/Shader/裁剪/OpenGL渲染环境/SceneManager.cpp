#include "SceneManager.h"
#include "Time.h"
#include "RenderOptions.h"


std::map<string, Scene*> SceneManager::m_mScene;
Scene* SceneManager::m_CurScene=nullptr;
POINT SceneManager::m_OriginalPos;//��¼�������ʱ��λ��
bool SceneManager::m_IsRotate=0;//�Ƿ�������ת
bool SceneManager::m_IsGlewInit = 0;//glew�Ƿ��Ѿ���ʼ��

AlphaBlendInfo SceneManager::m_Blend;//�Ƿ���alpha���
bool SceneManager::m_IsDepthTest=0;//�Ƿ�����Ȳ���
ProgramPointSize SceneManager::m_IsProgramPointSize;//�Ƿ��ɳ�����Ƶ�Ĵ�С
ScissorState SceneManager::m_ScissorState;//�ü�״̬

float SceneManager::m_ViewWidth=WINDOW_WIDTH;
float SceneManager::m_ViewHeight = WINDOW_HEIGHT;



void SceneManager::AddScene(string name,Scene* scene)
{
	if (scene == nullptr)
	{
		return;
	}
	m_mScene.insert(std::pair<string, Scene*>(name, scene));
}


void SceneManager::LoadScene(string name)
{
	auto it = m_mScene.find(name);
	if (it != m_mScene.end())
	{
		if (m_CurScene == it->second ||it->second==nullptr)
		{
			return;
		}
		if (m_CurScene != nullptr)
		{
			Scene* temp = m_CurScene;
			m_CurScene = nullptr;
			cout<<endl;
			temp->OnDesrory();
			temp->SystemDesrory();
		}
		else
		{
			InitGlew();
		}
		cout << endl;
		it->second->SystemAwake();
		it->second->Awake();
		it->second->SetViewPortSize(m_ViewWidth, m_ViewHeight);
		it->second->SystemStart();
		it->second->Start();
		m_CurScene = it->second;
	}
}

void SceneManager::ReSetScene()
{
	if (m_CurScene != nullptr)
	{
		m_CurScene->Start();
	}
}

void SceneManager::Update()
{
	Time::SetDeltaTime();
	if (m_CurScene != nullptr)
	{
		m_CurScene->SystemUpdate();
		m_CurScene->Update();

		m_CurScene->SystemOnDrawBegin();
		m_CurScene->OnDrawBegin();

		m_CurScene->Draw3D();
		m_CurScene->SystemDraw3D();

		m_CurScene->SystemDraw2D();
		m_CurScene->Draw2D();


		m_CurScene->SystemOnDraw2DOver();
		m_CurScene->SystemOnDrawOver();
		m_CurScene->OnDrawOver();
	}
}

int SceneManager::Event(UINT message, WPARAM wParam, LPARAM lParam,HWND hwnd)
{
	if (m_CurScene == nullptr)
	{
		return 0;
	}
	switch (message)
	{
	case WM_KEYDOWN:
		m_CurScene->OnKeyDown(wParam);
		break;
	case WM_KEYUP:
		if (wParam == 'F')
		{
			ReSetScene();
		}
		m_CurScene->OnKeyUp(wParam);
		break;
	case WM_MOUSEMOVE:
		if (m_IsRotate)
		{
			//��¼������ƫ������
			POINT currentPos;
			currentPos.x = LOWORD(lParam);
			currentPos.y = HIWORD(lParam);
			ClientToScreen(hwnd, &currentPos);
			//ƫ������=��������-���ʱ�������
			int deltaX = currentPos.x - m_OriginalPos.x;
			int deltaY = currentPos.y - m_OriginalPos.y;
			m_CurScene->OnMouseMove((float)deltaX, (float)deltaY);//������Ϣ
			SetCursorPos(m_OriginalPos.x, m_OriginalPos.y);//��λ��Ȼ���ټ���ƫ��
		}
		break;
	case WM_RBUTTONDOWN:
		m_OriginalPos.x = LOWORD(lParam);
		m_OriginalPos.y = HIWORD(lParam);
		ClientToScreen(hwnd, &m_OriginalPos);
		SetCapture(hwnd);
		ShowCursor(false);
		m_IsRotate = true;
		break;
	case WM_RBUTTONUP:
		m_IsRotate = false;
		SetCursorPos(m_OriginalPos.x, m_OriginalPos.y);
		ReleaseCapture();
		ShowCursor(true);
		break;
	case WM_MOUSEWHEEL:
		m_CurScene->OnMouseWheel(HIWORD(wParam));
		break;
	case WM_PAINT:
		//����
		m_CurScene->Update();
		break;
	case WM_DESTROY:
		PostQuitMessage(0);
		return 0;
	default:
		return (DefWindowProc(hwnd, message, wParam, lParam));
		break;
	}
	return 0;
}

void SceneManager::DrawGameObject(RenderAble* render)
{
	m_CurScene->m_3DRendList.InsertToRenderList(render);
}
void SceneManager::DrawGameObject(const RenderDomain& render )
{
	m_CurScene->m_3DRendList.InsertToRenderList(render);
}
void SceneManager::InitGlew()
{
	if (m_IsGlewInit)
	{
		return;
	}
	m_IsGlewInit = 1;
	glewInit();
}
void SceneManager::SetClearColor(const vec4& color)
{
	glClearColor(color.r, color.g, color.b, color.a);
}
void SceneManager::SetBlendState(AlphaBlendInfo info)
{
	if (m_Blend.AlphaBlend == info.AlphaBlend)
	{
		if (m_Blend.Type != info.Type)
		{
			glBlendFunc(GL_SRC_ALPHA, info.Type);
		}
		return;
	}
	m_Blend.AlphaBlend = info.AlphaBlend;
	if (m_Blend.AlphaBlend)
	{
		glEnable(GL_BLEND);//alpha���
		glBlendFunc(GL_SRC_ALPHA, info.Type);
	}
	else
	{
		glDisable(GL_BLEND);//alpha���
	}
}
void SceneManager::SetDepthTestState(bool isEnable)
{
	if (m_IsDepthTest == isEnable)
	{
		return;
	}
	m_IsDepthTest = isEnable;
	if (isEnable)
	{
		glEnable(GL_DEPTH_TEST);
	}
	else
	{
		glDisable(GL_DEPTH_TEST);
	}
}



void SceneManager::SetProgramPointSizeState(ProgramPointSize pointSize)
{
	if (m_IsProgramPointSize.isProgramPointSize == pointSize.isProgramPointSize)
	{
		return;
	}
	m_IsProgramPointSize.isProgramPointSize = pointSize.isProgramPointSize;
	if (m_IsProgramPointSize.isProgramPointSize)
	{
		glEnable(GL_POINT_SPRITE);
		glEnable(GL_PROGRAM_POINT_SIZE);//��Ĵ�С�ɳ������
	}
	else
	{
		glDisable(GL_POINT_SPRITE);
		glDisable(GL_PROGRAM_POINT_SIZE);//��Ĵ�С�ɳ������
	}
}

void SceneManager::SetScissorState(const ScissorState& scissor)
{
	if (m_ScissorState != scissor)
	{
		m_ScissorState = scissor;
		if (m_ScissorState.m_IsScissor)
		{
			glEnable(GL_SCISSOR_TEST);
			glScissor(m_ScissorState.xStart, m_ScissorState.yStart, m_ScissorState.width, m_ScissorState.height);
		}
		else
		{
			glDisable(GL_SCISSOR_TEST);
			glScissor(m_ScissorState.xStart, m_ScissorState.yStart, m_ScissorState.width, m_ScissorState.height);
		}
	}
}

