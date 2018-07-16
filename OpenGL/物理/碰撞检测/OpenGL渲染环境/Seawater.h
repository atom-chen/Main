#pragma once
#include "RenderAble_Light.h"

class SeaWater:public RenderAble_Light
{
public:
	virtual bool Init(const char* seaTexture,const char* foamTexture=nullptr, const char* lightTexture=nullptr);
	void Update();
public:
	void SetSeaSize(float width, float height);
	void SetWaveHeight(float height);
	void SetWaveMoveSpeed(float speed);
	void SetSeaTexture(const char* picPath);
	void SetFoamTexture(const char* picPath);
	void SetLightTexture(const char* picPath);
	void SetLight1(const Light& light);
protected:
private:
};