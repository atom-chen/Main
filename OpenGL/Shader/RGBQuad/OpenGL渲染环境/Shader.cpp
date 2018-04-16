#include "Shader.h"
#include "Vertex.h"
#include <afxcom_.h>
#include "Resource.h"

bool Shader::Init(const char* vertShaderPath, const char* fragmentShaderPath)
{
	m_Program = ResourceManager::GetProgram(vertShaderPath, fragmentShaderPath);
	ASSERT_INT_BOOL(m_Program);

	//获取各Location
	m_ModelMatrixLocation = glGetUniformLocation(m_Program, "ModelMatrix");
	m_ViewMatrixLocation = glGetUniformLocation(m_Program, "ViewMatrix");
	m_ProjactionMatrixLocation = glGetUniformLocation(m_Program, "ProjectionMatrix");
	m_ITModelMatrixLocation = glGetUniformLocation(m_Program, "IT_ModelMatrix");

	m_PositionLocation = glGetAttribLocation(m_Program, "position");
	m_ColorLocation = glGetAttribLocation(m_Program, "color");
	m_NormalLocation = glGetAttribLocation(m_Program,"normal");
	m_TexcoordLocation = glGetAttribLocation(m_Program,"texcoord");

	return 1;
}
void Shader::Destory()
{
	ResourceManager::RemoveProgram(m_Program);
	for (auto it = m_mUniformTextures.begin(); it!=m_mUniformTextures.end(); it++)
	{
		ResourceManager::RemovePic(it->second.texture);
	}
}
void Shader::SetTexture2D(const GLuint& texture, const char* nameInShader)
{
	auto it = m_mUniformTextures.find(nameInShader);
	//如果it不存在于map中，则新建去shader中获取
	if (it == m_mUniformTextures.end())
	{
		UniformTexture uTexture;
		uTexture.location = glGetUniformLocation(m_Program, nameInShader);
		if (uTexture.location != _INVALID_LOCATION_)
		{
			uTexture.texture = texture;
			if (uTexture.texture != _INVALID_ID_)
			{
				m_mUniformTextures.insert(std::pair<string, UniformTexture>(nameInShader, uTexture));
			}
		}
	}
	//如果存在，则认为想要替换图片
	else
	{
		it->second.texture = texture;
	}
}
void Shader::SetTexture2D(const char* imagePath, const char* nameInShader)
{
	ASSERT(imagePath);
	auto it = m_mUniformTextures.find(nameInShader);
	//如果it不存在于map中，则新建去shader中获取
	if (it == m_mUniformTextures.end())
	{
		UniformTexture texture;
		texture.location = glGetUniformLocation(m_Program, nameInShader);
		if (texture.location != _INVALID_LOCATION_)
		{
			//去读图片
			texture.texture = ResourceManager::GetPic(imagePath);
			if (texture.texture != _INVALID_ID_)
			{
				m_mUniformTextures.insert(std::pair<string, UniformTexture>(nameInShader, texture));
			}
		}
	}
	//如果存在，则认为想要替换图片
	else
	{
		it->second.texture = ResourceManager::GetPic(imagePath);
	}
}
void Shader::SetVec4(const char* nameInShader, float x, float y, float z, float w)
{
	auto it = m_mUniformVec4.find(nameInShader);
	//如果it不存在于map中，则新建去shader中获取
	if (it == m_mUniformVec4.end())
	{
		UniformVec4 vec4;
		vec4.location = glGetUniformLocation(m_Program, nameInShader);
		if (vec4.location != _INVALID_LOCATION_)
		{
			vec4.v[0] = x;
			vec4.v[1] = y;
			vec4.v[2] = z;
			vec4.v[3] = w;
			m_mUniformVec4.insert(std::pair<string, UniformVec4>(nameInShader,vec4));
		}
	}
	else
	{
		it->second.v[0] = x;
		it->second.v[1] = y;
		it->second.v[2] = z;
		it->second.v[3] = w;
	}
}
void Shader::SetFloat(const char* nameInShader, float value)
{
	auto it = m_mUniformFloats.find(nameInShader);
	//如果it不存在于map中，则新建去shader中获取
	if (it == m_mUniformFloats.end())
	{
		UniformFloat vec4;
		vec4.location = glGetUniformLocation(m_Program, nameInShader);
		if (vec4.location != _INVALID_LOCATION_)
		{
			vec4.value = value;
			m_mUniformFloats.insert(std::pair<string, UniformFloat>(nameInShader, vec4));
		}
	}
	else
	{
		it->second.value = value;
	}
}
void Shader::SetMatrix(const char* nameInShader, const glm::mat4& matrix)
{
	auto it = m_mUniformMatrixs.find(nameInShader);
	//如果it不存在于map中，则新建去shader中获取
	if (it == m_mUniformMatrixs.end())
	{
		UniformMatrix mat;
		mat.location = glGetUniformLocation(m_Program, nameInShader);
		if (mat.location != _INVALID_LOCATION_)
		{
			mat.value = matrix;
			m_mUniformMatrixs.insert(std::pair<string, UniformMatrix>(nameInShader, mat));
		}
	}
	else
	{
		it->second.value = matrix;
	}
}

void Shader::Begin() const
{
	glUseProgram(this->m_Program);
}

void Shader::End() const
{
	glUseProgram(_INVALID_ID_);
}

void Shader::Bind(const float *M, const float* V, const float *P, const float *IT )
{
	////////////////////////////////////////Uniform Begin//////////////////////////////////
	if (m_ModelMatrixLocation != _INVALID_LOCATION_ && M != nullptr)
	{
		glUniformMatrix4fv(this->m_ModelMatrixLocation, 1, GL_FALSE, M);
	}
	if (m_ViewMatrixLocation != _INVALID_LOCATION_ && V != nullptr)
	{
		glUniformMatrix4fv(this->m_ViewMatrixLocation, 1, GL_FALSE, V);
	}
	if (m_ProjactionMatrixLocation != _INVALID_LOCATION_ && P != nullptr)
	{
		glUniformMatrix4fv(this->m_ProjactionMatrixLocation, 1, GL_FALSE, P);
	}
	if (m_ITModelMatrixLocation != _INVALID_LOCATION_ && IT != nullptr)
	{
		glUniformMatrix4fv(this->m_ITModelMatrixLocation, 1, GL_FALSE, IT);
	}
	//map不为空，则开始贴纹理
	if (m_mUniformTextures.size() > 0)
	{
		for (auto it = m_mUniformTextures.begin(); it != m_mUniformTextures.end(); it++)
		{
			glUniform1i(it->second.location, 0);
			glBindTexture(GL_TEXTURE_2D, it->second.texture);
		}
	}
	if (m_mUniformVec4.size() > 0)
	{
		for (auto it = m_mUniformVec4.begin(); it != m_mUniformVec4.end(); it++)
		{
			glUniform4fv(it->second.location, 1, it->second.v);
		}
	}
	if (m_mUniformFloats.size() > 0)
	{
		for (auto it = m_mUniformFloats.begin(); it != m_mUniformFloats.end(); it++)
		{
			glUniform1f(it->second.location,it->second.value);
		}
	}
	if (m_mUniformMatrixs.size() > 0)
	{
		for (auto it = m_mUniformMatrixs.begin(); it != m_mUniformMatrixs.end(); it++)
		{
			glUniformMatrix4fv(it->second.location, 1, GL_FALSE, glm::value_ptr(it->second.value));
		}
	}
	
	////////////////////////////////////////Uniform End//////////////////////////////////

	///////////////////////////////////Attribute Begin///////////////////////////////////////
	//告诉GPU,怎么样去读取vbo的内存块
	//启用该插槽
	glEnableVertexAttribArray(this->m_PositionLocation);
	glVertexAttribPointer(m_PositionLocation, 4, GL_FLOAT, GL_FALSE, sizeof(Vertex), 0);//插槽的位置，插槽中数据有几个分量(x,y,z,w)，每个分量是什么类型，是否归一化，两个点之间的距离，设置的信息从vbo的啥地方开始取值

	glEnableVertexAttribArray(this->m_ColorLocation);
	glVertexAttribPointer(m_ColorLocation, 4, GL_FLOAT, GL_FALSE, sizeof(Vertex), (void*)(sizeof(float) * 4));

	glEnableVertexAttribArray(this->m_TexcoordLocation);
	glVertexAttribPointer(this->m_TexcoordLocation, 4, GL_FLOAT, GL_FALSE, sizeof(Vertex), (void*)(sizeof(float) * 8));

	glEnableVertexAttribArray(this->m_NormalLocation);
	glVertexAttribPointer(this->m_NormalLocation, 4, GL_FLOAT, GL_FALSE, sizeof(Vertex), (void*)(sizeof(float) * 12));
	///////////////////////////////////Attribute End///////////////////////////////////////
}



