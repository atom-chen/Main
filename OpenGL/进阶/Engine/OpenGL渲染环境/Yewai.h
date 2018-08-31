#pragma  once
#include "Scene.h"
#include "DirectionLight.h"
#include "PointLight.h"
#include "SpotLight.h"
#include "GameObject.h"
#include "Texture.h"
#include "Ground.h"
#include "SurroundParticle.h"
#include "SkyBox.h"
#include "SkyBoxC.h"
#include "UpParticle.h"


class Yewai:public Scene
{
public:
	virtual bool Awake();
	virtual void Start();

	virtual void Update();
	virtual void OnDrawBegin();
	virtual void Draw3D();
	virtual void Draw2D();
	virtual void OnDesrory();

	virtual void OnKeyDown(char KeyCode);
	virtual void OnKeyUp(char KeyCode);
	virtual void OnMouseMove(float deltaX, float deltaY);
	virtual void OnMouseWheel(int direction);
protected:
private:
	SkyBox m_Skybox;
	//Ground m_Ground;
	GameObject m_box;
	GameObject m_Niu;
	UpParticle m_ParticleSystem;
	DirectionLight m_DirectionLight;
	PointLight m_PointLight;
	SpotLight m_SpotLight;
	int m_BoxRotateY = 0;
};
