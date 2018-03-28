/*
#include <Windows.h>
#include <tchar.h>
#include <time.h>
#pragma comment(lib,"winmm.lib")
#define WINDOW_TITLE L"[����������Ϩ�����Ϸ��������]������Ŀ��"
const int WINDOWS_WIDTH = 800;
const int WINDOWS_HEIGHT = 600;
//�豸�������
HDC g_hdc = NULL;
bool Game_Init(HWND hwnd);
void Game_Paint(HWND hwnd);
bool Game_CleanUp(HWND hwnd);
LRESULT CALLBACK WndProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam);
int myyyymain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
	WNDCLASSEX wndClass = { 0 };
	//�ֽ���
	wndClass.cbSize = sizeof(WNDCLASSEX);
	//��ʽ
	wndClass.style = CS_HREDRAW | CS_VREDRAW;
	//���ڹ��̺���
	wndClass.lpfnWndProc = WndProc;
	//�����฽���ڴ�
	wndClass.cbClsExtra = 0;
	//���ڸ����ڴ�
	wndClass.cbWndExtra = 0;
	//�������ڹ��̺����ĳ����ʵ�����
	wndClass.hInstance = hInstance;
	//ͼ��
	wndClass.hIcon = (HICON)::LoadImage(NULL, L"icon.ico", IMAGE_ICON, 0, 0, LR_DEFAULTSIZE | LR_LOADFROMFILE);
	//�����
	wndClass.hCursor = LoadCursor(NULL, IDC_ARROW);
	//��ˢ���
	wndClass.hbrBackground = (HBRUSH)GetStockObject(WHITE_BRUSH);
	//�˵���Դ
	wndClass.lpszMenuName = 0;
	wndClass.lpszClassName = L"ForTheDreamOfGameDevelop";
	//ע�ᴰ��
	if (!RegisterClassEx(&wndClass))
	{
		return -1;
	}
	//��������
	HWND hwnd = CreateWindow(L"ForTheDreamOfGameDevelop", WINDOW_TITLE, WS_OVERLAPPEDWINDOW, CW_USEDEFAULT,
		CW_USEDEFAULT, WINDOWS_WIDTH, WINDOWS_HEIGHT, NULL, NULL, hInstance, NULL);
	//�ƶ�����ʾ�͸��´���
	MoveWindow(hwnd, 250, 80, WINDOWS_WIDTH, WINDOWS_HEIGHT, true);
	ShowWindow(hwnd, nCmdShow);
	UpdateWindow(hwnd);
	//��ʼ����Դ
	if (!Game_Init(hwnd))
	{
		//��ʾʧ��
		MessageBox(hwnd, L"��Դ��ʼ��ʧ��", L"��Ϣ����", 0);
		return false;
	}
	//��������
	//PlaySound(L"Born for This.wav", NULL, SND_FILENAME | SND_ASYNC | SND_LOOP);
	PlaySound(L"Born for This.wav", NULL, SND_FILENAME | SND_ASYNC | SND_LOOP);
	//ѭ��������Ϣ
	MSG msg = { 0 };
	while (msg.message != WM_QUIT)
	{
		if (PeekMessage(&msg, 0, 0, 0, PM_REMOVE))
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
	}
	//�˳���ע������
	UnregisterClass(L"ForTheDreamOfGameDevelop", wndClass.hInstance);
	return 0;
}
LRESULT CALLBACK WndProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	//�����¼������Ϣ�Ľṹ��
	PAINTSTRUCT paintStruct;
	switch (message)
	{
	case WM_PAINT:
		g_hdc = BeginPaint(hwnd, &paintStruct);
		//��ͼ
		Game_Paint(hwnd);
		EndPaint(hwnd, &paintStruct);
		//�ػ���Ϣ������¿ͻ�����ʾ
		ValidateRect(hwnd, NULL);
		break;
	case WM_KEYDOWN:
		//���̰�����Ϣ
		if (wParam == VK_ESCAPE)
		{
			DestroyWindow(hwnd);
		}
		break;
	case WM_DESTROY:
		Game_CleanUp(hwnd);
		//��ϵͳ�����и��߳�����ֹ����������ӦWM_DESTROY��Ϣ
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
	HFONT hFout = CreateFont(30, 0, 0, 0, 0, 0, 0, 0, GB2312_CHARSET, 0, 0, 0, 0, L"΢���ź�");
	//ѡ������
	SelectObject(g_hdc,hFout);
	//���ñ���
	SetBkMode(g_hdc, TRANSPARENT);
	//����
	wchar_t text1[] = L"RNG��С������";
	wchar_t text2[] = L"�����ջ����Ϊ��";
	wchar_t text3[] = L"------�������ǵķ�˿";
	//���text1
	SetTextColor(g_hdc, RGB(50,255,50));
	TextOut(g_hdc, 30, 150, text1, wcslen(text1));
	//���Text2
	SetTextColor(g_hdc, RGB(50, 50, 255));
	TextOut(g_hdc, 30, 200, text2, wcslen(text2));
	//���Text3
	SetTextColor(g_hdc, RGB(255, 150, 50));
	TextOut(g_hdc, 500, 250, text3, wcslen(text3));
	DeleteObject(hFout);
}
bool Game_CleanUp(HWND hwnd)
{
	return true;
}
int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
	myyyymain(hInstance, hPrevInstance, lpCmdLine, nCmdShow);
	return 0;
}
*/
