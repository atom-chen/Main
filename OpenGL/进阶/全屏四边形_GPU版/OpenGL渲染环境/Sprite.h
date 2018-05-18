#pragma  once
#include "ggl.h"
#include "Vertex.h"
#include "Shader.h"

class Sprite
{
public:
	void SetImage(const char* path);
	void SetImage(GLuint texture);
	virtual void Init(float x, float y, float width, float height, const char* vertShader="res/texture.vert");
	virtual void Draw(glm::mat4& viewMatrix, glm::mat4 &ProjectionMatrix);
	void SetPosition(float x, float y, float width, float height);
protected:
private:
	VertexBuffer VBO_NAME;
	Shader SHADER_NAME;
	mat4 MODELMATRIX_NAME;

	GLuint m_Texture;
};