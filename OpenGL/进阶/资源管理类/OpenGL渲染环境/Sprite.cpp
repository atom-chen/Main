#include "Sprite.h"
#include "Utils.h"
#include "Resource.h"

void Sprite::SetImage(const char* path)
{
	m_Texture = ResourceManager::GetPic(path);
	SHADER_NAME.SetTexture2D(m_Texture);
}
void Sprite::Init(float x, float y, float width, float height)
{
	//×ø±ê×ª»»
	x=x*WINDOW_WIDTH - WINDOW_WIDTH / 2;
	y = y*WINDOW_HEIGHT - WINDOW_HEIGHT / 2;
	MODELMATRIX_NAME = glm::translate(x, y, -1.0f);
	VBO_NAME.Init(4);

	VBO_NAME.SetPosition(0, x, y,-2);
	VBO_NAME.SetTexcoord(0, 0, 0);
	VBO_NAME.SetColor(0,1, 1, 1);

	VBO_NAME.SetPosition(1, x + width , y,-2);
	VBO_NAME.SetTexcoord(1, 1, 0);
	VBO_NAME.SetColor(1, 1, 1, 1);

	VBO_NAME.SetPosition(2, x + width,y + height,-2);
	VBO_NAME.SetTexcoord(2, 1, 1);
	VBO_NAME.SetColor(2, 1, 1, 1);

	VBO_NAME.SetPosition(3, x, y + height,-2);
	VBO_NAME.SetTexcoord(3, 0, 1);
	VBO_NAME.SetColor(3, 1, 1, 1);

	SHADER_NAME.Init("res/texture.vert", "res/texture.frag");
}
void Sprite::Draw(glm::mat4& viewMatrix, glm::mat4 &ProjectionMatrix)
{
	glEnable(GL_BLEND);//alpha»ìºÏ
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	BEGIN
		glDrawArrays(GL_QUADS, 0, VBO_NAME.GetLenth());
	END
	glDisable(GL_BLEND);
}