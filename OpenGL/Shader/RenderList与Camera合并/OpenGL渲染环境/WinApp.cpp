#include "WinApp.h"
#include "SceneManager.h"
#include "Time.h"
static  LRESULT CALLBACK    WindowProc(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
#define GWL_USERDATA (-21)
	/**
	*   ʹ��this���ݣ���ȫ�ֺ�����ת��Ϊ��ĳ�Ա��������
	*/
	EngineBehavior *  pThis = (EngineBehavior *)GetWindowLong(hWnd, GWL_USERDATA);
	if (pThis)
	{
		return  pThis->SystemEvent(msg, wParam, lParam);
	}
	if (WM_CREATE == msg)
	{
		CREATESTRUCT*   pCreate = (CREATESTRUCT*)lParam;
		SetWindowLong(hWnd, GWL_USERDATA, (DWORD_PTR)pCreate->lpCreateParams);
	}
	return  DefWindowProc(hWnd, msg, wParam, lParam);
}

void EngineBehavior ::Awake(const HINSTANCE& hInstance)
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
	ShowWindow(m_Hwnd,SW_SHOW);
	UpdateWindow(m_Hwnd);
	SystemStart();
}
void EngineBehavior ::InitGL()
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
void EngineBehavior ::Start()
{

}
void EngineBehavior ::Update()
{

}
void EngineBehavior::Event(UINT message, WPARAM wParam, LPARAM lParam)
{

}
int EngineBehavior ::SystemEvent(UINT message, WPARAM wParam, LPARAM lParam)
{
	Event(message, wParam, lParam);
	return SceneManager::Event(message, wParam, lParam, m_Hwnd);
}

void EngineBehavior ::OnDestory()
{

}
void EngineBehavior::SystemStart()
{
	Start();
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
		SystemUpdate();
	}
	SystemDestory();
}
void EngineBehavior::SystemUpdate()
{
	//������Ϸ�߼�
	SceneManager::Update();
	//����֮�� �ѻ��õĳ��ֳ���
	SwapBuffers(m_Hdc);
}
void EngineBehavior::SystemDestory()
{
	if (m_Hdc != NULL)
	{
		DeleteObject(m_Hdc);
	}
	OnDestory();
}