#include "SceneManager.h"


std::map<string, Scene*> SceneManager::m_mScene;
Scene* SceneManager::m_CurScene=nullptr;
POINT SceneManager::m_OriginalPos;//��¼�������ʱ��λ��
bool SceneManager::m_IsRotate=0;//�Ƿ�������ת

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
		if (m_CurScene != nullptr)
		{
			Scene* temp = m_CurScene;
			m_CurScene = nullptr;
			temp->OnDesrory();
		}
		it->second->Awake();
		it->second->Start();
		m_CurScene = it->second;
	}
}

void SceneManager::Update()
{
	if (m_CurScene != nullptr)
	{
		m_CurScene->Update();
		m_CurScene->OnDrawBegin();
		m_CurScene->Draw3D();
		m_CurScene->Draw2D();
		m_CurScene->OnDraw2DOver();
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
			int32_t deltaX = currentPos.x - m_OriginalPos.x;
			int32_t deltaY = currentPos.y - m_OriginalPos.y;
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