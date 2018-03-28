#include <Windows.h>
#include <tchar.h>
#include<string>
/*
using namespace std;
bool Game_init(const HWND& hwnd);
void Game_Paint(const HWND& hwnd);
void Game_Update(const HWND& hwnd);
bool Game_Exit(const HWND& hwnd);

#define WINDOW_WIDTH	932							//为窗口宽度定义的宏，以方便在此处修改窗口宽度
#define WINDOW_HEIGHT	700							//为窗口高度定义的宏，以方便在此处修改窗口高度
#define WINDOW_TITLE	L"【致我们永不熄灭的游戏开发梦想】透明贴图两套体系之：透明遮罩法"	//为窗口标题定义的宏
HDC g_hdc = NULL,g_mdc=NULL;
HBITMAP g_hBackGround[3];//3个位图
LRESULT CALLBACK WindowProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	PAINTSTRUCT paintStruct;
	switch (message)
	{
	case WM_PAINT:
		//绘制
		g_hdc = BeginPaint(hwnd, &paintStruct);
		Game_Paint(hwnd);
		EndPaint(hwnd,&paintStruct);
		ValidateRect(hwnd,NULL);
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

inline void initWndclass(WNDCLASSEX& wndclass, const HINSTANCE& hInstance,HWND& hwnd,const int &nCmdShow)
{
	wndclass.cbSize = sizeof(WNDCLASSEX);
	wndclass.style = CS_VREDRAW | CS_HREDRAW | CS_OWNDC | CS_DBLCLKS;
	wndclass.lpfnWndProc = WindowProc;
	wndclass.cbClsExtra = 0;
	wndclass.cbWndExtra = 0;
	wndclass.hInstance = hInstance;
	wndclass.hIcon = LoadIcon(NULL, IDI_APPLICATION);
	wndclass.hCursor - LoadCursor(NULL, IDC_ARROW);
	wndclass.hbrBackground = (HBRUSH)GetStockObject(WHITE_BRUSH);
	wndclass.lpszMenuName = NULL;
	wndclass.lpszClassName = L"WINCLASS";

	RegisterClassEx(&wndclass);
	hwnd = CreateWindowEx(NULL, wndclass.lpszClassName, WINDOW_TITLE, WS_OVERLAPPEDWINDOW | WS_VISIBLE
		,0,0,WINDOW_WIDTH,WINDOW_HEIGHT,NULL,NULL,wndclass.hInstance,NULL);
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
	initWndclass(wndclass, hInstance, hwnd,nCmdShow);
	MSG msg = { 0 };
	while (msg.message!=WM_QUIT)
	{
		if (PeekMessage(&msg,NULL,0,0,PM_REMOVE))
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
	g_hBackGround[0] =static_cast<HBITMAP>(LoadImage(NULL, L"bg.bmp",IMAGE_BITMAP ,WINDOW_WIDTH,
		WINDOW_HEIGHT, LR_LOADFROMFILE));
	g_hBackGround[1] = static_cast<HBITMAP>(LoadImage(NULL,L"character1.bmp",IMAGE_BITMAP,
		640,579,LR_LOADFROMFILE));
	g_hBackGround[2] = static_cast<HBITMAP>(LoadImage(NULL, L"character2.bmp", IMAGE_BITMAP,
		800, 584, LR_LOADFROMFILE));
	g_mdc = CreateCompatibleDC(g_hdc);

	Game_Paint(hwnd);
	ReleaseDC(hwnd, g_hdc);
	return true;
	
}
void Game_Paint(const HWND& hwnd)
{
	//贴上背景图
	SelectObject(g_mdc,g_hBackGround[0]);
	BitBlt(g_hdc, 0, 0, WINDOW_WIDTH, WINDOW_HEIGHT, g_mdc, 0, 0, SRCCOPY);
	//画第一个人物
	SelectObject(g_mdc, g_hBackGround[1]);
	//将屏蔽图与背景图做AND
	BitBlt(g_hdc, 50, WINDOW_HEIGHT - 579, 320, 640, g_mdc, 320, 0, SRCAND);
	//前景图与背景图做OR
	BitBlt(g_hdc, 50, WINDOW_HEIGHT - 579, 320, 640, g_mdc, 0, 0, SRCPAINT);
	
	//画第二个人物
	SelectObject(g_mdc, g_hBackGround[2]);
	BitBlt(g_hdc,450,WINDOW_HEIGHT-584,400,584,g_mdc,400,0,SRCAND);
	BitBlt(g_hdc, 450, WINDOW_HEIGHT - 584, 400, 584, g_mdc, 0, 0, SRCPAINT);
}
void Game_Update(const HWND& hwnd)
{

}
bool Game_Exit(const HWND& hwnd)
{
	//清理资源...
	for (int i = 0; i < 3; i++)
	{
		DeleteObject(g_hBackGround[i]);
	}
	DeleteDC(g_mdc);
	return true;
}
*/