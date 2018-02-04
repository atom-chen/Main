#pragma once
#include <cmath>
#include <algorithm>
#include <cassert>
#include<string>
#include <vector>
#include<list>
#include <deque>
#include <iostream>
#include <stdint.h>
using namespace std;

/************************************************************************/
/* ���ܣ������������õ�ȫ���Զ�����������
   ���ߣ�
                                                                   */
/************************************************************************/
namespace B3D
{
	class Color;         //��ɫ
	class Vector2;      //��ά����
	class Vector3;      //��ά����
	class Vector4;
	class Matrix44;

	class Vertex4D;       //����
	class Polyon4D;       //���ڶ�������
	class PolyonF4D;      //�������������
	class Object;
	class Plane3D;
	class RenderList4D;
	class Frustum;
}