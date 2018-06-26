#pragma once
#include "Light.h"

class DirectionLight:public Light
{
public:
	DirectionLight();
	virtual inline void SetAmbientColor(float r, float g, float b, float a = 1)
	{
		m_Color.SetAmbientColor(r, g, b, a);
	}
	virtual inline void SetDiffuseColor(float r, float g, float b, float a = 1)
	{
		m_Color.SetDiffuseColor(r, g, b, a);
	}
	virtual inline void SetSpecularColor(float r, float g, float b, float a = 1)
	{
			m_Color.SetSepcularColor(r, g, b, a);
	}
	virtual inline const vec4& GetAmbientColor() const
	{
		return m_Color.GetAmbientColor();
	}
	virtual inline const vec4& GetDiffuseColor() const
	{
		return m_Color.GetDiffuseColor();
	}
	virtual inline const vec4& GetSpecularColor() const
	{
		return m_Color.GetSepcularColor();
	}
	virtual inline int GetType()  const 
	{
		return m_Type;
	}
protected:
private:
	virtual inline void SetConstAttenuation(float v)
	{

	}
	virtual inline  float GetConstAttenuation() const
	{
		return INVALID;
	}
	virtual inline  void SetLinearAttenuation(float v)
	{

	}
	virtual inline  float GetLinearAttenuation() const
	{
		return INVALID;
	}
	virtual  inline void SetQuadricASttenuation(float v)
	{

	}
	virtual  inline float GetQuadricASttenuation() const
	{
		return INVALID;
	}
	virtual inline  void SetExponent(float v)
	{

	}
	virtual inline  float GetExponent() const
	{
		return INVALID;
	}
	virtual inline  void SetCutoff(float v)
	{

	}
	virtual inline  float GetCutoff() const
	{
		return INVALID;
	}
};