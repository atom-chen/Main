#pragma once
#include "ggl.h"
#include "Camera.h"
#include "Camera_2D.h"
#include "RenderList.h"

class Camera
{
public:
	Camera(Camera_1st *camera)
	{
		m_3DRendList.SetCamera(camera);
	}
protected:
private:
	Camera_1st *m_MainCamera;
	RenderList m_3DRendList;
};

class Scene
{
friend class SceneManager;
public:
	virtual bool Awake();
protected:

	virtual void Start();


	virtual void Update();
	virtual void OnDrawBegin();

	virtual void Draw3D();
	virtual void Draw2D();

	virtual void OnDrawOver();
	virtual void OnDesrory();

	virtual void SetViewPortSize(float width, float height);

	virtual void OnKeyDown(char KeyCode);//按下键盘时调用
	virtual void OnKeyUp(char KeyCode);//松开键盘时被调用
	virtual void OnMouseMove(float deltaX, float deltaY);//鼠标移动导致旋转时被调用
	virtual void OnMouseWheel(int direction);
private:
	void SystemAwake();
	void SystemStart();
	void SystemUpdate();
	void SystemOnDrawBegin();
	void SystemDraw3D();
	void SystemDraw2D();
	void SystemOnDraw2DOver();
	void SystemOnDrawOver();
	void SystemDesrory();


protected:
	//Camera_2D m_2DCamera;
	Camera_1st *m_MainCamera;
private:
	FrameBuffer* m_FrameBuf;
	RenderList m_3DRendList;
};