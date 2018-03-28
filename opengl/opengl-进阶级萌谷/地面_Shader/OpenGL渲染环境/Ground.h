#pragma  once
#include "ggl.h"
#include "Vertex.h"
#include "Utils.h"
#include "Shader.h"
class Ground
{
public:
	~Ground();
	bool Init();
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
	Shader m_Shader;
	GLuint m_Program;
	GLint m_PositionLocation, m_ColorLocation, m_NormalLocation;
	GLint m_ModeMatrixLocation, m_ViewMatrixLocation, m_ProjectionMatrix;
};