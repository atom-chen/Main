#include "ggl.h"
#include "Scene.h"
#pragma comment(lib,"opengl32.lib")
#pragma comment(lib,"glu32.lib")
void Update();
bool Game_Exit();
/************************************************************************/
/* ȫ�ֱ�������                                                              */
/************************************************************************/
unsigned g_frame = 9000000;                   //ÿ֡���ʱ��
DWORD g_tPre = 0, g_tNow = 0;             //��¼��ǰʱ�����һ�λ�ͼʱ��
HDC g_hdc;

void ShowErrMessage(const HWND & hwnd)
{
	MessageBox(hwnd, L"��Դ��ʼ��ʧ��", L"ERR", 0);
}
LRESULT CALLBACK WindowProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	switch (message)
	{
	case WM_PAINT:
		//����
		Draw();
		break;
	case WM_KEYDOWN:
		//������Ϣ
		if (wParam == VK_ESCAPE)
		{
			DestroyWindow(hwnd);
		}
		break;
	case WM_DESTROY:
		Game_Exit();
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
	wndclass.hbrBackground = NULL;
	wndclass.lpszMenuName = NULL;
	wndclass.lpszClassName = WINDOW_TITLE;

	RegisterClassEx(&wndclass);
	hwnd = CreateWindowEx(NULL, wndclass.lpszClassName,WINDOW_TITLE, WS_OVERLAPPEDWINDOW | WS_VISIBLE
		, 100, 100, WINDOW_WIDTH, WINDOW_HEIGHT, NULL, NULL, wndclass.hInstance, NULL);
	if (hwnd == NULL)
	{
		return false;
	}
	//����OpenGL��Ⱦ������
	g_hdc = GetDC(hwnd);
	//���ظ�ʽ������
	PIXELFORMATDESCRIPTOR pfd;
	memset(&pfd, 0, sizeof(PIXELFORMATDESCRIPTOR));
	pfd.nVersion = 1;
	pfd.nSize = sizeof(PIXELFORMATDESCRIPTOR);
	pfd.cColorBits = 32;
	pfd.cDepthBits = 24;
	pfd.cStencilBits = 8;
	pfd.iPixelType = PFD_TYPE_RGBA;
	pfd.iLayerType = PFD_MAIN_PLANE;
	pfd.dwFlags = PFD_DRAW_TO_WINDOW | PFD_SUPPORT_OPENGL | PFD_DOUBLEBUFFER;
	//ѡһ�����ظ�ʽ
	int pixelFormat = ChoosePixelFormat(g_hdc, &pfd);
	SetPixelFormat(g_hdc, pixelFormat, &pfd);
	HGLRC rc = wglCreateContext(g_hdc);
	if (rc == NULL)
	{
		return false;
	}
	wglMakeCurrent(g_hdc, rc);           //����OpenGL�豸����
	if (!Init())
	{
		return false;
	}
	MoveWindow(hwnd, 200, 20, WINDOW_WIDTH, WINDOW_HEIGHT, true);
	ShowWindow(hwnd, nCmdShow);
	UpdateWindow(hwnd);
	return true;
}
int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
	WNDCLASSEX wndclass = { 0 };
	HWND hwnd;
	if (!initWndclass(wndclass, hInstance, hwnd, nCmdShow))
	{
		ShowErrMessage(hwnd);
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
		//��ȡ��ǰʱ��
		g_tNow = GetTickCount();
		//����˴�ѭ������ʱ����ϴ����һ��֡��ʱ��
		if (g_tNow - g_tPre >= g_frame)
		{
			//������Ϸ�߼�
			Update();
			Draw();
			//����֮�� �ѻ��õĳ��ֳ���
			SwapBuffers(g_hdc);
			g_tPre = GetTickCount();
		}
	}
	//ע������
	UnregisterClass(wndclass.lpszClassName, wndclass.hInstance);
	return 0;
}
void Update()
{
	
}
bool Game_Exit()
{
	//�ͷ���Դ����
	if (g_hdc != NULL)
	{
		DeleteDC(g_hdc);
	}
	return true;
}