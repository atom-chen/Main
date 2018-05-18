#include "ggl.h"
#include "Scene.h"
#pragma comment(lib,"opengl32.lib")
#pragma comment(lib,"glu32.lib")
#pragma comment(lib,"winmm.lib")
#pragma comment(lib,"SOIL.lib")
#pragma comment(lib,"glew32.lib")


bool Game_Exit();
/************************************************************************/
/* ȫ�ֱ�������                                                              */
/************************************************************************/

HDC g_hdc;

POINT m_OriginalPos;//��¼�������ʱ��λ��
bool m_IsRotate = false;//�Ƿ�������ת

void ShowErrMessage(const HWND & hwnd)
{
	MessageBox(hwnd, L"��Դ��ʼ��ʧ��", L"ERR", 0);
}
LRESULT CALLBACK WindowProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	switch (message)
	{
	case WM_KEYDOWN:
		OnKeyDown(wParam);
		break;
	case WM_KEYUP:
		OnKeyUp(wParam);
		break;
	case WM_MOUSEMOVE:
		if (m_IsRotate)
		{
			//��¼������ƫ������
			POINT currentPos;
			currentPos.x = LOWORD(lParam);
			currentPos.y= HIWORD(lParam);
			ClientToScreen(hwnd, &currentPos);
			//ƫ������=��������-���ʱ�������
			int32_t deltaX = currentPos.x - m_OriginalPos.x;
			int32_t deltaY = currentPos.y - m_OriginalPos.y;
			OnMouseMove(deltaX, deltaY);//������Ϣ
			SetCursorPos(m_OriginalPos.x, m_OriginalPos.y);//��λ��Ȼ���ټ���ƫ��
		}
		break;
	case WM_RBUTTONDOWN:
		m_OriginalPos.x = LOWORD(lParam);
		m_OriginalPos.y = HIWORD(lParam);
		ClientToScreen(hwnd, &m_OriginalPos);
		SetCapture(hwnd);
		ShowCursor(false);
		m_IsRotate = true;
		break;
	case WM_RBUTTONUP:
		m_IsRotate = false;
		SetCursorPos(m_OriginalPos.x, m_OriginalPos.y);
		ReleaseCapture();
		ShowCursor(true);
		break;
	case WM_PAINT:
		//����
		Draw();
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
	pfd.cColorBits = 32;//��ɫ������ÿ������ռ��32bit
	pfd.cDepthBits = 24;//��Ȼ�����
	pfd.cStencilBits = 8;//�ɰ滺����
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
	glewInit();
	wglMakeCurrent(g_hdc, rc);           //����OpenGL�豸����
	if (!Init())
	{
		return false;
	}
	MoveWindow(hwnd, 200, 20, WINDOW_WIDTH, WINDOW_HEIGHT, true);
	SetViewPortSize(WINDOW_WIDTH, WINDOW_HEIGHT);
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
		//����˴�ѭ������ʱ����ϴ����һ��֡��ʱ��
		//������Ϸ�߼�
		Update();
		Draw();
		//����֮�� �ѻ��õĳ��ֳ���
		SwapBuffers(g_hdc);
	}
	//ע������
	UnregisterClass(wndclass.lpszClassName, wndclass.hInstance);
	return 0;
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