#include "Texture.h"
#include "Resource.h"
#include "SceneManager.h"


void UITexture::SetImage(const char* path)
{
	INIT_TEST_VOID
	m_Shader.SetTexture2D(path);
}
void UITexture::Init(float x, float y, float width, float height, const char* picPath, const char* vertShader, const char* fargShader,const char* picNameInShader)
{
	if (m_IsInit)
	{
		return;
	}
	m_IsInit = 1;
	RenderAble::SetPosition(x, y, 0);
	VBO_NAME.Init(4);

	VBO_NAME.SetPosition(0, 0, height, 0);
	VBO_NAME.SetTexcoord(0, 0, 1);
	VBO_NAME.SetColor(0, 1, 1, 1);

	VBO_NAME.SetPosition(1, 0, 0, 0);
	VBO_NAME.SetTexcoord(1, 0, 0);
	VBO_NAME.SetColor(1, 1, 1, 1);

	VBO_NAME.SetPosition(2, width, height, 0);
	VBO_NAME.SetTexcoord(2, 1, 1);
	VBO_NAME.SetColor(2, 1, 1, 1);

	VBO_NAME.SetPosition(3, width, 0, 0);
	VBO_NAME.SetTexcoord(3, 1, 0);
	VBO_NAME.SetColor(3, 1, 1, 1);

	SHADER_NAME.Init(vertShader, fargShader);
	m_Shader.SetTexture2D(picPath, picNameInShader);
	m_Options.DrawType = DRAW_TRIANGLES_STRIP;
}

void UITexture::Draw()
{
	INIT_TEST_VOID
	SceneManager::DrawUI(this);
}

void UITexture::SetSize(float x, float y, float width, float height)
{
	INIT_TEST_VOID
	SetPosition(x, y, -2);
	VBO_NAME.SetPosition(0, 0, 0, 0);
	VBO_NAME.SetPosition(1,width, 0, 0);
	VBO_NAME.SetPosition(2, 0, height,0);
	VBO_NAME.SetPosition(3, width, height, 0);
}