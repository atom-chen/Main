#pragma once
#include <cmath>
#include <algorithm>
#include <cassert>
#include<string>
#include <vector>
#include<list>
#include <deque>
#include <map>
#include <iostream>
#include <sstream>
#include <stdint.h>
#include <iterator>
using namespace std;

/************************************************************************/
/* 功能：定义引擎所用的全部自定义数据类型
   作者：
                                                                   */
/************************************************************************/
namespace B3D
{
#pragma region 类型
	class Color;         //颜色
	class Vector2;      //二维向量
	class Vector3;      //三维向量
	class Vector4;
	class Matrix44;    //4*4矩阵

	class Vertex4D;       //顶点
	class Polyon4D;       //基于顶点多边形
	class PolyonF4D;      //基于索引多边形
	class Object;          //游戏对象基类
	class Plane3D;         //3D平面
	class RenderList4D;   //渲染列表
	class Frustum;

	class Light;
#pragma endregion

	
#pragma region 引擎常量
	//窗口大小
	int WINDOWS_WIDTH = 800;
	int WINDOWS_HEIGHT = 600;
	const unsigned MaxLights = 8;

	//默认名称
	const string Default_Name = "Default";
	const string DefaultPath = "Media/";

	const float PI = 3.1415926535f;
	const float PI2 = 2 * PI;
	const float PI_DIV_2 = PI*0.5f;
	const float PI_DEV4 = PI*0.25f;
	const float PI_INV = 0.318309886f;
#pragma endregion


#pragma region 工具函数

	inline string GetPath(const string& FileURI)        //拿到文件URL
	{
		return DefaultPath + FileURI;
	}

	inline string ParseToSting(int num)        //int转String
	{
		stringstream stream;
		stream << num;
		string temp;
		stream >> temp;
		return temp;
	}

	inline int ParseToInt(const string& str)
	{
		int i = atoi(str.c_str());
		return i;
	}

	inline float ParseToFloat(const string& str)
	{
		float num = atof(str.c_str());
		return num;
	}

	template<class T>
	inline void DeletePtr(T* ptr)
	{
		if (ptr != nullptr)
		{
			delete ptr;
		}
		ptr = nullptr;
	}

	template<class T>
	inline void DeletePtrArray(T* ptr)
	{
		if (ptr != nullptr)
		{
			delete[] ptr;
		}
		ptr = nullptr;
	}
#define SafeDeleteArray(ptr)(if(ptr!=nullptr) delete [] ptr;ptr=nullptr;);
#define KEY_DOWN(vk_code)((GetAsyncKeyState(vk_code)&0x8000)?1:0);
#define KEY_UP(vk_code)((GetAsyncKeyState(vk_code)&0x8000)?0:1);

#define COLOR_16BIT(r,g,b)(((r)&0xff<<16)| ((g)&0xff)<<8) |(b)&0xff);

#pragma endregion
}