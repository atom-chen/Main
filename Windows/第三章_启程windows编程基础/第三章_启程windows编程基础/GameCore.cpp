#include <Windows.h>
#include <tchar.h>;
#define WINDOW_TITLE L"[����������Ϩ�����Ϸ��������]������Ŀ��"
const int WINDOWS_WIDTH = 800;
const int WINDOWS_HEIGHT = 600;
LRESULT CALLBACK WndProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam);
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
	wndClass.hIcon = (HICON)::LoadImage(NULL, L"icon.ico", IMAGE_ICON, 0, 0, LR_DEFAULTSIZE | LR_LOADFROMFILE);
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
	UnregisterClass(L"ForTheDreamOfGameDevelop",wndClass.hInstance);
	return 0;
}
LRESULT CALLBACK WndProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	switch (message)
	{
	case WM_PAINT:
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
	default:
		return DefWindowProc(hwnd, message, wParam, lParam);
	}
	return 0;

}
/*
int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
	return myyymain(hInstance, hPrevInstance, lpCmdLine, nCmdShow);
}
*/