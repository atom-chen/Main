#include "ggl.h"

class Light
{
public:
	void SetAmbientColor(float r, float g, float b, float a=1);//���û���������ɫ
	void SetDiffuseColor(float r, float g, float b, float a=1);//������������ɫ
	void SetSpecularColor(float r, float g, float b, float a=1);//���þ��淴����ɫ
	void Enable(bool isOpen);
	bool IsEnable();
protected:
	Light();
protected:
	GLenum m_LightID;
	bool m_IsEnable=false;
private:
};

//�����
class DirectionLight:public Light
{
public:
	DirectionLight(GLenum ID);
	void SetDirection(float x, float y, float z);//���÷��������䷽��λ��������Զ����
protected:
private:
};


class PointLight :public Light
{
public:
	PointLight(GLenum ID);
public:
	//˥��ϵ��f=1/(c+l*d,q*q*d)
	void SetPosition(float x, float y, float z);//����λ��
	void SetConstAttenuation(float v);//���ó���˥������
	void SetLinearAttenuation(float v);//��������˥������
	void SetQuadricASttenuation(float v);//����ƽ��˥������
	void Update(vec3 cameraPos);//�����ƹ�λ�ã������������ƫ�ƣ��ƹ�Ҳ����ƫ�ƣ�
private:
	float m_Position[3];
};

class SpotLight :public PointLight
{
public:
	SpotLight(GLenum ID);
public:
	void SetDirection(float x, float y, float z);//���þ۹�Ƶ����䷽��
	void SetExponent(float v);//�۹�Ʋ�˥���Ƕ�
	void SetCutoff(float v);//�۹�ƿɼ��Ƕ�

};