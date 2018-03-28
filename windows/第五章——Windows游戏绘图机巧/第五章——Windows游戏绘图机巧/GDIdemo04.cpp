#include <Windows.h>
#include <tchar.h>
#include<string>
/*
using namespace std;
bool Game_init(const HWND& hwnd);
void Game_Paint(const HWND& hwnd);
void Game_Update(const HWND& hwnd);
bool Game_Exit(const HWND& hwnd);

#define WINDOW_WIDTH	932							//Ϊ���ڿ�ȶ���ĺ꣬�Է����ڴ˴��޸Ĵ��ڿ��
#define WINDOW_HEIGHT	700							//Ϊ���ڸ߶ȶ���ĺ꣬�Է����ڴ˴��޸Ĵ��ڸ߶�
#define WINDOW_TITLE	L"������������Ϩ�����Ϸ�������롿͸����ͼ������ϵ֮��͸�����ַ�"	//Ϊ���ڱ��ⶨ��ĺ�
HDC g_hdc = NULL,g_mdc=NULL;
HBITMAP g_hBackGround[3];//3��λͼ
LRESULT CALLBACK WindowProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	PAINTSTRUCT paintStruct;
	switch (message)
	{
	case WM_PAINT:
		//����
		g_hdc = BeginPaint(hwnd, &paintStruct);
		Game_Paint(hwnd);
		EndPaint(hwnd,&paintStruct);
		ValidateRect(hwnd,NULL);
		break;
	case WM_KEYDOWN:
		//������Ϣ
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
		MessageBox(hwnd, L"��Դ��ʼ��ʧ��", L"ERR", 0);
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
		//������Ϸ��ѭ��
		Game_Update(hwnd);
	}
	//ע������
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
	//���ϱ���ͼ
	SelectObject(g_mdc,g_hBackGround[0]);
	BitBlt(g_hdc, 0, 0, WINDOW_WIDTH, WINDOW_HEIGHT, g_mdc, 0, 0, SRCCOPY);
	//����һ������
	SelectObject(g_mdc, g_hBackGround[1]);
	//������ͼ�뱳��ͼ��AND
	BitBlt(g_hdc, 50, WINDOW_HEIGHT - 579, 320, 640, g_mdc, 320, 0, SRCAND);
	//ǰ��ͼ�뱳��ͼ��OR
	BitBlt(g_hdc, 50, WINDOW_HEIGHT - 579, 320, 640, g_mdc, 0, 0, SRCPAINT);
	
	//���ڶ�������
	SelectObject(g_mdc, g_hBackGround[2]);
	BitBlt(g_hdc,450,WINDOW_HEIGHT-584,400,584,g_mdc,400,0,SRCAND);
	BitBlt(g_hdc, 450, WINDOW_HEIGHT - 584, 400, 584, g_mdc, 0, 0, SRCPAINT);
}
void Game_Update(const HWND& hwnd)
{

}
bool Game_Exit(const HWND& hwnd)
{
	//������Դ...
	for (int i = 0; i < 3; i++)
	{
		DeleteObject(g_hBackGround[i]);
	}
	DeleteDC(g_mdc);
	return true;
}
*/