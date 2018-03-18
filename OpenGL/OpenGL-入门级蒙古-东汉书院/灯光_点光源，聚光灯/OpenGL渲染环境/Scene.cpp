#include "Scene.h"
#include "Utils.h"
#include "SkyBox.h"
#include "Model.h"
#include "Ground.h"
#include "Light.h"

static SkyBox m_SkyBox;
static Model m_Model;
static Ground m_Ground;
static DirectionLight m_DirecionLight(GL_LIGHT0);
static PointLight m_PointLight(GL_LIGHT1);
static 
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

	//���õ���Ĳ���ϵ��
	m_Ground.SetAmbientMaterialColor(0.1f, 0.1f, 0.1f);
	m_Ground.SetDiffuseMaterialColor(0.4f, 0.4f, 0.4f);
	m_Ground.SetSpecularMaterialColor(0, 0, 0);

	return 1;
}
void Draw()
{
	glClearColor(0, 0, 0, 1.0f);     //������ʲô��ɫ��������
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);//����ǰ������Ȼ���������ɫ������
	//Ҫ�Ȼ���պУ������㷨��
	m_PointLight.Enable(0);
	m_DirecionLight.Enable(1);
	m_Model.Draw();
}

