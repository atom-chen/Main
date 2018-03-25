#include "Scene.h"
#include "Utils.h"
#include "SkyBox.h"
#include "Model.h"
#include "Ground.h"
#include "Light.h"
#include "Camera.h"
#include "Sprite.h"
#include "ParticleSystem.h"

SkyBox m_SkyBox;
Model m_Model;
Ground m_Ground;
DirectionLight m_DirecionLight(GL_LIGHT0);
PointLight m_PointLight(GL_LIGHT1);
SpotLight m_SpotLight(GL_LIGHT2);
Camera m_MainCamera;
Sprite m_Sprite1;
ParticleSystem m_ParticleSys;
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

	m_Sprite1.SetImage("res/ny.png");
	m_Sprite1.SetRect(0, 0, 600, 337);
	


	return 1;
}
void Update()
{
	float delta = GetFrameTime();
	m_MainCamera.Update(delta);
	m_PointLight.Update(m_MainCamera.GetPosition());
	m_SpotLight.Update(m_MainCamera.GetPosition());
	m_Model.Update(delta);
	m_ParticleSys.Update(delta);
}
void Draw()
{
	glClearColor(0, 0, 0, 1.0f);     //设置用什么颜色擦缓冲区
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);//作画前擦除深度缓冲区和颜色缓冲区
	//摆好摄像机
	m_MainCamera.SwitchTo3D();
	//要先画天空盒（画家算法）
	m_SkyBox.Draw(m_MainCamera.GetPosition());
	m_Model.Draw();
	m_Ground.Draw();

	m_MainCamera.SwitchTo2D();
	m_Sprite1.Draw();
	m_ParticleSys.Draw();
}
void OnKeyDown(char KeyCode)
{
	switch (KeyCode)
	{
	case 'A':
		m_MainCamera.MoveToLeft(1);
		break;
	case 'D':
		m_MainCamera.MoveToRight(1);
		break;
	case 'W':
		m_MainCamera.MoveToTop(1);
		break;
	case 'S':
		m_MainCamera.MoveToBottom(1);
		break;
	case 'Q':
		m_MainCamera.MoveToFront(1);
		break;
	case 'E':
		m_MainCamera.MoveToBack(1);
		break;
	//开关灯
	case 48:
		m_DirecionLight.Enable(!m_DirecionLight.IsEnable());
		break;
	case 49:
		m_PointLight.Enable(!m_PointLight.IsEnable());
		break;
	case 50:
		m_SpotLight.Enable(!m_SpotLight.IsEnable());
		break;
	case VK_LEFT:
		m_Model.MoveToLeft(1);
		break;
	case  VK_RIGHT:
		m_Model.MoveToRight(1);
		break;
	case VK_UP:
		m_Model.MoveToTop(1);
		break;
	case VK_DOWN:
		m_Model.MoveToBottom(1);
		break;
	case VK_F2:
		m_MainCamera.SwitchTo2D();
		break;
	case VK_F3:
		m_MainCamera.SwitchTo3D();
		break;
	}
}

void OnKeyUp(char KeyCode)
{
	switch (KeyCode)
	{
	case 'A':
		m_MainCamera.MoveToLeft(0);
		break;
	case 'D':
		m_MainCamera.MoveToRight(0);
		break;
	case 'W':
		m_MainCamera.MoveToTop(0);
		break;
	case 'S':
		m_MainCamera.MoveToBottom(0);
		break;
	case 'Q':
		m_MainCamera.MoveToFront(0);
		break;
	case 'E':
		m_MainCamera.MoveToBack(0);
		break;
	case VK_LEFT:
		m_Model.MoveToLeft(0);
		break;
	case  VK_RIGHT:
		m_Model.MoveToRight(0);
		break;
	case VK_UP:
		m_Model.MoveToTop(0);
		break;
	case VK_DOWN:
		m_Model.MoveToBottom(0);
		break;
	}
}

void OnMouseMove(float deltaX, float deltaY)
{
	//X鼠标移动导致up的偏移
	//Y鼠标的移动导致right的偏移
	float angleRotateByUp = deltaX / 1000.0f;
	float angleRotateByRight = deltaY / 1000.0f;
	m_MainCamera.Yaw(-angleRotateByUp);
	m_MainCamera.Pitch(-angleRotateByRight);
}

