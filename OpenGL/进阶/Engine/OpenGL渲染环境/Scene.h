#pragma once
#include "ggl.h"
#include "Camera.h"
#include "Camera_2D.h"
#include "RenderList.h"


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
	virtual void OnKeyDown(char KeyCode);
	virtual void OnKeyUp(char KeyCode);
	virtual void OnMouseMove(float deltaX, float deltaY);
	virtual void OnMouseWheel(int direction);
private:
	void InsertToRenderList(RenderAble* render);
	void InsertToRenderList(const RenderDomain& render);
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
	Camera *m_MainCamera;
};