#pragma once
//����ʲô����
enum DRAW_TYPE
{
	DRAW_TRIANGLES = 0x0004,
	DRAW_TRIANGLES_STRIP = 0x0005,
	DRAW_POINT = 0x1B00,
	DRAW_QUADS = 0x0007,
};
#pragma region alpha���
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

#pragma endregion alpha���


//����ѡ��
struct RenderOption
{
	DRAW_TYPE DrawType = DRAW_TRIANGLES;//�����ͼԪ
	bool DepthTest = 0;//��Ȳ�����Ϣ
	AlphaBlendInfo alphaBlend;//alpha�����Ϣ
	bool Program_Point_Size = 0;//���С��Ϣ
};


