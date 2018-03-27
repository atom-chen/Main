#include "Shader.h"
#include "Utils.h"

bool Shader::Init(const char* vertShaderPath, const char* fragmentShaderPath)
{
	int32_t nFileSize = 0;
	const char* vertexShaderCode =(char*) LoadFileContent(vertShaderPath,nFileSize);
	ASSERT_PTR_BOOL(vertexShaderCode);
	const char* fragmentShaderCode = (char*)LoadFileContent(fragmentShaderPath, nFileSize);
	ASSERT_PTR_BOOL(fragmentShaderCode);

	GLuint vsShader = CompileShader(GL_VERTEX_SHADER, vertexShaderCode);
	delete vertexShaderCode;
	ASSERT_INT_BOOL(vsShader);
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
	m_NormalLocation = glGetAttribLocation(m_Program, "normal");
	m_TexcoordLocation = glGetAttribLocation(m_Program, "texcoord");
	m_ColorLocation = glGetAttribLocation(m_Program, "color");

	m_TextureLocation =glGetUniformLocation(m_Program, "U_Texture");
	return 1;
}

void Shader::UseThis() const
{
	glUseProgram(this->m_Program);
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



