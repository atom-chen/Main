#pragma  once
#include "Scene.h"
#include "Light.h"
#include "GameObject.h"
#include "ParticleSystem.h"
#include "Texture.h"
#include "Light.h"


class Zhucheng:public Scene
{
public:
	virtual bool Awake();
	virtual void Start();

	virtual void Update();
	virtual void Draw3D();
	virtual void Draw2D();

	virtual void OnKeyDown(char KeyCode);//按下键盘时调用
	virtual void OnKeyUp(char KeyCode);//松开键盘时被调用
	virtual void OnMouseMove(float deltaX, float deltaY);//鼠标移动导致旋转时被调用
	virtual void OnMouseWheel(int32_t direction);
protected:
private:
	GameObject m_box;
	GameObject m_Niu;
	UITexture m_Texture;
	ParticleSystem m_ParticleSystem;
	DirectionLight m_DirectionLight;
	Light m_Sum;
};
