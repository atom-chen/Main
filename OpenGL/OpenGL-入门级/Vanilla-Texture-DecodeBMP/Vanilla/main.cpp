#include <Windows.h>
#include <tchar.h>
#include<string>
#include <GL/GL.h>
#include <GL/GLU.h>
#include "texture.h"
#include "utils.h"
#pragma comment(lib,"winmm.lib")
#pragma comment(lib,"opengl32.lib")
#pragma comment(lib,"glu32.lib")
using namespace std;
bool Game_init(const HWND& hwnd);
void Game_Paint(const HWND& hwnd);
void Game_Update(const HWND& hwnd);
bool Game_Exit(const HWND& hwnd);

#define WINDOW_WIDTH	800							//为窗口宽度定义的宏，以方便在此处修改窗口宽度
#define WINDOW_HEIGHT	600							//为窗口高度定义的宏，以方便在此处修改窗口高度
#define WINDOW_TITLE	L""	//为窗口标题定义的宏
//每帧间隔时间
unsigned g_frame = 100;
//记录当前时间和上一次绘图时间
DWORD g_tPre = 0, g_tNow = 0;
HDC g_hdc;

void ShowErrMessage(const HWND & hwnd)
{
	MessageBox(hwnd, L"资源初始化失败", L"ERR", 0);
}
LRESULT CALLBACK WindowProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	switch (message)
	{
	case WM_PAINT:
		//绘制
		Game_Paint(hwnd);

		break;
	case WM_KEYDOWN:
		//键盘信息
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
	//创建OpenGL渲染上下文
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
		//如果有消息的处理消息
		if (PeekMessage(&msg, NULL, 0, 0, PM_REMOVE))
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
		else{
			//获取当前时间
			g_tNow = GetTickCount();
			//如果此次循环运行时间和上次相差一个帧的时间
			if (g_tNow - g_tPre >= g_frame)
			{
				//处理游戏逻辑
				Game_Update(hwnd);
				Game_Paint(hwnd);
			}
		}
	}
	//注销窗口
	UnregisterClass(wndclass.lpszClassName, wndclass.hInstance);
	return 0;
}
bool Game_init(const HWND& hwnd)
{
	//像素格式描述符
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
	//选一个像素格式
	int pixelFormat = ChoosePixelFormat(g_hdc, &pfd);
	SetPixelFormat(g_hdc, pixelFormat, &pfd);
	HGLRC rc = wglCreateContext(g_hdc);
	if (g_hdc != NULL)
	{
		wglMakeCurrent(g_hdc, rc);           //设置OpenGL设备环境
	}
	else{
		return false;
	}
	glClearColor(0.1f, 0.4f, 0.6f, 1.0f);     //设置用什么颜色擦缓冲区

	//设置灯光颜色
	float blackColor[] = { 0.0f, 0.0f, 0.0f, 1.0f };
	float whiteColor[] = { 1.0f, 1.0f, 1.0f, 1.0f };
	glLightfv(GL_LIGHT0, GL_AMBIENT, whiteColor);
	glLightfv(GL_LIGHT0, GL_DIFFUSE, whiteColor);
	glLightfv(GL_LIGHT0, GL_SPECULAR, whiteColor);
	//设置材质
	float blackMaterial[] = { 0.0f, 0.0f, 0.0f, 1.0f };
	float ambientMaterial[] = { 0.1f, 0.1f, 0.1f, 1.0f };
	float DIFFUSEMaterial[] = { 0.5f, 0.5f, 0.5f, 1.0f };
	float SpecularMaterial[] = { 0.9f, 0.9f, 0.9f, 1.0f };
	glMaterialfv(GL_FRONT, GL_AMBIENT, ambientMaterial);
	glMaterialfv(GL_FRONT, GL_DIFFUSE, DIFFUSEMaterial);
	glMaterialfv(GL_FRONT, GL_SPECULAR, SpecularMaterial);
	//设置方向光的方向（太阳光）
	float lightPos[] = { 0.0f, 1.0f, 0.0f, 0.0f };
	glLightfv(GL_LIGHT0, GL_POSITION, lightPos);

	//开启光照
	glEnable(GL_LIGHTING);
	//设置光源
	glEnable(GL_LIGHT0);
	//glEnable(GL_DEPTH_TEST);

	glMatrixMode(GL_PROJECTION);       //告诉GPU，我要对正交投影矩阵进行操作
	gluPerspective(50.0f, 800.0f / 600.0f, 0.1f, 1000.0f);        //给正交投影矩阵设置值
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
	//glEnable(GL_CULL_FACE);
	//glFrontFace(GL_CW);
	Texture texture;
	texture.Init("5555_2.bmp");              //初始化OpenGL图片
	
	glEnable(GL_TEXTURE_2D);             //告诉固定管线 你开启了纹理
	glBindTexture(GL_TEXTURE_2D, texture.mTextureID);               //把纹理绑定为当前的纹理
    Game_Paint(hwnd);
	return true;
}
//游戏渲染
void Game_Paint(const HWND& hwnd)
{
	glLoadIdentity();
	//用之前设置的颜色去擦缓冲区
	glClear(GL_COLOR_BUFFER_BIT );
	//以下3个变换的本质是矩阵操作
	glColor4ub(255, 255, 255, 255);      //设置当前的颜色为白色（4字节的颜色）
	glBegin(GL_TRIANGLES);                  //我要开始画了
	//提供画点数据
	glNormal3f(0.0f, 1.0f, 0.0f);
	glTexCoord2f(0.0f, 0.0f);                      //指定纹理坐标
	glVertex3f(-1.0f, -0.5f, -2.0f);

	glNormal3f(0.0f, 1.0f, 0.0f);
	glTexCoord2f(1.0f, 0.0f);
	glVertex3f(1.0f, -0.5f, -2.0f);

	glNormal3f(0.0f, 1.0f, 0.0f);
	glTexCoord2f(0.5f, 1.0f);
	glVertex3f(0.0f, -0.5f, -10.0f);

	//glVertex3f(0.0f, 0.0f, -10.0f);

	//glVertex3f(5.0f, 0.0f, -10.0f);

	//glVertex3f(5.0f, -2.0f, -10.0f);
	glEnd();                             //画完了



	//画完之后 把画好的呈现出来
	SwapBuffers(g_hdc);

}
//游戏逻辑
void Game_Update(const HWND& hwnd)
{

}
bool Game_Exit(const HWND& hwnd)
{
	//释放资源对象
	if (g_hdc != NULL)
	{
		DeleteDC(g_hdc);
	}
	return true;
}