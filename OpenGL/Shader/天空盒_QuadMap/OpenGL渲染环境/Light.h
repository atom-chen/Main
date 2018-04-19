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
	Light();
	void SetType(LIGHT_TYPE type);
	inline int GetType() const{ return this->m_Type; }

	inline void SetAmbientColor(float r, float g, float b, float a=1);//���û���������ɫ
	inline void SetDiffuseColor(float r, float g, float b, float a = 1);//������������ɫ
	inline void SetSpecularColor(float r, float g, float b, float a = 1);//���þ��淴����ɫ
	inline const vec4& GetAmbientColor() const{ return this->m_Color.GetAmbientColor(); }
	inline const vec4& GetDiffuseColor() const{ return this->m_Color.GetDiffuseColor(); }
	inline const vec4& GetSpecularColor() const{ return this->m_Color.GetSepcularColor(); }

	inline void SetConstAttenuation(float v);
	inline float GetConstAttenuation() const { return this->m_ConstAttenuation; };
	inline void SetLinearAttenuation(float v);
	inline float GetLinearAttenuation() const { return this->m_LinearAttenuation; };
	inline void SetQuadricASttenuation(float v);
	inline float GetQuadricASttenuation() const { return this->m_QuadricASttenuation; };

	inline void SetExponent(float v){ this->m_Exponent = v; }
	inline float GetExponent() const{ return this->m_Exponent; }
	inline void SetCutoff(float v){ this->m_Cutoff = v; };
	inline float GetCutoff() const{ return this->m_Cutoff; }
protected:

protected:
	LIGHT_TYPE m_Type;

	LightColor m_Color;//3����ɫ

	float m_ConstAttenuation=1;//���ó���˥������
	float m_LinearAttenuation=0.1f;//��������˥������
	float m_QuadricASttenuation=0;//����ƽ��˥������

	float m_Exponent=90;//�۹�Ʋ�˥���Ƕ�
	float m_Cutoff=180;//�۹�ƿɼ��Ƕ�
private:
};

////�����
//class DirectionLight:public Light
//{
//public:
//	DirectionLight();
//	inline void SetDirection(float x, float y, float z);
//	inline void SetDirection(const vec3& direction);
//	inline vec3 GetDirection() const { return vec3(m_Direction.x, m_Direction.y, m_Direction.z); }
//protected:
//private:
//	vec4 m_Direction;//���÷��������䷽��λ��������Զ����
//};
//
//
//class PointLight :public Light
//{
//	//˥��ϵ��f=1/(c+l*d,q*q*d)
//public:
//	PointLight();
//public:
//	inline void SetPosition(float x, float y, float z);//����λ��
//	inline void SetPosition(const vec3& position);
//	inline vec3 GetPosition() const{ return vec3(this->m_Position); };
//	inline void SetConstAttenuation(float v);
//	inline float GetConstAttenuation() const { return this->m_ConstAttenuation; };
//	inline void SetLinearAttenuation(float v);
//	inline float GetLinearAttenuation() const { return this->m_LinearAttenuation; };
//	inline void SetQuadricASttenuation(float v);
//	inline float GetQuadricASttenuation() const { return this->m_QuadricASttenuation; };
//public:
//	void Update(const vec3& cameraPos);//�����ƹ�λ�ã������������ƫ�ƣ��ƹ�Ҳ����ƫ�ƣ�
//protected:
//	vec4 m_Position;
//	float m_ConstAttenuation;//���ó���˥������
//	float m_LinearAttenuation;//��������˥������
//	float m_QuadricASttenuation;//����ƽ��˥������
//};
//
//class SpotLight :public PointLight
//{
//public:
//	SpotLight();
//public:
//	inline void SetDirection(float x, float y, float z);
//	inline void SetDirection(const vec3& position);
//	inline vec3 GetDirection() const { return vec3(m_Direction.x, m_Direction.y, m_Direction.z); }
//	inline void SetExponent(float v);
//	inline float GetExponent() const{ return this->m_Exponent; }
//	inline void SetCutoff(float v);
//	inline float GetCutoff() const{ return this->m_Cutoff; }
//private:
//	vec4 m_Direction;//���þ۹�Ƶ����䷽��
//	float m_Exponent;//�۹�Ʋ�˥���뾶
//	float m_Cutoff;//�۹�ƿɼ��뾶
//};