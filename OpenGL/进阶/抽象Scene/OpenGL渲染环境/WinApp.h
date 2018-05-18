#pragma once
#include "ggl.h"
#include "Scene.h"

class WinApp
{
public:
	WinApp();
public:
	virtual void Awake(const HINSTANCE& hInstance,const int &nCmdShow);
	virtual void Start();
	virtual void Update();
	virtual int Event(UINT message, WPARAM wParam, LPARAM lParam);
	virtual void OnDestory();
public:
	bool InitDevice();

private:
	HDC m_Hdc;
	HINSTANCE m_HInstance;
	HWND m_Hwnd;
	int m_Width;
	int m_Height;
	Scene* m_CurScene;
	WNDCLASSEX wndclass;
	POINT m_OriginalPos;//记录按下鼠标时的位置
	bool m_IsRotate = false;//是否正在旋转

	EGLConfig m_Config;
	EGLSurface m_Surface;
	EGLContext m_Context;
	EGLDisplay m_Display;
};