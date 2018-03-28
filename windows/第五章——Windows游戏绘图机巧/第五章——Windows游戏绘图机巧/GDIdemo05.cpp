#include <Windows.h>
#include <tchar.h>
#include<string>
#pragma comment(lib,"winmm.lib")
#pragma comment(lib,"Msimg32.lib")
using namespace std;
bool Game_init(const HWND& hwnd);
void Game_Paint(const HWND& hwnd);
void Game_Update(const HWND& hwnd);
bool Game_Exit(const HWND& hwnd);

#define WINDOW_WIDTH	932							//为窗口宽度定义的宏，以方便在此处修改窗口宽度
#define WINDOW_HEIGHT	700							//为窗口高度定义的宏，以方便在此处修改窗口高度
#define WINDOW_TITLE	L"【致我们永不熄灭的游戏开发梦想】透明贴图两套体系之：透明遮罩法"	//为窗口标题定义的宏

HDC g_hdc = NULL, g_mdc = NULL;
HBITMAP g_hbitmap[3];
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
		if (wParam == VK_ESCAPE)
		{
			DestroyWindow(hwnd);
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

inline void initWndclass(WNDCLASSEX& wndclass, const HINSTANCE& hInstance,HWND& hwnd, const int &nCmdShow)
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
	MoveWindow(hwnd, 200, 20, WINDOW_WIDTH, WINDOW_HEIGHT, true);
	ShowWindow(hwnd, nCmdShow);
	UpdateWindow(hwnd);
	if (!Game_init(hwnd))
	{
		MessageBox(hwnd, L"资源初始化失败", L"ERR", 0);
	}
}
int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
	WNDCLASSEX wndclass = { 0 };
	HWND hwnd;
	initWndclass(wndclass, hInstance, hwnd, nCmdShow);
	MSG msg = { 0 };
	while (msg.message != WM_QUIT)
	{
		if (PeekMessage(&msg, NULL, 0, 0, PM_REMOVE))
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
		//处理游戏主循环
		Game_Update(hwnd);
	}
	//注销窗口
	UnregisterClass(wndclass.lpszClassName, wndclass.hInstance);
	return 0;
}
bool Game_init(const HWND& hwnd)
{
	g_hdc = GetDC(hwnd);
	//加载位图
	g_hbitmap[0] =static_cast<HBITMAP>(LoadImage(NULL, L"bg2.bmp", IMAGE_BITMAP, WINDOW_WIDTH, WINDOW_HEIGHT, LR_LOADFROMFILE));
	g_hbitmap[1] = static_cast<HBITMAP>(LoadImage(NULL, L"character12.bmp", IMAGE_BITMAP, 535, 650, LR_LOADFROMFILE));
	g_hbitmap[2] = static_cast<HBITMAP>(LoadImage(NULL, L"character22.bmp", IMAGE_BITMAP, 506, 650, LR_LOADFROMFILE));
	g_mdc = CreateCompatibleDC(g_hdc);
	Game_Paint(hwnd);
	ReleaseDC(hwnd, g_hdc);
	return true;

}
void Game_Paint(const HWND& hwnd)
{
	SelectObject(g_mdc, g_hbitmap[0]);
	BitBlt(g_hdc, 0, 0, WINDOW_WIDTH, WINDOW_HEIGHT, g_mdc, 0, 0, SRCCOPY);

	SelectObject(g_mdc, g_hbitmap[1]);
	TransparentBlt(g_hdc, 0, WINDOW_HEIGHT - 650, 535, 650, g_mdc, 0, 0, 535, 650, RGB(0, 0, 0));

	SelectObject(g_mdc, g_hbitmap[2]);
	TransparentBlt(g_hdc, 500, WINDOW_HEIGHT - 650, 506, 650, g_mdc, 0, 0, 506, 650, RGB(0, 0, 0));

}
void Game_Update(const HWND& hwnd)
{

}
bool Game_Exit(const HWND& hwnd)
{
	for (int i = 0; i < 3; i++)
	{
		DeleteObject(g_hbitmap[i]);
	}
	DeleteDC(g_mdc);
	return true;
}
