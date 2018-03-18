#include "ggl.h"

class Light
{
public:
	void SetAmbientColor(float r, float g, float b, float a);//设置环境照射颜色
	void SetDiffuseColor(float r, float g, float b, float a);//设置漫反射颜色
	void SetSpecularColor(float r, float g, float b, float a);//设置镜面反射颜色
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
	void SetDirection(float x, float y, float z);//设置方向光的照射方向（位置在无穷远处）
protected:
private:
};