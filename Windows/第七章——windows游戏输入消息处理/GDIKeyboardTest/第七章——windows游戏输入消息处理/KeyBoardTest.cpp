#include <Windows.h>
#include <tchar.h>
#include<string>
#pragma comment(lib,"winmm.lib")
#pragma comment(lib,"msimg32.lib")
using namespace std;
bool Game_init(const HWND& hwnd);
void Game_Paint(const HWND& hwnd);
void Game_Update(const HWND& hwnd);
bool Game_Exit(const HWND& hwnd);

#define WINDOW_WIDTH	932							//为窗口宽度定义的宏，以方便在此处修改窗口宽度
#define WINDOW_HEIGHT	700							//为窗口高度定义的宏，以方便在此处修改窗口高度
#define WINDOW_TITLE	L""	//为窗口标题定义的宏
HDC g_hdc = NULL, g_mdc = NULL,g_bufdc=NULL;
//每帧间隔时间
unsigned g_frame = 100;
//记录当前时间和上一次绘图时间
DWORD g_tPre = 0, g_tNow = 0;

LRESULT CALLBACK WindowProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	PAINTSTRUCT paintStruct;
	switch (message)
	{
	case WM_PAINT:
		//绘制
		g_hdc = BeginPaint(hwnd, &paintStruct);
		Game_Paint(hwnd);
		EndPaint(hwnd, &paintStruct);
		ValidateRect(hwnd, NULL);
		break;
	case WM_KEYDOWN:
		//键盘信息
		switch (wParam)
		{
		case VK_ESCAPE:
			DestroyWindow(hwnd);
			PostQuitMessage(0);
			break;
		case VK_UP:
			MessageBox(hwnd, L"你按了向上键", L"MessageBox", MB_OK);
			break;
		}
		
		break;
	case WM_DESTROY:
		Game_Exit(hwnd);
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
	wndclass.hbrBackground = (HBRUSH)GetStockObject(WHITE_BRUSH);
	wndclass.lpszMenuName = NULL;
	wndclass.lpszClassName = L"WINCLASS";

	RegisterClassEx(&wndclass);
	hwnd = CreateWindowEx(NULL, wndclass.lpszClassName, WINDOW_TITLE, WS_OVERLAPPEDWINDOW | WS_VISIBLE
		, 0, 0, WINDOW_WIDTH, WINDOW_HEIGHT, NULL, NULL, wndclass.hInstance, NULL);
	if (hwnd == NULL)
	{
		return false;
	}
	MoveWindow(hwnd, 200, 20, WINDOW_WIDTH, WINDOW_HEIGHT, true);
    ShowWindow(hwnd, nCmdShow);
	UpdateWindow(hwnd);
	if (!Game_init(hwnd))
	{
		return false;
	}
	return true;
}
int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
	WNDCLASSEX wndclass = { 0 };
	HWND hwnd;
	if (!initWndclass(wndclass, hInstance, hwnd, nCmdShow))
	{
		MessageBox(hwnd, L"资源初始化失败", L"ERR", 0);
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
		else{
			//获取当前时间
			g_tNow = GetTickCount();
			//如果此次循环运行时间和上次相差一个帧的时间
			if (g_tNow - g_tPre >= g_frame)
			{
				//处理游戏逻辑
				Game_Update(hwnd);
				Game_Paint(hwnd);
			}
		}
	}
	//注销窗口
	UnregisterClass(wndclass.lpszClassName, wndclass.hInstance);
	return 0;
}
bool Game_init(const HWND& hwnd)
{
	g_hdc = GetDC(hwnd);
	if (g_hdc == NULL)
	{
		return false;
	}
	g_mdc = CreateCompatibleDC(g_hdc);
	if (g_mdc == NULL)
	{
		return false;
	}
	g_bufdc = CreateCompatibleDC(g_hdc);
	if (g_bufdc == NULL)
	{
		return false;
	}
    
    Game_Paint(hwnd);
	return true;
}
//游戏渲染
void Game_Paint(const HWND& hwnd)
{

}
//游戏逻辑
void Game_Update(const HWND& hwnd)
{

}
bool Game_Exit(const HWND& hwnd)
{
	//释放资源对象

	if (g_mdc != NULL)
	{
		DeleteDC(g_mdc);
	}
	if (g_hdc != NULL)
	{
		ReleaseDC(hwnd, g_hdc);
	}
	if (g_bufdc != NULL)
	{
		ReleaseDC(hwnd, g_hdc);
	}
	return true;
}