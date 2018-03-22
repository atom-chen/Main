#include "Scene.h"
#include "Utils.h"
#include "SkyBox.h"
#include "Model.h"
#include "Ground.h"
#include "Light.h"
#include "Camera.h"


static SkyBox m_SkyBox;
static Model m_Model;
static Ground m_Ground;
static DirectionLight m_DirecionLight(GL_LIGHT0);
static PointLight m_PointLight(GL_LIGHT1);
static SpotLight m_SpotLight(GL_LIGHT2);
Camera m_MainCamera;
bool Init()
{
	glMatrixMode(GL_PROJECTION);
	gluPerspective(50.0f, 800.0f / 600.0f, 0.1f, 1000.0f);//定义视景体
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
	m_SkyBox.Init("res/");
	//去读模型
	m_Model.Init("res/Sphere.obj");
	m_Model.SetTexture("res/earth.bmp");
	m_Model.SetAmbientMaterialColor(0.1f, 0.1f, 0.1f);
	m_Model.SetDiffuseMaterialColor(0.8f, 0.8f, 0.8f);
	m_Model.SetSpecularMaterialColor(1.0f, 1, 1);

	//设置灯光的照射系数
	//方向光
	m_DirecionLight.SetAmbientColor(0.1f, 0.1f, 0.1f, 1.0f);//环境光
	m_DirecionLight.SetDiffuseColor(0.8f, 0.8f, 0.8f, 1.0f);//漫反射
	m_DirecionLight.SetSpecularColor(1.0f, 1.0f, 1.0f, 1.0f);//镜面反射
	m_DirecionLight.SetDirection(0, 1, 0);

	//点光源
	m_PointLight.SetPosition(0, 0, 30);
	m_PointLight.SetAmbientColor(0.1f, 0.1f, 0.1f);
	m_PointLight.SetDiffuseColor(0.4f, 0.4f, 0.4f);
	m_PointLight.SetSpecularColor(1, 1, 1);
	m_PointLight.SetConstAttenuation(1);
	m_PointLight.SetLinearAttenuation(0.2f);
	m_PointLight.SetQuadricASttenuation(0.01f);

	//聚光灯
	m_SpotLight.SetPosition(10, 50, -30);
	m_SpotLight.SetDirection(0, -10, 0);
	m_SpotLight.SetExponent(5);
	m_SpotLight.SetCutoff(10);
	m_SpotLight.SetAmbientColor(0.1f, 0.1f, 0.1f);
	m_SpotLight.SetDiffuseColor(0, 0.8f, 0);
	m_SpotLight.SetSpecularColor(1, 0, 0);

	//设置地面的材质系数
	m_Ground.SetAmbientMaterialColor(0.1f, 0.1f, 0.1f);
	m_Ground.SetDiffuseMaterialColor(0.4f, 0.4f, 0.4f);
	m_Ground.SetSpecularMaterialColor(0, 0, 0);

	return 1;
}
void Draw()
{
	glClearColor(0, 0, 0, 1.0f);     //设置用什么颜色擦缓冲区
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);//作画前擦除深度缓冲区和颜色缓冲区
	//摆好摄像机
	m_MainCamera.Update(g_frame);

	//要先画天空盒（画家算法）
	m_PointLight.Enable(0);
	m_DirecionLight.Enable(1);
	m_Model.Draw();
}
void OnKeyDown(char KeyCode)
{
	switch (KeyCode)
	{


	}
}
void OnKeyUp(char KeyCode)
{

}

void OnMouseMove(float deltaX, float deltaY)
{

}

