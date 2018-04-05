#include "WinApp.h"
static  LRESULT CALLBACK    WindowProc(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
#define GWL_USERDATA (-21)
	/**
	*   ʹ��this���ݣ���ȫ�ֺ�����ת��Ϊ��ĳ�Ա��������
	*/
	WinApp*  pThis = (WinApp*)GetWindowLong(hWnd, GWL_USERDATA);
	if (pThis)
	{
		return  pThis->Event(msg, wParam, lParam);
	}
	if (WM_CREATE == msg)
	{
		CREATESTRUCT*   pCreate = (CREATESTRUCT*)lParam;
		SetWindowLong(hWnd, GWL_USERDATA, (DWORD_PTR)pCreate->lpCreateParams);
	}
	return  DefWindowProc(hWnd, msg, wParam, lParam);
}

void WinApp::Awake(const HINSTANCE& hInstance)
{
	m_Width = WINDOW_WIDTH;
	m_Height = WINDOW_HEIGHT;
	m_HInstance = hInstance;
	wndclass.cbSize = sizeof(WNDCLASSEX);
	wndclass.style = CS_VREDRAW | CS_HREDRAW | CS_OWNDC | CS_DBLCLKS;
	wndclass.lpfnWndProc = WindowProc;
	wndclass.cbClsExtra = 0;
	wndclass.cbWndExtra = 0;
	wndclass.hInstance = hInstance;
	wndclass.hIcon = LoadIcon(NULL, IDI_APPLICATION);
	wndclass.hCursor = LoadCursor(NULL, IDC_ARROW);
	wndclass.hbrBackground = NULL;
	wndclass.lpszMenuName = NULL;
	wndclass.lpszClassName = WINDOW_TITLE;
	RegisterClassEx(&wndclass);
	m_Hwnd = CreateWindowEx(NULL, wndclass.lpszClassName, WINDOW_TITLE, WS_OVERLAPPEDWINDOW | WS_VISIBLE
		, CW_USEDEFAULT, CW_USEDEFAULT, m_Width, m_Height, NULL, NULL, wndclass.hInstance, this);
	if (m_Hwnd == NULL)
	{
		return;
	}

	InitGL();

	m_CurScene = new Scene;
	if (!m_CurScene->Awake())
	{
		return;
	}
	ShowWindow(m_Hwnd,SW_SHOW);
	UpdateWindow(m_Hwnd);
	Start();
}
void WinApp::InitGL()
{
	//����OpenGL��Ⱦ������
	m_Hdc = GetDC(m_Hwnd);
	//���ظ�ʽ������
	PIXELFORMATDESCRIPTOR pfd;
	memset(&pfd, 0, sizeof(PIXELFORMATDESCRIPTOR));
	pfd.nVersion = 1;
	pfd.nSize = sizeof(PIXELFORMATDESCRIPTOR);
	pfd.cColorBits = 32;//��ɫ������ÿ������ռ��32bit
	pfd.cDepthBits = 24;//��Ȼ�����
	pfd.cStencilBits = 8;//�ɰ滺����
	pfd.iPixelType = PFD_TYPE_RGBA;
	pfd.iLayerType = PFD_MAIN_PLANE;
	pfd.dwFlags = PFD_DRAW_TO_WINDOW | PFD_SUPPORT_OPENGL | PFD_DOUBLEBUFFER;
	//ѡһ�����ظ�ʽ
	int pixelFormat = ChoosePixelFormat(m_Hdc, &pfd);
	SetPixelFormat(m_Hdc, pixelFormat, &pfd);
	HGLRC rc = wglCreateContext(m_Hdc);
	if (rc == NULL)
	{
		return;
	}
	wglMakeCurrent(m_Hdc, rc);           //����OpenGL�豸����
}
void WinApp::Start()
{
	m_CurScene->Start();
	MSG msg = { 0 };
	while (msg.message != WM_QUIT)
	{
		//�������Ϣ�Ĵ�����Ϣ
		if (PeekMessage(&msg, NULL, 0, 0, PM_REMOVE))
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
		Update();
	}
}
void WinApp::Update()
{
	//����˴�ѭ������ʱ����ϴ����һ��֡��ʱ��
	//������Ϸ�߼�
	m_CurScene->Update();
	m_CurScene->OnDrawBegin();
	m_CurScene->Draw3D();
	m_CurScene->Draw2D();
	m_CurScene->OnDrawOver();
	//����֮�� �ѻ��õĳ��ֳ���
	SwapBuffers(m_Hdc);
}

int WinApp::Event(UINT message, WPARAM wParam, LPARAM lParam)
{
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
			ClientToScreen(m_Hwnd, &currentPos);
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
		ClientToScreen(m_Hwnd, &m_OriginalPos);
		SetCapture(m_Hwnd);
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
		return (DefWindowProc(m_Hwnd, message, wParam, lParam));
		break;
	}
	return 0;
}

void WinApp::OnDestory()
{
	if (m_Hdc != NULL)
	{
		DeleteObject(m_Hdc);
	}
}