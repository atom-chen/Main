#pragma  once
#include "ggl.h"
#include "Vertex.h"
#include "Utils.h"
class Ground
{
public:
	~Ground();
	void Init();
	void Draw();
	void SetAmbientMaterialColor(float r, float g, float b, float a = 1);
	void SetDiffuseMaterialColor(float r, float g, float b, float a = 1);
	void SetSpecularMaterialColor(float r, float g, float b, float a = 1);
protected:
private:
	//≤ƒ÷  Ù–‘
	float m_AmbientMaterial[4], m_DiffuseMaterial[4], m_SpecularMaterial[4];
	GLuint m_Vbo;
	VertexBuffer *m_VertexBuf;
};