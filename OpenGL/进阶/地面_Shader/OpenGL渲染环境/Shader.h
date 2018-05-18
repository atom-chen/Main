#include "ggl.h"


class Shader
{
public:
	
public:
	bool Init(const char* vertShaderPath, const char* fragmentShaderPath);
	void UseThis() const;
protected:

public:
	inline void SetModelMatrix(const glm::mat4& modelMatrix);
	inline void SetViewMatrix(const glm::mat4& viewMatrix);
	inline void SetProjectionMatrix(const glm::mat4& projectionMatrix);
	void SetMatrix(const glm::mat4& modelMatrix, const glm::mat4& viewMatrix, const glm::mat4& projectionMatrix);

	void SetTexture_2D(const GLuint& texture);


public:
	inline const GLuint GetProgram() const{ return m_Program; };

	inline const GLint& GetModelMatrixLocation() const{ return m_ModelMatrixLocation; };
	inline const GLint& GetViewMatrixLocation() const{ return m_ViewMatrixLocation; };
	inline const GLint& GetProjactionMatrixLocation() const{ return m_ProjactionMatrixLocation; };

	inline const GLint& GetPositionLocation() const{ return m_PositionLocation; };
	inline const GLint& GetColorLocation() const{ return m_ColorLocation; };
	inline const GLint& GetTexcoordLocation() const{ return m_TexcoordLocation; };
	inline const GLint& GetNormalLocation() const{ return m_NormalLocation; };

	inline const GLint& GetTextureLocation() const{ return m_TextureLocation; };
private:
	GLuint m_Program;
	GLint m_ModelMatrixLocation, m_ViewMatrixLocation, m_ProjactionMatrixLocation;
	GLint m_PositionLocation, m_ColorLocation, m_TexcoordLocation, m_NormalLocation;
	GLint m_TextureLocation;
};