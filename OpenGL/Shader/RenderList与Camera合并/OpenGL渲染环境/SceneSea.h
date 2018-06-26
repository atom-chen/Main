#pragma once
#include "Scene.h"
#include "DirectionLight.h"
#include "GameObject.h"
#include "Texture.h"
#include "Ground.h"
#include "SurroundParticle.h"
#include "SkyBox.h"
#include "SkyBoxC.h"


class SceneSea :public Scene
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
	DirectionLight m_DirectionLight;
	SkyBox m_Skybox;
};
