#include "WinApp.h"
#include "SceneManager.h"
#include "Time.h"
static  LRESULT CALLBACK    WindowProc(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
#define GWL_USERDATA (-21)
	/**
	*   使用this数据，将全局函数，转化为类的成员函数调用
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
	//创建OpenGL渲染上下文
	m_Hdc = GetDC(m_Hwnd);
	//像素格式描述符
	PIXELFORMATDESCRIPTOR pfd;
	memset(&pfd, 0, sizeof(PIXELFORMATDESCRIPTOR));
	pfd.nVersion = 1;
	pfd.nSize = sizeof(PIXELFORMATDESCRIPTOR);
	pfd.cColorBits = 32;//颜色缓冲区每个像素占用32bit
	pfd.cDepthBits = 24;//深度缓冲区
	pfd.cStencilBits = 8;//蒙版缓冲区
	pfd.iPixelType = PFD_TYPE_RGBA;
	pfd.iLayerType = PFD_MAIN_PLANE;
	pfd.dwFlags = PFD_DRAW_TO_WINDOW | PFD_SUPPORT_OPENGL | PFD_DOUBLEBUFFER;
	//选一个像素格式
	int pixelFormat = ChoosePixelFormat(m_Hdc, &pfd);
	SetPixelFormat(m_Hdc, pixelFormat, &pfd);
	HGLRC rc = wglCreateContext(m_Hdc);
	if (rc == NULL)
	{
		return;
	}
	wglMakeCurrent(m_Hdc, rc);           //设置OpenGL设备环境
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
		//如果有消息的处理消息
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
	//处理游戏逻辑
	SceneManager::Update();
	//画完之后 把画好的呈现出来
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