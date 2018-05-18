#include "TextureAmimation.h"

TextureAmimation::TextureAmimation()
{
	m_CurFrame = 1;
}

void TextureAmimation::Init(float x, float y, float widget, float height,int32_t durationTime,int32_t rowCount,int32_t colCount)
{
	Sprite::Init(x, y, widget, height);
	this->m_DurationTime = durationTime;
	this->m_Col = colCount;//һ������
	this->m_Row = rowCount;//һ������
	
	m_FrameTime=m_Row*m_Col / m_DurationTime;
	m_Timer = m_FrameTime;
}
void TextureAmimation::Update(float deltaTime)
{
	m_Timer += deltaTime;
	if (m_Timer >= m_FrameTime)
	{
		m_Timer = 0;
		if (m_CurFrame > m_Col*m_Row)
		{
			m_CurFrame = 1;
		}
		int32_t curRow = int32_t(m_CurFrame / m_Col);//��ǰ�ڵڼ���
		int32_t curCol = int32_t(m_CurFrame - curRow*m_Col);//��ǰ�ڵڼ���

		//��������ֵ�趨texcoord
	}
}
void TextureAmimation::Draw(glm::mat4& viewMatrix, glm::mat4 &ProjectionMatrix)
{

}