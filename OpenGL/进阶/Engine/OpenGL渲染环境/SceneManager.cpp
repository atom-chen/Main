#include "SceneManager.h"
#include "Time.h"
#include "RenderOptions.h"


std::map<string, Scene*> SceneManager::m_mScene;
Scene* SceneManager::m_CurScene=nullptr;
POINT SceneManager::m_OriginalPos;//记录按下鼠标时的位置
bool SceneManager::m_IsRotate=0;//是否正在旋转
bool SceneManager::m_IsGlewInit = 0;//glew是否已经初始化

AlphaBlendInfo SceneManager::m_Blend;//是否开启alpha混合
bool SceneManager::m_IsDepthTest=0;//是否开启深度测试
ProgramPointSize SceneManager::m_IsProgramPointSize;//是否由程序控制点的大小
ScissorState SceneManager::m_ScissorState;//裁剪状态

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
		LoadScene(it->second);
	}
}
void SceneManager::LoadScene(int index)
{
	if (index < m_mScene.size())
	{
		auto it = m_mScene.begin();
		for (int i = 0; i < index; i++)
		{
			it++;
		}
		LoadScene(it->second);
	}
}
void SceneManager::LoadScene(Scene* scene)
{
	if (scene == nullptr)
	{
		return;
	}
	Scene* temp = m_CurScene;
	m_CurScene = nullptr;
	if (temp == nullptr)
	{
		InitGlew();
	}
	cout << endl;
	scene->SystemAwake();
	scene->Awake();
	if (temp != nullptr)
	{
		cout << endl;
		temp->OnDesrory();
		temp->SystemDesrory();
	}
	scene->SetViewPortSize(m_ViewWidth, m_ViewHeight);
	scene->SystemStart();
	scene->Start();
	m_CurScene = scene;
}
void SceneManager::ReSetScene()
{
	if (m_CurScene != nullptr)
	{
		m_CurScene->Start();
	}
}
void SceneManager::DrawCommit()
{
	if (m_CurScene != nullptr && m_CurScene->m_MainCamera != nullptr)
	{
		m_CurScene->SystemDraw3D();
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
			//记录点击后的偏移坐标
			POINT currentPos;
			currentPos.x = LOWORD(lParam);
			currentPos.y = HIWORD(lParam);
			ClientToScreen(hwnd, &currentPos);
			//偏移坐标=现在坐标-点击时候的坐标
			int deltaX = currentPos.x - m_OriginalPos.x;
			int deltaY = currentPos.y - m_OriginalPos.y;
			m_CurScene->OnMouseMove((float)deltaX, (float)deltaY);//处理消息
			SetCursorPos(m_OriginalPos.x, m_OriginalPos.y);//复位，然后再计算偏移
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
		//绘制
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
	m_CurScene->InsertToRenderList(render);
}
void SceneManager::DrawGameObject(const RenderDomain& render )
{
	m_CurScene->InsertToRenderList(render);
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
		glEnable(GL_BLEND);//alpha混合
		glBlendFunc(GL_SRC_ALPHA, info.Type);
	}
	else
	{
		glDisable(GL_BLEND);//alpha混合
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



void SceneManager::SetProgramPointSizeState(bool isEnable)
{
	if (m_IsProgramPointSize.isProgramPointSize == isEnable)
	{
		return;
	}
	m_IsProgramPointSize.isProgramPointSize = isEnable;
	if (m_IsProgramPointSize.isProgramPointSize)
	{
		glEnable(GL_POINT_SPRITE);
		glEnable(GL_PROGRAM_POINT_SIZE);//点的大小由程序控制
	}
	else
	{
		glDisable(GL_POINT_SPRITE);
		glDisable(GL_PROGRAM_POINT_SIZE);//点的大小由程序控制
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

