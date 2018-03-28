//#include <Windows.h>
//#include <tchar.h>
//#include<string>
//using namespace std;
//bool Game_init(const HWND& hwnd);
//void Game_Paint(const HWND& hwnd);
//void Game_Update(const HWND& hwnd);
//bool Game_Exit(const HWND& hwnd);
//
//#define WINDOWS_TITLE L"WINCLASS";
//LRESULT CALLBACK WindowProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam)
//{
//	PAINTSTRUCT paintStruct;
//	switch (message)
//	{
//	case WM_TIMER:
//		MessageBox(hwnd, L"计时器消息", L"消息", MB_OKCANCEL|MB_DEFBUTTON1);
//	case WM_PAINT:
//		//绘制
//
//		break;
//	case WM_KEYDOWN:
//		//键盘信息
//		if (wParam == VK_ESCAPE)
//		{
//			DestroyWindow(hwnd);
//		}
//		break;
//	case WM_DESTROY:
//		Game_Exit(hwnd);
//		PostQuitMessage(0);
//		return 0;
//	default:
//		return (DefWindowProc(hwnd, message, wParam, lParam));
//		break;
//	}
//	return 0;
//};
//
//inline bool initWndclass(WNDCLASSEX& wndclass, const HINSTANCE& hInstance,HWND& hwnd,const int &nCmdShow)
//{
//	wndclass.cbSize = sizeof(WNDCLASSEX);
//	wndclass.style = CS_VREDRAW | CS_HREDRAW | CS_OWNDC | CS_DBLCLKS;
//	wndclass.lpfnWndProc = WindowProc;
//	wndclass.cbClsExtra = 0;
//	wndclass.cbWndExtra = 0;
//	wndclass.hInstance = hInstance;
//	wndclass.hIcon = LoadIcon(NULL, IDI_APPLICATION);
//	wndclass.hCursor = LoadCursor(NULL, IDC_ARROW);
//	wndclass.hbrBackground = (HBRUSH)GetStockObject(WHITE_BRUSH);
//	wndclass.lpszMenuName = NULL;
//	wndclass.lpszClassName = WINDOWS_TITLE;
//
//	RegisterClassEx(&wndclass);
//	hwnd=CreateWindowEx(NULL, wndclass.lpszClassName,L"Windows应用程序",WS_OVERLAPPEDWINDOW|WS_VISIBLE
//		,0,0,800,600,NULL,NULL,wndclass.hInstance,NULL);
//	ShowWindow(hwnd, nCmdShow);
//	UpdateWindow(hwnd);
//	if (!Game_init(hwnd))
//	{
//		MessageBox(hwnd, L"资源初始化失败", L"error", 0);
//		return 0;
//	}
//	return 1;
//}
//int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
//{
//	WNDCLASSEX wndclass = { 0 };
//	HWND hwnd;
//	if (!(initWndclass(wndclass, hInstance, hwnd, nCmdShow)))
//	{
//		return -1;
//	}
//	MessageBox(hwnd, L"计时器消息", L"消息",0);
//	MSG msg = { 0 };
//	while (msg.message!=WM_QUIT)
//	{
//		if (PeekMessage(&msg,NULL,0,0,PM_REMOVE))
//		{
//			TranslateMessage(&msg);
//			DispatchMessage(&msg);
//		}
//		//处理游戏主循环
//		Game_Update(hwnd);
//	}
//	//注销窗口
//	UnregisterClass(wndclass.lpszClassName, wndclass.hInstance);
//	return 0;
//}
//bool Game_init(const HWND& hwnd)
//{
//	SetTimer(hwnd, 1, 1000, NULL);
//	Game_Paint(hwnd);
//	return true;
//}
//void Game_Paint(const HWND& hwnd)
//{
//
//}
//void Game_Update(const HWND& hwnd)
//{
//
//}
//bool Game_Exit(const HWND& hwnd)
//{
//	//清理资源...
//	KillTimer(hwnd,1);
//	return true;
//}