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

inline void Shader::SetModelMatrix(const glm::mat4& modelMatrix)
{
	glUniformMatrix4fv(this->m_ModelMatrixLocation, 1, GL_FALSE, glm::value_ptr(modelMatrix));//���λ�ã����ü��������ľ��󣨲���Ͽ����Ǿ������飩,�Ƿ�ת�ã����ݵ���ʼ��ַ
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
	glUniformMatrix4fv(this->m_ModelMatrixLocation, 1, GL_FALSE, glm::value_ptr(modelMatrix));//���λ�ã����ü��������ľ��󣨲���Ͽ����Ǿ������飩,�Ƿ�ת�ã����ݵ���ʼ��ַ
	glUniformMatrix4fv(this->m_ViewMatrixLocation, 1, GL_FALSE, glm::value_ptr(viewMatrix));
	glUniformMatrix4fv(this->m_ProjactionMatrixLocation, 1, GL_FALSE, glm::value_ptr(projectionMatrix));
}

void Shader::SetTexture_2D(const GLuint& texture)
{
	glBindTexture(GL_TEXTURE_2D, texture);
	glUniform1i(this->m_TextureLocation, 0);
}



