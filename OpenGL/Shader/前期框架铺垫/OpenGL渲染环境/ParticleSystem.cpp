#include "ParticleSystem.h"
#include "Utils.h"
#include "Resource.h"
#include "Time.h"
#include "SceneManager.h"

///////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////����ϵͳ Begin/////////////////////////////////////////
void ParticleSystem::Init(const vec3& position,const int& maxNum,const char* picPath) 
{
	SetPosition(position);
	m_MaxPar = maxNum;

	m_VertexBuf.Init(m_MaxPar);//��ʼ�����ɸ�����
	m_Texture = ResourceManager::GetPic(picPath);
}


void ParticleSystem::Update()
{

}

void ParticleSystem::Draw()
{
	SceneManager::DrawGameObject(this);
}
/////////////////////////////////����ϵͳ End/////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////