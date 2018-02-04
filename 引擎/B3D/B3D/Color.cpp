#include "Color.h"

namespace B3D
{
	Color& Color::operator=(const Color& color)
	{
		this->r = color.a;
		this->g = color.g;
		this->b = color.b;
		this->a = color.a;
		return *this;
	}	

}
