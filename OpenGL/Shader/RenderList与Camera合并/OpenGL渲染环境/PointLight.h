#pragma once
#include "DirectionLight.h"

class PointLight:public DirectionLight
{
public:
	PointLight();
	virtual inline void SetConstAttenuation(float v)
	{
		m_ConstAttenuation = v;
	}
	virtual inline float GetConstAttenuation() const
	{
		return m_ConstAttenuation;
	}
	virtual inline void SetLinearAttenuation(float v)
	{
		m_LinearAttenuation = v;
	}
	virtual inline float GetLinearAttenuation() const
	{
		return m_LinearAttenuation;
	}
	virtual inline void SetQuadricASttenuation(float v)
	{
		m_QuadricASttenuation = v;
	}
	virtual inline float GetQuadricASttenuation() const
	{
		return m_QuadricASttenuation;
	}
protected:
	float m_ConstAttenuation = 1;//���ó���˥������
	float m_LinearAttenuation = 0.1f;//��������˥������
	float m_QuadricASttenuation = 0;//����ƽ��˥������
protected:
	virtual inline void SetExponent(float v)
	{

	}
	virtual inline float GetExponent() const
	{
		return INVALID;
	}
	virtual inline void SetCutoff(float v)
	{

	}
	virtual inline float GetCutoff() const
	{
		return INVALID;
	}
};