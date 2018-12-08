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

	virtual void OnKeyDown(char KeyCode);//���¼���ʱ����
	virtual void OnKeyUp(char KeyCode);//�ɿ�����ʱ������
	virtual void OnMouseMove(float deltaX, float deltaY);//����ƶ�������תʱ������
	virtual void OnMouseWheel(int direction);
protected:
private:
	GameObject m_Cube,mCube2;
	FrameBuffer mFBO,blurFBO;
	FullScreenQuad mFSQ,blurFSQ;

	FrameBuffer mFBO1, blurFBOH, blurFBOV;
	FullScreenQuad mFSQ1, blurFSQH, blurFSQV;
};