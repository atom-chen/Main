#pragma once
#include "ggl.h"
#include "Transform.h"

//不需要被渲染的物体继承此类
class Node
{
public:
protected:
	Transform m_Transform;
private:
};