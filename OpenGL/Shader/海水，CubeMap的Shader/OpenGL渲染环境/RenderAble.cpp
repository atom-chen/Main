#include "RenderAble.h"
#include "Resource.h"
#include "SceneManager.h"

RenderAble::RenderAble()
{
	m_ModelMatrix = glm::translate(this->m_Transform.m_Position.x, this->m_Transform.m_Position.y, this->m_Transform.m_Position.z)*glm::scale(m_Transform.m_Scale.x, m_Transform.m_Scale.y, m_Transform.m_Scale.z);
}
RenderAble::~RenderAble()
{

}
void RenderAble::Draw()
{
	if (m_IsInit)
	{
		SceneManager::DrawGameObject(this);
	}
}
void RenderAble::Destory()
{
	INIT_TEST_VOID;
	m_IsInit = 0;
	ResourceManager::RemoveProgram(m_Shader.GetProgram());//Çå³ýShader
	ResourceManager::RemoveBufferObject(m_VertexBuf.GetBuffer());
	
}


void RenderAble::SetPosition(float x, float y, float z)
{
	Node::SetPosition(x, y, z);
	UpdateTransform();
}
void RenderAble::SetPosition(const vec3& pos)
{
	Node::SetPosition(pos.x, pos.y, pos.z);
	UpdateTransform();
}
void RenderAble::SetRotate(float x, float y, float z)
{
	Node::SetRotate(x, y, z);
	UpdateTransform();
}
void RenderAble::SetRotate(const vec3& rot)
{
	Node::SetRotate(rot.x, rot.y, rot.z);
	UpdateTransform();
}

void RenderAble::SetScale(float x, float y, float z)
{
	Node::SetScale(x, y, z);
	UpdateTransform();
}
void RenderAble::SetScale(const vec3& scale)
{
	Node::SetScale(scale.x, scale.y, scale.z);
	UpdateTransform();
}
void RenderAble::UpdateTransform()
{
	m_ModelMatrix = glm::translate(m_Transform.m_Position);
	m_ModelMatrix = glm::rotate(m_ModelMatrix, m_Transform.m_Rotate.x, vec3(1, 0, 0));
	m_ModelMatrix = glm::rotate(m_ModelMatrix, m_Transform.m_Rotate.y, vec3(0, 1, 0));
	m_ModelMatrix = glm::rotate(m_ModelMatrix, m_Transform.m_Rotate.z, vec3(0, 0, 1));
	m_ModelMatrix *= glm::scale(m_Transform.m_Scale);
}

void RenderAble::InitShader(const char* vertexShader, const char* fragmentShader)
{
	if (m_IsInit)
	{
		m_Shader.Init(vertexShader, fragmentShader);
	}

}
void RenderAble::SetVec4(const char* nameInShader, float x, float y, float z, float w)
{
	if (m_IsInit)
	{
		m_Shader.SetVec4(nameInShader, x, y, z, w);
	}
}
void RenderAble::SetFloat(const char* nameInShader, float value)
{
	if (m_IsInit)
	{
		m_Shader.SetFloat(nameInShader, value);
	}
}
//void RenderAble::SetTexture2D(const char* path, const char* nameInShader)
//{
//	INIT_TEST_VOID
//	m_Shader.SetTexture2D(path, nameInShader);
//}
void RenderAble :: SetTexture2D(GLuint texture, const char* nameInShader)
{ 
	INIT_TEST_VOID 
	m_Shader.SetTexture2D(texture, nameInShader); 
}
void RenderAble :: SetVec4(const char* nameInShader, const vec4& vec)
{ 
	INIT_TEST_VOID 
	SetVec4(nameInShader, vec.x, vec.y, vec.z, vec.w); 
}