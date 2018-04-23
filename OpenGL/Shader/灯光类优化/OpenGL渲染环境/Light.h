#pragma  once
#include "ggl.h"
#include "LightColor.h"
#include "Node.h"
enum LIGHT_TYPE
{
	LIGHT_INVALID=0,
	LIGHT_DIRECTION=1,
	LIGHT_POINT=2,
	LIGHT_SPOT=3,
};


class Light:public Node
{
public:


	virtual int GetType()  const = 0;

	virtual void SetAmbientColor(float r, float g, float b, float a=1)=0;
	virtual void SetDiffuseColor(float r, float g, float b, float a = 1) = 0;
	virtual void SetSpecularColor(float r, float g, float b, float a = 1) = 0;
	virtual const vec4& GetAmbientColor() const = 0;
	virtual const vec4& GetDiffuseColor() const = 0;
	virtual const vec4& GetSpecularColor() const = 0;

	virtual  void SetConstAttenuation(float v) = 0;
	virtual  float GetConstAttenuation() const = 0;
	virtual  void SetLinearAttenuation(float v) = 0;
	virtual  float GetLinearAttenuation() const = 0;
	virtual  void SetQuadricASttenuation(float v )=0;
	virtual  float GetQuadricASttenuation() const = 0;
	virtual  void SetExponent(float v) = 0;
	virtual  float GetExponent() const = 0;
	virtual  void SetCutoff(float v) = 0;
	virtual  float GetCutoff() const = 0;
protected:
protected:
	LIGHT_TYPE m_Type;
	LightColor m_Color;//3ÖÖÑÕÉ«
};