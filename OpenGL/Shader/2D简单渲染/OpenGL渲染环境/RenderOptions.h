#pragma once
//����ʲô����
enum DRAW_TYPE
{
	DRAW_TRIANGLES = GL_TRIANGLES,
	DRAW_TRIANGLES_STRIP = GL_TRIANGLE_STRIP,
	DRAW_POINTS = GL_POINTS,
	DRAW_QUADS = GL_QUADS,
};
#pragma region alpha���
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
#pragma endregion alpha���

struct ProgramPointSize
{
	bool isProgramPointSize = 0;
	
};
//����ѡ��
struct RenderOption
{
	DRAW_TYPE DrawType = DRAW_TRIANGLES;//�����ͼԪ
	bool DepthTest = 0;//��Ȳ�����Ϣ
	AlphaBlendInfo alphaBlend;//alpha�����Ϣ
	bool Program_Point_Size = 0;//���С��Ϣ
};


