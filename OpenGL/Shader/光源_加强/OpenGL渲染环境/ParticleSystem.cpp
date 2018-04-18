#include "ParticleSystem.h"
#include "Utils.h"
#include "Resource.h"
#include "Time.h"
#include "SceneManager.h"

///////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////粒子系统 Begin/////////////////////////////////////////
void ParticleSystem::Init(const vec3& position,const int& maxNum,const char* picPath) 
{
	if (!m_IsInit)
	{
		m_IsInit = 1;
		SetPosition(position);
		m_MaxPar = maxNum;

		m_VertexBuf.Init(m_MaxPar);//初始化若干个顶点
	}
}


void ParticleSystem::Update()
{

}

void ParticleSystem::Draw()
{
	INIT_TEST_VOID
	SceneManager::DrawGameObject(this);
}
void ParticleSystem::Destory()
{
	if (m_IsInit)
	{
		RenderAble::Destory();

	}
}
/////////////////////////////////粒子系统 End/////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////