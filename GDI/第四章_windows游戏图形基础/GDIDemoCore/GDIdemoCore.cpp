#include <Windows.h>
#include <tchar.h>
/*
#define WINDOW_TITLE L"[至我们永不熄灭的游戏开发梦想]程序核心框架"
const int WINDOWS_WIDTH = 800;
const int WINDOWS_HEIGHT = 600;
//全局变量
//全局设备环境句柄
HDC g_hdc = NULL;
LRESULT CALLBACK WndProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam);
bool Game_Init(HWND hwnd);
void Game_Paint(HWND hwnd);
bool Game_CleanUp(HWND hwnd);
int myyymain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
	WNDCLASSEX wndClass = { 0 };
	//字节数
	wndClass.cbSize = sizeof(WNDCLASSEX);
	//样式
	wndClass.style = CS_HREDRAW | CS_VREDRAW;
	//窗口过程函数
	wndClass.lpfnWndProc = WndProc;
	//窗口类附加内存
	wndClass.cbClsExtra = 0;
	//窗口附加内存
	wndClass.cbWndExtra = 0;
	//包含窗口过程函数的程序的实例句柄
	wndClass.hInstance = hInstance;
	//图标
	wndClass.hIcon = (HICON)::LoadImage(NULL, L"icon.ico", IMAGE_ICON, 0, 0, LR_DEFAULTSIZE | LR_LOADFROMFILE);
	//光标句柄
	wndClass.hCursor = LoadCursor(NULL, IDC_ARROW);
	//画刷句柄
	wndClass.hbrBackground = (HBRUSH)GetStockObject(GRAY_BRUSH);
	//菜单资源
	wndClass.lpszMenuName = 0;
	wndClass.lpszClassName = L"ForTheDreamOfGameDevelop";
	//注册窗口
	if (!RegisterClassEx(&wndClass))
	{
		return -1;
	}
	//创建窗口
	HWND hwnd = CreateWindow(L"ForTheDreamOfGameDevelop", WINDOW_TITLE, WS_OVERLAPPEDWINDOW, CW_USEDEFAULT,
		CW_USEDEFAULT, WINDOWS_WIDTH, WINDOWS_HEIGHT, NULL, NULL, hInstance, NULL);
	//移动、显示和更新窗口
	MoveWindow(hwnd, 250, 80, WINDOWS_WIDTH, WINDOWS_HEIGHT, true);
	ShowWindow(hwnd, nCmdShow);
	UpdateWindow(hwnd);
	//初始化资源
	if (!Game_Init(hwnd))
	{
		//提示失败
		MessageBox(hwnd, L"资源初始化失败", L"消息窗口",0);
		return false;
	}
	//循环接收消息
	MSG msg = { 0 };
	while (msg.message != WM_QUIT)
	{
		if (PeekMessage(&msg, 0, 0, 0, PM_REMOVE))
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
	}
	//退出后注销窗口
	UnregisterClass(L"ForTheDreamOfGameDevelop", wndClass.hInstance);
	return 0;
}
LRESULT CALLBACK WndProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	//定义记录绘制信息的结构体
	PAINTSTRUCT paintStruct;
	switch (message)
	{
	case WM_PAINT:
		g_hdc = BeginPaint(hwnd, &paintStruct);
		//绘图
		Game_Paint(hwnd);
		EndPaint(hwnd, &paintStruct);
		//重绘信息，则更新客户端显示
		ValidateRect(hwnd, NULL);
		break;
	case WM_KEYDOWN:
		//键盘按下信息
		if (wParam == VK_ESCAPE)
		{
			DestroyWindow(hwnd);
		}
		break;
	case WM_DESTROY:
		Game_CleanUp(hwnd);
		//向系统表明有个线程有终止请求，用来相应WM_DESTROY信息
		PostQuitMessage(0);
	default:
		return DefWindowProc(hwnd, message, wParam, lParam);
	}
	return 0;

}
bool Game_Init(HWND hwnd)
{
	g_hdc = GetDC(hwnd);
	Game_Paint(hwnd);
	ReleaseDC(hwnd, g_hdc);
	return true;
}
void Game_Paint(HWND hwnd)
{

}
bool Game_CleanUp(HWND hwnd)
{
	return false;
}

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
	return myyymain(hInstance, hPrevInstance, lpCmdLine, nCmdShow);
}
*/
