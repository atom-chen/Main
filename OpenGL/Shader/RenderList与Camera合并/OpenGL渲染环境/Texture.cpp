#include "Texture.h"
#include "Resource.h"
#include "SceneManager.h"


void UITexture::SetImage(const char* path)
{
	m_Texture = ResourceManager::GetPic(path);
	SHADER_NAME.SetTexture2D(m_Texture);
}
void UITexture::SetImage(GLuint texture)
{
	m_Texture = texture;
	SHADER_NAME.SetTexture2D(texture);
}
void UITexture::Init(float x, float y, float width, float height, const char* vertShader)
{
	VBO_NAME.Init(4);

	VBO_NAME.SetPosition(0, x, y, -2);
	VBO_NAME.SetTexcoord(0, 0, 0);
	VBO_NAME.SetColor(0, 1, 1, 1);

	VBO_NAME.SetPosition(1, x + width, y, -2);
	VBO_NAME.SetTexcoord(1, 1, 0);
	VBO_NAME.SetColor(1, 1, 1, 1);

	VBO_NAME.SetPosition(2, x, y + height, -2);
	VBO_NAME.SetTexcoord(2, 0, 1);
	VBO_NAME.SetColor(2, 1, 1, 1);

	VBO_NAME.SetPosition(3, x + width, y + height, -2);
	VBO_NAME.SetTexcoord(3, 1, 1);
	VBO_NAME.SetColor(3, 1, 1, 1);

	SHADER_NAME.Init(SHADER_ROOT"texture.vert", SHADER_ROOT"texture.frag");
}

void UITexture::Draw(glm::mat4& viewMatrix, glm::mat4 &ProjectionMatrix)
{
	//SceneManager::SetBlendState(1);
	//SceneManager::SetDepthTestState(0);
	BEGIN
		glDrawArrays(GL_TRIANGLE_STRIP, 0, VBO_NAME.GetLenth());
	END
	//SceneManager::SetBlendState(0);
}

void UITexture::SetPosition(float x, float y, float width, float height)
{
	VBO_NAME.SetPosition(0, x, y, -0.2f);
	VBO_NAME.SetTexcoord(0, 0, 0);
	VBO_NAME.SetColor(0, 1, 1, 1);

	VBO_NAME.SetPosition(1, x + width, y, -0.2f);
	VBO_NAME.SetTexcoord(1, 1, 0);
	VBO_NAME.SetColor(1, 1, 1, 1);

	VBO_NAME.SetPosition(2, x, y + height, -0.2f);
	VBO_NAME.SetTexcoord(2, 0, 1);
	VBO_NAME.SetColor(2, 1, 1, 1);

	VBO_NAME.SetPosition(3, x + width, y + height, -0.2f);
	VBO_NAME.SetTexcoord(3, 1, 1);
	VBO_NAME.SetColor(3, 1, 1, 1);
}