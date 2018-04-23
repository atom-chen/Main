#pragma once

#include "ggl.h"

//��GPU����һ������
class Vertex
{
public:
	Vertex();
	void CopyFrom(const Vertex& from);
	void operator=(const Vertex& from);
public:
	void SetPosition(const float& x, const float& y, const float& z=1, const float& w=1);
	void SetNormal(const float& x, const float& y, const float& z=1, const float& w=1);
	void SetTexcoord(const float& x, const float& y, const float& z=1, const float& w=1);
	void SetColor(const float& r, const float& g, const float& b, const float& a = 1);

	inline vec4 GetNormal() const{ return vec4(m_Normal[0], m_Normal[1], m_Normal[2], m_Normal[3]); };

protected:
private:
	float m_Position[4];
	float m_Color[4];
	float m_Texcoord[4];
	float m_Normal[4];
};
