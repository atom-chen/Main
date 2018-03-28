#pragma  once
#include "ggl.h"
struct UniformTexture
{
	GLint location=_INVALID_LOCATION_;
	GLuint texture=_INVALID_ID_;
};

class Shader
{	
public:
	bool Init(const char* vertShaderPath, const char* fragmentShaderPath);
	void Begin() const;
	void End() const;
protected:

public:
	void Bind(float *M, float* V, float *P);


public:
	inline const GLuint GetProgram() const{ return m_Program; };

	inline const GLint& GetModelMatrixLocation() const{ return m_ModelMatrixLocation; };
	inline const GLint& GetViewMatrixLocation() const{ return m_ViewMatrixLocation; };
	inline const GLint& GetProjactionMatrixLocation() const{ return m_ProjactionMatrixLocation; };

	inline const GLint& GetPositionLocation() const{ return m_PositionLocation; };
	inline const GLint& GetColorLocation() const{ return m_ColorLocation; };
	inline const GLint& GetTexcoordLocation() const{ return m_TexcoordLocation; };
	inline const GLint& GetNormalLocation() const{ return m_NormalLocation; };

	inline const GLint& GetTextureLocation() const{ return m_Texture.location; };

	void SetTexture2D(const char* imagePath,const char* nameInShader="U_Texture");
private:
	GLuint m_Program;
	GLint m_ModelMatrixLocation, m_ViewMatrixLocation, m_ProjactionMatrixLocation;
	GLint m_PositionLocation, m_ColorLocation, m_TexcoordLocation, m_NormalLocation;
	UniformTexture m_Texture;
};