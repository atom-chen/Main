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
//#define WINDOW_WIDTH	932							//Ϊ���ڿ�ȶ���ĺ꣬�Է����ڴ˴��޸Ĵ��ڿ��
//#define WINDOW_HEIGHT	700							//Ϊ���ڸ߶ȶ���ĺ꣬�Է����ڴ˴��޸Ĵ��ڸ߶�
//#define WINDOW_TITLE	L"��ʱ������"	//Ϊ���ڱ��ⶨ��ĺ�
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
//			//����
//			g_hdc = BeginPaint(hwnd, &paintStruct);
//			Game_Paint(hwnd);
//			EndPaint(hwnd, &paintStruct);
//			ValidateRect(hwnd, NULL);
//			break;
//		case WM_KEYDOWN:
//			//������Ϣ
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
//			MessageBox(hwnd, L"��Դ��ʼ��ʧ��", L"ERR", 0);
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
//			//������Ϸ��ѭ��
//			Game_Update(hwnd);
//		}
//		//ע������
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
//		//���ļ���������
//		wchar_t filename[20];
//		//����λͼ
//		for (int i = 0; i < 11; i++)
//		{
//			//��filename���������ȫ����Ϊ0
//			memset(filename, 0, sizeof(filename));
//			//����ļ���
//			swprintf_s(filename, L"%d.bmp", i);
//			//����λͼ
//			g_hSprite[i] = (HBITMAP)LoadImage(NULL, filename, IMAGE_BITMAP, WINDOW_WIDTH, WINDOW_HEIGHT, LR_LOADFROMFILE);
//			if (g_hSprite[i] == NULL)
//			{
//				return false;
//			}
//		}
//		//���ü�ʱ��
//		SetTimer(hwnd, 1, 50, NULL);
//		return true;
//	}
//	//��Ϸ��Ⱦ
//	void Game_Paint(const HWND& hwnd)
//	{
//		if (g_iNum >= 11)
//		{
//			g_iNum = 0;
//		}
//		SelectObject(g_mdc, g_hSprite[g_iNum]);
//		//��ͼ
//		BitBlt(g_hdc, 0, 0, WINDOW_WIDTH, WINDOW_HEIGHT, g_mdc, 0, 0, SRCCOPY);
//		g_iNum++;
//	}
//	//��Ϸ�߼�
//	void Game_Update(const HWND& hwnd)
//	{
//
//	}
//	bool Game_Exit(const HWND& hwnd)
//	{
//		//�رռ�ʱ��
//		KillTimer(hwnd, 1);
//		//�ͷ���Դ����
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
