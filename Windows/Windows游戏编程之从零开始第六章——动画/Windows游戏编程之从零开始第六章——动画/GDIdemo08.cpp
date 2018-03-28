//#include <Windows.h>
//#include <tchar.h>
//#include<string>
//#pragma comment(lib,"winmm.lib")
//#pragma comment(lib,"msimg32.lib")
//using namespace std;
//bool Game_init(const HWND& hwnd);
//void Game_Paint(const HWND& hwnd);
//void Game_Update(const HWND& hwnd);
//bool Game_Exit(const HWND& hwnd);
//
//#define WINDOW_WIDTH	800							//Ϊ���ڿ�ȶ���ĺ꣬�Է����ڴ˴��޸Ĵ��ڿ��
//#define WINDOW_HEIGHT	600							//Ϊ���ڸ߶ȶ���ĺ꣬�Է����ڴ˴��޸Ĵ��ڸ߶�
//#define WINDOW_TITLE	L"͸������"	      //Ϊ���ڱ��ⶨ��ĺ�
//HDC g_hdc = NULL, g_mdc = NULL;
////ÿ֡���ʱ��
//unsigned g_frame = 100;
////��¼��ǰʱ�����һ�λ�ͼʱ��
//DWORD g_tPre = 1000, g_tNow = 0;
////����ͱ���λͼ
//HBITMAP g_hSprite=NULL, g_hBackGround=NULL;
////ͼ��
//unsigned g_iNum=0;
////��ͼ�ĺ�������
//unsigned g_iX=0, g_iY=0;
////3����
//HDC g_bufdc = NULL;
//
//LRESULT CALLBACK WindowProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam)
//{
//	PAINTSTRUCT paintStruct;
//	switch (message)
//	{
//	case WM_PAINT:
//		//����
//		g_hdc = BeginPaint(hwnd, &paintStruct);
//		Game_Paint(hwnd);
//		EndPaint(hwnd, &paintStruct);
//		ValidateRect(hwnd, NULL);
//		break;
//	case WM_KEYDOWN:
//		//������Ϣ
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
//inline bool initWndclass(WNDCLASSEX& wndclass, const HINSTANCE& hInstance, HWND& hwnd, const int &nCmdShow)
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
//	wndclass.lpszClassName = L"WINCLASS";
//
//	RegisterClassEx(&wndclass);
//	hwnd = CreateWindowEx(NULL, wndclass.lpszClassName, WINDOW_TITLE, WS_OVERLAPPEDWINDOW | WS_VISIBLE
//		, 0, 0, WINDOW_WIDTH, WINDOW_HEIGHT, NULL, NULL, wndclass.hInstance, NULL);
//	if (hwnd == NULL)
//	{
//		return false;
//	}
//	MoveWindow(hwnd, 200, 20, WINDOW_WIDTH, WINDOW_HEIGHT, true);
//    ShowWindow(hwnd, nCmdShow);
//	UpdateWindow(hwnd);
//	if (!Game_init(hwnd))
//	{
//		return false;
//	}
//	return true;
//}
//int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
//{
//	WNDCLASSEX wndclass = { 0 };
//	HWND hwnd;
//	if (!initWndclass(wndclass, hInstance, hwnd, nCmdShow))
//	{
//		MessageBox(hwnd, L"��Դ��ʼ��ʧ��", L"ERR", 0);
//		return -1;
//	}
//	MSG msg = { 0 };
//	while (msg.message != WM_QUIT)
//	{
//		//�������Ϣ�Ĵ�����Ϣ
//		if (PeekMessage(&msg, NULL, 0, 0, PM_REMOVE))
//		{
//			TranslateMessage(&msg);
//			DispatchMessage(&msg);
//		}
//		else{
//			//��ȡ��ǰʱ��
//			g_tNow = GetTickCount();
//			//����˴�ѭ������ʱ����ϴ����һ��֡��ʱ��
//			if (g_tNow - g_tPre >= g_frame)
//			{
//				Game_Paint(hwnd);
//			}
//		}
//		//������Ϸ�߼�
//		Game_Update(hwnd);
//	}
//	//ע������
//	UnregisterClass(wndclass.lpszClassName, wndclass.hInstance);
//	return 0;
//}
//bool Game_init(const HWND& hwnd)
//{
//	g_hdc = GetDC(hwnd);
//	if (g_hdc == NULL)
//	{
//		return false;
//	}
//	g_mdc = CreateCompatibleDC(g_hdc);
//	if (g_mdc == NULL)
//	{
//		return false;
//	}
//	g_bufdc = CreateCompatibleDC(g_hdc);
//	if (g_bufdc == NULL)
//	{
//		return false;
//	}
//	//��ʼ��λͼ
//	g_hSprite = static_cast<HBITMAP>(LoadImage(NULL, L"goright.bmp", IMAGE_BITMAP, 480, 108, LR_LOADFROMFILE));
//	g_hBackGround = static_cast<HBITMAP>(LoadImage(NULL, L"bg.bmp", IMAGE_BITMAP, WINDOW_WIDTH, WINDOW_HEIGHT, LR_LOADFROMFILE));
//	//��ʼ����ͼ��ʼ����
//	g_iX = 0;
//	g_iY = 350;
//	//��һ����λͼ��g_mdc����Ϊ����Ҫ��g_mdc�Ͻ���͸������
//	HBITMAP temp = CreateCompatibleBitmap(g_hdc, WINDOW_WIDTH, WINDOW_HEIGHT);
//	SelectObject(g_mdc, temp);
//	return true;
//}
////��Ϸ��Ⱦ����Ϸѭ������
//void Game_Paint(const HWND& hwnd)
//{
//	//��ͷ��ʼ
//	if (g_iNum >= 8)
//	{
//		g_iNum = 0;
//	}
//	//������ͼ����mdc
//	SelectObject(g_bufdc, g_hBackGround);
//	BitBlt(g_mdc, 0, 0, WINDOW_WIDTH, WINDOW_HEIGHT, g_bufdc, 0, 0, SRCCOPY);
//	//��mdc����͸������
//	SelectObject(g_bufdc, g_hSprite);
//	TransparentBlt(g_mdc, g_iX, g_iY, 60, 108, g_bufdc, g_iNum * 60, 0, 60, 108, RGB(255, 0, 0));   //��
//	//������õ�����hdc
//	BitBlt(g_hdc, 0, 0, WINDOW_WIDTH, WINDOW_HEIGHT, g_mdc, 0, 0, SRCCOPY);
//
//	//��������
//	g_tPre = GetTickCount();
//	g_iNum++;
//	g_iX += 10;     ////��ԭλ��+10�ĵط���ʼ��
//	if (g_iX >= WINDOW_WIDTH)
//	{
//		//��ͷ��ʼ��
//		g_iX = -60;
//	}
//	
//
//}
////��Ϸ�߼�
//void Game_Update(const HWND& hwnd)
//{
//	
//}
//bool Game_Exit(const HWND& hwnd)
//{
//	//�ͷ���Դ����
//	if (g_bufdc != NULL)
//	{
//		DeleteDC(g_bufdc);
//	}
//	if (g_mdc != NULL)
//	{
//		DeleteDC(g_mdc);
//	}
//	if (g_hdc != NULL)
//	{
//		ReleaseDC(hwnd, g_hdc);
//	}
//	if (g_hSprite != NULL)
//	{
//		DeleteObject(g_hSprite);
//	}
//	if (g_hSprite != NULL)
//	{
//		DeleteObject(g_hBackGround);
//	}
//
//	return true;
//}