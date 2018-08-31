#include "Shader.h"
#include "Utils.h"
#include "Vertex.h"
#include <afxcom_.h>

bool Shader::Init(const char* vertShaderPath, const char* fragmentShaderPath)
{
	char temp[128];
	memset(temp, 0, 128);
	strcat(temp, SHADER_ROOT);
	strcat(temp,vertShaderPath);
	int32_t nFileSize = 0;
	const char* vertexShaderCode = (char*)LoadFileContent(temp, nFileSize);
	ASSERT_PTR_BOOL(vertexShaderCode);
	GLuint vsShader = CompileShader(GL_VERTEX_SHADER, vertexShaderCode);
	delete vertexShaderCode;
	ASSERT_INT_BOOL(vsShader);
	printf("%s读取成功\r\n", vertShaderPath);

	memset(temp, 0, 128);
	strcat(temp, SHADER_ROOT);
	strcat(temp, fragmentShaderPath);
	const char* fragmentShaderCode = (char*)LoadFileContent(temp, nFileSize);
	ASSERT_PTR_BOOL(fragmentShaderCode);
	GLuint fsShader = CompileShader(GL_FRAGMENT_SHADER, fragmentShaderCode);
	delete fragmentShaderCode;
	ASSERT_INT_BOOL(fsShader);
	printf("%s读取成功\r\n", fragmentShaderPath);

	m_Program = CreateProgram(vsShader, fsShader);
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
		glDeleteTextures(1, &(it->second.texture));
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
			texture.texture = CreateTexture2DFromPNG(imagePath);
			if (texture.texture != _INVALID_ID_)
			{
				m_mUniformTextures.insert(std::pair<string, UniformTexture>(nameInShader, texture));
			}
		}
	}
	//如果存在，则认为想要替换图片
	else
	{
		glDeleteTextures(1, &(it->second.texture));
		it->second.texture = CreateTexture2DFromPNG(imagePath);
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
	////////////////////////////////////////Uniform Begin//////////////////////////////////
	glUniformMatrix4fv(this->m_ModelMatrixLocation, 1, GL_FALSE, M);
	glUniformMatrix4fv(this->m_ViewMatrixLocation, 1, GL_FALSE,V);
	glUniformMatrix4fv(this->m_ProjactionMatrixLocation, 1, GL_FALSE, P);

	//map不为空，则开始贴纹理
	if (m_mUniformTextures.size() > 0)
	{
		for (auto it = m_mUniformTextures.begin(); it != m_mUniformTextures.end(); it++)
		{
			glBindTexture(GL_TEXTURE_2D, it->second.texture);
			glUniform1i(it->second.location, 0);
		}
	}
	if (m_mUniformVec4.size() > 0)
	{
		for (auto it = m_mUniformVec4.begin(); it != m_mUniformVec4.end(); it++)
		{
			glUniform4fv(it->second.location, 1, it->second.v);
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

GLuint Shader::CompileShader(GLenum shaderType, const char* shaderCode)
{
	//创建一个shader对象
	GLuint shader = glCreateShader(shaderType);

	//把shader源码放到GPU
	glShaderSource(shader, 1, &shaderCode, nullptr);//shader,有一句代码（因为就包含在shaderCode里面，实际上是包含了整个文件），代码在哪里，

	//编译shader
	glCompileShader(shader);

	GLint compileResule = GL_TRUE;
	//查看shader的编译状态
	glGetShaderiv(shader, GL_COMPILE_STATUS, &compileResule);
	if (compileResule == GL_FALSE)
	{
		char szLog[1024] = { 0 };
		GLsizei logLen = 0;
		//拿到错误日志
		glGetShaderInfoLog(shader, 1024, &logLen, szLog);   //shader，错误日志最多多少字符，做输出拿到实际多少个字符，日志写在哪

		printf("Comoile Shaderr fail error log:%s\nshader code:\n%s\n", szLog, shaderCode);
		glDeleteShader(shader);
		shader = 0;
	}
	return shader;
}

GLuint Shader::CreateProgram(GLuint vertexShader, GLuint fragmentShader)
{
	GLuint program = glCreateProgram();
	//将shader绑定到程序上
	glAttachShader(program, vertexShader);
	glAttachShader(program, fragmentShader);

	glLinkProgram(program);
	//解绑定
	glDetachShader(program, vertexShader);
	glDetachShader(program, fragmentShader);

	GLint linkResult;
	//检查程序链接是否OK
	glGetProgramiv(program, GL_LINK_STATUS, &linkResult);
	if (linkResult == GL_FALSE)
	{
		char szLog[1024] = { 0 };
		GLsizei logLen = 0;
		//拿到错误日志
		glGetProgramInfoLog(program, 1024, &logLen, szLog);   //shader，错误日志最多多少字符，做输出拿到实际多少个字符，日志写在哪

		printf("Comoile Shaderr fail error log:%s\nshader code:\n%s\n", szLog, program);
		glDeleteShader(program);
		program = 0;
	}
	return program;
}

