#include "TextureAmimation.h"

TextureAmimation::TextureAmimation()
{
	m_CurFrame = 1;
}

void TextureAmimation::Init(float x, float y, float widget, float height,int32_t durationTime,int32_t rowCount,int32_t colCount)
{
	UITexture::Init(x, y, widget, height);
	this->m_DurationTime = durationTime;
	this->m_Lie = colCount;//一共几列
	this->m_Hand = rowCount;//一共几行
	
	m_FrameTime=m_Hand*m_Lie / m_DurationTime;
	m_Timer = m_FrameTime;
}
void TextureAmimation::Update(float deltaTime)
{
	m_Timer += deltaTime;
	if (m_Timer >= m_FrameTime)
	{
		m_Timer = 0;
		if (m_CurFrame > m_Lie*m_Hand)
		{
			m_CurFrame = 1;

			//缩放比例
			float   fR = 1.0f / m_Hand;
			float   fC = 1.0f / m_Lie;

			//! 计算当前行
			int cR = int(m_CurFrame / m_Lie);
			//! 计算当前列
			int cC = int(m_CurFrame - cR * m_Lie);

			for (int i = 0; i < 6; ++i)
			{
				//! 对uv坐标进行缩放（大小）
				position[i].u *= fR;
				position[i].v *= fC;

				//! 对uv坐标进行平移(修正位置)
				position[i].u += cC * fC;
				position[i].v += cR * fR;
			}
	}
}
void TextureAmimation::Draw(glm::mat4& viewMatrix, glm::mat4 &ProjectionMatrix)
{
	UITexture::Draw(viewMatrix, ProjectionMatrix);
}