#pragma  once
#include "ggl.h"
struct UniformTexture
{
	GLint location=_INVALID_LOCATION_;
	GLuint texture=_INVALID_ID_;
};

struct UniformVec4
{
	GLint location = _INVALID_LOCATION_;
	float v[4];
	UniformVec4()
	{
		memset(v, 0,sizeof(v));
	}
};
struct UniformFloat
{
	GLint location = _INVALID_LOCATION_;
	float value = INVALID;
};
struct UniformMatrix
{
	GLint location = _INVALID_LOCATION_;
	glm::mat4 value;
};

class Shader
{	
public:
	bool Init(const char* vertShaderPath, const char* fragmentShaderPath);
	void Begin() const;
	void End() const;
	void Destory();
protected:

public:
	void Bind(const float *M, const float* V, const float *P,const float *IT=nullptr);


public:
	inline const GLuint GetProgram() const{ return m_Program; };

	inline const GLint& GetModelMatrixLocation() const{ return m_ModelMatrixLocation; };
	inline const GLint& GetViewMatrixLocation() const{ return m_ViewMatrixLocation; };
	inline const GLint& GetProjactionMatrixLocation() const{ return m_ProjactionMatrixLocation; };
	inline const GLint& GetITModelMatrixLocation() const{ return m_ITModelMatrixLocation; };

	inline const GLint& GetPositionLocation() const{ return m_PositionLocation; };
	inline const GLint& GetColorLocation() const{ return m_ColorLocation; };
	inline const GLint& GetTexcoordLocation() const{ return m_TexcoordLocation; };
	inline const GLint& GetNormalLocation() const{ return m_NormalLocation; };

	
public:
	void SetTexture2D(const char* imagePath,const char* nameInShader="U_Texture_1");
	void SetTexture2D(const GLuint& texture, const char* nameInShader = "U_Texture_1");
	void SetVec4(const char* nameInShader, float x, float y, float z, float w);
	inline void SetVec4(const char* nameInShader, const vec4& vec){ SetVec4(nameInShader, vec.x, vec.y, vec.z, vec.w); }
	void SetFloat(const char* nameInShader, float value);
	void SetMatrix(const char* nameInShader,const glm::mat4& matrix);
private:

	
private:
	GLuint m_Program;
	//����ѡ��
	GLint m_ModelMatrixLocation, m_ViewMatrixLocation, m_ProjactionMatrixLocation,m_ITModelMatrixLocation;
	GLint m_PositionLocation, m_ColorLocation, m_TexcoordLocation, m_NormalLocation;
	//����ѡ��
	std::map<string, UniformTexture> m_mUniformTextures;
	std::map<string, UniformVec4> m_mUniformVec4;
	std::map<string, UniformFloat> m_mUniformFloats;
	std::map<string, UniformMatrix> m_mUniformMatrixs;
};