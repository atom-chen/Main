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
/* ���ܣ������������õ�ȫ���Զ�����������
   ���ߣ�
                                                                   */
/************************************************************************/
namespace B3D
{
#pragma region ����
	class Color;         //��ɫ
	class Vector2;      //��ά����
	class Vector3;      //��ά����
	class Vector4;
	class Matrix44;    //4*4����

	class Vertex4D;       //����
	class Polyon4D;       //���ڶ�������
	class PolyonF4D;      //�������������
	class Object;          //��Ϸ�������
	class Plane3D;         //3Dƽ��
	class RenderList4D;   //��Ⱦ�б�
	class Frustum;

	class Light;
#pragma endregion

	
#pragma region ���泣��
	//���ڴ�С
	int WINDOWS_WIDTH = 800;
	int WINDOWS_HEIGHT = 600;
	const unsigned MaxLights = 8;

	//Ĭ������
	const string Default_Name = "Default";
	const string DefaultPath = "Media/";

	const float PI = 3.1415926535f;
	const float PI2 = 2 * PI;
	const float PI_DIV_2 = PI*0.5f;
	const float PI_DEV4 = PI*0.25f;
	const float PI_INV = 0.318309886f;
#pragma endregion


#pragma region ���ߺ���

	inline string GetPath(const string& FileURI)        //�õ��ļ�URL
	{
		return DefaultPath + FileURI;
	}

	inline string ParseToSting(int num)        //intתString
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