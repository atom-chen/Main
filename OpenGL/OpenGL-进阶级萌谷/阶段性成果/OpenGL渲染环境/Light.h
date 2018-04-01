#include "ggl.h"

class Light
{
public:
	void SetAmbientColor(float r, float g, float b, float a=1);//设置环境照射颜色
	void SetDiffuseColor(float r, float g, float b, float a=1);//设置漫反射颜色
	void SetSpecularColor(float r, float g, float b, float a=1);//设置镜面反射颜色
	void Enable(bool isOpen);
	bool IsEnable();
protected:
	Light();
protected:
	GLenum m_LightID;
	bool m_IsEnable=false;
private:
};

//方向光
class DirectionLight:public Light
{
public:
	DirectionLight(GLenum ID);
	void SetDirection(float x, float y, float z);//设置方向光的照射方向（位置在无穷远处）
protected:
private:
};


class PointLight :public Light
{
public:
	PointLight(GLenum ID);
public:
	//衰减系数f=1/(c+l*d,q*q*d)
	void SetPosition(float x, float y, float z);//设置位置
	void SetConstAttenuation(float v);//设置常数衰减因子
	void SetLinearAttenuation(float v);//设置线性衰减因子
	void SetQuadricASttenuation(float v);//设置平方衰减因子
	void Update(vec3 cameraPos);//修正灯光位置（由于摄像机的偏移，灯光也跟着偏移）
private:
	float m_Position[3];
};

class SpotLight :public PointLight
{
public:
	SpotLight(GLenum ID);
public:
	void SetDirection(float x, float y, float z);//设置聚光灯的照射方向
	void SetExponent(float v);//聚光灯不衰减角度
	void SetCutoff(float v);//聚光灯可见角度

};