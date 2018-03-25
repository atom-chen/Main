#pragma once
#include <windows.h>
#include <gl/GL.h>
#include <functional>

class DisplayList
{
public:
	GLuint mDisplayList;
	void Init(std::function<void()>foo);
	void Draw();
};