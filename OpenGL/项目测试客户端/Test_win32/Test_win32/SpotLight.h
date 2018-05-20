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
	float m_ConstAttenuation = 1;//���ó���˥������
	float m_LinearAttenuation = 0.1f;//��������˥������
	float m_QuadricASttenuation = 0;//����ƽ��˥������

	float m_Exponent = 90;//�۹�Ʋ�˥���Ƕ�
	float m_Cutoff = 180;//�۹�ƿɼ��Ƕ�
};