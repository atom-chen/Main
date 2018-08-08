#pragma once
#include "ggl.h"
#include "Scene.h"

class EngineBehavior 
{
public:
	void Awake(const HINSTANCE& hInstance);
	int SystemEvent(UINT message, WPARAM wParam, LPARAM lParam);

	virtual void Start();
	virtual void Update();
	virtual void OnDestory();
	virtual void Event(UINT message, WPARAM wParam, LPARAM lParam);
private:
	void SystemStart();
	void SystemUpdate();
	void SystemDestory();

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