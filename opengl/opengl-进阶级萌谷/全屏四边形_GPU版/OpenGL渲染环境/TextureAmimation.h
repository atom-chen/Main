#pragma omce
#include "ggl.h"
#include "Resource.h"
#include "Sprite.h"

//��ͼ����
class TextureAmimation:public Sprite
{
public:
	TextureAmimation();
public:
	void Init(float x, float y, float widget, float height, int32_t durationTime, int32_t rowCount,int32_t colCount);
	void Update(float deltaTime);
	virtual void Draw(glm::mat4& viewMatrix, glm::mat4 &ProjectionMatrix);
private:
	int32_t m_CurFrame = 0;//��ǰ�ڼ�֡
	int32_t m_Row;//������
	int32_t m_Col;//������
	float m_DurationTime = 0;//��������ʱ��

	float m_Timer;//��ʱ��
	float m_FrameTime;//ÿ֡���ʱ��
};