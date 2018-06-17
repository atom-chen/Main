#pragma once
#include "ggl.h"

enum  COLOR
{
	White,
	Black,
	Red,
	Blue,
	Green,
};

float GetSlope(int x0, int y0, int x1, int y1);

void DrawPoint(int x, int y,COLOR color=COLOR::White);

void DrawPoint(int x, int y, unsigned char r, unsigned char g, unsigned b, unsigned a);

void CommitPoints();