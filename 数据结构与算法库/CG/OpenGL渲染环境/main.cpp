#include "ggl.h"
#include "Scene.h"
#pragma comment(lib,"opengl32.lib")
#pragma comment(lib,"glu32.lib")
void Update();
bool Game_Exit();
/************************************************************************/
/* 全局变量声明                                                              */
/************************************************************************/
unsigned g_frame = 100;                   //每帧间隔时间
DWORD g_tPre = 0, g_tNow = 0;             //记录当前时间和上一次绘图时间
HDC g_hdc;

void ShowErrMessage(const HWND & hwnd)
{
	MessageBox(hwnd, L"资源初始化失败", L"ERR", 0);
}
LRESULT CALLBACK WindowProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	switch (message)
	{
	case WM_PAINT:
		//绘制
		Draw();
		break;
	case WM_KEYDOWN:
		//键盘信息
		switch (wParam)
		{
		case '2':
			SwitchTo2D();
			break;
		case '3':
			SwitchTo3D();
			break;
		default:
			break;
		}
		if (wParam == VK_ESCAPE)
		{
			DestroyWindow(hwnd);
		}
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
		//获取当前时间
		g_tNow = GetTickCount();
		//如果此次循环运行时间和上次相差一个帧的时间
		if (g_tNow - g_tPre >= g_frame)
		{
			//处理游戏逻辑
			Update();
			Draw();
			//画完之后 把画好的呈现出来
			SwapBuffers(g_hdc);
			g_tPre = GetTickCount();
		}
	}
	//注销窗口
	UnregisterClass(wndclass.lpszClassName, wndclass.hInstance);
	return 0;
}
void Update()
{
	
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