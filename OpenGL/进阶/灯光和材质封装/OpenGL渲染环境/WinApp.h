#pragma once
#include "ggl.h"
#include "Scene.h"

class EngineBehavior 
{
public:
public:
	void Awake(const HINSTANCE& hInstance);
	int Event(UINT message, WPARAM wParam, LPARAM lParam);

	virtual void Start();
	virtual void Update();
	virtual void OnDestory();
public:
	void InitGL();
private:
	HDC m_Hdc;
	HINSTANCE m_HInstance;
	HWND m_Hwnd;
	int m_Width;
	int m_Height;
	WNDCLASSEX wndclass;
};