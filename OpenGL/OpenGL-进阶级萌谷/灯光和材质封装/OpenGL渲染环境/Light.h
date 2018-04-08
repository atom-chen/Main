#pragma  once
#include "ggl.h"
#include "LightColor.h"
enum LIGHT_TYPE
{
	LIGHT_INVALID=0,//不使用该灯
	LIGHT_DIRECTION=1,
	LIGHT_POINT=2,
	LIGHT_SPECULAR=3,
};


class Light
{
public:
	Light();
	void SetType(LIGHT_TYPE type);
	inline int GetType() const{ return this->m_Type; }

	inline void SetAmbientColor(float r, float g, float b, float a=1);//设置环境照射颜色
	inline void SetDiffuseColor(float r, float g, float b, float a = 1);//设置漫反射颜色
	inline void SetSpecularColor(float r, float g, float b, float a = 1);//设置镜面反射颜色
	inline const vec4& GetAmbientColor() const{ return this->m_Color.GetAmbientColor(); }
	inline const vec4& GetDiffuseColor() const{ return this->m_Color.GetDiffuseColor(); }
	inline const vec4& GetSpecularColor() const{ return this->m_Color.GetSepcularColor(); }

	inline void SetDirection(float x, float y, float z);
	inline void SetDirection(const vec3& direction);
	inline vec4 GetDirection() const { return m_Direction; }

	inline void SetPosition(float x, float y, float z);
	inline void SetPosition(const vec3& position);
	inline vec4 GetPosition() const{ return this->m_Position; };
	inline void SetConstAttenuation(float v);
	inline float GetConstAttenuation() const { return this->m_ConstAttenuation; };
	inline void SetLinearAttenuation(float v);
	inline float GetLinearAttenuation() const { return this->m_LinearAttenuation; };
	inline void SetQuadricASttenuation(float v);
	inline float GetQuadricASttenuation() const { return this->m_QuadricASttenuation; };

	inline void SetExponent(float v);
	inline float GetExponent() const{ return this->m_Exponent; }
	inline void SetCutoff(float v);
	inline float GetCutoff() const{ return this->m_Cutoff; }
protected:

protected:
	LIGHT_TYPE m_Type;

	LightColor m_Color;//3种颜色
	vec4 m_Direction;//光照方向
	vec4 m_Position;//光源位置

	float m_ConstAttenuation=1;//设置常数衰减因子
	float m_LinearAttenuation=0.1f;//设置线性衰减因子
	float m_QuadricASttenuation=0;//设置平方衰减因子

	float m_Exponent=5;//聚光灯不衰减半径
	float m_Cutoff=10;//聚光灯可见半径
private:
};

//方向光
class DirectionLight:public Light
{
public:
	DirectionLight();
	inline void SetDirection(float x, float y, float z);
	inline void SetDirection(const vec3& direction);
	inline vec3 GetDirection() const { return vec3(m_Direction.x, m_Direction.y, m_Direction.z); }
protected:
private:
	vec4 m_Direction;//设置方向光的照射方向（位置在无穷远处）
};


class PointLight :public Light
{
	//衰减系数f=1/(c+l*d,q*q*d)
public:
	PointLight();
public:
	inline void SetPosition(float x, float y, float z);//设置位置
	inline void SetPosition(const vec3& position);
	inline vec3 GetPosition() const{ return vec3(this->m_Position); };
	inline void SetConstAttenuation(float v);
	inline float GetConstAttenuation() const { return this->m_ConstAttenuation; };
	inline void SetLinearAttenuation(float v);
	inline float GetLinearAttenuation() const { return this->m_LinearAttenuation; };
	inline void SetQuadricASttenuation(float v);
	inline float GetQuadricASttenuation() const { return this->m_QuadricASttenuation; };
public:
	void Update(const vec3& cameraPos);//修正灯光位置（由于摄像机的偏移，灯光也跟着偏移）
protected:
	vec4 m_Position;
	float m_ConstAttenuation;//设置常数衰减因子
	float m_LinearAttenuation;//设置线性衰减因子
	float m_QuadricASttenuation;//设置平方衰减因子
};

class SpotLight :public PointLight
{
public:
	SpotLight();
public:
	inline void SetDirection(float x, float y, float z);
	inline void SetDirection(const vec3& position);
	inline vec3 GetDirection() const { return vec3(m_Direction.x, m_Direction.y, m_Direction.z); }
	inline void SetExponent(float v);
	inline float GetExponent() const{ return this->m_Exponent; }
	inline void SetCutoff(float v);
	inline float GetCutoff() const{ return this->m_Cutoff; }
private:
	vec4 m_Direction;//设置聚光灯的照射方向
	float m_Exponent;//聚光灯不衰减半径
	float m_Cutoff;//聚光灯可见半径
};