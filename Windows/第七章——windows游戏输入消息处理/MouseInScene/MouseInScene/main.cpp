#include "Tools.h"
#include <tchar.h>
#include<string>
#pragma comment(lib,"winmm.lib")
#pragma comment(lib,"msimg32.lib")
using namespace std;
bool Game_init(const HWND& hwnd);
void Game_Paint(const HWND& hwnd);
void Game_Update(const HWND& hwnd);
bool Game_Exit(const HWND& hwnd);

#define WINDOW_WIDTH	932							//Ϊ���ڿ�ȶ���ĺ꣬�Է����ڴ˴��޸Ĵ��ڿ��
#define WINDOW_HEIGHT	700							//Ϊ���ڸ߶ȶ���ĺ꣬�Է����ڴ˴��޸Ĵ��ڸ߶�
#define WINDOW_TITLE	L""	//Ϊ���ڱ��ⶨ��ĺ�
HDC g_hdc = NULL, g_mdc = NULL,g_bufdc=NULL;
//ÿ֡���ʱ��
unsigned g_frame = 100;
//��¼��ǰʱ�����һ�λ�ͼʱ��
DWORD g_tPre = 0, g_tNow = 0;

LRESULT CALLBACK WindowProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	PAINTSTRUCT paintStruct;
	switch (message)
	{
	case WM_PAINT:
		//����
		g_hdc = BeginPaint(hwnd, &paintStruct);
		Game_Paint(hwnd);
		EndPaint(hwnd, &paintStruct);
		ValidateRect(hwnd, NULL);
		break;
	case WM_KEYDOWN:
		//������Ϣ
		switch (wParam)
		{
		case VK_ESCAPE:
			DestroyWindow(hwnd);
			PostQuitMessage(0);
			break;
		case VK_UP:

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
		MessageBox(hwnd, L"��Դ��ʼ��ʧ��", L"ERR", 0);
		return -1;
	}
	MSG msg = { 0 };
	while (msg.message != WM_QUIT)
	{
		//�������Ϣ�Ĵ�����Ϣ
		if (PeekMessage(&msg, NULL, 0, 0, PM_REMOVE))
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
		else{
			//��ȡ��ǰʱ��
			g_tNow = GetTickCount();
			//����˴�ѭ������ʱ����ϴ����һ��֡��ʱ��
			if (g_tNow - g_tPre >= g_frame)
			{
				//������Ϸ�߼�
				Game_Update(hwnd);
				Game_Paint(hwnd);
			}
		}
	}
	//ע������
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
	LitmitCursor(hwnd);
    Game_Paint(hwnd);
	return true;
}
//��Ϸ��Ⱦ
void Game_Paint(const HWND& hwnd)
{

}
//��Ϸ�߼�
void Game_Update(const HWND& hwnd)
{

}
bool Game_Exit(const HWND& hwnd)
{
	//�ͷ���Դ����

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