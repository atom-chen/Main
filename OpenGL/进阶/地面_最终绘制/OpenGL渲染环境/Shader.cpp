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


	m_TextureLocation =glGetUniformLocation(m_Program, "U_Texture");


	return 1;
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

inline void Shader::SetModelMatrix(const glm::mat4& modelMatrix)
{
	glUniformMatrix4fv(this->m_ModelMatrixLocation, 1, GL_FALSE, glm::value_ptr(modelMatrix));//插槽位置，设置几个这样的矩阵（插槽上可能是矩阵数组）,是否转置，数据的起始地址
}

inline void Shader::SetViewMatrix(const glm::mat4& viewMatrix)
{
	glUniformMatrix4fv(this->m_ViewMatrixLocation, 1, GL_FALSE, glm::value_ptr(viewMatrix));
}

inline void Shader::SetProjectionMatrix(const glm::mat4& projectionMatrix)
{
	glUniformMatrix4fv(this->m_ProjactionMatrixLocation, 1, GL_FALSE, glm::value_ptr(projectionMatrix));
}

void Shader::SetMatrix(const glm::mat4& modelMatrix, const glm::mat4& viewMatrix, const glm::mat4& projectionMatrix)
{
	glUniformMatrix4fv(this->m_ModelMatrixLocation, 1, GL_FALSE, glm::value_ptr(modelMatrix));//插槽位置，设置几个这样的矩阵（插槽上可能是矩阵数组）,是否转置，数据的起始地址
	glUniformMatrix4fv(this->m_ViewMatrixLocation, 1, GL_FALSE, glm::value_ptr(viewMatrix));
	glUniformMatrix4fv(this->m_ProjactionMatrixLocation, 1, GL_FALSE, glm::value_ptr(projectionMatrix));
}

void Shader::SetTexture_2D(const GLuint& texture)
{
	glBindTexture(GL_TEXTURE_2D, texture);
	glUniform1i(this->m_TextureLocation, 0);
}



