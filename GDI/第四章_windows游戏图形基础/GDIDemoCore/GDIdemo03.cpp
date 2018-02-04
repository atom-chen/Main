#include <Windows.h>
#include <tchar.h>

#define WINDOW_TITLE L"GDIλͼʾ������"
const int WINDOWS_WIDTH = 800;
const int WINDOWS_HEIGHT = 600;
//ȫ�ֱ���
//ȫ���豸�������
HDC g_hdc = NULL,g_mdc=NULL;
//λͼ���
HBITMAP g_hbitmap = NULL;
LRESULT CALLBACK WndProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam);
bool Game_Init(HWND hwnd);
void Game_Paint(HWND hwnd);
bool Game_CleanUp(HWND hwnd);
int myyymain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
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
	//wndClass.hIcon = (HICON)::LoadImage(NULL, L"icon.ico", IMAGE_ICON, 0, 0, LR_DEFAULTSIZE | LR_LOADFROMFILE);
	wndClass.hIcon = NULL;
	//�����
	wndClass.hCursor = LoadCursor(NULL, IDC_ARROW);
	//��ˢ���
	wndClass.hbrBackground = (HBRUSH)GetStockObject(GRAY_BRUSH);
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
//���ڹ��̺���
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
	//��ȡ�豸����DC
	g_hdc = GetDC(hwnd);
	//����λͼ
	g_hbitmap = static_cast<HBITMAP>(LoadImage(NULL, L"luna.bmp", IMAGE_BITMAP, WINDOWS_HEIGHT, WINDOWS_WIDTH, LR_LOADFROMFILE));
	//��������DC
	g_mdc = CreateCompatibleDC(g_hdc);
	Game_Paint(hwnd);
	ReleaseDC(hwnd, g_hdc);
	return true;
}
void Game_Paint(HWND hwnd)
{
	//ѡ��λͼ����
	SelectObject(g_mdc, g_hbitmap);
	//��ͼ����0,0����ͼ���ߴ�Ϊ800*600
	BitBlt(g_hdc, 0, 0, WINDOWS_WIDTH, WINDOWS_HEIGHT, g_mdc, 0, 0, SRCCOPY);

}
bool Game_CleanUp(HWND hwnd)
{
	DeleteObject(g_hbitmap);
	DeleteObject(g_mdc);
	return true;
}

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
	return myyymain(hInstance, hPrevInstance, lpCmdLine, nCmdShow);
}

