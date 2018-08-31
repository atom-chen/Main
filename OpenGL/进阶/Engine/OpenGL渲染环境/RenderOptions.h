#pragma once
//绘制什么类型
enum DRAW_TYPE
{
	DRAW_TRIANGLES = GL_TRIANGLES,
	DRAW_TRIANGLES_STRIP = GL_TRIANGLE_STRIP,
	DRAW_POINTS = GL_POINTS,
	DRAW_QUADS = GL_QUADS,
};
#pragma region alpha混合
enum ALPHA_BLEND_TYPE
{
	ALPHA_BLEND_ONE = GL_ONE,
	ALPHA_BLEND_ONE_MINUS_SRC_ALPHA = GL_ONE_MINUS_SRC_ALPHA,
};
struct AlphaBlendInfo
{
	bool AlphaBlend = 0;
	ALPHA_BLEND_TYPE Type = ALPHA_BLEND_TYPE::ALPHA_BLEND_ONE;
};
#pragma endregion alpha混合

struct ProgramPointSize
{
	bool isProgramPointSize = 0;
	
};
//绘制选项
struct RenderOption
{
	DRAW_TYPE DrawType = DRAW_TRIANGLES;//所需的图元
	bool DepthTest = 0;//深度测试信息
	AlphaBlendInfo alphaBlend;//alpha混合信息
	bool Program_Point_Size = 0;//点大小信息
};


