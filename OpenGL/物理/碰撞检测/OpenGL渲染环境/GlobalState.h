#pragma once
#include "RenderOptions.h"


struct ScissorState
{
	bool m_IsScissor = 0;
	float xStart = 0;
	float yStart = 0;
	float width = WINDOW_WIDTH;
	float height = WINDOW_HEIGHT;
	ScissorState()
	{

	}
	ScissorState(const ScissorState& other) :m_IsScissor(other.m_IsScissor), xStart(other.xStart), yStart(other.yStart), width(other.width), height(other.height)
	{

	}
	ScissorState(bool IsOpen, float xStart, float yStart, float width, float height) :m_IsScissor(IsOpen), xStart(xStart), yStart(yStart), width(width), height(height)
	{


	}
	inline bool operator==(const ScissorState& other)
	{
		if (m_IsScissor == other.m_IsScissor && xStart == other.xStart && yStart == other.yStart &&  width == other.width && height == other.height)
		{
			return 1;
		}
		return 0;
	}
	inline bool operator!=(const ScissorState& other)
	{
		if (m_IsScissor == other.m_IsScissor && xStart == other.xStart && yStart == other.yStart &&  width == other.width && height == other.height)
		{
			return 0;
		}
		return 1;
	}
};