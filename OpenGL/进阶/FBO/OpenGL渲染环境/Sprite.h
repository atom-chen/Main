#pragma  once
#include "ggl.h"
#include "Vertex.h"
#include "Shader.h"

class Sprite
{
public:
	void SetImage(const char* path);
	virtual void Init(float x, float y, float widget, float height);
	virtual void Draw(glm::mat4& viewMatrix, glm::mat4 &ProjectionMatrix);
protected:
private:
	VertexBuffer VBO_NAME;
	Shader SHADER_NAME;
	mat4 MODELMATRIX_NAME;

	GLuint m_Texture;
};