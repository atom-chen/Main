#pragma once
#include "B3DClass.h"

namespace B3D
{
	class Color
	{
	public:                         //ÊôÐÔ
		unsigned char r, g, b, a;
	public:                         //¹¹Ôì
		Color(unsigned char r = 0, unsigned char g = 0, unsigned char b = 0, unsigned char a = 1) :r(r), g(g), b(b), a(a)
		{

		};
		Color(const Color& color)
		{
			this->a = color.a;
			this->g = color.g;
			this->b = color.b;
			this->r = color.r;
		}
		Color& operator=(const Color& color);
	public:
	private:


	};
}
