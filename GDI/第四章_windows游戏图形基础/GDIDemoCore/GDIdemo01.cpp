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
//��������
HPEN g_hPen[7] = { 0 };
//��ˢ����
HBRUSH g_hBrush[7] = { 0 };
//������ʽ����
int g_iPenStyle[7] = { PS_SOLID, PS_DASH, PS_DOT, PS_DASHDOT,PS_DASHDOTDOT,PS_NULL, PS_INSIDEFRAME };
//��ˢ��ʽ����
int g_iBrushStyle[6] = { HS_VERTICAL, HS_HORIZONTAL, HS_CROSS, HS_DIAGCROSS, HS_FDIAGONAL, HS_BDIAGONAL };
LRESULT CALLBACK WndProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam);
bool Game_Init(HWND hwnd);
void Game_Paint(HWND hwnd);
bool Game_CleanUp(HWND hwnd);
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
			//�����ⰴ����Ϣת��Ϊ�ַ���Ϣ
			TranslateMessage(&msg);
			//�ַ�һ����Ϣ������
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
	//��ʼ�����������
	srand(static_cast<unsigned int>(time(NULL)));
	//�����ʼ�����ʺͱ�ˢ
	for (int i = 0; i < 7; i++)
	{
		//����һ�������ɫ�Ļ���
		g_hPen[i] = CreatePen(g_iPenStyle[i], 1, RGB(rand() % 256, rand() % 256, rand() % 256));
		if (i != 6)
		{
			//������Ӱ��ˢ
			g_hBrush[i] = CreateHatchBrush(g_iBrushStyle[i], RGB(rand() % 256, rand() % 256, rand() % 256));
		}
		else{
			//����ʵ�Ļ�ˢ
			g_hBrush[i] = CreateSolidBrush(RGB(rand() % 256, rand() % 256, rand() % 256));
		}
	}
	Game_Paint(hwnd);
	//�ͷ���Դ
	ReleaseDC(hwnd, g_hdc);
	return true;
}
void Game_Paint(HWND hwnd)
{
	//���Ʋ���
	int y = 0;
	for (int i = 0; i < 7; i++)
	{
		//�ƶ���ʼ����y����
		y = (i + 1) * 70;
		//ѡ�񻭱�
		SelectObject(g_hdc, g_hPen[i]);
		//�ƶ����
		MoveToEx(g_hdc, 30, y, NULL);
		//��(30,y)��(100,y)���߶�
		LineTo(g_hdc, 100, y);
	}

	int x1 = 120;//�ϱ�x
	int x2 = 190;//�±�x
	//��7�ֲ�ͬ�Ļ�ˢ������
	for (int i = 0; i < 7; i++)
	{
		SelectObject(g_hdc, g_hBrush[i]);
		//��(x1,70)->(x2,y)������
		Rectangle(g_hdc, x1, 70, x2, y);
		//�����������ƶ�
		x1 += 90;
		x2 += 90;
	}
}
bool Game_CleanUp(HWND hwnd)
{
	//�ͷ����л��ʻ�ˢ
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
