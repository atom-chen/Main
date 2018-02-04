#include <Windows.h>
#include <tchar.h>;
#define WINDOW_TITLE L"[至我们永不熄灭的游戏开发梦想]程序核心框架"
const int WINDOWS_WIDTH = 800;
const int WINDOWS_HEIGHT = 600;
LRESULT CALLBACK WndProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam);
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
	UnregisterClass(L"ForTheDreamOfGameDevelop",wndClass.hInstance);
	return 0;
}
LRESULT CALLBACK WndProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	switch (message)
	{
	case WM_PAINT:
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
	default:
		return DefWindowProc(hwnd, message, wParam, lParam);
	}
	return 0;

}
/*
int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
	return myyymain(hInstance, hPrevInstance, lpCmdLine, nCmdShow);
}
*/