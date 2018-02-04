#include <Windows.h>
#include <tchar.h>
#include<string>
#pragma comment(lib,"winmm.lib")
#pragma comment(lib,"msimg32.lib")
using namespace std;
bool Game_init(const HWND& hwnd);
void Game_Paint(const HWND& hwnd);
void Game_Update(const HWND& hwnd);
bool Game_Exit(const HWND& hwnd);
void Left();
void Up();
void Down();
void Right();
#define WINDOW_WIDTH	932							//Ϊ���ڿ�ȶ���ĺ꣬�Է����ڴ˴��޸Ĵ��ڿ��
#define WINDOW_HEIGHT	700							//Ϊ���ڸ߶ȶ���ĺ꣬�Է����ڴ˴��޸Ĵ��ڸ߶�
#define WINDOW_TITLE	L""	//Ϊ���ڱ��ⶨ��ĺ�
HDC g_hdc = NULL, g_mdc = NULL,g_bufdc=NULL;
//λͼ
HBITMAP g_hSprite[4], g_HBackground;
//g_inum��¼ͼ�ţ�g_iX��g_iY
unsigned g_iNum = 0, g_iX = 0, g_iY = 0;
unsigned g_iDirection = 0;
//ÿ֡���ʱ��
unsigned g_frame = 30;
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
			Up();
			break;
		case VK_DOWN:
			Down();
			break;
		case VK_RIGHT:
			Right();
			break;
		case VK_LEFT:
			Left();
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
	HBITMAP temp;
	temp = CreateCompatibleBitmap(g_hdc, WINDOW_WIDTH, WINDOW_HEIGHT);
	SelectObject(g_mdc, temp);
	//�趨������ͼ��ʼλ�úͷ���
	g_iX = 150;
	g_iY = 350;
	g_iDirection = 3;
	g_iNum = 0;
	//����λͼ
	g_hSprite[0] = (HBITMAP)LoadImage(NULL, L"go1.bmp", IMAGE_BITMAP, 480, 217, LR_LOADFROMFILE);
	g_hSprite[1] = (HBITMAP)LoadImage(NULL, L"go2.bmp", IMAGE_BITMAP, 480, 217, LR_LOADFROMFILE);
	g_hSprite[2] = (HBITMAP)LoadImage(NULL, L"go3.bmp", IMAGE_BITMAP, 480, 217, LR_LOADFROMFILE);
	g_hSprite[3] = (HBITMAP)LoadImage(NULL, L"go4.bmp", IMAGE_BITMAP, 480, 217, LR_LOADFROMFILE);
	g_HBackground = (HBITMAP)LoadImage(NULL, L"bg.bmp", IMAGE_BITMAP, WINDOW_WIDTH, WINDOW_HEIGHT, LR_LOADFROMFILE);

    Game_Paint(hwnd);
	return true;
}
//��Ϸ��Ⱦ
void Game_Paint(const HWND& hwnd)
{
	//������ͼ
	SelectObject(g_bufdc, g_HBackground);
	BitBlt(g_mdc, 0, 0, WINDOW_WIDTH, WINDOW_HEIGHT, g_bufdc, 0, 0, SRCCOPY);    //��buf��ͼ������mdc

	//���շ���ȡ��ͼƬ
	if (g_iDirection > 3){ return; }
	if (g_hSprite[g_iDirection] == NULL){
		return;
	}
	SelectObject(g_bufdc, g_hSprite[g_iDirection]);
	//ȷ����ȡ����ͼ�Ŀ�Ⱥ͸߶�
	BitBlt(g_mdc, g_iX, g_iY, 60, 108, g_bufdc, g_iNum * 60, 108, SRCAND);     //�����������ɫ������AND����
	BitBlt(g_mdc, g_iX, g_iY, 60, 108, g_bufdc, g_iNum * 60, 0, SRCPAINT);     //���Ʒ���ɫ������OR����

	BitBlt(g_hdc, 0, 0, WINDOW_WIDTH, WINDOW_HEIGHT, g_mdc, 0, 0, SRCCOPY);
	g_tPre = GetTickCount();
	g_iNum++;
	if (g_iNum >= 8)
	{
		g_iNum = 0;
	}
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
void Up()
{
	g_iY -= 10;
	g_iDirection = 0;
	if (g_iY <= 0)
	{
		g_iY = 0;
	}
}
void Down()
{
	g_iY += 10;
	g_iDirection = 1;
	if (g_iY >= WINDOW_HEIGHT-100)
	{
		g_iY = WINDOW_HEIGHT-100;
	}
}
void Right()
{
	g_iX += 10;
	g_iDirection = 3;
	if (g_iX >= WINDOW_WIDTH - 100)
	{
		g_iX = WINDOW_WIDTH - 100;
	}
}
void Left()
{
	g_iX -= 10;
	g_iDirection = 2;
	if (g_iX <= 0)
	{
		g_iX = 0;
	}
}