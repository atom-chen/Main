//#include <Windows.h>
//#include <tchar.h>
//#include<string>
//#pragma comment(lib,"winmm.lib")
//using namespace std;
//
//
//	bool Game_init(const HWND& hwnd);
//	void Game_Paint(const HWND& hwnd);
//	void Game_Update(const HWND& hwnd);
//	bool Game_Exit(const HWND& hwnd);
//
//#define WINDOW_WIDTH	932							//为窗口宽度定义的宏，以方便在此处修改窗口宽度
//#define WINDOW_HEIGHT	700							//为窗口高度定义的宏，以方便在此处修改窗口高度
//#define WINDOW_TITLE	L"计时器动画"	//为窗口标题定义的宏
//	HDC g_hdc = NULL, g_mdc = NULL;
//	HBITMAP g_hSprite[12];
//	unsigned g_iNum = 0;
//	LRESULT CALLBACK WindowProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam)
//	{
//		PAINTSTRUCT paintStruct;
//		switch (message)
//		{
//		case WM_TIMER:
//			Game_Paint(hwnd);
//			break;
//		case WM_PAINT:
//			//绘制
//			g_hdc = BeginPaint(hwnd, &paintStruct);
//			Game_Paint(hwnd);
//			EndPaint(hwnd, &paintStruct);
//			ValidateRect(hwnd, NULL);
//			break;
//		case WM_KEYDOWN:
//			//键盘信息
//			if (wParam == VK_ESCAPE)
//			{
//				DestroyWindow(hwnd);
//			}
//			break;
//		case WM_DESTROY:
//			Game_Exit(hwnd);
//			PostQuitMessage(0);
//			return 0;
//		default:
//			return (DefWindowProc(hwnd, message, wParam, lParam));
//			break;
//		}
//		return 0;
//	};
//
//	inline bool initWndclass(WNDCLASSEX& wndclass, const HINSTANCE& hInstance, HWND& hwnd, const int &nCmdShow)
//	{
//		wndclass.cbSize = sizeof(WNDCLASSEX);
//		wndclass.style = CS_VREDRAW | CS_HREDRAW | CS_OWNDC | CS_DBLCLKS;
//		wndclass.lpfnWndProc = WindowProc;
//		wndclass.cbClsExtra = 0;
//		wndclass.cbWndExtra = 0;
//		wndclass.hInstance = hInstance;
//		wndclass.hIcon = LoadIcon(NULL, IDI_APPLICATION);
//		wndclass.hCursor = LoadCursor(NULL, IDC_ARROW);
//		wndclass.hbrBackground = (HBRUSH)GetStockObject(WHITE_BRUSH);
//		wndclass.lpszMenuName = NULL;
//		wndclass.lpszClassName = L"WINCLASS";
//
//		RegisterClassEx(&wndclass);
//		hwnd = CreateWindowEx(NULL, wndclass.lpszClassName, WINDOW_TITLE, WS_OVERLAPPEDWINDOW | WS_VISIBLE
//			, 0, 0, WINDOW_WIDTH, WINDOW_HEIGHT, NULL, NULL, wndclass.hInstance, NULL);
//		if (hwnd == NULL)
//		{
//			return false;
//		}
//		MoveWindow(hwnd, 200, 20, WINDOW_WIDTH, WINDOW_HEIGHT, true);
//		ShowWindow(hwnd, nCmdShow);
//		UpdateWindow(hwnd);
//		if (!Game_init(hwnd))
//		{
//			return false;
//		}
//		return true;
//	}
//	int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
//	{
//		WNDCLASSEX wndclass = { 0 };
//		HWND hwnd;
//		if (!initWndclass(wndclass, hInstance, hwnd, nCmdShow))
//		{
//			MessageBox(hwnd, L"资源初始化失败", L"ERR", 0);
//			return -1;
//		}
//		MSG msg = { 0 };
//		while (msg.message != WM_QUIT)
//		{
//			if (PeekMessage(&msg, NULL, 0, 0, PM_REMOVE))
//			{
//				TranslateMessage(&msg);
//				DispatchMessage(&msg);
//			}
//			//处理游戏主循环
//			Game_Update(hwnd);
//		}
//		//注销窗口
//		UnregisterClass(wndclass.lpszClassName, wndclass.hInstance);
//		return 0;
//	}
//	bool Game_init(const HWND& hwnd)
//	{
//		g_hdc = GetDC(hwnd);
//		if (g_hdc == NULL)
//		{
//			return false;
//		}
//		g_mdc = CreateCompatibleDC(g_hdc);
//		if (g_mdc == NULL)
//		{
//			return false;
//		}
//		//存文件名的数组
//		wchar_t filename[20];
//		//加载位图
//		for (int i = 0; i < 11; i++)
//		{
//			//将filename里面的数据全部置为0
//			memset(filename, 0, sizeof(filename));
//			//填充文件名
//			swprintf_s(filename, L"%d.bmp", i);
//			//加载位图
//			g_hSprite[i] = (HBITMAP)LoadImage(NULL, filename, IMAGE_BITMAP, WINDOW_WIDTH, WINDOW_HEIGHT, LR_LOADFROMFILE);
//			if (g_hSprite[i] == NULL)
//			{
//				return false;
//			}
//		}
//		//设置计时器
//		SetTimer(hwnd, 1, 50, NULL);
//		return true;
//	}
//	//游戏渲染
//	void Game_Paint(const HWND& hwnd)
//	{
//		if (g_iNum >= 11)
//		{
//			g_iNum = 0;
//		}
//		SelectObject(g_mdc, g_hSprite[g_iNum]);
//		//绘图
//		BitBlt(g_hdc, 0, 0, WINDOW_WIDTH, WINDOW_HEIGHT, g_mdc, 0, 0, SRCCOPY);
//		g_iNum++;
//	}
//	//游戏逻辑
//	void Game_Update(const HWND& hwnd)
//	{
//
//	}
//	bool Game_Exit(const HWND& hwnd)
//	{
//		//关闭计时器
//		KillTimer(hwnd, 1);
//		//释放资源对象
//		for (int i = 0; i < 12; i++)
//		{
//			if (g_hSprite[i] != NULL)
//			{
//				DeleteObject(g_hSprite[i]);
//			}
//		}
//		if (g_mdc != NULL)
//		{
//			DeleteDC(g_mdc);
//		}
//		if (g_hdc != NULL)
//		{
//			ReleaseDC(hwnd, g_hdc);
//		}
//		return true;
//	}
