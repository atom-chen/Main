#pragma once
#include "Scene.h"
#include "GameObject.h"
#include "FrameBuffer.h"
#include "FullScreenQuad.h"


class GaussScene: public Scene
{
public:
	virtual bool Awake();
	virtual void Start();

	virtual void Update();
	virtual void OnDrawBegin();
	virtual void Draw3D();
	virtual void Draw2D();
	virtual void OnDesrory();

	virtual void OnKeyDown(char KeyCode);//按下键盘时调用
	virtual void OnKeyUp(char KeyCode);//松开键盘时被调用
	virtual void OnMouseMove(float deltaX, float deltaY);//鼠标移动导致旋转时被调用
	virtual void OnMouseWheel(int direction);
protected:
private:
	GameObject m_Cube,mCube2;
	FrameBuffer mFBO,blurFBO;
	FullScreenQuad mFSQ,blurFSQ;

	FrameBuffer mFBO1, blurFBOH, blurFBOV;
	FullScreenQuad mFSQ1, blurFSQH, blurFSQV;
};