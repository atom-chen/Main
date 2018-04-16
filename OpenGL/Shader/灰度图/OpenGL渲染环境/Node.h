#pragma once
#include "ggl.h"
#include "Transform.h"

//����Ҫ����Ⱦ������̳д���
class Node
{
public:
	virtual void SetPosition(float x, float y, float z);
	inline const vec3& GetPosition() const{ return m_Transform.m_Position; };

	virtual void SetRotate(float x, float y, float z);
	inline const vec3& GetRotate() const{ return m_Transform.m_Rotate; };

	virtual void SetScale(float x, float y, float z);
	inline const vec3& GetScale() const{ return m_Transform.m_Scale; };
protected:
	Transform m_Transform;
};