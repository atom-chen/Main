#include "TextureAmimation.h"

TextureAmimation::TextureAmimation()
{
	m_CurFrame = 1;
}

void TextureAmimation::Init(float x, float y, float widget, float height,int32_t durationTime,int32_t rowCount,int32_t colCount)
{
	UITexture::Init(x, y, widget, height);
	this->m_DurationTime = durationTime;
	this->m_Lie = colCount;//һ������
	this->m_Hand = rowCount;//һ������
	
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

			//���ű���
			float   fR = 1.0f / m_Hand;
			float   fC = 1.0f / m_Lie;

			//! ���㵱ǰ��
			int cR = int(m_CurFrame / m_Lie);
			//! ���㵱ǰ��
			int cC = int(m_CurFrame - cR * m_Lie);

			for (int i = 0; i < 6; ++i)
			{
				//! ��uv����������ţ���С��
				position[i].u *= fR;
				position[i].v *= fC;

				//! ��uv�������ƽ��(����λ��)
				position[i].u += cC * fC;
				position[i].v += cR * fR;
			}
	}
}
void TextureAmimation::Draw(glm::mat4& viewMatrix, glm::mat4 &ProjectionMatrix)
{
	UITexture::Draw(viewMatrix, ProjectionMatrix);
}