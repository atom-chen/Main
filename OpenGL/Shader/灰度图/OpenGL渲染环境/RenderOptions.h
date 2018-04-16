#pragma once
//绘制什么类型
enum DRAW_TYPE
{
	DRAW_TRIANGLES = 0x0004,
	DRAW_TRIANGLES_STRIP = 0x0005,
	DRAW_POINT = 0x1B00,
	DRAW_QUADS = 0x0007,
};
#pragma region alpha混合
enum ALPHA_BLEND_TYPE
{
	ALPHA_BLEND_ONE = 1,
	ALPHA_BLEND_ONE_MINUS_SRC_ALPHA = 0x0303,
};
struct AlphaBlendInfo
{
	bool AlphaBlend = 0;
	ALPHA_BLEND_TYPE Type = ALPHA_BLEND_TYPE::ALPHA_BLEND_ONE;
};

#pragma endregion alpha混合


//绘制选项
struct RenderOption
{
	DRAW_TYPE DrawType = DRAW_TRIANGLES;//所需的图元
	bool DepthTest = 0;//深度测试信息
	AlphaBlendInfo alphaBlend;//alpha混合信息
	bool Program_Point_Size = 0;//点大小信息
};


