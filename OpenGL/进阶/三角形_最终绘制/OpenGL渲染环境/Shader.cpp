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

	//获取各Location
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
	m_Texture.location = glGetUniformLocation(m_Program, nameInShader);
	if (m_Texture.location == _INVALID_LOCATION_)
	{
		return;
	}
	m_Texture.texture = CreateTexture2DFromBMP(imagePath);
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

	if (m_Texture.location != _INVALID_LOCATION_)
	{
		glBindTexture(GL_TEXTURE_2D, m_Texture.texture);
		glUniform1i(m_Texture.location, 0);
	}

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
}



