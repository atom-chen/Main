#include "GameObject.h"
#include "Utils.h"

GameObject::GameObject()
{

}
void GameObject::Init()
{
	m_VB.Init(3);

	m_VB.SetPosition(0, -0.2f, -0.2f, -0.6f);
	m_VB.SetColor(0, 0.2f, 0.4f, 0.8f);
	m_VB.SetTexcoord(0, 0, 0);

	m_VB.SetPosition(1, 0.2f, -0.2f, -0.6f);
	m_VB.SetColor(1, 0.2f, 0.4f, 0.8f);
	m_VB.SetTexcoord(1, 1, 0);

	m_VB.SetPosition(2, 0, 0.2f, -0.6f);
	m_VB.SetColor(2, 0.2f, 0.4f, 0.8f);
	m_VB.SetTexcoord(2, 0.5f, 1);


	m_Shader.Init("res/test.vert", "res/text.frag");
	m_Shader.SetTexture2D("res/test.bmp","U_Texture_1");
	m_Shader.SetTexture2D("res/head.png","U_Texture_2");
}


void GameObject::Draw(glm::mat4& viewMatrix, glm::mat4 &ProjectionMatrix)
{
	glEnable(GL_DEPTH_TEST);
	m_VB.Begin();
	{
		m_Shader.Begin();
		{
			m_Shader.Bind(glm::value_ptr(m_ModelMatrix), glm::value_ptr(viewMatrix), glm::value_ptr(ProjectionMatrix));
			glDrawArrays(GL_TRIANGLES, 0, 3);
		}
		m_Shader.End();
	}
	m_VB.End();
}