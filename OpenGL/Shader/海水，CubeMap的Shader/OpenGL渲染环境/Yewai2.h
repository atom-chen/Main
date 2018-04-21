#pragma  once
#include "Scene.h"
#include "Light.h"
#include "GameObject.h"
#include "Texture.h"
#include "Ground.h"
#include "SurroundParticle.h"


class Yewai2 :public Scene
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
	Ground m_Ground;
	GameObject m_box;
	GameObject m_Niu;
	SurroundParticle m_Particle;
	Light m_DirectionLight;
	Light m_PointLight;
	Light m_SpotLight;

	int m_BoxRotateY = 0;
};
