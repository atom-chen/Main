#include <Windows.h>
#include <tchar.h>
#include<string>
#include <GL/GL.h>
#include <GL/GLU.h>
#include "texture.h"
#include "utils.h"
#include "ObjModel.h"
#pragma comment(lib,"winmm.lib")
#pragma comment(lib,"opengl32.lib")
#pragma comment(lib,"glu32.lib")
using namespace std;
bool Game_init(const HWND& hwnd);
void Game_Paint(const HWND& hwnd);
void Game_Update(const HWND& hwnd);
bool Game_Exit(const HWND& hwnd);

#define WINDOW_WIDTH	800							//Ϊ���ڿ�ȶ���ĺ꣬�Է����ڴ˴��޸Ĵ��ڿ��
#define WINDOW_HEIGHT	600							//Ϊ���ڸ߶ȶ���ĺ꣬�Է����ڴ˴��޸Ĵ��ڸ߶�
#define WINDOW_TITLE	L""	//Ϊ���ڱ��ⶨ��ĺ�
//ÿ֡���ʱ��
unsigned g_frame = 100;
//��¼��ǰʱ�����һ�λ�ͼʱ��
DWORD g_tPre = 0, g_tNow = 0;
HDC g_hdc;
Texture m_Texture;
ObjModel m_Model;

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
		Game_Paint(hwnd);

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
	//����OpenGL��Ⱦ������
	g_hdc = GetDC(hwnd);




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
	if (g_hdc != NULL)
	{
		wglMakeCurrent(g_hdc, rc);           //����OpenGL�豸����
	}
	else{
		return false;
	}
	glClearColor(0.1f, 0.4f, 0.6f, 1.0f);     //������ʲô��ɫ��������

	//���õƹ���ɫ
	float blackColor[] = { 0.0f, 0.0f, 0.0f, 1.0f };
	float whiteColor[] = { 1.0f, 1.0f, 1.0f, 1.0f };
	glLightfv(GL_LIGHT0, GL_AMBIENT, whiteColor);
	glLightfv(GL_LIGHT0, GL_DIFFUSE, whiteColor);
	glLightfv(GL_LIGHT0, GL_SPECULAR, whiteColor);
	//���ò���
	float blackMaterial[] = { 0.0f, 0.0f, 0.0f, 1.0f };
	float ambientMaterial[] = { 0.1f, 0.1f, 0.1f, 1.0f };
	float DIFFUSEMaterial[] = { 0.5f, 0.5f, 0.5f, 1.0f };
	float SpecularMaterial[] = { 0.9f, 0.9f, 0.9f, 1.0f };
	glMaterialfv(GL_FRONT, GL_AMBIENT, ambientMaterial);
	glMaterialfv(GL_FRONT, GL_DIFFUSE, DIFFUSEMaterial);
	glMaterialfv(GL_FRONT, GL_SPECULAR, SpecularMaterial);
	//���÷����ķ���̫���⣩
	float lightPos[] = { 0.0f, 1.0f, 0.0f, 0.0f };
	glLightfv(GL_LIGHT0, GL_POSITION, lightPos);

	//��������
	//glEnable(GL_LIGHTING);
	//���ù�Դ
	//glEnable(GL_LIGHT0);
	//glEnable(GL_DEPTH_TEST);

	glMatrixMode(GL_PROJECTION);       //����GPU����Ҫ������ͶӰ������в���
	gluPerspective(50.0f, 800.0f / 600.0f, 0.1f, 1000.0f);        //������ͶӰ��������ֵ
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
	//glEnable(GL_CULL_FACE);
	//glFrontFace(GL_CW);
	//m_Texture.Init("5555_2.bmp");              //��ʼ��OpenGLͼƬ
	//glEnable(GL_TEXTURE_2D);             //���߹̶����� �㿪��������
	//glBindTexture(GL_TEXTURE_2D, m_Texture.mTextureID);               //�������Ϊ��ǰ������
	m_Model.Init("Resource/Quad.obj");
    Game_Paint(hwnd);
	return true;
}
//��Ϸ��Ⱦ
void Game_Paint(const HWND& hwnd)
{
	glLoadIdentity();
	//��֮ǰ���õ���ɫȥ��������
	glClear(GL_COLOR_BUFFER_BIT );
	//����3���任�ı����Ǿ������
	glColor4ub(255, 255, 255, 255);      //���õ�ǰ����ɫΪ��ɫ��4�ֽڵ���ɫ��

	m_Model.Draw();
	//����֮�� �ѻ��õĳ��ֳ���
	SwapBuffers(g_hdc);

}
//��Ϸ�߼�
void Game_Update(const HWND& hwnd)
{

}
bool Game_Exit(const HWND& hwnd)
{
	//�ͷ���Դ����
	if (g_hdc != NULL)
	{
		DeleteDC(g_hdc);
	}
	return true;
}