#pragma once
#include "ggl.h"

struct Transform
{
public:
	Transform() :m_Position(0, 0, 0), m_Rotate(0, 0, 0), m_Scale(1,1,1)
	{
		
	}
	vec3 m_Position, m_Scale, m_Rotate;//旋转表示绕自身的旋转
protected:
private:
};