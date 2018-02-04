/*
#include <Windows.h>
#include <tchar.h>
#include <time.h>
#pragma comment(lib,"winmm.lib")
#define WINDOW_TITLE L"[至我们永不熄灭的游戏开发梦想]程序核心框架"
const int WINDOWS_WIDTH = 800;
const int WINDOWS_HEIGHT = 600;
//设备环境句柄
HDC g_hdc = NULL;
//画笔数组
HPEN g_hPen[7] = { 0 };
//画刷数组
HBRUSH g_hBrush[7] = { 0 };
//画笔样式数组
int g_iPenStyle[7] = { PS_SOLID, PS_DASH, PS_DOT, PS_DASHDOT,PS_DASHDOTDOT,PS_NULL, PS_INSIDEFRAME };
//画刷样式数组
int g_iBrushStyle[6] = { HS_VERTICAL, HS_HORIZONTAL, HS_CROSS, HS_DIAGCROSS, HS_FDIAGONAL, HS_BDIAGONAL };
LRESULT CALLBACK WndProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam);
bool Game_Init(HWND hwnd);
void Game_Paint(HWND hwnd);
bool Game_CleanUp(HWND hwnd);
int myyyymain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
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
	wndClass.hbrBackground = (HBRUSH)GetStockObject(WHITE_BRUSH);
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
		MessageBox(hwnd, L"资源初始化失败", L"消息窗口", 0);
		return false;
	}
	//播放音乐
	//PlaySound(L"Born for This.wav", NULL, SND_FILENAME | SND_ASYNC | SND_LOOP);
	PlaySound(L"Born for This.wav", NULL, SND_FILENAME | SND_ASYNC | SND_LOOP);
	//循环接收消息
	MSG msg = { 0 };
	while (msg.message != WM_QUIT)
	{
		if (PeekMessage(&msg, 0, 0, 0, PM_REMOVE))
		{
			//将虚拟按键信息转化为字符信息
			TranslateMessage(&msg);
			//分发一个消息给窗口
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
	//初始化随机数种子
	srand(static_cast<unsigned int>(time(NULL)));
	//随机初始化画笔和笔刷
	for (int i = 0; i < 7; i++)
	{
		//创建一个随机颜色的画笔
		g_hPen[i] = CreatePen(g_iPenStyle[i], 1, RGB(rand() % 256, rand() % 256, rand() % 256));
		if (i != 6)
		{
			//创建阴影画刷
			g_hBrush[i] = CreateHatchBrush(g_iBrushStyle[i], RGB(rand() % 256, rand() % 256, rand() % 256));
		}
		else{
			//创建实心画刷
			g_hBrush[i] = CreateSolidBrush(RGB(rand() % 256, rand() % 256, rand() % 256));
		}
	}
	Game_Paint(hwnd);
	//释放资源
	ReleaseDC(hwnd, g_hdc);
	return true;
}
void Game_Paint(HWND hwnd)
{
	//绘制操作
	int y = 0;
	for (int i = 0; i < 7; i++)
	{
		//移动开始画的y坐标
		y = (i + 1) * 70;
		//选择画笔
		SelectObject(g_hdc, g_hPen[i]);
		//移动光标
		MoveToEx(g_hdc, 30, y, NULL);
		//从(30,y)到(100,y)画线段
		LineTo(g_hdc, 100, y);
	}

	int x1 = 120;//上边x
	int x2 = 190;//下边x
	//以7种不同的画刷填充矩形
	for (int i = 0; i < 7; i++)
	{
		SelectObject(g_hdc, g_hBrush[i]);
		//在(x1,70)->(x2,y)填充矩形
		Rectangle(g_hdc, x1, 70, x2, y);
		//矩形两条边移动
		x1 += 90;
		x2 += 90;
	}
}
bool Game_CleanUp(HWND hwnd)
{
	//释放所有画笔画刷
	for (int i = 0; i < 7; i++)
	{
		DeleteObject(g_hPen[i]);
		DeleteObject(g_hBrush[i]);
	}
	return true;
}
int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
	myyyymain(hInstance, hPrevInstance, lpCmdLine, nCmdShow);
	return 0;
}
*/
