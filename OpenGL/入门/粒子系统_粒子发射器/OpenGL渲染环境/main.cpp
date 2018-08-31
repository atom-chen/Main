#include "ggl.h"
#include "Scene.h"
#pragma comment(lib,"opengl32.lib")
#pragma comment(lib,"glu32.lib")
#pragma comment(lib,"winmm.lib")
#pragma comment(lib,"SOIL.lib")

void Update();
bool Game_Exit();
/************************************************************************/
/* 全局变量声明                                                              */
/************************************************************************/

HDC g_hdc;

POINT m_OriginalPos;//记录按下鼠标时的位置
bool m_IsRotate = false;//是否正在旋转

void ShowErrMessage(const HWND & hwnd)
{
	MessageBox(hwnd, L"资源初始化失败", L"ERR", 0);
}
LRESULT CALLBACK WindowProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	switch (message)
	{
	case WM_KEYDOWN:
		OnKeyDown(wParam);
		break;
	case WM_KEYUP:
		OnKeyUp(wParam);
		break;
	case WM_MOUSEMOVE:
		if (m_IsRotate)
		{
			//记录点击后的偏移坐标
			POINT currentPos;
			currentPos.x = LOWORD(lParam);
			currentPos.y= HIWORD(lParam);
			ClientToScreen(hwnd, &currentPos);
			//偏移坐标=现在坐标-点击时候的坐标
			int deltaX = currentPos.x - m_OriginalPos.x;
			int deltaY = currentPos.y - m_OriginalPos.y;
			OnMouseMove(deltaX, deltaY);//处理消息
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
	case WM_PAINT:
		//绘制
		Draw();
		break;
	case WM_DESTROY:
		Game_Exit();
		PostQuitMessage(0);
		return 0;
	default:
		return (DefWindowProc(hwnd, message, wParam, lParam));
		break;
	}
	return 0;
};

inline bool initWndclass(WNDCLASSEX& wndclass, const HINSTANCE& hInstance, HWND& hwnd, const int &nCmdShow)
{
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
	hwnd = CreateWindowEx(NULL, wndclass.lpszClassName,WINDOW_TITLE, WS_OVERLAPPEDWINDOW | WS_VISIBLE
		, 100, 100, WINDOW_WIDTH, WINDOW_HEIGHT, NULL, NULL, wndclass.hInstance, NULL);
	if (hwnd == NULL)
	{
		return false;
	}
	//创建OpenGL渲染上下文
	g_hdc = GetDC(hwnd);
	//像素格式描述符
	PIXELFORMATDESCRIPTOR pfd;
	memset(&pfd, 0, sizeof(PIXELFORMATDESCRIPTOR));
	pfd.nVersion = 1;
	pfd.nSize = sizeof(PIXELFORMATDESCRIPTOR);
	pfd.cColorBits = 32;
	pfd.cDepthBits = 24;
	pfd.cStencilBits = 8;
	pfd.iPixelType = PFD_TYPE_RGBA;
	pfd.iLayerType = PFD_MAIN_PLANE;
	pfd.dwFlags = PFD_DRAW_TO_WINDOW | PFD_SUPPORT_OPENGL | PFD_DOUBLEBUFFER;
	//选一个像素格式
	int pixelFormat = ChoosePixelFormat(g_hdc, &pfd);
	SetPixelFormat(g_hdc, pixelFormat, &pfd);
	HGLRC rc = wglCreateContext(g_hdc);
	if (rc == NULL)
	{
		return false;
	}
	wglMakeCurrent(g_hdc, rc);           //设置OpenGL设备环境
	if (!Init())
	{
		return false;
	}
	MoveWindow(hwnd, 200, 20, WINDOW_WIDTH, WINDOW_HEIGHT, true);
	ShowWindow(hwnd, nCmdShow);
	UpdateWindow(hwnd);
	return true;
}
int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
	WNDCLASSEX wndclass = { 0 };
	HWND hwnd;
	if (!initWndclass(wndclass, hInstance, hwnd, nCmdShow))
	{
		ShowErrMessage(hwnd);
		return -1;
	}
	MSG msg = { 0 };
	while (msg.message != WM_QUIT)
	{
		//如果有消息的处理消息
		if (PeekMessage(&msg, NULL, 0, 0, PM_REMOVE))
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
		//如果此次循环运行时间和上次相差一个帧的时间
		//处理游戏逻辑
		Update();
		Draw();
		//画完之后 把画好的呈现出来
		SwapBuffers(g_hdc);
	}
	//注销窗口
	UnregisterClass(wndclass.lpszClassName, wndclass.hInstance);
	return 0;
}

bool Game_Exit()
{
	//释放资源对象
	if (g_hdc != NULL)
	{
		DeleteDC(g_hdc);
	}
	return true;
}