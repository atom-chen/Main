#include "Shader.h"
#include "Utils.h"
#include "Vertex.h"

bool Shader::Init(const char* vertShaderPath, const char* fragmentShaderPath)
{
	int32_t nFileSize = 0;
	const char* vertexShaderCode =(char*) LoadFileContent(vertShaderPath,nFileSize);
	ASSERT_PTR_BOOL(vertexShaderCode);
	GLuint vsShader = CompileShader(GL_VERTEX_SHADER, vertexShaderCode);
	delete vertexShaderCode;
	ASSERT_INT_BOOL(vsShader);

	const char* fragmentShaderCode = (char*)LoadFileContent(fragmentShaderPath, nFileSize);
	ASSERT_PTR_BOOL(fragmentShaderCode);
	GLuint fsShader = CompileShader(GL_FRAGMENT_SHADER, fragmentShaderCode);
	delete fragmentShaderCode;
	ASSERT_INT_BOOL(fsShader);

	m_Program = CreateProgram(vsShader, fsShader);
	ASSERT_INT_BOOL(m_Program);

	//��ȡ��Location
	m_ModelMatrixLocation = glGetUniformLocation(m_Program, "ModelMatrix");
	m_ViewMatrixLocation = glGetUniformLocation(m_Program, "ViewMatrix");
	m_ProjactionMatrixLocation = glGetUniformLocation(m_Program, "ProjectionMatrix");

	m_PositionLocation = glGetAttribLocation(m_Program, "position");
	m_ColorLocation = glGetAttribLocation(m_Program, "color");
	m_NormalLocation = glGetAttribLocation(m_Program, "normal");
	m_TexcoordLocation = glGetAttribLocation(m_Program, "texcoord");

	return 1;
}
void Shader::SetTexture2D(const char* imagePath, const char* nameInShader)
{
	auto it = m_mUniformTextures.find(nameInShader);
	//���it��������map�У����½�ȥshader�л�ȡ
	if (it == m_mUniformTextures.end())
	{
		UniformTexture texture;
		texture.location = glGetUniformLocation(m_Program, nameInShader);
		if (texture.location != _INVALID_LOCATION_)
		{
			//ȥ��ͼƬ
			texture.texture = CreateTexture2DFromPNG(imagePath);
			if (texture.texture != 0)
			{
				m_mUniformTextures.insert(std::pair<string, UniformTexture>(nameInShader, texture));
			}
		}
	}
	//������ڣ�����Ϊ��Ҫ�滻ͼƬ
	else
	{
		glDeleteTextures(1, &(it->second.texture));
		it->second.texture = CreateTexture2DFromPNG(imagePath);
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

void Shader::Bind(float *M, float* V, float *P)
{
	glUniformMatrix4fv(this->m_ModelMatrixLocation, 1, GL_FALSE, M);
	glUniformMatrix4fv(this->m_ViewMatrixLocation, 1, GL_FALSE,V);
	glUniformMatrix4fv(this->m_ProjactionMatrixLocation, 1, GL_FALSE, P);

	//map��Ϊ�գ���ʼ������
	if (m_mUniformTextures.size() > 0)
	{
		for (auto it = m_mUniformTextures.begin(); it != m_mUniformTextures.end(); it++)
		{
			glBindTexture(GL_TEXTURE_2D, it->second.texture);
			glUniform1i(it->second.location, 0);
		}
	}

	//����GPU,��ô��ȥ��ȡvbo���ڴ��
	//���øò��
	glEnableVertexAttribArray(this->m_PositionLocation);
	glVertexAttribPointer(m_PositionLocation, 4, GL_FLOAT, GL_FALSE, sizeof(Vertex), 0);//��۵�λ�ã�����������м�������(x,y,z,w)��ÿ��������ʲô���ͣ��Ƿ��һ����������֮��ľ��룬���õ���Ϣ��vbo��ɶ�ط���ʼȡֵ

	glEnableVertexAttribArray(this->m_ColorLocation);
	glVertexAttribPointer(m_ColorLocation, 4, GL_FLOAT, GL_FALSE, sizeof(Vertex), (void*)(sizeof(float) * 4));

	glEnableVertexAttribArray(this->m_TexcoordLocation);
	glVertexAttribPointer(this->m_TexcoordLocation, 4, GL_FLOAT, GL_FALSE, sizeof(Vertex), (void*)(sizeof(float) * 8));

	glEnableVertexAttribArray(this->m_NormalLocation);
	glVertexAttribPointer(this->m_NormalLocation, 4, GL_FLOAT, GL_FALSE, sizeof(Vertex), (void*)(sizeof(float) * 12));
}



