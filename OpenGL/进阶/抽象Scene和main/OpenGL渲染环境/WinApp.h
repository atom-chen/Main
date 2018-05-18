#pragma once
#include "ggl.h"
#include "Scene.h"

class WinApp
{
public:
	//WinApp();
public:
	virtual void Awake(const HINSTANCE& hInstance);
	virtual void Start();
	virtual void Update();
	virtual int Event(UINT message, WPARAM wParam, LPARAM lParam);
	virtual void OnDestory();
public:
	void InitGL();
private:
	HDC m_Hdc;
	HINSTANCE m_HInstance;
	HWND m_Hwnd;
	int m_Width;
	int m_Height;
	Scene* m_CurScene;
	WNDCLASSEX wndclass;
	POINT m_OriginalPos;//��¼�������ʱ��λ��
	bool m_IsRotate = false;//�Ƿ�������ת
};