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
/* 功能：定义引擎所用的全部自定义数据类型
   作者：
                                                                   */
/************************************************************************/
namespace B3D
{
	class Color;         //颜色
	class Vector2;      //二维向量
	class Vector3;      //三维向量
	class Vector4;
	class Matrix44;

	class Vertex4D;       //顶点
	class Polyon4D;       //基于顶点多边形
	class PolyonF4D;      //基于索引多边形
	class Object;
	class Plane3D;
	class RenderList4D;
	class Frustum;
}