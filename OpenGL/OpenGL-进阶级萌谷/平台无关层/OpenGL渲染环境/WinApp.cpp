#include "WinApp.h"
static  LRESULT CALLBACK    WindowProc(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
#define GWL_USERDATA (-21)
	/**
	*   使用this数据，将全局函数，转化为类的成员函数调用
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
void WinApp::Start()
{
	m_CurScene->Start();
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
	}
}
void WinApp::Update()
{
	//如果此次循环运行时间和上次相差一个帧的时间
	//处理游戏逻辑
	m_CurScene->Update();
	m_CurScene->OnDrawBegin();
	m_CurScene->Draw3D();
	m_CurScene->Draw2D();
	m_CurScene->OnDrawOver();
	//画完之后 把画好的呈现出来
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
			//记录点击后的偏移坐标
			POINT currentPos;
			currentPos.x = LOWORD(lParam);
			currentPos.y = HIWORD(lParam);
			ClientToScreen(m_Hwnd, &currentPos);
			//偏移坐标=现在坐标-点击时候的坐标
			int32_t deltaX = currentPos.x - m_OriginalPos.x;
			int32_t deltaY = currentPos.y - m_OriginalPos.y;
			m_CurScene->OnMouseMove((float)deltaX, (float)deltaY);//处理消息
			SetCursorPos(m_OriginalPos.x, m_OriginalPos.y);//复位，然后再计算偏移
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
		//绘制
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