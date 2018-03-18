#include "ggl.h"

class Light
{
public:
	void SetAmbientColor(float r, float g, float b, float a);//���û���������ɫ
	void SetDiffuseColor(float r, float g, float b, float a);//������������ɫ
	void SetSpecularColor(float r, float g, float b, float a);//���þ��淴����ɫ
	void Enable(bool isOpen);
protected:
	Light();
protected:
	GLenum m_LightID;
private:
};

class DirectionLight:public Light
{
public:
	DirectionLight(GLenum ID);
	void SetDirection(float x, float y, float z);//���÷��������䷽��λ��������Զ����
protected:
private:
};