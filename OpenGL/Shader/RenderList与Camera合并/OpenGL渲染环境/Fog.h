#pragma once
#include "GameObject.h"

class FogObj:public GameObject
{
public:
	virtual bool Init(const char* path, const char* vertexShader = SHADER_ROOT"fog.vert", const char* fragmentShader = SHADER_ROOT"fog.frag");
public:
	inline void SetFogNear(float v);
	inline void SetFogFar(float v);
	void SetExp(float v);
	void SetExpEx(float v);
protected:
	//线性雾需求数据
	float m_FogNear = 0.1f;
	float m_FogFar = 3;
	//非线性雾需求数据
	float m_Mul = 1;//雾随距离的衰减速度
	float m_Pow = 0.8f;
private:
};