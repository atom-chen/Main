#include <Windows.h>
#include <tchar.h> 
//���ڹ��̺���
//LRESULT CALLBACK WNDPROC(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam);
void mymain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{

	//����
	WNDCLASSEX wndClass = { 0 };
	wndClass.cbSize = sizeof(WNDCLASSEX);
	wndClass.style = CS_HREDRAW | CS_VREDRAW;
	wndClass.lpfnWndProc = NULL;
	wndClass.cbClsExtra = 0;
	wndClass.cbWndExtra = 0;
	wndClass.hInstance = hInstance;
	wndClass.hIcon = NULL;
	wndClass.hCursor = LoadCursor(NULL, IDC_ARROW);
	wndClass.hbrBackground = (HBRUSH)GetStockObject(GRAY_BRUSH);
	wndClass.lpszMenuName = NULL;
	wndClass.lpszClassName = _T("ForTheDreamOfGameDevelop!");
	//ע��
	RegisterClassEx(&wndClass);
	//����
	HWND hwnd = CreateWindow(L"ForTheDreamOfGameDevelop", L"�¾��ҵ���Ϸ����", WS_OVERLAPPEDWINDOW, CW_USEDEFAULT,
		CW_USEDEFAULT, 800, 600, NULL, NULL, hInstance, NULL);
	MoveWindow(hwnd, 200, 50, 800, 600, true);
	ShowWindow(hwnd, nCmdShow);
	UpdateWindow(hwnd);
	//ע��
	UnregisterClass(L"ForTheDreamOfGameDevelop", wndClass.hInstance);
}

