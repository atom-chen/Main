#pragma once
#include <windows.h>
#include <gl/GL.h>
#include "texture.h"
#include "displaylist.h"

class SkyBox
{
public:
	DisplayList mSkyBox;
	Texture*front, *back, *right, *left, *top, *bottom;
	void Init(const char*path);
	void Draw(float x,float y,float z);
};