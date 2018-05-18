#include "Model.h"
#include "Utils.h"
#include "Resource.h"

Model::Model() :m_Position(0, 0, 0), m_Scale(1,1,1)
{
	m_ModelMatrix = glm::translate(m_Position)*glm::scale(m_Scale);
	for (int i = 0; i < 4; i++)
	{
		if (i != 3)
		{
			m_AmbientMaterial[i] = 0.1f;
			m_DiffuseMaterial[i] = 0.6f;
			m_SpecularMaterial[i] = 1;
		}
		else
		{
			m_AmbientMaterial[i] = 1;
			m_DiffuseMaterial[i] = 1.0f;
			m_SpecularMaterial[i] = 1;
		}
	}
}
bool Model::Init(const char* path, const char* vertexShader, const char* fragmentShader)
{
	ResourceManager::GetModel(path,m_VertexBuf);
	
	if (vertexShader != nullptr)
	{
		if (fragmentShader != nullptr)
		{
			m_Shader.Init(vertexShader, fragmentShader);
		}
		else
		{
			m_Shader.Init(vertexShader, "res/obj.frag");
		}
	}
	else
	{
		m_Shader.Init("res/obj.vert", "res/obj.frag");
	}
	m_Shader.SetVec4("U_LightPos", 0, 1, 1, 0);
	m_Shader.SetVec4("U_LightAmbient", 1, 1, 1, 1);
	m_Shader.SetVec4("U_LightDiffuse", 1, 1, 1, 1);
	m_Shader.SetVec4("U_LightSpecular", 1, 1, 1, 1);
	this->SetAmbientMaterial(m_AmbientMaterial[0], m_AmbientMaterial[1], m_AmbientMaterial[2], m_AmbientMaterial[3]);
	this->SetDiffuseMaterial(m_DiffuseMaterial[0], m_DiffuseMaterial[1], m_DiffuseMaterial[2], m_DiffuseMaterial[3]);
	this->SetSpecularMaterial(m_SpecularMaterial[0], m_SpecularMaterial[1], m_SpecularMaterial[2], m_SpecularMaterial[3]);
	m_Shader.SetVec4("U_CameraPos", 0, 0,  0,1);
	m_Shader.SetVec4("U_LightOpt", 32, 0, 0, 1);
	return 1;
}
void Model::Update(const float& frameTime, const vec3& cameraPos)
{
	if (m_IsMoveToRight)
	{
		float delta = frameTime*m_MoveSpeed;
		SetPosition(m_Position.x + delta, m_Position.y, m_Position.z);
	}
	if (m_IsMoveToLeft)
	{
		float delta = frameTime*m_MoveSpeed;
		SetPosition(m_Position.x - delta, m_Position.y, m_Position.z);
	}
	if (m_IsMoveToTop)
	{
		float delta = frameTime*m_MoveSpeed;
		SetPosition(m_Position.x, m_Position.y, m_Position.z-delta);
	}
	if (m_IsMoveToBottom)
	{
		float delta = frameTime*m_MoveSpeed;
		SetPosition(m_Position.x, m_Position.y, m_Position.z + delta);
	}
	m_Shader.SetVec4("U_CameraPos", cameraPos.x, cameraPos.y, cameraPos.z, 1);
}

void Model::Draw(glm::mat4& viewMatrix, glm::mat4 &ProjectionMatrix)
{
	glm::mat4 ITMatrix = glm::inverseTranspose(this->m_ModelMatrix);
	glEnable(GL_DEPTH_TEST);
	BEGIN
	glUniformMatrix4fv(m_Shader.GetITModelMatrixLocation(), 1, GL_FALSE, glm::value_ptr(ITMatrix));
	DRAW
	END
}

void Model::SetPosition(float x, float y, float z)
{
	m_Position.x = x;
	m_Position.y = y;
	m_Position.z = z;
	m_ModelMatrix = glm::translate(x, y, z)*glm::scale(m_Scale.x, m_Scale.y, m_Scale.z);
}
void Model::SetScale(float x, float y, float z)
{
	m_Scale.x = x;
	m_Scale.y = y;
	m_Scale.z = z;
	m_ModelMatrix = glm::translate(m_Position.x, m_Position.y, m_Position.z)* glm::scale(x, y, z);
}


void Model::SetTexture2D(const char* path,const char* nameInShader)
{
	m_Shader.SetTexture2D(path,nameInShader);
}


void Model::SetAmbientMaterial(float r, float g, float b, float a)
{
	m_AmbientMaterial[0] = r;
	m_AmbientMaterial[1] = g;
	m_AmbientMaterial[2] = b;
	m_AmbientMaterial[3] = a;
	m_Shader.SetVec4("U_AmbientMaterial", r,g,b,a);
}

void Model::SetDiffuseMaterial(float r, float g, float b, float a)
{
	m_DiffuseMaterial[0] = r;
	m_DiffuseMaterial[1] = g;
	m_DiffuseMaterial[2] = b;
	m_DiffuseMaterial[3] = a;
	m_Shader.SetVec4("U_DiffuseMaterial",r,g,b,a);
}

void Model::SetSpecularMaterial(float r, float g, float b, float a)
{
	m_SpecularMaterial[0] = r;
	m_SpecularMaterial[1] = g;
	m_SpecularMaterial[2] = b;
	m_SpecularMaterial[3] = a;
	m_Shader.SetVec4("U_SpecularMaterial", r,g,b,a);
}

void Model::MoveToLeft(bool isMove)
{
	m_IsMoveToLeft = isMove;
}
void Model::MoveToRight(bool isMove)
{
	m_IsMoveToRight = isMove;
}
void Model::MoveToTop(bool isMove)
{
	m_IsMoveToTop = isMove;
}
void Model::MoveToBottom(bool isMove)
{
	m_IsMoveToBottom = isMove;
}
void Model::SetMoveSpeed(float speed)
{
	this->m_MoveSpeed = speed;
}