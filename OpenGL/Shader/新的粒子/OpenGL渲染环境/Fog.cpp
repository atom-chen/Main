#include "Fog.h"


bool FogObj::Init(const char* path, const char* vertexShader, const char* fragmentShader)
{
	if (GameObject::Init(path, vertexShader, fragmentShader))
	{
		SetFloat("U_FogNear", m_FogNear);
		SetFloat("U_FogFar", m_FogFar);
		SetFloat("U_FogMul", m_Mul);
		SetFloat("U_FogPow", m_Pow);
		return 1;
	}
	return 0;
}
void FogObj::SetFogNear(float v)
{
	m_FogNear = v;
	SetFloat("U_FogNear", m_FogNear);
}
void FogObj::SetFogFar(float v)
{
	m_FogFar = v;
	SetFloat("U_FogFar", m_FogFar);
}
void FogObj::SetExp(float v)
{
	m_Mul = v;
	SetFloat("U_FogMul", m_Mul);
}
void FogObj::SetExpEx(float v)
{
	m_Pow = v;
	SetFloat("U_FogPow", m_Pow);
}