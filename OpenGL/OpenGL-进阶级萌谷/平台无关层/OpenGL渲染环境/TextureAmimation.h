#pragma omce
#include "ggl.h"
#include "Resource.h"
#include "Texture.h"

//贴图动画
class TextureAmimation:public UITexture
{
public:
	TextureAmimation();
public:
	void Init(float x, float y, float widget, float height, int32_t durationTime, int32_t rowCount,int32_t colCount);
	void Update(float deltaTime);
	virtual void Draw(glm::mat4& viewMatrix, glm::mat4 &ProjectionMatrix);
private:
	int32_t m_CurFrame = 0;//当前第几帧
	int32_t m_Hand;//总行数
	int32_t m_Lie;//总列数
	float m_DurationTime = 0;//动画持续时间

	float m_Timer;//计时器
	float m_FrameTime;//每帧间隔时间
};