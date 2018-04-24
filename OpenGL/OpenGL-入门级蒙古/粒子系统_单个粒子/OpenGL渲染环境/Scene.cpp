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
Particle m_Particle(220,150,50,255,15);
bool Init()
{
	glMatrixMode(GL_PROJECTION);
	gluPerspective(50.0f, 800.0f / 600.0f, 0.1f, 1000.0f);//�����Ӿ���
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();

	m_SkyBox.Init("res/");
	//ȥ��ģ��
	m_Model.Init("res/Sphere.obj");
	m_Model.SetTexture("res/earth.bmp");
	m_Model.SetAmbientMaterialColor(0.1f, 0.1f, 0.1f);
	m_Model.SetDiffuseMaterialColor(0.8f, 0.8f, 0.8f);
	m_Model.SetSpecularMaterialColor(1.0f, 1, 1);

	//���õƹ������ϵ��
	//�����
	m_DirecionLight.SetAmbientColor(0.1f, 0.1f, 0.1f, 1.0f);//������
	m_DirecionLight.SetDiffuseColor(0.8f, 0.8f, 0.8f, 1.0f);//������
	m_DirecionLight.SetSpecularColor(1.0f, 1.0f, 1.0f, 1.0f);//���淴��
	m_DirecionLight.SetDirection(0, 1, 0);

	//���Դ
	m_PointLight.SetPosition(0, 0, 30);
	m_PointLight.SetAmbientColor(0.1f, 0.1f, 0.1f);
	m_PointLight.SetDiffuseColor(0.4f, 0.4f, 0.4f);
	m_PointLight.SetSpecularColor(1, 1, 1);
	m_PointLight.SetConstAttenuation(1);
	m_PointLight.SetLinearAttenuation(0.2f);
	m_PointLight.SetQuadricASttenuation(0.01f);

	//�۹��
	m_SpotLight.SetPosition(10, 50, -30);
	m_SpotLight.SetDirection(0, -10, 0);
	m_SpotLight.SetExponent(5);
	m_SpotLight.SetCutoff(10);
	m_SpotLight.SetAmbientColor(0.1f, 0.1f, 0.1f);
	m_SpotLight.SetDiffuseColor(0, 0.8f, 0);
	m_SpotLight.SetSpecularColor(1, 0, 0);

	//���õ���Ĳ���ϵ��
	m_Ground.SetAmbientMaterialColor(0.1f, 0.1f, 0.1f);
	m_Ground.SetDiffuseMaterialColor(0.4f, 0.4f, 0.4f);
	m_Ground.SetSpecularMaterialColor(0, 0, 0);

	m_Sprite1.SetImage("res/ny.png");
	m_Sprite1.SetRect(0, 0, 600, 337);

	
	m_Particle.SetTexture(CreateProcedureTexture(128,ALPHA_TYPE::ALPHA_GAUSSIAN));
	m_Particle.SetHalfSize(64);
	m_Particle.SetPosition(Vector3(0,0,0));

	return 1;
}
void Update()
{
	float delta = GetFrameTime();
	m_MainCamera.Update(delta);
	m_PointLight.Update(m_MainCamera.GetPosition());
	m_SpotLight.Update(m_MainCamera.GetPosition());
	m_Model.Update(delta);
	m_Particle.Update(delta);
}
void Draw()
{
	glClearColor(0, 0, 0, 1.0f);     //������ʲô��ɫ��������
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);//����ǰ������Ȼ���������ɫ������
	//�ں������
	m_MainCamera.SwitchTo3D();
	//Ҫ�Ȼ���պУ������㷨��
	m_SkyBox.Draw(m_MainCamera.GetPosition());
	m_Model.Draw();
	m_Ground.Draw();

	m_MainCamera.SwitchTo2D();
	m_Sprite1.Draw();
	m_Particle.Draw();
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
	//���ص�
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
	//X����ƶ�����up��ƫ��
	//Y�����ƶ�����right��ƫ��
	float angleRotateByUp = deltaX / 1000.0f;
	float angleRotateByRight = deltaY / 1000.0f;
	m_MainCamera.Yaw(-angleRotateByUp);
	m_MainCamera.Pitch(-angleRotateByRight);
}

