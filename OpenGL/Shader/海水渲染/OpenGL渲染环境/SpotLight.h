#pragma once
#include "PointLight.h"

class SpotLight :public PointLight
{
public:
	SpotLight();
public:
	virtual inline void SetExponent(float v)
	{
		m_Exponent = v;
	}
	virtual inline float GetExponent() const
	{
		return m_Exponent;
	}
	virtual inline void SetCutoff(float v)
	{
		m_Cutoff = v;
	}
	virtual inline float GetCutoff() const
	{
		return m_Cutoff;
	}
protected:
private:
	float m_ConstAttenuation = 1;//设置常数衰减因子
	float m_LinearAttenuation = 0.1f;//设置线性衰减因子
	float m_QuadricASttenuation = 0;//设置平方衰减因子

	float m_Exponent = 90;//聚光灯不衰减角度
	float m_Cutoff = 180;//聚光灯可见角度
};